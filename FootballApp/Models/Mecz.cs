public class Mecz
{
    public int Id { get; set; }
    public DateTime Data { get; set; }

    public int DrużynaDomowaId { get; set; }
    public Drużyna DrużynaDomowa { get; set; }

    public int DrużynaGościId { get; set; }
    public Drużyna DrużynaGości { get; set; }

    public int WynikDomowy { get; set; }
    public int WynikGości { get; set; }
}