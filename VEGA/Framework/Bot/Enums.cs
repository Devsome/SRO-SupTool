using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VEGA.Framework.Bot
{
    public class Enums
    {
        [Flags]
        public enum Opcodes : ushort
        {
            //CLIENT
            KeepAlive = 0x2002,
            ServerListRequest = 0x6101,
            EnterWorldRequest = 0x7001,
            CharacterListingRequest = 0x7007,
            WalkRequest = 0x7021,

            //SERVER
            ServerListReply = 0xA101,
            GetPm = 0x3026,
            SendPm = 0x7025,

        }
    }
}
