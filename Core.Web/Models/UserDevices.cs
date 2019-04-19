using System;
using System.Collections.Generic;

namespace Core.Web.Models
{
    public partial class UserDevices
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool IsPushEnabled { get; set; }
        public string PushKey { get; set; }

        public User User { get; set; }
    }
}
