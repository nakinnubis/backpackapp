using Core.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class UserIdentification
    {
        [Key]
        public int userIdentificationId { get; set; }
        [ForeignKey("User")]
        public Nullable<int> userId { get; set; }
        [ForeignKey("Nationalities")]
        public Nullable<int> nationality { get; set; }
        [ForeignKey("Identification_types")]
        public Nullable<int> identification_type { get; set; }
        public string identification_number { get; set; }
        public DateTime expiry_date { get; set; }
        public string id_copy { get; set; }

        public virtual User User { get; set; }
        public virtual Nationalities Nationalities { get; set; }
        public virtual Identification_types Identification_types { get; set; }

        public DateTime DOB { get; set; }
        public string gender { get; set; }

    }
}
