// //Zad 1
// using System;
// using System.Collections.Generic;
// using System.Threading;

// class Data
// {
//     public int ProducerId { get; set; }
// }

// class ProducerConsumer
// {
//     private static Queue<Data> queue = new Queue<Data>();
//     private static object lockObj = new object();
//     private static bool running = true;

//     public static void Main()
//     {
//         int producerCount = 3;
//         int consumerCount = 2;

//         List<Thread> threads = new List<Thread>();
//         Random rand = new Random();

//         for (int i = 0; i < producerCount; i++)
//         {
//             int id = i;
//             int delay = rand.Next(500, 1500);
//             threads.Add(new Thread(() => Producer(id, delay)));
//         }

//         for (int i = 0; i < consumerCount; i++)
//         {
//             int id = i;
//             int delay = rand.Next(700, 2000);
//             threads.Add(new Thread(() => Consumer(id, delay, producerCount)));
//         }

//         foreach (var thread in threads)
//             thread.Start();

//         Console.WriteLine("Press 'q' to quit.");
//         while (Console.ReadKey(true).KeyChar != 'q') ;

//         running = false;

//         foreach (var thread in threads)
//             thread.Join();

//         Console.WriteLine("Program zakończony.");
//     }

//     static void Producer(int id, int delay)
//     {
//         Random rand = new Random();
//         while (running)
//         {
//             Thread.Sleep(delay);
//             lock (lockObj)
//             {
//                 queue.Enqueue(new Data { ProducerId = id });
//                 Console.WriteLine($"Producent {id} dodał dane.");
//             }
//         }
//     }

//     static void Consumer(int id, int delay, int producerCount)
//     {
//         int[] stats = new int[producerCount];
//         while (running)
//         {
//             Thread.Sleep(delay);
//             lock (lockObj)
//             {
//                 if (queue.Count > 0)
//                 {
//                     var data = queue.Dequeue();
//                     stats[data.ProducerId]++;
//                     Console.WriteLine($"Konsument {id} odebrał dane od Producenta {data.ProducerId}.");
//                 }
//             }
//         }

//         Console.WriteLine($"Konsument {id} zakończył. Statystyki:");
//         for (int i = 0; i < producerCount; i++)
//             Console.WriteLine($"  Producent {i} - {stats[i]}");
//     }
// }




// // Zad 2
// using System;
// using System.IO;
// using System.Threading;

// class DirectoryMonitor
// {
//     private static bool running = true;

//     public static void Main()
//     {
//         string path = Path.Combine(Directory.GetCurrentDirectory(), "temp");

//         Thread watcher = new Thread(() => WatchDirectory(path));
//         watcher.Start();

//         Console.WriteLine("Monitoring katalogu. Wciśnij 'q' aby zakończyć.");
//         while (Console.ReadKey(true).KeyChar != 'q') ;

//         running = false;
//         watcher.Join();
//     }

//     static void WatchDirectory(string path)
//     {
//         FileSystemWatcher fsw = new FileSystemWatcher(path);
//         fsw.EnableRaisingEvents = true;
//         fsw.IncludeSubdirectories = false;

//         fsw.Created += (s, e) => Console.WriteLine($"Dodano plik {e.Name}");
//         fsw.Deleted += (s, e) => Console.WriteLine($"Usunięto plik {e.Name}");

//         while (running)
//         {
//             Thread.Sleep(500);
//         }

//         fsw.Dispose();
//     }
// }


// // Zad 3
// using System;
// using System.IO;
// using System.Threading;

// class FileSearcher
// {
//     private static AutoResetEvent foundFileEvent = new AutoResetEvent(false);
//     private static Queue<string> foundFiles = new Queue<string>();
//     private static bool done = false;

//     public static void Main()
//     {
//         string dir = Path.Combine(Directory.GetCurrentDirectory(), "temp");
//         string pattern = "ron";

//         Thread searchThread = new Thread(() => SearchFiles(dir, pattern));
//         searchThread.Start();

//         while (!done || foundFiles.Count > 0)
//         {
//             foundFileEvent.WaitOne();
//             while (foundFiles.Count > 0)
//             {
//                 string file = foundFiles.Dequeue();
//                 Console.WriteLine("Znaleziono: " + file);
//             }
//         }

//         searchThread.Join();
//         Console.WriteLine("Wyszukiwanie zakończone.");
//     }
//     static void SearchFiles(string path, string pattern)
//     {
//         try
//         {
//             foreach (var file in Directory.GetFiles(path))
//             {
//                 if (Path.GetFileName(file).Contains(pattern))
//                 {
//                     foundFiles.Enqueue(file);
//                     foundFileEvent.Set();
//                 }
//             }

//             foreach (var dir in Directory.GetDirectories(path))
//             {
//                 SearchFiles(dir, pattern);
//             }
//         }
//         catch { }

//         done = true;
//         foundFileEvent.Set();
//     }
// }


// Zad 4
using System;
using System.Collections.Generic;
using System.Threading;

class ThreadStartSync
{
    static int startedCount = 0;
    static ManualResetEvent allStartedEvent = new ManualResetEvent(false);
    static bool shouldStop = false;

    public static void Main()
    {
        int threadCount = 5;
        List<Thread> threads = new List<Thread>();
        object lockObj = new object();

        for (int i = 0; i < threadCount; i++)
        {
            int id = i;
            threads.Add(new Thread(() =>
            {
                Console.WriteLine($"Wątek {id} zaczyna.");
                lock (lockObj)
                {
                    startedCount++;
                    if (startedCount == threadCount)
                        allStartedEvent.Set();
                }

                while (!shouldStop)
                    Thread.Sleep(100);
                
                Console.WriteLine($"Wątek {id} zakończył działanie.");
            }));
        }

        foreach (var t in threads)
            t.Start();

        allStartedEvent.WaitOne();
        Console.WriteLine("Wszystkie wątki rozpoczęły działanie.");

        shouldStop = true;

        foreach (var t in threads)
            t.Join();

        Console.WriteLine("Wszystkie wątki zakończone.");
    }
}


