using Core.Web.Models;
using System.Collections.Generic;
namespace Core.Api.Models
{
    public class ActivityDetails
    {
        public int id { get; set; }

        public ActivityType Type { get; set; }
        public List<Activity_Photos> Photos { get; set; }
    }
}
