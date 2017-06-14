using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIModel
{
    class Logger
    {
        public delegate void logAction(string log);
        public static logAction action;
        public enum Level
        {
            Operation,     /*设备操作*/
            Bus,           /*总线、寄存器操作*/
            Command,       /*USB报文内容*/
        }

        static public Level mLevel = Level.Command;
#if DEBUG
        static public StreamWriter mLogFile = null;
#else
        static public StreamWriter mLogFile = new StreamWriter(new FileStream("TestBoard.log", FileMode.Append));
#endif
        public static void Show(Level level, string buf)
        {
            if (level <= mLevel)
            {
                string log = DateTime.Now.ToString(" yyyy-MM-dd HH:mm:ss ffff") + "---"
                    + "[" + level.ToString() + "] "
                    + buf;

                if (mLogFile == null)
                {
                    Console.WriteLine(log);
                }
                else
                {
                    mLogFile.Write(log + "\n");
                    mLogFile.Flush();
                }

                if (action!=null)
                {
                    action.Invoke(log);
                }
            }
        }
        public static void Show(Level level, byte[] bytes)
        {
            Show(level, ToHexString(bytes));
        }

        public static void SetFile(string path)
        {
            if (path != null)
            {
                mLogFile = new StreamWriter(new FileStream(path, FileMode.Append));
            }
        }
        public static string ToHexString(byte[] bytes)
        {
            string hexString = string.Empty;

            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                    strB.Append(" ");
                }

                hexString = strB.ToString();
            }
            return hexString;
        }
    }
}
