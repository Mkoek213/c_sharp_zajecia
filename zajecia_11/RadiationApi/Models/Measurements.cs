using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using CsvHelper.Configuration;

namespace RadiationApi.Models
{
    // Nazwa klasy pozostaje bez zmian, ale jej właściwości są dostosowane
    // do nowej struktury pliku CSV.
    public class Measurement
    {
        [Key]
        [BindNever]
        public int Id { get; set; }

        // Pierwsze 10 kolumn z nowego pliku CSV.
        public string? ApparatusId { get; set; }
        public string? ApparatusVersion { get; set; }
        public string? ApparatusSensorType { get; set; }
        public string? ApparatusTubeType { get; set; }
        public string? Temperature { get; set; }
        public string? Value { get; set; }
        public string? HitsNumber { get; set; }
        public string? CalibrationFunction { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
    }
}