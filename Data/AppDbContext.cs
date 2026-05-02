using CalorieTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CalorieTracker.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<DiaryEntry> DiaryEntries => Set<DiaryEntry>();
    public DbSet<DailyGoal> DailyGoals => Set<DailyGoal>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed a default daily goal so the app works out of the box
        modelBuilder.Entity<DailyGoal>().HasData(new DailyGoal
        {
            Id = 1,
            CalorieTarget = 2000,
            ProteinTargetG = 150,
            CarbsTargetG = 250,
            FatTargetG = 65,
            FiberTargetG = 30,
            UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });
    }
}