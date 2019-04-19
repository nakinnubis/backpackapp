using Core.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class ReviewReplies
    {
        [Key]
        public int replyId { get; set; }

        [ForeignKey("User")]
        public int userId { get; set; }
        public string reply { get; set; }
        public DateTime replyTime { get; set; }

        [ForeignKey("Reviews")]
        public int review_Id { get; set; }
        
        public virtual User User { get; set; }
        public virtual Reviews Reviews { get; set; }
    }
}
