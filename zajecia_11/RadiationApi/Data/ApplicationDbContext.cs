using Microsoft.EntityFrameworkCore;
using RadiationApi.Models;

namespace RadiationApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Reprezentuje tabelę 'Measurements' w bazie danych
        public DbSet<Measurement> Measurements { get; set; }
    }
}