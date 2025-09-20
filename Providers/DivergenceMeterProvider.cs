using System.Globalization;

namespace IN12B8_WindowsService;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

public class DivergenceMeterProvider : FormattedStringProvider
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://divergence.nyarchlinux.moe";

    private double _currentDivergence = 0f;
    private double _shownDivergence = 0f;
    private bool _isRunning = false;
    private const int IdleMinSeconds = 5;
    private const int IdleMaxSeconds = 10;

    public DivergenceMeterProvider(int duration) : base(duration)
    {
        _duration = duration;
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(BaseUrl);
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }
    public override void Init()
    {
        _isRunning = true;
        Task.Run(DivergenceFetchCycle);
    }
    public override string GetValueString()
    {
        string message = "40end.\n";
        string number = _shownDivergence.ToString(CultureInfo.InvariantCulture) + "000000000";
        
        for (int i = 7; i >= 0; i--)
        {
            message = number[i] + message;
        }

        return message;
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
                await ShowEffect(20);
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
        try
        {
            var response = await _httpClient.GetAsync("/api/divergence");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<DivergenceResponse>(json);

            _currentDivergence = result?.divergence ?? 0f;
        }
        catch
        {
            // ignored
        }
    }
    
    private class DivergenceResponse
    {
        public double divergence { get; set; }
    }
}

