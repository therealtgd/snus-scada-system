using ScadaSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Trending
{
    class TrendingCallback : ServiceReference.ITrendingServiceCallback
    {
        public void OnInputValueChanged(string tagName, double value)
        {
            Console.WriteLine($"Tag name: {tagName}, value changed to: {value}");
        }
    }


    class Program
    {
        private static ServiceReference.TrendingServiceClient client = new ServiceReference.TrendingServiceClient(new InstanceContext(new TrendingCallback()));
        static void Main(string[] args)
        {
            Console.WriteLine("Trending client started...");
            client.Init();
            Console.ReadKey();
        }
    }
}
