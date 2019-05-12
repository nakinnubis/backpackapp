using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Models
{
    public class UserOnlineActivity
    {
        public IEnumerable<ActivityCoverPhotos> ActivityCoverPhotos { get; set; }
        public int ActivityId { get; set; }
        public DateTime? LastEditedDate { get; set; }
        public string Title { get; set; }
        public int? UserId { get; set; }
    }
    public class ActivityCoverPhotos
    {
        public int activity_id { get; set; }
        public string url { get; set; }
        public bool cover_photo { get; set; }
    }

    public class GetActivityOption
    {
       
        public string title { get; set; }
        public string description { get; set; }
        public IEnumerable<Activity_Options> Activity_Options { get; set; }
    }

    public class Activity_Options
    {
        public int activityoptionid { get; set; }
        public string option_id { get; set; }
        public string fromAge { get; set; }
        public string toAge { get; set; }
        public string name { get; set; }
    }

}
