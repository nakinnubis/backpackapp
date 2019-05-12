using Core.Web.Models;
using System;
using System.Collections.Generic;

namespace Core.Api.Models
{
    public class Reservation
    {
        public int Booking_id { get; set; }
        public Nullable<int> activity_id { get; set; }
        public Nullable<int> remainingTickets { get; set; }
        public Nullable<int> booking_type { get; set; }
        public bool full_group { get; set; }
        public Nullable<System.DateTime> activity_Start { get; set; }
        public Nullable<System.DateTime> activity_End { get; set; }
        public string payment_method { get; set; }
        public string user_name { get; set; }
        public Nullable<decimal> booking_amount { get; set; }
        public bool is_paid { get; set; }
        public IEnumerable<ReservationTicketDetails> reservationTicketDetails { get; set; }
    }

    public class ReservationTicketDetails
    {
        public int Ticket_Id { get; set; }
        public int CustomerId { get; set; }
        public string UserPhoto_Url { get; set; }
        public Nullable<int> ChatId { get; set; }
        public bool MessageIcon { get; set; }
        public Nullable<Int64> ticket_number { get; set; }
        public string name { get; set; }
        public string mail { get; set; }
        public string mobile { get; set; }
        public bool primaryTicket { get; set; }
        public bool ticket_reviewd { get; set; }
        public bool ticket_checked_in { get; set; }
        public bool ticket_cancelled { get; set; }
        public bool user_verified { get; set; }
        public bool isGroupTicket { get; set; }
        public int numOfGroup { get; set; }
        public string nameOfGroup { get; set; }

    }
    public  class BookingDetails
    {
        public int? Booking_id { get; set; }
        public Nullable<int> activity_id { get; set; }
        public string activityName { get; set; }
        public Nullable<int> booking_type { get; set; }
        public bool full_group { get; set; }
        public Nullable<System.DateTime> activity_Start { get; set; } 
        public Nullable<System.DateTime> activity_End { get; set; }  
        public string payment_method { get; set; }
        public string user_name { get; set; }
        public Nullable<decimal> booking_amount { get; set; }
        public bool is_paid { get; set; }
        public IEnumerable<BookingTicketDetails> bookingTicketDetails { get; set; }
        public IEnumerable<AvaliabilityPricing> avaliabilityPricing { get; set; }
        public IEnumerable<CategoryModel> categoryModel { get; set; }
    }
    public class BookingDetailsGroup
    {
      
        public int? Booking_id { get; set; }
        public string activityName { get; set; }
        public decimal activity_group_price { get; set; }
        public Nullable<decimal> avaliability_group_price { get; set; }
        public Nullable<int> activity_id { get; set; }
        public Nullable<int> booking_type { get; set; }
        public bool full_group { get; set; }
        public Nullable<System.DateTime> activity_Start { get; set; }
        public Nullable<System.DateTime> activity_End { get; set; }
        public string payment_method { get; set; }
        public string user_name { get; set; }
        public Nullable<decimal> booking_amount { get; set; }
        public bool is_paid { get; set; }
        public IEnumerable<BookingTicketDetailsGroup> bookingTicketDetailsGroups { get; set; }
      
    }
    public class CategoryModel
    {
        public int category_id { get; set; }
        public int count { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
       // public decimal price_after_discount { get; set; }

    }
    public class BookingTicketDetails
    {
        public int Ticket_Id { get; set; }
        public Nullable<Int64> ticket_number { get; set; }
        public string name { get; set; }
        public string mail { get; set; }
        public string mobile { get; set; }
        public bool primaryTicket { get; set; }
        public bool ticket_reviewd { get; set; }
        public bool ticket_checked_in { get; set; }
        public bool ticket_cancelled { get; set; }
        public bool user_verified { get; set; }
        public bool isGroupTicket { get; set; }
        public int numOfGroup { get; set; }
        public string nameOfGroup { get; set; }
        public IEnumerable<Booking_Ticket_AddonsDetails> booking_Ticket_AddonsDetails { get; set; }
        

    }
    public class BookingTicketDetailsGroup
    {
        public int Ticket_Id { get; set; }
        public Nullable<Int64> ticket_number { get; set; }
        public string name { get; set; }
        public string mail { get; set; }
        public string mobile { get; set; }
        public bool primaryTicket { get; set; }
        public bool ticket_reviewd { get; set; }
        public bool ticket_checked_in { get; set; }
        public bool ticket_cancelled { get; set; }
        public bool user_verified { get; set; }
        public bool isGroupTicket { get; set; }
        public int numOfGroup { get; set; }
        public string nameOfGroup { get; set; }
        public IEnumerable<Booking_Ticket_AddonsDetailsGroup> booking_Ticket_AddonsDetailsGroups { get; set; }
    }
    public class Booking_Ticket_AddonsDetailsGroup {
        public int ticket_Id { get; set; }
        public int Addon_id { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public Nullable<decimal> price { get; set; }
        public string note { get; set; }
        public string provider_Username { get; set; }
        public int addonCount { get; set; }
    }

    public class Booking_Ticket_AddonsDetails
    {
        public int ticket_Id { get; set; }
        public int Addon_id { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public Nullable<decimal> price { get; set; }
        public string note { get; set; }
        public string provider_Username { get; set; }
        public int addons_number { get; set; }
      
    }

    public class reservation
    {
      
        //  public bool full_group { get; set; }

        public int? avaliabilityid { get; set; }
        public DateTime? activity_Start { get; set; }
        public DateTime? activity_End { get; set; }
        public IEnumerable<Booking> BookinfInfo { get; set; }
        public IEnumerable<IndividualCategoryModel> individualCategories { get; set; }

        //public IEnumerable<Booking_Ticket> BookingTickets { get; set; }
        //  public decimal Activity_group_price { get; set; }
        //public IEnumerable<Avaliability> avaliabilities{get;set; }
        // return 1 in case of group, 0 for individual
        public Nullable<int> isForGroup { get; set; }
        public Nullable<int> total_tickets { get; set; }
        public Nullable<decimal> Availability_group_Price { get; set; }
        public Nullable<int> totalCapacity { get; set; }
        public IEnumerable<AvaliabilityPricing> avaliabilityPricing { get; set; }
       // public IEnumerable<IndividualCategoryModel> individualCategories { get; set; }

    }

    public class AvaliabilityPricing
    {
        public int id { get; set; }
        public int individualCategoryId { get; set; }
        public int price { get; set; }
        public int priceAfterDiscount { get; set; }

    }

    public class IndividualCategoryModel
    {
        public int id { get; set; }
        public int activity_Id { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public decimal price_after_discount { get; set; }

        public Nullable<int> capacity { get; set; }
    }
    public class CalenderReservation
    {
        //  public bool full_group { get; set; }

        public int? avaliabilityid { get; set; }
        public DateTime? activity_Start { get; set; }
        public DateTime? activity_End { get; set; }
        public IEnumerable<Booking> Bookings { get; set; }
        //public IEnumerable<Booking_Ticket> BookingTickets { get; set; }
        //  public decimal Activity_group_price { get; set; }
        public IEnumerable<Avaliability> avaliabilities { get; set; }
        // return 1 in case of group, 0 for individual
        public Nullable<int> isForGroup { get; set; }
        public Nullable<int> total_tickets { get; set; }
        public Nullable<decimal> Availability_group_Price { get; set; }
        public Nullable<int> totalCapacity { get; set; }
        public IEnumerable<Avaliability_Pricing> avaliabilityPricing { get; set; }
        public IEnumerable<IndividualCategory> individualCategories { get; set; }

    }

}
