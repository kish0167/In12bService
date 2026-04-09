using IN12B8_WindowsService.Providers.FancyDivergenceMeter.Effects.Changers;

namespace IN12B8_WindowsService.Providers.FancyDivergenceMeter.Effects;

public class UnstableDigitEffect(int durationFrames, IChanger changer) : DivergenceMeterEffect
{
    public override bool IsFinished => Frame > durationFrames;
    private int _index = 0;


    protected override int[] GetDigits()
    {
        Current[_index] = changer.Change(Current[_index]);
        if (Frame < durationFrames) return Current;
        return Target;
    }

    public override void Reset(double targetValue)
    {
        base.Reset(targetValue);
        _index = Rng.Next(0, 7);
    }
}