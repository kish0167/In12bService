namespace IN12B8_WindowsService.Providers.FancyDivergenceMeter.Effects.Changers;

public class TubeCyclicChanger : IChanger
{
    private static readonly int[] Order = [4, 6, 7, 1, 9, 0, 2, 5, 3, 8];
    public int Change(int n)
    {
        if (n < 0 || n > 9) return -1;
        return Order[n];
    }
}