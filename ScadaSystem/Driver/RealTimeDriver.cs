using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Driver
{
    [DataContract]
    public class RealTimeDriver : IDriver
    {
        [DataMember]
        public static Dictionary<string, double> values = new Dictionary<string, double>();
        private static readonly object locker = new object();

        public double ReturnValue(string address)
        {
            double val = 0;
            lock (locker)
            {
                val = values.ContainsKey(address) ? values[address] : val;
            }
            return val;
        }
    }
}
