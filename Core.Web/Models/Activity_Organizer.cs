using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class Activity_Organizer
    {
        public int id { get; set; }
        [ForeignKey("Organizer_Type ")]
        public Nullable<int> Organizer_Typeid { get; set; }
        [ForeignKey("Activity")]
        public Nullable<int> activity_id { get; set; }
        public string mail { get; set; }
        public string name { get; set; }
        public string mobile { get; set; }


        public virtual Activity Activity { get; set; }
        public virtual Organizer_Type Organizer_Type { get; set; }
    }
}
