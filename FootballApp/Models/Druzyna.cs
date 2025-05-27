using System.ComponentModel.DataAnnotations;

namespace FootballApp.Models
{
    public class Druzyna
    {
        public int Id { get; set; }

        [Required]
        public string Nazwa { get; set; }

        [Required]
        public string Miasto { get; set; }

        // Navigation properties — no [Required]!
        public List<Zawodnik>? Zawodnicy { get; set; } = new();
        public List<Mecz>? MeczeDomowe { get; set; } = new();
        public List<Mecz>? MeczeGości { get; set; } = new();
    }
}