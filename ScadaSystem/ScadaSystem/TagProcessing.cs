using Driver;
using ScadaModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Xml.Serialization;

namespace ScadaSystem
{
    public static class TagProcessing
    {
        private const string XML_FILE = "scadaConfig.xml";


        public static Dictionary<string, Tag> tags = new Dictionary<string, Tag>();
        public static readonly object tagsLocker = new object();

        public static readonly Dictionary<string, IDriver> drivers = new Dictionary<string, IDriver>();

        public delegate void InputChangedDelegate(string tagName, double value);
        public static event InputChangedDelegate onInputChanged;

        private static Dictionary<string, Thread> threads = new Dictionary<string, Thread>();


        private static readonly object locker = new object();

        static TagProcessing()
        {
            XmlDeserialisation();
            drivers.Add("SimulationDriver", new SimulationDriver());
            RunSimulation();
        }

        public static void RunSimulation()
        {
            foreach (Tag tag in tags.Values)
            {
                if (tag is InTag)
                    RunThread((InTag)tag);
            }
                
        }

        public static void RunThread(InTag tag)
        {
            Thread t = new Thread(new ParameterizedThreadStart(SimulateTag));
            threads.Add(tag.Name, t);
            t.Start(tag);
        }

        public static void SimulateTag(Object obj)
        {
            InTag tag = (InTag)obj;
            double value;
            while (true)
            {
                if (tag.ScanEnabled)
                {
                    lock (locker)
                    {
                        value = Math.Abs(drivers[tag.Driver].ReturnValue(tag.IOAddress));
                    }
                    if (tag is DI)
                    {
                        value = value < 50 ? 0 : 1;
                    }
                    else if (tag is AI)
                    {
                        AI t = (AI)tag;
                        value = value < t.LowLimit ? t.LowLimit : value;
                        value = value > t.HighLimit ? t.HighLimit : value;

                        // Check for alarm
                    }

                    TagValue tagValue = new TagValue(tagName: tag.Name, time: DateTime.Now, value: value, type: tag.GetType().Name);
                    using (var db = new DatabaseContext())
                    {
                        try
                        {
                            db.TagValues.Add(tagValue);
                            db.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error while writing to database");
                        }
                    }
                    onInputChanged?.Invoke(tag.Name, value);
                }
                Thread.Sleep(tag.ScanTime * 1000);
            }
        }


        public static void XmlSerialisation()
        {
            using (var writer = new StreamWriter(XML_FILE))
            {
                var serializer = new XmlSerializer(typeof(List<Tag>));
                serializer.Serialize(writer, TagProcessing.tags.Values.ToList());
                Console.WriteLine("Serialization finished");
            }

        }

        public static void XmlDeserialisation()
        {
            if (!File.Exists(XML_FILE))
            {
                Console.WriteLine("File doesn't exist");
            }
            else
            {
                using (var reader = new StreamReader(XML_FILE))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Tag>));
                    var tagsList = (List<Tag>)serializer.Deserialize(reader);
                    lock (tagsLocker)
                    {
                        if (tagsList != null)
                        {
                            TagProcessing.tags = tagsList.ToDictionary(tag => tag.Name);
                        }
                    }
                }
            }
        }

    }
}