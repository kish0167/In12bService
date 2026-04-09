using IN12B8_WindowsService.Providers;
using Microsoft.Extensions.Options;

namespace IN12B8_WindowsService.CoreLogic;

public class In12BService(IOptions<In12BOptions> options) : BackgroundService
{
    private readonly BteSerialClient _bte = new();
    private FormattedStringProvider? _currentProvider;
    private readonly List<FormattedStringProvider> _providersList = options.Value.Providers;

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        _providersList.ForEach(provider => provider.Init());   
        await Run(ct);
    }
    
    private async Task Run(CancellationToken ct)
    {
        _ = _bte.StartAsync(ct);
        _ = MainCycle(ct);
        
        while (!ct.IsCancellationRequested)
        {
            await Task.Delay(_currentProvider?.Duration ?? 500, ct);
            SetNextProvider();
        }
    }

    private void SetNextProvider()
    {
        if (_providersList.Count == 0) return;
        int currentIndex = _currentProvider != null ? _providersList.IndexOf(_currentProvider) : -1;
        int nextIndex = (currentIndex + 1) % _providersList.Count;
        _currentProvider = _providersList[nextIndex];
    }
    
    private async Task MainCycle(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            if(_bte.Connected) _bte.SendData(_currentProvider?.GetValueString() ?? "167--16700end.\n");
            await Task.Delay(30, ct);
        }
    }
}