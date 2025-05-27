using Microsoft.AspNetCore.Authentication.Cookies;
using FootballApp.Models;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<FootballLeagueContext>(options =>
    options.UseSqlite("Data Source=footballleague.db"));

// Add cookie authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add these middlewares
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FootballLeagueContext>();

    context.Database.EnsureCreated();

    if (!context.Druzyny.Any())
    {
        var json = File.ReadAllText("Data/seed_druzyny.json");
        var druzyny = System.Text.Json.JsonSerializer.Deserialize<List<Druzyna>>(json);
        context.Druzyny.AddRange(druzyny);
        context.SaveChanges();
    }

    if (!context.Zawodnicy.Any())
    {
        var json = File.ReadAllText("Data/seed_zawodnicy.json");
        var zawodnicy = System.Text.Json.JsonSerializer.Deserialize<List<Zawodnik>>(json);
        context.Zawodnicy.AddRange(zawodnicy);
        context.SaveChanges();
    }

    if (!context.Mecze.Any())
    {
        var json = File.ReadAllText("Data/seed_mecze.json");
        var mecze = System.Text.Json.JsonSerializer.Deserialize<List<Mecz>>(json);
        context.Mecze.AddRange(mecze);
        context.SaveChanges();
    }

    if (!context.StatystykiZawodnikow.Any())
    {
        var json = File.ReadAllText("Data/seed_statystyki.json");
        var statystyki = System.Text.Json.JsonSerializer.Deserialize<List<StatystykiZawodnika>>(json);
        context.StatystykiZawodnikow.AddRange(statystyki);
        context.SaveChanges();
    }
}

app.Run();
