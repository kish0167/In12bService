using System.Globalization;
using System.Text.Json;
using IN12B8_WindowsService.Providers.FancyDivergenceMeter;

namespace IN12B8_WindowsService.Providers;

public class DivergenceMeterProvider(int duration)  : FormattedStringProvider(duration) 
{
    private readonly MoeDivergenceApiClient _api = new();

    private double _currentDivergence = 0f;
    private double _shownDivergence = 0f;
    private bool _isRunning = false;
    private const int IdleMinSeconds = 1;
    private const int IdleMaxSeconds = 3;
    
    public override void Init()
    {
        _isRunning = true;
        Task.Run(DivergenceFetchCycle);
    }
    
    public override string GetValueString()
    {
        string message = "40end.\n";
        string number = _shownDivergence.ToString(CultureInfo.InvariantCulture) + "000000000";
        return number.Substring(0, 8) + message;
    }

    private async Task DivergenceFetchCycle()
    {
        Random random = new();
        int staticDivergenceThreshold = 0;
        int staticDivergenceCap = IdleMinSeconds;
        
        while (_isRunning)
        {
            double prev = _currentDivergence;
            await UpdateDivergence();
            
            if (Math.Abs(_currentDivergence - prev) > 0.0000001f)
            {
                await ShowEffect(80);
            }
            else
            {
                staticDivergenceThreshold ++;
            }

            if (staticDivergenceThreshold >= staticDivergenceCap)
            {
                staticDivergenceThreshold = 0;
                staticDivergenceCap = (int)random.NextInt64(IdleMinSeconds, IdleMaxSeconds);
                await ShowEffect(10);
            }
            
            await Task.Delay(1000);
        }
    }
    
    private async Task ShowEffect(int duration)
    {
        Random random = new();
        double power = 10;

        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < duration; j++)
            {
                double extra = random.NextDouble() * power;
                extra -= _currentDivergence % power;
                _shownDivergence = _currentDivergence + extra;
                await Task.Delay(28);
            }

            power /= 10;
        }

        _shownDivergence = _currentDivergence;
    }

    private async Task UpdateDivergence()
    {
        _currentDivergence = await _api.GetDivergence();
    }
}

