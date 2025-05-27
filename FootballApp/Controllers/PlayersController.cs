using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FootballApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace FootballApp.Controllers
{
    [Authorize]
    public class PlayersController : Controller
    {
        private readonly FootballLeagueContext _context;

        public PlayersController(FootballLeagueContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewData["MenuTitle"] = "Players";
            var players = _context.Zawodnicy.Include(p => p.Druzyna).ToList();
            return View(players);
        }

    }
}
