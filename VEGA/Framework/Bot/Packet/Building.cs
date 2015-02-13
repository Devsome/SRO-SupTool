using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VEGA.Framework.Bot.Packet
{
    class Building
    {


        public static void BuildChatToClient(string chatMsg, byte chatType)
        {

        }

        public static void BuildPrivateMessage(string Text,string Getter)
        {
            if (Getter == "[BOT]System")
                Text = "global " + Text;

            Framework.Proxy.Packet p = new Framework.Proxy.Packet(0x7025);
            p.WriteUInt8(0x02); // for private
            p.WriteUInt8(Framework.Global.Global.chatCounter);
            p.WriteAscii(Getter); 
            p.WriteAscii(Text);
            Framework.Proxy.Main.ag_remote_security.Send(p);
        }
        

    }
}
