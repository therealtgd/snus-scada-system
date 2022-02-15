using ScadaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ScadaSystem
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TrendingService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TrendingService.svc or TrendingService.svc.cs at the Solution Explorer and start debugging.
    public class TrendingService : ITrendingService
    {
        private static List<InTag> inTags = new List<InTag>();

        private delegate void TagValueChangedDelegate(string name, double newValue);
        private static event TagValueChangedDelegate onTagValueChanged;
        private static readonly object eventLocker = new object();

        public List<InTag> getCurrentOutValues() => inTags;
    }
}
