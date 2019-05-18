using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Api.Models;
using Core.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using Core.Api.Helper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProRegistrationController : Controller, ISendVerificationCode
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
        [HttpPost]
        [Route("SendVerification")]
        public IActionResult SendVerification()
        {
            var smsproperties = new SmsProperties
            {
                url = "http://www.jawalbsms.ws/api.php/sendsms",
                //user="",
                Authentication= new MessageAuthentication
                {
                    user = "backpack",
                    pass= "4574529"
                },
                Parameter = new MessaageParameter
                {
                    message="Testing the Backpack Otp sending",
                    sender= "backpackapp",
                    to="97334037232"
                }

            };
           var testd = SendMessage(smsproperties);
            return Ok(testd);
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
        internal class DataObject
        {
            public string Name { get; set; }
        }
        public string SendMessage(SmsProperties smsProperties)
        {
            HttpClient client = new HttpClient();
            var url = smsProperties.url;
            client.BaseAddress = new Uri(url);
            var urlparams = $"?user={smsProperties.Authentication.user}&pass={smsProperties.Authentication.pass}&to{smsProperties.Parameter.to}&message={smsProperties.Parameter.message}&sender={smsProperties.Parameter.sender}";
            //this makes the request to accept json type by setting the Http header
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(urlparams).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var bodyms = response.StatusCode;
                if (bodyms != null)
                {
                    return  $"sent!!! with status {bodyms} Abiola testing send verification {response.ReasonPhrase}";
                }
                //var dataObjects = response.Content.ReadAsAsync<IEnumerable<DataObject>>().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll
                //foreach (var d in dataObjects)
                //{
                //    var bodyms = d.Name;
                //    Console.WriteLine("{0}", bodyms);
                //}
            }
            return $"{(int)response.StatusCode}, {response.ReasonPhrase}";

            //Make any other calls using HttpClient here.

            //Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
            //  client.Dispose();
            // throw new NotImplementedException();
        }
    }

}
