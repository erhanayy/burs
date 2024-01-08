using System;
using System.Collections.Generic;
//using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;

namespace MyAdmin.Helper
{
    public class EmailSender
    {
        private readonly string smtpHost = "smtp.gmail.com";
        private readonly int smtpPort = 587;
        private readonly string smtpUsername;
        private readonly string smtpPassword;

        public EmailSender(string test_username="", string test_password="")
        {
            smtpUsername = test_username == "" ? System.Configuration.ConfigurationManager.AppSettings["SmtpUsername"] : test_username;
            smtpPassword = test_password == "" ? System.Configuration.ConfigurationManager.AppSettings["SmtpPassword"] : test_password;
        }

        public void SendEmail(string to, string subject, string body)
        {
            using (SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort))
            {
                
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
  
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(smtpUsername);
                mailMessage.To.Add(to);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;

                try
                {
                    smtpClient.Send(mailMessage);
                }
                catch (Exception ex)
                {
                   
                    throw new ApplicationException($"E-posta gönderimi başarısız: {ex.Message}");
                }
            }
        }
    }
}