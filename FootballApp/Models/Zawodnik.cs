namespace FootballApp.Models
{
    public class Zawodnik
    {
        public int Id { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Pozycja { get; set; }

        public int? DruzynaId { get; set; }
        public Druzyna? Druzyna { get; set; }

        public StatystykiZawodnika Statystyki { get; set; }
    }
}