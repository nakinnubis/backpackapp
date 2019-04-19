using System;
using System.Linq;
using Core.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Api.Controllers
{

    [Produces("application/json")]
    [Route("api/Reviews")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        AppDbContext db = new AppDbContext();

        [Authorize]
        [Route("ActivityReview")]
        public IActionResult ActivityReview(int activityid, Reviews userReview)
        {
            var activity = db.Activity.Find(activityid);
            if (activity == null)
                return NotFound();

            int userid = int.Parse(HttpContext.User.FindFirst("userId").Value);


            Reviews reviews = new Reviews
            {
                activity_id = activityid,
                user_id = userid,
                review = userReview.review,
                date = DateTime.Now,
                rate = userReview.rate
            };

            db.Reviews.Add(reviews);
            db.SaveChanges();

            //To Set New Rate Value to This Activity
            var reviewsCount = db.Reviews.Where(x => x.activity_id == activityid).Count();
            decimal No = 2.00m;
            var newRate = (activity.rate + reviews.rate) / No;
            activity.rate = newRate;
            db.SaveChanges();

            return Ok(new { reviewId = reviews.reviewid });

        }

        [Authorize]
        [Route("ReportReview")]
        public IActionResult ReportReview(int reviewid)
        {
            var review = db.Reviews.Find(reviewid);
            if (review == null)
                return NotFound();

            review.isBlocked =true;
            db.SaveChanges();

            //int userid = int.Parse(HttpContext.User.FindFirst("userId").Value);

            //ReviewReports reviewReports = new ReviewReports {
            //    user_id=userid,
            //    reportDate=DateTime.Now,  
            //    reviewId=reviewid
            //};

            //db.ReviewReports.Add(reviewReports);
            // db.SaveChanges();

            return Ok(new { reviewId=review.reviewid });
        }

        [Authorize]
        [HttpPost]
        [Route("ReviewReplies")]
        public IActionResult ReviewReplies([FromBody]string reply, int reviewid)
        {
            var review = db.Reviews.Find(reviewid);
            if (review == null)
                return NotFound();

            int userid = int.Parse(HttpContext.User.FindFirst("userId").Value);

            ReviewReplies reviewReplies = new ReviewReplies
            {
                userId = userid,
                replyTime = DateTime.Now,
                review_Id= reviewid,
                reply=reply
            };

            db.ReviewReplies.Add(reviewReplies);
            db.SaveChanges();

            return Ok(new { replyId = reviewReplies.replyId });
        }




    }
}
