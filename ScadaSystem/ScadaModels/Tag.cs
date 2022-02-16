using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ScadaModels
{
    [DataContract]
    [XmlInclude(typeof(AI))]
    [XmlInclude(typeof(AO))]
    [XmlInclude(typeof(DI))]
    [XmlInclude(typeof(DO))]
    [KnownType(typeof(AI))]
    [KnownType(typeof(AO))]
    [KnownType(typeof(DI))]
    [KnownType(typeof(DO))]
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
