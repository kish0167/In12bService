using IN12B8_WindowsService.Extensions;
using IN12B8_WindowsService.Providers;
using IN12B8_WindowsService.Providers.FancyDivergenceMeter;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService();

builder.AddIn12BService(options =>
{
    options.Providers =
    [
        new FancyDivergenceMeterProvider(25000),
        new DateBackwardsProvider(5000),
        new ClockProvider(15000),
        new CountdownProvider(15000),
        new HardwareMonitoringProvider(6000)
    ];
});

IHost host = builder.Build();

host.Run();