using Driver;
using ScadaModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Xml.Serialization;

namespace ScadaSystem
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DatabaseManagerService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select DatabaseManagerService.svc or DatabaseManagerService.svc.cs at the Solution Explorer and start debugging.
    public class DatabaseManagerService : IDatabaseManagerService
    {
        private static Dictionary<string, User> authenticatedUsers = new Dictionary<string, User>();
        private static readonly object usersLocker = new object();
        

        public DatabaseManagerService()
        {
        }

        public bool AddTag(Tag newTag)
        {
            lock (TagProcessing.tagsLocker)
            {
                if (!TagProcessing.tags.ContainsKey(newTag.Name))
                {
                    if (newTag is InTag)
                    {
                        if (TagProcessing.drivers.ContainsKey(((InTag)newTag).Driver) && TagProcessing.VerifyDriverAddress((InTag)newTag))
                        {
                            TagProcessing.tags.Add(newTag.Name, newTag);
                            TagProcessing.RunThread((InTag)newTag);
                            TagProcessing.XmlSerialisation();
                            return true;
                        }
                    }
                    else
                    {
                        TagProcessing.tags.Add(newTag.Name, newTag);
                        TagProcessing.XmlSerialisation();
                        return true;
                    }
                }
            }
            return false;
        }
        
        public bool RemoveTag(string name)
        {
            lock (TagProcessing.tagsLocker)
            {
                if (TagProcessing.tags.ContainsKey(name))
                {
                    if (TagProcessing.tags[name] is InTag)
                        TagProcessing.RemoveThread(name);
                    TagProcessing.tags.Remove(name);
                    TagProcessing.XmlSerialisation();
                    return true;
                }
            }
            return false;
        }

        public bool ChangeOutputValue(string name, double value)
        {
            lock (TagProcessing.tagsLocker)
            {
                if (TagProcessing.tags.ContainsKey(name))
                {
                
                    if (TagProcessing.tags[name] is OutTag)
                    {
                        if (TagProcessing.tags[name] is AO)
                        { 
                            ((AO)TagProcessing.tags[name]).Value = value;
                            TagProcessing.XmlSerialisation();
                            return true;
                        }
                        else if (TagProcessing.tags[name] is DO && (value == 1 || value == 0))
                        {
                            ((DO)TagProcessing.tags[name]).Value = value;
                            TagProcessing.XmlSerialisation();
                            return true;
                        }
                        return false;
                    }
                }
            }
            return false;
        }

        public double GetOutputValue(string name)
        {
            lock (TagProcessing.tagsLocker)
            {
                if (TagProcessing.tags.ContainsKey(name))
                {
                    if (TagProcessing.tags[name] is OutTag)
                    {
                        return ((OutTag)TagProcessing.tags[name]).Value;
                    }
                }
                return -1;
            }
        }

        public bool TurnScanOn(string name)
        {
            lock (TagProcessing.tagsLocker)
            {
                if (TagProcessing.tags.ContainsKey(name))
                {
                    if (TagProcessing.tags[name] is InTag)
                    {
                        ((InTag)TagProcessing.tags[name]).ScanEnabled = true;
                        TagProcessing.XmlSerialisation();
                        return true;
                    }
                }
            }
            return false;
        }

        public bool TurnScanOff(string name)
        {
            lock (TagProcessing.tagsLocker)
            {
                if (TagProcessing.tags.ContainsKey(name))
                {
                    if (TagProcessing.tags[name] is InTag)
                    {
                        ((InTag)TagProcessing.tags[name]).ScanEnabled = false;
                        TagProcessing.XmlSerialisation();
                        return true;
                    }
                }
            }
            return false;
        }

        public bool Registration(string username, string password)
        {
            string encryptedPassword = EncryptData(password);
            User user = new User(username, encryptedPassword);
            using (var db = new DatabaseContext())
            {
                try
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return true;
        }

        public Tuple<bool, string> Login(string username, string password)
        {
            using (var db = new DatabaseContext())
            {
                foreach (var user in db.Users)
                {
                    if (username == user.Username && ValidateEncryptedData(password, user.EncryptedPassword))
                    {
                        string token = GenerateToken(username);
                        lock (usersLocker)
                        {
                            authenticatedUsers.Add(token, user);    
                        }
                        return new Tuple<bool, string>(true, token);
                    }
                }
            }
            return new Tuple<bool, string>(false, "Login failed");
        }

        public bool Logout(string token)
        {
            lock (usersLocker)
            {
                return authenticatedUsers.Remove(token);
            }
        }

        private static string EncryptData(string valueToEncrypt)
        {
            string GenerateSalt()
            {
                RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
                byte[] salt = new byte[32];
                crypto.GetBytes(salt);
                return Convert.ToBase64String(salt);
            }
            string EncryptValue(string strValue)
            {
                string saltValue = GenerateSalt();
                byte[] saltedPassword = Encoding.UTF8.GetBytes(saltValue + strValue);
                using (SHA256Managed sha = new SHA256Managed())
                {
                    byte[] hash = sha.ComputeHash(saltedPassword);
                    return $"{Convert.ToBase64String(hash)}:{saltValue}";
                }
            }
            return EncryptValue(valueToEncrypt);
        }

        private static bool ValidateEncryptedData(string valueToValidate, string valueFromDatabase)
        {
            string[] arrValues = valueFromDatabase.Split(':');
            string encryptedDbValue = arrValues[0];
            string salt = arrValues[1];
            byte[] saltedValue = Encoding.UTF8.GetBytes(salt + valueToValidate);
            using (var sha = new SHA256Managed())
            {
                byte[] hash = sha.ComputeHash(saltedValue);
                string enteredValueToValidate = Convert.ToBase64String(hash);
                return encryptedDbValue.Equals(enteredValueToValidate);
            }
        }

        private string GenerateToken(string username)
        {
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            byte[] randVal = new byte[32];
            crypto.GetBytes(randVal);
            string randStr = Convert.ToBase64String(randVal);
            return username + randStr;
        }

        public bool AddAlarm(string name, Alarm alarm)
        {
            lock (TagProcessing.tagsLocker)
            {
                if (TagProcessing.tags.ContainsKey(name))
                {
                    if (TagProcessing.tags[name] is AI)
                    {
                        ((AI)TagProcessing.tags[name]).Alarms.Add(alarm);
                        TagProcessing.XmlSerialisation();
                        return true;
                    }
                }
            }
            return false;
        }

        public bool RemoveAlarm(string name, int id)
        {
            lock (TagProcessing.tagsLocker)
            {
                if (TagProcessing.tags.ContainsKey(name))
                {
                    if (TagProcessing.tags[name] is AI)
                    {
                        try
                        {
                            ((AI)TagProcessing.tags[name]).Alarms.RemoveAt(id);
                            return true;
                        }
                        catch (Exception e)
                        {
                            return false;
                        }
                    }
                }
            }
            return false;
        }

        public List<Alarm> GetTagAlarms(string tagName)
        {
            lock (TagProcessing.tagsLocker)
            {
                if (TagProcessing.tags.ContainsKey(tagName))
                {
                    if (TagProcessing.tags[tagName] is AI)
                    {
                        return ((AI)TagProcessing.tags[tagName]).Alarms;
                    }
                }
            }
            return new List<Alarm>();
        }
    }
}
