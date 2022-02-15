using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ScadaModels
{  
    [DataContract]
    public class AO : OutTag
    {
        public double LowLimit { get; set; }
        public double HighLimit { get; set;}
        public string Units { get; set;}
    }
}
