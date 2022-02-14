using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager
{
    public class InTag : Tag
    {
        // public virtual Driver Driver { get; set; }
        public int ScanTime { get; set; }
        public bool OnOffScan { get; set; }
    }
}
