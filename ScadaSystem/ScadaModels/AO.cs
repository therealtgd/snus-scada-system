﻿using System;
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
        private long LowLimit;
        private long HighLimit;
        private string Units;
    }
}
