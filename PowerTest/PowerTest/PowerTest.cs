
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
        #region async ui interfaces
        private void UpdateTestSummary(string str)
        {
            if (!SendFileRunning)
            {
                TB_Result.Text = str;
            }
            if (!StabilityTestRunning)
            {
                TB_StabilityResult.Text = str;
            }
        }
        #endregion
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

            byte[] cmd = _HexStringToBytes(TB_Cmd.Text.Replace(" ", ""));
            byte[] rsp = port.Query(Frame(cmd));

            string output = BitConverter.ToString(rsp).Replace("-", " ");
            TB_Rsp.Text = output;
        }
        private static byte[] _HexStringToBytes(string hexString)
        {
            try
            {
                byte[] returnBytes = new byte[hexString.Length / 2];

                for (int i = 0; i < returnBytes.Length; i++)
                    returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);

                return returnBytes;
            }
            catch (Exception e)
            {
                return new byte[0];
            }
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

            Logger.mLevel = Logger.Level.Operation;
            SendFile aTransmission = new SendFile(port, TB_TestFile.Text, times, Properties.Settings.Default.TestMode);
            aTransmission.updateUi = UpdateUi;
            Thread t = new Thread(new ThreadStart(aTransmission.Run));
            t.Start();
            //aTransmission.Run();

            BTN_Start.Enabled = true;
        }
        delegate void AsynUpdateUi(string str);
        private void UpdateUi(string str)
        {
            Logger.Show(Logger.Level.Operation, str);

            if (InvokeRequired)
            {
                this.Invoke(new AsynUpdateUi(delegate(string s) {
                    TB_Result.Text = s;
                }), str);
            }
            else
            {
                TB_Result.Text = str;
            }
        }
        private int lastUiSummary = 1;
        private void ShowResult(Label lbl, DateTime start, int count, bool force = true)
        {
            const int UpdateIntval = 600;

            DateTime stop = System.DateTime.Now;
            TimeSpan ts = stop.Subtract(start);

            if (((int)ts.TotalSeconds / UpdateIntval) > lastUiSummary || force)
            {
                lbl.Text = String.Format(
                    "Test {0} {1} times(send {2} , receive {3}): Total {4} ms",
                    RB_PartA.Checked?"A":"B", count, port.GetSendCnt(), port.GetRecvCnt(), ts);

                Logger.Show(Logger.Level.Operation, lbl.Text);

                lastUiSummary = (int)ts.TotalSeconds / UpdateIntval;
            }
        }
        #region Stability Test
        string[] StabilityTest = new string[] {
            "SingleCurrent",
            "MultiCurrent",
        };
        private void BTN_StabilityStart_Click(object sender, EventArgs e)
        {
            if (!CB_ElecModuleEnable.Checked)
            {
                MessageBox.Show("ElectricModule should be checked");
                return;
            }

            int option = CMB_TestType.SelectedIndex;
            int duration = Int32.Parse(TB_Duration.Text);
            bool forceClose = CB_ForceClose.Checked;
            bool forceDisconnect = CB_ForceDisconnect.Checked;
            int cmdCount = 0;

            DateTime start = System.DateTime.Now;
            TB_StabilityResult.Text = StabilityTest[option] + " Start @ " + start + "(" + forceClose + "," + forceDisconnect + ")";
            Logger.Show(Logger.Level.Operation, TB_StabilityResult.Text);

            switch (option)
            {
                case 0:
                    cmdCount = SingleCurrentTest(start, duration);
                    break;
                case 1:
                    cmdCount = MultiCurrentTest(start, duration);
                    break;
            }

            ShowResult(TB_StabilityResult, start, cmdCount);
        }
        private void SaveData(StreamWriter dat, double[] current, double[] voltage)
        {
            string oneshot = "";

            oneshot += "Current: ";
            for (int i = 0; i < current.Length; i++)
            {
                oneshot += current[i].ToString("0.000") + " ";
            }

            oneshot += "Voltage: ";
            for (int i = 0; i < voltage.Length; i++)
            {
                oneshot += voltage[i].ToString("0.000") + " ";
            }

            dat.Write(oneshot + "\n");
            dat.Flush();
        }
        private int SingleCurrentTest(DateTime start, int duration, int interval = 100)
        {
            StreamWriter dat = new StreamWriter(new FileStream("SingleCurrentTest.dat", FileMode.Append));

            JzhPower jzh = port as JzhPower;
            double[] current;
            double[] voltage;
            int count = 0;

            jzh.SetCurrent(4);
            count++;

            TimeSpan ts = System.DateTime.Now.Subtract(start);
            while ((int)ts.TotalMinutes < duration)
            {
                if (CB_ForceClose.Checked)
                {
                    jzh.Open();
                    jzh.ReadVoltageAndCurrent(out current, out voltage);
                    jzh.Close();
                }
                else
                {
                    jzh.ReadVoltageAndCurrent(out current, out voltage);
                }
                SaveData(dat, current, voltage);
                count++;
                Thread.Sleep(interval);

                ts = System.DateTime.Now.Subtract(start);
            }

            return count;
        }
        private int MultiCurrentTest(DateTime start, int duration, int interval = 100)
        {
            StreamWriter dat = new StreamWriter(new FileStream("MultiCurrentTest.dat", FileMode.Append));

            JzhPower jzh = port as JzhPower;
            double[] testPoint = new double[4] { 1, 2, 3, 4 };
            double[] current;
            double[] voltage;
            int count = 0;

            for (int i = 0; i < testPoint.Length; i++)
            {
                jzh.SetCurrent(testPoint[i]);
                count++;

                TimeSpan ts = System.DateTime.Now.Subtract(start);
                while ((int)ts.TotalMinutes < (int)(duration / testPoint.Length))
                {
                    if (CB_ForceClose.Checked)
                    {
                        jzh.Open();
                        jzh.ReadVoltageAndCurrent(out current, out voltage);
                        jzh.Close();
                    }
                    else
                    {
                        jzh.ReadVoltageAndCurrent(out current, out voltage);
                    }
                    SaveData(dat, current, voltage);
                    count++;
                    Thread.Sleep(interval);

                    ts = System.DateTime.Now.Subtract(start);
                }
            }

            return count;
        }
        #endregion
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
        public static byte[] Frame(byte[] cmd)
        {
        #if jzh
            return cmd;
        #else
            const int USB_BUFF = 511;              /* depends on remote device */
            List<byte[]> frames = new List<byte[]>();

            int totalLen = cmd.Length + 2;/*add frame length field*/
            int i = 0;
            int fragmentLen = 0;          /* length of each frame */
            int loadLen = 0;              /* total length of loaded bytes*/

            fragmentLen = totalLen > USB_BUFF ? USB_BUFF : totalLen;
            byte[] frame = new byte[fragmentLen];
            frame[0] = Util.HighByte(cmd.Length + 2);
            frame[1] = Util.LowByte(cmd.Length + 2);
            i = 0;
            while (i < fragmentLen - 2)
            {
                frame[2 + i] = cmd[i];
                i++;
            }
            frames.Add(frame);
            loadLen += fragmentLen;

            while (loadLen < totalLen)
            {
                fragmentLen = (totalLen - loadLen) > USB_BUFF ? USB_BUFF : totalLen - loadLen;
                byte[] frame2 = new byte[fragmentLen];
                i = 0;
                while (i < fragmentLen)
                {
                    frame2[i] = cmd[loadLen - 2 + i];
                    i++;
                }
                frames.Add(frame2);
                loadLen += fragmentLen;
            }

            return frames[0];
        #endif
        }
        class SendFile
        {
            public delegate void UpdateUi(string str);
            public UpdateUi updateUi;

            public string mFile;
            public int mTimes;
            public string mMode;

            private Iport mPort;
            public SendFile(Iport port, string file, int times, string mode)
            {
                mPort = port;
                mFile = file;
                mTimes = times;
                mMode = mode;
            }

            const int UpdateIntval = 3600;
            private int lastUiSummary = 1;
            private bool ShouldUpdate(DateTime cur, DateTime start)
            {
                TimeSpan ts = cur.Subtract(start);

                if (((int)ts.TotalSeconds / UpdateIntval) > lastUiSummary)
                {
                    lastUiSummary = (int)ts.TotalSeconds / UpdateIntval;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            private bool ShouldContinue(int count, DateTime cur, DateTime start)
            {
                if (mMode.Equals("Times"))
                {
                    return (count < mTimes);
                }
                else
                {
                    return ((int)cur.Subtract(start).TotalMinutes < mTimes);
                }
            }
            private string Summary(DateTime start, DateTime end, int count)
            {
                return String.Format("Send File {0} ms : {1} times(send {2},receive{3})",
                    end.Subtract(start).TotalSeconds, count, mPort.GetSendCnt(), mPort.GetRecvCnt());
            }
            public void Run()
            {
                lastUiSummary = 1;

                int count = 0;
                DateTime start = System.DateTime.Now;
                DateTime cur = System.DateTime.Now;

                StreamReader sr = new StreamReader(mFile, Encoding.Default);

                while (ShouldContinue(count, cur, start))
                {
                    SendFileOnce(sr);
                    count++;

                    cur = System.DateTime.Now;
                    if (ShouldUpdate(cur, start))
                    {
                        updateUi(Summary(start, cur, count));
                    }
                }

                updateUi(Summary(start, cur, count));
            }
            private void SendFileOnce(StreamReader sr)
            {
                sr.BaseStream.Seek(0, SeekOrigin.Begin);

                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Substring(0, 2).Equals("//"))
                    {
                        String delay = line.Split(' ')[1];
                        Thread.Sleep(Int32.Parse(delay));
                    }
                    else
                    {
                        mPort.Query(Frame(_HexStringToBytes(line.Replace(" ", ""))));
                    }
                }
            }
        }
        class Util
        {
            static public byte HighByte(int value)
            {
                return (byte)((value & 0xff00) >> 8);
            }

            static public byte LowByte(int value)
            {
                return (byte)((value & 0x00ff));
            }
        }

    }
}
