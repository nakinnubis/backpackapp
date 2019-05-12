using Core.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Models
{
    public class CalenderActivityModel
    {
        public Avaliability Availabilities { get; set; }
        public AvaliabilityPricing AvaliabilityPricings { get; set; }
        public BookingModel Bookings { get; set; }
        public IndividualCategories IndividualCategories { get; set; }
        public Activity Activities { get; set; }
    }
}
