using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;

namespace VEGA
{
    public partial class fMain : Form
    {
        [DllImport("kernel32")]
        static extern bool AllocConsole();


        private Framework.Proxy.Main proxy = null;
        private Framework.Client.Main client = null;
        private Framework.Client.DLL dll = null;

        private fLoader loader = new fLoader();
        public static bool isDone = false;

        public fMain()
        {
            Framework.Bot.Packet.Parsing.OnLogMsg += new Framework.Bot.Packet.Parsing.LogMsg(onLogMsg);
            Framework.Proxy.Main.OnLogMsg += new Framework.Proxy.Main.LogMsg(onLogMsg);

            Framework.Bot.Packet.Parsing.OnAddForm += new Framework.Bot.Packet.Parsing.AddForm(onAddForm);
            Framework.Bot.Packet.Parsing.OnShowForm += new Framework.Bot.Packet.Parsing.ShowForm(onShowForm);
            Framework.Bot.Packet.Parsing.OnAddMsgToForm += new Framework.Bot.Packet.Parsing.AddMsgToForm(onAddMsgToForm);
            Framework.Bot.Packet.Parsing.OnAddControls += new Framework.Bot.Packet.Parsing.AddControls(OnAddControls);

            fLoader loader = new fLoader();
            Thread thSplash = new Thread(new ThreadStart(SplashScreen));
            thSplash.Start();

            while (!isDone)
                Thread.Sleep(200);

            thSplash.Abort();
            InitializeComponent();
            //AllocConsole();
        }

        private void SplashScreen()
        {
            Application.Run(loader);
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            coBoLoginServer.Items.Insert(0, Framework.Global.ProxyGlobal.Server_Address);
            coBoLoginServer.SelectedIndex = 0;
            onLogMsg("* Successfully started VEGA");
        }
        private void fMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (proxy == null)
                return;

            proxy.Stop();
        }


        private void btnLaunch_Click(object sender, EventArgs e)
        {
            proxy = new Framework.Proxy.Main();
            proxy.Start();

            client = new Framework.Client.Main();
            client.Launch();

            dll = new Framework.Client.DLL(client.procClient);
            dll.Inject();
        }
        private void btnDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Set Silkroad Directory";
            fbd.ShowNewFolderButton = false;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                foreach (string filepath in Directory.GetFiles(fbd.SelectedPath))
                {
                    string currentfile = filepath.ToLower();

                    if (currentfile.Contains("sro_client.exe"))
                    {
                        Framework.Global.Global.ClientFilePath = filepath;
                    }
                    else if (currentfile.Contains("media.pk2"))
                    {
                        Framework.Global.PK2Global.PK2MediaFilePath = filepath;
                    }
                }

                if (Framework.Global.Global.ClientFilePath != "" && Framework.Global.PK2Global.PK2MediaFilePath != "")
                {
                    Framework.Global.Global.ClientPath = fbd.SelectedPath;
                    Framework.Settings.Ini.WriteIni("SILKROAD", "PATH", Framework.Global.Global.ClientPath, "VEGA.ini");
                    Framework.Settings.Ini.WriteIni("SILKROAD", "CLIENTPATH", Framework.Global.Global.ClientFilePath, "VEGA.ini");
                    Framework.Settings.Ini.WriteIni("SILKROAD", "MEDIAPATH", Framework.Global.PK2Global.PK2MediaFilePath, "VEGA.ini");
                }
                else
                {
                    MessageBox.Show("Invalid Silkroad Directory", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }

        private void onLogMsg(string message)
        {
            if (lbLog.InvokeRequired)
            {
                lbLog.Invoke(new Action(
                    delegate
                    {
                        lbLog.Items.Insert(0, string.Format("[{0:HH:mm:ss}] {1}", DateTime.Now, message));
                    }
                    ));
            }
            else
            {
                lbLog.Items.Insert(0, string.Format("[{0:HH:mm:ss}] {1}", DateTime.Now, message));
            }
        }

        private void onAddForm(String charName)
        {
            TabPage tp = new TabPage(charName);
            ListBox lb = new ListBox();
            Button bt = new Button();
            TextBox tb = new TextBox();

            bt.Text = "Send";
            bt.Name = "btn" + charName;
            bt.SetBounds(368, 169, 57, 23);
            bt.Click += new EventHandler(onButtonClick);

            tb.Name = "txt" + charName;
            tb.SetBounds(3, 172,359, 20);

            lb.Name = "lbChat";
            lb.SetBounds(0 ,0, 422, 160);

            tp.Name = "tp" + charName;
            tp.Controls.Add(bt);
            tp.Controls.Add(tb);
            tp.Controls.Add(lb);

            if (tcChats.InvokeRequired)
            {
                tcChats.Invoke(new Action(delegate { tcChats.Controls.Add(tp); }));
            }
            else
            {
                tcChats.Controls.Add(tp);
            }

            Framework.Global.BotGlobal.activeConversations.Add(charName, tp);
        }

        private void onButtonClick(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            
            String pmReceiver = btn.Name.Substring(3, btn.Name.Length - 3);
            String pmMessage = tcChats.Controls.Find("tp" + pmReceiver, true)[0].Controls.Find("txt" + pmReceiver, true)[0].Text;
            onAddMsgToFormSelf(pmMessage, pmReceiver);

            Framework.Bot.Packet.Building.BuildPrivateMessage(pmMessage, pmReceiver);
            tcChats.Controls.Find("tp" + pmReceiver, true)[0].Controls.Find("txt" + pmReceiver, true)[0].Text = "";
        }

        private void OnAddControls(String charName)
        {
            //ListBox lb = (ListBox)Framework.Global.BotGlobal.activeConversations[charName].Controls.Find("lbChat", true)[0];
        }

        private void onAddMsgToFormSelf(String msg, String Receiver)
        {
            ListBox lb = (ListBox)Framework.Global.BotGlobal.activeConversations[Receiver].Controls.Find("lbChat", true)[0];
            if (lb.InvokeRequired)
            {
                lb.Invoke(new Action(delegate
                {
                    lb.Items.Add("[YOU] " + ": " + msg);
                    lb.SelectedIndex = lb.Items.Count - 1;
                    lb.SelectedIndex = -1;
                }));
            }
            else
            {
                lb.Items.Add("[YOU] " + ": " + msg);
                lb.SelectedIndex = lb.Items.Count - 1;
                lb.SelectedIndex = -1;
            }
        }

        private void onAddMsgToForm(String charName, String msg)
        {
            ListBox lb = (ListBox)Framework.Global.BotGlobal.activeConversations[charName].Controls.Find("lbChat", true)[0];
            if (lb.InvokeRequired)
            {
                lb.Invoke(new Action(delegate { 
                    lb.Items.Add("[PM] " + charName + ": " + msg);
                    lb.SelectedIndex = lb.Items.Count - 1;
                    lb.SelectedIndex = -1;
                }));
            }
            else
            {
                lb.Items.Add("[PM] " + charName + ": " + msg);
                lb.SelectedIndex = lb.Items.Count - 1;
                lb.SelectedIndex = -1;
            }
        }

        private void onShowForm(String charName)
        {
            TabPage tp = Framework.Global.BotGlobal.activeConversations[charName];

            if (tp.InvokeRequired)
            {
             
            }
            else
            {
              
            }
        }
       
        private void btnSendGlobal_Click(object sender, EventArgs e)
        {
            Framework.Bot.Packet.Building.BuildPrivateMessage(Convert.ToString(tbSendText.Text), Convert.ToString("[BOT]System"));
            onLogMsg("* Successfully pmed the BOT System");
        }

    }
}
