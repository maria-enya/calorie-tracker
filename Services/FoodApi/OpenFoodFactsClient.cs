using System.Net;
using System.Net.Http.Json;

namespace CalorieTracker.Services.FoodApi;

public class OpenFoodFactsClient : IFoodApiClient
{
    private readonly HttpClient _http;
    private readonly ILogger<OpenFoodFactsClient> _logger;
    private readonly string _username;
    private readonly string _password;
    private bool _isLoggedIn = false;

    public OpenFoodFactsClient(
        HttpClient http,
        ILogger<OpenFoodFactsClient> logger,
        IConfiguration config)
    {
        _http = http;
        _logger = logger;
        _username = config["OpenFoodFacts:Username"] ?? string.Empty;
        _password = config["OpenFoodFacts:Password"] ?? string.Empty;
    }

    private async Task EnsureLoggedInAsync()
    {
        if (_isLoggedIn) return;

        var loginData = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["user_id"] = _username,
            ["user_session_cookie"] = "1",
            ["password"] = _password,
        });

        var response = await _http.PostAsync("cgi/session.pl", loginData);
        _logger.LogInformation("Login status: {Status}", response.StatusCode);
        _isLoggedIn = true;
    }

    public async Task<List<FoodProduct>> SearchAsync(string query, int pageSize = 10)
    {
        if (string.IsNullOrWhiteSpace(query))
            return [];

        await EnsureLoggedInAsync();

        var encoded = Uri.EscapeDataString(query.Trim());
        var url = $"cgi/search.pl?search_terms={encoded}" +
                  $"&search_simple=1&action=process&json=1" +
                  $"&page_size={pageSize}" +
                  $"&fields=product_name,brands,code,image_url,nutriments";

        var response = await _http.GetAsync(url);
        _logger.LogInformation("Search status: {Status}", response.StatusCode);

        if (!response.IsSuccessStatusCode)
            return [];

        var result = await response.Content.ReadFromJsonAsync<SearchResponse>();
        return result?.Products
            .Where(p => !string.IsNullOrWhiteSpace(p.ProductName))
            .ToList() ?? [];
    }

    public async Task<FoodProduct?> GetByBarcodeAsync(string barcode)
    {
        await EnsureLoggedInAsync();

        var url = $"api/v0/product/{barcode}.json" +
                  $"?fields=product_name,brands,code,image_url,nutriments";

        var response = await _http.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<BarcodeResponse>();
        return result?.Status == 1 ? result.Product : null;
    }
}