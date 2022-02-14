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
        private long LowLimit { get; set; }
        private long HighLimit { get; set;}
        private string Units { get; set;}
    }
}
