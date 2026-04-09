namespace IN12B8_WindowsService.Providers;

public abstract class FormattedStringProvider(int duration)
{
    protected int _duration = duration;

    public int Duration => Math.Clamp(_duration, 10, 1000000);

    public virtual void Init() { }
    public abstract string GetValueString();
}