﻿using System;
using System.Collections.Generic;

namespace Core.Web.Models
{
    public partial class UserRoles
    {
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int ActivityId { get; set; }

        public Roles Role { get; set; }
        public User User { get; set; }
    }
}
