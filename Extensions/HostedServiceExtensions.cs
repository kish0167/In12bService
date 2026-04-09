using IN12B8_WindowsService.CoreLogic;

namespace IN12B8_WindowsService.Extensions;

public static class HostedServiceExtensions
{
    public static IHostApplicationBuilder AddIn12BService(
        this IHostApplicationBuilder builder, 
        Action<In12BOptions> configure)
    {
        builder.Services.Configure(configure);
        builder.Services.AddHostedService<In12BService>();
        return builder;
    }
}