using Driver;
using ScadaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace ScadaSystem
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TrendingService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TrendingService.svc or TrendingService.svc.cs at the Solution Explorer and start debugging.
    public class TrendingService : ITrendingService
    {

        public void initTrending()
        {
            TagProcessing.onInputChanged += OperationContext.Current.GetCallbackChannel<ITrendingCallback>().OnInputValueChanged;
        }
    }
}
