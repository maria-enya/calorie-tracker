using System.ComponentModel.DataAnnotations;

namespace CalorieTracker.Models;

public class ManualFoodViewModel
{
    [Required]
    [Display(Name = "Food name")]
    public string FoodName { get; set; } = string.Empty;

    [Required]
    [Range(1, 5000)]
    [Display(Name = "Quantity (g)")]
    public double Grams { get; set; } = 100;

    [Required]
    public string MealType { get; set; } = "Snack";

    public DateTime Date { get; set; } = DateTime.Today;

    [Range(0, 10000)]
    [Display(Name = "Calories (kcal)")]
    public double Calories { get; set; }

    [Range(0, 500)]
    [Display(Name = "Protein (g)")]
    public double ProteinG { get; set; }

    [Range(0, 1000)]
    [Display(Name = "Carbs (g)")]
    public double CarbsG { get; set; }

    [Range(0, 500)]
    [Display(Name = "Fat (g)")]
    public double FatG { get; set; }

    [Range(0, 200)]
    [Display(Name = "Fiber (g)")]
    public double FiberG { get; set; }
}