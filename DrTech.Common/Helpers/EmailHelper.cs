using DrTech.Common.Extentions;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace DrTech.Common.Helpers
{
    public class EmailHelper
    {
        public static void SendEmail(string subject, string body, string to)
        {
            string SendEmail = AppSettingsHelper.GetAttributeValue(Constants.AppSettings.EMAIL_SECTION, Constants.AppSettings.EMAIL_SECTION_SENT);
            if (SendEmail != "true")
            {
                return;
            }
            
            string host = AppSettingsHelper.GetAttributeValue(Constants.AppSettings.EMAIL_SECTION, Constants.AppSettings.EMAIL_SECTION_DOMAIN);
            string port = AppSettingsHelper.GetAttributeValue(Constants.AppSettings.EMAIL_SECTION, Constants.AppSettings.EMAIL_SECTION_PORT);
            string name = AppSettingsHelper.GetAttributeValue(Constants.AppSettings.EMAIL_SECTION, Constants.AppSettings.EMAIL_SECTION_FROM_NAME);
            string password = AppSettingsHelper.GetAttributeValue(Constants.AppSettings.EMAIL_SECTION, Constants.AppSettings.EMAIL_SECTION_PASSWORD);
            string from = AppSettingsHelper.GetAttributeValue(Constants.AppSettings.EMAIL_SECTION, Constants.AppSettings.EMAIL_SECTION_FROM_EMAIL);
            //  string cc = AppSettingsHelper.GetAttributeValue(Constants.AppSettings.EMAIL_SECTION, Constants.AppSettings.EMAIL_SECTION_CC_EMAIL);
            //string CollectorEmail = string.Empty;
            //if (to == "email")
            //    CollectorEmail = AppSettingsHelper.GetAttributeValue(Constants.AppSettings.EMAIL_SECTION, Constants.AppSettings.EMAIL_SECTION_TO_EMAIL);
            //else
            //    CollectorEmail = to;

            MailMessage msg = new MailMessage();

            //string[] lstTo = to.Split(";");

            

            msg.To.Add(new MailAddress(to));
            msg.From = new MailAddress(from, name);
            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient
            {
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(from, password),
                Port = Convert.ToInt32(port), // You can use Port 25 if 587 is blocked (mine is!)
                Host = host,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true
            };
            try
            {
                client.Send(msg);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
