using System;
using System.Collections.Generic;

namespace Core.Web.Models
{
    public partial class UserFavoriteActivities
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? ActivityId { get; set; }
        public DateTime? Additiondate { get; set; }
    }
}
