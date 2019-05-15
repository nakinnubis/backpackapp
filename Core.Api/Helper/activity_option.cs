using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Helper
{
   
    public class activity_option
    {
        public int activityoptionid { get; set; }
        public string option_id { get; set; }
        public object fromAge { get; set; }
        public object toAge { get; set; }
        public string name { get; set; }
    }
    public class individual_cat
    {
        public string name { get; set; }
        public string capacity { get; set; }
        public string price { get; set; }
        public string price_after_discount { get; set; }
    }
}
