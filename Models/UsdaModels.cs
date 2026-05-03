using System.Text.Json.Serialization;

namespace CalorieTracker.Services.FoodApi;

public class UsdaSearchResponse
{
    [JsonPropertyName("foods")]
    public List<UsdaFood> Foods { get; set; } = [];

    [JsonPropertyName("totalHits")]
    public int TotalHits { get; set; }
}

public class UsdaFood
{
    [JsonPropertyName("fdcId")]
    public int FdcId { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("brandOwner")]
    public string? BrandOwner { get; set; }

    [JsonPropertyName("dataType")]
    public string? DataType { get; set; }

    [JsonPropertyName("foodNutrients")]
    public List<UsdaNutrient> FoodNutrients { get; set; } = [];
}

public class UsdaNutrient
{
    [JsonPropertyName("nutrientId")]
    public int NutrientId { get; set; }

    [JsonPropertyName("nutrientName")]
    public string? NutrientName { get; set; }

    [JsonPropertyName("value")]
    public double Value { get; set; }

    [JsonPropertyName("unitName")]
    public string? UnitName { get; set; }
}