# Laboratorium 01: Pierwsze aplikacje konsolowe C# .NET Framework Core 7.0.
## Programowanie zaawansowane 2

- Maksymalna liczba punktĂłw: 10

- Skala ocen za punkty:
    - 9-10 ~ bardzo dobry (5.0)
    - 8 ~ plus dobry (4.5)
    - 7 ~ dobry (4.0)
    - 6 ~ plus dostateczny (3.5)
    - 5 ~ dostateczny (3.0)
    - 0-4 ~ niedostateczny (2.0)

Celem laboratorium jest zapoznanie z operacjami wejĹcia/wyjĹcia jÄzyka C# i praktyki implementacji prostych algorytmĂłw. 

NiektĂłre programy wymagajÄ podania z linii poleceĹ pewnych parametrĂłw. Dla uproszczenia przyjmijmy, Ĺźe programy nie muszÄ obsĹugiwaÄ wyjÄtkĂłw spowodowanych ewentualnymi bĹÄdami konwersji oraz, Ĺźe uĹźytkownicy podajÄ odpowiedniÄ liczbÄ parametrĂłw.

1. W programie Visual Studio Code stwĂłrz nowÄ aplikacjÄ konsolowÄ technologii .NET Framework 7.0 i uruchom go. Program ma pobieraÄ z linii komend zestaw napisĂłw oraz jako ostatni parametr liczbÄ powtĂłrzeĹ. Program ma wypisaÄ na ekran wszystkie napisy tyle razy, ile wynosiĹa wartoĹÄ ostatniego parametru (3 punkt).

```cs

> dotnet new console --framework net7.0
> dotnet run
```

2. Napisz program, ktĂłry bÄdzie pobieraĹ dane liczbowe klawiatury aĹź do momentu, kiedy uĹźytkownik wpisze 0. Program ma sumowaÄ wpisane liczby a na koĹcu wyliczyÄ ich ĹredniÄ. Wynik zapisz do pliku (2 punkty).

```cs

//Zapis linijki tekstu do pliku w trybie append
StreamWriter sw = new StreamWriter("NazwaPliku.txt", append:true);
sw.WriteLine("JakiĹ napis");
sw.Close();

```

3. Napisz program, ktĂłry w pliku tekstowym zawierajacym liczby znajdzie liczbÄ o najwiÄkszej wartoĹci. Program jako parametr (linii komend) ma pobieraÄ nazwÄ pliku. Jako wynik do konsoli proszÄ wypisaÄ tÄ liczbÄ oraz numery linijki, w ktĂłrych znaleziono liczbÄ, na przykĹad "555, linijka: 10" (2 punkty).

```cs

//czytanie z pliku tekstowego linijka po linijce aĹź do koĹca pliku
StreamReader sr = new StreamReader("NazwaPlikuTekstowego.txt");
while (!sr.EndOfStream)
{
    String napis = sr.ReadLine();
}
sr.Close();

```

4. Napisz program, który wypisze gamę dur, rozpoczynając od jednego wybranego z dwunastu dźwięków. Są następujące dźwięki: C, C#, D, D#, E, F, F#, G, G#, A, B, H.

Po dźwięku H znowu następuje dźwięk C. Pomiędzy każdym dźwiękiem jest różnica półtonu. Gama dur tworzona jest w następujący sposób: dźwięk podstawowy, a następnie dźwięki wyższe o: 2, 2, 1, 2, 2, 2, 1 ton.

Czyli gama C-dur to:
➡ C D E F G A H C

Gama C#-dur to:
➡ C# D# F F# G# B C C#

Gama kończy się zawsze tym samym dźwiękiem, od którego się zaczynała, i ma 8 dźwięków.

Program ma pobierać z klawiatury nazwę dźwięku, a na ekran wypisywać gamę. (3 punkty).