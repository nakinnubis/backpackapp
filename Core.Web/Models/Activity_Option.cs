using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class Activity_Option
    {
        public int id { get; set; }

        [ForeignKey("Activity")]
        public Nullable<int> activity_id { get; set; }

        [ForeignKey("Option")]
        public Nullable<int> option_id { get; set; }
        public string fromAge { get; set; }
        public string toAge { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual Option Option { get; set; }
    }
}
