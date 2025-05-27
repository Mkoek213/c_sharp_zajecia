using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FootballApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace FootballApp.Controllers
{
    [Authorize]
    public class MatchesController : Controller
    {
        private readonly FootballLeagueContext _context;

        public MatchesController(FootballLeagueContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewData["MenuTitle"] = "Matches";
            var matches = _context.Mecze.Include(m => m.DruzynaDomowa).Include(m => m.DruzynaGości).ToList();
            return View(matches);
        }

    }
}
