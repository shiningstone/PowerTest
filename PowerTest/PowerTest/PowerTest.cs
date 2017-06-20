
//#define rftp
#define jzh

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BIModel;
using System.Threading;
using System.IO;
using APPLEDIE;

namespace PowerTest
{

    public partial class PowerTest : Form
    {
        Iport port = null;
#region UI interface
        delegate void AsynUpdateUi(string str);
        private void UpdateResult(string str)
        {
            if (SendFileRunning)
            {
                TB_Result.Text = str;
            }
            else
            {
                TB_StabilityResult.Text = str;
            }
        }
        private void UpdateUi(string str)
        {
            Logger.Show(Logger.Level.Bus, str);

            if (InvokeRequired)
            {
                this.Invoke(new AsynUpdateUi(delegate (string s) {
                    UpdateResult(s);
                }), str);
            }
            else
            {
                UpdateResult(str);
            }
        }
        delegate void AsynTaskDone();
        private void EnableNewTest()
        {
            if (SendFileRunning)
            {
                SendFileRunning = false;
                BTN_Start.Enabled = true;
            }
            else
            {
                StabilityTestRunning = false;
                BTN_StabilityStart.Enabled = true;
            }
        }
        private void TaskDone()
        {
            Logger.Show(Logger.Level.Bus, "Task Done");

            if (InvokeRequired)
            {
                this.Invoke(new AsynTaskDone(delegate () {
                    EnableNewTest();
                }));
            }
            else
            {
                EnableNewTest();
            }
        }
#endregion
        public PowerTest()
        {
            InitializeComponent();

            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                CMB_ComList.Items.Add(port);
            }
            CMB_ComList.Items.Add("Mock");

            if (CMB_ComList.Items.Count > 0)
            {
                CMB_ComList.SelectedIndex = 0;
                BTN_ComCtrl.Enabled = true;
            }

            CMB_LogLevel.Items.Add("Operation");
            CMB_LogLevel.Items.Add("Bus");
            CMB_LogLevel.Items.Add("Command");
            CMB_LogLevel.SelectedIndex = 0;
            Logger.mLevel = Logger.Level.Operation;

            /*Single Command Test*/
            if (TB_Cmd.Text.Length == 0)
            {
                TB_Cmd.Text = Properties.Settings.Default.DefaultCmd;
            }
            /*Multiple Command Test*/
            TB_TestFile.Text = Properties.Settings.Default.TestFile;
            TB_TestTimes.Text = Properties.Settings.Default.TestTimes;
            if (Properties.Settings.Default.TestMode.Equals("Times"))
            {
                RB_TestTimes.Checked = true;
            }
            else
            {
                RB_TestMinutes.Checked = true;
            }
            /*Stability Test*/
            string[] StabilityTest = JzhTest.GetAllTests();
            for (int i = 0; i < StabilityTest.Length; i++)
            {
                CMB_TestType.Items.Add(StabilityTest[i]);
            }
            CMB_TestType.SelectedIndex = 0;

            if (CB_LogEnable.Checked)
            {
                Logger.updateUI += new Logger.UpdateUI(log => { TB_Log.Text += "\r\n" + log; });
            }

            CB_ElecModuleEnable.Checked = true;
            RB_PartA.Checked = true;
        }

        private bool SendFileRunning = false;
        private bool StabilityTestRunning = false;
        private void BTN_ComCtrl_Click(object sender, EventArgs e)
        {
            if (port == null)
            {
                if (!CB_ElecModuleEnable.Checked)
                {
                    port = Comm.GetComm(CMB_ComList.SelectedItem.ToString(), 115200);
                }
                else
                {
                    string part = RB_PartA.Checked ? "A" : "B";
                    port = new JzhPower(Comm.GetComm(CMB_ComList.SelectedItem.ToString(), 38400), part);
                }
                port.Open();

                BTN_ComCtrl.Text = "Close";
            }
            else
            {
                if (port.IsOpen())
                {
                    port.Close();
                    BTN_ComCtrl.Text = "Open";
                }
                else
                {
                    port.Open();
                    BTN_ComCtrl.Text = "Close";
                }
            }

        }

        private void BTN_Send_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.DefaultCmd = TB_Cmd.Text;
            Properties.Settings.Default.Save();

            int forceSleep = 0;
            try
            {
                forceSleep = Int32.Parse(TB_Sleep.Text);
            }
            catch
            { }

            byte[] cmd = Util.HexStringToBytes(TB_Cmd.Text.Replace(" ", ""));

            BTN_Send.Enabled = false;
            byte[] rsp = port.Query(Util.Frame(cmd), forceSleep);
            BTN_Send.Enabled = true;

            string output = BitConverter.ToString(rsp).Replace("-", " ");
            TB_Rsp.Text = output;
        }

        private void BTN_Select_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Cmd文件（*.cmd）|*.cmd";

            sfd.FilterIndex = 1;
            sfd.InitialDirectory = Environment.CurrentDirectory;
            sfd.RestoreDirectory = true;
            sfd.FileName = Properties.Settings.Default.TestFile;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                TB_TestFile.Text = sfd.FileName;
            }
        }

        private void BTN_Start_Click(object sender, EventArgs e)
        {
            int times = 0;
            SendFileRunning = true;

            if (RB_TestTimes.Checked)
            {
                times = Int32.Parse(TB_TestTimes.Text);
                Properties.Settings.Default.TestTimes = TB_TestTimes.Text;
                Properties.Settings.Default.TestMode = "Times";
            }
            else
            {
                times = Int32.Parse(TB_TestMinutes.Text);
                Properties.Settings.Default.TestMinutes = TB_TestMinutes.Text;
                Properties.Settings.Default.TestMode = "Minute";
            }

            Properties.Settings.Default.TestFile = TB_TestFile.Text;
            Properties.Settings.Default.Save();

            BTN_Start.Enabled = false;

            TB_Result.Text = "Performance @ " + System.DateTime.Now + ", File: " + Path.GetFileName(TB_TestFile.Text);
            Logger.Show(Logger.Level.Operation, TB_Result.Text);

            //Logger.mLevel = Logger.Level.Operation;
            SendFile aTransmission = new SendFile(port, TB_TestFile.Text, times, Properties.Settings.Default.TestMode, CB_LogFileEnable.Checked);
            aTransmission.updateUi = UpdateUi;
            aTransmission.taskDone = TaskDone;
            Thread t = new Thread(new ThreadStart(aTransmission.Run));
            t.Start();
        }
        private void BTN_StabilityStart_Click(object sender, EventArgs e)
        {
            if (!CB_ElecModuleEnable.Checked)
            {
                MessageBox.Show("ElectricModule should be checked");
                return;
            }

            StabilityTestRunning = true;
            BTN_StabilityStart.Enabled = false;

            int option = CMB_TestType.SelectedIndex;
            int duration = Int32.Parse(TB_Duration.Text);
            bool forceClose = CB_ForceClose.Checked;
            bool forceDisconnect = CB_ForceDisconnect.Checked;

            JzhTest aTest = null;
            switch (option)
            {
                case 0:
                    {
                        aTest = new SingleCurrentTest(port, 2000, duration, 1000, CB_LogFileEnable.Checked);
                        break;
                    }
                case 1:
                    {
                        double[] tests = new double[] { 500, 1000, 1500, 2000 };
                        aTest = new MultiCurrentTest(port, tests, duration, 1000, CB_LogFileEnable.Checked);
                        break;
                    }
            }

            aTest.updateUi = UpdateUi;
            aTest.taskDone = TaskDone;
            Thread t = new Thread(new ThreadStart(aTest.Run));
            t.Start();
        }
        private void CB_LogEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (CB_LogEnable.Checked)
            {
                Logger.updateUI += new Logger.UpdateUI(log => { TB_Log.Text += "\r\n" + log; });
            }
            else
            {
                TB_Log.Text = "";
                Logger.updateUI = null;
            }
        }
        private void CB_ElecModuleEnable_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void CMB_LogLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (CMB_LogLevel.SelectedIndex)
            {
                case 0:
                    Logger.mLevel = Logger.Level.Operation;
                    break;
                case 1:
                    Logger.mLevel = Logger.Level.Bus;
                    break;
                case 2:
                    Logger.mLevel = Logger.Level.Command;
                    break;
            }
        }

        private void BTN_Temp_Click(object sender, EventArgs e)
        {
            JzhPower jzh = port as JzhPower;

            int[] chnl = new int[] { 1 };
            string[] str = new string[40];
            for (int i = 0; i < str.Length; i++)
            {
                str[i] = "0";
            }
            for (int i=0;i<chnl.Length;i++)
            {
                if (i == chnl[i])
                {
                    str[i] = "1";
                }
            }

            jzh.SetCurrentChannels(1000, str);
        }
    }
}
