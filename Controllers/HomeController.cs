using CalorieTracker.Data;
using CalorieTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CalorieTracker.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly AppDbContext _db;

    public HomeController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var today = DateTime.Today;

        var entries = await _db.DiaryEntries
            .Where(e => e.UserId == userId && e.Date == today)
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();

        var goal = await _db.DailyGoals
            .FirstOrDefaultAsync(g => g.UserId == userId)
            ?? new DailyGoal { UserId = userId };

        var vm = new DashboardViewModel
        {
            Today = today,
            TodayEntries = entries,
            Goal = goal
        };

        return View(vm);
    }
}