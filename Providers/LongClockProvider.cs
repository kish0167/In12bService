namespace IN12B8_WindowsService;

public class LongClockProvider : FormattedStringProvider
{
    public override void Init()
    {
        
    }

    public override string GetValueString()
    { 
        DateTime now = DateTime.Now;
        string msg = "2:end.\n";
        msg = now.Millisecond / 10 + msg;
        
        if (msg.Length < 9)
        {
            msg = "0" + msg;
        }
        
        msg = now.ToString("HHmmss") + msg;
        return msg;
    }

    public LongClockProvider(int duration) : base(duration) { }
}