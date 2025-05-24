using Microsoft.EntityFrameworkCore;
using FootballApp.Models;

public class FootballLeagueContext : DbContext
{
    public FootballLeagueContext(DbContextOptions<FootballLeagueContext> options) : base(options) { }

    public DbSet<Druzyna> Druzyny { get; set; }
    public DbSet<Zawodnik> Zawodnicy { get; set; }
    public DbSet<Mecz> Mecze { get; set; }
    public DbSet<StatystykiZawodnika> StatystykiZawodnikow { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Mecz>()
            .HasOne(m => m.DruzynaDomowa)
            .WithMany(d => d.MeczeDomowe)
            .HasForeignKey(m => m.DruzynaDomowaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Mecz>()
            .HasOne(m => m.DruzynaGości)
            .WithMany(d => d.MeczeGości)
            .HasForeignKey(m => m.DruzynaGościId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Zawodnik>()
            .HasOne(z => z.Statystyki)
            .WithOne(s => s.Zawodnik)
            .HasForeignKey<StatystykiZawodnika>(s => s.ZawodnikId);

        base.OnModelCreating(modelBuilder);
    }
}
