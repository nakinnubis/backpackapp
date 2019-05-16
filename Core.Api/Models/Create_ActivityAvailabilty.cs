using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Models
{
    public class Create_ActivityAvailabilty
    {
        public IEnumerable<Avalibilitymodel> avalibilityModels { get; set; }
    }

    public class Avalibilitymodel
    {
        public DateTime activity_Start { get; set; }
        public DateTime activity_End { get; set; }
        public string startdate { get; set; }
        public string starthour { get; set; }
        public string enddate { get; set; }
        public string endhour { get; set; }
        public int isForGroup { get; set; }
    }
}


