using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FootballApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering; // Required for SelectList

namespace FootballApp.Controllers
{
    [Authorize] // Assuming matches also require authorization
    public class MatchesController : Controller
    {
        private readonly FootballLeagueContext _context;
        // private readonly ILogger<MatchesController> _logger; // Optional: for logging

        public MatchesController(FootballLeagueContext context /*, ILogger<MatchesController> logger */)
        {
            _context = context;
            // _logger = logger;
        }

        // GET: Matches
        public async Task<IActionResult> Index()
        {
            ViewData["MenuTitle"] = "Matches";
            var matches = await _context.Mecze
                                    .Include(m => m.DruzynaDomowa)
                                    .Include(m => m.DruzynaGości)
                                    .OrderByDescending(m => m.Data) // Optional: order by date
                                    .ToListAsync();
            return View(matches);
        }

        // GET: Matches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mecz = await _context.Mecze
                .Include(m => m.DruzynaDomowa)
                .Include(m => m.DruzynaGości)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mecz == null)
            {
                return NotFound();
            }
            ViewData["MenuTitle"] = "Match Details";
            return View(mecz);
        }

        // GET: Matches/Create
        public IActionResult Create()
        {
            ViewData["MenuTitle"] = "Schedule New Match";
            // Populate dropdowns for teams
            ViewData["DruzynaDomowaId"] = new SelectList(_context.Druzyny.OrderBy(d => d.Nazwa), "Id", "Nazwa");
            ViewData["DruzynaGościId"] = new SelectList(_context.Druzyny.OrderBy(d => d.Nazwa), "Id", "Nazwa");
            return View(new Mecz { Data = DateTime.Today }); // Default to today's date
        }

        // POST: Matches/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Data,DruzynaDomowaId,DruzynaGościId,WynikDomowy,WynikGości")] Mecz mecz)
        {
            if (mecz.DruzynaDomowaId == mecz.DruzynaGościId)
            {
                ModelState.AddModelError("DruzynaGościId", "Home team and Away team cannot be the same.");
            }

            // Ensure Wynik values are null if submitted as empty strings,
            // though model binding for int? usually handles this.
            // No explicit handling needed here if model types are correct.

            if (ModelState.IsValid)
            {
                _context.Add(mecz);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["MenuTitle"] = "Schedule New Match";
            ViewData["DruzynaDomowaId"] = new SelectList(_context.Druzyny.OrderBy(d => d.Nazwa), "Id", "Nazwa", mecz.DruzynaDomowaId);
            ViewData["DruzynaGościId"] = new SelectList(_context.Druzyny.OrderBy(d => d.Nazwa), "Id", "Nazwa", mecz.DruzynaGościId);
            return View(mecz);
        }

        // GET: Matches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mecz = await _context.Mecze.FindAsync(id);
            if (mecz == null)
            {
                return NotFound();
            }
            ViewData["MenuTitle"] = "Edit Match";
            ViewData["DruzynaDomowaId"] = new SelectList(_context.Druzyny.OrderBy(d => d.Nazwa), "Id", "Nazwa", mecz.DruzynaDomowaId);
            ViewData["DruzynaGościId"] = new SelectList(_context.Druzyny.OrderBy(d => d.Nazwa), "Id", "Nazwa", mecz.DruzynaGościId);
            return View(mecz);
        }

        // POST: Matches/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Data,DruzynaDomowaId,DruzynaGościId,WynikDomowy,WynikGości")] Mecz mecz)
        {
            if (id != mecz.Id)
            {
                return NotFound();
            }

            if (mecz.DruzynaDomowaId == mecz.DruzynaGościId)
            {
                ModelState.AddModelError("DruzynaGościId", "Home team and Away team cannot be the same.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mecz);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeczExists(mecz.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MenuTitle"] = "Edit Match";
            ViewData["DruzynaDomowaId"] = new SelectList(_context.Druzyny.OrderBy(d => d.Nazwa), "Id", "Nazwa", mecz.DruzynaDomowaId);
            ViewData["DruzynaGościId"] = new SelectList(_context.Druzyny.OrderBy(d => d.Nazwa), "Id", "Nazwa", mecz.DruzynaGościId);
            return View(mecz);
        }

        // GET: Matches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mecz = await _context.Mecze
                .Include(m => m.DruzynaDomowa)
                .Include(m => m.DruzynaGości)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mecz == null)
            {
                return NotFound();
            }
            ViewData["MenuTitle"] = "Delete Match";
            return View(mecz);
        }

        // POST: Matches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mecz = await _context.Mecze.FindAsync(id);
            if (mecz != null)
            {
                _context.Mecze.Remove(mecz);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool MeczExists(int id)
        {
            return _context.Mecze.Any(e => e.Id == id);
        }
    }
}