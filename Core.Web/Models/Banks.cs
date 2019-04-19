using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class Banks
    {
        [Key]
        public int bank_id { get; set; }
        public string bank_name_ar { get; set; }
        public string bank_name_en { get; set; }
        public virtual ICollection<User> user { get; set; }

    }
}
