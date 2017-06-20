
using System;
using System.IO;
using System.Text;
using System.Threading;

using APPLEDIE;
using BIModel;

abstract class LongTermTest
{
    public delegate void UpdateUi(string str);
    public UpdateUi updateUi;
    const int UpdateIntval = 5;
    private int lastUiSummary = 1;

    public delegate void TaskDone();
    public TaskDone taskDone;

    public abstract void Prepare();
    public abstract void SingleRun();

    protected Iport mPort;
    public string mMode;         /*0-count; 1-timer*/
    public int mTimes;           /*minutes when mode is 1*/
    public DateTime mStart;

    protected bool mLogFileEnable = false;
    protected LongTermTest(Iport port, string mode, int times, bool logFileEnable)
    {
        mPort = port;
        mMode = mode;
        mTimes = times;
        mLogFileEnable = logFileEnable;
    }

    private bool ShouldUpdate(DateTime cur)
    {
        TimeSpan ts = cur.Subtract(mStart);

        if (((int)ts.TotalSeconds / UpdateIntval) > lastUiSummary)
        {
            lastUiSummary = (int)ts.TotalSeconds / UpdateIntval;
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool ShouldContinue(int count, DateTime cur)
    {
        if (mMode.Equals("Times"))
        {
            return (count < mTimes);
        }
        else
        {
            return ((int)cur.Subtract(mStart).TotalMinutes < mTimes);
        }
    }
    private string Summary(DateTime start, DateTime end, int count)
    {
        return String.Format("{0} s : {1} times(send {2},receive{3})",
            end.Subtract(start).TotalSeconds, count, mPort.GetSendCnt(), mPort.GetRecvCnt());
    }
    public void Run()
    {
        lastUiSummary = 1;

        int count = 0;
        mStart = System.DateTime.Now;
        DateTime cur = System.DateTime.Now;

        Prepare();

        while (ShouldContinue(count, cur))
        {
            SingleRun();
            count++;

            cur = System.DateTime.Now;
            if (ShouldUpdate(cur))
            {
                updateUi(Summary(mStart, cur, count));
            }
        }

        updateUi(Summary(mStart, cur, count));
        if (taskDone != null)
        {
            taskDone();
        }
    }
}
class SendFile : LongTermTest
{
    private string mFile;
    private StreamReader mSr;
    public SendFile(Iport port, string file, int times, string mode, bool logFile = false)
        : base(port, mode, times, logFile)
    {
        mFile = file;
    }
    override public void Prepare()
    {
        mSr = new StreamReader(mFile, Encoding.Default);
        if (mLogFileEnable)
        {
            Logger.SetFile("SendFile_" + mStart.ToString("yyMMddHHmmss"));
        }
        else
        {
            Logger.mLogFile = null;
        }
    }
    override public void SingleRun()
    {
        mSr.BaseStream.Seek(0, SeekOrigin.Begin);

        String line;
        while ((line = mSr.ReadLine()) != null)
        {
            if (line.Substring(0, 2).Equals("//"))
            {
                String delay = line.Split(' ')[1];
                Thread.Sleep(Int32.Parse(delay));
            }
            else
            {
                mPort.Query(Util.Frame(Util.HexStringToBytes(line.Replace(" ", ""))));
            }
        }
    }
}
abstract class JzhTest : LongTermTest
{
    public static string[] GetAllTests()
    {
        return new string[] {
            "SingleCurrent",
            "MultiCurrent",
            "SetCurrentPart",
        };
    }

    protected int mInterval = 0;
    protected StreamWriter mDatFile = null;
    protected JzhPower mJzh = null;
    protected string mTestName = "";
    public JzhTest(string testname, Iport port, int duration, int interval = 1000, bool logFile = false)
        : base(port, "Minute", duration, logFile)
    {
        mTestName = testname;
        mJzh = port as JzhPower;
    }
    protected string FileName()
    {
        return mTestName + "_" + mStart.ToString("yyMMddHHmmss");
    }
    public override void Prepare()
    {
        if (mDatFile == null)
        {
            mDatFile = new StreamWriter(new FileStream(FileName() + ".dat", FileMode.Append));

            if (mLogFileEnable)
            {
                Logger.SetFile(FileName());
            }
            else
            {
                Logger.mLogFile = null;
            }
        }
    }
    private double[] prevCurrents = null;
    private double[] prevVoltages = null;
    private bool IsValidData(double[] current, double[] voltage)
    {
        if (prevCurrents == null)
        {
            prevCurrents = current;
            prevVoltages = voltage;

            return true;
        }
        else
        {
            for (int i = 0; i < current.Length; i++)
            {
                if (current[i] != prevCurrents[i])
                {
                    return false;
                }
            }
            for (int i = 0; i < voltage.Length; i++)
            {
                if (voltage[i] != prevVoltages[i])
                {
                    return false;
                }
            }
        }

        return true;
    }
    protected void SaveData(StreamWriter dat, double[] current, double[] voltage)
    {
        if (IsValidData(current, voltage))
        {

        }

        string oneshot = "Current: ";
        for (int i = 0; i < current.Length; i++)
        {
            oneshot += current[i].ToString("0.000") + " ";
        }

        oneshot += "Voltage: ";
        for (int i = 0; i < voltage.Length; i++)
        {
            oneshot += voltage[i].ToString("0.000") + " ";
        }

        dat.Write(oneshot + "\n");
        dat.Flush();
    }
}
class SingleCurrentTest : JzhTest
{
    public double mInitVal;
    public SingleCurrentTest(Iport port, double initVal, int duration, int interval = 1000, bool logFile = false)
        : base("SingleCurrentTest", port, duration, interval, logFile)
    {
        mInitVal = initVal;
    }
    public void SetDatFile(StreamWriter sw)
    {
        mDatFile = sw;
    }
    protected string mPart = "A";
    public override void Prepare()
    {
        base.Prepare();

        mJzh.part = "A";
        mPart = mJzh.part;
        mJzh.SetCurrent(mInitVal);

        mJzh.part = "B";
        mPart = mJzh.part;
        mJzh.SetCurrent(mInitVal);
    }
    public override void SingleRun()
    {
        double[] currents = new double[80];
        double[] voltages = new double[80];

        double[] current;
        double[] voltage;

        mJzh.part = "A";
        mJzh.ReadVoltageAndCurrent(out current, out voltage);
        current.CopyTo(currents, 0);
        voltage.CopyTo(voltages, 0);

        mJzh.part = "B";
        mJzh.ReadVoltageAndCurrent(out current, out voltage);
        current.CopyTo(currents, 40);
        voltage.CopyTo(voltages, 40);

        SaveData(mDatFile, currents, voltages);

        Thread.Sleep(mInterval);
    }
}
class MultiCurrentTest : JzhTest
{
    protected double[] mTestPoints;
    public MultiCurrentTest(Iport port, double[] testPoints, int duration, int interval = 1000, bool logFile = false)
        : base("MultiCurrentTest", port, duration, interval, logFile)
    {
        mTestPoints = testPoints;
    }
    public override void SingleRun()
    {
        for (int i = 0; i < mTestPoints.Length; i++)
        {
            SingleCurrentTest aPointTest = new SingleCurrentTest(mPort, mTestPoints[i], mTimes / mTestPoints.Length, mInterval);
            aPointTest.updateUi = updateUi;
            aPointTest.SetDatFile(mDatFile);
            aPointTest.Run();
        }
    }
}
class SetCurrentPartTest : JzhTest
{
    public double mInitVal;
    public SetCurrentPartTest(Iport port, double initVal, int duration, int interval = 1000, bool logFile = false)
        : base("SetCurrentPartTest", port, duration, interval, logFile)
    {
        mInitVal = initVal;
    }
    public void SetDatFile(StreamWriter sw)
    {
        mDatFile = sw;
    }
    string[] GetChnl(int chnl)
    {
        string[] str = new string[40];
        for (int i = 0; i < str.Length; i++)
        {
            if (i == chnl)
            {
                str[i] = "1";
            }
            else
            {
                str[i] = "0";
            }
        }

        return str;
    }
    public override void SingleRun()
    {
        double[] currents = new double[] { 500, 1000, 1500, 2000 };
        for (int chnl = 0; chnl < 40; chnl++)
        {
            for (int testPoint = 0; testPoint < currents.Length; testPoint++)
            {
                mJzh.SetCurrentChannels(currents[testPoint], GetChnl(chnl));
            }
        }
    }
}
