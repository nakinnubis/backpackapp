using System;
using System.Collections.Generic;

namespace Core.Web.Models
{
    public partial class ActivityOption
    {
        public int Id { get; set; }
        public int? ActivityId { get; set; }
        public int? OptionId { get; set; }
        public string FromAge { get; set; }
        public string ToAge { get; set; }

        public Activity Activity { get; set; }
        public Option Option { get; set; }
    }
}
