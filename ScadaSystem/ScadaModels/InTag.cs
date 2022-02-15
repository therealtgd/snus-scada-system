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
        public IDriver Driver { get; set; }
        [DataMember]
        public int ScanTime { get; set;}
        [DataMember]
        public bool ScanEnabled { get; set;}

        //public InTag(string name, string description, IDriver driver, string ioaddress, int scanTime, bool scanEnabled) : base(name, description, ioaddress)
        //{
        //    Driver = driver;
        //    ScanTime = scanTime;
        //    ScanEnabled = scanEnabled;
        //}
    }
}
