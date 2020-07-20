using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;
using TShockAPI.ServerSideCharacters;

namespace Hydra.Extensions
{
    public class FileToolsB
    {
		private const string MotdFormat =
			   "This is [c/FF0000:%map%] on [c/00FFFF:TShock for Terraria].\n[c/00FF00:Current players:] [c/FFFF00:%players%]\nType [c/FF0000:/help] for a list of commands.\n";
		/// <summary>
		/// Path to the file containing the rules.
		/// </summary>
		internal static string RulesPath
		{
			get { return Path.Combine(TShock.SavePath, "rules.txt"); }
		}

		/// <summary>
		/// Path to the file containing the message of the day.
		/// </summary>
		internal static string MotdPath
		{
			get { return Path.Combine(TShock.SavePath, "motd.txt"); }
		}

		/// <summary>
		/// Path to the file containing the whitelist.
		/// </summary>
		internal static string WhitelistPath
		{
			get { return Path.Combine(TShock.SavePath, "whitelist.txt"); }
		}

		/// <summary>
		/// Path to the file containing the config.
		/// </summary>
		internal static string ConfigPath
		{
			get { return Path.Combine(TShock.SavePath, "config.json"); }
		}

		/// <summary>
		/// Path to the file containing the config.
		/// </summary>
		internal static string ServerSideCharacterConfigPath
		{
			get { return Path.Combine(TShock.SavePath, "sscconfig.json"); }
		}

		/// <summary>
		/// Creates an empty file at the given path.
		/// </summary>
		/// <param name="file">The path to the file.</param>
		public static void CreateFile(string file)
		{
			File.Create(file).Close();
		}

		/// <summary>
		/// Creates a file if the files doesn't already exist.
		/// </summary>
		/// <param name="file">The path to the files</param>
		/// <param name="data">The data to write to the file.</param>
		public static void CreateIfNot(string file, string data = "")
		{
			if (!File.Exists(file))
			{
				File.WriteAllText(file, data);
			}
		}

		/// <summary>
		/// Sets up the configuration file for all variables, and creates any missing files.
		/// </summary>
		public static void SetupConfig()
		{
			if (!Directory.Exists(TShock.SavePath))
			{
				Directory.CreateDirectory(TShock.SavePath);
			}

			CreateIfNot(RulesPath, "Respect the admins!\nDon't use TNT!");
			CreateIfNot(MotdPath, MotdFormat);

			CreateIfNot(WhitelistPath);
			if (File.Exists(ConfigPath))
			{
				TShock.Config = ConfigFile.Read(ConfigPath);
				// Add all the missing config properties in the json file
			}
			TShock.Config.Write(ConfigPath);

			if (File.Exists(ServerSideCharacterConfigPath))
			{
				TShock.ServerSideCharacterConfig = ServerSideConfig.Read(ServerSideCharacterConfigPath);
				// Add all the missing config properties in the json file
			}
			else
			{
				TShock.ServerSideCharacterConfig = new ServerSideConfig
				{
					StartingInventory =
						new List<NetItem>
						{
							new NetItem(-15, 1, 0),
							new NetItem(-13, 1, 0),
							new NetItem(-16, 1, 0)
						}
				};
			}
			TShock.ServerSideCharacterConfig.Write(ServerSideCharacterConfigPath);
		}

		/// <summary>
		/// Tells if a user is on the whitelist
		/// </summary>
		/// <param name="ip">string ip of the user</param>
		/// <returns>true/false</returns>
		public static bool OnWhitelist(string ip)
		{
			if (!TShock.Config.EnableWhitelist)
			{
				return true;
			}
			CreateIfNot(WhitelistPath, "127.0.0.1");
			using (var tr = new StreamReader(WhitelistPath))
			{
				string whitelist = tr.ReadToEnd();
				ip = TShock.Utils.GetRealIP(ip);
				bool contains = whitelist.Contains(ip);
				if (!contains)
				{
					foreach (var line in whitelist.Split(Environment.NewLine.ToCharArray()))
					{
						if (string.IsNullOrWhiteSpace(line))
							continue;
						contains = UtilsB.GetIPv4AddressFromHostname(line).Equals(ip);
						if (contains)
							return true;
					}
					return false;
				}
				return true;
			}
		}
	}
}
