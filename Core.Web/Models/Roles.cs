using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class Roles
    {
        [Key]
        public int role_id { get; set; }
        public string role_name_ar { get; set; }
        public string role_name_en { get; set; }
        public virtual ICollection<User_roles> userRoles { get; set; }
    }
}
