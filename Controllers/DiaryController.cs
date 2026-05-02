using CalorieTracker.Data;
using CalorieTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CalorieTracker.Controllers;

[Authorize]
public class DiaryController : Controller
{
    private readonly AppDbContext _db;

    public DiaryController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index(DateTime? date)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var selectedDate = date?.Date ?? DateTime.Today;

        var entries = await _db.DiaryEntries
            .Where(e => e.UserId == userId && e.Date == selectedDate)
            .OrderBy(e => e.MealType).ThenBy(e => e.CreatedAt)
            .ToListAsync();

        var goal = await _db.DailyGoals
            .FirstOrDefaultAsync(g => g.UserId == userId)
            ?? new DailyGoal { UserId = userId };

        var vm = new DiaryViewModel
        {
            SelectedDate = selectedDate,
            Entries = entries,
            Goal = goal,
            PreviousDate = selectedDate.AddDays(-1),
            NextDate = selectedDate.AddDays(1),
        };

        return View(vm);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var entry = await _db.DiaryEntries
            .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

        if (entry is null) return NotFound();

        _db.DiaryEntries.Remove(entry);
        await _db.SaveChangesAsync();
        return Ok();
    }
}