using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class Option
    {
       
        public Option()
        {
            this.Activity_Option = new HashSet<Activity_Option>();
        }

        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
        public bool haveAge { get; set; }
  
        public virtual ICollection<Activity_Option> Activity_Option { get; set; }
    }
}
