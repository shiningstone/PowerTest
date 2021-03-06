﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIModel;

namespace APPLEDIE
{
    public class ElectricModule
    {
        private static Dictionary<string, Dictionary<string, byte[]>> cmdDictionary;
        static ElectricModule()
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

        private Comm com = null;
        private string part = null;
        public ElectricModule(int floor, int num)
        {
#if false
            var info = ConfigReader.GetItem("MAP_ELECTRIC_F" + floor + "N" + num).Split(',');
            com = new Comm(info[0], 38400);
            part = info[1];
#endif
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

                var cmdSet = new byte[][] { cmdDictionary[part]["SetCurrent00to19"], cmdDictionary[part]["SetCurrent20to39"] };
                byte[] currentPara = new byte[] { 0x00, 0x00, (byte)(current * 10 / 256), (byte)(current * 10 % 256) };
                foreach (var item in cmdSet)
                {
                    List<byte> cmd = item.ToList();
                    for (int channel = 0; channel < 20; channel++)
                        cmd.InsertRange(cmd.Count - 5, currentPara);
                    com.Query(cmd.ToArray(), 100);
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
                var response = com.Query(cmd, 3000);
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
            var response = com.Query(cmd, 100);
            if (9 == response.Length)
                return;
            throw new Exception("Electirc Module Remote Control Response Error.");
        }

        private void SetUpVoltage(byte[] cmd)
        {
            try
            {
                var response = com.Query(cmd, 200);
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
            var response = com.Query(cmd, 100);
            if (9 == response.Length)
                return;
            throw new Exception("Electirc Module Disable Control Response Error.");
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
                    cmdDictionary[part]["SetCurPart20to39"] = new byte[] { 0x68, 0x01, 0x01, 0x5D, 0x13, 0x40, 0x00, 0x00, strToToHexByte(hexlocstr[2])[0], strToToHexByte(hexlocstr[1])[0], strToToHexByte(hexlocstr[0])[0] };
                    cmdDictionary[part]["SetCurPart00to19"] = new byte[] { 0x68, 0x01, 0x01, 0x5D, 0x13, 0x40, 0x00, 0x01, strToToHexByte(hexlocstr[5])[0], strToToHexByte(hexlocstr[4])[0], strToToHexByte(hexlocstr[3])[0] };
                }
                else
                {
                    cmdDictionary[part]["SetCurPart20to39"] = new byte[] { 0x68, 0x01, 0x01, 0x5D, 0x13, 0x40, 0x00, 0x02, strToToHexByte(hexlocstr[2])[0], strToToHexByte(hexlocstr[1])[0], strToToHexByte(hexlocstr[0])[0] };
                    cmdDictionary[part]["SetCurPart00to19"] = new byte[] { 0x68, 0x01, 0x01, 0x5D, 0x13, 0x40, 0x00, 0x03, strToToHexByte(hexlocstr[5])[0], strToToHexByte(hexlocstr[4])[0], strToToHexByte(hexlocstr[3])[0] };
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
