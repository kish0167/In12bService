namespace IN12B8_WindowsService.Providers.FancyDivergenceMeter.Effects.Changers;

public class ExcludingRandomChanger : IChanger
{
    private readonly RandomChanger _randomChanger = new();
    
    public int Change(int n)
    {
        int random = _randomChanger.Change(n);
        if (random == n) random = (random + 1) % 10;
        return random;
    }
}