using Core.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class Activity_Add_Ons
    {
        public int id { get; set; }
        [ForeignKey("Activity")]
        public Nullable<int> activity_id { get; set; }
        [ForeignKey("Add_Ons")]
        public Nullable<int> add_ons_id { get; set; }
        public Nullable<decimal> price { get; set; }
        public string note { get; set; }
        public string provider_Username { get; set; }
        public int addons_number { get; set; }

        public virtual Booking_Ticket_Addon Booking_Ticket_Addons { get; set; }
        public virtual Activity Activity { get; set; }
        public virtual Add_Ons Add_Ons { get; set; }
    }
}
