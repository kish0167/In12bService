namespace IN12B8_WindowsService;

public class DateBackwardsProvider : FormattedStringProvider
{
    public override void Init()
    {
        
    }

    public override string GetValueString()
    {
        string msg = "0:end.\n";
        msg = DateTime.Now.ToString("yyyyMMdd") + msg; 

        return msg;
    }

    public DateBackwardsProvider(int duration) : base(duration) { }
}