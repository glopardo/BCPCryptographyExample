using BCP.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Encrypter
{
    class Program
    {
        static void Main(string[] args)
        {
            const string certificateThumbPrint = "d4e2c6acb703896ad723d795bb94025fb0c89847";
            var dict = new Dictionary<string, string>();
            var i = 1;

            Console.WriteLine("---------------------------------------");
            Console.WriteLine("--   BCP Client Password Encrypter   --");
            Console.WriteLine("---------------------------------------\n");
            Console.Write("Enter client {0}'s id: ", i);

            var idCliente = Console.ReadLine();

            while (idCliente != string.Empty)
            {
                Console.Write("Enter client {0}'s connection string: ", i);
                var passCliente = Console.ReadLine();
                dict.Add(idCliente, passCliente);
                i++;
                Console.Write("Enter client {0}'s id: ", i);
                idCliente = Console.ReadLine();
            }

            using (var outputFile = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), "ClientsPasswords.txt")))
            {
                foreach (var entry in dict)
                {
                    var encryptedPass = CriptographyUtils.Encrypt(Encoding.UTF8.GetBytes(entry.Value), certificateThumbPrint);
                    outputFile.WriteLine("\"{0}\": \"{1}\"", entry.Key, Convert.ToBase64String(encryptedPass));
                }
            }
        }
    }
}
