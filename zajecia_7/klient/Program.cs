// // Zad 1
// using System;
// using System.Net.Sockets;
// using System.Text;

// class Program
// {
//     static void Main()
//     {
//         const string serverIp = "127.0.0.1";
//         const int port = 5000;
//         const int bufferSize = 1024;

//         try
//         {
//             using TcpClient client = new TcpClient(serverIp, port);
//             NetworkStream stream = client.GetStream();

//             Console.WriteLine("Wprowadź wiadomość do wysłania:");
//             string message = Console.ReadLine() ?? "";

//             byte[] messageBytes = Encoding.UTF8.GetBytes(message);
//             if (messageBytes.Length > bufferSize)
//                 Array.Resize(ref messageBytes, bufferSize);

//             stream.Write(messageBytes, 0, messageBytes.Length);

//             // Odbierz odpowiedź
//             byte[] buffer = new byte[bufferSize];
//             int bytesRead = stream.Read(buffer, 0, bufferSize);
//             string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
//             Console.WriteLine("Odpowiedź od serwera: " + response);
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine("Błąd: " + ex.Message);
//         }
//     }
// }

// Zad 2
using System;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main()
    {
        const string serverIp = "127.0.0.1";
        const int port = 5000;

        try
        {
            using TcpClient client = new TcpClient(serverIp, port);
            NetworkStream stream = client.GetStream();

            Console.WriteLine("Wprowadź wiadomość do wysłania:");
            string message = Console.ReadLine() ?? "";
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            byte[] messageSize = BitConverter.GetBytes(messageBytes.Length);

            stream.Write(messageSize, 0, messageSize.Length);
            stream.Write(messageBytes, 0, messageBytes.Length);

            byte[] responseSizeBuffer = new byte[4];
            stream.Read(responseSizeBuffer, 0, 4);
            int responseSize = BitConverter.ToInt32(responseSizeBuffer, 0);

            byte[] responseBuffer = new byte[responseSize];
            int bytesRead = stream.Read(responseBuffer, 0, responseSize);
            string response = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);

            Console.WriteLine("Odpowiedź od serwera: " + response);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Błąd: " + ex.Message);
        }
    }
}
