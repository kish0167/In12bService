using IN12B8_WindowsService.Providers;

namespace IN12B8_WindowsService.CoreLogic;

public class In12BOptions
{
    public List<FormattedStringProvider> Providers = [new ClockProvider(10000)];
}