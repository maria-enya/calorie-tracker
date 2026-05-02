namespace CalorieTracker.Models;

public class DailyGoal
{
    public int Id { get; set; }
    public double CalorieTarget { get; set; } = 2000;
    public double ProteinTargetG { get; set; } = 150;
    public double CarbsTargetG { get; set; } = 250;
    public double FatTargetG { get; set; } = 65;
    public double FiberTargetG { get; set; } = 30;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}