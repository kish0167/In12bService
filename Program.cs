using IN12B8_WindowsService;
using IN12B8_WindowsService.CoreLogic;
using IN12B8_WindowsService.Extensions;
using IN12B8_WindowsService.Providers;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService();
builder.Services.AddHostedService<In12BService>();
builder.AddIn12BService(options =>
{
    options.Providers =
    [
        new DivergenceMeterProvider(1000)
    ];
});

IHost host = builder.Build();

host.Run();