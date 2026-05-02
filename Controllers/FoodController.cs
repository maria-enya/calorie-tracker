using CalorieTracker.Data;
using CalorieTracker.Services;
using CalorieTracker.Services.FoodApi;
using Microsoft.AspNetCore.Mvc;

namespace CalorieTracker.Controllers;

public class FoodController : Controller
{
    private readonly IFoodApiClient _foodApi;
    private readonly AppDbContext _db;

    public FoodController(IFoodApiClient foodApi, AppDbContext db)
    {
        _foodApi = foodApi;
        _db = db;
    }

    // GET /food/search
    public IActionResult Search()
    {
        return View();
    }

    // GET /food/results?query=banana  (called by htmx)
    public async Task<IActionResult> Results(string query)
    {
        if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
            return PartialView("_NoResults");

        var products = await _foodApi.SearchAsync(query, pageSize: 12);
        return PartialView("_SearchResults", products);
    }

    [HttpPost]
    public async Task<IActionResult> Add(
    string barcode,
    string foodName,
    double grams,
    string mealType,
    DateTime date)
    {
        var product = await _foodApi.GetByBarcodeAsync(barcode);

        if (product is null)
            product = new FoodProduct { ProductName = foodName, Barcode = barcode };

        var entry = NutritionCalculator.BuildEntry(product, grams, mealType, date);
        _db.DiaryEntries.Add(entry);
        await _db.SaveChangesAsync();

        // Return success message — htmx swaps the button
        return Content("<span class='text-green-600 font-medium text-sm'>✓ Added!</span>", "text/html");
    }
}