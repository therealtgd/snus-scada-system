using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ScadaSystem
{
    [ServiceContract]
    interface IDatabaseManager
    {
        [OperationContract]
        void ChangeOutputValue(); 
        [OperationContract]
        float GetOutputValue();
        [OperationContract]
        void TurnScanOn();
        [OperationContract]
        void TurnScanOff(); 
        [OperationContract]
        void AddTag();
        [OperationContract]
        void RemoveTag();
    }
}
