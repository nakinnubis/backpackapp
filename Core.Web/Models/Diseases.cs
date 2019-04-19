using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class Diseases
    {

        [Key]
        public int diseases_id { get; set; }
        public string diseases_name_ar { get; set; }
        public string diseases_name_en { get; set; }
        public virtual ICollection<User_Diseases> User_Diseases { get; set; }
    }
}
