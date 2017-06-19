
using System;
using System.IO;
using System.Text;
using System.Threading;

using APPLEDIE;
using BIModel;
using PowerTest.Test;

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

    protected LongTermTest(Iport port, string mode, int times)
    {
        mPort = port;
        mMode = mode;
        mTimes = times;
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
        taskDone();
    }
}
class SendFile : LongTermTest
{
    private string mFile;
    private StreamReader mSr;
    public SendFile(Iport port, string file, int times, string mode)
        : base(port, mode, times)
    {
        mFile = file;
    }
    override public void Prepare()
    {
        mSr = new StreamReader(mFile, Encoding.Default);
        Logger.SetFile("SendFile_" + mStart.ToString("yyMMddHHmmss"));
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
    protected int mInterval = 0;
    protected StreamWriter mDatFile = null;
    protected JzhPower mJzh = null;
    public JzhTest(Iport port, int duration, int interval = 1000)
        : base(port, "Minute", duration)
    {
        mJzh = port as JzhPower;
    }
    protected void SaveData(StreamWriter dat, double[] current, double[] voltage)
    {
        string oneshot = "";

        oneshot += "Current: ";
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
    public SingleCurrentTest(Iport port, double initVal, int duration, int interval = 1000)
        : base(port, duration, interval)
    {
        mInitVal = initVal;
    }
    public void SetDatFile(StreamWriter sw)
    {
        mDatFile = sw;
    }
    public override void Prepare()
    {
        if (mDatFile == null)
        {
            mDatFile = new StreamWriter(new FileStream("SingleCurrentTest_" + mStart.ToString("yyMMddHHmmss") + ".dat", FileMode.Append));
            Logger.SetFile("SingleCurrentTest_" + mStart.ToString("yyMMddHHmmss"));
        }
        mJzh.SetCurrent(mInitVal);
    }
    public override void SingleRun()
    {
        double[] current;
        double[] voltage;

        mJzh.ReadVoltageAndCurrent(out current, out voltage);
        SaveData(mDatFile, current, voltage);
        Thread.Sleep(mInterval);
    }
}
class MultiCurrentTest : JzhTest
{
    protected double[] mTestPoints;
    public MultiCurrentTest(Iport port, double[] testPoints, int duration, int interval = 1000)
        : base(port, duration, interval)
    {
        mTestPoints = testPoints;
    }
    public override void Prepare()
    {
        mDatFile = new StreamWriter(new FileStream("MultiCurrentTest_" + mStart.ToString("yyMMddHHmmss") + ".dat", FileMode.Append));
        Logger.SetFile("MultiCurrentTest_" + mStart.ToString("yyMMddHHmmss"));
    }
    public override void SingleRun()
    {
        for (int i = 0; i < mTestPoints.Length; i++)
        {
            SingleCurrentTest aPointTest = new SingleCurrentTest(mPort, mTestPoints[i], mTimes / 4, mInterval);
            aPointTest.updateUi = updateUi;
            aPointTest.SetDatFile(mDatFile);
            aPointTest.Run();
        }
    }
}
