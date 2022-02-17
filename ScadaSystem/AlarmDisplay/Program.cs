using ScadaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AlarmDisplay
{
    class AlarmDisplayCallback : ServiceReference.IAlarmDisplayServiceCallback
    {
        public void OnAlarmValue(Alarm alarm)
        {
            Console.WriteLine("==================== Alarm ====================");
            for (int i = 0; i < alarm.Priority; i++)
            {
                Console.WriteLine(alarm.ToString());
            }
        }
    }


    class Program
    {
        private static ServiceReference.AlarmDisplayServiceClient client = new ServiceReference.AlarmDisplayServiceClient(new InstanceContext(new AlarmDisplayCallback()));
        static void Main(string[] args)
        {
            Console.WriteLine("Alarm Display client started...");
            client.Init();
            Console.ReadKey();
        }
    }
}
