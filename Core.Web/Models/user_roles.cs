using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class User_roles
    {
        [Key]
        public int user_role_id { get; set; }

        [ForeignKey("user")]
        public int user_id { get; set; }
        [ForeignKey("roles")]
        public int role_id { get; set; }
        public int activity_id { get; set; }
        public virtual Roles roles { get; set; }
        public virtual User user { get; set; }
    }
}
