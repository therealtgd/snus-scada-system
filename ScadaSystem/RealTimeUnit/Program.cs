using ScadaModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RealTimeUnit
{
    class Program
    {
        private static CspParameters csp;
        private static RSACryptoServiceProvider rsa;

        const string EXPORT_FOLDER = @"C:\public_key\";
        const string PUBLIC_KEY_FILE = @"rsaPublicKey.txt";

        private static ServiceReference.RealTimeUnitServiceClient client = new ServiceReference.RealTimeUnitServiceClient();

        static void Main(string[] args)
        {
            Program program = new Program();
            CreateAsmKeys();
            ExportPublicKey();
            program.AddRealTimeUnit();

        }
        
        private void AddRealTimeUnit()
        {
            while (true)
            {
                Console.WriteLine(MenuUtils.GetMenuHeader("Add RTU"));
                string id = MenuUtils.GetString("Id:", required: true);
                double lowLimit = MenuUtils.GetDouble("Low limit:", required: true);
                double highLimit = MenuUtils.GetDouble("High limit:", required: true);
                string address = MenuUtils.GetString("RTD Address:", required: true);

                string message = id + "," +  address + "," + lowLimit + "," + highLimit;
                byte[] signature = SignMessage(message);

                if (!client.AddRTU(message, signature))
                {
                    Console.WriteLine("Failed to add RTU");
                    Console.WriteLine("Press any key to continue or 'x' to quit");
                    switch (Console.ReadLine().ToLower())
                    {
                        case "x":
                            System.Environment.Exit(0);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Successfuly added RTU");
                    Console.WriteLine("Press any key to start sending values");
                    Console.ReadKey();

                    Console.WriteLine("Sending values...");
                    while (true)
                    {
                        double value = new Random().NextDouble() * (highLimit-lowLimit) + lowLimit;
                        Console.WriteLine($"Sending value: {value}, to address: {address}");
                        message = address + "," + value;
                        signature = SignMessage(message);
                        client.SendValue(message, signature);
                        Thread.Sleep(3000);
                    }
                }
            }

        }

        public static void CreateAsmKeys()
        {
            csp = new CspParameters();
            rsa = new RSACryptoServiceProvider(csp);
        }

        private static byte[] SignMessage(string message)
        {
            using (SHA256 sha = SHA256Managed.Create())
            {
                var hashValue = sha.ComputeHash(Encoding.UTF8.GetBytes(message));
                var formatter = new RSAPKCS1SignatureFormatter(rsa);
                formatter.SetHashAlgorithm("SHA256");
                return formatter.CreateSignature(hashValue);
            }
        }


        private static void ExportPublicKey()
        {
            //Kreiranje foldera za eksport ukoliko on ne postoji
            if (!(Directory.Exists(EXPORT_FOLDER)))
                Directory.CreateDirectory(EXPORT_FOLDER);
            string path = Path.Combine(EXPORT_FOLDER, PUBLIC_KEY_FILE);
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.Write(rsa.ToXmlString(false));
            }
        }

        private static void DisplayMessage(string message, bool keyToContinue = false)
        {
            Console.WriteLine(message);
            if (keyToContinue)
            {
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }


    }
}
