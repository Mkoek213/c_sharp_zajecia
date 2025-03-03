// // Zad 2
// int sum = 0;
// int counter = 0;
// while (true){
//     int liczba = int.Parse(Console.ReadLine());
//     if (liczba == 0){
//         break;
//     }else{
//         sum += liczba;
//     }
//     counter++;
// }
// int avg = (counter > 0) ? (sum / counter) : 0;
// StreamWriter sw = new StreamWriter("wynik_zad2.txt", append:true);
// sw.WriteLine("Srednia: "+ avg);
// sw.Close();



// // Zad 3
// using System;
// using System.Collections.Generic;
// string file_name = Console.ReadLine();
// StreamReader sr = new StreamReader(file_name);
// int max_num = -int.MaxValue;
// int max_line = 0;
// int curr_line = 0;
// while (!sr.EndOfStream){
//     String napis = sr.ReadLine();
//     string[] words = napis.Split(' ');
//     foreach (string word in words){
//         if (int.TryParse(word, out int num)){
//             Console.WriteLine(num);
//             if (num > max_num){
//                 max_num = num;
//                 max_line = curr_line;
//             }
//         }
//     }
//     curr_line++;
// }
// sr.Close();
// Console.WriteLine("Najwieksza liczba: "+ max_num + " w linii: "+ max_line);


// Zad 4
using System;
using System.Collections.Generic;

string sound = Console.ReadLine();

Dictionary<string, int> values = new Dictionary<string, int>
{
    { "C", 2 }, { "C#", 2 }, { "D", 2 }, { "D#", 2}, { "E", 1 }, { "F", 2 }, { "F#", 2 },
    { "G", 2 }, { "G#", 2 }, { "A", 2 }, { "B", 2 }, { "H", 1 }
};

Dictionary<string, int> sounds = new Dictionary<string, int>
{
    { "C", 1 }, { "C#", 2 }, { "D", 3 }, { "D#", 4 }, { "E", 5 }, { "F", 6 },
    { "F#", 7 }, { "G", 8 }, { "G#", 9 }, { "A", 10 }, { "B", 11 }, { "H", 12 }
};

Dictionary<int, string> indexToSound = new Dictionary<int, string>
{
    { 1, "C" }, { 2, "C#" }, { 3, "D" }, { 4, "D#" }, { 5, "E" }, { 6, "F" },
    { 7, "F#" }, { 8, "G" }, { 9, "G#" }, { 10, "A" }, { 11, "B" }, { 12, "H" }
};

List<string> dur = new List<string> { sound };
int idx = sounds[sound];

for (int i = 0; i < 7; i++)
{
    int step = values[sound];
    int new_sound = (idx + step) % 12;
    if (new_sound == 0) new_sound = 12;

    sound = indexToSound[new_sound]; 
    dur.Add(sound);
    idx = new_sound;
}
Console.WriteLine(string.Join(" ", dur));
