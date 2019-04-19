using System;
using System.Collections.Generic;

namespace Core.Web.Models
{
    public partial class BookingTicket
    {
        public BookingTicket()
        {
            BookingTicketAddon = new HashSet<BookingTicketAddon>();
            TicketMessages = new HashSet<TicketMessages>();
        }

        public int TicketId { get; set; }
        public int? BookingId { get; set; }
        public long? TicketNumber { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Mobile { get; set; }
        public bool PrimaryTicket { get; set; }
        public bool TicketReviewd { get; set; }
        public bool TicketCheckedIn { get; set; }
        public bool TicketCancelled { get; set; }
        public bool UserVerified { get; set; }
        public bool IsGroupTicket { get; set; }
        public int NumOfGroup { get; set; }
        public string NameOfGroup { get; set; }
        public int? CategoryId { get; set; }
        public decimal? TicketPrice { get; set; }
        public int UserId { get; set; }

        public Booking Booking { get; set; }
        public IndividualCategories Category { get; set; }
        public ICollection<BookingTicketAddon> BookingTicketAddon { get; set; }
        public ICollection<TicketMessages> TicketMessages { get; set; }
    }
}
