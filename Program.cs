using IN12B8_WindowsService.CoreLogic;
using IN12B8_WindowsService.Extensions;
using IN12B8_WindowsService.Providers;
using IN12B8_WindowsService.Providers.FancyDivergenceMeter;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService();

builder.AddIn12BService(options =>
{
    options.Providers =
    [
        new FancyDivergenceMeterProvider(1000),
    ];
});

IHost host = builder.Build();

host.Run();