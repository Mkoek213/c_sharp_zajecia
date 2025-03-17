// ZAD 2
// String last_word = "";
// StreamWriter sw = new StreamWriter("zad2.txt", append:true);
// while (true){
//     string word = Console.ReadLine();
//     if (word == "koniec!"){
//         break;
//     }
//     if (String.Compare(word, last_word) > 0){
//         last_word = word;
//     }
//     sw.WriteLine(word);
// }
// sw.Close();
// Console.WriteLine($"Ostatnie słowo (leksykograficznie największe): {last_word}");

// ZAD 3
// class Zad{
//     static void Main(string[] args){
//         string file_name = args[0];
//         string word = args[1];
//         StreamReader sr = new StreamReader(file_name);
//         int line_idx = 0;
//         while (!sr.EndOfStream)
//         {   
//             String lnapis = sr.ReadLine();
//             for (int i = 0; i < lnapis.Length; i++)
//             {
//                 if (lnapis[i] == word[0])
//                 {
//                     bool found = true;
//                     for (int j = 1; j < word.Length; j++)
//                     {
//                         if (lnapis[i + j] != word[j])
//                         {
//                             found = false;
//                             break;
//                         }
//                     }
//                     if (found)
//                     {
//                         Console.WriteLine($"linijka: {line_idx}, pozycja: {i}");
//                     }
//                 }
//             }
//             line_idx++;
//         }
//         sr.Close();
//     }
// }

// ZAD 4
// class Zad{
//     static void Main(string[] args){
//         string file_name = args[0];
//         int n = int.Parse(args[1]);
//         int range_start = int.Parse(args[2]);
//         int range_end = int.Parse(args[3]);
//         int seed = int.Parse(args[4]);
//         string liczby = args[5];
//         bool rzeczywiste = false;
//         bool calkowite = false;
//         if (liczby == "rzeczywiste"){
//             rzeczywiste = true;
//         }else{
//             calkowite = true;
//         }
//         StreamWriter sw = new StreamWriter(file_name);
//         Random rand = new Random(seed);
//         for (int i = 0; i < n; i++){
//             if (rzeczywiste){
//                 double num = rand.NextDouble() * (range_end - range_start) + range_start;
//                 sw.WriteLine(num);
//             }
//             if (calkowite){
//                 int num = rand.Next(range_start, range_end);
//                 sw.WriteLine(num);
//             }
//         }
//         sw.Close();
//     }
// }

// ZAD 5
// class Zad
// {
//     static void Main(string[] args)
//     {
//         string file_name = args[0];
//         int n = int.Parse(args[1]);
//         int range_start = int.Parse(args[2]);
//         int range_end = int.Parse(args[3]);
//         int seed = int.Parse(args[4]);
//         string liczby = args[5];

//         bool rzeczywiste = false;
//         bool calkowite = false;

//         int chars = 0;

//         // Declare the variables outside the if-blocks for proper scoping
//         double min_num = double.MaxValue;
//         double max_num = double.MinValue;
//         double avg = 0;
//         double sum = 0;

//         int int_min_num = int.MaxValue;
//         int int_max_num = int.MinValue;
//         int int_avg = 0;
//         int int_sum = 0;

//         if (liczby == "rzeczywiste")
//         {
//             rzeczywiste = true;
//         }
//         else
//         {
//             calkowite = true;
//         }

//         StreamWriter sw = new StreamWriter(file_name);
//         Random rand = new Random(seed);

//         for (int i = 0; i < n; i++)
//         {

//             if (rzeczywiste)
//             {
//                 double num = rand.NextDouble() * (range_end - range_start) + range_start;
//                 sw.WriteLine(num);
//                 chars += num.ToString().Length;
//                 if (num < min_num)
//                 {
//                     min_num = num;
//                 }
//                 if (num > max_num)
//                 {
//                     max_num = num;
//                 }
//                 sum += num;
//             }

//             if (calkowite)
//             {
//                 int num = rand.Next(range_start, range_end);
//                 sw.WriteLine(num);
//                 chars += num.ToString().Length;
//                 if (num < int_min_num)
//                 {
//                     int_min_num = num;
//                 }
//                 if (num > int_max_num)
//                 {
//                     int_max_num = num;
//                 }
//                 int_sum += num;
//             }
//         }

//         sw.Close();

//         // Calculate averages based on type
//         if (rzeczywiste)
//         {
//             avg = sum / n;
//             Console.WriteLine($"Liczba linii: {n}");
//             Console.WriteLine($"Liczba znaków: {chars}");
//             Console.WriteLine($"Max: {max_num}");
//             Console.WriteLine($"Min: {min_num}");
//             Console.WriteLine($"Srednia: {avg}");
//         }
//         else if (calkowite)
//         {
//             int_avg = int_sum / n;
//             Console.WriteLine($"Liczba linii: {n}");
//             Console.WriteLine($"Liczba znaków: {chars}");
//             Console.WriteLine($"Max: {int_max_num}");
//             Console.WriteLine($"Min: {int_min_num}");
//             Console.WriteLine($"Srednia: {int_avg}");
//         }
//     }
// }

// ZAD 6
class Zad{
    static void Main(string[] args){
        string file_name = args[0];
        int n = int.Parse(args[1]);
        int range_start = int.Parse(args[2]);
        int range_end = int.Parse(args[3]);
        int seed = int.Parse(args[4]);
        string liczby = args[5];
        bool rzeczywiste = false;
        bool calkowite = false;
        if (liczby == "rzeczywiste"){
            rzeczywiste = true;
        }else{
            calkowite = true;
        }
        StreamWriter sw = new StreamWriter(file_name);
        Random rand = new Random(seed);
        for (int i = 0; i < n; i++){
            if (rzeczywiste){
                double num = rand.NextDouble() * (range_end - range_start) + range_start;
                sw.WriteLine(num);
            }
            if (calkowite){
                int num = rand.Next(range_start, range_end);
                sw.WriteLine(num);
            }
        }
        sw.Close();
    }
    StreamReader sr = new StreamReader(file_name);
    int pivot = sr.ReadLine();
    while (!sr.EndOfStream){
        string line = sr.ReadLine();
        if (rzeczywiste){
            double num = double.Parse(line);
            if (num < pivot){
                
            }
        }
        if (calkowite){
            int num = int.Parse(line);
            if (num < pivot){
                
            }
        }
    }
}