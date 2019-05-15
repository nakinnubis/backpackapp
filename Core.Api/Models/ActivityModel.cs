using System;
using System.Collections.Generic;

namespace Core.Api.Models
{
    public class ActivityModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string activity_Location { get; set; }
        public bool? has_individual_categories { get; set; }
        public bool? has_specific_capacity { get; set; }
        public string meeting_Location { get; set; }
        public Nullable<decimal> activity_Lat { get; set; }
        public Nullable<decimal> activity_Lang { get; set; }
        public Nullable<decimal> meeting_Lat { get; set; }
        public Nullable<decimal> meeting_Lang { get; set; }
        public bool status { get; set; }
        public Nullable<int> notice_in_advance { get; set; }
        public Nullable<int> booking_window { get; set; }
        public Nullable<decimal> Activity_length { get; set; }
        //public Nullable<decimal> rate { get; set; }
        //public Nullable<int> total_tickets { get; set; }
        //public Nullable<int> remaining_tickets { get; set; }
        public string requirements { get; set; }
        public Nullable<int> totalCapacity { get; set; }
        public Nullable<int> min_capacity_group { get; set; }
        public Nullable<int> max_capacity_group { get; set; }
        public decimal group_price { get; set; }
        public bool apply_discount { get; set; }
        public bool bookingAvailableForIndividuals { get; set; }
        public bool bookingAvailableForGroups { get; set; }
        public int stepNumber { get; set; }
        public bool isCompleted { get; set; }
        public bool capacityIsUnlimited { get; set; }
        public IEnumerable<string> time_option { get; set; }


        public ActivityTypemodel Category { get; set; }
        public IEnumerable<photomodel> Activity_Photos { get; set; }
        public IEnumerable<addonsmodel> Activity_Add_Ons { get; set; }
        public IEnumerable<RuleModel> Activity_Rules { get; set; }
        public IEnumerable<OrganizerModel> Activity_Organizer { get; set; }
        public string Activity_Option { get; set; }
        public  IEnumerable<Avaliabilitymodel> Avaliabilities { get; set; }
        public string Individual_Categories { get; set; }
    }

    public class photomodel
    {
        public int id { get; set; }
        public string url { get; set; }
        public bool cover_photo { get; set; }

    }
    public class addonsmodel
    {
        public int addons_id { get; set; } //Eman
        public int activityAddons_id { get; set; } //Eman
        public string name { get; set; }
        public Nullable<decimal> price { get; set; }
        public string note { get; set; }
        public string provider_Username { get; set; }
        public int addons_number { get; set; }
        public string icon { get; set; }
    }

    public class Avaliabilitymodel
    {
        public int Id { get; set; }
        public Nullable<DateTime> activitystart { get; set; }
        public Nullable<DateTime> actvityEnd { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string starthour { get; set; }
        public string endhour { get; set; }
        public Nullable<int> isForGroup { get; set; }
        public Nullable<int> isForIndividual { get; set; }
     //   public IEnumerable<AvaliabilityPricing> avaliabilityPricings { get; set; }

    }

    public class ActivityTypemodel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string url { get; set; }
    }

    public class Individualmodel
    {
        public int Id { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public Nullable<decimal> priceafterdiscout { get; set; }
        public Nullable<int> Capacity { get; set; }

    }

    public class OrganizerModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int Type_id { get; set; }
        public string OrganizerType { get; set; }
        public string TypeDescription { get; set; }

    }

    public class RuleModel
    {
        public int Id { get; set; }
        public string description { get; set; }
    }
    public class OptionModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string fromAge { get; set; }
        public string toAge { get; set; }
        public string description { get; set; }
        public string icon { get; set; }

    }
}
