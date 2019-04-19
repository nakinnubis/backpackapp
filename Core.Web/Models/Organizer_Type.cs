using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class Organizer_Type
    {
       
        public Organizer_Type()
        {
            this.Activity_Organizer = new HashSet<Activity_Organizer>();
        }

        public int id { get; set; }
        public string type { get; set; }

        public string description { get; set; }
        public virtual ICollection<Activity_Organizer> Activity_Organizer { get; set; }
    }
}
