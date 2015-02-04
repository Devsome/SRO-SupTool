using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace VEGA.Framework.Settings
{
    public class Ini
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);

        public static void WriteIni(string section, string key, string value, string filename)
        {
            bool b = WritePrivateProfileString(section, key, value, AppDomain.CurrentDomain.BaseDirectory + filename);
        }

        public static string ReadIni(string section, string key, string filename)
        {
            StringBuilder sb = new StringBuilder(500);
            GetPrivateProfileString(section, key, null, sb, (uint)sb.Capacity + 1, AppDomain.CurrentDomain.BaseDirectory + filename);
            return sb.ToString();
        }

        public static void LoadIni()
        {
            Framework.Global.Global.ClientPath = Framework.Settings.Ini.ReadIni("SILKROAD", "PATH", "VEGA.ini");
            Framework.Global.Global.ClientFilePath = Framework.Settings.Ini.ReadIni("SILKROAD", "CLIENTPATH", "VEGA.ini");
            Framework.Global.PK2Global.PK2MediaFilePath = Framework.Settings.Ini.ReadIni("SILKROAD", "MEDIAPATH", "VEGA.ini");
        }
    }
}
