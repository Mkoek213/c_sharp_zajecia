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

4. Napisz program, ktĂłry wypisze gamÄ dur rozpoczynajÄc od jednego wybranego z dwunastu dĹşwiÄkĂłw. SÄ nastÄpujÄce dĹşwiÄki:
C, C#, D, D#, E, F, F#, G, G#, A, B, H

Po dĹşwiÄku H znowu nastÄpuje dĹşwiÄk C. PomiÄdzy kaĹźdem dĹşwiÄkiem jest rĂłĹźnica pĂłĹ tonu. Gama dur tworzona jest w nastÄpujÄcy sposĂłb: dĹşwiÄk podstawowy, a nastÄpnie dĹşwiÄki wyĹźsze o: 2, 2, 1, 2, 2, 2, 1 ton. Czyli gama C-dur to: 

C D E F G A H C 

Gama C# dur to: 

C#, D#, F, F#, G#, B, C, C#

Gama koĹczy siÄ zawsze tym samym dĹşwiÄkiem, od ktĂłrego siÄ zaczynaĹa i ma 8 dĹşwiÄkĂłw. Program ma pobieraÄ z klawiaturÄ nazwÄ dĹşwiÄku a na ekran wypisywaÄ gamÄ (3 punkty).