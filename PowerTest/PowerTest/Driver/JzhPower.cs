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
                    ["ReadParameters"] = new byte[] { 0x68, 0x01, 0x01, 0x08, 0x10, 0x30, 0x00, 0x28 },
                    ["SetVol00t1935"] =
                        new byte[] { 0x68, 0x01, 0x01, 0x10, 0x13, 0x40, 0x03, 0x00, 0x23, 0x63 },
                    ["SetVol20t3935"] =
                        new byte[] { 0x68, 0x01, 0x01, 0x10, 0x13, 0x40, 0x03, 0x01, 0x23, 0x62 },
                    ["SetVol00t1940"] =
                        new byte[] { 0x68, 0x01, 0x01, 0x10, 0x13, 0x40, 0x03, 0x00, 0x28, 0x68 },
                    ["SetVol20t3940"] =
                        new byte[] { 0x68, 0x01, 0x01, 0x10, 0x13, 0x40, 0x03, 0x01, 0x28, 0x69 },
                    ["SetVol00t1945"] =
                        new byte[] { 0x68, 0x01, 0x01, 0x10, 0x13, 0x40, 0x03, 0x00, 0x2D, 0x6D },
                    ["SetVol20t3945"] =
                        new byte[] { 0x68, 0x01, 0x01, 0x10, 0x13, 0x40, 0x03, 0x01, 0x2D, 0x6C },
                    ["SetVol00t1950"] =
                        new byte[] { 0x68, 0x01, 0x01, 0x10, 0x13, 0x40, 0x03, 0x00, 0x32, 0x72 },
                    ["SetVol20t3950"] =
                        new byte[] { 0x68, 0x01, 0x01, 0x10, 0x13, 0x40, 0x03, 0x01, 0x32, 0x73 }
                },
                ["B"] = new Dictionary<string, byte[]>
                {
                    ["SetCurrent00to19"] =
                        new byte[] { 0x68, 0x01, 0x01, 0x5D, 0x13, 0x40, 0x00, 0x02, 0xFF, 0xFF, 0xFF, 0xFF, 0x0C },
                    ["SetCurrent20to39"] =
                        new byte[] { 0x68, 0x01, 0x01, 0x5D, 0x13, 0x40, 0x00, 0x03, 0xFF, 0xFF, 0xFF, 0xFF, 0x0D },
                    ["ReadParameters"] = new byte[] { 0x68, 0x01, 0x01, 0x08, 0x10, 0x30, 0x01, 0x29 },
                    ["SetVol00t1935"] =
                        new byte[] { 0x68, 0x01, 0x01, 0x10, 0x13, 0x40, 0x03, 0x02, 0x23, 0x61 },
                    ["SetVol20t3935"] =
                        new byte[] { 0x68, 0x01, 0x01, 0x10, 0x13, 0x40, 0x03, 0x03, 0x23, 0x60 },
                    ["SetVol00t1940"] =
                        new byte[] { 0x68, 0x01, 0x01, 0x10, 0x13, 0x40, 0x03, 0x02, 0x28, 0x6A },
                    ["SetVol20t3940"] =
                        new byte[] { 0x68, 0x01, 0x01, 0x10, 0x13, 0x40, 0x03, 0x03, 0x28, 0x6B },
                    ["SetVol00t1945"] =
                        new byte[] { 0x68, 0x01, 0x01, 0x10, 0x13, 0x40, 0x03, 0x02, 0x2D, 0x6F },
                    ["SetVol20t3945"] =
                        new byte[] { 0x68, 0x01, 0x01, 0x10, 0x13, 0x40, 0x03, 0x03, 0x2D, 0x6E },
                    ["SetVol00t1950"] =
                        new byte[] { 0x68, 0x01, 0x01, 0x10, 0x13, 0x40, 0x03, 0x02, 0x32, 0x70 },
                    ["SetVol20t3950"] =
                        new byte[] { 0x68, 0x01, 0x01, 0x10, 0x13, 0x40, 0x03, 0x03, 0x32, 0x71 }
                }
            };

        }

        protected Comm com = null;
        public string part = null;
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
        public JzhPower(Comm comm, string p = "A")
        {
            com = comm;
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
                if (cmd[5]==0x40 && cmd[6]==0x0f)      /*get firmware version*/
                {
                    return 0x0E;
                }
                else
                {
                    return 328;
                }
            }
            else if (cmd[4] == (byte)JzhOp.Write)
            {
                if (cmd[3] == 9)                        /*connect & disconnect*/
                {
                    return 9;
                }
                else
                {
                    return 9;
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
        public string GetFwVer()
        {
            byte[] cmd = { 0x68, 0x01, 0x01, 0x08, 0x10, 0x40, 0x0F, 0x57 };
            /*68 01 01 0e 90 40 0f 20 17 06 20 00 22 e2    Fw ver: date:20170620 Major: 0 Minor: 0x22*/

            byte[] rsp = Query(cmd);

            byte[] major = new byte[] { rsp[11] };
            byte[] minor = new byte[] { rsp[12] };
            byte[] date = new byte[4];
            Array.Copy(rsp, 7, date, 0, date.Length);

            return String.Format("{0}-{1}.{2}", Util.BytesToHexString(date), Util.BytesToHexString(major), Util.BytesToHexString(minor));
        }
        public void SetCurrent(double current)
        {
            try
            {
                this.EnableControl();
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

                Logger.Show(Logger.Level.Bus, String.Format("SetCurrent: {0}", current));
                var cmdSet = new byte[][] { cmdDictionary[part]["SetCurrent00to19"], cmdDictionary[part]["SetCurrent20to39"] };
                byte[] currentPara = new byte[] { 0x00, 0x00, (byte)(current * 10 / 256), (byte)(current * 10 % 256) };
                foreach (var item in cmdSet)
                {
                    List<byte> cmd = item.ToList();
                    for (int channel = 0; channel < 20; channel++)
                        cmd.InsertRange(cmd.Count - 5, currentPara);
                    Query(cmd.ToArray(), 100);
                }

                if (part.Equals("A"))
                {
                    for (int i = 0; i < 40; i++)
                    {
                        mCurrents[i] = current;
                    }
                }
                else
                {
                    for (int i = 40; i < 80; i++)
                    {
                        mCurrents[i] = current;
                    }
                }
            }
            finally
            {
                this.DisableControl();
            }
        }

        public void ReadVoltageAndCurrent(out double[] current, out double[] voltage)
        {
            Logger.Show(Logger.Level.Bus, String.Format("ReadVoltageAndCurrent"));

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
#if false
            Logger.Show(Logger.Level.Bus, String.Format("EnableControl"));

            var cmd = new byte[] { 0x68, 0x01, 0x01, 0x09, 0x13, 0x40, 0x0C, 0x01, 0x57 };
            var response = Query(cmd, 100);
            if (9 == response.Length)
                return;
            throw new Exception("Electirc Module Remote Control Response Error.");
#endif
        }
        private void SetUpVoltage(byte[] cmd)
        {
            Logger.Show(Logger.Level.Bus, String.Format("SetUpVoltage"));

            try
            {
                cmd[3] = 0x0a;
                byte sc = GetCheckSum(cmd, cmd.Length - 1);
                cmd[cmd.Length - 1] = sc;

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
#if false
            Logger.Show(Logger.Level.Bus, String.Format("DisableControl"));

            var cmd = new byte[] { 0x68, 0x01, 0x01, 0x09, 0x13, 0x40, 0x0C, 0x02, 0x54 };
            var response = Query(cmd, 100);
            if (9 == response.Length)
                return;
            throw new Exception("Electirc Module Disable Control Response Error.");
#endif
        }

        protected double[] mCurrents = new double[80];

        public void SetCurrentChannels(double current, string[] location)
        {
            for (int group = 0; group < 2; group++)
            {
                /*command*/
                byte[] header = new byte[] { 0x68, 0x01, 0x01, 0x5D, 0x13, 0x40, 0x00 };
                List<byte> cmd = header.ToList();

                /*group*/
                int partIdx = part.Equals("A") ? 0 : 1;
                cmd.Add((byte)(partIdx*2 + group));

                /*currents*/
                Util.BitsArray map = new Util.BitsArray();
                byte[] data = new byte[] { 0x00, 0x00, (byte)(current * 10 / 256), (byte)(current * 10 % 256) };
                for (int chnl = 0; chnl < 20; chnl++)
                {
                    if (location[group * 20 + chnl].Equals("1"))
                    {
                        cmd.InsertRange(cmd.Count, data);
                        mCurrents[partIdx * 40 + group * 20 + chnl] = current;
                        map.Set(chnl);
                    }
                    else
                    {
                        byte[] prevSetting = new byte[] { 0x00, 0x00,
                            (byte)(mCurrents[partIdx * 40 + group * 20 + chnl] * 10 / 256),
                            (byte)(mCurrents[partIdx * 40 + group * 20 + chnl] * 10 % 256)
                        };
                        cmd.InsertRange(cmd.Count, prevSetting);
                        if (mCurrents[partIdx * 40 + group * 20 + chnl] > 0)
                        {
                            map.Set(chnl);
                        }
                    }
                }

                /*flags*/
                cmd.InsertRange(cmd.Count, map.mMap);

                /*cs*/
                cmd.Add(GetCheckSum(cmd.ToArray(), cmd.Count));

                /*Send*/
                Query(cmd.ToArray(), 100);
            }
        }
        public void SetCurPart(double current, string[] location)
        {
            string[] hexlocstr = new string[6];
            hexlocstr = this.GetLocationByte(location);
            try
            {
                this.EnableControl();
                if (part == "A")
                {
                    cmdDictionary[part]["SetCurPart20to39"] = new byte[] { 0x68, 0x01, 0x01, 0x5D, 0x13, 0x40, 0x00, 0x00, /*FLAG*/0x00, strToToHexByte(hexlocstr[2])[0], strToToHexByte(hexlocstr[1])[0], strToToHexByte(hexlocstr[0])[0] };
                    cmdDictionary[part]["SetCurPart00to19"] = new byte[] { 0x68, 0x01, 0x01, 0x5D, 0x13, 0x40, 0x00, 0x01, /*FLAG*/0x00, strToToHexByte(hexlocstr[5])[0], strToToHexByte(hexlocstr[4])[0], strToToHexByte(hexlocstr[3])[0] };
                }
                else
                {
                    cmdDictionary[part]["SetCurPart20to39"] = new byte[] { 0x68, 0x01, 0x01, 0x5D, 0x13, 0x40, 0x00, 0x02, /*FLAG*/0x00, strToToHexByte(hexlocstr[2])[0], strToToHexByte(hexlocstr[1])[0], strToToHexByte(hexlocstr[0])[0] };
                    cmdDictionary[part]["SetCurPart00to19"] = new byte[] { 0x68, 0x01, 0x01, 0x5D, 0x13, 0x40, 0x00, 0x03, /*FLAG*/0x00, strToToHexByte(hexlocstr[5])[0], strToToHexByte(hexlocstr[4])[0], strToToHexByte(hexlocstr[3])[0] };
                }
                var cmdSet = new byte[][] { cmdDictionary[part]["SetCurPart20to39"], cmdDictionary[part]["SetCurPart00to19"] };
                byte[] currentPara = new byte[] { 0x00, 0x00, (byte)(current * 10 / 256), (byte)(current * 10 % 256) };
                byte[] tcurr = new byte[] { 0x00, 0x00, (byte)(0 * 10 / 256), (byte)(0 * 10 % 256) };
                int count = 0;
                foreach (var item in cmdSet)
                {
                    List<byte> cmd = item.ToList();
                    byte chsum;
                    //Add the location
                    for (int channel = 0; channel < 20; channel++)
                    {
                        if (location[channel + count * 20] == "1")
                            cmd.InsertRange(cmd.Count - 4, currentPara);
                        else
                            cmd.InsertRange(cmd.Count - 4, tcurr);
                    }
                    count = 1;
                    chsum = GetCheckSum(cmd.ToArray(), cmd.Count);
                    cmd.Add(chsum);
                    com.Query(cmd.ToArray(), 100);
                }
            }
            finally
            {
                this.DisableControl();
            }
        }
        public static byte GetCheckSum(byte[] data, int length)
        {
            byte temp = 0;
            for (int i = 1; i < length; i++)
            {
                temp ^= data[i];
            }
            return temp;
        }
        private byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        private string[] GetLocationByte(string[] loction)
        {
            string[] hexlocstr = new string[6];
            string tempstr = "";
            for (int i = 0; i < 40; i++)
                tempstr = tempstr + loction[i].ToString();
            hexlocstr[0] = tempstr.Substring(0, 8);
            hexlocstr[1] = tempstr.Substring(7, 8);
            hexlocstr[2] = tempstr.Substring(15, 4);
            hexlocstr[3] = tempstr.Substring(19, 8);
            hexlocstr[4] = tempstr.Substring(27, 8);
            hexlocstr[5] = tempstr.Substring(35, 4);
            for (int j = 0; j < 6; j++)
            {
                hexlocstr[j] = string.Format("{0:x}", Convert.ToInt32(hexlocstr[j], 2)); ;
            }
            return hexlocstr;
        }
    }
}
