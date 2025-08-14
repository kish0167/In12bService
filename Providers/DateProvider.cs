namespace IN12B8_WindowsService;

public class DateProvider : FormattedStringProvider
{
    public override void Init()
    {
        
    }

    public override string GetValueString()
    {
        string msg = "28end.\n";
        msg = DateTime.Now.ToString("ddMMyyyy") + msg; 

        return msg;
    }

    public DateProvider(int duration) : base(duration) { }
}