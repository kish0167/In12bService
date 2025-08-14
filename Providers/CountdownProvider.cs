namespace IN12B8_WindowsService;

public class CountdownProvider : FormattedStringProvider
{
    private DateTime EndTime = new(2025, 12, 13);
    public override void Init()
    {
        
    }

    public override string GetValueString()
    {
        string msg = "00end.\n";
        int seconds = (int)(EndTime - DateTime.Now).TotalSeconds;
        msg = seconds + msg;
        msg = "00000000" + msg;
        return msg;
    }

    public CountdownProvider(int duration) : base(duration) { }
}