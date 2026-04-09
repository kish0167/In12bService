namespace IN12B8_WindowsService.Providers;

public class DebugProvider(int duration) : FormattedStringProvider(duration)
{
    public override string GetValueString()
    {
        return "1627498300end.\n";
    }
}