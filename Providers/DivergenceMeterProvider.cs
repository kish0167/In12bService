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
        int staticDivergenceCap = 120;
        
        while (_isRunning)
        {
            double prev = _currentDivergence;
            await UpdateDivergence();
            
            if (Math.Abs(_currentDivergence - prev) > 0.0000001f)
            {
                await ShowEffect();
            }
            else
            {
                staticDivergenceThreshold ++;
            }

            if (staticDivergenceThreshold >= staticDivergenceCap)
            {
                staticDivergenceThreshold = 0;
                staticDivergenceCap = (int)random.NextInt64(120, 600);
                await ShowEffect();
            }
            
            await Task.Delay(1000);
        }
    }

    private async Task ShowEffect()
    {
        Random random = new();
        double divider = 10f;

        for (int i = 0; i < 40; i++)
        {
            for (int j = 0; j < 50; j++)
            {
                double randDouble = (random.NextDouble() - 0.5f) * divider * 2;
                _shownDivergence = double.Abs(_currentDivergence + randDouble);
                await Task.Delay(10);
            }
            
            divider /= 2;
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

