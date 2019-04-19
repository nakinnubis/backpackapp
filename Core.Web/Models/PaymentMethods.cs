using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class PaymentMethods
    {
        public int id { get; set; }
        public string name { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
