using Microsoft.AspNetCore.Mvc;

namespace HealthcareApi.Controllers;

public class TestController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}