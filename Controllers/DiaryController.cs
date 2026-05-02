using CalorieTracker.Data;
using CalorieTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CalorieTracker.Controllers;

public class DiaryController : Controller
{
    private readonly AppDbContext _db;

    public DiaryController(AppDbContext db)
    {
        _db = db;
    }

    // GET /diary?date=2026-05-01
    public async Task<IActionResult> Index(DateTime? date)
    {
        var selectedDate = date?.Date ?? DateTime.Today;

        var entries = await _db.DiaryEntries
            .Where(e => e.Date == selectedDate)
            .OrderBy(e => e.MealType)
            .ThenBy(e => e.CreatedAt)
            .ToListAsync();

        var goal = await _db.DailyGoals.FirstOrDefaultAsync() ?? new DailyGoal();

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

    // DELETE /diary/delete/5  (called by htmx)
    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var entry = await _db.DiaryEntries.FindAsync(id);
        if (entry is null) return NotFound();

        _db.DiaryEntries.Remove(entry);
        await _db.SaveChangesAsync();

        // htmx will remove the element from the DOM
        return Ok();
    }
}