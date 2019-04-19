using Core.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class IndividualCategory
    {

        public int id { get; set; }

        [ForeignKey("activity")]
        public int activityid { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public decimal price_after_discount { get; set; }

        public Nullable<int> capacity { get; set; }
        
        public virtual ICollection<Booking_individual_category_capacity> Booking_individual_category_capacity { get; set; }
        public virtual ICollection<Avaliability_Pricing> Avaliability_Pricings { get; set; }
        public virtual Activity activity { get; set; }
        public virtual ICollection<Booking_Ticket> Booking_Tickets { get; set; }


    }
}
