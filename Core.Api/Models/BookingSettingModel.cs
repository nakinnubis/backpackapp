using Core.Web.Models;
using System;
using System.Collections.Generic;

namespace Core.Api.Models
{
    public class BookingSettingModel
    {
        public int activityid { get; set; }
        public int? notice_in_advance { get; set; }
        public int? booking_window { get; set; }
        public bool bookingAvailableForIndividuals { get; set; }
        public bool bookingAvailableForGroups { get; set; }
        public decimal group_price { get; set; }
        public int? totalCapacity { get; set; }
        public int? min_capacity_group { get; set; }
        public int? max_capacity_group { get; set; }
        public decimal? Activity_length { get; set; }
        public bool capacityIsUnlimited { get; set; }
        public bool apply_discount { get; set; }
        public float price_discount { get; set; }
        public bool? has_individual_categories { get; set; }
        public bool? has_specific_capacity { get; set; }
        public List<individual_categories> individual_categories { get; set; }
        public IEnumerable<Avaliability_Pricing> avalibilityPricingModels { get; set; }
        public  IEnumerable<Avaliability> avalibilityModels { get; set; }
        public AvalibilityModel avalibilityModel { get; set; }
        public IEnumerable<IndividualCategory> individualCategories { get; set; }  //****   
        public IEnumerable<Activity_Organizer> activity_Organizers { get; set; }
        
   
       // public bool apply_discount { get; set; }
    }
    public class individual_categories
    {
        public string name { get; set; }
        public string capacity { get; set; }
        public string price { get; set; }
        public string price_after_discount { get; set; }
    }
    public class AvalibilityModel
    {
        public int id { get; set; }
        public DateTime? activity_Start { get; set; }
        public DateTime? activity_End { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string starthour { get; set; }
        public string endhour { get; set; }
        public bool? reoccuring { get; set; }
        // return 1 in case of group, 0 for individual
        public int? isForGroup { get; set; }
        public decimal? group_Price { get; set; }
        public int? total_tickets { get; set; }
   
        
    }
    public class AvalibilityPricingModel
    {
        public int avalibilityId { get; set; }
        public int individualCategoryId { get; set; }
        public int price { get; set; }
        public int priceAfterDiscount { get; set; }
    }
    }
