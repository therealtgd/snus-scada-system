using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager
{
    public class AI : InTag
    {
        // public virtual List<Alarm> Alarms;
        public long LowLimit { get; set; }
        public long HighLimit { get; set; }
        public string Units { get; set; }
    }
}
