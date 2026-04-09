using System.Globalization;

namespace IN12B8_WindowsService.Providers.FancyDivergenceMeter.Effects;

public abstract class DivergenceMeterEffect
{ 
    public abstract bool IsFinished { get; }
    protected abstract int[] GetDigits();

    protected int Frame;
    protected readonly Random Rng = new();
    protected int[] Target = [];
    protected int[] Current = [];
    
    public virtual void Reset(double targetValue)
    {
        Frame = 0;
        Target = Decompose(targetValue);
        Current = Decompose(targetValue);
    }
    public virtual int[] GetNextFrameDigits()
    {
        int[] digits = GetDigits();
        Frame ++;
        return digits;
    }

    private int[] Decompose(double value)
    {
        string s = value.ToString("F6", CultureInfo.InvariantCulture).Replace(".", "");
        return s.PadRight(7, '0').Select(c => c - '0').ToArray();
    }
}