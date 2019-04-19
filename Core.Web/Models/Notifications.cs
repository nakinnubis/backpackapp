using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class Notifications
    {
        [Key]
        public int notification_id { get; set; }
        public string notification_desc { get; set; }
        [ForeignKey("user")]
        public int user_id { get; set; }
        public virtual User user { get; set; }
    }
}
