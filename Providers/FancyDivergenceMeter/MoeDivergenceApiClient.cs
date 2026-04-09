using System.Text.Json;
using System.Text.Json.Serialization;

namespace IN12B8_WindowsService.Providers.FancyDivergenceMeter;

public class MoeDivergenceApiClient
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://divergence.nyarchlinux.moe";

    public MoeDivergenceApiClient()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(BaseUrl);
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }
    
    public async Task<double> GetDivergence()
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/api/divergence");
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
            DivergenceResponse? result = JsonSerializer.Deserialize<DivergenceResponse>(json);
            return result?.Divergence ?? 0;
        }
        catch
        {
            return 0;
        }
    }
    
    private class DivergenceResponse
    {
        [JsonPropertyName("divergence")]
        public double Divergence { get; set; }
    }
}