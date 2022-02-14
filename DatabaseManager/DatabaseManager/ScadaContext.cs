using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager
{
    public class ScadaContext : DbContext
    {
        public DbSet<DI> DIs { get; set; }
        public DbSet<DO> DOs { get; set; }
        public DbSet<AI> AIs { get; set; }
        public DbSet<AO> AOs { get; set; }
    }
}
