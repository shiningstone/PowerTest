using System;
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
                        new byte[] {0x68, 0x01, 0x01, 0x5D, 0x13, 0x40, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x0E},
                    ["SetCurrent20to39"] =
                        new byte[] {0x68, 0x01, 0x01, 0x5D, 0x13, 0x40, 0x00, 0x01, 0xFF, 0xFF, 0xFF, 0xFF, 0x0F},
                    ["ReadParameters"] = new byte[] {0x68, 0x01, 0x01, 0x08, 0x10, 0x30, 0x00, 0x28}
                },
                ["B"] = new Dictionary<string, byte[]>
                {
                    ["SetCurrent00to19"] =
                        new byte[] {0x68, 0x01, 0x01, 0x5D, 0x13, 0x40, 0x00, 0x02, 0xFF, 0xFF, 0xFF, 0xFF, 0x0C},
                    ["SetCurrent20to39"] =
                        new byte[] {0x68, 0x01, 0x01, 0x5D, 0x13, 0x40, 0x00, 0x03, 0xFF, 0xFF, 0xFF, 0xFF, 0x0D},
                    ["ReadParameters"] = new byte[] {0x68, 0x01, 0x01, 0x08, 0x10, 0x30, 0x01, 0x29}
                }
            };

        }

        private Comm com = null;
        private string part = null;
        public ElectricModule(int floor, int num)
        {
#if false
            var info = ConfigReader.GetItem("MAP_ELECTRIC_F" + floor + "N" + num).Split(',');
            com =new Comm(info[0],38400);
            part = info[1];
#endif
        }
        public ElectricModule(string comNum)
        {
            com = new Comm(comNum, 38400);
        }

        private byte[] Query(byte[] cmd, int sleep=0)
        {
            return com.Query(cmd, sleep); 
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
                var cmdSet = new byte[][] {cmdDictionary[part]["SetCurrent00to19"],cmdDictionary[part]["SetCurrent20to39"] };
                byte[] currentPara = new byte[]{0x00, 0x00, (byte)(current*10/256),(byte)(current*10%256)};
                foreach (var item in cmdSet)
                {
                    List<byte> cmd = item.ToList();
                    for (int channel = 0; channel < 20; channel++)
                        cmd.InsertRange(cmd.Count-5,currentPara);
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
                    data.RemoveRange(0,7);
                    current = new double[40];
                    voltage = new double[40];
                    for (int channel = 0; channel < 40; channel++)
                    {
                        voltage[channel] = (data[channel * 8 + 0] * 0xFFFFFF + data[channel * 8 + 1] * 0xFFFF + data[channel * 8 + 2] * 0xFF + data[channel * 8 + 3]) / 1000.00;
                        current[channel] = (data[channel * 8 + 4] * 0xFFFFFF + data[channel * 8 + 5] * 0xFFFF + data[channel * 8 + 6] * 0xFF + data[channel * 8 + 7]) / 1000.00;
                    }
                    return;
                }
                throw new Exception("Read Voltage And Current Response Error:"+response.Length);

            }
            finally
            {
                this.DisableControl();
            }
        }
        
        private void EnableControl()
        {
            var cmd = new byte[] { 0x68, 0x01, 0x01, 0x09, 0x13, 0x40, 0x0C, 0x01, 0x57 };
            var response = com.Query(cmd,100);
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
            var response = com.Query(cmd,100);
            if (9 == response.Length)
                return;
            throw new Exception("Electirc Module Disable Control Response Error.");
        }
    }
}
