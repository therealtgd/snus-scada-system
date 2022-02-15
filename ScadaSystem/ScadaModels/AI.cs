﻿using System;
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
        public double LowLimit { get; set; }
        [DataMember]
        public double HighLimit { get; set;}
        [DataMember]
        public string Units { get; set;}
        [DataMember]
        public List<Alarm> Alarms { get; set;}
    }
}
