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
        [DataMember]
        private int TagName { get; set; }
        [DataMember]
        private string Description { get; set;}
        [DataMember]
        private string IOAddress { get; set;}
    }
}
