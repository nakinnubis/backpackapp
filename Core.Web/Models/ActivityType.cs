using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class ActivityType
    {
        
        public ActivityType()
        {
            this.Activities = new HashSet<Activity>();
        }
        public int id { get; set; }
        public string Name { get; set; }
        public string url { get; set; }


        public virtual ICollection<Activity> Activities { get; set; }
    }
}
