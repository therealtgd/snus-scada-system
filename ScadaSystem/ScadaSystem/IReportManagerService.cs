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
    public interface IReportManagerService
    {
        [OperationContract]
        List<AlarmValue> GetAlarmsByDate(DateTime dateFrom, DateTime dateTo, string sortBy, bool descending = false);
        [OperationContract]
        List<AlarmValue> GetAlarmsByPriority(int priority, bool descending = false);
        [OperationContract]
        List<TagValue> GetTagValuesByDate(DateTime dateFrom, DateTime dateTo, bool descending = false);
        [OperationContract]
        List<TagValue> GetMostRecentAIValues(bool descending = false);
        [OperationContract]
        List<TagValue> GetMostRecentDIValues(bool descending = false);
        [OperationContract]
        List<TagValue> GetAllTagValuesByID(string id, bool descending = false);
    }
}
