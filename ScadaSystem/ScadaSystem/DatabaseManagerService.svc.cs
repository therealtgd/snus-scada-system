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
        private const string XML_FILE = "scadaConfig.xml"; 

        private static Dictionary<string, User> authenticatedUsers = new Dictionary<string, User>();
        private static readonly object usersLocker = new object();

        private static Dictionary<string, Tag> tags = new Dictionary<string, Tag>();
        private static readonly object tagsLocker = new object();

        private static IDriver simulationDriver = new SimulationDriver();

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

        public void ChangeOutputValue(string name, double value)
        {
            lock (tagsLocker)
            {
                if (tags.ContainsKey(name))
                {
                    if (tags[name] is OutTag)
                    {
                        ((OutTag)tags[name]).Value = value;
                    }
                }
            }
        }

        public double GetOutputValue(string name)
        {
            lock (tagsLocker)
            {
                if (tags.ContainsKey(name))
                {
                    if (tags[name] is OutTag)
                    {
                        return ((OutTag)tags[name]).Value;
                    }
                }
                return -1;
            }
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

        public bool Registration(string username, string password)
        {
            string encryptedPassword = EncryptData(password);
            User user = new User(username, encryptedPassword);
            using (var db = new UsersContext())
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
            using (var db = new UsersContext())
            {
                Registration("admin", "admin");
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

        public static void XmlSerialisation()
        {
            using (var writer = new StreamWriter(XML_FILE))
            {
                var serializer = new XmlSerializer(typeof(List<Tag>));
                serializer.Serialize(writer, tags.Values.ToList());
                Console.WriteLine("Serialization finished");
            }

        }

        public void XmlDeserialisation()
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

                    if ((bool)(tags?.Any()))
                    {
                        foreach (Tag tag in tagsList)
                        {
                            if (tag is InTag)
                            {
                                simulationDriver = ((InTag)tag).Driver;
                                break;
                            }
                        }
                    }
                    lock (tagsLocker)
                    { 
                        if (tagsList != null)
                        {
                            tags = tagsList.ToDictionary(tag => tag.Name);
                        }
                    }
                }
            }
            if (simulationDriver == null)
            {
                simulationDriver = new SimulationDriver();
            }
        }

    }
}
