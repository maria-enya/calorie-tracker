namespace CalorieTracker.Models;

public class DiaryEntry
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string MealType { get; set; } = "Snack"; // Breakfast, Lunch, Dinner, Snack
    public string FoodName { get; set; } = string.Empty;
    public string? FoodBarcode { get; set; }
    public double QuantityGrams { get; set; }

    // Macros (per entry, already scaled to quantity)
    public double Calories { get; set; }
    public double ProteinG { get; set; }
    public double CarbsG { get; set; }
    public double FatG { get; set; }
    public double FiberG { get; set; }
    public double SugarG { get; set; }

    // Vitamins & minerals
    public double? VitaminCMg { get; set; }
    public double? VitaminAMcg { get; set; }
    public double? CalciumMg { get; set; }
    public double? IronMg { get; set; }
    public double? SodiumMg { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}