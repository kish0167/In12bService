using IN12B8_WindowsService.Providers.FancyDivergenceMeter.Effects.Changers;

namespace IN12B8_WindowsService.Providers.FancyDivergenceMeter.Effects;

public class SequentialFailEffect(IChanger changer) : DivergenceMeterEffect
{
    public override bool IsFinished => Frame >= 21;
    protected override int[] GetDigits()
    {
        for (int i = 0; i < Current.Length; i++)
        {
            if (i == (Frame / 3) % Current.Length)
            {
                Current[i] = changer.Change(Current[i]);
            }
            else
            {
                Current[i] = Target[i];
            }
        }
        return Current;
    }
}