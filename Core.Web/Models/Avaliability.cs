using Core.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class Avaliability
    {
        public int id { get; set; }
        [ForeignKey("Activity")]
        public Nullable<int> activity_id { get; set; }
        public Nullable<System.DateTime> activity_Start { get; set; }
        public Nullable<System.DateTime> activity_End { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string starthour { get; set; }
        public string endhour { get; set; }
        public bool? reoccuring { get; set; }
        // return 1 in case of group, 0 for individual
        public Nullable<int> isForGroup { get; set; }
        public Nullable<int> isForIndividual { get; set; }
        public Nullable<decimal> group_Price { get; set; }
        public Nullable<int> total_tickets { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual ICollection<Avaliability_Pricing> Avaliability_Pricings { get; set; }
    }
}
