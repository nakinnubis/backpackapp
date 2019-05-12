using Core.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class Booking
    {
       
        public Booking()
        {
            this.Booking_Tickets = new HashSet<Booking_Ticket>();
           
        }

  
        public int id { get; set; }

        [ForeignKey("User")]
        public Nullable<int> user_id { get; set; }
        [ForeignKey("Activity")]
        public Nullable<int> activity_id { get; set; }
        [ForeignKey("Avaliability")]
        public Nullable<int> avaliability_id { get; set; }
        //[ForeignKey("User1")]
        public Nullable<int> customer_id { get; set; }
        public Nullable<int> booking_type { get; set; }
        public bool full_group { get; set; }
        [ForeignKey("PaymentMethods")]
        public int payment_method { get; set; }
        public Nullable<decimal> booking_amount { get; set; }
        public bool is_paid { get; set; }
        public DateTime bookingDate { get; set; }
        public virtual Activity Activity { get; set; }
        public virtual Avaliability Avaliability { get; set; }
        public virtual User User { get; set; }
        //public virtual User User1 { get; set; }
        public virtual PaymentMethods PaymentMethods { get; set; }
        public virtual ICollection<Booking_individual_category_capacity> Booking_individual_category_capacity { get; set; }
        public virtual ICollection<Booking_Ticket> Booking_Tickets { get; set; }
        public string time_option { get; set; }
    }
}
