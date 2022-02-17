﻿using ScadaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ScadaSystem
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAlarmDisplayService" in both code and config file together.
    [ServiceContract(CallbackContract = typeof(IAlarmDisplayCallback))]
    public interface IAlarmDisplayService
    {
        [OperationContract]
        void Init();
    }

    public interface IAlarmDisplayCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnAlarmValue(Alarm alarm);
    }
}
