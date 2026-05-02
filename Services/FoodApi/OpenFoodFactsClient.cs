using System.Net.Http.Json;
using System.Text.Json;

namespace CalorieTracker.Services.FoodApi;

public class OpenFoodFactsClient : IFoodApiClient
{
    private readonly HttpClient _http;
    private readonly ILogger<OpenFoodFactsClient> _logger;
    private readonly string _username;
    private readonly string _password;

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

    public async Task<List<FoodProduct>> SearchAsync(string query, int pageSize = 10)
    {
        if (string.IsNullOrWhiteSpace(query))
            return [];

        try
        {
            var encoded = Uri.EscapeDataString(query.Trim());
            var url = $"cgi/search.pl?search_terms={encoded}" +
                      $"&search_simple=1&action=process&json=1" +
                      $"&page_size={pageSize}" +
                      $"&fields=product_name,brands,code,image_url,nutriments" +
                      $"&user_id={_username}&user_key={_password}";

            var response = await _http.GetFromJsonAsync<SearchResponse>(url);
            return response?.Products
                .Where(p => !string.IsNullOrWhiteSpace(p.ProductName))
                .ToList() ?? [];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "OpenFoodFacts search failed for query: {Query}", query);
            return [];
        }
    }

    public async Task<FoodProduct?> GetByBarcodeAsync(string barcode)
    {
        try
        {
            var url = $"api/v0/product/{barcode}.json" +
                      $"?fields=product_name,brands,code,image_url,nutriments" +
                      $"&user_id={_username}&user_key={_password}";

            var response = await _http.GetFromJsonAsync<BarcodeResponse>(url);
            return response?.Status == 1 ? response.Product : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "OpenFoodFacts barcode lookup failed: {Barcode}", barcode);
            return null;
        }
    }
}