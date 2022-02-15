using ScadaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ScadaSystem
{
    [ServiceContract]
    public interface IDatabaseManagerService
    {
        [OperationContract]
        void ChangeOutputValue(string name, double value);
        [OperationContract]
        double GetOutputValue(string name);
        [OperationContract]
        void TurnScanOn(string name);
        [OperationContract]
        void TurnScanOff(string name);
        [OperationContract]
        void AddTag(Tag newTag);
        [OperationContract]
        void RemoveTag(string name);
    }
}
