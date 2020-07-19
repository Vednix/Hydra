using Hydra.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace Hydra.Initializer
{
    [ApiVersion(2, 1)]
    public class Plugin : TerrariaPlugin
    {
        public override Version Version => new Version(1, 0, 1, 0);

        public override string Name
        {
            get { return "Hydra Initializer"; }
        }

        public override string Author
        {
            get { return "Vednix"; }
        }

        //public override string Description
        //{
        //    get { return ""; }
        //}

        public Plugin(Main game) : base(game)
        {
            Order = 0;
        }
        internal static bool Wait = false;
        public override void Initialize()
        {
            ServerApi.Hooks.NetGetData.Register(this, NetHooks.GetData);
            ServerApi.Hooks.ServerJoin.Register(this, SetLanguage.OnJoin);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.NetGetData.Deregister(this, NetHooks.GetData);
                ServerApi.Hooks.ServerJoin.Deregister(this, SetLanguage.OnJoin);
                Base.isDisposed = true;
            }
            base.Dispose(disposing);
        }
    }
}

