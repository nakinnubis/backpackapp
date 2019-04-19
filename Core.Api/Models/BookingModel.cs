using Core.Web.Models;
using System;
using System.Collections.Generic;

namespace Core.Api.Models
{
    public class BookingModel
    {
        public Nullable<int> activity_id { get; set; }
        public Nullable<int> avaliability_id { get; set; }

        //public Nullable<int> customer_id { get; set; }
        public Nullable<int> booking_type { get; set; }
        public bool full_group { get; set; }
        public Nullable<decimal> booking_amount { get; set; }
        public IEnumerable<BookingTicket> bookingTicket { get; set; }
        public  IEnumerable<Booking_individual_category_capacity> bookingIndividualCategoryCapacity { get; set; }

    }

  
    public class BookingTicket
    {
        public string name { get; set; }
        public string mail { get; set; }
        public string mobile { get; set; }
        public bool primaryTicket { get; set; }
        public bool ticket_reviewd { get; set; }
        public Nullable<int> ticket_number { get; set; }
        public Nullable<int> category_id { get; set; }
        public Nullable<decimal> ticket_price { get; set; }

        //{
        //    get { return ticket_number; }
        //    set
        //    {
        //        using (ApplicationDbContext db = new ApplicationDbContext())
        //        {
        //            var tickateum = db.Booking_Ticket.Where(x=>x.ticket_number>100000000).Max(x => x.ticket_number);
        //            ticket_number = (tickateum == null ? 100000000 : tickateum.Value + 1);
        //        }
        //    }

        //}
        public bool ticket_checked_in { get; set; }
        public bool ticket_cancelled { get; set; }
        public bool isGroupTicket { get; set; }
        public int numOfGroup { get; set; }
        public string nameOfGroup { get; set; }
        public IEnumerable<Booking_Ticket_AddonsModel> Booking_Ticket_AddonsModel { get; set; }
    }

    public class Booking_Ticket_AddonsModel
    {
        public int Addons_id { get; set; }
        public int ticket_id { get; set; }
        public int addonCount { get; set; }
    }

    public class BookingPaid
    {
        public int Booking_id { get; set; }
        public int payment_method { get; set; }
        public Nullable<decimal> booking_amount { get; set; }
    
    }

    public class MessageModel
    {
        public string message { get; set; }
    }
}
