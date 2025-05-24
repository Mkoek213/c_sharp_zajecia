using Microsoft.EntityFrameworkCore;

public class FootballLeagueContext : DbContext
{
    public FootballLeagueContext(DbContextOptions<FootballLeagueContext> options) : base(options) { }

    public DbSet<Drużyna> Druzyny { get; set; }
    public DbSet<Zawodnik> Zawodnicy { get; set; }
    public DbSet<Mecz> Mecze { get; set; }
    public DbSet<StatystykiZawodnika> StatystykiZawodników { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Relacje mecze - drużyny
        modelBuilder.Entity<Mecz>()
            .HasOne(m => m.DrużynaDomowa)
            .WithMany(d => d.MeczeDomowe)
            .HasForeignKey(m => m.DrużynaDomowaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Mecz>()
            .HasOne(m => m.DrużynaGości)
            .WithMany(d => d.MeczeGości)
            .HasForeignKey(m => m.DrużynaGościId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relacja zawodnik-statystyki
        modelBuilder.Entity<Zawodnik>()
            .HasOne(z => z.Statystyki)
            .WithOne(s => s.Zawodnik)
            .HasForeignKey<StatystykiZawodnika>(s => s.ZawodnikId);

        base.OnModelCreating(modelBuilder);
    }
}
