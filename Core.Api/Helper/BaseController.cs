using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Helper
{
    [ApiController]
    public abstract class BaseController : Controller
    {
        public int GetUserId()
        {
            int userId= int.Parse(this.User.Claims.First(i => i.Type == "UserId").Value);
            return userId;
        }
    }
}
