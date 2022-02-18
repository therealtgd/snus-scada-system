using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ScadaModels
{
    public class MenuUtils
    {
        public static string GetMenuHeader(string message)
        {
            int multiplier = 10;
            string decorator = new string('=', multiplier);
            return decorator + " " + message + " " + decorator;
        }

        public static string GetString(string message, bool required = false)
        {
            Func<string, string> getStr = (m) =>
            {
                //Console.Clear();
                Console.WriteLine(m);
                return Console.ReadLine();
            };

            if (!required)
                return getStr(message).Trim();
            else
            {
                while (true)
                {
                    string val = getStr(message);
                    if (val != "") return val.Trim(); else continue;
                }
            }
        }

        public static double GetDouble(string message, bool required = false)
        {
            Func<string, double> getDbl = (m) =>
            {
                while (true)
                {
                    //Console.Clear();
                    Console.WriteLine(m);
                    string valStr = Console.ReadLine();
                    double val;
                    if (valStr.Trim() == "")
                        return -1000;
                    else if (double.TryParse(valStr, out val))
                        return val;
                    else
                        Console.WriteLine("Value invalid");
                }
            };

            if (!required)
                return getDbl(message);
            else
            {
                while (true)
                {
                    double val = getDbl(message);
                    if (val != -1000)
                        return val;
                    else Console.WriteLine("Value required");
                }
            }
        }

        public static int GetBinary(string message, bool required = false)
        {

            Func<string, int> getBin = (m) =>
            {
                while (true)
                {
                    //Console.Clear();
                    Console.WriteLine(m);
                    string valStr = Console.ReadLine();
                    int val;
                    if (valStr.Trim() == "")
                        return -1000;
                    else if (int.TryParse(valStr, out val))
                        if (val == 0 || val == 1)
                            return val;
                        else
                            Console.WriteLine("Value must be 0 or 1");
                    else
                        Console.WriteLine("Value invalid");
                }
            };

            if (!required)
                return getBin(message);
            else
            {
                while (true)
                {
                    int val = getBin(message);
                    if (val != -1000)
                        return val;
                    else Console.WriteLine("Value required");
                }
            }
        }

        public static int GetInt(string message, bool required = false)
        {
            Func<string, int> getInt = (m) =>
            {
                while (true)
                {
                    //Console.Clear();
                    Console.WriteLine(m);
                    string valStr = Console.ReadLine();
                    int val;
                    if (valStr.Trim() == "")
                        return -1000;
                    else if (int.TryParse(valStr, out val))
                        return val;
                    else
                        Console.WriteLine("Value invalid");
                }
            };

            if (!required)
                return getInt(message);
            else
            {
                while (true)
                {
                    int val = getInt(message);
                    if (val != -1000)
                        return val;
                    else Console.WriteLine("Value required");
                }
            }
        }

        public static DateTime GetDateTime(string message, bool required = false)
        {
            Func<string, DateTime> getDate = (m) =>
            {
                while (true)
                {
                    Console.WriteLine(m + " (MM/DD/YYYY HH:MM:SS AM/PM):");
                    string date = Console.ReadLine();
                    DateTime dateOut;
                    if (date.Trim() == "")
                        return DateTime.Parse("1/11/1111 1:11:11 PM");
                    if (DateTime.TryParse(date, out dateOut))
                        return dateOut;
                    else
                        Console.WriteLine("Value invalid.");
                }
            };

            if (!required)
                return getDate(message);
            else
            {
                while (true)
                {
                    DateTime val = getDate(message);
                    if (val != DateTime.Parse("1/11/1111 1:11:11 PM"))
                        return val;
                    else Console.WriteLine("Value required");
                }
            }
        }
    }
    
    public class CryptoUtils
    {
        public static byte[] SignMessage(string message, RSACryptoServiceProvider rsa)
        {
            using (SHA256 sha = SHA256Managed.Create())
            {
                var hashValue = sha.ComputeHash(Encoding.UTF8.GetBytes(message));
                var formatter = new RSAPKCS1SignatureFormatter(rsa);
                formatter.SetHashAlgorithm("SHA256");
                return formatter.CreateSignature(hashValue);
            }
        }

    }
}
