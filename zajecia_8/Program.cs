// Exc1
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


// Exc2
// using System;
// using System.IO;
// using System.Security.Cryptography;
// using System.Text;

// class Program
// {
//     static void Main(string[] args)
//     {
//         if (args.Length != 3)
//         {
//             Console.WriteLine("Usage: Program <inputFile> <hashFile> <algorithm>");
//             return;
//         }

//         string inputFile = args[0];
//         string hashFile = args[1];
//         string algorithmName = args[2].ToUpper();

//         if (!File.Exists(inputFile))
//         {
//             Console.WriteLine($"Input file {inputFile} does not exist.");
//             return;
//         }

//         string computedHash = ComputeHash(inputFile, algorithmName);

//         if (!File.Exists(hashFile))
//         {
//             // Save computed hash to hash file
//             File.WriteAllText(hashFile, computedHash);
//             Console.WriteLine($"Hash saved to {hashFile}.");
//         }
//         else
//         {
//             // Read existing hash
//             string existingHash = File.ReadAllText(hashFile).Trim();

//             if (string.Equals(computedHash, existingHash, StringComparison.OrdinalIgnoreCase))
//             {
//                 Console.WriteLine("Hash matches.");
//             }
//             else
//             {
//                 Console.WriteLine("Hash does NOT match.");
//             }
//         }
//     }

//     static string ComputeHash(string filePath, string algorithmName)
//     {
//         using (FileStream stream = File.OpenRead(filePath))
//         {
//             HashAlgorithm algorithm;
//             switch (algorithmName)
//             {
//                 case "SHA256":
//                     algorithm = SHA256.Create();
//                     break;
//                 case "SHA512":
//                     algorithm = SHA512.Create();
//                     break;
//                 case "MD5":
//                     algorithm = MD5.Create();
//                     break;
//                 default:
//                     throw new ArgumentException("Unsupported algorithm. Use SHA256, SHA512, or MD5.");
//             }

//             byte[] hashBytes = algorithm.ComputeHash(stream);
//             return BitConverter.ToString(hashBytes).Replace("-", "").ToUpperInvariant();
//         }
//     }
// }


//Exc 3
// using System;
// using System.IO;
// using System.Security.Cryptography;
// using System.Text;
// using System.Xml.Serialization;

// class Program
// {
//     static void Main(string[] args)
//     {
//         if (args.Length != 2)
//         {
//             Console.WriteLine("Usage: Program <inputFile> <signatureFile>");
//             return;
//         }

//         string inputFile = args[0];
//         string signatureFile = args[1];

//         if (!File.Exists(inputFile))
//         {
//             Console.WriteLine($"Input file {inputFile} does not exist.");
//             return;
//         }

//         if (!File.Exists("privateKey.xml") || !File.Exists("publicKey.xml"))
//         {
//             Console.WriteLine("Key files (privateKey.xml and publicKey.xml) are missing.");
//             return;
//         }

//         if (!File.Exists(signatureFile))
//         {
//             // Signature file doesn't exist, create it
//             byte[] data = File.ReadAllBytes(inputFile);
//             byte[] signature = SignData(data);

//             File.WriteAllBytes(signatureFile, signature);
//             Console.WriteLine($"Signature created and saved to {signatureFile}.");
//         }
//         else
//         {
//             // Signature file exists, verify
//             byte[] data = File.ReadAllBytes(inputFile);
//             byte[] signature = File.ReadAllBytes(signatureFile);

//             bool valid = VerifyData(data, signature);

//             if (valid)
//                 Console.WriteLine("Signature is valid.");
//             else
//                 Console.WriteLine("Signature is INVALID.");
//         }
//     }

//     static byte[] SignData(byte[] data)
//     {
//         using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
//         {
//             string privateKeyXml = File.ReadAllText("privateKey.xml");
//             rsa.FromXmlString(privateKeyXml);

//             // Use SHA256 as hash algorithm
//             return rsa.SignData(data, CryptoConfig.MapNameToOID("SHA256"));
//         }
//     }

//     static bool VerifyData(byte[] data, byte[] signature)
//     {
//         using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
//         {
//             string publicKeyXml = File.ReadAllText("publicKey.xml");
//             rsa.FromXmlString(publicKeyXml);

//             return rsa.VerifyData(data, CryptoConfig.MapNameToOID("SHA256"), signature);
//         }
//     }
// }

// public static class RSAExtensions
// {
//     public static void FromXmlString(this RSACryptoServiceProvider rsa, string xmlString)
//     {
//         var serializer = new XmlSerializer(typeof(RSAParameters));
//         using (var reader = new StringReader(xmlString))
//         {
//             RSAParameters parameters = (RSAParameters)serializer.Deserialize(reader);
//             rsa.ImportParameters(parameters);
//         }
//     }

//     public static string ToXmlString(this RSACryptoServiceProvider rsa, bool includePrivateParameters)
//     {
//         var parameters = rsa.ExportParameters(includePrivateParameters);
//         var serializer = new XmlSerializer(typeof(RSAParameters));
//         using (var writer = new StringWriter())
//         {
//             serializer.Serialize(writer, parameters);
//             return writer.ToString();
//         }
//     }
// }

// Exc 4
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class Program
{
    // Hardcoded salt (it could be random, but here fixed is fine for the exercise)
    private static readonly byte[] salt = Encoding.UTF8.GetBytes("SomeFixedSaltValue");

    static void Main(string[] args)
    {
        if (args.Length != 4)
        {
            Console.WriteLine("Usage: Program <inputFile> <outputFile> <password> <operationType>");
            Console.WriteLine("operationType: 0 = encrypt, 1 = decrypt");
            return;
        }

        string inputFile = args[0];
        string outputFile = args[1];
        string password = args[2];
        int operationType = int.Parse(args[3]);

        if (operationType == 0)
        {
            EncryptFile(inputFile, outputFile, password);
            Console.WriteLine($"File {inputFile} encrypted to {outputFile}.");
        }
        else if (operationType == 1)
        {
            DecryptFile(inputFile, outputFile, password);
            Console.WriteLine($"File {inputFile} decrypted to {outputFile}.");
        }
        else
        {
            Console.WriteLine("Invalid operation type.");
        }
    }

    static void EncryptFile(string inputFile, string outputFile, string password)
    {
        using (Aes aes = Aes.Create())
        {
            var key = DeriveKey(password, aes.KeySize / 8, aes.BlockSize / 8);
            aes.Key = key.Item1;
            aes.IV = key.Item2;

            using (FileStream fsInput = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
            using (FileStream fsEncrypted = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
            using (CryptoStream cs = new CryptoStream(fsEncrypted, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                fsInput.CopyTo(cs);
            }
        }
    }

    static void DecryptFile(string inputFile, string outputFile, string password)
    {
        using (Aes aes = Aes.Create())
        {
            var key = DeriveKey(password, aes.KeySize / 8, aes.BlockSize / 8);
            aes.Key = key.Item1;
            aes.IV = key.Item2;

            using (FileStream fsEncrypted = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
            using (FileStream fsDecrypted = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
            using (CryptoStream cs = new CryptoStream(fsEncrypted, aes.CreateDecryptor(), CryptoStreamMode.Read))
            {
                cs.CopyTo(fsDecrypted);
            }
        }
    }

    static Tuple<byte[], byte[]> DeriveKey(string password, int keyBytes, int ivBytes)
    {
        using (var pdb = new Rfc2898DeriveBytes(password, salt, 10000)) // 10000 iterations
        {
            byte[] key = pdb.GetBytes(keyBytes);
            byte[] iv = pdb.GetBytes(ivBytes);
            return Tuple.Create(key, iv);
        }
    }
}

