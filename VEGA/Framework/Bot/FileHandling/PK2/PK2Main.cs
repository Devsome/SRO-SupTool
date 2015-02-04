using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace VEGA.Framework.Bot.FileHandling.PK2
{
    public class PK2Main
    {
        private Framework.Bot.FileHandling.PK2.PK2Class pk2Class = new Framework.Bot.FileHandling.PK2.PK2Class(Framework.Global.PK2Global.PK2MediaFilePath);
        private UnicodeEncoding enc = new UnicodeEncoding();

        public void SetUp()
        {
            Framework.Global.PK2Global.SetEmpty();
        }

        public void GetServerIPAddress()
        {
            byte[] bServerIPAddress = pk2Class.getFile("divisioninfo.txt");

            if (bServerIPAddress == null)
                return;

            using (StreamReader sr = new StreamReader(new MemoryStream(bServerIPAddress)))
            {
                string strServerIPAddress = sr.ReadToEnd();
                string[] arrServerIPAddress = strServerIPAddress.Split('\0');
                Global.ProxyGlobal.Server_Division = arrServerIPAddress[arrServerIPAddress.Length - 6];
                Global.ProxyGlobal.Server_Address = arrServerIPAddress[arrServerIPAddress.Length - 2];
            }
        }
        public void GetServerPort()
        {
            byte[] bServerPort = pk2Class.getFile("gateport.txt");

            if (bServerPort == null)
                return;

            using (StreamReader sr = new StreamReader(new MemoryStream(bServerPort)))
            {
                Global.ProxyGlobal.Server_Gateway_Port = Convert.ToInt32(sr.ReadToEnd().Replace("\0", ""));
            }
        }
        public void CleanUp()
        {
            pk2Class = null;
            enc = null;
            GC.Collect();
        }
    }
}
