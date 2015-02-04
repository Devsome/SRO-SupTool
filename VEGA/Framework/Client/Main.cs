using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace VEGA.Framework.Client
{
    class Main
    {
        public Process procClient = null;
        private IntPtr procHWND = IntPtr.Zero;

        public int procID = 0;

        public Main()
        {
            procClient = new Process();
            procClient.StartInfo.WorkingDirectory = Framework.Global.Global.ClientPath;
            procClient.StartInfo.FileName = Framework.Global.Global.ClientFilePath;
            procClient.StartInfo.Arguments = "/22 0 0";
        }

        public void Launch()
        {
            if (File.Exists(procClient.StartInfo.FileName))
                procClient.Start();

            procID = procClient.Id;
            procHWND = procClient.MainWindowHandle;
        }
        public void Close()
        {
            if (procClient == null || procClient.HasExited)
                return;

            procClient.Kill();
        }
    }
}
