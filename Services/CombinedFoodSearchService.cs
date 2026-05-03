using CalorieTracker.Services.FoodApi;

namespace CalorieTracker.Services;

public class CombinedFoodSearchService
{
    private readonly IFoodApiClient _openFoodFacts;
    private readonly UsdaFoodClient _usda;
    private readonly ILogger<CombinedFoodSearchService> _logger;

    public CombinedFoodSearchService(
        IFoodApiClient openFoodFacts,
        UsdaFoodClient usda,
        ILogger<CombinedFoodSearchService> logger)
    {
        _openFoodFacts = openFoodFacts;
        _usda = usda;
        _logger = logger;
    }

    public async Task<CombinedSearchResult> SearchAsync(string query, int pageSize = 8)
    {
        // Run both searches in parallel
        var usdaTask = _usda.SearchAsync(query, pageSize);
        var offTask = _openFoodFacts.SearchAsync(query, pageSize);

        await Task.WhenAll(usdaTask, offTask);

        return new CombinedSearchResult
        {
            UsdaResults = usdaTask.Result,
            OpenFoodFactsResults = offTask.Result,
        };
    }
}

public class CombinedSearchResult
{
    public List<FoodProduct> UsdaResults { get; set; } = [];
    public List<FoodProduct> OpenFoodFactsResults { get; set; } = [];

    public bool HasAny => UsdaResults.Any() || OpenFoodFactsResults.Any();
}