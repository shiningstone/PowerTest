namespace PowerTest
{
    partial class PowerTest
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.drpComList = new System.Windows.Forms.ComboBox();
            this.BTN_ComCtrl = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TB_Cmd = new System.Windows.Forms.TextBox();
            this.TB_Rsp = new System.Windows.Forms.TextBox();
            this.BTN_Send = new System.Windows.Forms.Button();
            this.TB_TestFile = new System.Windows.Forms.TextBox();
            this.BTN_Select = new System.Windows.Forms.Button();
            this.BTN_Start = new System.Windows.Forms.Button();
            this.TB_TestTimes = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TB_Log = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.CB_ElecModuleEnable = new System.Windows.Forms.CheckBox();
            this.CB_LogEnable = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "COM";
            // 
            // drpComList
            // 
            this.drpComList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.drpComList.FormattingEnabled = true;
            this.drpComList.Location = new System.Drawing.Point(54, 31);
            this.drpComList.Name = "drpComList";
            this.drpComList.Size = new System.Drawing.Size(68, 21);
            this.drpComList.TabIndex = 14;
            // 
            // BTN_ComCtrl
            // 
            this.BTN_ComCtrl.Enabled = false;
            this.BTN_ComCtrl.Location = new System.Drawing.Point(320, 31);
            this.BTN_ComCtrl.Name = "BTN_ComCtrl";
            this.BTN_ComCtrl.Size = new System.Drawing.Size(75, 23);
            this.BTN_ComCtrl.TabIndex = 16;
            this.BTN_ComCtrl.Text = "Open";
            this.BTN_ComCtrl.UseVisualStyleBackColor = true;
            this.BTN_ComCtrl.Click += new System.EventHandler(this.BTN_ComCtrl_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Command";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 132);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Response";
            // 
            // TB_Cmd
            // 
            this.TB_Cmd.Location = new System.Drawing.Point(68, 104);
            this.TB_Cmd.Name = "TB_Cmd";
            this.TB_Cmd.Size = new System.Drawing.Size(278, 20);
            this.TB_Cmd.TabIndex = 19;
            // 
            // TB_Rsp
            // 
            this.TB_Rsp.Location = new System.Drawing.Point(68, 132);
            this.TB_Rsp.Name = "TB_Rsp";
            this.TB_Rsp.ReadOnly = true;
            this.TB_Rsp.Size = new System.Drawing.Size(278, 20);
            this.TB_Rsp.TabIndex = 20;
            // 
            // BTN_Send
            // 
            this.BTN_Send.Location = new System.Drawing.Point(351, 102);
            this.BTN_Send.Name = "BTN_Send";
            this.BTN_Send.Size = new System.Drawing.Size(55, 23);
            this.BTN_Send.TabIndex = 21;
            this.BTN_Send.Text = "Send";
            this.BTN_Send.UseVisualStyleBackColor = true;
            this.BTN_Send.Click += new System.EventHandler(this.BTN_Send_Click);
            // 
            // TB_TestFile
            // 
            this.TB_TestFile.Location = new System.Drawing.Point(88, 34);
            this.TB_TestFile.Name = "TB_TestFile";
            this.TB_TestFile.Size = new System.Drawing.Size(207, 20);
            this.TB_TestFile.TabIndex = 22;
            // 
            // BTN_Select
            // 
            this.BTN_Select.Location = new System.Drawing.Point(316, 31);
            this.BTN_Select.Name = "BTN_Select";
            this.BTN_Select.Size = new System.Drawing.Size(75, 23);
            this.BTN_Select.TabIndex = 23;
            this.BTN_Select.Text = "Select...";
            this.BTN_Select.UseVisualStyleBackColor = true;
            this.BTN_Select.Click += new System.EventHandler(this.BTN_Select_Click);
            // 
            // BTN_Start
            // 
            this.BTN_Start.Location = new System.Drawing.Point(316, 60);
            this.BTN_Start.Name = "BTN_Start";
            this.BTN_Start.Size = new System.Drawing.Size(75, 23);
            this.BTN_Start.TabIndex = 24;
            this.BTN_Start.Text = "Start";
            this.BTN_Start.UseVisualStyleBackColor = true;
            this.BTN_Start.Click += new System.EventHandler(this.BTN_Start_Click);
            // 
            // TB_TestTimes
            // 
            this.TB_TestTimes.Location = new System.Drawing.Point(10, 62);
            this.TB_TestTimes.Name = "TB_TestTimes";
            this.TB_TestTimes.Size = new System.Drawing.Size(100, 20);
            this.TB_TestTimes.TabIndex = 25;
            this.TB_TestTimes.Text = "1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(116, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 26;
            this.label4.Text = "Times";
            // 
            // TB_Log
            // 
            this.TB_Log.Location = new System.Drawing.Point(416, 30);
            this.TB_Log.Multiline = true;
            this.TB_Log.Name = "TB_Log";
            this.TB_Log.ReadOnly = true;
            this.TB_Log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TB_Log.Size = new System.Drawing.Size(429, 381);
            this.TB_Log.TabIndex = 27;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CB_ElecModuleEnable);
            this.groupBox1.Controls.Add(this.BTN_ComCtrl);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.drpComList);
            this.groupBox1.Location = new System.Drawing.Point(4, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(406, 76);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Com";
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(4, 82);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(407, 84);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Single Command Test";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.TB_TestFile);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.BTN_Select);
            this.groupBox3.Controls.Add(this.TB_TestTimes);
            this.groupBox3.Controls.Add(this.BTN_Start);
            this.groupBox3.Location = new System.Drawing.Point(4, 172);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(407, 88);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Multiple Command Text";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 29;
            this.label5.Text = "Command File";
            // 
            // CB_ElecModuleEnable
            // 
            this.CB_ElecModuleEnable.AutoSize = true;
            this.CB_ElecModuleEnable.Location = new System.Drawing.Point(163, 36);
            this.CB_ElecModuleEnable.Name = "CB_ElecModuleEnable";
            this.CB_ElecModuleEnable.Size = new System.Drawing.Size(96, 17);
            this.CB_ElecModuleEnable.TabIndex = 30;
            this.CB_ElecModuleEnable.Text = "ElectricModule";
            this.CB_ElecModuleEnable.UseVisualStyleBackColor = true;
            // 
            // CB_LogEnable
            // 
            this.CB_LogEnable.AutoSize = true;
            this.CB_LogEnable.Location = new System.Drawing.Point(766, 7);
            this.CB_LogEnable.Name = "CB_LogEnable";
            this.CB_LogEnable.Size = new System.Drawing.Size(80, 17);
            this.CB_LogEnable.TabIndex = 29;
            this.CB_LogEnable.Text = "Log Enable";
            this.CB_LogEnable.UseVisualStyleBackColor = true;
            this.CB_LogEnable.CheckedChanged += new System.EventHandler(this.CB_LogEnable_CheckedChanged);
            // 
            // PowerTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 414);
            this.Controls.Add(this.CB_LogEnable);
            this.Controls.Add(this.TB_Log);
            this.Controls.Add(this.BTN_Send);
            this.Controls.Add(this.TB_Rsp);
            this.Controls.Add(this.TB_Cmd);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "PowerTest";
            this.Text = "Power Test";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox drpComList;
        private System.Windows.Forms.Button BTN_ComCtrl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TB_Cmd;
        private System.Windows.Forms.TextBox TB_Rsp;
        private System.Windows.Forms.Button BTN_Send;
        private System.Windows.Forms.TextBox TB_TestFile;
        private System.Windows.Forms.Button BTN_Select;
        private System.Windows.Forms.Button BTN_Start;
        private System.Windows.Forms.TextBox TB_TestTimes;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TB_Log;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox CB_ElecModuleEnable;
        private System.Windows.Forms.CheckBox CB_LogEnable;
    }
}

