using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FootballApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

// File: Controllers/PlayersController.cs
// ... (usings remain the same) ...

namespace FootballApp.Controllers
{
    [Authorize]
    public class PlayersController : Controller
    {
        private readonly FootballLeagueContext _context;
        private readonly ILogger<PlayersController> _logger;

        public PlayersController(FootballLeagueContext context, ILogger<PlayersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: /Players/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["MenuTitle"] = "Create New Player";
            ViewData["DruzynaId"] = new SelectList(_context.Druzyny, "Id", "Nazwa");
            // Zawodnik's constructor now initializes Statystyki
            var zawodnik = new Zawodnik(); 
            _logger.LogInformation("GET Create: Initialized new Zawodnik (Statystyki initialized by its constructor).");
            return View(zawodnik);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Imie,Nazwisko,Pozycja,DruzynaId,Statystyki")] Zawodnik zawodnik) // Changed here
        {
            // ... rest of your existing Create POST method ...
            // The logging you have is still very valuable.
            _logger.LogInformation("CREATE POST - Immediately after model binding: Zawodnik.Statystyki: Mecze={Mecze}, Gole={Gole}, Asysty={Asysty}. Is Statystyki object null? {IsNull}",
                zawodnik.Statystyki.Mecze, 
                zawodnik.Statystyki.Gole,
                zawodnik.Statystyki.Asysty,
                zawodnik.Statystyki == null ? "Yes" : "No"); 

            if (zawodnik.DruzynaId == null)
            {
                ModelState.Remove("Druzyna");
            }

            if (ModelState.IsValid)
            {
                _logger.LogInformation("CREATE POST - Before SaveChanges (ModelState is Valid): Zawodnik.Statystyki: Mecze={Mecze}, Gole={Gole}, Asysty={Asysty}",
                    zawodnik.Statystyki.Mecze, zawodnik.Statystyki.Gole, zawodnik.Statystyki.Asysty);

                _context.Add(zawodnik);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Player {Imie} {Nazwisko} created successfully with ID {PlayerId}. Final Stats in object: Mecze={Mecze}, Gole={Gole}, Asysty={Asysty}",
                    zawodnik.Imie, zawodnik.Nazwisko, zawodnik.Id,
                    zawodnik.Statystyki.Mecze, zawodnik.Statystyki.Gole, zawodnik.Statystyki.Asysty);
                
                return RedirectToAction(nameof(Index));
            }

            _logger.LogWarning("Create POST: Invalid ModelState for new player. Errors: {Errors}", string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            
            _logger.LogInformation("Create POST (Invalid ModelState): Returning view with Statystyki: Mecze={Mecze}, Gole={Gole}, Asysty={Asysty}",
                zawodnik.Statystyki.Mecze, zawodnik.Statystyki.Gole, zawodnik.Statystyki.Asysty);

            ViewData["DruzynaId"] = new SelectList(_context.Druzyny, "Id", "Nazwa", zawodnik.DruzynaId);
            ViewData["MenuTitle"] = "Create New Player";
            return View(zawodnik);
        }

        // GET: Players/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Edit GET: ID is null.");
                return NotFound();
            }

            var zawodnikFromDb = await _context.Zawodnicy
                                     .Include(z => z.Statystyki) 
                                     .FirstOrDefaultAsync(z => z.Id == id.Value);

            if (zawodnikFromDb == null)
            {
                _logger.LogWarning("Edit GET: Zawodnik with ID {Id} not found for editing.", id);
                return NotFound();
            }

            if (zawodnikFromDb.Statystyki == null)
            {
                _logger.LogInformation("Edit GET: Zawodnik ID {Id} has NULL Statystyki from DB. Initializing for form.", id);
                zawodnikFromDb.Statystyki = new StatystykiZawodnika(); 
            }

            ViewData["DruzynaId"] = new SelectList(_context.Druzyny, "Id", "Nazwa", zawodnikFromDb.DruzynaId);
            ViewData["MenuTitle"] = "Edit Player";
            return View(zawodnikFromDb); 
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,Imie,Nazwisko,Pozycja,DruzynaId")] Zawodnik zawodnikViewModel, 
            [Bind(Prefix = "Statystyki")] StatystykiZawodnika statystykiFromForm) // Bind stats separately
        {
            if (id != zawodnikViewModel.Id) {
                _logger.LogWarning("Edit POST: ID mismatch. Route ID: {RouteId}, ViewModel ID: {ViewModelId}", id, zawodnikViewModel.Id);
                return NotFound();
            }

            _logger.LogInformation("EDIT POST - statystykiFromForm (separate param AFTER BINDING): Id={StatId}, M={Mecze}, G={Gole}, A={Asysty}",
                statystykiFromForm.Id, statystykiFromForm.Mecze,
                statystykiFromForm.Gole, statystykiFromForm.Asysty);

            // Update zawodnikViewModel.Statystyki with values from statystykiFromForm
            // This makes zawodnikViewModel consistent if returned to the view on error.
            // Zawodnik constructor initializes zawodnikViewModel.Statystyki, so it's not null.
            zawodnikViewModel.Statystyki.Id = statystykiFromForm.Id;
            zawodnikViewModel.Statystyki.Mecze = statystykiFromForm.Mecze;
            zawodnikViewModel.Statystyki.Gole = statystykiFromForm.Gole;
            zawodnikViewModel.Statystyki.Asysty = statystykiFromForm.Asysty;
            // ZawodnikId for Statystyki is not directly bound from form for statystykiFromForm; it's contextual.

            if (zawodnikViewModel.DruzynaId == null) { 
                ModelState.Remove("Druzyna"); 
            }

            if (ModelState.IsValid) // Validates both zawodnikViewModel (its bound props) and statystykiFromForm
            {
                var zawodnikToUpdate = await _context.Zawodnicy
                                                 .Include(z => z.Statystyki)
                                                 .FirstOrDefaultAsync(z => z.Id == id);
                
                if (zawodnikToUpdate == null) {
                     _logger.LogWarning("Edit POST: Zawodnik to update with ID {Id} not found in DB.", id);
                    return NotFound();
                }

                zawodnikToUpdate.Imie = zawodnikViewModel.Imie;
                zawodnikToUpdate.Nazwisko = zawodnikViewModel.Nazwisko;
                zawodnikToUpdate.Pozycja = zawodnikViewModel.Pozycja;
                zawodnikToUpdate.DruzynaId = zawodnikViewModel.DruzynaId;

                if (zawodnikToUpdate.Statystyki != null) 
                {
                    _logger.LogInformation("EDIT POST - Updating existing DB stats (ID: {DbStatId}) for Zawodnik ID {ZawId} with Form Stats (ID: {FormStatId}) M:{M}, G:{G}, A:{A}",
                                        zawodnikToUpdate.Statystyki.Id, zawodnikToUpdate.Id, statystykiFromForm.Id,
                                        statystykiFromForm.Mecze, statystykiFromForm.Gole, statystykiFromForm.Asysty);

                    zawodnikToUpdate.Statystyki.Mecze = statystykiFromForm.Mecze;
                    zawodnikToUpdate.Statystyki.Gole = statystykiFromForm.Gole;
                    zawodnikToUpdate.Statystyki.Asysty = statystykiFromForm.Asysty;
                }
                else 
                {
                    _logger.LogInformation("EDIT POST - No existing stats for Zawodnik ID {ZawId}. Creating new stats with Form Stats M:{M}, G:{G}, A:{A}",
                                        zawodnikToUpdate.Id, statystykiFromForm.Mecze, statystykiFromForm.Gole, statystykiFromForm.Asysty);
                    zawodnikToUpdate.Statystyki = new StatystykiZawodnika
                    {
                        Mecze = statystykiFromForm.Mecze,
                        Gole = statystykiFromForm.Gole,
                        Asysty = statystykiFromForm.Asysty,
                        ZawodnikId = zawodnikToUpdate.Id 
                    };
                }

                try { 
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Player with ID {Id} updated successfully.", zawodnikToUpdate.Id);
                } 
                catch (DbUpdateException ex) { /* ... (your existing error handling) ... */ }

                if (ModelState.IsValid) { return RedirectToAction(nameof(Index)); }
            }

            _logger.LogWarning("Edit POST: Invalid ModelState for Zawodnik ID {Id}. Errors: {Errors}", id, string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            
            // zawodnikViewModel.Statystyki has been updated from statystykiFromForm earlier.
            ViewData["DruzynaId"] = new SelectList(_context.Druzyny, "Id", "Nazwa", zawodnikViewModel.DruzynaId);
            ViewData["MenuTitle"] = "Edit Player";
            return View(zawodnikViewModel);
        }

        public async Task<IActionResult> Index()
        {
            ViewData["MenuTitle"] = "Players";
            var players = await _context.Zawodnicy
                .Include(p => p.Druzyna)
                .Include(p => p.Statystyki)
                .ToListAsync();
            return View(players);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Delete GET: ID is null.");
                return NotFound();
            }

            var zawodnik = await _context.Zawodnicy
                .Include(z => z.Druzyna) 
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zawodnik == null)
            {
                _logger.LogWarning("Delete GET: Zawodnik with ID {Id} not found for deletion.", id);
                return NotFound();
            }
            ViewData["MenuTitle"] = "Delete Player";
            return View(zawodnik);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zawodnik = await _context.Zawodnicy.FindAsync(id);
            if (zawodnik != null)
            {
                _context.Zawodnicy.Remove(zawodnik); 
                await _context.SaveChangesAsync();
                _logger.LogInformation("Zawodnik with ID {Id} deleted successfully.", id);
            }
            else
            {
                _logger.LogWarning("DeleteConfirmed POST: Zawodnik with ID {Id} not found.", id);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ZawodnikExists(int id)
        {
            return _context.Zawodnicy.Any(e => e.Id == id);
        }

    }
}