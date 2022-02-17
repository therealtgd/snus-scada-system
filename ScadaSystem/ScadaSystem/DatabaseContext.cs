using ScadaModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ScadaSystem
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<TagValue> TagValues { get; set; }
        public DbSet<AlarmValue> AlarmValues { get; set; }
    }
}