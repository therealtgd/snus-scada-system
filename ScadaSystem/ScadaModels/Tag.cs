using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ScadaModels
{
    [DataContract]
    public class Tag
    {
        //public Tag(string name, string description, string ioaddress)
        //{
        //    Name = name;
        //    Description = description;
        //    IOAddress = ioaddress;
        //}

        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set;}
        [DataMember]
        public string IOAddress { get; set;}
    }
}
