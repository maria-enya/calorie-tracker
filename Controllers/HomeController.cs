using CalorieTracker.Data;
using CalorieTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CalorieTracker.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _db;

    public HomeController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var today = DateTime.Today;

        var entries = await _db.DiaryEntries
            .Where(e => e.Date == today)
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();

        var goal = await _db.DailyGoals.FirstOrDefaultAsync() ?? new DailyGoal();

        var vm = new DashboardViewModel
        {
            Today = today,
            TodayEntries = entries,
            Goal = goal
        };

        return View(vm);
    }
}