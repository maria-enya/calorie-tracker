using CalorieTracker.Models;
using CalorieTracker.Services.FoodApi;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static System.Net.WebRequestMethods;

namespace CalorieTracker.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IFoodApiClient _foodApi;
    private readonly HttpClient _http;

    public HomeController(ILogger<HomeController> logger, IFoodApiClient foodApi)
    {
        _logger = logger;
        _foodApi = foodApi;
        _http = new HttpClient { BaseAddress = new Uri("https://world.openfoodfacts.org/") };
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // Temporary test action - delete after verifying
    public async Task<IActionResult> TestApi()
    {
        try
        {
            var url = "cgi/search.pl?search_terms=banana" +
                      "&search_simple=1&action=process&json=1" +
                      "&page_size=5" +
                      "&fields=product_name,brands,code,nutriments";

            var response = await _http.GetAsync(url);  // _http is the raw HttpClient

            var statusCode = response.StatusCode;
            var body = await response.Content.ReadAsStringAsync();

            return Json(new
            {
                StatusCode = statusCode.ToString(),
                BodyPreview = body.Length > 500 ? body[..500] : body
            });
        }
        catch (Exception ex)
        {
            return Json(new { Error = ex.Message });
        }
    }
}
