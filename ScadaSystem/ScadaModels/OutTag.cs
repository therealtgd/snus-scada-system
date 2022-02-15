using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ScadaModels
{
    [DataContract]
    public class OutTag : Tag
    {
        [DataMember]
        public double InitialValue { get; set; }
        [DataMember]
        public double Value { get; set; }
    }
}
