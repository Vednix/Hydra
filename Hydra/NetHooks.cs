using Hydra.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrariaApi.Server;
using TShockAPI;

namespace Hydra
{
    public class NetHooks
    {
        public static void GetData(GetDataEventArgs e)
        {
            if (e == null || Hydra.Base.isDisposed || e.Handled)
                return;

            TSPlayer player = TShockB.Players[e.Msg.whoAmI];
            if (player == null)
                return;

            switch (e.MsgID)
            {
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
    }
}
