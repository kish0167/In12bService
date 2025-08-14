namespace IN12B8_WindowsService;

public class In12BService
{
    private BteSerialClient _bte = new();
    private FormattedStringProvider? _currentProvider = null;
    private List<FormattedStringProvider> _providersList = new();
    
    public async Task Run(CancellationToken ct)
    {
        CreateProviders();
        await _bte.Connect();
        SetNextProvider();
        _ = Task.Run(CurrentProviderCycle);
        
        while (!ct.IsCancellationRequested)
        {
            await Task.Delay(_currentProvider.Duration);
            SetNextProvider();
        }
    }

    private void CreateProviders()
    {
        _providersList.Add(new DateProvider(3000));
        _providersList.Add(new DivergenceMeterProvider(5000));
        _providersList.Add(new ClockProvider(5000));
        _providersList.Add(new CountdownProvider(10000));
        _providersList.Add(new HardwareMonitoringProvider(4000));
        
        ////
        
        foreach (FormattedStringProvider provider in _providersList)
        {
            provider.Init();
        }
    }

    private void SetNextProvider()
    {
        int index = 0;
        
        if (_currentProvider != null)
        {
            index = _providersList.IndexOf(_currentProvider) + 1;
            if (index >= _providersList.Count)
            {
                index = 0;
            }
        }

        _currentProvider = _providersList[index];
    }
    
    private async Task CurrentProviderCycle()
    {
        while (true)
        {
            _bte.SendString(_currentProvider?.GetValueString() ?? "0100011100end.\n");
            await Task.Delay(30);
        }
    }
}