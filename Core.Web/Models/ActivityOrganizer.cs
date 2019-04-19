using System;
using System.Collections.Generic;

namespace Core.Web.Models
{
    public partial class ActivityOrganizer
    {
        public int Id { get; set; }
        public int? OrganizerTypeid { get; set; }
        public int? ActivityId { get; set; }
        public string Mail { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }

        public Activity Activity { get; set; }
        public OrganizerType OrganizerType { get; set; }
    }
}
