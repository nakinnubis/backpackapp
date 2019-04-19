using Core.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class Identification_types
    {
        [Key]
        public int identification_type_id { get; set; }
        public string identification_type_ar { get; set; }
        public string identification_type_en { get; set; }
        public virtual ICollection<UserIdentification> UserIdentifications { get; set; }
    }
}
