namespace IN12B8_WindowsService.Providers.FancyDivergenceMeter.Effects;

public class FailingTubeEffect : DivergenceMeterEffect
{
    public override bool IsFinished => Frame > _animation.Length;
    private int _index = 0;
    private readonly int[] _animation = [
        0, 0, 1, 1, 0,
        0, 0, 0, 0, 1,
        1, 1, 0, 1, 0,
        0, 0, 1, 1, 0,
    ];

    protected override int[] GetDigits()
    {
        if (Frame < _animation.Length)
        {
            Current[_index] = _animation[Frame] == 1 ? Target[_index] : -1;
            return Current;
        }
        else
        {
            return Target;
        }
    }

    public override void Reset(double targetValue)
    {
        base.Reset(targetValue);
        _index = Rng.Next(0, 7);
    }
}