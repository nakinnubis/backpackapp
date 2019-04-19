using Core.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class TicketMessage
    {
        [Key]
        public int id { get; set; }
        [ForeignKey("booking_Tickets")]
        public int ticketId { get; set; }
        [ForeignKey("user")]
        public int fromUser { get; set; }
       
        public int toUser { get; set; }
        public string Message { get; set; }
        public DateTime date { get; set; }
        public virtual Booking_Ticket booking_Tickets { get; set; }
        public virtual User  user { get; set; }
    
    }
}
