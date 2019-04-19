using Core.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class User_Diseases
    {
        [Key]
        public int user_diseases_id { get; set; }

        [ForeignKey("user")]
        public int user_id { get; set; }

        [ForeignKey("Diseases")]
        public Nullable<int> diseases_id { get; set; }
        public string others { get; set; }

        public virtual User user { get; set; }
        public virtual Diseases Diseases { get; set; }
    }
}
