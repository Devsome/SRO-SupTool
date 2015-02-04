using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VEGA.Framework.Global
{
    public class BotGlobal
    {
        public static List<Framework.Bot.Structs.Silkroad.Server> server;
        public static List<Framework.Bot.Structs.Silkroad.ListedCharacter> listedCharacters;

        public static Dictionary<String, TabPage> activeConversations = new Dictionary<String, TabPage>();
    }
}
