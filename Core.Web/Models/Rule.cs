using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class Rule
    {
        public Rule()
        {
            this.Activity_Rules = new HashSet<Activity_Rule>();
        }
        public int id { get; set; }
        public string description { get; set; }
        [ForeignKey("user")]
        public Nullable<int> add_by { get; set; }
        public Nullable<bool> isdeleted { get; set; }
        public virtual User user { get; set; }
        public virtual ICollection<Activity_Rule> Activity_Rules { get; set; }
    }
}
