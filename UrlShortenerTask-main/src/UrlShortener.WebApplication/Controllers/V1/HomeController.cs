using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.WebApplication.Controllers.V1;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult TechTaskDetails()
    {
        return View("TechTaskDetails");
    }
}