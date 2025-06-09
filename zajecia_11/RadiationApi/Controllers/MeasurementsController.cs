using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RadiationApi.Data;
using RadiationApi.Models;

namespace RadiationApi.Controllers
{
    [Route("api/[controller]")] // Adres bazowy: /api/measurements
    [ApiController]
    public class MeasurementsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // Wstrzykujemy kontekst bazy danych przez konstruktor
        public MeasurementsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/measurements
        // Pobiera wszystkie rekordy
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Measurement>>> GetMeasurements()
        {
            return await _context.Measurements.ToListAsync();
        }

        // GET: api/measurements/5
        // Pobiera jeden rekord o podanym ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Measurement>> GetMeasurement(int id)
        {
            var measurement = await _context.Measurements.FindAsync(id);

            if (measurement == null)
            {
                return NotFound(); // Zwraca błąd 404, jeśli nie znaleziono
            }

            return measurement;
        }

        // POST: api/measurements
        // Tworzy nowy rekord
        [HttpPost]
        public async Task<ActionResult<Measurement>> PostMeasurement(Measurement measurement)
        {
            // Ustawiamy Id na 0, aby baza danych nadała nową wartość
            measurement.Id = 0; 
            _context.Measurements.Add(measurement);
            await _context.SaveChangesAsync();

            // Zwraca status 201 Created z lokalizacją nowego zasobu
            return CreatedAtAction(nameof(GetMeasurement), new { id = measurement.Id }, measurement);
        }

        // PUT: api/measurements/5
        // Aktualizuje istniejący rekord
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMeasurement(int id, Measurement measurement)
        {
            if (id != measurement.Id)
            {
                return BadRequest("ID w URL nie zgadza się z ID w ciele żądania.");
            }

            _context.Entry(measurement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeasurementExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Zwraca status 204 No Content po sukcesie
        }

        // DELETE: api/measurements/5
        // Usuwa rekord o podanym ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeasurement(int id)
        {
            var measurement = await _context.Measurements.FindAsync(id);
            if (measurement == null)
            {
                return NotFound();
            }

            _context.Measurements.Remove(measurement);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MeasurementExists(int id)
        {
            return _context.Measurements.Any(e => e.Id == id);
        }
    }
}