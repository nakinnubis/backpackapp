using System;
using System.Collections.Generic;

namespace Core.Web.Models
{
    public partial class OrganizerType
    {
        public OrganizerType()
        {
            ActivityOrganizer = new HashSet<ActivityOrganizer>();
        }

        public int Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        public ICollection<ActivityOrganizer> ActivityOrganizer { get; set; }
    }
}
