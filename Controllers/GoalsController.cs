using CalorieTracker.Data;
using CalorieTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CalorieTracker.Controllers;

public class GoalsController : Controller
{
    private readonly AppDbContext _db;

    public GoalsController(AppDbContext db)
    {
        _db = db;
    }

    // GET /goals
    public async Task<IActionResult> Index()
    {
        var goal = await _db.DailyGoals.FirstOrDefaultAsync() ?? new DailyGoal();
        return View(goal);
    }

    // POST /goals
    [HttpPost]
    public async Task<IActionResult> Index(DailyGoal model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var existing = await _db.DailyGoals.FirstOrDefaultAsync();

        if (existing is null)
        {
            model.UpdatedAt = DateTime.UtcNow;
            _db.DailyGoals.Add(model);
        }
        else
        {
            existing.CalorieTarget = model.CalorieTarget;
            existing.ProteinTargetG = model.ProteinTargetG;
            existing.CarbsTargetG = model.CarbsTargetG;
            existing.FatTargetG = model.FatTargetG;
            existing.FiberTargetG = model.FiberTargetG;
            existing.UpdatedAt = DateTime.UtcNow;
        }

        await _db.SaveChangesAsync();

        TempData["Success"] = "Goals saved successfully!";
        return RedirectToAction(nameof(Index));
    }
}