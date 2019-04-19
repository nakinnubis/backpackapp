using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;


namespace Core.Api.Models
{
   
    public class MailHelper
    {

        public bool SendMail(string email, string message, string title = "Your Password ")
        {

            var fromAddress = new MailAddress("", "From Name");
            var toAddress = new MailAddress(email, "To Name");
            const string fromPassword = "";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
        };

            using (var msg = new MailMessage(fromAddress, toAddress)
            {
                Subject = title,
                Body = message
            }) 

                try
                {
                    smtp.Send(msg);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
        }
       


    }


}
