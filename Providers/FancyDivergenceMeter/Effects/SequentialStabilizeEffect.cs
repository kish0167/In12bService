using IN12B8_WindowsService.Providers.FancyDivergenceMeter.Effects.Changers;

namespace IN12B8_WindowsService.Providers.FancyDivergenceMeter.Effects;

public class SequentialStabilizeEffect(int framesPerDigit, IChanger changer) : DivergenceMeterEffect
{
    public override bool IsFinished => Frame > 7 * framesPerDigit;

    public override void Reset(double targetValue)
    {
        base.Reset(targetValue);
        for (int i = 0; i < 7; i++) Current[i] = Rng.Next(0, 10);
    }

    protected override int[] GetDigits()
    {
        int stabilizedCount = Frame / framesPerDigit;

        for (int i = 0; i < 7; i++)
        {
            if (i < stabilizedCount)
            {
                Current[i] = Target[i];
            }
            else
            {
                Current[i] = changer.Change(Current[i]);
            }
        }
        
        return Current;
    }
}