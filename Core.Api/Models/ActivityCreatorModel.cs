using Core.Web.Models;
using System;
using System.Collections.Generic;

namespace Core.Api.Models
{
    public class ActivityCreatorModel
    {
        public int id { get; set; }
       
        public Nullable<int> user_id { get; set; }
       
        public Nullable<int> type_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public bool has_individual_categories { get; set; }
        public bool has_specific_capacity { get; set; }
        public string activity_Location { get; set; }
        public string meeting_Location { get; set; }
        public Nullable<decimal> activity_Lat { get; set; }
        public Nullable<decimal> activity_Lang { get; set; }
        public Nullable<decimal> meeting_Lat { get; set; }
        public Nullable<decimal> meeting_Lang { get; set; }
        public bool LocationFlag { get; set; }
        public bool status { get; set; }
        public string requirements { get; set; }
        public bool isdeleted { get; set; }
        public DateTime? modified_date { get; set; }

        public  IEnumerable<Activity_Photo> Activity_Photos { get; set; }
        public  IEnumerable<Activity_Add_Ons> Activity_Add_Ons{ get; set; }
        public  IEnumerable<Activity_Option> Activity_Options { get; set; }
        public  IEnumerable<Activity_Rule> Activity_Rules { get; set; }

    }

    public class Activity_Photo
    {
        public int activityId { get; set; }
        public string url { get; set; }
    
        public string base64Img { get; set; }
        public bool cover_photo { get; set; }

    }

   



}
