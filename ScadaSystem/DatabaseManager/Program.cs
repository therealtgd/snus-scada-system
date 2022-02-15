using ScadaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DatabaseManager
{
    class Program
    {
        private static ServiceReference.DatabaseManagerServiceClient client = new ServiceReference.DatabaseManagerServiceClient();
        private bool showMainMenu = true;
        private bool isLoggedIn = false;

        static void Main(string[] args)
        {
            

        }

        private void Run()
        {
            while (showMainMenu)
            {
                showMainMenu = MainMenu();
                while (isLoggedIn)
                {
                    ProgramMenu();
                }
            }
            
        }

        private bool MainMenu()
        {
            Console.Clear();
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Login");
            Console.WriteLine("x) Exit");
            Console.Write("\r\nSelect an option: ");

            switch (Console.ReadLine().ToLower())
            {
                case "1":
                    Login();
                    return !isLoggedIn;
                case "x":
                    return false;
                default:
                    return true;
            }
        }

        private bool ProgramMenu()
        {
            Console.Clear();
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Change output value");
            Console.WriteLine("2) Get output value");
            Console.WriteLine("3) Turn scan on");
            Console.WriteLine("4) Turn scan off");
            Console.WriteLine("5) Add tag");
            Console.WriteLine("6) Remove tag");
            Console.WriteLine("7) Register user");
            Console.WriteLine("0) Logout");
            Console.WriteLine("x) Logout & Exit");
            Console.Write("\r\nSelect an option: ");

            switch (Console.ReadLine().ToLower())
            {
                case "1":
                    ChangeOutputValue();
                    return true;
                case "2":
                    GetOutputValue();
                    return true;
                case "3":
                    TurnScanOn();
                    return true;
                case "4":
                    TurnScanOff();
                    return true;
                case "5":
                    AddTagMenu();
                    return true;
                case "6":
                    RemoveTag();
                    return true;
                case "7":
                    RegisterUser();
                    return true;
                case "0":
                    Logout();
                    return false;
                case "x":
                    Logout();
                    showMainMenu = false;
                    return false;
                default:
                    return true;
            }
        }

        private void AddTagMenu()
        {
            Console.Clear();
            Console.WriteLine("Choose tag type:");
            Console.WriteLine("1) Digital input");
            Console.WriteLine("2) Digital output");
            Console.WriteLine("3) Analog input");
            Console.WriteLine("4) Analog output");
            Console.Write("\r\nSelect an option: ");

            switch (Console.ReadLine().ToLower())
            {
                case "1":
                    DIMenu();
                    break;
                case "2":
                    DOMenu();
                    break;
                case "3":
                    AIMenu();
                    break;
                case "4":
                    AOMenu();
                    break;
                default:
                    return true;
            }
        }

        private void AOMenu()
        {
            throw new NotImplementedException();
        }

        private void AIMenu()
        {
            throw new NotImplementedException();
        }

        private void DOMenu()
        {
            throw new NotImplementedException();
        }

        private void DIMenu()
        {
            string name = GetTagName();
            string description = GetTagDescription();
            IDriver driver = GetTagDriver();
            string ioaddress = GetTagIOAddress();
            int scanTime = GetTagScanTime();
            bool scanEnabled = GetTagScanEnabled();

            Tag tag = new DI() {
                Name = name,
                Description = description,
                Driver = driver,
                IOAddress = ioaddress,
                ScanTime = scanTime,
                ScanEnabled = scanEnabled
            };
            if (client.AddTag(tag));

        }

        private bool GetTagScanEnabled()
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine("Scan time (0=Off, 1=On): ");
                switch (Console.ReadLine())
                {
                    case "0":
                        return false;
                    case "1":
                        return true;
                    default:
                        Console.WriteLine("Invalid value. Try again.");
                        continue;
                }
            }
            
        }

        private int GetTagScanTime()
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine("Scan time: ");
                string scanTimeStr = Console.ReadLine();
                int scanTime;
                if (int.TryParse(scanTimeStr, out scanTime))
                    return scanTime;
                else
                {
                    Console.WriteLine("Invalid value. Try again.");
                    continue;
                }
            }
            
        }  

        private string GetTagIOAddress()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("I/O Address*: ");
                string ioaddress = Console.ReadLine();
                if (ioaddress != "") return ioaddress; else continue;
            }
        }

        private IDriver GetTagDriver()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Driver*: ");
                string driver = Console.ReadLine();
                if (driver != "") return new SimulationDriver(); else continue;
            }
        }

        private string GetTagDescription()
        {
            Console.Clear();
            Console.WriteLine("Description: ");
            return Console.ReadLine();
        }

        private static string GetTagName()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Tag name*: ");
                string name = Console.ReadLine();
                if (name != "") return name; else continue;
            }
          
        }

        private void RemoveTag()
        {
            throw new NotImplementedException();
        }

        private void TurnScanOff()
        { 
            string tagName = GetTagName();
            if (tagName != "")
                client.TurnScanOff(tagName);
        }

        private void TurnScanOn()
        {
            string tagName = GetTagName();
            if (tagName != "")
                client.TurnScanOn(tagName);
        }

        

        private void GetOutputValue()
        {
            throw new NotImplementedException();
        }

        private void Logout()
        {
            throw new NotImplementedException();
        }

        private void ChangeOutputValue()
        {
            Console.Clear();
            Console.WriteLine("Enter tag name: ");
            string tagName = Console.ReadLine();
            Console.WriteLine("Enter value name: ");
        }

        private void Login()
        {
            Console.Clear();
            Console.WriteLine("Username: ");
            string username = Console.ReadLine();
            Console.WriteLine("Password: ");
            string password = Console.ReadLine();
            if (username != "" && password != "")
            {
                isLoggedIn = client.Login(username, password);
            }
        }

        private static bool RegisterUser()
        {
            bool register = true;
            while (register)
            {
                Console.Clear();
                Console.WriteLine("Username: ");
                string username = Console.ReadLine();
                if (username.ToLower() == "x") {
                    register = false;
                    break;
                }
                if (!ValidUsername(username))
                {
                    Console.WriteLine("Username must be [3-9] characters long, and can't contain special characters.");
                    continue;
                }
                break;
            }
            while (register)
            {
                Console.Clear();
                Console.WriteLine("Password: ");
                string password = Console.ReadLine();
                if (password.ToLower() == "x")
                {
                    register = false;
                    break;
                }
                if (!ValidPassword(password))
                {
                    Console.WriteLine("Username must be [3-9] characters long, and can't contain special characters.");
                    continue;
                }
                break;
            }
            return register;
        }

        private static bool ValidUsername(string username)
        {
            return Regex.IsMatch(username, "^[a-zA-Z][a-zA-Z0-9]{3,9}$");
        }

        private static bool ValidPassword(string password)
        {
            return password.Length >= 8;
        }

    }
}
