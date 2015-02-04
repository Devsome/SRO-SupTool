namespace VEGA
{
    partial class fMain
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fMain));
            this.grpSilkroad = new System.Windows.Forms.GroupBox();
            this.coBoLoginServer = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnDirectory = new System.Windows.Forms.Button();
            this.btnLaunch = new System.Windows.Forms.Button();
            this.lbLog = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbSendText = new System.Windows.Forms.TextBox();
            this.btnSendGlobal = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tcChats = new System.Windows.Forms.TabControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.grpSilkroad.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSilkroad
            // 
            this.grpSilkroad.Controls.Add(this.coBoLoginServer);
            this.grpSilkroad.Controls.Add(this.label7);
            this.grpSilkroad.Controls.Add(this.btnDirectory);
            this.grpSilkroad.Controls.Add(this.btnLaunch);
            this.grpSilkroad.Location = new System.Drawing.Point(12, 12);
            this.grpSilkroad.Name = "grpSilkroad";
            this.grpSilkroad.Size = new System.Drawing.Size(223, 191);
            this.grpSilkroad.TabIndex = 1;
            this.grpSilkroad.TabStop = false;
            this.grpSilkroad.Text = "Silkroad";
            // 
            // coBoLoginServer
            // 
            this.coBoLoginServer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.coBoLoginServer.FormattingEnabled = true;
            this.coBoLoginServer.Location = new System.Drawing.Point(15, 48);
            this.coBoLoginServer.Name = "coBoLoginServer";
            this.coBoLoginServer.Size = new System.Drawing.Size(151, 21);
            this.coBoLoginServer.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 32);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Loginserver";
            // 
            // btnDirectory
            // 
            this.btnDirectory.Location = new System.Drawing.Point(15, 116);
            this.btnDirectory.Name = "btnDirectory";
            this.btnDirectory.Size = new System.Drawing.Size(130, 23);
            this.btnDirectory.TabIndex = 2;
            this.btnDirectory.Text = "Set Silkroad Directory";
            this.btnDirectory.UseVisualStyleBackColor = true;
            this.btnDirectory.Click += new System.EventHandler(this.btnDirectory_Click);
            // 
            // btnLaunch
            // 
            this.btnLaunch.Location = new System.Drawing.Point(15, 86);
            this.btnLaunch.Name = "btnLaunch";
            this.btnLaunch.Size = new System.Drawing.Size(130, 24);
            this.btnLaunch.TabIndex = 2;
            this.btnLaunch.Text = "Launch Silkroad";
            this.btnLaunch.UseVisualStyleBackColor = true;
            this.btnLaunch.Click += new System.EventHandler(this.btnLaunch_Click);
            // 
            // lbLog
            // 
            this.lbLog.FormattingEnabled = true;
            this.lbLog.Location = new System.Drawing.Point(6, 19);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(665, 82);
            this.lbLog.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbSendText);
            this.groupBox1.Controls.Add(this.btnSendGlobal);
            this.groupBox1.Location = new System.Drawing.Point(241, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(448, 56);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Global via [BOT]System";
            // 
            // tbSendText
            // 
            this.tbSendText.Location = new System.Drawing.Point(6, 23);
            this.tbSendText.Name = "tbSendText";
            this.tbSendText.Size = new System.Drawing.Size(348, 20);
            this.tbSendText.TabIndex = 1;
            this.tbSendText.Text = "If you need help, just Pm me";
            // 
            // btnSendGlobal
            // 
            this.btnSendGlobal.Location = new System.Drawing.Point(360, 23);
            this.btnSendGlobal.Name = "btnSendGlobal";
            this.btnSendGlobal.Size = new System.Drawing.Size(82, 20);
            this.btnSendGlobal.TabIndex = 0;
            this.btnSendGlobal.Text = "Send";
            this.btnSendGlobal.UseVisualStyleBackColor = true;
            this.btnSendGlobal.Click += new System.EventHandler(this.btnSendGlobal_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lbLog);
            this.groupBox3.Location = new System.Drawing.Point(12, 327);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(677, 107);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Log";
            // 
            // tcChats
            // 
            this.tcChats.Location = new System.Drawing.Point(6, 19);
            this.tcChats.Name = "tcChats";
            this.tcChats.SelectedIndex = 0;
            this.tcChats.Size = new System.Drawing.Size(436, 221);
            this.tcChats.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tcChats);
            this.groupBox2.Location = new System.Drawing.Point(241, 75);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(448, 246);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Privatechat";
            // 
            // fMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 445);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpSilkroad);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "fMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Supportertool for Elamidas ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fMain_FormClosing);
            this.Load += new System.EventHandler(this.fMain_Load);
            this.grpSilkroad.ResumeLayout(false);
            this.grpSilkroad.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSilkroad;
        private System.Windows.Forms.Button btnLaunch;
        private System.Windows.Forms.Button btnDirectory;
        private System.Windows.Forms.ListBox lbLog;
        private System.Windows.Forms.ComboBox coBoLoginServer;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbSendText;
        private System.Windows.Forms.Button btnSendGlobal;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TabControl tcChats;
        private System.Windows.Forms.GroupBox groupBox2;

    }
}

