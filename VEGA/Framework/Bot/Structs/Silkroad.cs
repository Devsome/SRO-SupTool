using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VEGA.Framework.Bot.Structs
{
    public class Silkroad
    {
        public struct Server
        {
            public ushort id;
            public string name;
            public ushort curUser;
            public ushort maxUser;
            public bool isOnline;
        }
        public struct ListedCharacter
        {
            public byte characterSlotId;
            public string name;
        }
    }
}
