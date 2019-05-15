using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Core.Api.Models;
using Core.Api.Helper;
using Core.Web.Models;
using System.Drawing;
using Microsoft.AspNetCore.Cors;
using System.Threading.Tasks;

namespace Core.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    //[Authorize]
    public class UserController : BaseController
    {

        AppDbContext db = new AppDbContext();

        [HttpPost]

        [Route("createOrganizer")]
        public IActionResult createOrganizer([FromBody]Activity_Organizer organizer)
        {
            if (organizer.name != null)
            {
                db.Activity_Organizer.Add(organizer);
                db.SaveChanges();
                return Ok(new { organizerid = organizer.id });
            }
            return BadRequest(new { message = "Please,insert Organizer Name" });
        }


        [HttpGet]

        [Route("GetOrganizers")]
        public IActionResult GetOrganizers()  //***
        {
            var organizers = db.Activity_Organizer.Include(x => x.Organizer_Type).Select(x => new
            {
                x.id,
                x.name,
                x.Organizer_Type.type,
                x.Organizer_Type.description,
                x.mobile,
                x.mail
            }).ToList();

            if (organizers != null)
                return Ok(organizers);
            else
                return NoContent();
        }



        [HttpGet]

        [Route("GetOrganizerTypes")]
        public IActionResult GetOrganizerTypes()  //***
        {
            var organizerTypes = db.Organizer_Type.Select(x => new { x.type, x.id, x.description }).ToList();
            if (organizerTypes != null)
                return Ok(organizerTypes);
            else
                return NoContent();
        }


        [HttpPost]

        [Route("ForgetPassword")]
        public IActionResult ForgetPassword([FromBody]string email)
        {
            var user = db.User.Where(x => x.mail == email).First();
            if (user != null)
            {
                new MailHelper().SendMail(email, "Dear " + user.user_name + " your Password " + user.password);
                return Ok();
            }
            return Ok(new { message = "Please,insert your email correctly " });

        }


        [Route("ShowStatistics")]
        public IActionResult ShowStatistics(string providerId)
        {
            //1- Earning Statistics
            var statistics = db.Activity.Join(db.Booking, a => a.id, b => b.activity_id, (a, b) => new { a, b }).Where(c => c.b.user_id == int.Parse(providerId)).
                Select(c => new { activityid = c.a.id, c.a.title, c.b.booking_amount });

            var earning_statistics = statistics.GroupBy(x => new { x.activityid, x.title }).Select(g => new { g.Key.activityid, g.Key.title, TotalAmount = g.Sum(s => s.booking_amount) });

            //2- Ticket Statistics
            var Bookings = db.Activity.Join(db.Booking, a => a.id, b => b.activity_id, (a, b) => new { a, b }).Where(c => c.b.user_id == int.Parse(providerId)).
                Select(c => new { activityid = c.a.id, c.a.title, bookingid = c.b.id, c.b.booking_amount });
            var tstatistics = Bookings.Join(db.Booking_Ticket, b => b.bookingid, t => t.booking_id, (b, t) => new { b, t }).
                Select(c => new { activityid = c.b.activityid, c.b.title, c.t.ticket_number });

            var tickets_statistics = tstatistics.GroupBy(x => new { x.activityid, x.title }).Select(g => new { g.Key.activityid, g.Key.title, NumberOfTickets = g.Count() });

            //
            if (earning_statistics != null || tickets_statistics != null)
                return Ok(Json(new
                {
                    revenue = earning_statistics,
                    tickets = tickets_statistics
                }).Value);
            //return Ok(earning_statistics);
            else
                return NoContent();
        }


        [HttpGet]
        [Route("GetProfile")]
        [Authorize]
        public IActionResult GetProfile(string user_id)  //*

        {
            try
            {
                //GetUserId();
                int userid = int.Parse(user_id);
                var reguser = db.User.Find(userid);
                if (reguser == null)
                    return Ok(new { status = 0, message = "User doesn't exist" });

                var user = db.User.Where(u => u.id == userid).Select(u => new
                {
                   id= u.id,
                  first_name = u.first_name,
                   last_name= u.last_name,
                  mail=  u.mail,
                   user_name= u.user_name,
                   password= u.password,
                   mobile= u.mobile,
                    UserPhoto_Url = Path.Combine((u.UserPhoto_Url == null ? "NA" : GetUserImage.OnlineImagePathForUserPhoto + u.UserPhoto_Url)),
                    user_Type=u.user_Type,
                    gender=u.gender,
                   DOB= u.DOB,
                   bio=u.bio,
                    preferablePrice=u.preferablePrice
                });

                if (user != null)
                    return Ok(user);
                else
                    return NoContent();
            }
            catch (Exception)
            {
                return Ok(new { status = -1, message = "Failed to retrieve usert information." });
            }
        }
        [HttpGet]

        [Route("Inbox")]
        public IActionResult Inbox()  //***
        {
            int userid = int.Parse(HttpContext.User.FindFirst("userId").Value);

            var messagesInbox = db.Chat.Include(x => x.user).Include(x => x.user1).Where(x => x.firstUser == userid | x.secondUser == userid)
                               .Select(x => new InboxChat
                               {
                                   chatId = x.chatId,
                                   lastUpdateDate = x.lastUpdateDate,
                                   firstUser = (x.user.id == userid) ? x.secondUser : x.firstUser,
                                   UserName = (x.user.id == userid) ? x.user1.user_name : x.user.user_name,
                                   UserPhoto_Url = (x.user.id == userid) ? Path.Combine((x.user1.UserPhoto_Url == null ? "NA" : GetUserImage.OnlineImagePathForUserPhoto + x.user1.UserPhoto_Url)) :
                                                                          Path.Combine((x.user.UserPhoto_Url == null ? "NA" : GetUserImage.OnlineImagePathForUserPhoto + x.user.UserPhoto_Url)),
                                   unReadableMessageNo = db.MessageReplies.Where(s => s.user.id != userid & s.isReadable == false & s.chatId == x.chatId).Count(),
                                   activityName = db.MessageReplies.Where(s => s.chatId == x.chatId).Select(s => s.typeId).FirstOrDefault() == 2 ?
                                  db.Booking_Ticket.Include(q => q.Booking).Where(q => q.ticket_id == db.MessageReplies.Where(s => s.chatId == x.chatId).Select(s => s.regardingId).FirstOrDefault()).Select(q => q.Booking.Activity.title).FirstOrDefault() : "NA"
                               }).OrderByDescending(x => x.lastUpdateDate).ToList();

            return Ok(messagesInbox);

        }

        [HttpGet]

        [Route("CustomerInbox")]
        public IActionResult CustomerInbox()  //***
        {
            int userid = int.Parse(HttpContext.User.FindFirst("userId").Value);

            //Ticket ids 
            //var Tickets = db.MessageReplies.Include(x=>x.Chat).Where(x => (x.Chat.firstUser == userid| x.Chat.secondUser == userid) & x.typeId == 2)
            //             .Select(x => x.regardingId).Distinct();

            //var chats = new List<ChatItemModel>();
            //foreach (var ticket in Tickets)
            //{
            //    var activity = db.Booking_Ticket.Include(x => x.Booking).Where(x => x.ticket_id == ticket)
            //        .Select(x => new ChatItemModel
            //        {
            //            title = x.Booking.Activity.title,
            //            description = x.Booking.Activity.description,
            //            activityPhoto_Url = x.Booking.Activity.Activity_Photos.Where(s => s.cover_photo == true & s.activity_id == x.Booking.activity_id).Select(s => s.url).FirstOrDefault(),
            //            ticketId = ticket,
            //            unReadableMessageNo = db.MessageReplies.Where(s => s.regardingId == ticket & s.isReadable == false & s.typeId == 2).Count(),
            //            lastMessage = db.MessageReplies.Where(s => s.regardingId == ticket & s.typeId == 2).Select(s => s.date).Last()
            //        }).First();
            //    chats.Add(activity);
            //}
            //return Ok(chats.OrderByDescending(x => x.lastMessage));
            var Chats = db.MessageReplies.Include(x => x.Chat).Where(x => (x.Chat.firstUser == userid | x.Chat.secondUser == userid) & x.typeId == 2)
                .Select(x => new { x.chatId, x.regardingId }).Distinct();
            var chats = new List<ChatItemModel>();

            foreach (var Chat in Chats)
            {
                var activity = db.Booking_Ticket.Include(x => x.Booking).Where(x => x.ticket_id == Chat.regardingId)
                    .Select(x => new ChatItemModel
                    {
                        chatId = Chat.chatId,
                        title = x.Booking.Activity.title,
                        description = x.Booking.Activity.description,
                        activityPhoto_Url = x.Booking.Activity.Activity_Photos.Where(s => s.cover_photo == true & s.activity_id == x.Booking.activity_id).Select(s => s.url).FirstOrDefault(),
                        ticketId = Chat.regardingId,
                        unReadableMessageNo = db.MessageReplies.Where(s => s.regardingId == Chat.regardingId & s.isReadable == false & s.typeId == 2 & s.chatId == Chat.chatId).Count(),
                        lastMessage = db.MessageReplies.Where(s => s.regardingId == Chat.regardingId & s.typeId == 2 & s.chatId == Chat.chatId).Select(s => s.date).Last()
                    }).First();
                chats.Add(activity);
            }
            return Ok(chats.OrderByDescending(x => x.lastMessage));

        }
        [HttpGet]

        [Route("Cust_Chat")]
        public IActionResult Cust_Chat(int ticketId, int chatId)  //***
        {
            var avaliabilityTicket = db.Booking_Ticket.Include(x => x.Booking).Where(x => x.ticket_id == ticketId).Select(x => new { x.Booking.Avaliability.activity_Start, x.Booking.avaliability_id });
            var messages = db.MessageReplies.Include(x => x.user).Where(x => x.typeId == 2 & x.regardingId == ticketId & x.chatId == chatId)
                            .Select(x => new Cust_MessageInbox
                            {
                                message = x.message,
                                date = x.date,
                                messageReplayId = x.messageReplayId,
                                userId = x.userId,
                                user_name = x.user.user_name,
                                UserPhoto_Url = Path.Combine((x.user.UserPhoto_Url == null ? "NA" : GetUserImage.OnlineImagePathForUserPhoto + x.user.UserPhoto_Url))
                            }).OrderByDescending(x => x.messageReplayId);

            var unReadableMessagess = db.MessageReplies.Include(x => x.user).Where(x => x.regardingId == ticketId & x.isReadable == false & x.userId != GetUserId() & x.chatId == chatId).ToList();
            foreach (var item in unReadableMessagess)
            {
                item.isReadable = true;
            }
            db.SaveChanges();

            return Ok(new { avaliabilityTicket, messages });
        }

        [HttpGet]
        [Route("User_Chat")]
        public IActionResult User_Chat(int chatId)  //***
        {
            var chat = db.Chat.Find(chatId);
            if (chat == null)
                return BadRequest();

            var messages = db.MessageReplies.Include(x => x.user).Where(x => x.chatId == chatId)
                             .Select(x => new MessageInbox
                             {
                                 message = x.message,
                                 date = x.date,
                                 regardingId = x.regardingId,
                                 typeId = (x.typeId == 0) ? "General Message" : (x.typeId == 1) ? "Activity Message" : "Ticket Message",  // (0 >>>General ,1 >>>ActivityId ,2 >>>TicketId)
                                 messageReplayId = x.messageReplayId,
                                 userId = x.userId,
                                 user_name = x.user.user_name,
                                 UserPhoto_Url = Path.Combine((x.user.UserPhoto_Url == null ? "NA" : GetUserImage.OnlineImagePathForUserPhoto + x.user.UserPhoto_Url))
                             }).OrderByDescending(x => x.messageReplayId);

            var unReadableMessagess = db.MessageReplies.Include(x => x.user).Where(x => x.chatId == chatId & x.isReadable == false & x.userId != GetUserId()).ToList();
            foreach (var item in unReadableMessagess)
            {
                item.isReadable = true;
            }
            db.SaveChanges();

            return Ok(messages);
        }


        [HttpPost]

        [Route("send_message")]
        public IActionResult send_message([FromBody]MessageReply messageReply, int chatId, int availabilityId)  //***
        {

            Chat chat = new Chat();
            var tickets = db.Booking_Ticket.Include(x => x.Booking).Where(x => x.Booking.avaliability_id == availabilityId)
                               .Select(x => new { x.ticket_id, x.mail }).ToList();
            if (tickets.Count != 0)
            {
                foreach (var item in tickets)
                {
                    var userid = db.User.Where(s => s.mail == item.mail).Select(s => s.id).FirstOrDefault();
                    if (userid != null)
                    {
                        chat = db.Chat.Where(x => (x.firstUser == GetUserId() & x.secondUser == userid) |
                                                (x.secondUser == GetUserId() & x.firstUser == userid)).FirstOrDefault();
                        if (chat == null)
                        {
                            chat = new Chat();
                            chat.firstUser = GetUserId();
                            chat.secondUser = userid;
                            db.Chat.Add(chat);
                        }

                        MessageReply messageReply_ = new MessageReply()
                        {
                            date = DateTime.Now,
                            userId = GetUserId(),
                            chatId = chat.chatId,
                            message = messageReply.message,
                            regardingId = item.ticket_id,
                            typeId = 2
                        };

                        db.MessageReplies.Add(messageReply_);

                        chat.lastUpdateDate = messageReply_.date;
                        db.SaveChanges();
                    }
                }
                return Ok();
            }

            chat = db.Chat.Find(chatId);
            if (chat == null)
            {
                var user_mail = db.Booking_Ticket.Where(x => x.ticket_id == messageReply.regardingId).Select(x => x.mail).FirstOrDefault();
                if (user_mail == null)
                    return Ok(new { status = 0, message = "plz, insert chatId or regardingId or availabilityId" });
                var user_id = db.User.Where(x => x.mail == user_mail).Select(x => x.id).FirstOrDefault();
                chat = db.Chat.Where(x => (x.firstUser == GetUserId() & x.secondUser == user_id) |
                                             (x.secondUser == GetUserId() & x.firstUser == user_id)).FirstOrDefault();
                if (chat == null)
                {
                    chat = new Chat();
                    chat.firstUser = GetUserId();
                    chat.secondUser = user_id;
                    db.Chat.Add(chat);
                }
            }

            messageReply.date = DateTime.Now;
            messageReply.userId = GetUserId();
            messageReply.chatId = chat.chatId;
            db.MessageReplies.Add(messageReply);

            chat.lastUpdateDate = messageReply.date;
            db.SaveChanges();
            return Ok();
            // return Ok(new { MessageReplayId = messageReply.messageReplayId });
        }


        //delete photo of user from (website not app) 

        [Route("DeleteUserPhotoOnServer")]
        public IActionResult DeleteUserPhotoOnServer(int userId)
        {
            var user = db.User.Find(userId);
            if (user != null)
            {
                var directory = new DirectoryInfo("Userimages");
                if (directory.Exists == true)
                {
                    foreach (FileInfo file in directory.GetFiles())
                    {

                        if (file.Name == userId + ".png")
                        {
                            file.Delete();
                            return Ok(new { status = 1, message = "photo deleted successfully" });
                        }
                        return Ok(new { status = 0, message = "Photo Not found" });
                    }
                }
                return Ok(new { status = 0, message = "User Folder not found" });
            }
            return Ok(new { status = 0, message = "User Not found" });
        }


        //Save user photo on server (website) 

        [HttpPost]
        [Route("Create_URLOfUserPhoto")]
        public IActionResult Create_URLOfUserPhoto([FromBody]string Photo, string userId)
        {
            int user_id = int.Parse(userId);
            var user = db.User.Find(user_id);
            if (user != null)
            {
                var path = Path.Combine("Userimages");
                var directory = new DirectoryInfo(path);
                if (directory.Exists == false)
                    directory.Create();
                else
                {
                    foreach (FileInfo file in directory.GetFiles())
                    {
                        if (file.Name == userId + ".png")
                            file.Delete();
                    }
                }
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(Photo)))
                {
                    using (Bitmap bm2 = new Bitmap(ms))
                    {
                        bm2.Save("Userimages/" + userId + ".png");
                    }
                }
                var url = GetUserImage.OnlineImagePathForUserPhoto + Path.Combine(userId + ".png");

                return Ok(new { UserPhotoURL = url });
            }
            return Ok(new { message = "PLZ , Check user id" });
        }
        ///Sly just added this 6th of May 2019
        [Route("UpdateOrganizerInfo")]
        [HttpPost]
        public async Task<IActionResult> UpdateOrganizationInfo([FromBody]OrganizerInfo organizerInfo,string userId)
        {
            if (organizerInfo != null)
            {
                try
                {
                    int user_id = int.Parse(userId);
                    var user = db.User.Find(user_id);
                    SaveImageToPath(organizerInfo.base64Img, user, "UserImages");
                    var orgdata = db.User.FirstOrDefault(o => o.id == user_id);
                    orgdata.about_organization = organizerInfo.About;
                    orgdata.organization_name = organizerInfo.Name;
                    orgdata.bio = organizerInfo.Bio;
                    orgdata.mail = organizerInfo.Email;
                    orgdata.mobile = organizerInfo.phone;
                    orgdata.UserPhoto_Url = GetUserImage.OnlineImagePathForUserPhoto + Path.Combine(user.id + ".png");
                    await db.SaveChangesAsync();
                    return Ok(new { message = "successfully submitted" });
                }
                catch (Exception e)
                {

                    return Ok(new { message = $"Error occured, {e.Message} could help resolve it" });
                }

            }
            return BadRequest(new { message = "Error occured or submission is invalid" });
        }
///Sly just added this 6th of May 2019
       
        [HttpPost]
        [Route("VerificationIdentifcation")]
        public async Task<IActionResult> VerificationIdentifcation([FromBody]VerificationIdentifcation verificationIdentifcation,string userId)
        {
            if (verificationIdentifcation != null)
            {
                try
                {
                    //  int.Parse(userId);
                    int user_id = int.Parse(userId);
                    var user = db.User.Find(user_id);                    
                    SaveImageToPath(verificationIdentifcation.base64Img, user, "Organizationimages");
                    var orgdata = new UserIdentification
                    {
                        DOB = verificationIdentifcation.Dob,
                        gender = verificationIdentifcation.Gender,
                        nationality = verificationIdentifcation.Nationality,
                        identification_type = verificationIdentifcation.IdentificationType,
                        identification_number = verificationIdentifcation.IdentificationNumber,
                        expiry_date = verificationIdentifcation.ExpiryDate,
                        id_copy = GetUserImage.OrganizationPhoto + Path.Combine($"{user.id}.png"),
                    };
                    await db.UserIdentifications.AddAsync(orgdata);
                    await db.SaveChangesAsync();
                    return Ok(new { message = "successfully submitted" ,imageurl= GetUserImage.OrganizationPhoto + Path.Combine($"{user.id}.png") });
                }
                catch (Exception e)
                {

                    return Ok(new { message = $"Error occured, {e.Message} could help resolve it" });
                }
                
              //  orgdata.ex
            }
             return BadRequest(new { message = "Error occured or submission is invalid" });
        }
        ///Sly just added this 6th of May 2019
        [HttpPost]
        [Route("UpdateBankPreference")]        
        public async Task<IActionResult> UpdateBankPreference([FromBody]BankAccountPayment bankAccountPayment,string userId)
        {
            if(bankAccountPayment != null)
            {
                try
                {
                    int user_id = int.Parse(userId);
                    var user = db.User.Find(user_id);
                    var orgdata = db.User.FirstOrDefault(o => o.id == user_id);
                    var bank = db.Banks.FirstOrDefault(b => b.bank_name_en == bankAccountPayment.BankName || b.bank_name_ar == bankAccountPayment.BankName);
                    orgdata.bank_id = bank.bank_id;
                    orgdata.receive_cash_payment = bankAccountPayment.receive_cash_payment;
                    orgdata.receive_online_payment = bankAccountPayment.receive_online_payment;
                    orgdata.recieve_money_transfer = bankAccountPayment.recieve_money_transfer;
                    orgdata.IBAN_number = bankAccountPayment.IBankNumber;
                    await db.SaveChangesAsync();
                    return Ok(new { message = "successfully updated" });
                }
                catch (Exception e)
                {

                    return Ok(new { message = $"Error occured, {e.Message} could help resolve it" });
                }

            }
            return BadRequest(new { message = "empty submission or invalid request" });
        }
        [HttpGet]
        [Route("GetBankPreference")]
        public IActionResult GetBankPreference()
        {
            try
            {
                //GetUserId();
                int userid = GetUserId();
                if (userid > 0)
                {
                    var reguser = db.User.Find(userid);
                    var orgdata = db.User.Select(c=> new { c.id,receive_cash_payment=c.receive_cash_payment, receive_online_payment =c.receive_online_payment, recieve_money_transfer=c.recieve_money_transfer, bank_name_en=c.Banks.bank_name_en, IBankNumber=c.IBAN_number }).FirstOrDefault(o => o.id == userid);
                    var getbankpreference = orgdata;
                    return Ok(new { message = "successful", bankpreference = getbankpreference });
                }

                return Ok(new { message = "bad Request!!!", bankpreference = "" });
            }
            catch (Exception e)
            {

                return Ok(new { message = "bad Request!!!", bankpreference = $"{e.Message}" });
            }
        }
        private void SaveImageToPath(string Photo,User user,string foldername)
        {
            if (user != null)
            {
                var path = Path.Combine(foldername);
                var directory = new DirectoryInfo(path);
                if (directory.Exists == false)
                    directory.Create();
                else
                {
                    foreach (FileInfo file in directory.GetFiles())
                    {
                        if (file.Name == user.id + ".png")
                            file.Delete();
                    }
                }
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(Photo)))
                {
                    using (Bitmap bm2 = new Bitmap(ms))
                    {
                        bm2.Save($"{foldername}/" + user.id + ".png");
                    }
                }

            }
        }
     
        //******************************
        //[HttpPost]
        //[Route("api/DocumentUpload/MediaUpload")]
        //public async Task<HttpResponseMessage> MediaUpload()
        //{
        //    // Check if the request contains multipart/form-data.  
        //    if (!Request.Content.IsMimeMultipartContent())
        //    {
        //        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        //    }

        //    var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
        //    //access form data  
        //    NameValueCollection formData = provider.FormData;
        //    //access files  
        //    IList<HttpContent> files = provider.Files;

        //    HttpContent file1 = files[0];
        //    var thisFileName = file1.Headers.ContentDisposition.FileName.Trim('\"');

        //    ////-------------------------------------For testing----------------------------------  
        //    //to append any text in filename.  
        //    //var thisFileName = file1.Headers.ContentDisposition.FileName.Trim('\"') + DateTime.Now.ToString("yyyyMMddHHmmssfff"); //ToDo: Uncomment this after UAT as per Jeeevan  

        //    //List<string> tempFileName = thisFileName.Split('.').ToList();  
        //    //int counter = 0;  
        //    //foreach (var f in tempFileName)  
        //    //{  
        //    //    if (counter == 0)  
        //    //        thisFileName = f;  

        //    //    if (counter > 0)  
        //    //    {  
        //    //        thisFileName = thisFileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + f;  
        //    //    }  
        //    //    counter++;  
        //    //}  

        //    ////-------------------------------------For testing----------------------------------  

        //    string filename = String.Empty;
        //    Stream input = await file1.ReadAsStreamAsync();
        //    string directoryName = String.Empty;
        //    string URL = String.Empty;
        //    string tempDocUrl = WebConfigurationManager.AppSettings["DocsUrl"];

        //    if (formData["ClientDocs"] == "ClientDocs")
        //    {
        //        var path = HttpRuntime.AppDomainAppPath;
        //        directoryName = System.IO.Path.Combine(path, "ClientDocument");
        //        filename = System.IO.Path.Combine(directoryName, thisFileName);

        //        //Deletion exists file  
        //        if (File.Exists(filename))
        //        {
        //            File.Delete(filename);
        //        }

        //        string DocsPath = tempDocUrl + "/" + "ClientDocument" + "/";
        //        URL = DocsPath + thisFileName;

        //    }


        //    //Directory.CreateDirectory(@directoryName);  
        //    using (Stream file = File.OpenWrite(filename))
        //    {
        //        input.CopyTo(file);
        //        //close file  
        //        file.Close();
        //    }

        //    var response = Request.CreateResponse(HttpStatusCode.OK);
        //    response.Headers.Add("DocsUrl", URL);
        //    return response;
        //}




    }


}

