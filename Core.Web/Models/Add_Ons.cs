using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class Add_Ons
    {
      
        public Add_Ons()
        {
            this.Activity_Add_Ons = new HashSet<Activity_Add_Ons>();
        }

        public int id { get; set; }
        public string name { get; set; }
        public string icon { get; set; }

        [ForeignKey("user")]
        public Nullable<int> add_by { get; set; }
        public Nullable<bool> isdeleted { get; set; }

        public virtual User user { get; set; }
        public virtual ICollection<Activity_Add_Ons> Activity_Add_Ons { get; set; }
    }
}
