using System;
using System.Collections.Generic;

namespace Core.Web.Models
{
    public partial class ActivityAddOns
    {
        public ActivityAddOns()
        {
            BookingTicketAddon = new HashSet<BookingTicketAddon>();
        }

        public int Id { get; set; }
        public int? ActivityId { get; set; }
        public int? AddOnsId { get; set; }
        public decimal? Price { get; set; }
        public string Note { get; set; }
        public string ProviderUsername { get; set; }
        public int AddonsNumber { get; set; }

        public Activity Activity { get; set; }
        public AddOns AddOns { get; set; }
        public ICollection<BookingTicketAddon> BookingTicketAddon { get; set; }
    }
}
