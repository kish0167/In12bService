namespace IN12B8_WindowsService.Providers;

public class DateBackwardsProvider(int duration) : FormattedStringProvider(duration)
{
    public override string GetValueString()
    {
        string msg = "0:end.\n";
        msg = DateTime.Now.ToString("yyyyMMdd") + msg; 

        return msg;
    }
}