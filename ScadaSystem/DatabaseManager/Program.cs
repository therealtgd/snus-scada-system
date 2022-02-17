﻿using ScadaModels;
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
        private string userToken;

        static void Main(string[] args)
        {
            Program program = new Program();
            program.Run();
        }

        private void Run()
        {
            while (showMainMenu)
            {
                //client.Registration("admin", "admin");
                MainMenu();
                //isLoggedIn = true;
                //userToken = "admin/lnXWiMvewaBUmywRhWZrjeWDFCratlQtSHrresS8F8=";
                while (isLoggedIn)
                {
                    ProgramMenu();
                }
            }
        }

        private void MainMenu()
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
                    break;
                case "x":
                    showMainMenu = false;
                    break;
                default:
                    break;
            }
        }

        private void ProgramMenu()
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
                    break;
                case "2":
                    GetOutputValue();
                    break;
                case "3":
                    TurnScanOn();
                    break;
                case "4":
                    TurnScanOff();
                    break;
                case "5":
                    AddTagMenu();
                    break;
                case "6":
                    RemoveTag();
                    break;
                case "7":
                    RegisterUser();
                    break;
                case "0":
                    Logout();
                    break;
                case "x":
                    Logout();
                    showMainMenu = false;
                    break;
                default:
                    break;
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
            string description = GetString("Description:");
            string ioaddress = GetString("I/O Address:", true);
            double initValue = GetDouble("Initial value:");
            double lowLimit = GetDouble("Low limit:");
            double hightLimit = GetDouble("High limit:");
            string units = GetString("Units:");

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
            AddTag(tag);
        }

        private void AIMenu()
        {
            string name = GetString("Name:", true);
            string description = GetString("Description:");
            string driver = GetString("Driver:", true);
            string ioaddress = GetString("I/O Address:", true);
            int scanTime = GetTagScanTime();
            bool scanEnabled = GetTagScanEnabled();
            List<Alarm> alarms = GetTagAlarmsList(name);
            double lowLimit = GetDouble("Low limit:");
            double hightLimit = GetDouble("High limit:");
            string units = GetString("Units:");

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
            AddTag(tag);
        }

        private void DOMenu()
        {
            string name = GetString("Name:", true);
            string description = GetString("Description:");
            string ioaddress = GetString("I/O Address:", true);
            double initialValue = GetBinary("Initial value (0 or 1):");

            DO tag = new DO()
            {
                Name = name,
                Description = description,
                IOAddress = ioaddress,
                InitialValue = initialValue,
            };
            AddTag(tag);
        }

        private void DIMenu()
        {
            string name = GetString("Name:", true);
            string description = GetString("Description:");
            string driver = GetString("Driver:", true);
            string ioaddress = GetString("I/O Address:", true);
            int scanTime = GetTagScanTime();
            bool scanEnabled = GetTagScanEnabled();

            DI tag = new DI()
            {
                Name = name,
                Description = description,
                Driver = driver,
                IOAddress = ioaddress,
                ScanTime = scanTime,
                ScanEnabled = scanEnabled
            };
            AddTag(tag);

        }

        private static void AddTag(Tag tag)
        {
            if (client.AddTag(tag))
                DisplayMessage($"Added tag with name: {tag.Name}", true);
            else
                DisplayMessage($"Failed to add tag with name: {tag.Name}", true);
        }

        private string GetString(string message, bool required = false)
        {
            Func<string, string> getStr = (m) =>
            {
                //Console.Clear();
                Console.WriteLine(m);
                return Console.ReadLine();
            };

            if (!required)
                return getStr(message);
            else
            {
                while (true)
                {
                    string val = getStr(message);
                    if (val != "") return val; else continue;
                }
            }
        }

        private double GetDouble(string message, bool required = false)
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
                    if (val != 1000)
                        return val;
                    else Console.WriteLine("Value required");
                }
            }
        }

        private int GetBinary(string message, bool required = false)
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
                    if (val != 1000)
                        return val;
                    else Console.WriteLine("Value required");
                }
            }
        }

        private int GetInt(string message, bool required = false)
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
                    if (val != 1000)
                        return val;
                    else Console.WriteLine("Value required");
                }
            }
        }


        private List<Alarm> GetTagAlarmsList(string tagName)
        {
            Console.Clear();
            List<Alarm> alarmsList = new List<Alarm>();
            while (true)
            {
                Console.WriteLine("Alarm menu");
                Console.WriteLine("1) Add alarm");
                Console.WriteLine("x) Save & Exit");
                Console.Write("\r\nSelect an option: ");

                switch (Console.ReadLine().ToLower())
                {
                    case "1":
                        alarmsList = AddAlarm(alarmsList, tagName);
                        break;
                    case "x":
                        return alarmsList;
                    default:
                        break;
                } 
            }
        }

        private List<Alarm> AddAlarm(List<Alarm> alarmsList, string tagName)
        {
            int type;
            while (true)
            {
                type = GetInt("Type (0=Low, 1=High):", true);
                if (!Enum.IsDefined(typeof(AlarmType), type))
                {
                    DisplayMessage("Invalid alarm type. Try again.", keyToContinue: true);
                    continue;
                }
                break;
            }
            int priority = GetInt("Priority:", true);
            double limit = GetDouble("Limit: ", true);
            Alarm alarm = new Alarm() { Type = (AlarmType)type, Priority = priority, Limit = limit, TagName = tagName };
            alarmsList.Add(alarm);
            return alarmsList;
        }

        private bool GetTagScanEnabled()
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine("Scan enabled (0=Off, 1=On): ");
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

        private void RemoveTag()
        {
            string tagName = GetString("Tag name:", true);
            if (client.RemoveTag(tagName))
                DisplayMessage($"Removed tag with name: {tagName}", true);
            else
                DisplayMessage($"Failed to remove tag with name: {tagName}", true);
        }

        private void TurnScanOff()
        { 
            string tagName = GetString("Tag name:", true);
            if (client.TurnScanOff(tagName))
                DisplayMessage($"Turned scan off, tag: {tagName}", true);
            else
                DisplayMessage($"Failed to turn scan off, tag: {tagName}", true);
        }

        private void TurnScanOn()
        {
            string tagName = GetString("Tag name:", true);
            if (client.TurnScanOn(tagName))
                DisplayMessage($"Turned scan on, tag: {tagName}", true);
            else
                DisplayMessage($"Failed to turn scan off, tag: {tagName}", true);
        }

        

        private void GetOutputValue()
        {
            Console.Clear();
            string tagName = GetString("Tag name:", true);
            double val = client.GetOutputValue(tagName);
            if (val != -1)
                DisplayMessage($"Output Tag: {tagName}, value: {val}", keyToContinue: true);
            else
                DisplayMessage("Tag not found or isn't output tag.", true);
        }

        private void Logout()
        {
            if (isLoggedIn)
                isLoggedIn = !client.Logout(userToken);
        }

        private void ChangeOutputValue()
        {
            Console.Clear();
            string tagName = GetString("Tag name:", true);
            double newVal = GetDouble("New value:");
            if (client.ChangeOutputValue(tagName, newVal))
                DisplayMessage($"Tag: {tagName} value set to: {newVal}", keyToContinue: true);
            else
                DisplayMessage("Tag not found or value invalid.", true);
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

        private void Login()
        {
            string username = GetString("Username:", true);
            string password = GetString("Password:", true);
            if (username != "" && password != "")
            {
                Tuple<bool, string> parms = client.Login(username, password);
                isLoggedIn = parms.Item1;
                if (isLoggedIn)
                {
                    userToken = parms.Item2;
                    DisplayMessage("Login successful", true);
                }
                else
                {
                    DisplayMessage("Login failed", true);
                }
            }
        }

        private void RegisterUser()
        {
            string username = "";
            string password = "";
            bool register = true;
            while (register)
            {
                username = GetString("Username:", true);
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
                password = GetString("Password:", true);
                if (password.ToLower() == "x")
                {
                    register = false;
                    break;
                }
                if (!ValidPassword(password))
                {
                    Console.WriteLine("Password must be at least 8 characters long.");
                    continue;
                }
                break;
            }
            if (username != "" && password != "")
            {
                if (client.Registration(username, password))
                {
                    DisplayMessage("Registration successful", true);
                    return;
                }
            }
            DisplayMessage("Registration unsuccessful or canceled", true);
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
