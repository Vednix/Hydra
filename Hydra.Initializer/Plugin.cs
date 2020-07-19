using System;
using Terraria;
using TerrariaApi.Server;

namespace Hydra.Initializer
{
    [ApiVersion(2, 1)]
    public class Plugin : TerrariaPlugin
    {
        public override Version Version => Base.Version;

        public override string Name
        {
            get { return "Hydra Initializer"; }
        }

        public override string Author
        {
            get { return "Vednix"; }
        }

        public Plugin(Main game) : base(game)
        {
            Order = 0;
        }
        internal static bool Wait = false;
        public override void Initialize()
        {
            ServerApi.Hooks.NetGetData.Register(this, NetHooks.GetData);
            ServerApi.Hooks.ServerJoin.Register(this, SetLanguage.OnJoin);
            ServerApi.Hooks.GameInitialize.Register(this, Base.OnHydraInitialize);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.NetGetData.Deregister(this, NetHooks.GetData);
                ServerApi.Hooks.ServerJoin.Deregister(this, SetLanguage.OnJoin);
                ServerApi.Hooks.GameInitialize.Deregister(this, Base.OnHydraInitialize);
                Base.isDisposed = true;
            }
            base.Dispose(disposing);
        }
    }
}

