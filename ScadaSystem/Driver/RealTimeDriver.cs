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

        public double ReturnValue(string address)
        {
            return 0;
        }
    }
}
