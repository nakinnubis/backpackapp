using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Helper
{
    public class SmsProperties
    {
        public string url { get; set; }
        public string comand { get; set; }

        public MessageAuthentication Authentication {get;set;}
        public MessaageParameter Parameter { get; set; }
       
    }
    public class MessageAuthentication
    {
        public string user { get; set; }
        public string pass { get; set; }
    }
    public class MessaageParameter
    {
        public string to { get; set; }
        public string message { get; set; }
        public string sender { get; set; }
    }
}
