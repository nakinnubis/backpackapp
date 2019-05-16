using System;
using System.Collections.Generic;
using Core.Web.Models;

namespace Core.Api.Controllers
{
    internal class TestBooking
    {
        public List<Avaliability> Availability { get; set; }
        public int? totalCapacity { get; set; }
        public DateTime? Bookingpate { get; set; }
    }
}