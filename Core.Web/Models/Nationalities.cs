using Core.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace Core.Web.Models
{
    public class Nationalities
    {

        [Key]
        public int nationality_id { get; set; }
        public string nationality_name_ar { get; set; }
        public string nationality_name_en { get; set; }

        public virtual ICollection<UserIdentification> UserIdentifications { get; set; }
      
    }

  
}
