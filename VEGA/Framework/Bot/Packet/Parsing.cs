using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace VEGA.Framework.Bot.Packet
{
    public class Parsing
    {
        public delegate void LogMsg(string message);
        public delegate void AddForm(string charName);
        public delegate void AddControls(string charName);
        public delegate void ShowForm(string charName);
        public delegate void AddMsgToForm(string charName, string msg);
        public delegate void UpdateInventory();
        public static event LogMsg OnLogMsg;
        public static event AddForm OnAddForm;
        public static event AddControls OnAddControls;
        public static event AddMsgToForm OnAddMsgToForm;



        #region "Client -> Server"
        public static void EnterWorldRequest(byte[] data)
        {
            using (BinaryReader br = new BinaryReader(new MemoryStream(data)))
            {
                string charName = new string(br.ReadChars(br.ReadUInt16()));
                OnLogMsg(string.Format("* Entering world with [{0}]", charName));
            }
        }
        #endregion
        #region "Server -> Client"
        public static void ServerListReply(byte[] data)
        {
            Framework.Global.BotGlobal.server = new List<Framework.Bot.Structs.Silkroad.Server>();

            using (BinaryReader br = new BinaryReader(new MemoryStream(data)))
            {
                bool newGateway = br.ReadBoolean();

                while (newGateway)
                {
                    br.ReadByte();
                    br.ReadBytes(br.ReadUInt16());              //Skip: GateWayName

                    newGateway = br.ReadBoolean();
                }

                bool newServer = br.ReadBoolean();

                while (newServer)
                {
                    Framework.Bot.Structs.Silkroad.Server server = new Framework.Bot.Structs.Silkroad.Server();
                    server.id = br.ReadUInt16();
                    server.name = new string(br.ReadChars(br.ReadUInt16()));
                    server.curUser = br.ReadUInt16();
                    server.maxUser = br.ReadUInt16();
                    server.isOnline = br.ReadBoolean();

                    Framework.Global.BotGlobal.server.Add(server);

                    br.ReadByte();

                    newServer = br.ReadBoolean();
                }
            }

            foreach (Framework.Bot.Structs.Silkroad.Server server in Framework.Global.BotGlobal.server)
            {
                string state;

                if (server.isOnline)
                    state = "Online";
                else
                    state = "Check";

                OnLogMsg("* Server: " + string.Format("[ID] {0} [Name] {1} [Online] {2}/{3} [State] {4}", server.id, server.name, server.curUser, server.maxUser, state));
            }
        }

        public static void GetPm(byte[] data)
        {
            using (BinaryReader br = new BinaryReader(new MemoryStream(data)))
            {
                byte result = br.ReadByte();
                string charName;
                switch (result)
                {
                    case 1: // all
                        double PlayerID = br.ReadDouble();
                        charName = "Dunno";
                        break;
                    case 2: // privat
                        charName = new string(br.ReadChars(br.ReadUInt16()));
                        string getMessage = new string(br.ReadChars(br.ReadUInt16()));
                        //OnLogMsg("You got a PM from [" + charName + "]" + "[" + getMessage + "]");
                        
                        if (!Framework.Global.BotGlobal.activeConversations.ContainsKey(charName))
                        {
                            OnAddForm(charName);
                            OnAddMsgToForm(charName, getMessage);
                            OnAddControls(charName);
                        }
                        else
                        {
                            OnAddMsgToForm(charName, getMessage);
                        }
                        break;

                }
            }
        }

        public static void SendPm(byte[] data)
        {
            // buffer 
        }
        #endregion
    }
}
