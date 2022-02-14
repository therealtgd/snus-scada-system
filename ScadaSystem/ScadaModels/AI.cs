using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ScadaModels
{
    [DataContract]
    public class AI : InTag
    {
        [DataMember]
        private long LowLimit { get; set; }
        [DataMember]
        private long HighLimit { get; set;}
        [DataMember]
        private string Units { get; set;}
        [DataMember]
        private List<Alarm> Alarms { get; set;}
    }
}
