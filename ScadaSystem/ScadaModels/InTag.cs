using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ScadaModels
{
    [DataContract]
    public class InTag : Tag
    {
        [DataMember]
        private Driver Driver { get; set; }
        [DataMember]
        private int ScanTime { get; set;}
        [DataMember]
        private bool OnOffScan { get; set;}
    }
}
