// // Zad 1
// using System;
// using System.Net;
// using System.Net.Sockets;
// using System.Text;

// class Program
// {
//     static void Main()
//     {
//         const int port = 5000;
//         const int bufferSize = 1024;
//         const string serverIp = "127.0.0.1";

//         TcpListener server = new TcpListener(IPAddress.Parse(serverIp), port);
//         server.Start();
//         Console.WriteLine("Serwer oczekuje na połączenie...");

//         using TcpClient client = server.AcceptTcpClient();
//         Console.WriteLine("Klient połączony.");

//         NetworkStream stream = client.GetStream();
//         byte[] buffer = new byte[bufferSize];

//         int bytesRead = stream.Read(buffer, 0, bufferSize);
//         string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
//         Console.WriteLine("Odebrano wiadomość: " + message);

//         // Przygotuj odpowiedź
//         string response = "odczytalem: " + message;
//         byte[] responseBytes = Encoding.UTF8.GetBytes(response);
//         if (responseBytes.Length > bufferSize)
//             Array.Resize(ref responseBytes, bufferSize);

//         stream.Write(responseBytes, 0, responseBytes.Length);

//         client.Close();
//         server.Stop();
//         Console.WriteLine("Serwer zakończył działanie.");
//     }
// }


//Zad 2
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main()
    {
        const int port = 5000;
        const string serverIp = "127.0.0.1";

        TcpListener server = new TcpListener(IPAddress.Parse(serverIp), port);
        server.Start();
        Console.WriteLine("Serwer oczekuje na połączenie...");

        using TcpClient client = server.AcceptTcpClient();
        Console.WriteLine("Klient połączony.");

        NetworkStream stream = client.GetStream();

        byte[] sizeBuffer = new byte[4];
        stream.Read(sizeBuffer, 0, 4);
        int messageLength = BitConverter.ToInt32(sizeBuffer, 0);
        Console.WriteLine("Odebrano ilosc bajtow: " + messageLength);

        byte[] messageBuffer = new byte[messageLength];
        int bytesRead = stream.Read(messageBuffer, 0, messageLength);
        string message = Encoding.UTF8.GetString(messageBuffer, 0, bytesRead);
        Console.WriteLine("Odebrano wiadomość: " + message);

        string response = "odczytalem: " + message;
        byte[] responseBytes = Encoding.UTF8.GetBytes(response);
        byte[] responseSize = BitConverter.GetBytes(responseBytes.Length);

        stream.Write(responseSize, 0, responseSize.Length);
        stream.Write(responseBytes, 0, responseBytes.Length);

        client.Close();
        server.Stop();
        Console.WriteLine("Serwer zakończył działanie.");
    }
}
