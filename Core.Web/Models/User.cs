using Core.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class User
    {
      
        public User()
        {
            this.Activities = new HashSet<Activity>();
            this.Bookings = new HashSet<Booking>();
            this.Reviews = new HashSet<Reviews>();
        }
       
        public int id { get; set; }
        public int user_Type { get; set; }
        public string user_name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public bool tempUser { get; set; }

        public string mobile { get; set; }
        public string customerphoto64 { get; set; }
        public string UserPhoto_Url { get; set; }
        public string bio { get; set; }
        public string address { get; set; }
        public DateTime DOB { get; set; }
       
        public string organization_name { get; set; }
        public string organization_type { get; set; }
        public string about_organization { get; set; }
        public bool receive_cash_payment { get; set; }
        public bool receive_online_payment { get; set; }
        public bool recieve_money_transfer { get; set; }
        [ForeignKey("Banks")]
        public Nullable<int> bank_id { get; set; }
        public string IBAN_number { get; set; }
        public string mail { get; set; }
        public string password { get; set; }
        public bool isProvider { get; set; }
        public string gender { get; set; }
        public string age { get; set; }

        public string activationcode { get; set; }
        public bool isregistered { get; set; }
        public bool isverified { get; set; }
        public int preferablePrice { get; set; }
        public string email { get; set; }
        public string registrationType { get; set; }
        public virtual Banks Banks { get; set; }

        public virtual ICollection<Notifications> Notifications { get; set; }
        public virtual ICollection<UserIdentification> UserIdentifications { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
        public virtual ICollection<User_roles> User_roles { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        //public virtual ICollection<Booking> Bookings1 { get; set; }
        public virtual ICollection<Rule> Rules { get; set; }
        public virtual ICollection<Add_Ons> Add_Ons { get; set; }
        public virtual ICollection<Activity_Log> Activity_Logs { get; set; }
        public virtual ICollection<Reviews> Reviews { get; set; }
        public virtual ICollection<ReviewReports> ReviewReports { get; set; }
        public virtual ICollection<TicketMessage> TicketMessages { get; set; }
        public virtual ICollection<User_Diseases> User_Diseases { get; set; }
        public virtual ICollection<FollowUpHealth> FollowUpHealths { get; set; }
        public virtual ICollection<Booking_Ticket> Booking_Tickets { get; set; }
        public virtual ICollection<UserDevice> UserDevices { get; set; }

        public virtual ICollection<MessageReply>  MessageReplies { get; set; }
        //public virtual ICollection<Chat>  Chat1 { get; set; }
        //public virtual ICollection<Chat>  Chat2 { get; set; }

    }
}
