using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager
{
    public class Tag
    {
        [Key]
        public int TagName { get; set; }
        public string Description { get; set; }
        public string IOAddress { get; set; }
    }
}
