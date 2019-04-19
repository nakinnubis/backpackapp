using Core.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class Booking_Ticket_Addon
    {
        [Key]
        public int booking_ticket_addons_id { get; set; }
        [ForeignKey("Booking_Ticket")]
        public int ticketId { get; set; }
        [ForeignKey("Activity_Add_Ons")]
        public int addon_id { get; set; }
        public int addonCount { get; set; }
        public virtual Booking_Ticket Booking_Ticket { get; set; }
        public virtual Activity_Add_Ons Activity_Add_Ons { get; set; }
    }
}
