using Core.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Models
{
    public class UserModel
    {
        
    }
    public class ChatItemModel
    {
        public int chatId { get; set; }
        public string activityPhoto_Url { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int unReadableMessageNo { get; set; }
        public int ticketId { get; set; }
        public DateTime lastMessage { get; set; }
    }
    
    public class UserDiseasesModel
    {
        public IEnumerable<User_Diseases> user_Diseases { get; set; }
        public FollowUpHealth FollowUpHealth { get; set; }
    }
    public class UserDiseases_Get
    {
        public IEnumerable<UserDiseases> user_DiseasesList { get; set; }

        public FollowUpHealthModel FollowUpHealth { get; set; }
    }
    public class UserDiseases
    {
        public int user_diseases_id { get; set; }
        public Nullable<int> diseases_id { get; set; }
        public string other { get; set; }
    }
    public class FollowUpHealthModel
    {
         
    public int FollowUpHealthId { get; set; }
    public bool hospitalization { get; set; }
    public string hospitalizationDetails { get; set; }
    public bool dietaryRestrictions { get; set; }
    public string dietaryRestrictionsDetails { get; set; }
    public bool medication { get; set; }
    public string medicationDetails { get; set; }


    }


    public class SearchActivities
    {
        public double MapLat { get; set; }  
        public double MapLng { get; set; }

    }

}
