using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ScadaSystem
{
    [ServiceContract]
    public interface IRealTimeUnitService
    {
        [OperationContract]
        bool AddRTU(string message, byte[] signature);
        [OperationContract]
        void SendValue(string message, byte[] signature);
    }
}
