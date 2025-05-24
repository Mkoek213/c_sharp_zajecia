public class Mecz
{
    public int Id { get; set; }
    public DateTime Data { get; set; }

    public int DruzynaDomowaId { get; set; }
    public Drużyna DrużynaDomowa { get; set; }

    public int DruzynaGościId { get; set; }
    public Druzyna DruzynaGości { get; set; }

    public int WynikDomowy { get; set; }
    public int WynikGości { get; set; }
}