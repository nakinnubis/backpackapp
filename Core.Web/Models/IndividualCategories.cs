using System;
using System.Collections.Generic;

namespace Core.Web.Models
{
    public partial class IndividualCategories
    {
        public IndividualCategories()
        {
            AvaliabilityPricings = new HashSet<AvaliabilityPricings>();
            BookingIndividualCategoryCapacity = new HashSet<BookingIndividualCategoryCapacity>();
            BookingTicket = new HashSet<BookingTicket>();
        }

        public int Id { get; set; }
        public int Activityid { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal PriceAfterDiscount { get; set; }
        public int? Capacity { get; set; }

        public Activity Activity { get; set; }
        public ICollection<AvaliabilityPricings> AvaliabilityPricings { get; set; }
        public ICollection<BookingIndividualCategoryCapacity> BookingIndividualCategoryCapacity { get; set; }
        public ICollection<BookingTicket> BookingTicket { get; set; }
    }
}
