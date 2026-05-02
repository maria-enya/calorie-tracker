using System.ComponentModel.DataAnnotations;

namespace CalorieTracker.Models;

public class DailyGoal
{
    public int Id { get; set; }

    [Required]
    [Range(500, 10000, ErrorMessage = "Calories must be between 500 and 10,000")]
    [Display(Name = "Daily Calories")]
    public double CalorieTarget { get; set; } = 2000;

    [Required]
    [Range(0, 500, ErrorMessage = "Protein must be between 0 and 500g")]
    [Display(Name = "Protein (g)")]
    public double ProteinTargetG { get; set; } = 150;

    [Required]
    [Range(0, 1000, ErrorMessage = "Carbs must be between 0 and 1000g")]
    [Display(Name = "Carbohydrates (g)")]
    public double CarbsTargetG { get; set; } = 250;

    [Required]
    [Range(0, 300, ErrorMessage = "Fat must be between 0 and 300g")]
    [Display(Name = "Fat (g)")]
    public double FatTargetG { get; set; } = 65;

    [Required]
    [Range(0, 100, ErrorMessage = "Fiber must be between 0 and 100g")]
    [Display(Name = "Fiber (g)")]
    public double FiberTargetG { get; set; } = 30;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}