using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ScadaModels
{
    public enum AlarmType
    {
        Low,
        High
    }

    [DataContract]
    [KnownType(typeof(AlarmValue))]
    public class Alarm
    {
        [DataMember]
        public AlarmType Type { get; set; }
        [DataMember]
        public int Priority { get; set; }
        [DataMember]
        public double Limit { get; set; }
        [DataMember]
        public string TagName { get; set; }

        public Alarm() { }
        public Alarm(AlarmType type, int priority, double limit, string tagName)
        {
            Type = type;
            Priority = priority;
            Limit = limit;
            TagName = tagName;
        }

        public override string ToString()
        {
            return $"Alarm: Type={Type}, Priority={Priority}, Limit={Limit}, TagName={TagName}";
        }
    }

    [DataContract]
    public class AlarmValue : Alarm
    {
        [Key]
        public int Id { get; set; }
        [DataMember]
        public DateTime Time { get; set; }
        [DataMember]
        public double Value { get; set; }
        
        public AlarmValue() { }
        public AlarmValue(Alarm alarm, DateTime time, double value) : base(alarm.Type, alarm.Priority, alarm.Limit, alarm.TagName)
        {
            Time = time;
            Value = value;
        }

        public override string ToString()
        {
            return base.ToString() + " " + $"Time={Time}, Value={Value}";
        }
    }
}