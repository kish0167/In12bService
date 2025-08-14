namespace IN12B8_WindowsService;

public class Worker : BackgroundService
{
    public Worker() { }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        In12BService service = new();
        await service.Run(stoppingToken);
        
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }
}