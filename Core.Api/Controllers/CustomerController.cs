using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Api.Helper;
using Core.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Web.Models;
using System.Drawing;
using GeoCoordinatePortable;
using Microsoft.AspNetCore.Cors;

namespace Core.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/customer")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class CustomerController : BaseController
    {
        AppDbContext db = new AppDbContext();


        [HttpPost]
        [AllowAnonymous]
        [Route("MobileRegisteration")]
        public IActionResult MobileRegisteration([FromBody]Customer regcustomer)  //***
        {
            Random r = new Random();
            string code = code = r.Next(0, 9).ToString() + r.Next(0, 9).ToString() + r.Next(0, 9).ToString() + r.Next(0, 9).ToString();

            var user = db.User.SingleOrDefault(x => x.mobile == regcustomer.Mobile);//.FirstOrDefault();
            if (user != null)
            {
                //user = db.User.Where(x => x.mobile == regcustomer.Mobile && x.isverified == false).FirstOrDefault();
                if (!user.isverified)// (user != null)
                {
                    //new SmsHelper().SendSms(mobile, "Your BackPack Activation Code is: " + code);
                    return Ok(new { status = 0, message = "Mobile already exists but not verified yet", verificationcode = code, verified = false, nextpage = "verification" });
                }

                //user = db.User.Where(x => x.mobile == regcustomer.Mobile && x.isverified == true).FirstOrDefault();
                else//if (user != null)
                {
                    return Ok(new { status = 0, message = "User already exists, please try to login", verified = true, nextpage = "login" });
                }
            }

            try
            {
                //new SmsHelper().SendSms(mobile, "Your BackPack Activation Code is: " + code);

                User customer = new User
                {
                    mobile = regcustomer.Mobile,
                    activationcode = code,
                    isverified = false,
                    isregistered = false,
                    isProvider = false
                    //,user_Type = 0 //Customer
                };

                db.User.Add(customer);
                db.SaveChanges();

                return Ok(new { status = 1, message = "SMS has been sent to this mobile number.", verificationcode = code, verified = false, nextpage = "registeration" });
            }
            catch (Exception)
            {
                return Ok(new { status = -1, message = "Failed to send an SMS, please check this mobile number or try again later." });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("CustomerRegisteration")]
        public IActionResult CustomerRegisteration([FromBody]User customer)  //***
        {
            try
            {
                var regcustomer = db.User.Where(x => x.mobile == customer.mobile).FirstOrDefault();
                if (regcustomer == null)
                    return Ok(new { status = 0, message = "Customer doesn't exist" });


                regcustomer.first_name = customer.first_name;
                regcustomer.last_name = customer.last_name;
                regcustomer.mail = customer.mail;
                regcustomer.isverified = true;
                regcustomer.isregistered = true;
                regcustomer.UserPhoto_Url = string.Concat(regcustomer.id.ToString(), "", ".png");
                /*Upload customer image*/
                if (customer.customerphoto64 != null)
                {
                    string FilePath = string.Concat(GetUserImage.ImagePathForUserPhoto, regcustomer.id.ToString(), "", ".png");

                    using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(customer.customerphoto64)))
                    {
                        using (Bitmap bm2 = new Bitmap(ms))
                        {
                            bm2.Save(FilePath);
                        }
                    }
                }
                /**/

                db.SaveChanges();

                return Ok(new { status = 1, message = "Customer information registered successfully" });
            }
            catch (Exception)
            {
                return Ok(new { status = -1, message = "Failed to register customer information." });
            }
        }

        [HttpPost]
        [Authorize]
        [Route("EditProfile")]
        public IActionResult EditProfile([FromBody]User customer)  //***
        {
            try
            {
                 // var idd = int.Parse(customer.);
                //string id, string user_name
                var idd = GetUserId();
                var imgurlss = new List<string>();
               
                //var testid = 7;
                var regcustomer = db.User.Find(idd);
                if (regcustomer == null)
                {
                    return Ok(new { status = 0, message = "Customer doesn't exist" });
                }
                else if(regcustomer!=null)
                {
                    if (customer.user_name != null)
                    {
                        regcustomer.user_name = customer.user_name;
                    }
                    if (customer.password!=null)
                    {
                        regcustomer.password = customer.password;
                    }
                    if (customer.mail != null)
                    {
                        regcustomer.mail = customer.mail;
                    }
                    if (customer.gender!=null)
                    {
                        regcustomer.gender = customer.gender;
                    }
                    if (customer.first_name!=null)
                    {
                        regcustomer.first_name = customer.first_name;
                    }
                    if (customer.last_name!=null)
                    {
                        regcustomer.last_name = customer.last_name;
                    }
                    if (customer.mobile!=null)
                    {
                        regcustomer.mobile = customer.mobile;
                    }
                    if (customer.preferablePrice!=null)
                    {
                        regcustomer.preferablePrice = customer.preferablePrice;
                    }
                    if (customer.customerphoto64 != null)
                    {
                        regcustomer.UserPhoto_Url = string.Concat(regcustomer.id.ToString(), "", ".png");
                        string FilePath = string.Concat(GetUserImage.ImagePathForUserPhoto, regcustomer.id.ToString(), ".png");
                        ImageSaveHelper.SaveImageToPath(customer.customerphoto64, regcustomer, "UserImages");
                        var imageurl = $"{GetUserImage.OnlineImagePathForUserPhoto}{regcustomer.UserPhoto_Url}";
                        imgurlss.Add(imageurl);
                        db.SaveChanges();
                        //using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(customer.customerphoto64)))
                        //{
                        //    using (Bitmap bm2 = new Bitmap(ms))
                        //    {
                        //        bm2.Save(FilePath);
                        //    }
                        //}
                        if (imgurlss!=null)
                        {
                            return Ok(new { status = 1, message = "Customer information updated successfully", imageurl = imgurlss.FirstOrDefault() });
                        }
                        return Ok(new { status = 1, message = "Customer information updated successfully", imageurl = imgurlss.FirstOrDefault() });
                    }
                    return Ok(new { status = -1, message = "Failed to update customer information." });
                               }
                else
                {
                    return Ok(new { status = -1, message = "Failed to update customer information." });
                }
            }
            catch (Exception)
            {
                return Ok(new { status = -1, message = "Failed to update customer information." });
            }
        }

        [HttpPost]
        [Authorize]
        [Route("AddActivityToFavorite")]
        public IActionResult AddActivityToFavorite([FromBody]User_Favorite_Activities favoritactivity)  //***
        {
            try
            {
                var activity = db.Activity.Find(favoritactivity.activity_id);
                if (activity == null)
                    return Ok(new { status = 0, message = "Activity doesn't exist" });

                var user = db.User.Find(favoritactivity.user_id);
                if (user == null)
                    return Ok(new { status = 0, message = "User doesn't exist" });

                var favactivity = db.User_Favorite_Activities.Where(fa => fa.activity_id == favoritactivity.activity_id & fa.user_id == favoritactivity.user_id).FirstOrDefault();
                if (favactivity != null)
                    return Ok(new { status = 0, message = "Activity already in your favorite list" });

                User_Favorite_Activities favact = new User_Favorite_Activities
                {
                    activity_id = favoritactivity.activity_id,
                    user_id = favoritactivity.user_id,
                    additiondate = DateTime.Now
                };

                db.User_Favorite_Activities.Add(favact);
                db.SaveChanges();

                return Ok(new { status = 1, message = "Activity has been added successfully to your favorite list" });
            }
            catch (Exception)
            {
                return Ok(new { status = -1, message = "Failed to add activity to your favorite list." });
            }
        }

        [HttpPost]
        [Authorize]
        [Route("RemoveActivityFromFavorite")]
        public IActionResult RemoveActivityFromFavorite([FromBody]User_Favorite_Activities favoritactivity)  //***
        {
            try
            {
                var activity = db.Activity.Find(favoritactivity.activity_id);
                if (activity == null)
                    return Ok(new { status = 0, message = "Activity doesn't exist" });

                var user = db.User.Find(favoritactivity.user_id);
                if (user == null)
                    return Ok(new { status = 0, message = "User doesn't exist" });

                var favactivity = db.User_Favorite_Activities.Where(fa => fa.user_id == favoritactivity.user_id & fa.activity_id == favoritactivity.activity_id).FirstOrDefault();
                if (favactivity != null)
                {
                    db.User_Favorite_Activities.Remove(favactivity);
                    db.SaveChanges();
                    return Ok(new { status = 1, message = "Activity has been removed successfully from your favorite list" });
                }
                return Ok(new { status = -1, message = "Failed to remove activity from your favorite list." });
            }
            catch (Exception)
            {
                return Ok(new { status = -1, message = "Failed to remove activity from your favorite list." });
            }
        }

        [Route("ListCustomerFavoriteActivities")]
        [Authorize]
        public IActionResult ListCustomerFavoriteActivities(int customerid)
        {
            var user = db.User.Find(customerid);
            if (user == null)
                return Ok(new { status = 0, message = "User doesn't exist" });
            var activities = db.User_Favorite_Activities.Where(f => f.user_id == customerid);
            if (activities == null)
                return Ok(new { status = 0, message = "Sorry, you don't have favorite activities" });

            var favactivities = db.User_Favorite_Activities.Join(db.Activity.Where(x=> x.status == true & x.isCompleted == true)/*.Include(x => x.ActivityType).Include(x => x.Activity_Photos).Where(x => x.isdeleted == false)*/, fa => fa.activity_id,
                a => a.id, (fa, a) => new { fa, a }).Where(c => c.fa.user_id == customerid)
                .Select(c => new {
                    id = c.a.id,
                    title = c.a.title,
                    activity_Location = c.a.activity_Location,
                    rate = c.a.rate,
                    additiondate = c.fa.additiondate,
                    description = c.a.description,
                    FirstIndividualPrice = c.a.Individual_Categories.Select(s => s.price).FirstOrDefault(),
                    Category = new ActivityTypemodel
                    {
                        Id = c.a.ActivityType.id,
                        Name = c.a.ActivityType.Name,
                        url = Path.Combine(GetUserImage.OnlineImagePathForActivityTypePhoto + c.a.ActivityType.url)
                    },
                    Activity_Photos = c.a.Activity_Photos.Where(s => s.cover_photo == true).Select(s => s.url).FirstOrDefault() == null ? "NA" :
                                               c.a.Activity_Photos.Where(s => s.cover_photo == true).Select(s => GetUserImage.OnlineImagePathForActivity+s.url).First(),
                });
            //.OrderByDescending(c => c.additiondate);//.Take(1);
            if (favactivities != null)
                return Ok(Json(favactivities).Value);
            else
                return NoContent();
        }
     
        [Route("CustomerActivitiesDetails")]
        [HttpGet]
        public IActionResult CustomerActivitiesDetails(int activityId)
        {
            var userid = HttpContext.User.FindFirst("userId") != null ? GetUserId() : -1;

            var activityDetails = db.Activity.Include(x => x.Individual_Categories).Include(x => x.Activity_Photos).Where(x => x.id == activityId)
             .Select(x => new {
                 id = x.id,
                 title = x.title,
                 Category = new ActivityTypemodel
                 {
                     Id = x.ActivityType.id,
                     Name = x.ActivityType.Name,
                     url = Path.Combine(GetUserImage.OnlineImagePathForActivityTypePhoto+x.ActivityType.url)
                 },
                 description = x.description,
                 activity_Location = x.activity_Location,
                 meeting_Location = x.meeting_Location,
                 activity_Lat = x.activity_Lat,
                 activity_Lang = x.activity_Lang,
                 meeting_Lat = x.meeting_Lat,
                 meeting_Lang = x.meeting_Lang,
                 Activity_length = x.Activity_length,
                 //**
                 apply_discount = x.apply_discount,
                 bookingAvailableForGroups = x.bookingAvailableForGroups,
                 bookingAvailableForIndividuals = x.bookingAvailableForIndividuals,
                 isCompleted = x.isCompleted,
                 booking_window = x.booking_window,
                 notice_in_advance = x.notice_in_advance,
                 status = x.status,
                 totalCapacity = x.totalCapacity,
                 stepNumber = x.stepNumber,
                 //**
                 Activity_Photos = x.Activity_Photos.Select(s => new photomodel
                 {
                     id = s.id,
                     url = Path.Combine(GetUserImage.OnlineImagePathForActivity+ s.url),
                     cover_photo = s.cover_photo
                 }).ToList(),
        
                 Activity_Option = x.Activity_Option.Select(s => new OptionModel
                 {
                     id = s.Option.id,
                     name = s.Option.name,
                     fromAge = s.fromAge,
                     toAge = s.toAge,
                     icon = s.Option.icon,
                     description = s.Option.description,
                 }).ToList(),

                 Activity_Add_Ons = x.Activity_Add_Ons.Select(s => new addonsmodel
                 {
                     addons_id = s.Add_Ons.id,
                     activityAddons_id = s.id,
                     name = s.Add_Ons.name,
                     addons_number = s.addons_number,
                     note = s.note,
                     price = s.price,
                     provider_Username = s.provider_Username,
                     icon = s.Add_Ons.icon
                 }).ToList(),

                 Activity_Rules = x.Activity_Rules.Select(s => new RuleModel
                 {
                     Id = s.Rule.id,
                     description = s.Rule.description
                 }).ToList(),

                 requirements = x.requirements,

                 capacityIsUnlimited = x.capacityIsUnlimited,
                 max_capacity_group = x.max_capacity_group,
                 min_capacity_group = x.min_capacity_group,
                 group_price = x.group_price
                 ,
                 Activity_Organizer = x.Activity_Organizer.Select(s => new OrganizerModel
                 {
                     Id = s.id,
                     Name = s.name,
                     Email = s.mail,
                     Mobile = s.mobile,
                     Type_id = s.Organizer_Type.id,
                     OrganizerType = s.Organizer_Type.type,
                     TypeDescription = s.Organizer_Type.description
                 }).ToList(),

                 Individual_Categories = x.Individual_Categories.Select(s => new Individualmodel
                 {
                     Id = s.id,
                     name = s.name,
                     price = s.price,
                     priceafterdiscout = s.price_after_discount,
                     Capacity = s.capacity
                 }).ToList(),

                 reviewsCount = x.Reviews.Where(s => s.activity_id == x.id).Count(),
                 x.rate,
                 FirstIndividualPrice = x.Individual_Categories.Select(s => s.price).FirstOrDefault(),
                 provider = new
                 {
                     x.user_id,
                     x.User.user_name,
                     x.User.first_name,
                     x.User.last_name,
                     x.User.mail,
                     UserPhoto_Url = Path.Combine(GetUserImage.OnlineImagePathForUserPhoto + x.User.UserPhoto_Url)
                 },
                 favActivity =(userid==-1)?false: db.User_Favorite_Activities.Where(s => s.activity_id == x.id & s.user_id == userid).Count() == 0 ? false : true,

                 reviews = x.Reviews.Select(w => new
                 {
                     w.reviewid,
                     w.User.user_name,
                     UserPhoto_Url = Path.Combine(GetUserImage.OnlineImagePathForUserPhoto + w.User.UserPhoto_Url),
                     w.rate,
                     w.review,
                     w.date
                 }).ToList(),

                 Avaliabilities = x.Avaliabilities.Select(s => new
                 {
                     s.id,
                     s.activity_Start,
                     s.activity_End,
                     s.group_Price,
                     s.total_tickets,
                     s.isForGroup,
                     Avaliability_Pricings = s.Avaliability_Pricings.Select(d => new {
                         d.id,
                         d.price,
                         d.priceAfterDiscount,
                         d.individualCategoryId,
                         d.IndividualCategory.name
                     }).ToList()
                 }).ToList()

             });
            return Ok(new { activityDetails });
        }
        [Route("ListHomeActivities")]
        [HttpPost]
        public IActionResult ListHomeActivities([FromBody]SearchActivities search)
        {
            var userid = HttpContext.User.FindFirst("userId") != null ? GetUserId() : -1; 

            var coord = new GeoCoordinate(search.MapLat, search.MapLng);

            if (search.MapLat == 0.0 | search.MapLng == 0.0)
            {
                var activities = db.Activity.Where(x => x.isdeleted == false & x.status == true & x.isCompleted == true)
                          .Select(x => new
                          {
                              x.id,
                              x.title,
                              activityPhoto_Url = x.Activity_Photos.Where(s => s.cover_photo == true).Select(s => s.url).FirstOrDefault() == null ? "NA" :
                                                  x.Activity_Photos.Where(s => s.cover_photo == true).Select(s => s.url).First(),
                              reviewsCount = x.Reviews.Where(s => s.activity_id == x.id).Count(),
                              x.rate,
                              categoryId = x.ActivityType.id,
                              price = x.Individual_Categories.Select(s => s.price).FirstOrDefault(),
                              x.group_price,
                              favActivity = (userid==-1)?false:(db.User_Favorite_Activities.Where(s => s.activity_id == x.id & s.user_id == userid).Count() == 0)? false : true,
                              x.activity_Lang,
                              x.activity_Lat
                          }).ToList().OrderBy(x => x.id).GroupBy(p => p.categoryId)
                            .Select(s => new { activities = s.Take(2), more = s.Skip(2).Count() });
                if (activities != null)
                    return Ok(activities);
                else
                    return NoContent();
            }
            else
            {
                var activities = db.Activity.Where(x=>x.activity_Lat==(decimal)coord.Latitude && x.activity_Lang== (decimal)coord.Longitude)
                          .Select(x => new
                          {
                              x.id,
                              x.title,
                              activityPhoto_Url = x.Activity_Photos.Where(s => s.cover_photo == true).Select(s => s.url).FirstOrDefault() == null ? "NA" :
                                                  x.Activity_Photos.Where(s => s.cover_photo == true).Select(s => s.url).First(),
                              reviewsCount = x.Reviews.Where(s => s.activity_id == x.id).Count(),
                              x.rate,
                              categoryId = x.ActivityType.id,
                              price = x.Individual_Categories.Select(s => s.price).FirstOrDefault(),
                              x.group_price,
                              favActivity = (userid == -1) ? false : (db.User_Favorite_Activities.Where(s => s.activity_id == x.id & s.user_id == userid).Count() == 0) ? false : true,
                              x.activity_Lang,
                              x.activity_Lat
                          }).ToList().OrderBy(x => x.id).GroupBy(p => p.categoryId)
                            .Select(s => new { activities = s, more = s.Count() });
                if (activities != null)
                    return Ok(activities);
                else
                    return NoContent();
            }        

         
        }

        //[HttpGet]
        [HttpPost]
        [Route("ShowMoreActivities")]
        public IActionResult ShowMoreActivities([FromBody]SearchActivities search ,int categoryId)
        {
            var userid = HttpContext.User.FindFirst("userId") != null ? GetUserId() : -1;

            var coord = new GeoCoordinate(search.MapLat, search.MapLng);
            var radiusInMeters = 1 * 1000;


            var activityType = db.ActivityType.Find(categoryId);
            if (activityType == null)
                return BadRequest();


            var activities = db.Activity.Where(x => x.ActivityType.id == categoryId & x.isdeleted == false & x.status == true & x.isCompleted == true)
                     .Select(x => new
                     {
                         x.id,
                         x.title,
                         activityPhoto_Url = x.Activity_Photos.Where(s => s.cover_photo == true).Select(s => s.url).FirstOrDefault() == null ? "NA" :
                                             x.Activity_Photos.Where(s => s.cover_photo == true).Select(s => GetUserImage.OnlineImagePathForActivity + s.url).First(),
                         reviewsCount = x.Reviews.Where(s => s.activity_id == x.id).Count(),
                         x.rate,
                         price = x.Individual_Categories.Select(s => s.price).FirstOrDefault(),
                         x.group_price,
                         favActivity = (userid == -1) ? false : (db.User_Favorite_Activities.Where(s => s.activity_id == x.id & s.user_id == userid).Count() == 0 ? false : true),
                         x.activity_Lang,
                         x.activity_Lat
                     }).OrderBy(x => x.id).Skip(2).ToList();
               
              
        
         if (search.MapLat != 0.0 | search.MapLng != 0.0)
            { return Ok(activities.Where(x => x.activity_Lang != null & x.activity_Lat != null)
                .Where(x => new GeoCoordinate(Convert.ToDouble(x.activity_Lat),
                                                          Convert.ToDouble(x.activity_Lang)).GetDistanceTo(coord) <= radiusInMeters)); }
        
          
            return Ok(activities);

           
              
        }

        [HttpPost]
        [Route("ActivitiesByCategory")]
        [AllowAnonymous]
        public IActionResult ActivitiesByCategory([FromBody]SearchActivities search, int categoryId)
        {
            var userid = HttpContext.User.FindFirst("userId") != null ? GetUserId() : -1;


            var NearestActivities = new List<object> { };
            var coord = new GeoCoordinate(search.MapLat, search.MapLng);
            var radiusInMeters = 1 * 1000;

            var activityType = db.ActivityType.Find(categoryId);
            if (activityType == null)
                return BadRequest();

            var activites = db.Activity.Where(x => x.ActivityType.id == categoryId & x.isdeleted == false & x.status == true & x.isCompleted == true).ToList()
                          .Select(x => new
                          {
                              x.id,
                              x.title,
                              activityPhoto_Url = x.Activity_Photos.Where(s => s.cover_photo == true).Select(s => s.url).FirstOrDefault() == null ? "NA" :
                                                  x.Activity_Photos.Where(s => s.cover_photo == true).Select(s =>GetUserImage.OnlineImagePathForActivity+ s.url).First(),
                              reviewsCount = x.Reviews.Where(s => s.activity_id == x.id).Count(),
                              x.rate,
                              price = x.Individual_Categories.Select(s => s.price).FirstOrDefault(),
                              x.group_price,
                              favActivity =(userid==-1)?false: db.User_Favorite_Activities.Where(s => s.activity_id == x.id & s.user_id == userid).Count() == 0 ? false : true,
                              x.activity_Lang,
                              x.activity_Lat
                          });
            if (search.MapLat != 0.0 | search.MapLng != 0.0)
            {
                return Ok( activites.Where(x => x.activity_Lang != null & x.activity_Lat != null)
                         .Where(x => new GeoCoordinate(Convert.ToDouble(x.activity_Lat),
                                                      Convert.ToDouble(x.activity_Lang)).GetDistanceTo(coord) <= radiusInMeters)); 
            }
            return Ok(activites);
        }
       

        [HttpGet]
        [Authorize]
        [Route("CustomerActivities")]
        public IActionResult CustomerActivities()
        {
            int userid = GetUserId();
        
            var FutureActivities = db.Booking_Ticket.Include(x => x.Booking).Include(x => x.Booking.Activity)
                                  .Where(x => x.userId == userid & x.Booking.Avaliability.activity_Start >= DateTime.Now)
                                  .Select(x => new {
                                      x.Booking.Activity.id,
                                      x.Booking.Activity.title,
                                      x.Booking.Activity.description,
                                      x.Booking.Activity.activity_Lang,
                                      x.Booking.Activity.activity_Lat,
                                      x.Booking.Activity.activity_Location,
                                      x.Booking.Activity.meeting_Lang,
                                      x.Booking.Activity.meeting_Lat,
                                      x.Booking.Activity.meeting_Location,
                                      x.Booking.Avaliability.activity_Start,
                                      activityPhoto_UrL = x.Booking.Activity.Activity_Photos.Where(s => s.cover_photo == true).Select(s =>GetUserImage.OnlineImagePathForActivity+ s.url).FirstOrDefault(),
                                      x.Booking.avaliability_id,
                                      x.ticket_id,
                                      price = x.Booking.Activity.Individual_Categories.Select(s => s.price).FirstOrDefault(),
                                      favActivity = db.User_Favorite_Activities.Where(s => s.activity_id == x.Booking.Activity.id & s.user_id == GetUserId()).Count() == 0 ? false : true,
                                      Provider = new
                                      {
                                          id = x.Booking.Activity.user_id,
                                          user_name = x.Booking.Activity.User.user_name,
                                          first_name = x.Booking.Activity.User.first_name,
                                          last_name = x.Booking.Activity.User.last_name,
                                          mail = x.Booking.Activity.User.mail,
                                          mobile = x.Booking.Activity.User.mobile,
                                          UserPhoto_Url = Path.Combine(GetUserImage.OnlineImagePathForUserPhoto + x.Booking.Activity.User.UserPhoto_Url)
                                      }
                                  });
            var PastActivities = db.Booking_Ticket.Include(x => x.Booking).Include(x => x.Booking.Activity)
                                  .Where(x => x.userId == userid & x.Booking.Avaliability.activity_Start < DateTime.Now)
                                  .Select(x => new {
                                      x.Booking.Activity.id,
                                      x.Booking.Activity.title,
                                      x.Booking.Activity.description,
                                      x.Booking.Activity.activity_Lang,
                                      x.Booking.Activity.activity_Lat,
                                      x.Booking.Activity.activity_Location,
                                      x.Booking.Activity.meeting_Lang,
                                      x.Booking.Activity.meeting_Lat,
                                      x.Booking.Activity.meeting_Location,
                                      x.Booking.Avaliability.activity_Start,
                                      activityPhoto_Url = x.Booking.Activity.Activity_Photos.Where(s => s.cover_photo == true).Select(s =>GetUserImage.OnlineImagePathForActivity+ s.url).FirstOrDefault(),
                                      x.Booking.avaliability_id,
                                      x.ticket_id,
                                      price = x.Booking.Activity.Individual_Categories.Select(s => s.price).FirstOrDefault(),
                                      favActivity = db.User_Favorite_Activities.Where(s => s.activity_id == x.Booking.Activity.id & s.user_id == GetUserId()).Count() == 0 ? false : true,
                                      Provider = new
                                      {
                                          id = x.Booking.Activity.user_id,
                                          user_name = x.Booking.Activity.User.user_name,
                                          first_name = x.Booking.Activity.User.first_name,
                                          last_name = x.Booking.Activity.User.last_name,
                                          mail = x.Booking.Activity.User.mail,
                                          mobile = x.Booking.Activity.User.mobile,
                                          UserPhoto_Url = Path.Combine(GetUserImage.OnlineImagePathForUserPhoto + x.Booking.Activity.User.UserPhoto_Url)
                                      }
                                  });
            return Ok(new { FutureActivities, PastActivities });
        }

        [Route("ListActivities")]
        [HttpPost]
        public IActionResult ListActivities(Filter filteroptins)
        {
            var d = Path.Combine(GetUserImage.OnlineImagePathForActivity + "2");
            var userid = HttpContext.User.FindFirst("userId") != null ? GetUserId() : -1;

            var lstOptions = string.IsNullOrEmpty(filteroptins.options) ? new List<int>() : filteroptins.options.Split(',').Select(s => int.Parse(s)).ToList();
            var aval = db.Avaliability.Where(x => x.activity_Start >= filteroptins.fromdate & x.activity_Start <= filteroptins.todate).Select(x => x.id);
            var categoryprice = db.Individual_Categories.Where(x => x.price >= filteroptins.fromprice & x.price <= filteroptins.toprice).Select(x => x.id);
            var avalPrice = db.Avaliability_Pricings.Where(x => x.price >= filteroptins.fromprice & x.price <= filteroptins.toprice).Select(x => x.avaliabilityId);


            var activities =
                db.Activity.Where(x => x.status == true & x.isCompleted == true).Include(x => x.Activity_Option).
                Include(x => x.ActivityType).Include(x => x.Activity_Photos).Include(x => x.Avaliabilities)
                          .Where(x => x.isdeleted == false)
                             .Select(x => new
                             {
                                 x.id,
                                 x.title,
                                 x.activity_Location,
                                 x.status,
                                 x.stepNumber,
                                 x.isCompleted,
                                 x.rate,
                                 x.bookingAvailableForGroups,
                                 x.bookingAvailableForIndividuals,
                                 favActivity = (userid == -1) ? false : db.User_Favorite_Activities.Where(s => s.activity_id == x.id & s.user_id == userid).Count() == 0 ? false : true,
                                 avaliability = x.Avaliabilities.Where(s => s.activity_id == x.id)
                                              .Select(s => s.id).ToList(),
                                 price = x.Individual_Categories.Where(s => s.activityid == x.id).Select(s => s.id).ToList()

                                 ,
                                 Category = new ActivityTypemodel
                                 {
                                     Id = x.ActivityType.id,
                                     Name = x.ActivityType.Name,
                                     url = Path.Combine(GetUserImage.OnlineImagePathForActivityTypePhoto + x.ActivityType.url)
                                 },
                                 Activity_Photos = x.Activity_Photos.Select(s => new photomodel
                                 {
                                     id = s.id,
                                   //  url =Path.Combine(GetUserImage.OnlineImagePathForActivity + s.url),
                                     url = "http://backpackapis.wasltec.org/myimages/" + s.url,
                                     cover_photo = s.cover_photo
                                 }).ToList()
                                 ,
                                 Provider = new
                                 {
                                     id = x.User.id,
                                     user_name = x.User.user_name,
                                     first_name = x.User.first_name,
                                     last_name = x.User.last_name,
                                     mail = x.User.mail,
                                     mobile = x.User.mobile,
                                     UserPhoto_Url = Path.Combine(GetUserImage.OnlineImagePathForUserPhoto + x.User.UserPhoto_Url)
                                 }

                                 ,
                                 Options = x.Activity_Option.Select(o => o.option_id.Value),
                                 x.activity_Lang,
                                 x.activity_Lat
                               });

                                 var z = activities;

            if (lstOptions.Count() != 0)
            {
                activities = activities.Where(x => x.Options.Intersect(lstOptions).Any());
            }
            if (filteroptins.allowgroup != null)
                activities = activities.Where(a => a.bookingAvailableForGroups == filteroptins.allowgroup);


            if (filteroptins.allowindividual != null)
                activities = activities.Where(a => a.bookingAvailableForIndividuals == filteroptins.allowindividual);


            if (filteroptins.todate != null && filteroptins.fromdate != null)
            {
                activities = activities.Where(x => x.avaliability.Intersect(aval).Any());
                z = activities.Where(x => x.avaliability.Intersect(avalPrice).Any());
            }

            if (filteroptins.fromprice != null && filteroptins.toprice != null)
                activities = activities.Where(x => x.price.Intersect(categoryprice).Any()).Union(z.Intersect(activities));

            if (!string.IsNullOrEmpty(filteroptins.keyword))
                activities = activities.Where(x => x.title.Contains(filteroptins.keyword));

            if (!string.IsNullOrEmpty(filteroptins.categoryId.ToString()))
                activities = activities.Where(x => x.Category.Id == filteroptins.categoryId);

            if (activities != null)
                return Ok(new { activities, NumOfActivities = activities.Count() });

            else
                return NoContent();
        }

        [Authorize]
        [Route("getuseridverification")]
        public IActionResult getuseridverification()  //***
        {
            int userid = GetUserId();
            //int userid = 29;
            var regcustomer = db.User.Find(userid);
            if (regcustomer == null)
                return Ok(new { status = 0, message = "Customer doesn't exist" });

            var useridverification = db.UserIdentifications.Where(x => x.userId == userid).
            Select(x => new
            {
                x.userIdentificationId,
                x.identification_number,
                x.expiry_date,
                x.identification_type,
                x.Nationalities.nationality_id,
                id_copy = Path.Combine(GetUserImage.OnlineImagePathForUserPhoto + x.id_copy)
            }).ToList();

            if (useridverification != null)
                return Ok(useridverification);
            else
                return NoContent();
        }

        [HttpPost]
        [Authorize]
        [Route("VerificationId")]
        public IActionResult VerificationId([FromBody]UserIdentification userIdentification)
        {
            //var user = db.User.Find(userIdentification.userId);
            //if (user == null)
            //    return Ok(new { status = 0, message = "PLZ, check userId" });
            if (userIdentification.identification_type == 0 | userIdentification.identification_number.Count() == 0)
                return Ok(new { status = 0, message = "PLZ, check identification_number or identification_type " });


            var vid = db.UserIdentifications.Where(x => x.userId == GetUserId());
            db.UserIdentifications.RemoveRange(vid);
            db.SaveChanges();


            userIdentification.userId = GetUserId();
            /*Upload customer Id copy*/
            try {
                if (userIdentification.id_copy != null)
                {
                    string FilePath = string.Concat(GetUserImage.ImagePathForUserPhoto, userIdentification.userId.ToString(), "_ID", ".png");

                    using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(userIdentification.id_copy)))
                    {
                        using (Bitmap bm2 = new Bitmap(ms))
                        {
                            bm2.Save(FilePath);
                        }
                    }
                }
            }
            catch (Exception) { }
            /**/
            userIdentification.id_copy = userIdentification.userId.ToString() + "_ID" + ".png";
            db.UserIdentifications.Add(userIdentification);
            db.SaveChanges();

            return Ok(new { UserIdentification_Id = userIdentification.userIdentificationId });

        }
        [HttpPost]
        [Authorize]
        [Route("UserDisease")]

        public IActionResult UserDisease([FromBody]UserDiseasesModel userDiseasesModel)  //*
        {
            var ud = db.User_Diseases.Where(x => x.user_id == GetUserId());
            db.User_Diseases.RemoveRange(ud);

            var fuh = db.FollowUpHealth.Where(f => f.user_id == GetUserId());
            db.FollowUpHealth.RemoveRange(fuh);

            db.SaveChanges();
            /*foreach (User_Diseases item in ud)
            {
                db.User_Diseases.Remove(item);
            }*/


            if (userDiseasesModel.user_Diseases.Count() != 0)
            {
                foreach (var item in userDiseasesModel.user_Diseases)
                {
                    item.user_id = GetUserId();
                    db.User_Diseases.Add(item);
                }
            }
            userDiseasesModel.FollowUpHealth.user_id = GetUserId();
            db.FollowUpHealth.Add(userDiseasesModel.FollowUpHealth);


            db.SaveChanges();
            return Ok();
        }


        [Authorize]
        [Route("GetUserDisease")]
        public IActionResult GetUserDisease()  //***
        {
            UserDiseases_Get userDiseases_Get = new UserDiseases_Get();

            userDiseases_Get.user_DiseasesList = db.User_Diseases.Where(x => x.user_id == GetUserId())
                .Select(x => new Models.UserDiseases
                {
                    diseases_id = x.diseases_id,
                    user_diseases_id = x.user_diseases_id,
                    other = (x.others == null) ? "-" : x.others
                }).ToList();

            userDiseases_Get.FollowUpHealth = db.FollowUpHealth.Where(x => x.user_id == GetUserId()).Select(x => new FollowUpHealthModel
            {
                FollowUpHealthId = x.FollowUpHealthId,
                dietaryRestrictions = x.dietaryRestrictions,
                dietaryRestrictionsDetails = x.dietaryRestrictionsDetails == null ? "-" : x.dietaryRestrictionsDetails,
                hospitalization = x.hospitalization,
                hospitalizationDetails = x.hospitalizationDetails == null ? "-" : x.hospitalizationDetails,
                medication = x.medication,
                medicationDetails = x.medicationDetails == null ? "-" : x.hospitalizationDetails
            }).FirstOrDefault();

            return Ok(userDiseases_Get);
        }
        [Authorize]
        [Route("Chk_verificationId")]
        public IActionResult Chk_verificationId()
        {
            var user = db.User.Find(GetUserId());
            if (user == null)
                return Ok(new { status = 0, message = "PLZ, check userId" });

            var verificationId = db.UserIdentifications.Where(x => x.userId == GetUserId()).Select(x => new
            {
                x.userIdentificationId,
                x.identification_number,
                x.gender,
                x.DOB,
                //x.id_copy,
                id_copy = Path.Combine(GetUserImage.OnlineImagePathForUserPhoto + x.id_copy),
                x.expiry_date,
                Identification_type = new
                {
                    x.Identification_types.identification_type_id,
                    x.Identification_types.identification_type_ar,
                    x.Identification_types.identification_type_en
                },
                Nationality = new
                {
                    x.Nationalities.nationality_id,
                    x.Nationalities.nationality_name_ar,
                    x.Nationalities.nationality_name_en
                }
            }).ToList();
            if (verificationId == null)
                return Ok(new { status = 0, message = "insert verification id " });

            return Ok(new { status = 1, verificationId });
        }

        [Authorize]
        [Route("Chk_UserDeclaration")]
        public IActionResult Chk_UserDeclaration()
        {
            var user = db.User.Find(GetUserId());
            if (user == null)
                return Ok(new { status = 0, message = "PLZ, check userId" });
            var declaration = db.User_Diseases.Where(x => x.user_id == GetUserId()).ToList();

            if (declaration == null)
                return Ok(new { status = 0, message = "Plz, Insert declaration Info" });

            return Ok(new { status = 1 });
        }


        //[HttpPost]
        //
        //[Route("send_message")]
        //public IActionResult send_message([FromBody]MessageReply messageReply, int ticketId)  //*
        //{

        //    if (ticketId != 0)
        //    {
        //        //Activity Creator
        //        int userid = (int)db.Booking_Ticket.Include(x => x.Booking).Where(x => x.ticket_id == ticketId)
        //                     .Select(x => x.Booking.Activity.user_id).First();

        //        //Customer Id
        //        int customerId= (int)db.Booking_Ticket.Where(x => x.ticket_id == ticketId)
        //                     .Select(x => x.userId).First();

        //        var chatMessages = db.MessageReplies.Include(x => x.Chat).Where(x => x.regardingId == ticketId & (x.Chat.firstUser == userid | x.Chat.secondUser == userid))
        //                      .Select(x => x.Chat).FirstOrDefault();
        //        if (chatMessages == null)
        //        {
        //            Chat chat_ = new Chat();
        //            chat_.firstUser = customerId;
        //            chat_.secondUser = userid;
        //            chat_.lastUpdateDate = DateTime.Now;
        //            db.Chat.Add(chat_);
        //            db.SaveChanges();

        //            MessageReply messageReply_ = new MessageReply()
        //            {
        //                date = DateTime.Now,
        //                userId = GetUserId(),
        //                chatId = chat_.chatId,
        //                message = messageReply.message,
        //                regardingId = ticketId,
        //                typeId = 2
        //            };

        //            db.MessageReplies.Add(messageReply_);

        //            chat_.lastUpdateDate = messageReply_.date;
        //            db.SaveChanges();
        //        }
        //        else
        //        {
        //            MessageReply messageReply_ = new MessageReply()
        //            {
        //                date = DateTime.Now,
        //                userId = GetUserId(),
        //                chatId = chatMessages.chatId,
        //                message = messageReply.message,
        //                regardingId = ticketId,
        //                typeId = 2
        //            };

        //            db.MessageReplies.Add(messageReply_);

        //            chatMessages.lastUpdateDate = messageReply_.date;
        //            db.SaveChanges();

        //        }
        //        return Ok(new { status = 1 });
        //    }
        //    return Ok(new { status = 0 });

        //}

        public class Customer
        {
            public string Mobile { get; set; }
            public int customerid { get; set; }
        }
        public class Filter
        {
            public string options;
            public bool? allowgroup;
            public Nullable<bool> allowindividual;
            public decimal? fromprice;
            public decimal? toprice;
            public DateTime? fromdate;
            public DateTime? todate;
            public int categoryId;
            public string keyword;
        }
    }
}
