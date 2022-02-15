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
                    break;
            }
        }

        private void AOMenu()
        {
            string name = GetString("Name:", true);
            string description = GetString("Description:", false);
            string ioaddress = GetString("I/O Address:", true);
            double initValue = GetDouble("Initial value:");
            double lowLimit = GetDouble("Low limit:");
            double hightLimit = GetDouble("High limit:");
            string units = GetString("Units:", false);

            AO tag = new AO()
            {
                Name = name,
                Description = description,
                IOAddress = ioaddress,
                InitialValue = initValue,
                LowLimit = lowLimit,
                HighLimit = hightLimit,
                Units = units
            };
            client.AddTag(tag);
        }

        private void AIMenu()
        {
            string name = GetString("Name:", true);
            string description = GetString("Description:", false);
            IDriver driver = GetTagDriver();
            string ioaddress = GetString("I/O Address:", true);
            int scanTime = GetTagScanTime();
            bool scanEnabled = GetTagScanEnabled();
            List<Alarm> alarms = GetTagAlarmsList();
            double lowLimit = GetDouble("Low limit:");
            double hightLimit = GetDouble("High limit:");
            string units = GetString("Units:", false);

            AI tag = new AI()
            {
                Name = name,
                Description = description,
                Driver = driver,
                IOAddress = ioaddress,
                ScanTime = scanTime,
                ScanEnabled = scanEnabled,
                Alarms = alarms,
                LowLimit = lowLimit,
                HighLimit = hightLimit,
                Units = units
            };
            client.AddTag(tag);
        }

        private void DOMenu()
        {
            string name = GetString("Name:", true);
            string description = GetString("Description:", false);
            string ioaddress = GetString("I/O Address:", true);
            double initialValue = GetDouble("Initial value:");

            DO tag = new DO()
            {
                Name = name,
                Description = description,
                IOAddress = ioaddress,
                InitialValue = initialValue,
            };
            client.AddTag(tag);
        }

        private void DIMenu()
        {
            string name = GetString("Name:", true);
            string description = GetString("Description:", false);
            IDriver driver = GetTagDriver();
            string ioaddress = GetString("I/O Address:", true);
            int scanTime = GetTagScanTime();
            bool scanEnabled = GetTagScanEnabled();

            DI tag = new DI() {
                Name = name,
                Description = description,
                Driver = driver,
                IOAddress = ioaddress,
                ScanTime = scanTime,
                ScanEnabled = scanEnabled
            };
            client.AddTag(tag);

        }

        private string GetString(string message, bool required)
        {
            Func<string, string> getStr = (m) =>
            {
                Console.Clear();
                Console.WriteLine(m);
                return Console.ReadLine();
            };

            if (!required)
                return getStr(message);
            else
            {
                while (required)
                {
                    Console.Clear();
                    Console.WriteLine("I/O Address*: ");
                    string ioaddress = getStr(message);
                    if (ioaddress != "") return ioaddress; else continue;
                }
            }
            return "";
        }

        private double GetDouble(string message)
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine(message);
                string valStr = Console.ReadLine();
                double val;
                if (double.TryParse(valStr, out val))
                    return val;
                else
                {
                    Console.WriteLine("Invalid value. Try again.");
                    continue;
                }
            }
        }

        private List<Alarm> GetTagAlarmsList()
        {
            return new List<Alarm>();
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

        private void RemoveTag()
        {
            throw new NotImplementedException();
        }

        private void TurnScanOff()
        { 
            string tagName = GetString("Tag name:", true);
            if (tagName != "")
                client.TurnScanOff(tagName);
        }

        private void TurnScanOn()
        {
            string tagName = GetString("Tag name:", true);
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
