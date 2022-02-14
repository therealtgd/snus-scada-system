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
        private int TagName;
        [DataMember]
        private string Description;
        [DataMember]
        private string IOAddress;
    }
}
