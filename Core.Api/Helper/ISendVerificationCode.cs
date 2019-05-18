using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Helper
{
    interface ISendVerificationCode
    {
        string SendMessage(SmsProperties smsProperties);
    }
}
