using System;
using System.Collections.Generic;

namespace Core.Web.Models
{
    public partial class UserIdentifications
    {
        public int UserIdentificationId { get; set; }
        public int? UserId { get; set; }
        public int? Nationality { get; set; }
        public int? IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string IdCopy { get; set; }
        public DateTime? Dob { get; set; }
        public string Gender { get; set; }

        public IdentificationTypes IdentificationTypeNavigation { get; set; }
        public Nationalities NationalityNavigation { get; set; }
        public User User { get; set; }
    }
}
