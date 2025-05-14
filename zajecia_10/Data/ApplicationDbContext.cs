using Microsoft.EntityFrameworkCore;
using LoginApp.Models;

namespace LoginApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Login> Loginy { get; set; }
        public DbSet<Dane> Dane { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed danych
            modelBuilder.Entity<Login>().HasData(
                new Login { Id = 1, Username = "admin", PasswordHash = "1234" },
                new Login { Id = 2, Username = "user",  PasswordHash = "pass" }
            );
            modelBuilder.Entity<Dane>().HasData(
                new Dane { Id = 1, Text = "Przykładowy wpis 1" },
                new Dane { Id = 2, Text = "Przykładowy wpis 2" }
            );
        }
    }
}
