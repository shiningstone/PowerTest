using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLEDIE
{
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

        public class BitsArray
        {
            public byte[] mMap = new byte[4];
            public void Set(int[] bits)
            {
                for (int i = 0; i < bits.Length; i++)
                {
                    Set(bits[i]);
                }
            }
            public void Clr(int[] bits)
            {
                for (int i = 0; i < bits.Length; i++)
                {
                    Clr(bits[i]);
                }
            }
            public void Set(int bit)
            {
                if (bit < 20)
                {
                    mMap[3-bit/8] |= (byte)(1 << bit % 8);
                }
                else
                {
                    throw new Exception("BitsArray : error range " + bit.ToString() + ", should between 0-19");
                }
            }
            public void Clr(int bit)
            {
                if (bit < 20)
                {
                    mMap[3 - bit / 8] &= (byte)(~(1 << bit % 8));
                }
                else
                {
                    throw new Exception("BitsArray : error range " + bit.ToString() + ", should between 0-19");
                }
            }
        }
        public static byte[] HexStringToBytes(string hexString)
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
        public static byte[] Frame(byte[] cmd)
        {
#if true
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
    }
}
