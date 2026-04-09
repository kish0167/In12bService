namespace IN12B8_WindowsService.Providers.FancyDivergenceMeter.Effects.Changers;

public class CyclicChanger : IChanger
{
    public int Change(int n)
    {
        return (n + 1) % 10;
    }
}