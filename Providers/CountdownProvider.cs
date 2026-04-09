namespace IN12B8_WindowsService.Providers;

public class CountdownProvider(int duration) : FormattedStringProvider(duration)
{
    private readonly DateTime _endTime = new(2025, 12, 13);

    public override string GetValueString()
    {
        string msg = "00end.\n";
        int seconds = (int)(_endTime - DateTime.Now).TotalSeconds;
        msg = Math.Abs(seconds) + msg;
        msg = "00000000" + msg;
        return msg;
    }
}