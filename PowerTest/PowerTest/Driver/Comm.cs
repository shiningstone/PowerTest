﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace BIModel
{
    public interface Iport
    {
        void Open();
        void Close();
        bool IsOpen();
        byte[] Query(byte[] cmd, int msWait = 0);

        int GetSendCnt();
        int GetRecvCnt();
    }

    public class Comm : Iport
    {
        private static Dictionary<string, object> lockDict = new Dictionary<string, object>();

        /* Exceptions */
        const string SendError = "Comm Send Error ";
        const string RecvError = "Comm Receive Error ";
        const string RecvTimeout = "Comm Receive Timeout ";
        /* Configurations */
        const int RECV_MIN_TIME = 50;       /*设得太小会导致一个包需要多次接收；设得太大会导致时间浪费*/
        const int RECV_CHECK_INT = 1;       /*设得太小浪费CPU；设得太大会导致时间浪费*/
        const int RECV_TIMEOUT = 3000; 
        private byte[] RECV_BUF = new byte[0x200];

        private readonly SerialPort com = null;
        private bool ArriveFlag = false;
        public int SendCnt = 0;
        public int RecvCnt = 0;             /*由于接收大包有可能分成几次，RecvCnt不一定等于SendCnt*/

        public static Comm GetComm(string portName, int baudrate)
        {
            if (!portName.Equals("Mock"))
            {
                return new Comm(portName, baudrate);
            }
            else
            {
                return new CommMock(portName, baudrate);
            }
        }
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
        virtual public void Open()
        {
            this.com.Open();
            this.com.DataReceived += (sender, e) => { ArriveFlag = true; };

            SendCnt = 0;
            RecvCnt = 0;
        }
        virtual public void Close()
        {
            this.com.Close();
        }
        virtual public bool IsOpen()
        {
            return this.com.IsOpen;
        }
        virtual public int GetSendCnt()
        {
            return SendCnt;
        }
        virtual public int GetRecvCnt()
        {
            return RecvCnt;
        }
        public int Send(byte[] cmd)
        {
            Logger.Show(Logger.Level.Command, cmd);

            ArriveFlag = false;

            try
            {
                this.com.Write(cmd, 0, cmd.Length);
                SendCnt++;
            }
            catch (Exception e)
            {
                Logger.Show(Logger.Level.Error, this.com.PortName + "Send Error : " + e.ToString());
                throw new Exception(this.com.PortName + SendError);
            }

            return 0;
        }
        private bool IsReceived(int msTimeout)
        {
            int count = msTimeout / RECV_CHECK_INT;
            while (!ArriveFlag)
            {
                Thread.Sleep(RECV_CHECK_INT);
                count--;

                if ((count == 0) && (msTimeout > 0))
                {
                    return false;
                }
            }

            return true;
        }
        public byte[] Recv(int msWait, int msTimeout = RECV_TIMEOUT)
        {
            if (IsReceived(msTimeout))
            {
                Thread.Sleep(msWait);

                try
                {
                    int len = this.com.Read(RECV_BUF, 0, RECV_BUF.Length);
                    byte[] ret = new byte[len];
                    Array.Copy(RECV_BUF, ret, ret.Length);

                    RecvCnt++;
                    Logger.Show(Logger.Level.Command, ret);

                    return ret;
                }
                catch (Exception e)
                {
                    Logger.Show(Logger.Level.Error, this.com.PortName + "Recv Error : " + e.ToString());
                    throw new Exception(this.com.PortName + RecvError);
                }
            }
            else
            {
                Logger.Show(Logger.Level.Warning, this.com.PortName + " Recv timeout : " + msTimeout.ToString() + " ms");
                throw new Exception(this.com.PortName + " " + RecvTimeout);
            }
        }
        virtual public byte[] Query(byte[] cmd, int msWait=0)
        {
            string errMessage = "";

            lock (lockDict[com.PortName])
            {
                try
                {
                    if (this.com.IsOpen == false)
                    {
                        this.com.Open();
                    }

                    Send(cmd);
                    return Recv(msWait);
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    Logger.Show(Logger.Level.Error, this.com.PortName + " error:" + errMessage);
                    this.com.Close();
                    throw new Exception(this.com.PortName + " error:" + errMessage);
                }
            }
        }
    }

    public class CommMock : Comm
    {
        public CommMock(string portName, int baudrate = 19200, StopBits stopBits = StopBits.One, Parity parity = Parity.None)
            : base(portName, baudrate, stopBits, parity)
        {
        }
        public override void Open()
        {
            SendCnt = 0;
            RecvCnt = 0;
        }
        public override void Close()
        {
        }
        public override bool IsOpen()
        {
            return true;
        }
        public override byte[] Query(byte[] cmd, int msWait = 0)
        {
            Logger.Show(Logger.Level.Command, cmd);

            byte[] rsp = Respond(cmd);
            Logger.Show(Logger.Level.Command, rsp);
            return rsp;
        }

        #region JZH 
        private byte[] Respond(byte[] cmd)
        {
            byte[] rsp;

            if (cmd[4] == 0x13)
            {
                if (cmd[3] == 0x09)/*connect & disconnect*/
                {
                    rsp = new byte[] { 0x68, 0x01, 0x01, 0x09, 0x93, 0x40, 0x00, 0x4B, 0x91 };
                }
                else
                {
                    rsp = new byte[] { 0x68, 0x01, 0x01, 0x09, 0x93, 0x40, 0x00, 0x4B, 0x91 };
                }
            }
            else
            {
                if (cmd[5] == 0x40 && cmd[6] == 0x0f)
                {
                    rsp = new byte[] { 0x68,0x01,0x01,0x0e,0x90,0x40,0x0f,0x20,0x17,0x06,0x20,0x00,0x22,0xe2 };
                }
                else
                {
                    rsp = new byte[328];

                    rsp[0] = cmd[0];
                    rsp[1] = cmd[1];
                    rsp[2] = cmd[2];
                    rsp[4] = (byte)(cmd[4] + 0x80);

                    byte cs = 0;
                    for (int i = 1; i < rsp.Length - 1; i++)
                    {
                        cs = (byte)(cs ^ rsp[i]);
                    }
                    rsp[rsp.Length - 1] = cs;

                    Thread.Sleep(2000);
                }
            }

            return rsp;
        }
        #endregion
    }

}
