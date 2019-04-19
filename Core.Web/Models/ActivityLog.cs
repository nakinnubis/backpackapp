using System;
using System.Collections.Generic;

namespace Core.Web.Models
{
    public partial class ActivityLog
    {
        public int ActivityLogId { get; set; }
        public int ActivityId { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; }
        public DateTime LogDate { get; set; }

        public User User { get; set; }
    }
}
