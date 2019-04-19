using System;
using System.Collections.Generic;

namespace Core.Web.Models
{
    public partial class AddOns
    {
        public AddOns()
        {
            ActivityAddOns = new HashSet<ActivityAddOns>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int? AddBy { get; set; }
        public bool? Isdeleted { get; set; }

        public User AddByNavigation { get; set; }
        public ICollection<ActivityAddOns> ActivityAddOns { get; set; }
    }
}
