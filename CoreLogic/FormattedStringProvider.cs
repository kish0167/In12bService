namespace IN12B8_WindowsService;

public abstract class FormattedStringProvider
{
    protected int _duration;

    public int Duration => _duration;

    public FormattedStringProvider(int duration)
    {
        _duration = duration;
    }

    public virtual void Init() { }
    public abstract string GetValueString();
}