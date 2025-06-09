using CsvHelper;
using CsvHelper.Configuration;
using RadiationApi.Models;
using System.Globalization;

namespace RadiationApi.Data
{
    public static class DataSeeder
    {
        public static void Seed(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated();

                if (context.Measurements.Any())
                {
                    Console.WriteLine("Baza danych zawiera już dane. Pomijam import.");
                    return;
                }

                Console.WriteLine("Baza danych jest pusta. Rozpoczynam import danych z nowego pliku CSV...");

                // --- ZMIANA: Konfiguracja CsvHelper ---
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    // Ustawiamy poprawny separator danych
                    Delimiter = ";", 
                    HasHeaderRecord = true,
                };

                // Ścieżka do nowego pliku CSV
                var filePath = Path.Combine(AppContext.BaseDirectory, "apparatus_measurements.csv");

                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"BŁĄD: Nie znaleziono pliku danych pod ścieżką: {filePath}");
                    return;
                }

                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, config))
                {
                    // --- ZMIANA: Rejestracja nowego mapowania ---
                    csv.Context.RegisterClassMap<ApparatusMeasurementMap>();

                    var records = csv.GetRecords<Measurement>().Take(100).ToList();

                    context.Measurements.AddRange(records);
                    context.SaveChanges();

                    Console.WriteLine($"Zaimportowano {records.Count} rekordów do bazy danych.");
                }
            }
        }
    }

    // --- ZMIANA: Nowa klasa mapująca dla nowego formatu CSV ---
    public sealed class ApparatusMeasurementMap : ClassMap<Measurement>
    {
        public ApparatusMeasurementMap()
        {
            // Mapujemy nazwy kolumn z pliku CSV na właściwości w klasie Measurement
            Map(m => m.ApparatusId).Name("apparatusId");
            Map(m => m.ApparatusVersion).Name("apparatusVersion");
            Map(m => m.ApparatusSensorType).Name("apparatusSensorType");
            Map(m => m.ApparatusTubeType).Name("apparatusTubeType");
            Map(m => m.Temperature).Name("temperature");
            Map(m => m.Value).Name("value");
            Map(m => m.HitsNumber).Name("hitsNumber");
            Map(m => m.CalibrationFunction).Name("calibrationFunction");
            Map(m => m.StartTime).Name("startTime");
            Map(m => m.EndTime).Name("endTime");
        }
    }
}