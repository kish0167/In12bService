namespace IN12B8_WindowsService.Providers;

public class ClockProvider(int duration) : FormattedStringProvider(duration)
{
    public override string GetValueString()
    {
        return DateTime.Now.ToString("HHxmmxss") + "24end.\n";
    }
}