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

    public HomeController(ILogger<HomeController> logger, IFoodApiClient foodApi)
    {
        _logger = logger;
        _foodApi = foodApi;
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

}
