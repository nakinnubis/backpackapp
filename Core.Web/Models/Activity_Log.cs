using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class Activity_Log
    {
        [Key]
        public int activity_log_id { get; set; }
        public int activity_id { get; set; }
        [ForeignKey("user")]
        public int user_id { get; set; }
        public string action { get; set; }
        public DateTime log_date { get; set; }
        public virtual User user { get; set; }
    }
}
