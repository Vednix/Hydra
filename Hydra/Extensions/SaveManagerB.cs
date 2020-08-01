using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Terraria;
using Terraria.IO;
using TerrariaApi.Server;
using TShockAPI;

namespace Hydra.Extensions
{
	class SaveManagerB : IDisposable
	{
		// Singleton
		private static readonly SaveManagerB instance = new SaveManagerB();
		private SaveManagerB()
		{
			_saveThread = new Thread(SaveWorker);
			_saveThread.Name = "Hydra_TShock SaveManager Worker";
			_saveThread.Start();
		}
		public static SaveManagerB Instance { get { return instance; } }

		// Producer Consumer
		private EventWaitHandle _wh = new AutoResetEvent(false);
		private Object _saveLock = new Object();
		private Queue<SaveTask> _saveQueue = new Queue<SaveTask>();
		private Thread _saveThread;
		private int saveQueueCount { get { lock (_saveLock) return _saveQueue.Count; } }

		/// <summary>
		/// SaveWorld event handler which notifies users that the server may lag
		/// </summary>
		public void OnSaveWorld(WorldSaveEventArgs args)
		{
			if (TShock.Config.AnnounceSave)
			{
				// Protect against internal errors causing save failures
				// These can be caused by an unexpected error such as a bad or out of date plugin
				try
				{
					Logger.doLogLang(DefaultMessage: "Saving the World. Momentary lag may be noticed for a few seconds.", Config.DebugLevel.All, Base.CurrentHydraLanguage, Base.Name,
								   	 PortugueseMessage: "Salvando Mundo. Lag momentâneo poderá ser notado durante alguns segundos.",
								 	 SpanishMessage: "Salvando al mundo. El retraso momentáneo puede notarse durante unos segundos.");
					TShockB.AllSendMessage(DefaultMessage: "Saving the World. Momentary lag may be noticed for a few seconds.", Color.DarkSeaGreen,
										   PortugueseMessage: "Salvando Mundo. Lag momentâneo poderá ser notado durante alguns segundos.",
										   SpanishMessage: "Salvando al mundo. El retraso momentáneo puede notarse durante unos segundos.");
				}
				catch (Exception ex)
				{
					Logger.doLog($"World saved notification failed.\n{ex.Message}", Config.DebugLevel.Error, Base.Name);
				}
			}
		}

		/// <summary>
		/// Saves the map data
		/// </summary>
		/// <param name="wait">wait for all pending saves to finish (default: true)</param>
		/// <param name="resetTime">reset the last save time counter (default: false)</param>
		/// <param name="direct">use the realsaveWorld method instead of saveWorld event (default: false)</param>
		public void SaveWorld(bool wait = true, bool resetTime = false, bool direct = false)
		{
			EnqueueTask(new SaveTask(resetTime, direct));
			if (!wait)
				return;

			// Wait for all outstanding saves to complete
			int count = saveQueueCount;
			while (0 != count)
			{
				Thread.Sleep(50);
				count = saveQueueCount;
			}
		}

		/// <summary>
		/// Processes any outstanding saves, shutsdown the save thread and returns
		/// </summary>
		public void Dispose()
		{
			EnqueueTask(null);
			_saveThread.Join();
			_wh.Close();
		}

		private void EnqueueTask(SaveTask task)
		{
			lock (_saveLock)
			{
				_saveQueue.Enqueue(task);
			}
			_wh.Set();
		}

		private void SaveWorker()
		{
			while (true)
			{
				lock (_saveLock)
				{
					// NOTE: lock for the entire process so wait works in SaveWorld
					if (_saveQueue.Count > 0)
					{
						SaveTask task = _saveQueue.Dequeue();
						if (null == task)
							return;
						else
						{
							// Ensure that save handler errors don't bubble up and cause a recursive call
							// These can be caused by an unexpected error such as a bad or out of date plugin
							try
							{
								if (task.direct)
								{
									OnSaveWorld(new WorldSaveEventArgs());
									WorldFile.saveWorld(task.resetTime);
								}
								else
									WorldFile.saveWorld(task.resetTime);

								Logger.doLogLang(DefaultMessage: "World Saved.", Config.DebugLevel.Info, Base.CurrentHydraLanguage, Base.Name,
													PortugueseMessage: "Mundo Salvo.",
												  SpanishMessage: "Mundo Salvado.");
								TShockB.AllSendMessage(DefaultMessage: "World Saved.", Color.SeaGreen,
													   PortugueseMessage: "Mundo Salvo.",
													   SpanishMessage: "Mundo Salvado.");
							
								TShock.Log.Info(string.Format("World saved at ({0})", Main.worldPathName));
							}
							catch (Exception ex)
							{
								Logger.doLog($"World save failed.\n{ex}", Config.DebugLevel.Critical, Base.Name);
							}
						}
					}
				}
				_wh.WaitOne();
			}
		}

		class SaveTask
		{
			public bool resetTime { get; set; }
			public bool direct { get; set; }
			public SaveTask(bool resetTime, bool direct)
			{
				this.resetTime = resetTime;
				this.direct = direct;
			}

			public override string ToString()
			{
				return string.Format("resetTime {0}, direct {1}", resetTime, direct);
			}
		}
	}
}
