// Celem laboratorium jest zapoznanie z działaniem funkcji pozwalających na bezpośrednią komunikację z bazą danych SQLite. Po wykonaniu wszystkich poleceń powinien powstać program, który pozwala na wczytanie danych z pliku CSV do tabeli w bazie SQLite oraz wyświetla te dane. Ta tabela ma zostać utworzona automatycznie na podstawie danych, które zawarte są w pliku. Przykład pliku:


// ```cs
// pole1,pole2,pole3
// 1,abc,4
// 3,dxx,
// 5,asdaa,5.6
// 7,,0.12
// ```

// Powyższy plik to struktura danych z trzema kolumnami o nazwach pole1, pole2 oraz pole3 - kolumn w tabeli jest dokładnie tyle, ile kolumn w nagłówku (headerze) pliku. Pierwsza kolumna jest typu integer, druga typu text, trzecia typu real - należy to wywnioskować na podstawie wartości w pliku. Druga i trzecia kolumna może zawierać wartości null - brak wpisanej w danej kolumnie wartości (pusty napis) powinien być zinterpretowany jako null. Wartości kolumny pole1: 1, 3, 5, 7; wartości kolumny pole2: abc, dxx, asdaa, NULL; wartości kolumny pole3: 4,NULL,5.6,0.12.

// Laboratorium składa się z kilku podpunktów - każdy przetestuj przed pokazaniem go prowadzącemu.

// 1. [3 punkty] Napisz metodę wczytującą dane z pliku CSV do dowolnej struktury danych (na przykład na List<List<String>>). Metoda ma zwrócić tą strukturę danych oraz informację o nazwach kolumn - nazwy kolumn proszę wczytać z headera pliku. Metoda jako argument ma przyjmować nazwę pliku csv oraz separator dzielący od siebie poszczególne wartości w kolumnach. Zakładamy, że ten separator nie występuje nigdzie jako wartość kolumny, na przykład, jeśli separatorem jest przecinek, to przecinek nie występuje w kolumnach z napisami.

// 2. [2 punkty] Napisz metodę, która jako parametr pobiera dane zwrócone przez metodę z punktu 1. Metoda na podstawie tych danych ma zwracać typy danych dla poszczególnych kolumn oraz czy kolumna może przyjmować wartości NULL czy też nie. Zakładamy, że jeśli kolumna nigdy nie ma wartości NULL, to nie może przyjmować wartości NULL. Jeżeli wszystkie pola tej kolumny można zrzutować na int, kolumna jest typu INTEGER, jeżeli kolumna nie jest typu INTEGER a wszystkie jej pola można zrzutować na double, to jest typu REAL. W pozostałym przypadku kolumna jest typu TEXT.

// 3. [2 punkty] Napisz metodę, która jako parametry przyjmuje dane zwrócone przez metodę z punktu 2, nazwę tabeli do utworzenia oraz obiekt klasy SqliteConnection. Metoda na podstawie danych ma utworzyć w bazie odpowiednią tabelę (o zdanych nazwach kolumn i typach) i nazwie zgodnej z przekazaną. Połączenie z bazą przekazywane jest w obiekcie SqliteConnection.

// 4. [2 punkty] Napisz metodę, która jako parametry przyjmuje dane, które mają znaleźć się w tabeli (dane zostały wczytane metodą z punktu 1), nazwę tabeli oraz obiekt klasy SqliteConnection. Metoda ma wypełnić tabelę utworzoną w punkcie 3 tymi danymi.

// 5. [1 punkt] Napisz metodę, która jako parametr przyjmuje nazwę tabeli oraz obiekt klasy SqliteConnection. Metoda przy pomocy kwerendy SELECT ma wypisać do konsoli wszystkie dane, które znajdują się w tej tabeli. Proszę wypisać również nazwy kolumn.


// Zad 1
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;


class Program 
{
    static void Main(string[] args)
    {
        string filePath = "data.csv"; // Ścieżka do pliku CSV
        string separator = ","; // Separator używany w pliku CSV

        // Wczytanie danych z pliku CSV
        var (data, columnNames) = ReadCsv(filePath, separator);

        // Określenie typów danych dla kolumn
        var columnTypes = GetColumnTypes(data);

        // Utworzenie bazy danych SQLite
        using (var connection = new SQLiteConnection("Data Source=db5.db;Version=3;"))
        {
            connection.Open();

            // Utworzenie tabeli w bazie danych
            CreateTable(connection, "MyTable", columnNames, columnTypes);

            // Wypełnienie tabeli danymi
            InsertData(connection, "MyTable", data);

            // Wyświetlenie danych z tabeli
            DisplayData(connection, "MyTable");
        }
    }
    // Metoda do wczytania danych z pliku CSV
    static (List<List<string>>, List<string>) ReadCsv(string filePath, string separator)
    {
        var data = new List<List<string>>();
        var columnNames = new List<string>();

        using (var reader = new StreamReader(filePath))
        {
            // Wczytanie nagłówka
            var headerLine = reader.ReadLine();
            if (headerLine != null)
            {
                columnNames = headerLine.Split(new[] { separator }, StringSplitOptions.None).ToList();
            }

            // Wczytanie danych
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line != null)
                {
                    var values = line.Split(new[] { separator }, StringSplitOptions.None).ToList();
                    data.Add(values);
                }
            }
        }

        return (data, columnNames);
    }
    // Metoda do określenia typów danych dla kolumn
    static List<(string ColumnName, string DataType, bool AllowNull)> GetColumnTypes(List<List<string>> data)
    {
        var columnTypes = new List<(string ColumnName, string DataType, bool AllowNull)>();

        for (int i = 0; i < data[0].Count; i++)
        {
            var columnData = data.Select(row => row[i]).ToList();
            bool allowNull = columnData.Any(value => string.IsNullOrEmpty(value));
            string dataType;
            string columnName = data[0][i];

            if (columnData.All(value => int.TryParse(value, out _)))
            {
                dataType = "INTEGER";
            }
            else if (columnData.All(value => double.TryParse(value, out _)))
            {
                dataType = "REAL";
            }
            else
            {
                dataType = "TEXT";
            }

            columnTypes.Add((columnName, dataType, allowNull));
        }

        return columnTypes;
    }
    // Metoda do utworzenia tabeli w bazie danych
    static void CreateTable(SQLiteConnection connection, string tableName, List<string> columnNames, List<(string ColumnName, string DataType, bool AllowNull)> columnTypes)
    {
        var createTableQuery = new StringBuilder($"CREATE TABLE IF NOT EXISTS {tableName} (");

        for (int i = 0; i < columnNames.Count; i++)
        {
            createTableQuery.Append($"{columnNames[i]} {columnTypes[i].DataType}");
            if (!columnTypes[i].AllowNull)
            {
                createTableQuery.Append(" NOT NULL");
            }
            if (i < columnNames.Count - 1)
            {
                createTableQuery.Append(", ");
            }
        }

        createTableQuery.Append(");");

        using (var command = new SQLiteCommand(createTableQuery.ToString(), connection))
        {
            command.ExecuteNonQuery();
        }
    }
    // Metoda do wypełnienia tabeli danymi
    static void InsertData(SQLiteConnection connection, string tableName, List<List<string>> data)
    {
        foreach (var row in data)
        {
            var insertQuery = $"INSERT INTO {tableName} VALUES ({string.Join(", ", row.Select(value => $"'{value}'"))});";
            using (var command = new SQLiteCommand(insertQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
    // Metoda do wyświetlenia danych z tabeli
    static void DisplayData(SQLiteConnection connection, string tableName)
    {
        var selectQuery = $"SELECT * FROM {tableName};";
        using (var command = new SQLiteCommand(selectQuery, connection))
        using (var reader = command.ExecuteReader())
        {
            // Wyświetlenie nagłówków
            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.Write($"{reader.GetName(i)}\t");
            }
            Console.WriteLine();

            // Wyświetlenie danych
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write($"{reader[i]}\t");
                }
                Console.WriteLine();
            }
        }
    }
}