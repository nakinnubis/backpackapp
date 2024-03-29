﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Core.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Core.Web.Models;
using Core.Api.Models;
using Core.Api.Helper;
using Microsoft.AspNetCore.Cors;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Core.Api.Controllers
{
    [Route("api/Activity")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ActivityController : BaseController// ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        AppDbContext db = new AppDbContext();

        public ActivityController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

      
        //To get all activity types with id , name and online Url
        [HttpGet]
        [Route("GetActivityTypes")]
        public IActionResult GetActivityTypes()
        {
            var activityType = db.ActivityType.Select(x => new { x.id,
                                                                 x.Name,
                                                                 url = GetUserImage.OnlineImagePathForActivityTypePhoto + x.url }).ToList();
            if (activityType != null)
                return Ok(activityType);
            else
                return NoContent();
        }

        /// <summary>
        /// To get all option (id,name,online url of icon and age if option=Child)
        /// such as (Family ,Male ,Female)
        /// fromAge and ToAge  save in activityOption
        /// </summary>
        /// <returns></returns>
      
        [HttpGet]
        [Route("GetOptions")]
        public IActionResult GetOptions()  //***
        {
            var Options = db.Option.Select(x => new { x.id,
                                                      x.name,
                                                      x.icon,
                                                      x.haveAge }).ToList();
            if (Options != null)
                return Ok(Options);
            else
                return NoContent();
        }

        
        /// <summary>
        /// To get all Upcoming Activity related to current user which not deleted and completed .
        /// </summary>
        /// <returns></returns>
        
        [Route("UpcomingActivity")]
        [Authorize]
        public IActionResult UpcomingActivity()
        {
            int userId = GetUserId();
            var UpcomingActivity = db.Avaliability.Include(x => x.Activity)
                                  .Where(x => x.activity_Start >= DateTime.Now & x.Activity.isdeleted == false 
                                              & x.Activity.isCompleted == true & x.Activity.user_id==userId)
                                  .Select(x => new
                                  {
                                      x.activity_id,
                                      x.Activity.title,
                                      x.activity_Start,
                                      x.Activity.isCompleted,
                                      x.isForGroup,
                                      x.Activity.status,
                                      x.total_tickets,
                                      x.id
                                  })
                                   ;
            return Ok(UpcomingActivity);

        }

        /// <summary>
        /// get offline&online&incomplete activity related to current user which not deleted .
        /// </summary>
        /// <returns> 3 Array of offline&online&incomplete activity  </returns>
        [Route("GetAllActivities")]
        [Authorize]
        public IActionResult GetAllActivities()  //offline&online&incomplete 
        {
            
            var activities =
              db.Activity.Include(x => x.ActivityType).Include(x => x.Activity_Photos)
                     .Where(x => x.isdeleted == false && x.user_id == GetUserId()).ToList()
                           .Select(x => new
                           {
                               x.id,
                               x.title,
                               x.activity_Location,
                               x.status,
                               x.stepNumber,
                               x.isCompleted,
                               x.rate,
                               Category = new ActivityTypemodel
                               {
                                   Id = x.ActivityType.id,
                                   Name = x.ActivityType.Name,
                                   url = GetUserImage.OnlineImagePathForActivityTypePhoto + x.ActivityType.url
                               },
                               Activity_Photos = x.Activity_Photos.Select(s => new photomodel
                               {
                                   id = s.id,
                                   url = GetUserImage.OnlineImagePathForActivity + s.url,
                                   cover_photo = s.cover_photo
                               }).ToList(),
                           }).ToList();
            //i remove & 
            var OfflineActivities =activities.Where(x=>x.status==false || x.isCompleted == true).ToList();
            var OnlinneActivites= activities.Where(x => x.status || x.isCompleted == false).ToList();
            var IncompleteActivites= activities.Where(x =>  x.isCompleted == false).ToList();
            if (activities != null)
            return Ok( new { OnlinneActivites,OfflineActivities, IncompleteActivites , all=activities } );
            else
                return NoContent();
        }

        //[Authorize]
        [HttpGet]
       [ Route("GetActivityByDate")]
        public IActionResult GetActivityByDate(DateTime date)
        {
            var activity = db.Avaliability.Include(x=>x.Avaliability_Pricings).Where(f => f.activity_Start.Value.Date == date.Date).Select(x => new
            {
                avaliabilityid = x.id,
                x.Activity.title,
                x.Activity.activity_Location,
                x.Activity.status,
                x.Activity.stepNumber,
                x.Activity.isCompleted,
                x.Activity.rate,
                x.Activity.totalCapacity,
                x.activity_Start,
               x.activity_End,
                Availability_group_Price = x.group_Price,
               x.isForGroup,
             x.total_tickets,
                avaliabilityPricing = x.Avaliability_Pricings.Select(f => new AvaliabilityPricing
                {
                    id = f.id,
                    individualCategoryId = f.individualCategoryId,
                    price = f.price,
                    priceAfterDiscount = f.priceAfterDiscount
                }).AsEnumerable(),
                Category = new ActivityTypemodel
                {
                    Id = x.Activity.ActivityType.id,
                    Name = x.Activity.ActivityType.Name,
                    url = GetUserImage.OnlineImagePathForActivityTypePhoto + x.Activity.ActivityType.url
                },
                //x.Activity.Avaliabilities,
               BookingInfo=x.Activity.Bookings.Where(d=>d.bookingDate!=null).AsEnumerable(),
                x.Activity.bookingAvailableForGroups,
                x.Activity.bookingAvailableForIndividuals,
                Activity_Photos = x.Activity.Activity_Photos.Select(s => new photomodel
                {
                    id = s.id,
                    url = GetUserImage.OnlineImagePathForActivity + s.url,
                    cover_photo = s.cover_photo
                }).ToList(),
            }).ToList();          
            if (activity != null){
                return Ok(activity);
            }
            return BadRequest( new { message="Request returned empty data"});
        }
        /// <summary>
        ///  get all reviews which not blocked  of certain activity 
        /// </summary>
        /// <param name="activityid"></param>
        /// <returns> online activity url  </returns>
        [Route("GetActivityReview")]
        
        public IActionResult GetActivityReview(int activityid)
        {
            var activities = db.Reviews.Include(x => x.Activity)
                              .Where(x => x.activity_id == activityid & x.isBlocked == false)
                             .Select(x => new
                             {
                                 x.reviewid,
                                 x.Activity.title,
                                 x.review,
                                 x.rate,
                                 x.activity_id,
                                 x.date,
                                 x.User.user_name,
                                 UserPhoto_Url = Path.Combine(GetUserImage.OnlineImagePathForUserPhoto + x.User.UserPhoto_Url)
                             })
                             .ToList();
            if (activities != null)
                return Ok(activities);
            else
                return NoContent();
        }

        /// <summary>
        /// get activity data from (Activity,ActivityType,Activity_Photos,Activity_Add_Ons, Activity_Rules,
        ///                         Avaliabilities,Individual_Categories,Activity_Organizer,Activity_Option)
        /// </summary>
        /// <param name="activityid"></param>
        /// <returns></returns>
        [Route("GetActivityDetails")]
        [Authorize]
        public IActionResult GetActivityDetails(int activityid)
        {
           // JavaScriptSerializer jss = new JavaScriptSerializer();
            var details = (from a in db.Activity.Include(x => x.Activity_Add_Ons)
                                            .Include(x => x.Activity_Photos).Include(x => x.ActivityType)
                                            .Include(x => x.Activity_Option).Include(x => x.Activity_Organizer)
                                            .Include(x => x.Avaliabilities)
                                            .Include(x => x.Individual_Categories)
                           where a.id == activityid & a.isdeleted == false
                           select new 
                           {
                               id = a.id,
                               title = a.title,
                               description = a.description,
                               bookingAvailableForGroups = a.bookingAvailableForGroups,
                               bookingAvailableForIndividuals = a.bookingAvailableForIndividuals,
                               activity_Lang = a.activity_Lang,
                               activity_Lat = a.activity_Lat,
                               meeting_Lang = a.meeting_Lang,
                               meeting_Lat = a.meeting_Lat,
                               isCompleted = a.isCompleted,
                               stepNumber = a.stepNumber,
                               has_individual_categories=a.has_individual_categories,
                               has_specific_capacity=a.has_specific_capacity,
                               capacityIsUnlimited = a.capacityIsUnlimited,
                              // time_option=db.Booking.Where(c=>c.activity_id==activityid &&a.).Select(c=>c.time_option).AsEnumerable(),
                               Category = new ActivityTypemodel
                               {
                                   Id = a.ActivityType.id,
                                   Name = a.ActivityType.Name,
                                   url = GetUserImage.OnlineImagePathForActivityTypePhoto + a.ActivityType.url
                               },
                               Activity_Photos = a.Activity_Photos.Select(s => new photomodel
                               {
                                   id = s.id,
                                   url = GetUserImage.OnlineImagePathForActivity + s.url,
                                   cover_photo = s.cover_photo
                               }).ToList(),
                               Activity_Add_Ons = a.Activity_Add_Ons.Select(s => new addonsmodel
                               {
                                   addons_id = s.Add_Ons.id,  //Eman
                                   activityAddons_id = s.id  //Eman
                                                                   ,
                                   name = s.Add_Ons.name
                                                                   ,
                                   addons_number = s.addons_number
                                                                   ,
                                   note = s.note,
                                   price = s.price
                                                                   ,
                                   provider_Username = s.provider_Username,
                                   icon = s.Add_Ons.icon
                               }).ToList(),
                               activity_Location = a.activity_Location,
                               meeting_Location = a.meeting_Location,
                               Activity_Rules = a.Activity_Rules.Select(s => new RuleModel
                               {
                                   Id = s.Rule.id
                                                                                        ,
                                   description = s.Rule.description
                               }).ToList(),
                               requirements = a.requirements,
                               status = a.status,
                               notice_in_advance = a.notice_in_advance,
                               booking_window = a.booking_window,
                               Avaliabilities = db.Avaliability.Where(f=>f.activity_id==activityid).Select(s => new Avaliabilitymodel
                               {
                                   Id = s.id,
                                   activitystart = s.activity_Start,
                                   actvityEnd = s.activity_End,
                                   isForGroup = s.isForGroup,
                                   startdate=s.startdate,
                                   enddate=s.enddate,
                                   starthour=s.starthour,
                                   endhour=s.endhour
                                   //isprovider = s.isForGroup
                               }).ToList(),
                               Activity_length = a.Activity_length,
                               totalCapacity = a.totalCapacity,
                               min_capacity_group = a.min_capacity_group,
                               max_capacity_group = a.max_capacity_group,
                               group_price = a.group_price,
                               apply_discount = a.apply_discount,
                               Individual_Categories = DeZerial(a.individual_categories),
                               Activity_Organizer = a.Activity_Organizer.Select(s => new OrganizerModel
                               {
                                   Id = s.id,
                                   Name = s.name,
                                   Email = s.mail,
                                   Mobile = s.mobile,
                                   Type_id = s.Organizer_Type.id,
                                   OrganizerType = s.Organizer_Type.type,
                                   TypeDescription = s.Organizer_Type.description
                               }).ToList(),
                               Activity_Option = DeZerial(a.activity_option)
                           }).ToList();


            if (details != null)
                return Ok(details);
            else
                return NoContent();
        }
        public dynamic DeZerial(string json)
        {
            if(json != null)
            {
               return JsonConvert.DeserializeObject(json);
            }
            return null;
        }
        //To change activity status from online to offline and vice versa
        [Authorize]
        [Route("ChangeStatus")]
        public IActionResult ChangeStatus(int activityId)
        {
            var activity = db.Activity.Find(activityId);
            if (activity != null)
            {
                activity.status = !activity.status;
                db.SaveChanges();
                return Ok(new { activityid = +activity.id });
            }

            return BadRequest(new { message = "Please,check data" });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookingSettingModel"></param>
        /// <param name="activityId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("CalenarAvailablility")]
        public IActionResult CalenarAvailablility([FromBody]BookingSettingModel bookingSettingModel, int activityId)
        {
            Avaliability availbility;
            if (bookingSettingModel.avalibilityModel.id == 0)
            {
                availbility = new Avaliability
                {
                    activity_End = bookingSettingModel.avalibilityModel.activity_End,
                    activity_Start = bookingSettingModel.avalibilityModel.activity_Start,
                    group_Price = bookingSettingModel.avalibilityModel.group_Price,
                    isForGroup = bookingSettingModel.avalibilityModel.isForGroup,
                    activity_id = activityId,
                    total_tickets = 0,
                };
                db.Avaliability.Add(availbility);
                db.SaveChanges();
            }
            else    //edit
            {
                availbility = db.Avaliability.Find(bookingSettingModel.avalibilityModel.id);
                if (availbility == null)
                    return BadRequest(new { message = "Please, Check activity id" });

                var reservation = db.Booking.Where(x => x.avaliability_id == availbility.id).Select(x=>x.id).ToList();
                //if (reservation.Count != 0)
                //    return Ok("Can't edit this availability");
                //Edit
                availbility.activity_Start = bookingSettingModel.avalibilityModel.activity_Start;
                availbility.activity_End = bookingSettingModel.avalibilityModel.activity_End;
                availbility.group_Price = bookingSettingModel.avalibilityModel.group_Price;
                //  availbility.Activity.apply_discount = bookingSettingModel.apply_discount;
                db.SaveChanges();
              
            }
            var activity = db.Activity.Find(activityId);
            activity.apply_discount = bookingSettingModel.apply_discount;

            foreach (var price in bookingSettingModel.avalibilityPricingModels)
            {
                if (price.id == 0)
                {
                    price.avaliabilityId = availbility.id;
                    db.Avaliability_Pricings.Add(price);
                 
                }
                else
                {
                    var avaliability_Pricing = db.Avaliability_Pricings.Find(price.id);
                    if (avaliability_Pricing != null)
                    {
                        avaliability_Pricing.price = price.price;
                        avaliability_Pricing.priceAfterDiscount = price.priceAfterDiscount;
                        avaliability_Pricing.individualCategoryId = price.individualCategoryId;
                    }
                }
                db.SaveChanges();
            }
         
            return Ok(new { avaliabilityId = availbility.id });
        }

        /// <summary>
        /// delete availablility and check if there booking of this avalibility 
        /// </summary>
        /// <param name="availablilityId"></param>
        /// <returns></returns>
        [Authorize]
        [Route("DeleteAvailablility")]
        public IActionResult DeleteAvailablility(int availablilityId)
        {
            var Availablility = db.Avaliability.Find(availablilityId);
            if (Availablility == null)
                return NotFound();
            var booking = db.Booking.Where(x => x.avaliability_id == availablilityId).ToList();
            if (booking.Count() != 0)
                return Ok(new { status = 0, message = "Can't delete this avaliability" });

            db.Avaliability.Remove(Availablility);
            db.SaveChanges();
            return Ok(new { status = 1, message = "deleted successfully" });

        }
        /// <summary>
        /// Edits Availability endpoints
        /// </summary>
        /// <param name="availablilityId"></param>
        /// <returns></returns>
        [Authorize]
        [Route("EditAvailablility")]
        public IActionResult EditAvailablility(int availablilityId)
        {
            var Availablility = db.Avaliability.Find(availablilityId);
            if (Availablility == null)
                return NotFound();
            var booking = db.Booking.Where(x => x.avaliability_id == availablilityId).ToList();
            if (booking.Count() != 0)
                return Ok(new { status = 0, message = "Can't edit this avaliability" });

            db.Avaliability.Remove(Availablility);
            db.SaveChanges();
            return Ok(new { status = 1, message = "deleted successfully" });

        }



        // create Individual Category of activity
        //[HttpPost]
        //
        //[Route("createIndividualCategory")]
        //public IActionResult createIndividualCategory([FromBody] IndividualCategory individualCategory)
        //{
        //    if (individualCategory != null)
        //    {
        //        if (individualCategory.activityid != 0)
        //        {
        //            db.Individual_Categories.Add(individualCategory);
        //            db.SaveChanges();
        //            return Ok(new { avaliability_id = individualCategory.id });
        //        }
        //        return BadRequest();
        //    }
        //    return BadRequest(new { message = "Please,check data" });
        //}


        // Get all Individual Category ofactivity 
        [Authorize]
        [Route("GetIndividualCategory")]
        public IActionResult GetIndividualCategory(int activityid)
        {
            if (activityid != 0)
            {
                var IndividualCategory = db.Individual_Categories.Where(x => x.activityid == activityid)
                                          .Select(x => new
                                          {
                                              x.id,
                                              x.name,
                                              x.price,
                                              x.price_after_discount,
                                              x.capacity
                                          }).ToList();
                return Ok(IndividualCategory);
            }
            return NotFound();
        }

        //delete photo of activity from (website not app) 
        [Authorize]
        [Route("DeleteActivityPhotoOnServer")]
        public IActionResult DeleteActivityPhotoOnServer(int activityid, int photoid)
        {
            var activity = db.Activity.Find(activityid);

            var directory = new DirectoryInfo("Activity_Image/" + activity.id);
            if (directory.Exists == true)
            {
                foreach (FileInfo file in directory.GetFiles())
                {
                    if (file.Name == photoid + ".png")
                    {
                        file.Delete();
                        return Ok(new { status = 1, message = "photo deleted successfully" });
                    }
                    return Ok(new { status = 0, message = "Photo Not found" });
                }
            }
            return Ok(new { status = 0, message = "Activity Folder not found" });
        }


        //delete photo of activity from (app) and delete directory if not find any remaining photos 
        [Authorize]
        [Route("DeleteActivityPhoto")]
        public IActionResult DeleteActivityPhoto(int activityid, int? photoid)
        {
            var activity = db.Activity.Find(activityid);
            if (activity == null && photoid == null)
            {
                return Ok(new { status = 0, message = "Check data" });
            }
            var photo = db.Activity_Photos.Find(photoid);
            if (photo == null)
            {
                return Ok(new { status = 0, message = "photo doesn't exist" });
            }

            var directory = new DirectoryInfo("Activity_Image/" + activity.id);
            if (directory.Exists == true)
            {
                foreach (FileInfo file in directory.GetFiles())
                {
                    if (file.Name == photoid + ".png")
                        file.Delete();
                }
            }
            db.Activity_Photos.Remove(photo);
            db.SaveChanges();
            var remaining_photo = db.Activity_Photos.Where(x => x.activity_id == activityid).ToList();
            if (remaining_photo.Count() == 0)
                directory.Delete();

            return Ok(new { status = 1, message = "photo deleted successfully" });
        }

        //To get all availablility from activity 
        [Authorize]
        [Route("GetAvailablility")]  //Not Used
        public IActionResult GetAvailablility(int activityid)
        {
            if (activityid != 0)
            {
                var avaliabilityActivity = db.Avaliability.Where(x => x.activity_id == activityid)
                                          .Select(x => new { x.activity_Start, x.activity_End, x.isForGroup, x.id }).ToList();
                return Ok(avaliabilityActivity);
            }
            return NotFound();
        }

        ////To get overview 
        //[Route("ActivitiesOverview")] //Not Used
        //public IActionResult ActivitiesOverview()
        //{
        //    var activities = db.Activity.Select(a => new { a.id, a.title }).ToList();
        //    if (activities != null)
        //        return Ok(activities);
        //    else
        //        return NoContent();
        //}


        /// <summary>
        /// To create and edit Avaliability_Pricing from activity according to mode and avaliabilityid
        /// mode=0 (Add)  , mode=1 (Edit)
        /// 
        /// </summary>
        /// <param name="avalibilityPricingModel"></param>
        /// <param name="avaliabilityid"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("Avaliability_Pricing")]
        public IActionResult Avaliability_Pricing([FromBody]AvalibilityPricingModel avalibilityPricingModel, int avaliabilityid, int mode)
        {
            Avaliability avaliability = db.Avaliability.Find(avaliabilityid);
            if (avaliability == null)
                return BadRequest();
            if (mode == 2)
            {
                var avaliability_Pricings = db.Avaliability_Pricings.Where(x => x.avaliabilityId == avaliabilityid).ToList();

                if (avaliability_Pricings.Count != 0)
                {
                    foreach (var price in avaliability_Pricings)
                        db.Avaliability_Pricings.Remove(price);
                    db.SaveChanges();
                }
            }

            Avaliability_Pricing avaliability_Pricing = new Avaliability_Pricing
            {
                avaliabilityId = avaliabilityid,
                individualCategoryId = avalibilityPricingModel.individualCategoryId,
                price = avalibilityPricingModel.price,
                priceAfterDiscount = avalibilityPricingModel.priceAfterDiscount
            };
            db.Avaliability_Pricings.Add(avaliability_Pricing);
            db.SaveChanges();

            return Ok(new { Avaliability_Pricing_id = avaliability_Pricing.id });
        }

        //Create Activity in More Requests (Request 1)   
        //mode=0 (Add)  , mode=1 (Edit)

        [Authorize]
        [HttpPost]
        [Route("Create_Activity")]
        public IActionResult Create_Activity([FromBody]ActivityCreatorModel activityCreatorModel, int activityid, int mode)
        {

            if (activityCreatorModel != null)
            {
                int userid = int.Parse(HttpContext.User.FindFirst("userId").Value);
                Activity activity = db.Activity.Find(activityid);
                if (activity == null & mode == 2)
                    return BadRequest();

                if (mode == 1)  //add
                {
                    Activity activity_add = new Activity
                    {
                        title = activityCreatorModel.title,
                        type_id = activityCreatorModel.type_id,
                        description = activityCreatorModel.description,
                        has_individual_categories = activityCreatorModel.has_individual_categories,
                        has_specific_capacity = activityCreatorModel.has_specific_capacity,
                         user_id = userid,
                        isdeleted = false
                    };
                    db.Activity.Add(activity_add);
                    activity_add.stepNumber = 1;  //(create Title,Description,Type,Options)
                    foreach (var option in activityCreatorModel.Activity_Options)
                    {
                        Activity_Option activity_Option = new Activity_Option
                        {
                            option_id = option.option_id,
                            activity_id = activity_add.id
                        };
                        db.Activity_Option.Add(activity_Option);
                    }
                    db.SaveChanges();  //edit , Add
                    return Ok(new { activityId = activity_add.id });
                }

                if (mode == 2)
                {
                    activity.title = activityCreatorModel.title;
                    activity.type_id = activityCreatorModel.type_id;
                    activity.description = activityCreatorModel.description;
                    activity.user_id = userid;
                    activity.isdeleted = false;
                    db.SaveChanges();
                    var activity_options = db.Activity_Option.Where(x => x.activity_id == activityid).ToList();
                    if (activity_options.Count != 0)
                    {
                        foreach (var option in activity_options)
                            db.Activity_Option.Remove(option);

                        db.SaveChanges();
                    }
                    foreach (var option in activityCreatorModel.Activity_Options)
                    {
                        Activity_Option activity_Option = new Activity_Option
                        {
                            option_id = option.option_id,
                            activity_id = activity.id,
                            fromAge = option.fromAge,
                            toAge = option.toAge
                        };
                        db.Activity_Option.Add(activity_Option);
                        db.SaveChanges();
                    }
                    return Ok(new { activityId = activity.id });
                }
            }
            return BadRequest();
        }

        //(Request 2)
        [Authorize]
        [HttpPost]
        [Route("Create_ActivityPhoto")]
        public IActionResult Create_ActivityPhoto([FromBody]ActivityCreatorModel activityCreatorModel, int activityId, int mode)
        {
            var activity = db.Activity.Find(activityId);
            if (activity != null)
            {
                if (mode == 2)
                {
                    var activity_photo = db.Activity_Photos.Where(x => x.activity_id == activityId).ToList();
                    var directory = new DirectoryInfo("Activity_Image/" + activity.id);
                    if (directory.Exists == true)
                    {
                        foreach (FileInfo file in directory.GetFiles())
                        {
                            file.Delete();
                        }
                        directory.Delete();
                    }


                    if (activity_photo.Count != 0)
                    {
                        foreach (var photo in activity_photo)
                            db.Activity_Photos.Remove(photo);

                        db.SaveChanges();
                    }
                }

                foreach (var photo in activityCreatorModel.Activity_Photos)
                {
                    Activity_Photos activity_Photos = new Activity_Photos
                    {
                        activity_id = activity.id,
                        cover_photo = photo.cover_photo
                    };
                    db.Activity_Photos.Add(activity_Photos);
                    db.SaveChanges();

                    var path = Path.Combine("Activity_Image", "" + activity.id);
                    var directory = new DirectoryInfo(path);
                    if (directory.Exists == false)
                        directory.Create();

                    using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(photo.base64Img)))
                    {
                        using (Bitmap bm2 = new Bitmap(ms))
                        {
                            bm2.Save("Activity_Image/" + activity.id + "/" + activity_Photos.id + ".png");
                        }
                    }

                    string webRootPath = _hostingEnvironment.WebRootPath;
                    string contentRootPath = _hostingEnvironment.ContentRootPath;
                    //   string FilePath = string.Concat(GetUserImage.ImagePathForUserPhoto, regcustomer.id.ToString(), ".png");
                    //  activity_Photos.url = Path.Combine(_hostingEnvironment.ContentRootPath, "Activity_Image/" + activity.id + "/" + activity_Photos.id + ".png");
                    activity_Photos.url = string.Concat(activity.id + "/" + activity_Photos.id + ".png");
                }
                db.SaveChanges();

                if (mode == 1)
                {
                    activity.stepNumber = 2;   // 2 (Create Activity_Photo)
                    db.SaveChanges();
                }
                return Ok(new { activityId = activity.id });
            }
            return BadRequest();
        }

        //(Request 3)
        [Authorize]
        [HttpPost]
        [Route("Create_ActivityAddons")]
        public IActionResult Create_ActivityAddons([FromBody]ActivityCreatorModel activityCreatorModel, int activityId, int mode)
        { //When Pass id in header and List in Body (list= null) 

            var activity = db.Activity.Find(activityId);
            if (activity != null)
            {
                if (mode == 2)
                {
                    var activity_Add_Ons = db.Activity_Add_Ons.Where(x => x.activity_id == activityId).ToList();
                    if (activity_Add_Ons.Count != 0)
                    {
                        foreach (var addon in activity_Add_Ons)
                            db.Activity_Add_Ons.Remove(addon);

                        db.SaveChanges();
                    }
                }
                foreach (var addon in activityCreatorModel.Activity_Add_Ons)
                {
                    Activity_Add_Ons activity_Add_Ons = new Activity_Add_Ons
                    {
                        activity_id = activity.id,
                        add_ons_id = addon.add_ons_id,
                        price = addon.price,
                        provider_Username = addon.provider_Username,
                        addons_number = addon.addons_number,
                        note = addon.note
                    };
                    db.Activity_Add_Ons.Add(activity_Add_Ons);
                }
                db.SaveChanges();
                if (mode == 1)
                {
                    activity.stepNumber = 3;   // 3 (Create Addons)
                    db.SaveChanges();
                }
                return Ok(new { activityId = activity.id });
            }
            return BadRequest();
        }

        //(Request 4)
        [Authorize]
        [HttpPost]
        [Route("Create_ActivityLocation")]
        public IActionResult Create_ActivityLocation([FromBody]ActivityCreatorModel activityCreatorModel, int activityId, int mode)
        {
            var activity = db.Activity.Find(activityId);
            if (activity != null)
            {
                activity.activity_Location = activityCreatorModel.activity_Location;
                activity.meeting_Location = activityCreatorModel.meeting_Location;

                activity.activity_Lat = activityCreatorModel.activity_Lat;
                activity.activity_Lang = activityCreatorModel.activity_Lang;
                activity.LocationFlag = activityCreatorModel.LocationFlag;
                if (activityCreatorModel.LocationFlag == true)
                {
                    activity.meeting_Lat = activityCreatorModel.activity_Lat;
                    activity.meeting_Lang = activityCreatorModel.activity_Lang;
                }
                else
                {
                    activity.meeting_Lat = activityCreatorModel.meeting_Lat;
                    activity.meeting_Lang = activityCreatorModel.meeting_Lang;
                    if (activity.meeting_Lat == activity.activity_Lat & activity.meeting_Lang == activity.activity_Lang)
                    {
                        activity.LocationFlag = true;
                    }
                }
                if (mode == 1)
                    activity.stepNumber = 4;       // 4 (Create Location)

                db.SaveChanges();
                return Ok(new { activityId = activity.id });
            }
            return BadRequest();
        }

        //(Request 5)
        [Authorize]
        [HttpPost]
        [Route("Create_ActivityRules")]
        public IActionResult Create_ActivityRules([FromBody]ActivityCreatorModel activityCreatorModel, int activityId, int mode)
        {
            var activity = db.Activity.Find(activityId);

            if (activity != null)
            {
                activity.requirements = activityCreatorModel.requirements;

                if (mode == 2)
                {
                    var activity_Rules = db.Activity_Rule.Where(x => x.activity_id == activityId).ToList();
                    if (activity_Rules.Count != 0)
                    {
                        foreach (var rule in activity_Rules)
                            db.Activity_Rule.Remove(rule);

                        db.SaveChanges();
                    }
                }
                foreach (var rule in activityCreatorModel.Activity_Rules)
                {
                    Activity_Rule activity_Rule = new Activity_Rule
                    {
                        activity_id = activity.id,
                        rule_id = rule.rule_id
                    };
                    db.Activity_Rule.Add(activity_Rule);
                }
                db.SaveChanges();
                if (mode == 1)
                    activity.stepNumber = 5;   // 5 (Create Rules &Requirments)

                db.SaveChanges();
                return Ok(new { activityId = activity.id });
            }
            return BadRequest();
        }

        //(Request 6)
        [Authorize]
        [HttpPost]
        [Route("Create_BookingPrefrence")]
        public IActionResult Create_BookingPrefrence([FromBody]BookingSettingModel bookingSettingModel, int activityId, int mode)
        {
            var activity = db.Activity.Find(activityId);

            if (activity != null)
            {
                activity.notice_in_advance = bookingSettingModel.notice_in_advance;
                activity.booking_window = bookingSettingModel.booking_window;
                activity.bookingAvailableForIndividuals = bookingSettingModel.bookingAvailableForIndividuals;
                activity.bookingAvailableForGroups = bookingSettingModel.bookingAvailableForGroups;

                if (mode == 1)
                    activity.stepNumber = 6;   // 6 (Create Booking Prefrences)

                db.SaveChanges();
                return Ok(new { activityId = activity.id });
            }
            return BadRequest();
        }


        //(Request 7)
       [Authorize]
        [HttpPost]
        [Route("Create_ActivityAvailabilty")]  //Eman
        public IActionResult Create_ActivityAvailabilty([FromBody]BookingSettingModel bookingSettingModel, int activityId, int mode)
        {
            var activity = db.Activity.Find(activityId);
           
            if (activity != null)
            {
                if (mode == 2)
                {

                    List<int> nonUpdated_ids = new List<int>();
                    foreach (var aval in bookingSettingModel.avalibilityModels)
                    {
                       // var startday = aval.activity_Start.Value.DayOfWeek;
                       // var endday = aval.activity_End.Value.DayOfWeek;

                        if (aval.id == 0)
                        {//i need to prob this part further                            
                            aval.total_tickets = 0;
                            aval.activity_id = activity.id;
                            db.Avaliability.Add(aval);
                            db.SaveChanges();
                        }
                        else
                        {
                            var reservation = db.Booking.Where(x => x.avaliability_id == aval.id).Select(x => x.id).ToList();
                            if (reservation.Count != 0)
                            {
                                nonUpdated_ids.Add(aval.id);
                            }
                            else
                            {
                                var availbility = db.Avaliability.Find(aval.id);
                                //now Abiola i need to ensure i change the day of the week part of aval.activity_start to the day sent by Ebuka
                                //same for end date
                                if(availbility != null)
                                {
                                    availbility.activity_Start = aval.activity_Start;
                                    availbility.activity_End = aval.activity_End;
                                    availbility.enddate = aval.enddate;
                                    availbility.startdate = aval.startdate;
                                    availbility.isForGroup = aval.isForGroup;
                                    availbility.starthour = aval.starthour;
                                    availbility.endhour = aval.endhour;
                                    //I disable this temporarily based on ebuka and monsur request
                                    // availbility.group_Price = aval.group_Price;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    var avanew = new Avaliability
                                    {
                                        activity_id= activityId,
                                       activity_Start = aval.activity_Start,
                                  activity_End = aval.activity_End,
                                    enddate = aval.enddate,
                                    startdate = aval.startdate,
                                   isForGroup = aval.isForGroup,
                                   starthour = aval.starthour,
                                  endhour = aval.endhour,
                                };
                                    db.Avaliability.Add(avanew);
                                    db.SaveChanges();
                                }
                                
                            }
                        }

                    }
                    if (nonUpdated_ids.Count != 0)
                        return Ok(new { message = " There are some availability that cannot be modified because they have reservations ", availability_ids = nonUpdated_ids });
                }


                if (mode == 1)
                {
                    foreach (var aval in bookingSettingModel.avalibilityModels)
                    {
                        var startday = aval.activity_Start.Value.DayOfWeek;
                        var endday = aval.activity_End.Value.DayOfWeek;
                        aval.total_tickets = 0;
                        aval.activity_id = activity.id;
                        db.Avaliability.Add(aval);
                    }

                    activity.stepNumber = 7;      // 7 (Create Availabilty)
                    db.SaveChanges();
                }

                return Ok(new { message = "Successfully submitted, Thanks ", activityId = activity.id });
            }
            return BadRequest();
        }

        //(Request 8)
        // [Authorize]
        [HttpPost]
        [Route("Create_ActivityLength")]
        public IActionResult Create_ActivityLength([FromBody]BookingSettingModel bookingSettingModel, int activityId)
        {
            var activity = db.Activity.Find(activityId);

            if (activity != null)
            {
                //added the has category and has capacity to this 
                activity.Activity_length = bookingSettingModel.Activity_length;
                activity.totalCapacity = bookingSettingModel.totalCapacity;
                activity.min_capacity_group = bookingSettingModel.min_capacity_group;
                activity.max_capacity_group = bookingSettingModel.max_capacity_group;
                activity.capacityIsUnlimited = bookingSettingModel.capacityIsUnlimited;
                activity.has_individual_categories = bookingSettingModel.has_individual_categories;
                activity.has_specific_capacity = bookingSettingModel.has_specific_capacity;
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(bookingSettingModel.individual_categories);
                activity.modified_date = DateTime.Now.Date;
                activity.individual_categories= json;
                db.SaveChanges();
                return Ok(new { message = "activity length created successfully", activityId = activity.id, IndividualCategories = activity.individual_categories });
                //if (mode == 2)
                //{
                //    foreach (var individual in bookingSettingModel.individualCategories)
                //    {
                //        if (individual.id == 0)
                //        {
                //            individual.activityid = activity.id;
                //            db.Individual_Categories.Add(individual);

                //        }
                //        else
                //        {
                //            var old_individual = db.Individual_Categories.Find(individual.id);
                //            if (old_individual == null)
                //                return BadRequest();
                //            old_individual.capacity = individual.capacity;
                //        }

                //    }
                //    db.SaveChanges();

                //}
                //if (mode == 1)
                //{
                //    foreach (var individual in bookingSettingModel.individualCategories)
                //    {
                //        individual.activityid = activity.id;
                //        db.Individual_Categories.Add(individual);
                //    }
                //    activity.stepNumber = 8;   // 8 (Create Capacity & Length)
                //    db.SaveChanges();
                //    return Ok(new { message = "activity length created successfully", activityId = activity.id,IndividualCategories =activity.individual_categories});
                //}
                //var individualCategories = db.Individual_Categories.Where(x => x.activityid == activity.id).Select(x => new { x.id, x.name, x.capacity, x.price });
                //return Ok(new { activityId = activity.id, IndividualCategories = individualCategories });
            }
            return Ok(new { message="activity is null or empty"});

        }

        //(Request 9)
        [Authorize]
        [HttpPost]
        [Route("Create_ActivityPricing")]
        public IActionResult Create_ActivityPricing([FromBody]BookingSettingModel bookingSettingModel, int activityId, int mode)
        {
            var activity = db.Activity.Find(activityId);
            if (activity != null)
            {
                activity.group_price = bookingSettingModel.group_price;
                activity.apply_discount = bookingSettingModel.apply_discount;
                activity.price_discount = bookingSettingModel.price_discount;
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(bookingSettingModel.individual_categories);
                activity.modified_date = DateTime.Now.Date;
                activity.individual_categories = json;
                db.SaveChanges();
                return Ok(new { activityId = activity.id, individual_categories = json });
                //foreach (var individual in bookingSettingModel.individualCategories)
                //{
                //    var oldIndividual = db.Individual_Categories.Find(individual.id);
                //    if (oldIndividual == null)
                //        return BadRequest();
                //    oldIndividual.price = individual.price;
                //    oldIndividual.price_after_discount = individual.price_after_discount;

                //    db.SaveChanges();
                //}
                //if (mode == 1)
                //{
                //    activity.stepNumber = 9;   // 9 (Create Pricing & Payment)
                //    db.SaveChanges();
                //}

                //var individualCategories = db.Individual_Categories.Where(x => x.activityid == activity.id).Select(x => new { x.id, x.name, x.capacity, x.price });
               
            }
            return BadRequest();
        }

        [Authorize]
        [Route("DeleteIndividualCategory")]
        public IActionResult DeleteIndividualCategory( int individualCategoryId)
        {
            var individualCategory = db.Individual_Categories.Find(individualCategoryId);
            if (individualCategory == null)
                return BadRequest();
            var booking = db.Booking_individual_category_capacity.Where(x => x.category_id == individualCategoryId).ToList();
            if (booking.Count == 0)
            {
                db.Individual_Categories.Remove(individualCategory);
                db.SaveChanges();
                return Ok(new { status = 1 });
            }
            return Ok(new { status=0,message="Can't delete this category"});
        }



        //(Request 10)
        [Authorize]
        [HttpPost]
        [Route("Create_ActivityOrganizers")]
        public IActionResult Create_ActivityOrganizers([FromBody]BookingSettingModel bookingSettingModel, int activityId, int mode)
        {
            var activity = db.Activity.Find(activityId);
            if (activity != null)
            {
                if (mode == 2)
                {
                    var organizers = db.Activity_Organizer.Where(x => x.Activity.id == activityId).ToList();
                    if (organizers.Count != 0)
                    {
                        foreach (var organize in organizers)
                            db.Activity_Organizer.Remove(organize);

                        db.SaveChanges();
                    }
                }
                foreach (var organizer in bookingSettingModel.activity_Organizers)
                {
                    Activity_Organizer activity_Organizer = new Activity_Organizer
                    {
                        activity_id = activity.id,
                        name = organizer.name,
                        mail = organizer.mail,
                        mobile = organizer.mobile,
                        Organizer_Typeid = organizer.Organizer_Typeid
                    };
                    db.Activity_Organizer.Add(activity_Organizer);
                }
                if (mode == 1)
                {
                    activity.stepNumber = 10;   // 10 (Create Organizer) 
                    activity.isCompleted = true;
                    activity.status = true;
                }
                db.SaveChanges();
                return Ok(new { activityId = activity.id });
            }
            return BadRequest();
        }
        //Create Activity in one Request
        [Authorize]
        [HttpPost]
        [Route("CreateActivity")]
        public IActionResult CreateActivity([FromBody]ActivityCreatorModel activityCreatorModel)
        {
            int userid = int.Parse(HttpContext.User.FindFirst("userId").Value);

            if (activityCreatorModel != null)
            {
                Activity activity = new Activity
                {
                    title = activityCreatorModel.title,
                    type_id = activityCreatorModel.type_id,
                    description = activityCreatorModel.description,
                    activity_Location = activityCreatorModel.activity_Location,
                    meeting_Location = activityCreatorModel.meeting_Location,
                    requirements = activityCreatorModel.requirements,
                    user_id = userid
                };
                db.Activity.Add(activity);
                db.SaveChanges();

                foreach (var photo in activityCreatorModel.Activity_Photos)
                {
                    Activity_Photos activity_Photos = new Activity_Photos
                    {
                        url = GetUserImage.OnlineImagePathForActivity+ photo.url,
                        activity_id = activity.id,
                        cover_photo = photo.cover_photo
                    };
                    db.Activity_Photos.Add(activity_Photos);
                }

                foreach (var option in activityCreatorModel.Activity_Options)
                {
                    Activity_Option activity_Option = new Activity_Option
                    {
                        activity_id = activity.id,
                        option_id = option.option_id
                    };
                    db.Activity_Option.Add(activity_Option);
                }

                foreach (var addon in activityCreatorModel.Activity_Add_Ons)
                {
                    Activity_Add_Ons activity_Add_Ons = new Activity_Add_Ons
                    {
                        activity_id = activity.id,
                        add_ons_id = addon.add_ons_id,
                        price = addon.price,
                        provider_Username = addon.provider_Username,
                        addons_number = addon.addons_number,
                        note = addon.note
                    };
                    db.Activity_Add_Ons.Add(activity_Add_Ons);
                }


                foreach (var rule in activityCreatorModel.Activity_Rules)
                {
                    Activity_Rule activity_Rule = new Activity_Rule
                    {
                        activity_id = activity.id,
                        rule_id = rule.rule_id
                    };
                    db.Activity_Rule.Add(activity_Rule);
                }
                db.SaveChanges();
                return Ok(activity);
                //db.Entry(activity).State = EntityState.Modified;     
            }
            return BadRequest(new { message = "Check Data" });
        }
        [Authorize]
        [HttpPost]
        [Route("Create_PhotoURL")]
        public IActionResult Create_PhotoURL([FromBody]Activity_Photo Photo, int activityId)
        {
            var activity = db.Activity.Find(activityId);
            if (activity != null)
            {
                Activity_Photos activity_Photos = new Activity_Photos
                {
                    activity_id = activityId,
                    cover_photo = Photo.cover_photo
                };
                db.Activity_Photos.Add(activity_Photos);
                //db.SaveChanges();

                var path = Path.Combine("Activity_Image", "" + activityId);
                var directory = new DirectoryInfo(path);
                if (directory.Exists == false)
                    directory.Create();

                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(Photo.base64Img)))
                {
                    using (Bitmap bm2 = new Bitmap(ms))
                    {
                        bm2.Save("Activity_Image/" + activityId + "/" + activity_Photos.id + ".png");
                    }
                }
                var url = GetUserImage.OnlineImagePathForActivity+Path.Combine(activity.id + "/" + activity_Photos.id + ".png");
               // db.SaveChanges();
                return Ok(new {PhotoURL= url });
            }
            return Ok(new { message = "PLZ , Check activity id" });
        }     


        [Route("DeleteActivity")]
        [Authorize]
        public IActionResult DeleteActivity(int id)
        {
            var activity = db.Activity.Find(id);
            if (activity == null)
            {
                return Ok(new { status = 0 });
            }
            activity.isdeleted = true;
            db.SaveChanges();
            return Ok(new { status=1});
        }

        [HttpGet]
        [Authorize]
        [Route("ActivityListing")]
        public IActionResult ActivityListing()
        {
            int userid = int.Parse(HttpContext.User.FindFirst("userId").Value);
            if (userid != null)
            {
                var useractivitylisting = db.Activity.Where(a => a.user_id == userid).AsEnumerable();
                return Ok(new { useractivitylisting });
            }            
            return BadRequest(new { message ="Invalid request, user does not exist"})  ;
        }
         [HttpGet]
        [Route("GetOnlineActivity")]
       
        public async Task<IActionResult> GetOnlineActivity(bool status, bool isCompleted)
        {
            var onlineactivity =  db.Activity.Where(c =>  c.isCompleted == isCompleted ? true : false).AsEnumerable();
            var onlineact = onlineactivity.Select(d => new UserOnlineActivity
            {
                ActivityCoverPhotos = d.Activity_Photos.Select(f => new ActivityCoverPhotos
                {
                    activity_id = f.activity_id.Value,
                    url = f.url,
                    cover_photo = f.cover_photo
                }),
                ActivityId = d.id,
                Title = d.title,
                LastEditedDate = d.modified_date
            }
                  ).OrderByDescending(c => c.ActivityId).AsEnumerable();
            return Ok(new { message = "Successful", activity = onlineact });
        }
        [HttpGet]
        [Route("GetUserOnlineActivity")]

        public IActionResult GetUserOnlineActivity(bool isCompleted)
        {
            //; 
            // var userid = GetUserId();
            var userid = GetUserId(); ;
            if (isCompleted ==false)
            {

                ///  var onlineactivity =await .ToListAsync();

                var onlineact = db.Activity.Where(c => c.user_id == userid && !c.status|| !c.isCompleted).ToList();
                var usractivity = onlineact.Select(d => new
                {
                    ActivityCoverPhotos = db.Activity_Photos.Where(c => c.activity_id == d.id).Select(f => new
                    {
                        activity_id = f.activity_id.Value,
                        url = f.url,
                        cover_photo = f.cover_photo
                    }),
                    user = d.user_id,
                    ActivityId = d.id,
                    Title = d.title,
                    LastEditedDate = d.modified_date ?? d.creation_date,
                    isCompleted=d.isCompleted,
                    status=d.status,
                    rate = db.Reviews.Where(c => c.activity_id == d.id).Select(c=>c.rate).Average()
                }
                    ).OrderByDescending(c => c.ActivityId).Select(c => new
                    {
                        user = c.user,
                        ActivityCoverPhotos = c.ActivityCoverPhotos,
                        ActivityId = c.ActivityId,
                        Title = c.Title,
                        LastEditedDate = c.LastEditedDate,
                        isCompleted=c.isCompleted,
                        status=c.status,
                        Rate=c.rate??0
                    }).ToList();
                if (onlineact != null)
                {
                    return Ok(new { message = "Successful",online="online", activity = usractivity, userid = userid });
                }
            }
            else if(isCompleted==true)
            {

                var onlineact = db.Activity.Where(c => c.user_id == userid &&c.status||c.isCompleted).ToList();
                   var usractivity = onlineact.Select(d => new 
                     {
                         ActivityCoverPhotos = db.Activity_Photos.Where(c => c.activity_id == d.id).Select(f => new 
                         {
                             activity_id = f.activity_id.Value,
                             url = f.url,
                             cover_photo = f.cover_photo
                         }),
                       user= d.user_id,
                         ActivityId = d.id,
                         Title = d.title,
                         LastEditedDate = d.modified_date ?? d.creation_date,
                      rate = db.Reviews.Where(c => c.activity_id == d.id).Select(c => c.rate).Average()
                   }
                    ).OrderByDescending(c => c.ActivityId).Select(c => new
                    {
                        user = c.user,
                        ActivityCoverPhotos = c.ActivityCoverPhotos,
                        ActivityId = c.ActivityId,
                        Title = c.Title,
                        LastEditedDate = c.LastEditedDate,
                        Rate = c.rate
                    }).ToList();
                if (onlineact != null)
                {
                    return Ok(new { message = "Successful", offline = "offline", activity = usractivity, userid= userid });
                }
            }    

            return Ok(new { message = "Successful", activity = new List<UserOnlineActivity>() });
        }
     
        [HttpGet]
        [Route("GetActivityOption")]
       
        public IActionResult GetActivityOption()
        {            
            var activityoptions = db.Option.Select(f=> new GetActivityOptionModel {
                option_id = f.id,
                fromAge = f.fromAge.Value,
                _class = f._class,
                toAge=f.toAge.Value,
                name = f.name
            }).AsEnumerable();
            if (activityoptions != null)
            {
                return Ok(new { message = "Successful", activityoptions = activityoptions });
            }
            return Ok(new { message = "Successful but no activity matches", activityoptions = ""});
        }

        [HttpPost]
        [Route("EditActivity")]
      //  [Authorize]
        public IActionResult EditActivity([FromBody] GetActivityOption editActivity, string activityid)
        {
           
            //foreach (var options in listofactivityoptions)
            //{
            //    UpdateEditActivity(options, activity_id, actcount);
            //}
  int activity_id = int.Parse(activityid);
            try
            {
              
                int userid = GetUserId();
                // var type_id= db.Option.Where(c=>c.name=)
                var activityoption = db.Activity.Where(a => a.id == activity_id && a.user_id == userid).FirstOrDefault();
                var listofactivityoptions = editActivity.Activity_Options;
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(listofactivityoptions);
                activityoption.modified_date = DateTime.Now.Date;
                activityoption.activity_option = json;
                var updateactvitytable = db.Activity.Where(c => c.id == activity_id).FirstOrDefault();
                updateactvitytable.title = editActivity.title;
                updateactvitytable.description = editActivity.description;
                updateactvitytable.modified_date = DateTime.UtcNow;
                db.SaveChanges();
                return Ok(new
                {
                    message = "Successful",
                    activity = activity_id
                });
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    message = $"Error Occured {e.Message}",
                    activity = activity_id
                });
            }
            
            
        }
        public void UpdateEditActivity()
        {

        }
        //public void UpdateEditActivity(Activity_Options options, int activity_id, int counts)
        //{
        //    var optid = int.Parse(options.option_id);
        //    var data = db.Activity_Option.Where(d => d.activity_id == activity_id).ElementAt(counts);
        //    if (data != null)
        //    {
                
        //       data.activity_id = activity_id;
        //       data.option_id = optid;
        //       data.fromAge = options.fromAge;
        //       data.toAge = options.toAge;
        //        db.SaveChanges();
        //    }
        //    //else
        //    //{
        //    //    var newoptionadd = new Activity_Option
        //    //    {
        //    //        activity_id = activity_id,
        //    //        option_id = int.Parse(options.option_id),
        //    //        fromAge = options.fromAge,
        //    //        toAge = options.toAge
        //    //    };
        //    //    db.Activity_Option.Add(newoptionadd);
        //    //    db.SaveChanges();
        //    //}
           
        //}

    }

}
