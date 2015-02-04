using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace VEGA.Framework.Bot.Packet
{
    public class Handling
    {
        //Client -> Server
        public static void handlePacket(ushort opcode, byte[] buffer)
        {
            try
            {

                switch (opcode)
                {
                    #region "Client -> Server"
                    //7001
                    case (ushort)Framework.Bot.Enums.Opcodes.EnterWorldRequest:
                        Framework.Bot.Packet.Parsing.EnterWorldRequest(buffer);
                        break;
                    #endregion
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        //Server -> Client
        public static void handlePacket(byte[] buffer)
        {
            using (BinaryReader br = new BinaryReader(new MemoryStream(buffer)))
            {
                try
                {
                    ushort dataLen = br.ReadUInt16();
                    ushort opcode = br.ReadUInt16();
                    ushort securityBytes = br.ReadUInt16();
                    byte[] data = br.ReadBytes(dataLen);

                    switch (opcode)
                    {
                        #region "Server -> Client"
                        //A101
                        case (ushort)Framework.Bot.Enums.Opcodes.ServerListReply:
                            Framework.Bot.Packet.Parsing.ServerListReply(data);
                            break;
                        case (ushort)Framework.Bot.Enums.Opcodes.GetPm:
                            Framework.Bot.Packet.Parsing.GetPm(data);
                            break;
                        case(ushort)Framework.Bot.Enums.Opcodes.SendPm:
                            VEGA.Framework.Global.Global.chatCounter++;
                            Framework.Bot.Packet.Parsing.SendPm(data);
                            break;
                        #endregion
                    }
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }
        }

    }
}
