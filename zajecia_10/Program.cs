using LoginApp.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Dodaj EF Core z SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

builder.Services.AddControllersWithViews();
builder.Services.AddSession();

var app = builder.Build();

// Przy starcie utwórz bazę i tabele, zasiej dane
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/IO/Logowanie");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=IO}/{action=Logowanie}/{id?}");

app.Run();
