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
            this.CMB_ComList = new System.Windows.Forms.ComboBox();
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
            this.RB_PartA = new System.Windows.Forms.RadioButton();
            this.CB_ElecModuleEnable = new System.Windows.Forms.CheckBox();
            this.RB_PartB = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.RB_TestMinutes = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.TB_TestMinutes = new System.Windows.Forms.TextBox();
            this.RB_TestTimes = new System.Windows.Forms.RadioButton();
            this.TB_Result = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.CB_LogEnable = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.TB_StabilityResult = new System.Windows.Forms.Label();
            this.BTN_StabilityStart = new System.Windows.Forms.Button();
            this.CB_ForceDisconnect = new System.Windows.Forms.CheckBox();
            this.CB_ForceClose = new System.Windows.Forms.CheckBox();
            this.Minutes = new System.Windows.Forms.Label();
            this.TB_Duration = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.CMB_TestType = new System.Windows.Forms.ComboBox();
            this.CMB_LogLevel = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.TB_Sleep = new System.Windows.Forms.TextBox();
            this.CB_LogFileEnable = new System.Windows.Forms.CheckBox();
            this.BTN_Temp = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
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
            // CMB_ComList
            // 
            this.CMB_ComList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CMB_ComList.FormattingEnabled = true;
            this.CMB_ComList.Location = new System.Drawing.Point(54, 31);
            this.CMB_ComList.Name = "CMB_ComList";
            this.CMB_ComList.Size = new System.Drawing.Size(68, 21);
            this.CMB_ComList.TabIndex = 14;
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
            this.TB_Cmd.Text = "68 01 01 08 10 30 00 28 ";
            // 
            // TB_Rsp
            // 
            this.TB_Rsp.Location = new System.Drawing.Point(68, 132);
            this.TB_Rsp.Name = "TB_Rsp";
            this.TB_Rsp.ReadOnly = true;
            this.TB_Rsp.Size = new System.Drawing.Size(243, 20);
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
            this.TB_TestTimes.Location = new System.Drawing.Point(30, 62);
            this.TB_TestTimes.Name = "TB_TestTimes";
            this.TB_TestTimes.Size = new System.Drawing.Size(56, 20);
            this.TB_TestTimes.TabIndex = 25;
            this.TB_TestTimes.Text = "1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(87, 65);
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
            this.TB_Log.Size = new System.Drawing.Size(429, 407);
            this.TB_Log.TabIndex = 27;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.RB_PartA);
            this.groupBox1.Controls.Add(this.CB_ElecModuleEnable);
            this.groupBox1.Controls.Add(this.RB_PartB);
            this.groupBox1.Controls.Add(this.BTN_ComCtrl);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.CMB_ComList);
            this.groupBox1.Location = new System.Drawing.Point(4, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(406, 76);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Com";
            // 
            // RB_PartA
            // 
            this.RB_PartA.AutoSize = true;
            this.RB_PartA.Location = new System.Drawing.Point(183, 46);
            this.RB_PartA.Name = "RB_PartA";
            this.RB_PartA.Size = new System.Drawing.Size(32, 17);
            this.RB_PartA.TabIndex = 31;
            this.RB_PartA.TabStop = true;
            this.RB_PartA.Text = "A";
            this.RB_PartA.UseVisualStyleBackColor = true;
            // 
            // CB_ElecModuleEnable
            // 
            this.CB_ElecModuleEnable.AutoSize = true;
            this.CB_ElecModuleEnable.Location = new System.Drawing.Point(165, 23);
            this.CB_ElecModuleEnable.Name = "CB_ElecModuleEnable";
            this.CB_ElecModuleEnable.Size = new System.Drawing.Size(96, 17);
            this.CB_ElecModuleEnable.TabIndex = 30;
            this.CB_ElecModuleEnable.Text = "ElectricModule";
            this.CB_ElecModuleEnable.UseVisualStyleBackColor = true;
            this.CB_ElecModuleEnable.CheckedChanged += new System.EventHandler(this.CB_ElecModuleEnable_CheckedChanged);
            // 
            // RB_PartB
            // 
            this.RB_PartB.AutoSize = true;
            this.RB_PartB.Location = new System.Drawing.Point(221, 46);
            this.RB_PartB.Name = "RB_PartB";
            this.RB_PartB.Size = new System.Drawing.Size(32, 17);
            this.RB_PartB.TabIndex = 32;
            this.RB_PartB.TabStop = true;
            this.RB_PartB.Text = "B";
            this.RB_PartB.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Location = new System.Drawing.Point(3, 92);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(407, 84);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Single Command Test";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(310, 45);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(51, 13);
            this.label10.TabIndex = 40;
            this.label10.Text = "sleep(ms)";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.RB_TestMinutes);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.TB_TestMinutes);
            this.groupBox3.Controls.Add(this.RB_TestTimes);
            this.groupBox3.Controls.Add(this.TB_Result);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.TB_TestFile);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.BTN_Select);
            this.groupBox3.Controls.Add(this.TB_TestTimes);
            this.groupBox3.Controls.Add(this.BTN_Start);
            this.groupBox3.Location = new System.Drawing.Point(3, 191);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(407, 108);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Multiple Command Test";
            // 
            // RB_TestMinutes
            // 
            this.RB_TestMinutes.AutoSize = true;
            this.RB_TestMinutes.Location = new System.Drawing.Point(135, 64);
            this.RB_TestMinutes.Name = "RB_TestMinutes";
            this.RB_TestMinutes.Size = new System.Drawing.Size(14, 13);
            this.RB_TestMinutes.TabIndex = 36;
            this.RB_TestMinutes.TabStop = true;
            this.RB_TestMinutes.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(210, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 13);
            this.label6.TabIndex = 35;
            this.label6.Text = "Minutes";
            // 
            // TB_TestMinutes
            // 
            this.TB_TestMinutes.Location = new System.Drawing.Point(153, 61);
            this.TB_TestMinutes.Name = "TB_TestMinutes";
            this.TB_TestMinutes.Size = new System.Drawing.Size(56, 20);
            this.TB_TestMinutes.TabIndex = 34;
            this.TB_TestMinutes.Text = "1";
            // 
            // RB_TestTimes
            // 
            this.RB_TestTimes.AutoSize = true;
            this.RB_TestTimes.Location = new System.Drawing.Point(12, 65);
            this.RB_TestTimes.Name = "RB_TestTimes";
            this.RB_TestTimes.Size = new System.Drawing.Size(14, 13);
            this.RB_TestTimes.TabIndex = 33;
            this.RB_TestTimes.TabStop = true;
            this.RB_TestTimes.UseVisualStyleBackColor = true;
            // 
            // TB_Result
            // 
            this.TB_Result.AutoSize = true;
            this.TB_Result.Location = new System.Drawing.Point(9, 90);
            this.TB_Result.Name = "TB_Result";
            this.TB_Result.Size = new System.Drawing.Size(0, 13);
            this.TB_Result.TabIndex = 30;
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
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.BTN_Temp);
            this.groupBox4.Controls.Add(this.TB_StabilityResult);
            this.groupBox4.Controls.Add(this.BTN_StabilityStart);
            this.groupBox4.Controls.Add(this.CB_ForceDisconnect);
            this.groupBox4.Controls.Add(this.CB_ForceClose);
            this.groupBox4.Controls.Add(this.Minutes);
            this.groupBox4.Controls.Add(this.TB_Duration);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.CMB_TestType);
            this.groupBox4.Location = new System.Drawing.Point(5, 312);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(406, 125);
            this.groupBox4.TabIndex = 37;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Stability Test";
            // 
            // TB_StabilityResult
            // 
            this.TB_StabilityResult.AutoSize = true;
            this.TB_StabilityResult.Location = new System.Drawing.Point(8, 106);
            this.TB_StabilityResult.Name = "TB_StabilityResult";
            this.TB_StabilityResult.Size = new System.Drawing.Size(0, 13);
            this.TB_StabilityResult.TabIndex = 38;
            // 
            // BTN_StabilityStart
            // 
            this.BTN_StabilityStart.Location = new System.Drawing.Point(316, 83);
            this.BTN_StabilityStart.Name = "BTN_StabilityStart";
            this.BTN_StabilityStart.Size = new System.Drawing.Size(75, 23);
            this.BTN_StabilityStart.TabIndex = 37;
            this.BTN_StabilityStart.Text = "Start";
            this.BTN_StabilityStart.UseVisualStyleBackColor = true;
            this.BTN_StabilityStart.Click += new System.EventHandler(this.BTN_StabilityStart_Click);
            // 
            // CB_ForceDisconnect
            // 
            this.CB_ForceDisconnect.AutoSize = true;
            this.CB_ForceDisconnect.Location = new System.Drawing.Point(135, 88);
            this.CB_ForceDisconnect.Name = "CB_ForceDisconnect";
            this.CB_ForceDisconnect.Size = new System.Drawing.Size(150, 17);
            this.CB_ForceDisconnect.TabIndex = 43;
            this.CB_ForceDisconnect.Text = "With Connect/Disconnect";
            this.CB_ForceDisconnect.UseVisualStyleBackColor = true;
            // 
            // CB_ForceClose
            // 
            this.CB_ForceClose.AutoSize = true;
            this.CB_ForceClose.Location = new System.Drawing.Point(18, 88);
            this.CB_ForceClose.Name = "CB_ForceClose";
            this.CB_ForceClose.Size = new System.Drawing.Size(108, 17);
            this.CB_ForceClose.TabIndex = 42;
            this.CB_ForceClose.Text = "With Open/Close";
            this.CB_ForceClose.UseVisualStyleBackColor = true;
            // 
            // Minutes
            // 
            this.Minutes.AutoSize = true;
            this.Minutes.Location = new System.Drawing.Point(155, 54);
            this.Minutes.Name = "Minutes";
            this.Minutes.Size = new System.Drawing.Size(44, 13);
            this.Minutes.TabIndex = 41;
            this.Minutes.Text = "Minutes";
            // 
            // TB_Duration
            // 
            this.TB_Duration.Location = new System.Drawing.Point(81, 51);
            this.TB_Duration.Name = "TB_Duration";
            this.TB_Duration.Size = new System.Drawing.Size(68, 20);
            this.TB_Duration.TabIndex = 40;
            this.TB_Duration.Text = "1";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, 54);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 13);
            this.label8.TabIndex = 39;
            this.label8.Text = "Duration";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 13);
            this.label7.TabIndex = 38;
            this.label7.Text = "Test Type";
            // 
            // CMB_TestType
            // 
            this.CMB_TestType.FormattingEnabled = true;
            this.CMB_TestType.Location = new System.Drawing.Point(80, 21);
            this.CMB_TestType.Name = "CMB_TestType";
            this.CMB_TestType.Size = new System.Drawing.Size(121, 21);
            this.CMB_TestType.TabIndex = 0;
            // 
            // CMB_LogLevel
            // 
            this.CMB_LogLevel.FormattingEnabled = true;
            this.CMB_LogLevel.Location = new System.Drawing.Point(469, 5);
            this.CMB_LogLevel.Name = "CMB_LogLevel";
            this.CMB_LogLevel.Size = new System.Drawing.Size(121, 21);
            this.CMB_LogLevel.TabIndex = 38;
            this.CMB_LogLevel.SelectedIndexChanged += new System.EventHandler(this.CMB_LogLevel_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(416, 11);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(51, 13);
            this.label9.TabIndex = 39;
            this.label9.Text = "LogLevel";
            // 
            // TB_Sleep
            // 
            this.TB_Sleep.AcceptsReturn = true;
            this.TB_Sleep.Location = new System.Drawing.Point(367, 132);
            this.TB_Sleep.Name = "TB_Sleep";
            this.TB_Sleep.Size = new System.Drawing.Size(37, 20);
            this.TB_Sleep.TabIndex = 40;
            this.TB_Sleep.Text = "0";
            // 
            // CB_LogFileEnable
            // 
            this.CB_LogFileEnable.AutoSize = true;
            this.CB_LogFileEnable.Checked = true;
            this.CB_LogFileEnable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_LogFileEnable.Location = new System.Drawing.Point(605, 8);
            this.CB_LogFileEnable.Name = "CB_LogFileEnable";
            this.CB_LogFileEnable.Size = new System.Drawing.Size(99, 17);
            this.CB_LogFileEnable.TabIndex = 37;
            this.CB_LogFileEnable.Text = "Log File Enable";
            this.CB_LogFileEnable.UseVisualStyleBackColor = true;
            // 
            // BTN_Temp
            // 
            this.BTN_Temp.Location = new System.Drawing.Point(314, 19);
            this.BTN_Temp.Name = "BTN_Temp";
            this.BTN_Temp.Size = new System.Drawing.Size(75, 23);
            this.BTN_Temp.TabIndex = 41;
            this.BTN_Temp.Text = "TempDbg";
            this.BTN_Temp.UseVisualStyleBackColor = true;
            this.BTN_Temp.Click += new System.EventHandler(this.BTN_Temp_Click);
            // 
            // PowerTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 440);
            this.Controls.Add(this.CB_LogFileEnable);
            this.Controls.Add(this.TB_Sleep);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.CMB_LogLevel);
            this.Controls.Add(this.groupBox4);
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
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CMB_ComList;
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
        private System.Windows.Forms.Label TB_Result;
        private System.Windows.Forms.RadioButton RB_TestMinutes;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox TB_TestMinutes;
        private System.Windows.Forms.RadioButton RB_TestTimes;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox CMB_TestType;
        private System.Windows.Forms.Label Minutes;
        private System.Windows.Forms.TextBox TB_Duration;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox CB_ForceDisconnect;
        private System.Windows.Forms.CheckBox CB_ForceClose;
        private System.Windows.Forms.Button BTN_StabilityStart;
        private System.Windows.Forms.Label TB_StabilityResult;
        private System.Windows.Forms.RadioButton RB_PartA;
        private System.Windows.Forms.RadioButton RB_PartB;
        private System.Windows.Forms.ComboBox CMB_LogLevel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox TB_Sleep;
        private System.Windows.Forms.CheckBox CB_LogFileEnable;
        private System.Windows.Forms.Button BTN_Temp;
    }
}

