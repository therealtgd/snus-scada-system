using ScadaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportManager
{
    class Program
    {
        private static ServiceReference.ReportManagerServiceClient client = new ServiceReference.ReportManagerServiceClient();
        static void Main(string[] args)
        {
            while (true)
                MainMenu();
        }

        private static void MainMenu()
        {
            Console.Clear();
            Console.WriteLine(MenuUtils.GetMenuHeader("Report Manager Menu"));
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Get alarms by date range");
            Console.WriteLine("2) Get alarms by priority");
            Console.WriteLine("3) Get tag values by date of arrival");
            Console.WriteLine("4) Get most recent AI tag values");
            Console.WriteLine("5) Get most recent DI tag values");
            Console.WriteLine("6) Get tag values by tag name");
            Console.WriteLine("x) Exit");
            Console.Write("\r\nSelect an option: ");

            switch (Console.ReadLine().ToLower())
            {
                case "1":
                    GetAlarmsByDate();
                    break;
                case "2":
                    GetAlarmsByPriority();
                    break;
                case "3":
                    GetTagValuesByDate();
                    break;
                case "4":
                    GetMostRecentAIValues();
                    break;
                case "5":
                    GetMostRecentDIValues();
                    break;
                case "6":
                    GetAllTagValuesByID();
                    break;
                case "x":
                    System.Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }

        private static void GetAlarmsByDate()
        {
            DateTime dateFrom = MenuUtils.GetDateTime("Date from", true);
            DateTime dateTo = MenuUtils.GetDateTime("Date to", true);
            string sortBy = MenuUtils.GetString("Sort by (date or priority):", true);
            bool descending = Convert.ToBoolean(MenuUtils.GetInt("Descending=1, Ascending=0: "));

            AlarmValue[] vals = client.GetAlarmsByDate(dateFrom, dateTo, sortBy, descending: descending);
            PrintAlarmValues(vals);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

        }

        private static void GetAlarmsByPriority()
        {
            int priority = MenuUtils.GetInt("Priority:", true);
            bool descending = Convert.ToBoolean(MenuUtils.GetInt("Descending=1, Ascending=0: "));

            AlarmValue[] vals = client.GetAlarmsByPriority(priority, descending: descending);
            PrintAlarmValues(vals);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void GetTagValuesByDate()
        {
            DateTime dateFrom = MenuUtils.GetDateTime("Date from", true);
            DateTime dateTo = MenuUtils.GetDateTime("Date to", true);
            bool descending = Convert.ToBoolean(MenuUtils.GetInt("Descending=1, Ascending=0: "));

            ServiceReference.TagValue[] vals = client.GetTagValuesByDate(dateFrom, dateTo, descending: descending);
            PrintTagValues(vals);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void GetMostRecentAIValues()
        {
            bool descending = Convert.ToBoolean(MenuUtils.GetInt("Descending=1, Ascending=0: "));

            ServiceReference.TagValue[] vals = client.GetMostRecentAIValues(descending: descending);
            PrintTagValues(vals);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void GetMostRecentDIValues()
        {
            bool descending = Convert.ToBoolean(MenuUtils.GetInt("Descending=1, Ascending=0: "));

            ServiceReference.TagValue[] vals = client.GetMostRecentDIValues(descending: descending);
            PrintTagValues(vals);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void GetAllTagValuesByID()
        {
            string id = MenuUtils.GetString("Tag name: ", true);
            bool descending = Convert.ToBoolean(MenuUtils.GetInt("Descending=1, Ascending=0: "));

            ServiceReference.TagValue[] vals = client.GetAllTagValuesByID(id, descending: descending);
            PrintTagValues(vals);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void PrintAlarmValues(AlarmValue[] vals)
        {
            if (vals.Length != 0)
            {
                foreach (AlarmValue value in vals)
                {
                    Console.WriteLine("=================================");
                    Console.WriteLine($"AlarmValue: Type={value.Type}, Priority={value.Priority}, Limit={value.Limit}, TagName={value.TagName}, Time={value.Time}, Value={value.Value}");
                }
            }
            else
            {
                Console.WriteLine("No alarm values which fit the criterion...");
            }
            
        }

        private static void PrintTagValues(ServiceReference.TagValue[] vals)
        {
            if (vals.Length != 0)
            {
                foreach (ServiceReference.TagValue value in vals)
                {
                    Console.WriteLine("=================================");
                    Console.WriteLine($"TagValue: TagName={value.TagName}, Time={value.Time}, Value={value.Value}, Type={value.Type}");
                }
            }
            else
            {
                Console.WriteLine("No tag values which fit the criterion...");
            }
        }

    }
}
