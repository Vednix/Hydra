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
            ServerApi.Hooks.NetGetData.Register(this, NetHooks.OnGetData);
            ServerApi.Hooks.ServerJoin.Register(this, SetLanguage.OnJoin);
            ServerApi.Hooks.NetGreetPlayer.Register(this, NetHooks.OnGreetPlayer);
            ServerApi.Hooks.GameInitialize.Register(this, Base.OnHydraInitialize);
            ServerApi.Hooks.GamePostInitialize.Register(this, Base.OnHydraPostInitialize);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.NetGetData.Deregister(this, NetHooks.OnGetData);
                ServerApi.Hooks.ServerJoin.Deregister(this, SetLanguage.OnJoin);
                ServerApi.Hooks.NetGreetPlayer.Deregister(this, NetHooks.OnGreetPlayer);
                ServerApi.Hooks.GameInitialize.Deregister(this, Base.OnHydraInitialize);
                ServerApi.Hooks.GamePostInitialize.Deregister(this, Base.OnHydraPostInitialize);
                Base.isDisposed = true;
            }
            base.Dispose(disposing);
        }
    }
}

