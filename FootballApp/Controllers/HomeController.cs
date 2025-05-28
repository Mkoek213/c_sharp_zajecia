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

            // Redirect to AddPlayers action with the newly created team id
            return RedirectToAction(nameof(AddPlayers), new { teamId = team.Id });
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

    // GET: show AddPlayers form
    [HttpGet]
    public IActionResult AddPlayers(int teamId)
    {
        var team = _context.Druzyny.Find(teamId);
        if (team == null) return NotFound();

        ViewData["TeamName"] = team.Nazwa;
        ViewData["TeamId"]   = team.Id;
        ViewData["Players"]  = _context.Zawodnicy.Where(p => p.DruzynaId == teamId).ToList();

        // Pass an empty Zawodnik for the form
        return View(new Zawodnik());
    }

    [HttpPost]
    public IActionResult AddPlayers(int teamId, Zawodnik player, string submitAction)
    {
        var team = _context.Druzyny.Find(teamId);
        if (team == null) return NotFound();

        if (submitAction == "Add")
        {
            if (ModelState.IsValid) 
            {
                player.DruzynaId = teamId;
                _context.Zawodnicy.Add(player);
                _context.SaveChanges();
                _logger.LogInformation("Successfully added player {PlayerName} to team {TeamId}", player.Nazwisko, teamId); 

                ViewData["TeamName"] = team.Nazwa;
                ViewData["TeamId"] = team.Id;
                ViewData["Players"] = _context.Zawodnicy
                    .Include(z => z.Statystyki)
                    .Where(p => p.DruzynaId == teamId)
                    .ToList();
                return View(new Zawodnik { Statystyki = new StatystykiZawodnika() }); 
            }
            else
            {
                _logger.LogWarning("Invalid player model when trying to add to team {TeamId}. Errors: {errors}",
                    teamId,
                    string.Join("; ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)));
                _logger.LogDebug("Invalid Player data received: Imie='{Imie}', Nazwisko='{Nazwisko}', Pozycja='{Pozycja}', Mecze='{Mecze}', Gole='{Gole}', Asysty='{Asysty}'",
                    player.Imie, player.Nazwisko, player.Pozycja, player.Statystyki?.Mecze, player.Statystyki?.Gole, player.Statystyki?.Asysty);               ViewData["TeamName"] = team.Nazwa;
                ViewData["TeamId"] = team.Id;
                ViewData["Players"] = _context.Zawodnicy
                    .Include(z => z.Statystyki)
                    .Where(p => p.DruzynaId == teamId)
                    .ToList();
                return View(player); 
            }
        }

        if (submitAction == "Done")
        {
            return RedirectToAction(nameof(AddMatches), new { teamId });
        }

        ViewData["TeamName"] = team.Nazwa;
        ViewData["TeamId"] = team.Id;
        ViewData["Players"] = _context.Zawodnicy
            .Include(z => z.Statystyki)
            .Where(p => p.DruzynaId == teamId)
            .ToList();

        return View(new Zawodnik { Statystyki = new StatystykiZawodnika() });
    }





    // GET: show the AddMatches form
    [HttpGet]
    public IActionResult AddMatches(int teamId)
    {
        var team = _context.Druzyny.Find(teamId);
        if (team == null) return NotFound();

        ViewData["TeamName"] = team.Nazwa;
        ViewData["TeamId"]   = team.Id;

        // Existing matches for this team
        ViewData["Matches"] = _context.Mecze
            .Include(m => m.DruzynaDomowa)
            .Include(m => m.DruzynaGości)
            .Where(m => m.DruzynaDomowaId == team.Id || m.DruzynaGościId == team.Id)
            .ToList();

        // Other teams for the opponent dropdown
        ViewData["Teams"] = _context.Druzyny.Where(t => t.Id != teamId).ToList();

        // Pass an empty Mecz as the form model
        return View(new Mecz { DruzynaDomowaId = teamId });
    }

    // POST: handle form submissions
    [HttpPost]
    public IActionResult AddMatches(Mecz match)
    {
        if (ModelState.IsValid)
        {
            _context.Mecze.Add(match);
            _context.SaveChanges();
            return RedirectToAction(nameof(AddMatches), new { teamId = match.DruzynaDomowaId });
        }
        else
        {
            _logger.LogWarning("Invalid match model: {errors}", string.Join("; ", ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)));

        }
        // On error, fall through to GET logic to repopulate ViewData...
        return AddMatches(match.DruzynaDomowaId);
    }




}

