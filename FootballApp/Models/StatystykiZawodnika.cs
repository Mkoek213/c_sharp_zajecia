namespace FootballApp.Models
{
    public class StatystykiZawodnika
    {
        public int Id { get; set; }
        public int ZawodnikId { get; set; }
        public Zawodnik? Zawodnik { get; set; }

        public int Mecze { get; set; }
        public int Gole { get; set; }
        public int Asysty { get; set; }
    }
}