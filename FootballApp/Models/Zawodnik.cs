public class Zawodnik
{
    public int Id { get; set; }
    public string Imię { get; set; }
    public string Nazwisko { get; set; }
    public string Pozycja { get; set; }

    public int DrużynaId { get; set; }
    public Drużyna Drużyna { get; set; }

    public StatystykiZawodnika Statystyki { get; set; }
}