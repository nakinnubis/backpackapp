using System;
using System.Collections.Generic;

namespace Core.Web.Models
{
    public partial class BookingIndividualCategoryCapacity
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int CategoryId { get; set; }
        public int Count { get; set; }

        public Booking Booking { get; set; }
        public IndividualCategories Category { get; set; }
    }
}
