namespace IN12B8_WindowsService.Providers;

public class LongClockProvider(int duration) : FormattedStringProvider(duration)
{
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
}