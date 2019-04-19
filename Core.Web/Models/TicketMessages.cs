using System;
using System.Collections.Generic;

namespace Core.Web.Models
{
    public partial class TicketMessages
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public int FromUser { get; set; }
        public int ToUser { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }

        public User FromUserNavigation { get; set; }
        public BookingTicket Ticket { get; set; }
    }
}
