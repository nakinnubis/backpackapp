using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class Booking_individual_category_capacity
    {
        public int id { get; set; }
        [ForeignKey("Booking")]
        public int booking_id { get; set; }
        [ForeignKey("IndividualCategory")]
        public int category_id { get; set; }
        public int count { get; set; }
        public virtual Booking Booking { get; set; }
        public virtual IndividualCategory IndividualCategory { get; set; }

    }
}
