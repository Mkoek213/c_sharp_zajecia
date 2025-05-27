using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FootballApp.Models;
using System.Linq;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;


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
        ViewData["MenuTitle"] = "Teams";
        var teams = _context.Druzyny.ToList();
        return View(teams);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Druzyna team)
    {
        if (ModelState.IsValid)
        {
            _context.Druzyny.Add(team);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(team);
    }

    public IActionResult Edit(int id)
    {
        var team = _context.Druzyny.Find(id);
        if (team == null)
            return NotFound();

        return View(team);
    }

    [HttpPost]
    public IActionResult Edit(int id, Druzyna updatedTeam)
    {
        if (id != updatedTeam.Id)
            return BadRequest();

        if (ModelState.IsValid)
        {
            _context.Update(updatedTeam);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        return View(updatedTeam);
    }

    public IActionResult Delete(int id)
    {
        var team = _context.Druzyny.Find(id);
        if (team == null)
            return NotFound();

        return View(team);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        var team = _context.Druzyny
            .Include(t => t.Zawodnicy)
            .FirstOrDefault(t => t.Id == id);

        if (team == null)
            return NotFound();

        // Set teamless players to "Free"
        foreach (var player in team.Zawodnicy)
        {
            player.DruzynaId = null;
            // Optionally add a "Status" field like player.Status = "Free Agent";
        }

        _context.Druzyny.Remove(team);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

}

