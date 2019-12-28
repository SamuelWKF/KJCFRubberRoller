using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Services.Description;

namespace KJCFRubberRoller.Models
{
    public class SendMail
    {
        public static void sendMail(string to, string subject, string body)
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(ConfigurationManager.AppSettings.Get("smtpEmail"));
            msg.IsBodyHtml = true;
            msg.To.Add(to);
            msg.Subject = subject;
            msg.Body = body;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32(587));
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings.Get("smtpEmail"), ConfigurationManager.AppSettings.Get("smtpPass"));
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = true;
            smtpClient.Send(msg);
        }
    }
}