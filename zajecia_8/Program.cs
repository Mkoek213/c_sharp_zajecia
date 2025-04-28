// // 1. [3 punkty] Program szyfrujący przy pomocy kryptografii klucza asymetrycznego. Napisz program który jako parametr przyjmuje typ polecenia. W zależności od wybranego typu polecenia:
// //     - Jeżeli typ polecenia = 0 program ma wygenerować i zapisać do dwóch plików (o dowolnych nazwach, można je wpisać "na sztywno") klucz publiczny oraz klucz prywatny algorytmu RSA.
// //     - Jeżeli typ polecenia = 1, program dodatkowo pobiera nazwę dwóch plików (a), (b). Podany plik (a) ma zostać zaszyfrowany przy pomocy klucza publicznego odczytanego z pliku, który został stworzony przy pomocy tego programu kiedy typ polecenia = 0. Zaszyfrowane dane mają być zapisane w pliku (b).
// //     - Jeżeli typ polecenia = 2, program dodatkowo pobiera nazwę dwóch plików (a), (b). Podany plik (a) ma zostać odszyfrowany przy pomocy klucza prywatnego odczytanego z pliku, który został stworzony przy pomocy tego programu kiedy typ polecenia = 0. Odszyfrowane dane mają być zapisane w pliku (b).

// using System;
// using System.IO;
// using System.Security.Cryptography;
// using System.Text;
// using System.Threading.Tasks;
// using System.Xml.Serialization;


// class Program
// {
//     static void Main(string[] args)
//     {
//         if (args.Length < 1)
//         {
//             Console.WriteLine("Usage: Program <command> [<inputFile> <outputFile>]");
//             return;
//         }

//         int command = int.Parse(args[0]);

//         switch (command)
//         {
//             case 0:
//                 GenerateKeys();
//                 break;
//             case 1:
//                 if (args.Length != 3)
//                 {
//                     Console.WriteLine("Usage: Program 1 <inputFile> <outputFile>");
//                     return;
//                 }
//                 EncryptFile(args[1], args[2]);
//                 break;
//             case 2:
//                 if (args.Length != 3)
//                 {
//                     Console.WriteLine("Usage: Program 2 <inputFile> <outputFile>");
//                     return;
//                 }
//                 DecryptFile(args[1], args[2]);
//                 break;
//             default:
//                 Console.WriteLine("Invalid command.");
//                 break;
//         }
//     }
//     static void GenerateKeys()
//     {
//         using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
//         {
//             // Export public key
//             string publicKey = rsa.ToXmlString(false);
//             File.WriteAllText("publicKey.xml", publicKey);

//             // Export private key
//             string privateKey = rsa.ToXmlString(true);
//             File.WriteAllText("privateKey.xml", privateKey);

//             Console.WriteLine("Keys generated and saved to publicKey.xml and privateKey.xml.");
//         }
//     }
//     static void EncryptFile(string inputFile, string outputFile)
//     {
//         using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
//         {
//             // Load public key
//             string publicKey = File.ReadAllText("publicKey.xml");
//             rsa.FromXmlString(publicKey);

//             // Read file data
//             byte[] dataToEncrypt = File.ReadAllBytes(inputFile);

//             // Encrypt data
//             byte[] encryptedData = rsa.Encrypt(dataToEncrypt, false);

//             // Write encrypted data to file
//             File.WriteAllBytes(outputFile, encryptedData);

//             Console.WriteLine($"File {inputFile} encrypted and saved to {outputFile}.");
//         }
//     }
//     static void DecryptFile(string inputFile, string outputFile)
//     {
//         using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
//         {
//             // Load private key
//             string privateKey = File.ReadAllText("privateKey.xml");
//             rsa.FromXmlString(privateKey);

//             // Read encrypted data
//             byte[] encryptedData = File.ReadAllBytes(inputFile);

//             // Decrypt data
//             byte[] decryptedData = rsa.Decrypt(encryptedData, false);

//             // Write decrypted data to file
//             File.WriteAllBytes(outputFile, decryptedData);

//             Console.WriteLine($"File {inputFile} decrypted and saved to {outputFile}.");
//         }
//     }
// }

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 3)
        {
            Console.WriteLine("Usage: Program <inputFile> <hashFile> <algorithm>");
            return;
        }

        string inputFile = args[0];
        string hashFile = args[1];
        string algorithmName = args[2].ToUpper();

        if (!File.Exists(inputFile))
        {
            Console.WriteLine($"Input file {inputFile} does not exist.");
            return;
        }

        string computedHash = ComputeHash(inputFile, algorithmName);

        if (!File.Exists(hashFile))
        {
            // Save computed hash to hash file
            File.WriteAllText(hashFile, computedHash);
            Console.WriteLine($"Hash saved to {hashFile}.");
        }
        else
        {
            // Read existing hash
            string existingHash = File.ReadAllText(hashFile).Trim();

            if (string.Equals(computedHash, existingHash, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Hash matches.");
            }
            else
            {
                Console.WriteLine("Hash does NOT match.");
            }
        }
    }

    static string ComputeHash(string filePath, string algorithmName)
    {
        using (FileStream stream = File.OpenRead(filePath))
        {
            HashAlgorithm algorithm;
            switch (algorithmName)
            {
                case "SHA256":
                    algorithm = SHA256.Create();
                    break;
                case "SHA512":
                    algorithm = SHA512.Create();
                    break;
                case "MD5":
                    algorithm = MD5.Create();
                    break;
                default:
                    throw new ArgumentException("Unsupported algorithm. Use SHA256, SHA512, or MD5.");
            }

            byte[] hashBytes = algorithm.ComputeHash(stream);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToUpperInvariant();
        }
    }
}
