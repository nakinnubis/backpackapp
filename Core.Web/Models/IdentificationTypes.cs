using System;
using System.Collections.Generic;

namespace Core.Web.Models
{
    public partial class IdentificationTypes
    {
        public IdentificationTypes()
        {
            UserIdentifications = new HashSet<UserIdentifications>();
        }

        public int IdentificationTypeId { get; set; }
        public string IdentificationTypeAr { get; set; }
        public string IdentificationTypeEn { get; set; }

        public ICollection<UserIdentifications> UserIdentifications { get; set; }
    }
}
