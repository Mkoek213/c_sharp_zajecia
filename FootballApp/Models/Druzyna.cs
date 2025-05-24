namespace FootballApp.Models
{
    public class Druzyna
    {
        public int Id { get; set; }
        public string Nazwa { get; set; }
        public string Miasto { get; set; }

        public ICollection<Zawodnik> Zawodnicy { get; set; }
        public ICollection<Mecz> MeczeDomowe { get; set; }
        public ICollection<Mecz> MeczeGości { get; set; }
    }
}