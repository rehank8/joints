using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace TeacherApplication
{
    public  class GmailHelper
    {
        public static void Send(string toMessage,string subject,string messageBody)
        {
            MailAddress from = new MailAddress("mohammedkhateeb7@gmailcom");
            MailAddress to = new MailAddress(toMessage);
            MailMessage message = new MailMessage(from, to);
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.BodyEncoding = System.Text.Encoding.ASCII;
            message.Body = messageBody;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            NetworkCredential nc = new NetworkCredential("sandeep.mvn@deccansoft.com", "sandeep.mvn@123");
            smtp.Credentials = nc;
            smtp.Send(message);
        }
    }
}