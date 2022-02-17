using ScadaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ScadaSystem
{
    [ServiceContract (CallbackContract = typeof(ITrendingCallback))]
    public interface ITrendingService
    {
        [OperationContract]
        void Init();
    }

    public interface ITrendingCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnInputValueChanged(string tagName, double value);
    }
}
