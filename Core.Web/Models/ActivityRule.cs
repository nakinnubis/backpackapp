using System;
using System.Collections.Generic;

namespace Core.Web.Models
{
    public partial class ActivityRule
    {
        public int Id { get; set; }
        public int? ActivityId { get; set; }
        public int? RuleId { get; set; }

        public Activity Activity { get; set; }
        public Rule Rule { get; set; }
    }
}
