using CalorieTracker.Models;
using CalorieTracker.Services.FoodApi;

namespace CalorieTracker.Services;

public static class NutritionCalculator
{
    public static DiaryEntry BuildEntry(
        FoodProduct product,
        double grams,
        string mealType,
        DateTime date)
    {
        var n = product.Nutriments;
        double factor = grams / 100.0;

        return new DiaryEntry
        {
            Date = date.Date,
            MealType = mealType,
            FoodName = BuildFoodName(product),
            FoodBarcode = product.Barcode,
            QuantityGrams = grams,

            Calories = Round(n?.CaloriesPer100g * factor),
            ProteinG = Round(n?.ProteinPer100g * factor),
            CarbsG = Round(n?.CarbsPer100g * factor),
            FatG = Round(n?.FatPer100g * factor),
            FiberG = Round(n?.FiberPer100g * factor),
            SugarG = Round(n?.SugarPer100g * factor),

            VitaminCMg = RoundNullable(n?.VitaminCPer100g * factor * 1000),
            VitaminAMcg = RoundNullable(n?.VitaminAPer100g * factor * 1000000),
            CalciumMg = RoundNullable(n?.CalciumPer100g * factor * 1000),
            IronMg = RoundNullable(n?.IronPer100g * factor * 1000),
            SodiumMg = RoundNullable(n?.SodiumPer100g * factor * 1000),
        };
    }

    private static string BuildFoodName(FoodProduct product)
    {
        var name = product.ProductName ?? "Unknown food";
        if (!string.IsNullOrWhiteSpace(product.Brands))
            name += $" ({product.Brands})";
        return name;
    }

    private static double Round(double? value) =>
        Math.Round(value ?? 0, 1);

    private static double? RoundNullable(double? value) =>
        value.HasValue ? Math.Round(value.Value, 2) : null;
}