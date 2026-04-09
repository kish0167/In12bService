namespace IN12B8_WindowsService.Providers;

public class DateProvider(int duration) : FormattedStringProvider(duration)
{
    public override string GetValueString()
    {
        string msg = "28end.\n";
        msg = DateTime.Now.ToString("ddMMyyyy") + msg; 
        return msg;
    }
}