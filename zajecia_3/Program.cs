//Stwórz publiczną, abstrakcyjną klasę PosiadaczRachunku w której będzie znajdować się przeciążenie metody String ToString() z klasy object. Metoda przeciążająca ma być również abstrakcyjna.

abstract class PosiadaczRachunku
{
    public abstract override string ToString();
}

class OsobaFizyczna: PosiadaczRachunku
{

public OsobaFizyczna(string imie, string nazwisko, string drugieImie, string pesel, string numerPaszportu)
{
    this.imie = imie;
    this.nazwisko = nazwisko;
    this.drugieImie = drugieImie;
    if ((pesel.Length != 11 || pesel == null) && string.IsNullOrEmpty(numerPaszportu))
    {
        throw new Exception("PESEL musi mieć 11 znaków");
    }
    this.pesel = pesel;
    this.numerPaszportu = numerPaszportu;
    if (string.IsNullOrEmpty(pesel) && string.IsNullOrEmpty(numerPaszportu))
    {
        throw new Exception("PESEL albo numer paszportu muszą być nie null");
    }
}

private string imie;
private string nazwisko;
private string drugieImie;
private string pesel;
private string numerPaszportu;

public override string ToString()
{
    return $"Osoba fizyczna: {imie}   {nazwisko} ";
}

public string Imie
    {
        get { return imie; }
        set { imie = value; }
    }

    public string Nazwisko
    {
        get { return nazwisko; }
        set { nazwisko = value; }
    }

    public string DrugieImie
    {
        get { return drugieImie; }
        set { drugieImie = value; }
    }

    public string Pesel
    {
        get { return pesel; }
        set 
        { 
        if (pesel.Length != 11 || pesel == null)
        {
            throw new Exception("PESEL musi mieć 11 znaków");
        }   
        pesel = value; 
        }
    }

    public string NumerPaszportu
    {
        get { return numerPaszportu; }
        set { numerPaszportu = value; }
    }
}


class OsobaPrawna: PosiadaczRachunku
{

    public OsobaPrawna(string nazwa, string siedziba)
    {
        this.nazwa = nazwa;
        this.siedziba = siedziba;
    }

    private string nazwa;
    private string siedziba;

    public string Nazwa
    {
        get { return nazwa; }
    }

    public string Siedziba
    {
        get { return siedziba; }
    }

    public override string ToString()
    {
        return $"Osoba prawna: {nazwa}   {siedziba} ";
    }
}


class RachunekBankowy
{
    
    public RachunekBankowy(string numer, int stanRachunku, bool czyDozwolonyDebet, List<PosiadaczRachunku>posiadaczeRachunku)
    {
        this.numer = numer;
        this.stanRachunku = stanRachunku;
        this.czyDozwolonyDebet = czyDozwolonyDebet;
        if (posiadaczeRachunku.Count == 0)
        {
            throw new Exception("Lista posiadaczy nie może być pusta");
        }
    }

    private string numer;
    private int stanRachunku;
    private bool czyDozwolonyDebet;
    private List<PosiadaczRachunku>posiadaczeRachunku = new List<PosiadaczRachunku>();
    private List<Transakcja> transakcje = new List<Transakcja>();

    public string Numer
    {
        get { return numer; }
        set { numer = value; }
    }

    public int StanRachunku
    {
        get { return stanRachunku; }
        set { stanRachunku = value; }
    }

    public bool CzyDozwolonyDebet
    {
        get { return czyDozwolonyDebet; }
        set { czyDozwolonyDebet = value; }
    }

    public List<PosiadaczRachunku> PosiadaczeRachunku
    {
        get { return posiadaczeRachunku; }
    }

    public void DodajPosiadacza(PosiadaczRachunku posiadacz)
    {
        posiadaczeRachunku.Add(posiadacz);
    }

    public void UsunPosiadacza(PosiadaczRachunku posiadacz)
    {
        if (posiadaczeRachunku.Contains(posiadacz))
        {
            posiadaczeRachunku.Remove(posiadacz);
        }
    }

    public void DlugoscListyPosiadaczy()
    {
        Console.WriteLine(posiadaczeRachunku.Count);
    }

    static public void DodajTransakcje(RachunekBankowy rachunekZrodlowy, RachunekBankowy rachunekDocelowy, int kwota, string opis)
    {
        if (kwota < 0 || rachunekZrodlowy == null && rachunekDocelowy == null || rachunekZrodlowy.CzyDozwolonyDebet == false && rachunekZrodlowy.StanRachunku < kwota)
        {
            throw new Exception("Kwota nie może być ujemna, rachunek źródłowy i docelowy nie mogą być null, rachunek źródłowy nie może mieć debetu i stan rachunku nie może być mniejszy od kwoty");
        }

        if (rachunekZrodlowy == null) //wpłata gotówkowa
        {
            rachunekDocelowy.StanRachunku += kwota;
            Transakcja transakcja = new Transakcja(null, rachunekDocelowy, kwota, opis);
            rachunekDocelowy.transakcje.Add(transakcja);
        }

        if (rachunekDocelowy == null) //wypłata gotówkowa
        {
            rachunekZrodlowy.StanRachunku -= kwota;
            Transakcja transakcja = new Transakcja(rachunekZrodlowy, null, kwota, opis);
            rachunekZrodlowy.transakcje.Add(transakcja);
        }

        if (rachunekZrodlowy != null && rachunekDocelowy != null) //przelew
        {
            rachunekZrodlowy.StanRachunku -= kwota;
            rachunekDocelowy.StanRachunku += kwota;
            Transakcja transakcja = new Transakcja(rachunekZrodlowy, rachunekDocelowy, kwota, opis);
            rachunekZrodlowy.transakcje.Add(transakcja);
            rachunekDocelowy.transakcje.Add(transakcja);
        }
    }

    public static RachunekBankowy operator +(RachunekBankowy rachunek, PosiadaczRachunku posiadacz)
    {
        if (rachunek.PosiadaczeRachunku.Contains(posiadacz))
        {
            throw new Exception("Posiadacz już jest na liście posiadaczy rachunku");
        }
        rachunek.posiadaczeRachunku.Add(posiadacz);
        return rachunek;
    }

    public static RachunekBankowy operator -(RachunekBankowy rachunek, PosiadaczRachunku posiadacz)
    {
        if (rachunek.PosiadaczeRachunku.Count < 1 || !rachunek.PosiadaczeRachunku.Contains(posiadacz))
        {
            throw new Exception("Posiadacz nie jest na liście posiadaczy rachunku lub lista posiadaczy jest pusta");
        }
        rachunek.posiadaczeRachunku.Remove(posiadacz);
        return rachunek;
    }

    public override string ToString()
    {
        string posiadacze_text = "";
        string transakcje_text = "";
        foreach (var posiadacz in this.posiadaczeRachunku)
        {
            posiadacze_text += posiadacz.ToString() + " ";
        }
        foreach (var transakcja in this.transakcje)
        {
            transakcje_text += transakcja.ToString() + " ";
        }
        return $"Rachunek bankowy: {numer} stan rachunku: {stanRachunku} posiadacze: {posiadacze_text} transakcje: {transakcje_text}";
    }
}


class Transakcja
{

    public Transakcja(RachunekBankowy rachunekZrodlowy, RachunekBankowy rachunekDocelowy, int kwota, string opis)
    {
        this.rachunekZrodlowy = rachunekZrodlowy;
        this.rachunekDocelowy = rachunekDocelowy;
        this.kwota = kwota;
        this.opis = opis;

        if (rachunekZrodlowy == null || rachunekDocelowy == null)
        {
            throw new Exception("Rachunek źródłowy i docelowy nie mogą być null");
        }
    }

    private RachunekBankowy rachunekZrodlowy;
    private RachunekBankowy rachunekDocelowy;
    private int kwota;
    private string opis;

    public RachunekBankowy RachunekZrodlowy
    {
        get { return rachunekZrodlowy; }
        set { rachunekZrodlowy = value; }
    }

    public RachunekBankowy RachunekDocelowy
    {
        get { return rachunekDocelowy; }
        set { rachunekDocelowy = value; }
    }

    public int Kwota
    {
        get { return kwota; }
        set { kwota = value; }
    }

    public string Opis
    {
        get { return opis; }
        set { opis = value; }
    }

    public override string ToString()
    {
        return $"Transakcja: {rachunekZrodlowy} -> {rachunekDocelowy} kwota: {kwota} opis: {opis}";
    }
}


class Program
{
static void Main()
{
        // Tworzenie posiadaczy rachunków
        OsobaFizyczna osoba1 = new OsobaFizyczna("Jan", "Kowalski", "Adam", "12345678901", "");
        OsobaPrawna firma1 = new OsobaPrawna("TechCorp", "Warszawa");
        OsobaPrawna firma2 = new OsobaPrawna("SoftCorp", "Kraków");

        // Tworzenie listy posiadaczy
        List<PosiadaczRachunku> posiadacze = new List<PosiadaczRachunku> { osoba1, firma1 };

        // Tworzenie rachunku bankowego
        RachunekBankowy rachunek = new RachunekBankowy("987654321", 10000, true, posiadacze);

        // Dodanie i usunięcie posiadacza
        OsobaFizyczna osoba2 = new OsobaFizyczna("Anna", "Nowak", "Maria", "", "12345678901");
        rachunek.DodajPosiadacza(osoba2);
        rachunek.UsunPosiadacza(firma1);

        // Wyświetlenie dlugosci listy posiadaczy rachunku
        rachunek.DlugoscListyPosiadaczy();
        Console.WriteLine("\n\n");

        // Tworzenie drugiego rachunku
        RachunekBankowy rachunek2 = new RachunekBankowy("123123123", 5000, false, new List<PosiadaczRachunku> { osoba2 });

        // Przelew między rachunkami
        Transakcja przelew = new Transakcja(rachunek, rachunek2, 1500, "Opłata za usługi");
        Console.WriteLine(przelew);
        Console.WriteLine("\n\n");

        // Test przeciżenia operatora
        rachunek = rachunek + firma2;
        rachunek.ToString();
        Console.WriteLine("\n\n");
        rachunek = rachunek - osoba2;
        rachunek.ToString();
    }
}
