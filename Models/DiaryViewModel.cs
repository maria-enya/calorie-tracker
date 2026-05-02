namespace CalorieTracker.Models;

public class DiaryViewModel
{
    public DateTime SelectedDate { get; set; }
    public DateTime PreviousDate { get; set; }
    public DateTime NextDate { get; set; }
    public List<DiaryEntry> Entries { get; set; } = [];
    public DailyGoal Goal { get; set; } = new();

    // Computed totals
    public double TotalCalories => Entries.Sum(e => e.Calories);
    public double TotalProtein => Entries.Sum(e => e.ProteinG);
    public double TotalCarbs => Entries.Sum(e => e.CarbsG);
    public double TotalFat => Entries.Sum(e => e.FatG);
    public double TotalFiber => Entries.Sum(e => e.FiberG);

    // Progress percentages (capped at 100%)
    public int CaloriePercent => Math.Min(100, (int)(TotalCalories / Goal.CalorieTarget * 100));
    public int ProteinPercent => Math.Min(100, (int)(TotalProtein / Goal.ProteinTargetG * 100));
    public int CarbsPercent => Math.Min(100, (int)(TotalCarbs / Goal.CarbsTargetG * 100));
    public int FatPercent => Math.Min(100, (int)(TotalFat / Goal.FatTargetG * 100));

    // Group entries by meal type
    public Dictionary<string, List<DiaryEntry>> EntriesByMeal =>
        Entries.GroupBy(e => e.MealType)
               .ToDictionary(g => g.Key, g => g.ToList());

    public bool IsToday => SelectedDate.Date == DateTime.Today;
}