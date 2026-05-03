using System.Security.Claims;
using CalorieTracker.Data;
using CalorieTracker.Models;
using CalorieTracker.Services;
using CalorieTracker.Services.FoodApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalorieTracker.Controllers;

[Authorize]
public class FoodController : Controller
{
    private readonly IFoodApiClient _foodApi;
    private readonly CombinedFoodSearchService _search;
    private readonly AppDbContext _db;
    private readonly ILogger<FoodController> _logger;

    public FoodController(
        IFoodApiClient foodApi,
        CombinedFoodSearchService search,
        AppDbContext db,
        ILogger<FoodController> logger)
    {
        _foodApi = foodApi;
        _search = search;
        _db = db;
        _logger = logger;
    }

    // GET /food/search
    public IActionResult Search() => View();

    // GET /food/results?query=banana  (htmx)
    public async Task<IActionResult> Results(string query)
    {
        if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
            return PartialView("_NoResults");

        var results = await _search.SearchAsync(query);
        return PartialView("_SearchResults", results);
    }

    // POST /food/add  (from API search results)
    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> Add(
    string barcode, string foodName,
    double grams, string mealType, DateTime date,
    double caloriesPer100g = 0,
    double proteinPer100g = 0,
    double carbsPer100g = 0,
    double fatPer100g = 0,
    double fiberPer100g = 0,
    double sugarPer100g = 0)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        FoodProduct? product = null;

        if (caloriesPer100g > 0 || proteinPer100g > 0)
        {
            product = new FoodProduct
            {
                ProductName = foodName,
                Barcode = barcode,
                Nutriments = new Nutriments
                {
                    CaloriesPer100g = caloriesPer100g,
                    ProteinPer100g = proteinPer100g,
                    CarbsPer100g = carbsPer100g,
                    FatPer100g = fatPer100g,
                    FiberPer100g = fiberPer100g,
                    SugarPer100g = sugarPer100g,
                }
            };
        }

        if (product is null && !barcode.StartsWith("usda-"))
            product = await _foodApi.GetByBarcodeAsync(barcode);

        product ??= new FoodProduct { ProductName = foodName, Barcode = barcode };

        var entry = NutritionCalculator.BuildEntry(product, grams, mealType, date);
        entry.UserId = userId;

        _db.DiaryEntries.Add(entry);
        await _db.SaveChangesAsync();

        return Content(
            "<span class='text-green-600 font-medium text-sm'>✓ Added!</span>",
            "text/html");
    }

    // GET /food/manual
    public IActionResult Manual() => View(new ManualFoodViewModel
    {
        Date = DateTime.Today
    });

    // POST /food/manual
    [HttpPost]
    public async Task<IActionResult> Manual(ManualFoodViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var entry = new DiaryEntry
        {
            UserId = userId,
            Date = model.Date.Date,
            MealType = model.MealType,
            FoodName = model.FoodName,
            QuantityGrams = model.Grams,
            Calories = model.Calories,
            ProteinG = model.ProteinG,
            CarbsG = model.CarbsG,
            FatG = model.FatG,
            FiberG = model.FiberG,
        };

        _db.DiaryEntries.Add(entry);
        await _db.SaveChangesAsync();

        TempData["Success"] = $"{model.FoodName} added to diary!";
        return RedirectToAction("Search");
    }
}