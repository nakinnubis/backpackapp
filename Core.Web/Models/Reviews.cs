using Core.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class Reviews
    {
        [Key]
        public int reviewid { get; set; }
        [ForeignKey("Activity")]
        public Nullable<int> activity_id { get; set; }
        public Nullable<decimal> rate { get; set; }
        public string review { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        [ForeignKey("User")]
        public Nullable<int> user_id { get; set; }
        public bool isBlocked { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<ReviewReports> reviewReports { get; set; }
        public virtual ICollection<ReviewReplies> reviewReplies { get; set; }
    }
}
