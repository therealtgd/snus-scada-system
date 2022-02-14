using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager
{
    class DatabaseManager
    {
        private DbContext DbContext;

        public void ChangeOutputValue() { }
        public float GetOutputValue() { return 0; }
        public void TurnScanOn() { }
        public void TurnScanOff() { }
        public void AddTag() { }
        public void RemoveTag() { }
        public void Login() { }
        public void Logout() { }
        public void Register() { }
    }
}
