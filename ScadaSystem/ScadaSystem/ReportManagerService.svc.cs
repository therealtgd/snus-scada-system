using ScadaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ScadaSystem
{
    public class ReportManagerService : IReportManagerService
    {
        public List<AlarmValue> GetAlarmsByDate(DateTime dateFrom, DateTime dateTo, string sortBy, bool descending = false)
        {
            using (var db = new DatabaseContext())
            {
                try
                {
                    var vals = db.AlarmValues.Where(aV => aV.Time >= dateFrom && aV.Time <= dateTo);
                    if (sortBy.Equals("date"))
                    {
                        if (descending)
                            vals = vals.OrderByDescending(aV => aV.Time);
                        else
                            vals = vals.OrderBy(aV => aV.Time);
                    }
                    if (sortBy.Equals("priority"))
                    {
                        if (descending)
                            vals = vals.OrderByDescending(aV => aV.Priority);
                        else
                            vals = vals.OrderBy(aV => aV.Priority);
                    }
                    return vals.ToList();
                }
                catch (Exception e)
                {
                    return new List<AlarmValue>();
                }
            }
        }

        public List<AlarmValue> GetAlarmsByPriority(int priority, bool descending = true)
        {
            using (var db = new DatabaseContext())
            {
                try
                {
                    var vals = db.AlarmValues.Where(aV => aV.Priority == priority);
                    if (descending)
                        vals = vals.OrderByDescending(aV => aV.Time);
                    else
                        vals = vals.OrderBy(aV => aV.Time);
                    return vals.ToList();
                }
                catch (Exception e)
                {
                    return new List<AlarmValue>();
                }
            }
        }

        public List<TagValue> GetAllTagValuesByID(string id, bool descending = true)
        {
            using (var db = new DatabaseContext())
            {
                try
                {
                    var vals = db.TagValues.Where(tV => tV.TagName == id);
                    if (descending)
                        vals = vals.OrderByDescending(aV => aV.Time);
                    else
                        vals = vals.OrderBy(aV => aV.Time);
                    return vals.ToList();
                }
                catch (Exception e)
                {
                    return new List<TagValue>();
                }
            }
        }

        public List<TagValue> GetMostRecentAIValues(bool descending = true)
        {
            List<TagValue> retVal = new List<TagValue>();
            using (var db = new DatabaseContext())
            {
                try
                {
                    var vals = db.TagValues.Where(tV => tV.Type == "AI");
                    foreach (TagValue v1 in vals)
                    {
                        foreach (TagValue v2 in retVal)
                        {
                            if (v1.TagName == v2.TagName && v1.Time > v2.Time)
                            {
                                retVal.Remove(v2);
                                retVal.Add(v1);
                            }
                        }
                    }
                    retVal = descending ? retVal.OrderByDescending(tV => tV.Time).ToList() : retVal.OrderBy(tV => tV.Time).ToList();
                    return retVal;
                }
                catch (Exception e)
                {
                    return new List<TagValue>();
                }
            }
        }

        public List<TagValue> GetMostRecentDIValues(bool descending = true)
        {
            List<TagValue> retVal = new List<TagValue>();
            using (var db = new DatabaseContext())
            {
                try
                {
                    var vals = db.TagValues.Where(tV => tV.Type == "DI");
                    foreach (TagValue v1 in vals)
                    {
                        foreach (TagValue v2 in retVal)
                        {
                            if (v1.TagName == v2.TagName && v1.Time > v2.Time)
                            {
                                retVal.Remove(v2);
                                retVal.Add(v1);
                            }
                        }
                    }
                    retVal = descending ? retVal.OrderByDescending(tV => tV.Time).ToList() : retVal.OrderBy(tV => tV.Time).ToList();
                    return retVal;
                }
                catch (Exception e)
                {
                    return new List<TagValue>();
                }
            }
        }

        public List<TagValue> GetTagValuesByDate(DateTime dateFrom, DateTime dateTo, bool descending = true)
        {
            using (var db = new DatabaseContext())
            {
                try
                {
                    var vals = db.TagValues.Where(tV => tV.Time >= dateFrom && tV.Time <= dateTo);
                    if (descending)
                        vals = vals.OrderByDescending(aV => aV.Value);
                    else
                        vals = vals.OrderBy(aV => aV.Value);
                    return vals.ToList();
                }
                catch (Exception e)
                {
                    return new List<TagValue>();
                }
            }
        }
    }
}
