using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Api.Models;
using Core.Web.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProRegistrationController : Controller
    {
        AppDbContext db = new AppDbContext();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpPost]
        [Route("RegisterPro")]
        public IActionResult RegisterPro([FromBody] ProRegister proRegister)
        {
            if (proRegister != null)
            {
                var regtype = proRegister.RegType.ToLower();
                if (regtype != "organization" || regtype != "individual" || regtype != "customer")
                {
                    var user = new User();
                    if (regtype== "customer")
                    {
                        user.user_Type = 0;
                        user.email = proRegister.Email.ToLower();
                        user.password = proRegister.Password;
                        user.mail = proRegister.Email.ToLower();
                        user.registrationType = proRegister.RegType.ToLower();
                        
                    }
                    if (regtype == "individual")
                    {
                        user.user_Type = 1;
                        user.email = proRegister.Email.ToLower();
                        user.password = proRegister.Password;
                        user.mail = proRegister.Email.ToLower();
                        user.registrationType = proRegister.RegType.ToLower();
                    }
                    if(regtype == "organization")
                    {
                        user.user_Type = 1;
                        user.email = proRegister.Email.ToLower();
                        user.password = proRegister.Password;
                        user.mail = proRegister.Email.ToLower();
                        user.registrationType = proRegister.RegType.ToLower();
                    }                
                    if (!db.User.Any(c => c.email == user.email))
                    {
                        db.User.Add(user);
                        db.SaveChanges();
                        var userid = db.User.FirstOrDefault(c => c.email==user.email);
                        return Ok(new { message = "user saved successfully", userId = userid });
                    }
                    else
                    {
                        return Ok(new { message = "user exist already" });
                    }

                }
                return Ok(new { message = "user type is invalid, can only be organization, individual or customer" });
            }
            return Ok(new { message = "fields are invalid" });
        }
        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
