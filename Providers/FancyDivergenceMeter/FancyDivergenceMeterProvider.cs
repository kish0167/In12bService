namespace IN12B8_WindowsService.Providers.FancyDivergenceMeter;

public class FancyDivergenceMeterProvider(int duration) : FormattedStringProvider(duration)
{
    public override void Init()
    {
        
    }
    
    public override string GetValueString()
    {
        return "--------22end.\n";
    }
}