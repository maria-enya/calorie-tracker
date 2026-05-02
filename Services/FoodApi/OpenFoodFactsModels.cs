using System.Text.Json.Serialization;

namespace CalorieTracker.Services.FoodApi;

public class SearchResponse
{
    [JsonPropertyName("products")]
    public List<FoodProduct> Products { get; set; } = [];

    [JsonPropertyName("count")]
    public int Count { get; set; }
}

public class BarcodeResponse
{
    [JsonPropertyName("product")]
    public FoodProduct? Product { get; set; }

    [JsonPropertyName("status")]
    public int Status { get; set; }
}

public class FoodProduct
{
    [JsonPropertyName("product_name")]
    public string? ProductName { get; set; }

    [JsonPropertyName("brands")]
    public string? Brands { get; set; }

    [JsonPropertyName("code")]
    public string? Barcode { get; set; }

    [JsonPropertyName("image_url")]
    public string? ImageUrl { get; set; }

    [JsonPropertyName("nutriments")]
    public Nutriments? Nutriments { get; set; }
}

public class Nutriments
{
    [JsonPropertyName("energy-kcal_100g")]
    public double? CaloriesPer100g { get; set; }

    [JsonPropertyName("proteins_100g")]
    public double? ProteinPer100g { get; set; }

    [JsonPropertyName("carbohydrates_100g")]
    public double? CarbsPer100g { get; set; }

    [JsonPropertyName("fat_100g")]
    public double? FatPer100g { get; set; }

    [JsonPropertyName("fiber_100g")]
    public double? FiberPer100g { get; set; }

    [JsonPropertyName("sugars_100g")]
    public double? SugarPer100g { get; set; }

    [JsonPropertyName("vitamin-c_100g")]
    public double? VitaminCPer100g { get; set; }

    [JsonPropertyName("vitamin-a_100g")]
    public double? VitaminAPer100g { get; set; }

    [JsonPropertyName("calcium_100g")]
    public double? CalciumPer100g { get; set; }

    [JsonPropertyName("iron_100g")]
    public double? IronPer100g { get; set; }

    [JsonPropertyName("sodium_100g")]
    public double? SodiumPer100g { get; set; }
}