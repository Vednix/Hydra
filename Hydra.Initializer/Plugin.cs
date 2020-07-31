using System;
using System.Reflection;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.Hooks;

namespace Hydra.Initializer
{
    [ApiVersion(2, 1)]
    public class Plugin : TerrariaPlugin
    {
        public override Version Version => Base.Version;
        public override string Name => Base.Name;
        public override string Author => Base.Author;
        public Plugin(Main game) : base(game)
        {
            Order = 0;
            Base.game = game; //maybe useful
        }
        internal static bool Wait = false;
        public override void Initialize()
        {
            if (Base.isHydraEnabled())
            {
                ServerApi.Hooks.NetGetData.Register(this, NetHooks.OnGetData);
                ServerApi.Hooks.ServerJoin.Register(this, SetLanguage.OnJoin);
                ServerApi.Hooks.ServerLeave.Register(this, NetHooks.OnLeave);
                ServerApi.Hooks.NetGreetPlayer.Register(this, NetHooks.OnGreetPlayer);
                ServerApi.Hooks.GameInitialize.Register(this, Base.OnHydraInitialize, 10);
                ServerApi.Hooks.GamePostInitialize.Register(this, Base.OnHydraPostInitialize);
                ServerApi.Hooks.ServerChat.Register(this, NetHooks.OnChat);
                GeneralHooks.ReloadEvent += Hydra.Config.OnReloadEvent;
            }
            else Base.isDisposed = true;
        }
        protected override void Dispose(bool disposing)
        {
            if (!Base.isDisposed) return;
            if (disposing)
            {
                ServerApi.Hooks.NetGetData.Deregister(this, NetHooks.OnGetData);
                ServerApi.Hooks.ServerJoin.Deregister(this, SetLanguage.OnJoin);
                ServerApi.Hooks.ServerLeave.Deregister(this, NetHooks.OnLeave);
                ServerApi.Hooks.NetGreetPlayer.Deregister(this, NetHooks.OnGreetPlayer);
                ServerApi.Hooks.GameInitialize.Deregister(this, Base.OnHydraInitialize);
                ServerApi.Hooks.GamePostInitialize.Deregister(this, Base.OnHydraPostInitialize);
                ServerApi.Hooks.ServerChat.Deregister(this, NetHooks.OnChat);
                GeneralHooks.ReloadEvent -= Hydra.Config.OnReloadEvent;
                Base.isDisposed = true;
            }
            base.Dispose(disposing);
        }
    }
}

