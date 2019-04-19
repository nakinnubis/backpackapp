using System;
using System.Collections.Generic;

namespace Core.Web.Models
{
    public partial class BookingTicketAddon
    {
        public int BookingTicketAddonsId { get; set; }
        public int TicketId { get; set; }
        public int AddonId { get; set; }
        public int AddonCount { get; set; }

        public ActivityAddOns Addon { get; set; }
        public BookingTicket Ticket { get; set; }
    }
}
