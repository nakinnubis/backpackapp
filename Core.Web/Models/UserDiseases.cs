using System;
using System.Collections.Generic;

namespace Core.Web.Models
{
    public partial class UserDiseases
    {
        public int UserDiseasesId { get; set; }
        public int UserId { get; set; }
        public int? DiseasesId { get; set; }
        public string Others { get; set; }

        public Diseases Diseases { get; set; }
        public User User { get; set; }
    }
}
