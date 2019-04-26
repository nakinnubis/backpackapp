using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Api.Models;
using Core.Web.Models;
using System.IO;
using Core.Api.Helper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class BookingController : Controller
    {
        AppDbContext db = new AppDbContext();

        [HttpPost]
        [Route("BookingPaid")]
        [Authorize]
        public IActionResult BookingPaid([FromBody]BookingPaid bookingPaid)
        {
            var booking = db.Booking.Find(bookingPaid.Booking_id);
            if (booking == null)
                return Ok(new { status = 0, message = "Check data" });

            if (booking.is_paid == true)
                return Ok(new { message = "Booking is already Paid" });
            else
            {
                booking.is_paid = true;
                booking.payment_method = bookingPaid.payment_method;
                db.SaveChanges();
                return Ok();
            }
        }
       
        [Route("Chat")]
        [Authorize]
        public IActionResult Chat(int ticketid)
        {
          int userid = int.Parse(HttpContext.User.FindFirst("userId").Value);
            //var messages = db.TicketMessages.Where(s => s.ticketId == ticketid & (s.toUser == userid | s.fromUser == userid))
            //             .Select(x => new { x.Message, x.date,x.toUser,x.fromUser }).OrderBy(x => x.date);
            var messages = db.TicketMessages.Where(s => s.ticketId == ticketid & (s.toUser == userid | s.fromUser == userid))
                         .Select(x => new { x.Message, x.date, x.toUser, x.fromUser }).OrderBy(x => x.date);
            return Ok(messages);
        }



        //[HttpPost]
        //[Route("sendMessage")]
        //
        //public IActionResult sendMessage([FromBody]MessageModel messageModel, int ticketid)
        //{
        //    int userid = int.Parse(HttpContext.User.FindFirst("userId").Value); //fromUser
        //    var mail = db.Booking_Ticket.Where(s => s.ticket_id == ticketid).Select(x=>x.mail).First();
        //    TicketMessage message = new TicketMessage()
        //    {
        //         Message=messageModel.message,
        //         fromUser=userid,
        //         date=DateTime.Now,
        //         ticketId=ticketid,
        //         toUser=db.User.Where(s=>s.mail==mail).Select(s=>s.id)==null?0: db.User.Where(s => s.mail == mail).Select(s => s.id).First()
        //    };
        //    db.TicketMessages.Add(message);
        //    db.SaveChanges();
        //    return Ok(new {status=1, message = "Message send successfully" });
        //}

        //[HttpPost]
        //[Route("sendMessageForAll")]
        //
        //public IActionResult sendMessageForAll([FromBody]MessageModel messageModel, int availabilityId)
        //{
        //    var tickets = db.Booking_Ticket.Include(x => x.Booking).Where(x => x.Booking.avaliability_id == availabilityId)
        //                        .Select(x => new { x.ticket_id, x.mail }).ToList();
        //    int userid = int.Parse(HttpContext.User.FindFirst("userId").Value); //fromUser

        //    foreach (var item in tickets)
        //    {
        //        var user = db.User.Where(s => s.mail == item.mail & s.tempUser == true);
        //        if (user.Count()==0) { }
        //        else
        //        {
        //            TicketMessage message = new TicketMessage()
        //            {
        //                Message = messageModel.message,
        //                fromUser = userid,
        //                date = DateTime.Now,
        //                ticketId = item.ticket_id,
        //                toUser = user.Select(s=>s.id).First()
        //            };
        //            db.TicketMessages.Add(message);
        //        }
        //    }
        //    db.SaveChanges();
        //    return Ok(new { status = 1, message = "Message send successfully" });
        //}

        [Route("User_Verified_Ticket")]
        [Authorize]
        public IActionResult User_Verified_Ticket(int ticketid)
        {
            var Ticket = db.Booking_Ticket.Find(ticketid);
            if (Ticket == null)
                return NotFound();

            if (Ticket.user_verified == true)
                return Ok(new { message = "Ticket is already verified" });
            else
            {
                Ticket.user_verified = true;
                db.SaveChanges();
                return Ok(new { Ticket_id = Ticket.ticket_id });
            }
        }

        [Route("Checkin_Ticket")]
        [Authorize]
        public IActionResult Checkin_Ticket(int ticketid)
        {
            var Ticket = db.Booking_Ticket.Find(ticketid);
            if (Ticket == null)
                return NotFound();

            if (Ticket.ticket_checked_in == true)
                return Ok(new { message = "Ticket is already checked" });
            else
            {
                Ticket.ticket_checked_in = true;
                db.SaveChanges();
                return Ok(new { Ticket_id = Ticket.ticket_id });
            }
        }

        [Route("Cancel_Ticket")]
        [Authorize]
        public IActionResult Cancel_Ticket(int ticketid)
        {
            var Ticket = db.Booking_Ticket.Find(ticketid);
            if (Ticket == null)
                return NotFound();

            if (Ticket.ticket_cancelled == true)
                return Ok(new {status=0, message = "Ticket is already cancelled" });

            DateTime? avalibailityDate = db.Booking.Where(x => x.id == Ticket.booking_id).Select(x => x.Avaliability.activity_Start).First();
            if (avalibailityDate < DateTime.Now)
                return Ok(new { status = 0, message = "This ticket can't be cancelled , availablitiy time is over" });
            else
            {
                Ticket.ticket_cancelled = true;
                //descrease numbers of tickets of this availabilty
                var avall = db.Booking_Ticket.Include(x => x.Booking).Where(x => x.ticket_id == ticketid)
                                                .Select(x => new { x.Booking.Avaliability, x.booking_id ,x.isGroupTicket,x.numOfGroup}).First();
                if (avall.isGroupTicket != false)
                {
                    avall.Avaliability.total_tickets = avall.Avaliability.total_tickets - avall.numOfGroup;
                }
                else
                {
                    avall.Avaliability.total_tickets--;
                }

                Booking booking = db.Booking.Where(s => s.id == avall.booking_id).FirstOrDefault();
                booking.booking_amount = booking.booking_amount - Ticket.ticket_price;

                // subtract Booking_individual_category_capacity count
                var bookingIndividualCategory = db.Booking_individual_category_capacity.Where(x => x.booking_id == booking.id & x.category_id == Ticket.category_id).FirstOrDefault();

                if (bookingIndividualCategory != null)
                    bookingIndividualCategory.count--;
                
                db.SaveChanges();
                return Ok(new { Ticket_id = Ticket.ticket_id });
            }
        }

        [HttpPost]
        [Route("CreateBooking")]
        [Authorize]
        public IActionResult CreateBooking([FromBody] BookingModel bookingModel)
        {
            if (bookingModel != null)
            {
                if (bookingModel.bookingTicket == null)
                {
                    return Ok(new { status = 0, message = "No Tickets in this Reservation" });
                }
                //Total Capacity
                var activityCapacity = db.Activity.Where(x => x.id == bookingModel.activity_id).Select(x => x.totalCapacity).First();

                //Remaining Tickets
                var avalibilty = db.Avaliability.Where(x => x.id == bookingModel.avaliability_id).First();

                //ticket_require in this reservation
                // var ticket_require = bookingModel.bookingIndividualCategoryCapacity.Select(x => x.count).Sum();
                int ticket_require = bookingModel.bookingTicket.Count();

                //if Group Ticket
                if (bookingModel.bookingTicket.Select(a => a.isGroupTicket).First() == true)
                    ticket_require = bookingModel.bookingTicket.Select(x => x.numOfGroup).First();

                if (activityCapacity >= avalibilty.total_tickets + ticket_require)
                {
                    int userid = int.Parse(HttpContext.User.FindFirst("userId").Value);
                    avalibilty.total_tickets = avalibilty.total_tickets + ticket_require;   //increase numbers of reservation tickets of this availabilty

                    Booking booking = new Booking
                    {
                        user_id = userid,
                        full_group = bookingModel.full_group,
                        is_paid = false,
                        payment_method = 4,
                        booking_type = bookingModel.booking_type,
                        booking_amount = bookingModel.booking_amount,
                        activity_id = bookingModel.activity_id,
                        avaliability_id = bookingModel.avaliability_id,
                        bookingDate=DateTime.Now
                    };

                    db.Booking.Add(booking);
                    db.SaveChanges();

                    if (bookingModel.bookingIndividualCategoryCapacity != null)
                    { //Eman
                        foreach (var category_capacity in bookingModel.bookingIndividualCategoryCapacity)
                        {
                            category_capacity.booking_id = booking.id;

                            db.Booking_individual_category_capacity.Add(category_capacity);
                        }
                        db.SaveChanges();
                    }

                    foreach (var bookingticket in bookingModel.bookingTicket)
                    {
                        Int64? max = db.Booking_Ticket.Select(x => x.ticket_number).Max();
                        User user = db.User.Where(x => x.mail == bookingticket.mail).FirstOrDefault();
                        Booking_Ticket booking_Ticket;
                  
                        if (user == null)
                        {
                             user = new User
                            {
                                user_name = bookingticket.name,
                                mail = bookingticket.mail,
                                mobile = bookingticket.mobile,
                                password = "123456",
                                tempUser = true
                            };
                            db.User.Add(user);
                        }

                        booking_Ticket = new Booking_Ticket
                        {
                            booking_id = booking.id,
                            mail = bookingticket.mail,
                            name = bookingticket.name,
                            mobile = bookingticket.mobile,
                            primaryTicket = bookingticket.primaryTicket,
                            ticket_number = max + 1,
                            ticket_reviewd = bookingticket.ticket_reviewd,
                            ticket_checked_in = bookingticket.ticket_checked_in,
                            ticket_cancelled = bookingticket.ticket_cancelled,
                            isGroupTicket = bookingticket.isGroupTicket,
                            nameOfGroup = bookingticket.nameOfGroup,
                            numOfGroup = bookingticket.numOfGroup,
                            userId=user.id
                        };

                        if (bookingticket.category_id != null)
                        {
                            booking_Ticket.ticket_price = db.Individual_Categories.Where(q => q.id == bookingticket.category_id).Select(q => q.price).First();
                            booking_Ticket.category_id = bookingticket.category_id;
                        }
                        else {
                            booking_Ticket.ticket_price = booking.booking_amount;
                        }
                        db.Booking_Ticket.Add(booking_Ticket);
                        db.SaveChanges();


                        decimal? price = 0;
                        foreach (var ticket_addon in bookingticket.Booking_Ticket_AddonsModel)
                        {
                            Booking_Ticket_Addon booking_Ticket_AddonsModel = new Booking_Ticket_Addon()
                            {
                                addon_id = ticket_addon.Addons_id,
                                ticketId = booking_Ticket.ticket_id,
                                addonCount = ticket_addon.addonCount
                            };
                            db.Booking_Ticket_Addon.Add(booking_Ticket_AddonsModel);
                            db.SaveChanges();
                            if (ticket_addon.addonCount == 0)
                            {
                                price = price + db.Activity_Add_Ons.Where(x => x.activity_id == bookingModel.activity_id & x.id == ticket_addon.Addons_id)
                               .Select(x => x.price).FirstOrDefault()==null?0: db.Activity_Add_Ons.Where(x => x.activity_id == bookingModel.activity_id & x.id == ticket_addon.Addons_id)
                               .Select(x => x.price).First(); 
                            }                   
                        }

                        booking_Ticket.ticket_price = booking_Ticket.ticket_price + price;
                        db.SaveChanges();
                    }
              
                    return Ok(new { Bookingid = booking.id });
                }
                return Ok(new {status=0, message = "No Tickets" });
            }
            return BadRequest(new { message = "Check Data" });
        }
        #region 
        //[Route("GetBookingDetailsByTicketNo")]
        //
        //public IActionResult GetBookingDetailsByTicketNo(int ticketNo)
        //{
        //    var bookingid = db.Booking_Ticket.Where(x => x.ticket_number == ticketNo).Select(x => x.booking_id).First();
        //    if (bookingid == null)
        //        return NotFound();

        //    var Bookingdetails = (from a in db.Booking.Include(x => x.Avaliability).Include(x => x.Booking_individual_category_capacity).Include(x => x.Booking_Tickets)
        //                                    .ThenInclude(w => w.Booking_Ticket_Addons).ThenInclude(e => e.Activity_Add_Ons)
        //                          where a.id == bookingid
        //                          select new BookingDetails
        //                          {
        //                              Booking_id = bookingid,
        //                              activity_id = a.activity_id,
        //                              is_paid = a.is_paid,
        //                              payment_method = a.PaymentMethods.name,
        //                              activity_End = a.Avaliability.activity_End,
        //                              activity_Start = a.Avaliability.activity_Start,
        //                              full_group = a.full_group,
        //                              booking_amount = a.booking_amount,
        //                              booking_type = a.booking_type,

        //                              categoryModel = a.Booking_individual_category_capacity.Select(w => new CategoryModel
        //                              {
        //                                  name = w.IndividualCategory.name,
        //                                  price = w.IndividualCategory.price,
        //                                  count = w.count,
        //                                  category_id = w.category_id
        //                              }),
        //                              bookingTicketDetails = a.Booking_Ticket.Select(s => new BookingTicketDetails
        //                              {
        //                                  Ticket_Id = s.ticket_id,
        //                                  name = s.name,
        //                                  mail = s.mobile,
        //                                  primaryTicket = s.primaryTicket,
        //                                  ticket_cancelled = s.ticket_cancelled,
        //                                  ticket_checked_in = s.ticket_checked_in,
        //                                  mobile = s.mobile,
        //                                  ticket_number = s.ticket_number,
        //                                  ticket_reviewd = s.ticket_reviewd,
        //                                  user_verified = s.user_verified,
        //                                  booking_Ticket_AddonsDetails = s.Booking.Booking_Ticket.Select(d =>
        //                                  new Booking_Ticket_AddonsDetails
        //                                  {
        //                                      ticket_Id = d.ticket_id,
        //                                      Addon_id = d.Booking_Ticket_Addons.addon_id,
        //                                      name = d.Booking_Ticket_Addons.Activity_Add_Ons.Add_Ons.name,
        //                                      price = d.Booking_Ticket_Addons.Activity_Add_Ons.price,
        //                                      note = d.Booking_Ticket_Addons.Activity_Add_Ons.note,
        //                                      provider_Username = d.Booking_Ticket_Addons.Activity_Add_Ons.provider_Username,
        //                                      addons_number = d.Booking_Ticket_Addons.Activity_Add_Ons.addons_number,
        //                                      icon = d.Booking_Ticket_Addons.Activity_Add_Ons.Add_Ons.icon
        //                                  }).Where(d => d.ticket_Id == s.ticket_id).ToList()
        //                              }).ToList()

        //                          });
        //    return Ok(Bookingdetails);

        //}

        #endregion

        [Route("GetReservations")]
        [Authorize]
        public IActionResult GetReservations(int activityid, DateTime? date)
        {
            bool currentUser;
            int? activityUser = db.Activity.Where(a=>a.id==activityid).Select(a=>a.user_id).First();
            int? userid = int.Parse(HttpContext.User.FindFirst("userId").Value);
            if(activityUser == userid)
            {
                currentUser = true;    //Activity_Creator
            }
            else
            {    //Organizers
                var currentMail = db.User.Where(x => x.id == userid).Select(x => x.mail);
                var c = db.Activity_Organizer.Where(d => d.activity_id == activityid).Select(d=>d.mail ).ToList();
                currentUser = (c.Contains(currentMail.ToString()))?  true :  false;
                                                                  
            }


            var reservations = from a in db.Booking.Include(x => x.Avaliability).Include(x => x.Booking_Tickets)
                                 .Include(x => x.User).Include(x => x.PaymentMethods)
                                 .Where(x => x.activity_id == activityid)
                               select new Reservation
                               {
                                   Booking_id = a.id,
                                   activity_id = a.activity_id,
                                   is_paid = a.is_paid,
                                   user_name = a.User.user_name,
                                   remainingTickets=(a.Activity.totalCapacity!=null&a.Avaliability.total_tickets!=null) ?a.Activity.totalCapacity-a.Avaliability.total_tickets:0,
                                   activity_End = a.Avaliability.activity_End,
                                   activity_Start = a.Avaliability.activity_Start,
                                   payment_method = a.PaymentMethods.name,
                                   full_group = a.full_group,
                                   booking_amount = a.booking_amount,
                                   booking_type = a.booking_type,
                                   reservationTicketDetails = a.Booking_Tickets.Select(s => new ReservationTicketDetails
                                   {
                                       Ticket_Id = s.ticket_id,
                                       CustomerId = s.userId,
                                       UserPhoto_Url = Path.Combine((s.user.UserPhoto_Url == null ? "NA" : GetUserImage.OnlineImagePathForUserPhoto + s.user.UserPhoto_Url)),
                                       ChatId =db.MessageReplies.Include(x => x.Chat).Where(x => x.regardingId == s.ticket_id & 
                                                 (x.Chat.firstUser == s.userId | x.Chat.secondUser == s.userId)).Select(x => x.Chat.chatId).FirstOrDefault(),
                                       MessageIcon = (db.User.Where(l=>l.mail==s.mail).Select(l=>l.tempUser).First()&currentUser)?true:false,
                                       name = s.name,
                                       mail = s.mail,
                                       primaryTicket = s.primaryTicket,
                                       ticket_cancelled = s.ticket_cancelled,
                                       ticket_checked_in = s.ticket_checked_in,
                                       mobile = s.mobile,
                                       ticket_number = s.ticket_number,
                                       ticket_reviewd = s.ticket_reviewd,
                                       user_verified = s.user_verified,
                                       isGroupTicket = s.isGroupTicket,
                                       nameOfGroup = s.nameOfGroup,
                                       numOfGroup = s.numOfGroup
                                   }).Where(s => s.ticket_cancelled == false).ToList()
                               };
            if (date != null)
                reservations = reservations.Where(x => x.activity_Start == date);
            if (reservations != null)
                return Ok(Json(reservations).Value);
            else
                return NoContent();
        }

        [HttpGet]
        [Route("GetBookingDetails")]
    //    [Authorize]
        public IActionResult GetBookingDetails(int Bookingid, long ticketNo)
        {
            int? bookingid;
            if (ticketNo != 0)
            {
                bookingid = db.Booking_Ticket.Where(x => x.ticket_number == ticketNo).Select(x => x.booking_id).FirstOrDefault();
                if (bookingid == null)
                    return Ok(new { message = "There is no ticket with this number", status = 0 });
            }
            else
            {
                var booking = db.Booking.Find(Bookingid);
                if (booking == null)
                    return Ok(new { message = "There is no booking , Check data", status = 0 });
                bookingid = Bookingid;
            }


            var ForGroup = db.Booking.Where(w => w.full_group == true & w.id == bookingid);
            if (ForGroup.Count() == 0)
            {
                var Bookingdetails = (from a in db.Booking.Include(x => x.Avaliability).Include(x => x.Booking_individual_category_capacity)
                                      .Include(x => x.Booking_Tickets).ThenInclude(w => w.Booking_Ticket_Addon).ThenInclude(e => e.Activity_Add_Ons)
                                      where a.id == bookingid
                                      select new BookingDetails
                                      {
                                          Booking_id = bookingid,
                                          activity_id = a.activity_id,
                                          activityName = a.Activity.title,
                                          is_paid = a.is_paid,
                                          payment_method = a.PaymentMethods.name,
                                          activity_End = a.Avaliability.activity_End,
                                          activity_Start = a.Avaliability.activity_Start,
                                          full_group = a.full_group,
                                          booking_amount = a.booking_amount,
                                          booking_type = a.booking_type,
                                          avaliabilityPricing = a.Avaliability.Avaliability_Pricings
                                                                    .Where(w => a.Booking_individual_category_capacity.Select(s => s.category_id).
                                                                    Contains(w.individualCategoryId))
                                              .Select(s => new AvaliabilityPricing
                                              {
                                                  id = s.id,
                                                  individualCategoryId = s.individualCategoryId,
                                                  price = s.price,
                                                  priceAfterDiscount = s.priceAfterDiscount
                                              }),
                                          categoryModel = a.Booking_individual_category_capacity.Select(w => new CategoryModel
                                          {
                                              name = w.IndividualCategory.name,
                                              price = w.IndividualCategory.price,
                                              count = w.count,
                                              category_id = w.category_id
                                          }),
                                          bookingTicketDetails = a.Booking_Tickets.Select(s => new BookingTicketDetails
                                          {
                                              Ticket_Id = s.ticket_id,
                                              name = s.name,
                                              mail = s.mobile,
                                              primaryTicket = s.primaryTicket,
                                              ticket_cancelled = s.ticket_cancelled,
                                              ticket_checked_in = s.ticket_checked_in,
                                              mobile = s.mobile,
                                              ticket_number = s.ticket_number,
                                              ticket_reviewd = s.ticket_reviewd,
                                              user_verified = s.user_verified,
                                              isGroupTicket = s.isGroupTicket,
                                              nameOfGroup = s.nameOfGroup,
                                              numOfGroup = s.numOfGroup,
                                              booking_Ticket_AddonsDetails = s.Booking_Ticket_Addon.Select(d =>
                                              new Booking_Ticket_AddonsDetails
                                              {
                                                  ticket_Id = d.ticketId,
                                                  Addon_id = d.addon_id,
                                                  name=d.Activity_Add_Ons.Add_Ons.name,                               
                                                  price = d.Activity_Add_Ons.price,
                                                  note = d.Activity_Add_Ons.note,
                                                  provider_Username = d.Activity_Add_Ons.provider_Username,
                                                  addons_number = d.Activity_Add_Ons.addons_number,
                                                  icon = d.Activity_Add_Ons.Add_Ons.icon,
                                              })/*.Where(d => d.ticket_Id == s.ticket_id)*/.ToList()
                                          }).ToList()

                                      });

                if (Bookingdetails != null)
                    return Ok(Bookingdetails);

                else
                    return NoContent();
            }
            else
            {
                var Bookingdetails = (from a in db.Booking.Include(x => x.Booking_individual_category_capacity).Include(x => x.Avaliability).ThenInclude(x => x.Avaliability_Pricings)
                                                .Include(x => x.Booking_Tickets).ThenInclude(w => w.Booking_Ticket_Addon)
                                                .ThenInclude(e => e.Activity_Add_Ons)
                                      where a.id == bookingid
                                      select new BookingDetailsGroup
                                      {
                                          Booking_id = bookingid,
                                          activity_id = a.activity_id,
                                          activityName = a.Activity.title,
                                          activity_group_price = a.Activity.group_price,
                                          avaliability_group_price = a.Avaliability.group_Price,
                                          is_paid = a.is_paid,
                                          payment_method = a.PaymentMethods.name,
                                          activity_End = a.Avaliability.activity_End,
                                          activity_Start = a.Avaliability.activity_Start,
                                          full_group = a.full_group,
                                          booking_amount = a.booking_amount,
                                          booking_type = a.booking_type,
                                          bookingTicketDetailsGroups = a.Booking_Tickets.Select(s => new BookingTicketDetailsGroup
                                          {
                                              Ticket_Id = s.ticket_id,
                                              name = s.name,
                                              mail = s.mobile,
                                              primaryTicket = s.primaryTicket,
                                              ticket_cancelled = s.ticket_cancelled,
                                              ticket_checked_in = s.ticket_checked_in,
                                              mobile = s.mobile,
                                              ticket_number = s.ticket_number,
                                              ticket_reviewd = s.ticket_reviewd,
                                              user_verified = s.user_verified,
                                              isGroupTicket = s.isGroupTicket,
                                              nameOfGroup = s.nameOfGroup,
                                              numOfGroup = s.numOfGroup,
                                              booking_Ticket_AddonsDetailsGroups = s.Booking_Ticket_Addon.Select(d =>
                                              new Booking_Ticket_AddonsDetailsGroup
                                              {
                                                  ticket_Id = d.ticketId,
                                                  Addon_id = d.addon_id,
                                                  name = d.Activity_Add_Ons.Add_Ons.name,
                                                  price = d.Activity_Add_Ons.price,
                                                  note = d.Activity_Add_Ons.note,
                                                  provider_Username = d.Activity_Add_Ons.provider_Username,
                                                  icon = d.Activity_Add_Ons.Add_Ons.icon,
                                                  addonCount = d.addonCount   //*********
                                              })/*.Where(d => d.ticket_Id == s.ticket_id)*/.ToList()
                                          }).ToList()

                                      });

                if (Bookingdetails != null)
                    return Ok(Bookingdetails);

                else
                    return NoContent();

            }
        }

        [Route("CalenderReservation")]
        [Authorize]
        public IActionResult CalenderReservation(int activityid)
        {
            var activity = db.Activity.Where(a => a.id == activityid);
            if (activity == null)
                return BadRequest();

            var reservations = db.Avaliability.Include(x => x.Avaliability_Pricings).Where(a => a.activity_id == activityid).Select(c => new reservation
            {
                totalCapacity = c.Activity.totalCapacity,
                // Activity_group_price = a.Activity.group_price,
                activity_Start = c.activity_Start,
                activity_End = c.activity_End,
                avaliabilityid = c.id,
                Availability_group_Price = c.group_Price,
                isForGroup = c.isForGroup,
                total_tickets = c.total_tickets,
                 BookinfInfo = c.Activity.Bookings.Where(g => g.activity_id == activityid).AsEnumerable(),
                //individualCategories=a.Activity.Individual_Categories.Select(s=> new IndividualCategoryModel {
                //    id=s.id,
                //    name=s.name,
                //    capacity=s.capacity,
                //    price=s.price,
                //    price_after_discount=s.price_after_discount,
                //    activity_Id = s.activityid
                //}),
                avaliabilityPricing = c.Avaliability_Pricings.Select(f=> new AvaliabilityPricing {
                    id =f.id,
                individualCategoryId= f.individualCategoryId,
                price= f.price,
                priceAfterDiscount= f.priceAfterDiscount
                }).AsEnumerable()

            }).ToList();
                //from a in db.Avaliability.Include(x => x.Avaliability_Pricings)
                //               where a.activity_id == activityid
                //               select ;

            return Ok(reservations);


        }

        #region g
        [HttpPut("{id}")]
        [Route("EditBooking")]
        public IActionResult EditBooking(int id, [FromBody] Booking model)
        {
            var booking = db.Booking.Find(id);
            if (booking != null)
            {
                booking.id = id;
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return Ok(new { message = "Booking edit successfully" });
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        [Route("DeleteBooking")]
        public IActionResult Delete(int id)
        {
            var booking = db.Booking.Find(id);
            if (booking == null)
            {
                return NotFound();
            }
            db.Booking.Remove(booking);
            db.SaveChanges();
            return NoContent();
        }

        [HttpGet]
        [Route("GetActivityByDate")]
        public IActionResult GetActivityByDate(DateTime date)
        {
            var activity = db.Avaliability.Include(x => x.Avaliability_Pricings).Where(f => f.activity_Start.Value.Date == date.Date).Select(x => new
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
                BookingInfo = x.Activity.Bookings.Where(d => d.bookingDate != null).AsEnumerable(),
                x.Activity.bookingAvailableForGroups,
                x.Activity.bookingAvailableForIndividuals,
                Activity_Photos = x.Activity.Activity_Photos.Select(s => new photomodel
                {
                    id = s.id,
                    url = GetUserImage.OnlineImagePathForActivity + s.url,
                    cover_photo = s.cover_photo
                }).ToList(),
            }).ToList();
            if (activity != null)
            {
                return Ok(activity);
            }
            return BadRequest(new { message = "Request returned empty data" });
        }


        #endregion
    }

}
