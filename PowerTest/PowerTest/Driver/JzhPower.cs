using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIModel;

namespace APPLEDIE
{
    public class JzhPower : Iport
    {
        public static string QueryFail = "JzhPower Query fail ";
        public static string RspError = "Jzh Response error ";

        private static Dictionary<string, Dictionary<string, byte[]>> cmdDictionary;
        protected int SendCnt = 0;
        protected int RecvCnt = 0;

        public enum RspValid
        {
            Ok,
            IdError,
            LengthError,
            NotAccept,
            CrcError,
        }

        static JzhPower()
        {
            cmdDictionary = new Dictionary<string, Dictionary<string, byte[]>>
            {
                ["A"] = new Dictionary<string, byte[]>
                {
                    ["SetCurrent00to19"] =
                        new byte[] { 0x68, 0x01, 0x01, 0x5D, 0x13, 0x40, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x0E },
                    ["SetCurrent20to39"] =
                        new byte[] { 0x68, 0x01, 0x01, 0x5D, 0x13, 0x40, 0x00, 0x01, 0xFF, 0xFF, 0xFF, 0xFF, 0x0F },
                    ["ReadParameters"] = new byte[] { 0x68, 0x01, 0x01, 0x08, 0x10, 0x30, 0x00, 0x28 }
                },
                ["B"] = new Dictionary<string, byte[]>
                {
                    ["SetCurrent00to19"] =
                        new byte[] { 0x68, 0x01, 0x01, 0x5D, 0x13, 0x40, 0x00, 0x02, 0xFF, 0xFF, 0xFF, 0xFF, 0x0C },
                    ["SetCurrent20to39"] =
                        new byte[] { 0x68, 0x01, 0x01, 0x5D, 0x13, 0x40, 0x00, 0x03, 0xFF, 0xFF, 0xFF, 0xFF, 0x0D },
                    ["ReadParameters"] = new byte[] { 0x68, 0x01, 0x01, 0x08, 0x10, 0x30, 0x01, 0x29 }
                }
            };

        }

        protected Comm com = null;
        protected string part = null;
        public JzhPower()
        { }
        public JzhPower(int floor, int num)
        {
#if false
            var info = ConfigReader.GetItem("MAP_ELECTRIC_F" + floor + "N" + num).Split(',');
            com =new Comm(info[0],38400);
            part = info[1];
#endif
        }
        public JzhPower(string comNum, string p = "A")
        {
            com = new Comm(comNum, 38400);
            part = p;
        }

        #region Iport
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
            return this.com.IsOpen();
        }
        public int GetSendCnt()
        {
            return SendCnt;
        }
        public int GetRecvCnt()
        {
            return RecvCnt;
        }
        #endregion
        #region jzh frame defination
        public enum JzhOp
        {
            Read = 0x10,
            Write = 0x13,
        }
        public enum JzhRet
        {
            Accept = 0x4b,
            Unaccept = 0x45,
        }

        const int IgnoreLengthCheck = -1;

        private bool IsIdValid(byte[] cmd, byte[] rsp)
        {
            return rsp[4] == cmd[4] + 0x80;
        }
        private int ExpectLen(byte[] cmd)
        {
            if (cmd[4] == (byte)JzhOp.Read)
            {
                return 328;
            }
            else if (cmd[4] == (byte)JzhOp.Write)
            {
                if (cmd[3] == 9)                        /*connect & disconnect*/
                {
                    return 9;
                }
                else
                {
                    return 8;
                }
            }
            else
            {
                return IgnoreLengthCheck;
            }

        }
        private byte XOR(byte[] bytes)
        {
            byte result = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                result = (byte)(result ^ bytes[i]);
            }
            return result;
        }
        private byte[] RspData(byte[] rsp)
        {
            byte[] data = new byte[rsp.Length - 2];
            Array.Copy(rsp, 1, data, 0, rsp.Length - 2);
            return data;
        }
        private RspValid IsValid(byte[] cmd, byte[] rsp)
        {
            if (!IsIdValid(cmd, rsp))
            {
                return RspValid.IdError;
            }
            else if (ExpectLen(cmd) != IgnoreLengthCheck && (rsp.Length != ExpectLen(cmd)))
            {
                return RspValid.LengthError;
            }
            else if (rsp[rsp.Length - 1] != XOR(RspData(rsp)))
            {
                return RspValid.CrcError;
            }
            else if ((cmd[4] == (byte)JzhOp.Write) && (cmd[6] == (byte)JzhRet.Unaccept))
            {
                return RspValid.NotAccept;
            }
            else
            {
                return RspValid.Ok;
            }
        }
        #endregion
        public byte[] Query(byte[] cmd, int sleep = 0)
        {
            const int MAX_TRY = 3;
            int times = 0;

            while (times < MAX_TRY)
            {
                try
                {
                    SendCnt++;

                    byte[] rsp = com.Query(cmd, sleep);
                    RspValid flag = IsValid(cmd, rsp);
                    if (flag == RspValid.Ok)
                    {
                        RecvCnt++;
                        return rsp;
                    }
                    else
                    {
                        Logger.Show(Logger.Level.Warning, RspError + flag.ToString());
                        throw new Exception(RspError + flag.ToString());
                    }
                }
                catch (Exception e)
                {
                    times++;

                    if (times < MAX_TRY - 1)
                    {
                        Logger.Show(Logger.Level.Warning, QueryFail + "," + e.Message + ", retry ...");
                    }
                    else
                    {
                        Logger.Show(Logger.Level.Error, QueryFail + "," + e.Message + ", abort");
                        throw new Exception(QueryFail + e.Message);
                    }
                }
            }

            return new byte[0];
        }
        public void SetCurrent(double current)
        {
            if (current < 4001)
            {
                this.SetUpVoltage(cmdDictionary["A"]["SetVol00t1940"]);
                this.SetUpVoltage(cmdDictionary["A"]["SetVol20t3940"]);
                this.SetUpVoltage(cmdDictionary["B"]["SetVol00t1940"]);
                this.SetUpVoltage(cmdDictionary["B"]["SetVol20t3940"]);
            }
            else //if ((current > 4000) && (current < 4501))
            {
                this.SetUpVoltage(cmdDictionary["A"]["SetVol00t1945"]);
                this.SetUpVoltage(cmdDictionary["A"]["SetVol20t3945"]);
                this.SetUpVoltage(cmdDictionary["B"]["SetVol00t1945"]);
                this.SetUpVoltage(cmdDictionary["B"]["SetVol20t3945"]);
            }
            //else
            //{
            //    this.SetUpVoltage(cmdDictionary["A"]["SetVol00t1950"]);
            //    this.SetUpVoltage(cmdDictionary["A"]["SetVol20t3950"]);
            //    this.SetUpVoltage(cmdDictionary["B"]["SetVol00t1950"]);
            //    this.SetUpVoltage(cmdDictionary["B"]["SetVol20t3950"]);
            //}

            try
            {
                this.EnableControl();
                var cmdSet = new byte[][] { cmdDictionary[part]["SetCurrent00to19"], cmdDictionary[part]["SetCurrent20to39"] };
                byte[] currentPara = new byte[] { 0x00, 0x00, (byte)(current * 10 / 256), (byte)(current * 10 % 256) };
                foreach (var item in cmdSet)
                {
                    List<byte> cmd = item.ToList();
                    for (int channel = 0; channel < 20; channel++)
                        cmd.InsertRange(cmd.Count - 5, currentPara);
                    Query(cmd.ToArray(), 100);
                }
            }
            finally
            {
                this.DisableControl();
            }
        }

        public void ReadVoltageAndCurrent(out double[] current, out double[] voltage)
        {
            try
            {
                this.EnableControl();
                byte[] cmd = cmdDictionary[part]["ReadParameters"].ToArray();
                var response = Query(cmd, 3000);
                if (328 == response.Length)
                {
                    var data = response.ToList();
                    data.RemoveRange(0, 7);
                    current = new double[40];
                    voltage = new double[40];
                    for (int channel = 0; channel < 40; channel++)
                    {
                        voltage[channel] = (data[channel * 8 + 0] * 0xFFFFFF + data[channel * 8 + 1] * 0xFFFF + data[channel * 8 + 2] * 0xFF + data[channel * 8 + 3]) / 1000.00;
                        current[channel] = (data[channel * 8 + 4] * 0xFFFFFF + data[channel * 8 + 5] * 0xFFFF + data[channel * 8 + 6] * 0xFF + data[channel * 8 + 7]) / 1000.00;
                    }
                    return;
                }
                throw new Exception("Read Voltage And Current Response Error:" + response.Length);

            }
            finally
            {
                this.DisableControl();
            }
        }

        private void EnableControl()
        {
            var cmd = new byte[] { 0x68, 0x01, 0x01, 0x09, 0x13, 0x40, 0x0C, 0x01, 0x57 };
            var response = Query(cmd, 100);
            if (9 == response.Length)
                return;
            throw new Exception("Electirc Module Remote Control Response Error.");
        }
        private void SetUpVoltage(byte[] cmd)
        {
            try
            {
                var response = Query(cmd, 200);
                //if (9 == response.Length)
                //return;
                //throw new Exception("Electirc Set voltage Response Error.");
            }
            catch
            { }
        }
        private void DisableControl()
        {
            var cmd = new byte[] { 0x68, 0x01, 0x01, 0x09, 0x13, 0x40, 0x0C, 0x02, 0x54 };
            var response = Query(cmd, 100);
            if (9 == response.Length)
                return;
            throw new Exception("Electirc Module Disable Control Response Error.");
        }
    }
}
