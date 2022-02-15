using ScadaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ScadaSystem
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DatabaseManagerService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select DatabaseManagerService.svc or DatabaseManagerService.svc.cs at the Solution Explorer and start debugging.
    public class DatabaseManagerService : IDatabaseManagerService
    {
        private static Dictionary<string, Tag> tags = new Dictionary<string, Tag>();
        private static readonly object tagsLocker = new object();

        public void AddTag(Tag newTag)
        {
            lock (tagsLocker)
            {
                if (!tags.ContainsKey(newTag.Name))
                {
                    tags.Add(newTag.Name, newTag);
                }
            }
        }
        
        public void RemoveTag(string name)
        {
            lock (tagsLocker)
            {
                if (tags.ContainsKey(name))
                {
                    tags.Remove(name);
                }
            }
        }

        public void ChangeOutputValue()
        {
            throw new NotImplementedException();
        }

        public float GetOutputValue()
        {
            throw new NotImplementedException();
        }

        public void TurnScanOn(string name)
        {
            lock (tagsLocker)
            {
                if (tags.ContainsKey(name))
                {
                    if (tags[name] is InTag)
                    {
                        ((InTag)tags[name]).ScanEnabled = true;
                    }
                }
            }
        }

        public void TurnScanOff(string name)
        {
            lock (tagsLocker)
            {
                if (tags.ContainsKey(name))
                {
                    if (tags[name] is InTag)
                    {
                        ((InTag)tags[name]).ScanEnabled = false;
                    }
                }
            }
        }

    }
}
