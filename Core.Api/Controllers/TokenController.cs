using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Core.Api.Helper;
using Core.Web.Data;
using Core.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Core.Api.Controllers
{

    [Produces("application/json")]
    [Route("api/Token")]
    public class TokenController : ControllerBase
    {
        private IConfiguration _config;

        public TokenController(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateToken([FromBody]LoginModel login)
        {
            IActionResult response = Unauthorized();
            var user = Authenticate(login);

            if (user != null)
            {
                if (user.Id != -1)
                {
                    var tokenString = BuildToken(user);
                    Random r = new Random();
                    string code = code = r.Next(0, 9).ToString() + r.Next(0, 9).ToString() + r.Next(0, 9).ToString() + r.Next(0, 9).ToString();
                    return Ok(new { token = tokenString, user, verificationcode = code });
                }
                else
                {
                    return NotFound(new { code = "-1", message = "email or Password Not Valid" });
                }
            }

            return response;
        }


        private string BuildToken(UserModel user)
        {
            var claims = new[] {
               new Claim("UserId", user.Id.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel Authenticate(LoginModel login)
        {
            AppDbContext db = new AppDbContext();
            var user = login.LoginType == LoginType.customer ?
                                          db.User.Where(x => x.mobile == login.Mobile & x.isregistered == true).FirstOrDefault() :
                                          db.User.Where(x => x.mail == login.Email & x.password == login.Password).FirstOrDefault();

            if (user != null)
            {
                return new UserModel {
                    Id = user.id,
                    UserName = user.user_name,
                    first_name = user.first_name,
                    last_name = user.last_name,
                    mail = user.mail,
                    isProvider = user.isProvider,
                    mobile = user.mobile,
                    preferablePrice = user.preferablePrice,
                    UserPhoto_Url = Path.Combine(GetUserImage.OnlineImagePathForUserPhoto + user.UserPhoto_Url)
                };
            }

            else
            {
                return new UserModel { Id = -1, UserName = "User not found" };
            }
        }

        public class LoginModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string Mobile { get; set; }
            public LoginType LoginType { get; set; }
        }

        public enum LoginType { user = 1, customer = 0 }
        
        private class UserModel
        {
            public int Id { get; set; }
            public string UserName { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string mobile { get; set; }
            public string mail { get; set; }
            public bool isProvider { get; set; }
            public string UserPhoto_Url { get; set; }
            public int preferablePrice { get; set; }

        }

    }
}
