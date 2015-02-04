using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VEGA.Framework.Global
{
    public class Global
    {
        public static string ClientPath = "";
        public static string ClientFilePath = "";
        public static string DllFilePath = AppDomain.CurrentDomain.BaseDirectory + "ubx.dll";

        public static byte chatCounter = 0;

        public static int WeaponElixirCount;
        public static int ShieldElixirCount;
        public static int AccessoryElixirCount;
        public static int ProtectorElixirCount;
        public static int LuckyCount;

        public static byte PimpIndex = 0;
        public static bool Fusing = false;
    }
}
