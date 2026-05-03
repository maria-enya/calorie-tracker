using System.Net.Http.Json;

namespace CalorieTracker.Services.FoodApi;

public class UsdaFoodClient
{
    private readonly HttpClient _http;
    private readonly ILogger<UsdaFoodClient> _logger;
    private readonly string _apiKey;

    // USDA nutrient IDs
    private const int EnergyId = 1008;
    private const int ProteinId = 1003;
    private const int CarbsId = 1005;
    private const int FatId = 1004;
    private const int FiberId = 1079;
    private const int SugarsId = 2000;
    private const int SodiumId = 1093;
    private const int CalciumId = 1087;
    private const int IronId = 1089;
    private const int VitCId = 1162;
    private const int VitAId = 1106;

    public UsdaFoodClient(HttpClient http,
        ILogger<UsdaFoodClient> logger,
        IConfiguration config)
    {
        _http = http;
        _logger = logger;
        _apiKey = config["Usda:ApiKey"] ?? string.Empty;
    }

    public async Task<List<FoodProduct>> SearchAsync(string query, int pageSize = 10)
    {
        if (string.IsNullOrWhiteSpace(query)) return [];

        try
        {
            var encoded = Uri.EscapeDataString(query.Trim());
            var url = $"foods/search?query={encoded}" +
                      $"&pageSize={pageSize}" +
                      $"&dataType=Foundation,SR%20Legacy,Branded" +
                      $"&api_key={_apiKey}";

            var response = await _http.GetFromJsonAsync<UsdaSearchResponse>(url);

            return response?.Foods
                .Where(f => !string.IsNullOrWhiteSpace(f.Description))
                .Select(MapToFoodProduct)
                .ToList() ?? [];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "USDA search failed for: {Query}", query);
            return [];
        }
    }

    private static FoodProduct MapToFoodProduct(UsdaFood food)
    {
        double GetNutrient(int id) =>
            food.FoodNutrients.FirstOrDefault(n => n.NutrientId == id)?.Value ?? 0;

        return new FoodProduct
        {
            ProductName = ToTitleCase(food.Description ?? "Unknown"),
            Brands = food.BrandOwner,
            Barcode = $"usda-{food.FdcId}",
            Nutriments = new Nutriments
            {
                CaloriesPer100g = GetNutrient(EnergyId),
                ProteinPer100g = GetNutrient(ProteinId),
                CarbsPer100g = GetNutrient(CarbsId),
                FatPer100g = GetNutrient(FatId),
                FiberPer100g = GetNutrient(FiberId),
                SugarPer100g = GetNutrient(SugarsId),
                SodiumPer100g = GetNutrient(SodiumId) / 1000,
                CalciumPer100g = GetNutrient(CalciumId) / 1000,
                IronPer100g = GetNutrient(IronId) / 1000,
                VitaminCPer100g = GetNutrient(VitCId) / 1000,
                VitaminAPer100g = GetNutrient(VitAId) / 1000000,
            }
        };
    }

    private static string ToTitleCase(string text) =>
        System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
}