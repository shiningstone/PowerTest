using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace BIModel
{
    public class Comm
    {
        private static Dictionary<string, object> lockDict = new Dictionary<string, object>();
        private readonly SerialPort com = null;
        public Comm(string portName, int baudrate=19200, StopBits stopBits=StopBits.One, Parity parity=Parity.None)
        {
            this.com = new SerialPort();
            this.com.BaudRate = baudrate;
            this.com.StopBits = stopBits;
            this.com.DataBits = 8;
            this.com.Parity = parity;
            this.com.PortName = portName;
            this.com.ReadTimeout = 5000;
            if (false == lockDict.ContainsKey(com.PortName))
                lockDict[com.PortName] = new object();
        }
        public void Open()
        {
            this.com.Open();
        }
        public void Close()
        {
            this.com.Close();
        }
        public bool IsOpen()
        {
            return this.com.IsOpen;
        }
        public byte[] Query(byte[] cmd, int sleep=0)
        {
            lock (lockDict[com.PortName])
            {
                string errMessage = "";
                for (int k = 0; k < 3; k++)
                {
                    try
                    {
                        if (this.com.IsOpen == false)
                        {
                            this.com.Open();
                        }
                        bool receive = false;
                        this.com.DataReceived += (sender, e) => { receive = true; };
                        this.com.Write(cmd, 0, cmd.Length);
                        Logger.Show(Logger.Level.Command, cmd);

                        int cnt = 0;
                        while (!receive)
                        {
                            System.Threading.Thread.Sleep(20);
                            cnt += 20;
                            if (cnt > 5000)
                            {
                                throw new Exception("Response Timeout 5s.");
                            }
                        }
                        System.Threading.Thread.Sleep(sleep);
                        byte[] recvBuffer = new byte[0x200];
                        int recvCount = this.com.Read(recvBuffer, 0, recvBuffer.Length);
                        byte[] ret = new byte[recvCount];
                        Array.Copy(recvBuffer, ret, ret.Length);
                        Logger.Show(Logger.Level.Command, ret);

                        return ret;
                    }
                    catch (Exception ex)
                    {
                        errMessage = ex.Message;
                    }
                    finally
                    {
                        this.com.Close();
                    }
                }
                throw new Exception(this.com.PortName + " error:" + errMessage);
            }
        }
    }
}
