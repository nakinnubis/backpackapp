using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Models
{
    public class InboxChat
    {
        public int chatId { get; set; }
        public DateTime lastUpdateDate { get; set; }

        public int firstUser { get; set; }
        public string UserName { get; set; }
        public string UserPhoto_Url { get; set; }
        public int unReadableMessageNo { get; set; }
        public string activityName { get; set; }

    }

    public class MessageInbox
    {
        public int messageReplayId { get; set; }
        public string message { get; set; }
        public DateTime date { get; set; }

        public int userId { get; set; }
        public string user_name { get; set; }
        public string UserPhoto_Url { get; set; }

        public int regardingId { get; set; }
        public string typeId { get; set; }

    }

    public class Cust_MessageInbox
    {
        public int messageReplayId { get; set; }
        public string message { get; set; }
        public DateTime date { get; set; }

        public int userId { get; set; }
        public string user_name { get; set; }
        public string UserPhoto_Url { get; set; }

    }

}
