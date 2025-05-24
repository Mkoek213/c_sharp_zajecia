using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FootballApp.Models;
using System.Linq;

namespace FootballApp.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly FootballLeagueContext _context;

    public HomeController(ILogger<HomeController> logger, FootballLeagueContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        // Example: fetch teams and pass to view
        var teams = _context.Druzyny.ToList();
        return View(teams);
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
