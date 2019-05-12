using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Models
{
    public class BookingCalenderApi
    {
    }



  

    public class CalenderActivity
    {
        public IEnumerable<Availability> availability { get; set; }
    }

    public class Availability
    {
        public int activity_id { get; set; }
        public DateTime activity_Start { get; set; }
        public DateTime activity_End { get; set; }
        public int? isForGroup { get; set; }
        public int? isForIndividual { get; set; }
        public decimal? group_Price { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string starthour { get; set; }
        public string endhour { get; set; }
        public bool reoccuring { get; set; }
        public IEnumerable<Individual_Categories>  individual_categories { get; set; }
    }

    public class Individual_Categories
    {
        public int activityid { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public int price_after_discount { get; set; }
        public int capacity { get; set; }
    }























    //public class BookingCalenderModel
    //{
    //    public IEnumerable<Availability> availability { get; set; }
    //    public IEnumerable<Availability_Pricing> availability_pricing { get; set; }
    //    public IEnumerable<Booking> bookings { get; set; }
    //    public IEnumerable<Individual_Categories> individual_categories { get; set; }
    //}

    //public class Availability
    //{
    //    public int activity_id { get; set; }
    //    public string activity_Start { get; set; }
    //    public string activity_End { get; set; }
    //    public int isForGroup { get; set; }
    //    public string group_Price { get; set; }
    //    public int total_tickets { get; set; }
    //    public string startdate { get; set; }
    //    public string enddate { get; set; }
    //    public string starthour { get; set; }
    //    public string endhour { get; set; }
    //    public int reoccuring { get; set; }
    //}

    //public class Availability_Pricing
    //{
    //    public int avaliabilityId { get; set; }
    //    public int individualCategoryId { get; set; }
    //    public int price { get; set; }
    //    public int priceAfterDiscount { get; set; }
    //}

    //public class Booking
    //{
    //    public int user_id { get; set; }
    //    public int activity_id { get; set; }
    //    public int avaliability_id { get; set; }
    //    public int booking_type { get; set; }
    //    public int full_group { get; set; }
    //    public int payment_method { get; set; }
    //    public int booking_amount { get; set; }
    //    public int is_paid { get; set; }
    //    public string bookingDate { get; set; }
    //    public string customer_id { get; set; }
    //}

    //public class Individual_Categories
    //{
    //    public int activityid { get; set; }
    //    public string name { get; set; }
    //    public int price { get; set; }
    //    public int price_after_discount { get; set; }
    //    public int capacity { get; set; }
    //}

}
