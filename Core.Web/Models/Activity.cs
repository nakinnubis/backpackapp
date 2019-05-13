using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class Activity
    {

        public Activity()
        { 
            this.Activity_Photos = new HashSet<Activity_Photos>();
            this.Avaliabilities = new HashSet<Avaliability>();
            this.Bookings = new HashSet<Booking>();
            this.Reviews = new HashSet<Reviews>();
            this.Activity_Add_Ons = new HashSet<Activity_Add_Ons>();
            this.Activity_Option = new HashSet<Activity_Option>();
            this.Activity_Organizer = new HashSet<Activity_Organizer>();
            this.Activity_Rules = new HashSet<Activity_Rule>();
            this.Individual_Categories= new HashSet<IndividualCategory>();
        }
        public int id { get; set; }
        [ForeignKey("User")]
        public Nullable<int> user_id { get; set; }
        [ForeignKey("ActivityType")]
        public Nullable<int> type_id { get; set; }
        public string title { get; set; }
        public bool? has_individual_categories { get; set; }
        public bool? has_specific_capacity { get; set; }
        public string description { get; set; }
        public string activity_Location { get; set; }
        public string meeting_Location { get; set; }
        public Nullable<decimal> activity_Lat { get; set; }
        public Nullable<decimal> activity_Lang { get; set; }
        public Nullable<decimal> meeting_Lat { get; set; }
        public Nullable<decimal> meeting_Lang { get; set; }
        public bool LocationFlag { get; set; }
        public bool status { get; set; }
        public Nullable<int> notice_in_advance { get; set; }
        public Nullable<int> booking_window { get; set; }
        public Nullable<decimal> Activity_length { get; set; }
        [Range(1.00, 5.00,  ErrorMessage = "Rate Must be between 1 to 5.")]
        public Nullable<decimal> rate { get; set; }
        public Nullable<int> remaining_tickets { get; set; }
        public string requirements { get; set; }
        public Nullable<int> totalCapacity { get; set; }
        public bool capacityIsUnlimited { get; set; }
        public Nullable<int> min_capacity_group { get; set; }
        public Nullable<int> max_capacity_group { get; set; }
        public decimal group_price { get; set; }
        public bool apply_discount { get; set; }
        public float price_discount { get; set; }
        public bool bookingAvailableForIndividuals { get; set; }
        public bool bookingAvailableForGroups { get; set; }
        public bool isdeleted { get; set; }
        public DateTime? creation_date { get; set; }
        // 1 (create Title,Description,Type)
        // 2 (Create Photo)
        // 3 (Create Addons)
        // 4 (Create Location)
        // 5 (Create Rules &Requirments)
        // 6 (Create Booking Prefrences)
        // 7 (Create Availabilty)
        // 8 (Create Capacity & Length)
        // 9 (Create Pricing & Payment)
        // 10 (Create Organizer)  
        public int stepNumber { get; set; }
        public bool isCompleted { get; set; }

        public virtual ActivityType ActivityType { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Activity_Photos> Activity_Photos { get; set; }

        public virtual ICollection<Avaliability> Avaliabilities { get; set; }
      
        public virtual ICollection<Booking> Bookings { get; set; }
  
        public virtual ICollection<Reviews> Reviews { get; set; }
 
        public virtual ICollection<Activity_Add_Ons> Activity_Add_Ons { get; set; }
       
        public virtual ICollection<Activity_Option> Activity_Option { get; set; }

        public virtual ICollection<Activity_Organizer> Activity_Organizer { get; set; }

        public virtual ICollection<Activity_Rule> Activity_Rules { get; set; }
      
        public virtual ICollection<IndividualCategory> Individual_Categories { get; set; }
        public string individual_categories { get; set; }
        public DateTime? modified_date { get; set; }






    }
}



