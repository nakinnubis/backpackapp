using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class Activity_Rule
    {
        public int id { get; set; }
        [ForeignKey("Activity")]
        public Nullable<int> activity_id { get; set; }

        [ForeignKey("Rule")]
        public Nullable<int> rule_id { get; set; }

  
        public virtual Activity Activity { get; set; }
        public virtual Rule Rule { get; set; }
    }
}
