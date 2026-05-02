namespace CalorieTracker.Models;

public class DashboardViewModel
{
    public DateTime Today { get; set; } = DateTime.Today;
    public DailyGoal Goal { get; set; } = new();
    public List<DiaryEntry> TodayEntries { get; set; } = [];

    // Totals
    public double TotalCalories => TodayEntries.Sum(e => e.Calories);
    public double TotalProtein => TodayEntries.Sum(e => e.ProteinG);
    public double TotalCarbs => TodayEntries.Sum(e => e.CarbsG);
    public double TotalFat => TodayEntries.Sum(e => e.FatG);
    public double TotalFiber => TodayEntries.Sum(e => e.FiberG);

    // Remaining
    public double RemainingCalories => Math.Max(0, Goal.CalorieTarget - TotalCalories);

    // Percentages (capped at 100)
    public int CaloriePercent => Percent(TotalCalories, Goal.CalorieTarget);
    public int ProteinPercent => Percent(TotalProtein, Goal.ProteinTargetG);
    public int CarbsPercent => Percent(TotalCarbs, Goal.CarbsTargetG);
    public int FatPercent => Percent(TotalFat, Goal.FatTargetG);
    public int FiberPercent => Percent(TotalFiber, Goal.FiberTargetG);

    // Last 3 entries for quick view
    public List<DiaryEntry> RecentEntries => TodayEntries.TakeLast(3).ToList();

    public bool HasEntries => TodayEntries.Any();

    private static int Percent(double value, double target) =>
        target <= 0 ? 0 : Math.Min(100, (int)(value / target * 100));
}