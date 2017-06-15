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


namespace PowerTest
{
    public partial class PowerTest : Form
    {
        enum BoardType {
            ElecModule,
            RftpTest,
        }
        BoardType gBoard = BoardType.RftpTest;

        Comm port = null;
        public PowerTest()
        {
            InitializeComponent();

            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                drpComList.Items.Add(port);
            }
            if (drpComList.Items.Count > 0)
            {
                drpComList.SelectedIndex = 0;
                BTN_ComCtrl.Enabled = true;
            }

            TB_TestFile.Text = Properties.Settings.Default.TestFile;
            TB_TestTimes.Text = Properties.Settings.Default.TestTimes;
            if (TB_Cmd.Text.Length == 0) {
                TB_Cmd.Text = Properties.Settings.Default.DefaultCmd;
            }

            if (CB_LogEnable.Checked)
            {
                Logger.action += new Logger.logAction(log => { TB_Log.Text += "\r\n" + log; });
            }
        }

        private void BTN_ComCtrl_Click(object sender, EventArgs e)
        {
            if (port == null)
            {
                port = new Comm(drpComList.SelectedItem.ToString(), 115200);
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
                Properties.Settings.Default.TestFile = sfd.FileName.ToString();
                Properties.Settings.Default.Save();
                TB_TestFile.Text = sfd.FileName;
            }
        }

        private void BTN_Start_Click(object sender, EventArgs e)
        {
            string file = TB_TestFile.Text;
            int times = Int32.Parse(TB_TestTimes.Text);
            Properties.Settings.Default.TestTimes = TB_TestTimes.Text;
            Properties.Settings.Default.Save();
            BTN_Start.Enabled = false;

            SendFile(file, times);

            BTN_Start.Enabled = true;
        }

        private void SendFile(string file, int times)
        {
            int count = 0;

            DateTime start = System.DateTime.Now;
            while (count < times)
            {
                StreamReader sr = new StreamReader(file, Encoding.Default);
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    port.Query(Frame(_HexStringToBytes(line.Replace(" ", ""))));
                }

                count++;
            }
            DateTime stop = System.DateTime.Now;
            TimeSpan ts = stop.Subtract(start);

            Logger.Show(Logger.Level.Operation, String.Format(
                "Performance Test {0} times(send {1} , receive {2}): Total {3} ms",
                count, port.SendCnt, port.RecvCnt, ts));
        }

        private void CB_LogEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (CB_LogEnable.Checked)
            {
                Logger.action += new Logger.logAction(log => { TB_Log.Text += "\r\n" + log; });
            }
            else
            {
                TB_Log.Text = "";
                Logger.action = null;
            }
        }
        /* this function is used for rftp board*/
        private byte[] Frame(byte[] cmd)
        {
            if (gBoard == BoardType.ElecModule)
            {
                return cmd;
            }
            else
            {
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
