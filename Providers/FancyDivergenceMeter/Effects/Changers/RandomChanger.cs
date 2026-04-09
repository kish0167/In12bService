namespace IN12B8_WindowsService.Providers.FancyDivergenceMeter.Effects.Changers;

public class RandomChanger : IChanger
{
    private readonly Random _rng = new();
    
    public int Change(int n)
    {
        return _rng.Next(0, 10);
    }
}