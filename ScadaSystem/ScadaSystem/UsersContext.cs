using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ScadaSystem
{
    public class UsersContext : DbContext
    {
        public DbSet<User> Users { get; set; }
    }
}