using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FootballApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballApp.Controllers;

[Authorize]
public class StatsController : Controller
{
    private readonly FootballLeagueContext _context;

    public StatsController(FootballLeagueContext context)
    {
        _context = context;
    }

    public IActionResult Index()
        {
            ViewData["MenuTitle"] = "Player Stats";
            var stats = _context.StatystykiZawodnikow.Include(s => s.Zawodnik).ToList();
            return View(stats);
        }

}
