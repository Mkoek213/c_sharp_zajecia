using Microsoft.EntityFrameworkCore;
using RadiationApi.Data;

var builder = WebApplication.CreateBuilder(args);

// --- Konfiguracja usług ---

// 1. Rejestracja kontekstu bazy danych SQLite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=radiation.db";
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// 2. Dodanie kontrolerów API
builder.Services.AddControllers();

// 3. Konfiguracja Swaggera do dokumentacji i testowania API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
    { 
        Title = "Radiation Measurements API", 
        Version = "v1" 
    });
});

// --- Budowanie aplikacji ---
var app = builder.Build();

// --- Konfiguracja potoku HTTP ---

// Zawsze używaj Swaggera w środowisku deweloperskim
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Radiation API V1");
        c.RoutePrefix = string.Empty; // Swagger dostępny pod adresem głównym
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// --- Import danych na starcie aplikacji ---
try
{
    DataSeeder.Seed(app);
}
catch (Exception ex)
{
    Console.WriteLine($"Wystąpił błąd podczas importu danych: {ex.Message}");
}


// --- Uruchomienie aplikacji ---
app.Run();