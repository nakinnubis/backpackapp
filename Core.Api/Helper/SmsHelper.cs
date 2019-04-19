using Core.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Core.Api.Helper
{
    public class SmsHelper
    {
        private string ConvertToUnicode(string val)
        {
            string msg2 = string.Empty;

            for (int i = 0; i < val.Length; i++)
            {
                msg2 += convertToUnicode(System.Convert.ToChar(val.Substring(i, 1)));
            }

            return msg2;
        }
        private string convertToUnicode(char ch)
        {
            System.Text.UnicodeEncoding class1 = new System.Text.UnicodeEncoding();
            byte[] msg = class1.GetBytes(System.Convert.ToString(ch));

            return fourDigits(msg[1] + msg[0].ToString("X"));
        }
        private string fourDigits(string val)
        {
            string result = string.Empty;

            switch (val.Length)
            {
                case 1: result = "000" + val; break;
                case 2: result = "00" + val; break;
                case 3: result = "0" + val; break;
                case 4: result = val; break;
            }

            return result;
        }
        public bool SendSms(string number, string message)
        { 
                string c = number[0].ToString();
                if (c == "0")
                {
                    number = number.Remove(0, 1);
                }
                message = ConvertToUnicode(message);

                HttpWebRequest req = (HttpWebRequest)
                WebRequest.Create("http://www.mobily.ws/api/msgSend.php");
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                string postData = "mobile=96650000000" + "&password=xxxxxx" + "&numbers=966" + number + "&sender=050xxxxxxx" + "&msg=" + message + "&applicationType=68";
                req.ContentLength = postData.Length;
                StreamWriter stOut = new
                StreamWriter(req.GetRequestStream(),
                System.Text.Encoding.ASCII);
                stOut.Write(postData);
                stOut.Close();
                // Do the request to get the response
                string strResponse;
                StreamReader stIn = new StreamReader(req.GetResponse().GetResponseStream());
                strResponse = stIn.ReadToEnd();
              //  new MailHelper().SendMail("haythamrajab@gmail.com", strResponse);
                stIn.Close();
                return true;
    
        }


    }
}
