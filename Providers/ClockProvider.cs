namespace IN12B8_WindowsService;

public class ClockProvider : FormattedStringProvider
{
    public override void Init()
    {
        
    }

    public override string GetValueString()
    {
        return DateTime.Now.ToString("HHxmmxss") + "24end.\n";
    }

    public ClockProvider(int duration) : base(duration) { }
}