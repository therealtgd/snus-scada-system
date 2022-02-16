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
        bool ChangeOutputValue(string name, double value);
        [OperationContract]
        double GetOutputValue(string name);
        [OperationContract]
        bool TurnScanOn(string name);
        [OperationContract]
        bool TurnScanOff(string name);
        [OperationContract]
        bool AddTag(Tag newTag);
        [OperationContract]
        bool RemoveTag(string name);

        [OperationContract]
        bool Registration(string username, string password);
        [OperationContract]
        Tuple<bool, string> Login(string username, string password);
        [OperationContract]
        bool Logout(string token);
    }
}
