using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Core.Web.Models;
using Core.Api.Helper;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : BaseController
    {
        AppDbContext db = new AppDbContext();




        [HttpPost, Route("AddDevice")]
        [Authorize]
        public IActionResult AddDevice([FromBody] UserDevice userDevice)
        {
            var userId= GetUserId();
            var user = db.User.Find(userId);

                if (user != null)
                {
                    UserDevice device = (from a in this.db.UserDevices
                                  where a.PushKey == userDevice.PushKey
                                  where a.userId == GetUserId()
                                  select a).FirstOrDefault<UserDevice>();

                    if (device == null)
                    {
                    userDevice.userId = GetUserId();
                    userDevice.IsPushEnabled = true;
                    // device.PushKey = (userDevice.IsPushEnabled ? userDevice.PushKey : string.Empty); //Cookin_Pro
                    db.UserDevices.Add(userDevice);
                    db.SaveChanges();
                    return Ok(new { status=1,message= "Device Added Successfully" });
                    }
                return Ok(new { status = 1,  message = "Device already Exist" });
                }

            return Ok(new { status = 0, message = "User not Exists" });

        }


        //[Authorize]
        //[HttpPost, Route("SendAndroidNotification")]
        //public IActionResult SendIOSNotification(string pushKey, string message, object data)
        //{
        //    try
        //    {
        //        var applicationID = "";
        //        WebRequest tRequest;
        //        var SENDER_ID = "";
        //        tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
        //        tRequest.Method = "post";
        //        tRequest.ContentType = "application/json";
        //        tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
        //        tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));
        //        var PushObject = new 
        //        {
        //            to = pushKey,
        //            notification = new 
        //            {
        //                title = message,
        //                body = data
        //            }
        //        };

        //        Byte[] byteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(PushObject));
        //        tRequest.ContentLength = byteArray.Length;
        //        Stream dataStream = tRequest.GetRequestStream();
        //        dataStream.Write(byteArray, 0, byteArray.Length);
        //        dataStream.Close();
        //        WebResponse tResponse = tRequest.GetResponse();
        //        dataStream = tResponse.GetResponseStream();
        //        StreamReader tReader = new StreamReader(dataStream);
        //        String sResponseFromServer = tReader.ReadToEnd();
        //        tReader.Close();
        //        dataStream.Close();
        //        tResponse.Close();

        //        return Ok(new { status = 1 });

        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new { status = ex });
        //    }
        //}

        [Authorize]
        [HttpPost, Route("SendAndroidNotification")]
        public IActionResult SendAndroidNotification([FromBody]PushObject pushObject)
        {

            try
            {
                var applicationID = "AAAAoXaP0aU:APA91bHt8F-i3HS-66FBmZEz36cacY9WMLsNZi6zhteYGxmdHQGS9-zWF_VoIlxt2tKwn8Jn3AfWcJBMTjzDGt20JcKdb85NxY-JAIlupW2w-bbCXlHIY6PwIeY-GjSe5VwXxQNIyEyi";
                WebRequest tRequest;
                var SENDER_ID = "693478871461";
                tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));

                var data = new 
                {
                    to =pushObject.pushKey,
                    data = new 
                    {
                        title = pushObject.title,
                        body = pushObject.body
                    }
                };

                Byte[] byteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
                tRequest.ContentLength = byteArray.Length;
                Stream dataStream = tRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse tResponse = tRequest.GetResponse();
                dataStream = tResponse.GetResponseStream();
                StreamReader tReader = new StreamReader(dataStream);
                String sResponseFromServer = tReader.ReadToEnd();
                tReader.Close();
                dataStream.Close();
                tResponse.Close();
                return Ok(new { status = 1 });
            }
            catch (Exception ex)
            {
                return Ok(new { status = ex});
            }

        }
    }
    public class PushObject
    {
        public string title { get; set; }
        public string body { get; set; }
        public string pushKey  { get; set; }
     
    }
   
}
