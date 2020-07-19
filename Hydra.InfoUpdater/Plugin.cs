using Hydra.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace Hydra.InfoUpdater
{
    [ApiVersion(2, 1)]
    public class Plugin : TerrariaPlugin
    {
        public override Version Version => new Version(1, 0, 0, 0);

        public override string Name
        {
            get { return "Hydra Updated"; }
        }

        public override string Author
        {
            get { return "Vednix"; }
        }

        public override string Description
        {
            get { return "Update Hydra"; }
        }

        public Plugin(Main game) : base(game)
        {
            Order = 0;
        }
        internal static bool Wait = false;
        public override void Initialize()
        {
            //ServerApi.Hooks.GamePostInitialize.Register(this, PostInitialize);
            //ServerApi.Hooks.ServerJoin.Register(this, OnJoin);
            ServerApi.Hooks.NetGetData.Register(this, NetHooks_GetData);
            //TerrariaApi.Server.SendDataEventArgs
            //ServerApi.Hooks.NetGetData.Register(this, HandleItemOwner);
        }

        private void NetHooks_GetData(GetDataEventArgs e)
        {
            if (e == null || Hydra.Base.isDisposed || e.Handled)
                return;

            TSPlayer player = TShockB.Players[e.Msg.whoAmI];
            if (player == null)
                return;

            switch (e.MsgID)
            {
                case PacketTypes.NpcItemStrike:
                    Console.WriteLine($"NPCITEMSTRIKE");
                    break;
                case PacketTypes.ItemOwner:
                    {
                        int itemIndex = BitConverter.ToInt16(e.Msg.readBuffer, e.Index);
                        int newOwnerPlayerIndex = e.Msg.readBuffer[e.Index + 2];
                        if (itemIndex == 400)
                        {
                            if (newOwnerPlayerIndex == 16)
                                TSPlayerB.isMobile[e.Msg.whoAmI] = true;
                            else
                                TSPlayerB.isMobile[e.Msg.whoAmI] = false;
                        }
                        break;
                    }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.NetGetData.Deregister(this, NetHooks_GetData);
                Hydra.Base.isDisposed = true;
            }
            base.Dispose(disposing);
        }
    }
}

