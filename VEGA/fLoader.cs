using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace VEGA
{
    public partial class fLoader : Form
    {
        public fLoader()
        {
            InitializeComponent();
        }

        private void fLoader_Load(object sender, EventArgs e)
        {
            Framework.Settings.Ini.LoadIni();
            Thread thThread = new Thread(new ThreadStart(LoadPK2Data));
            thThread.Start();
        }

        private void LoadPK2Data()
        {
            Framework.Bot.FileHandling.PK2.PK2Main pk2 = new Framework.Bot.FileHandling.PK2.PK2Main();
            UpdateControls(10, "Setting Up");
            pk2.SetUp();
            UpdateControls(20, "Getting Server IP Address");
            pk2.GetServerIPAddress();
            UpdateControls(30, "Getting Server Port");
            pk2.GetServerPort();
            UpdateControls(100, "Cleaning Up");
            pk2.CleanUp();
            fMain.isDone = true;
        }

        private void UpdateControls(int progress, string status)
        {
            if (pbProgress.InvokeRequired)
            {
                pbProgress.Invoke(new Action(
                    delegate
                    {
                        pbProgress.Value = progress;
                    }
                    ));
            }
            else
            {
                pbProgress.Value = progress;
            }

            if (labStatus.InvokeRequired)
            {
                labStatus.Invoke(new Action(
                    delegate
                    {
                        labStatus.Text = string.Format("Status: {0}", status);
                    }
                    ));
            }
            else
            {
                labStatus.Text = string.Format("Status: {0}", status);
            }
        }
    }
}
