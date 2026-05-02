using CalorieTracker.Data;
using CalorieTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CalorieTracker.Controllers;

[Authorize]
public class GoalsController : Controller
{
    private readonly AppDbContext _db;

    public GoalsController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var goal = await _db.DailyGoals.FirstOrDefaultAsync(g => g.UserId == userId)
            ?? new DailyGoal { UserId = userId };
        return View(goal);
    }

    [HttpPost]
    public async Task<IActionResult> Index(DailyGoal model)
    {
        if (!ModelState.IsValid) return View(model);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var existing = await _db.DailyGoals.FirstOrDefaultAsync(g => g.UserId == userId);

        if (existing is null)
        {
            model.UserId = userId;
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