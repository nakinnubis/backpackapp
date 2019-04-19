using Core.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class ReviewReports
    {
        [Key]
        public int reportId { get; set; }

        [ForeignKey("Reviews")]
        public int reviewId { get; set; }
        public DateTime reportDate { get; set; }
   

        [ForeignKey("User")]
        public Nullable<int> user_id { get; set; }
        public virtual User User { get; set; }
        public virtual Reviews  Reviews{ get; set; }

    }
}
