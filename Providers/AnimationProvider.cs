namespace IN12B8_WindowsService.Providers;

public class AnimationProvider(int duration) : FormattedStringProvider(duration)
{
    private int _frame = 0;
    public override string GetValueString()
    {
        string s = "00end.\n";
        s = OneApproximatePosition(Math.Sin(_frame * 0.2) * 3.5, Math.Cos(_frame * 0.205) * 3.5) + s;
        _frame++;
        return s;
    }

    private string OneApproximatePosition(double position0, double position1)
    {
        return GetDigitAtIndex(1, (int)(position0 + 4), (int)(position1 + 4));
    }

    private string GetDigitAtIndex(int digit, int index0, int index1)
    {
        string s = "";
        for (int i = 0; i < 8; i++)
        {
            s += (i == index0 || i == index1) ? digit.ToString() : "-";
        }

        return s;
    }
}