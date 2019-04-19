using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Core.Web.Data;
using Core.Web.Models;

namespace Core.Web.Models
{
    public class Booking_Ticket
    {
        [Key]
        public int ticket_id { get; set; }

        [ForeignKey("Booking")]
        public Nullable<int> booking_id { get; set; }
       public Nullable<long> ticket_number { get; set; }

        public string name { get; set; }
        public string mail { get; set; }
        public string mobile { get; set; }

        [ForeignKey("user")]
        public int userId { get; set; }
        public bool primaryTicket { get; set; }
        public bool ticket_reviewd { get; set; }
        public bool ticket_checked_in { get; set; }
        public bool ticket_cancelled { get; set; }
        public bool user_verified { get; set; }
        public bool isGroupTicket { get; set; }
        public int numOfGroup { get; set; }
        public string nameOfGroup { get; set; }
        [ForeignKey("IndividualCategory")]
        public Nullable<int> category_id { get; set; }
        public Nullable<decimal> ticket_price { get; set; }

        public virtual ICollection<Booking_Ticket_Addon> Booking_Ticket_Addon { get; set; }
        public virtual ICollection<TicketMessage> TicketMessages { get; set; }
        public virtual Booking Booking { get; set; }
        public virtual IndividualCategory IndividualCategory { get; set; }
        public virtual User user { get; set; }

    }
}
