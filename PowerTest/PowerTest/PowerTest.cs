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
            byte[] cmd = _HexStringToBytes(TB_Cmd.Text.Replace(" ", ""));
            byte[] rsp = port.Query(cmd);

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
            while (count < times)
            {
                StreamReader sr = new StreamReader(file, Encoding.Default);
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    Logger.Show(Logger.Level.Command, line.ToString());
                }

                count++;
            }
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
    }
}
