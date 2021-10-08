using DrTech.Amal.Common.Extentions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using static DrTech.Amal.Common.Extentions.Constants;

namespace DrTech.Amal.Common.Helpers
{
    public class EmailHelper
    {
        public static void SendEmail(string subject, string body, string to)
        {
            string SendEmail = System.Configuration.ConfigurationManager.AppSettings[AppSettings.EMAIL_SECTION_SENT].ToString();
            if (SendEmail != "true")
            {
                return;
            }

            string host = System.Configuration.ConfigurationManager.AppSettings[AppSettings.EMAIL_SECTION_DOMAIN].ToString();
            string port = System.Configuration.ConfigurationManager.AppSettings[AppSettings.EMAIL_SECTION_PORT].ToString();
            string name = System.Configuration.ConfigurationManager.AppSettings[AppSettings.EMAIL_SECTION_FROM_NAME].ToString();
            string password = System.Configuration.ConfigurationManager.AppSettings[AppSettings.EMAIL_SECTION_PASSWORD].ToString();
            string from = System.Configuration.ConfigurationManager.AppSettings[AppSettings.EMAIL_SECTION_FROM_EMAIL].ToString();

            //  string cc = AppSettingsHelper.GetAttributeValue(Constants.AppSettings.EMAIL_SECTION, Constants.AppSettings.EMAIL_SECTION_CC_EMAIL);
            //string CollectorEmail = string.Empty;
            //if (to == "email")
            //    CollectorEmail = AppSettingsHelper.GetAttributeValue(Constants.AppSettings.EMAIL_SECTION, Constants.AppSettings.EMAIL_SECTION_TO_EMAIL);
            //else
            //    CollectorEmail = to;

            MailMessage msg = new MailMessage();

            //string[] lstTo = to.Split(";");



            msg.To.Add(new MailAddress(to));
           // msg.CC.Add(new MailAddress("mehreen@amalforlife.com"));
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

        public static void SendEmailByTemplate(string subject, string to,string QRPath)
        {
            string path = HttpContext.Current.Server.MapPath("~/Template/email1.html");

            string OriginalPath = QRPath;


            string body = System.IO.File.ReadAllText(path);
           // string body;
            //using (WebClient clients = new WebClient())
            //{
            //    body = clients.DownloadString("MYHTML/My-Waste.html");
            //}
            //string FilePath = "MYHTML\\My-Waste.html";
            //StreamReader str = new StreamReader(FilePath);
            //string MailText = str.ReadToEnd();
            //str.Close();

            string SendEmail = System.Configuration.ConfigurationManager.AppSettings[AppSettings.EMAIL_SECTION_SENT].ToString();
            if (SendEmail != "true")
            {
                return;
            }

            string host = System.Configuration.ConfigurationManager.AppSettings[AppSettings.EMAIL_SECTION_DOMAIN].ToString();
            string port = System.Configuration.ConfigurationManager.AppSettings[AppSettings.EMAIL_SECTION_PORT].ToString();
            string name = System.Configuration.ConfigurationManager.AppSettings[AppSettings.EMAIL_SECTION_FROM_NAME].ToString();
            string password = System.Configuration.ConfigurationManager.AppSettings[AppSettings.EMAIL_SECTION_PASSWORD].ToString();
            string from = System.Configuration.ConfigurationManager.AppSettings[AppSettings.EMAIL_SECTION_FROM_EMAIL].ToString();

            //  string cc = AppSettingsHelper.GetAttributeValue(Constants.AppSettings.EMAIL_SECTION, Constants.AppSettings.EMAIL_SECTION_CC_EMAIL);
            //string CollectorEmail = string.Empty;
            //if (to == "email")
            //    CollectorEmail = AppSettingsHelper.GetAttributeValue(Constants.AppSettings.EMAIL_SECTION, Constants.AppSettings.EMAIL_SECTION_TO_EMAIL);
            //else
            //    CollectorEmail = to;

            MailMessage msg = new MailMessage();

            //string[] lstTo = to.Split(";");



            
            body = body.Replace("NameGPN", subject);
            msg.To.Add(new MailAddress(to));
            // msg.CC.Add(new MailAddress("mehreen@amalforlife.com"));
            msg.From = new MailAddress(from, name);
            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = true;
           // msg.Attachments.Add(new Attachment(QRCode));
            if (msg.IsBodyHtml)
            {

              
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(msg.Body, Encoding.UTF8, "text/html");

                QRPath = QRPath.Replace("~", "");
                QRPath=QRPath.Replace("/", "\\");
                string QRCODE = $"{AppDomain.CurrentDomain.BaseDirectory}"+ QRPath + "";
                LinkedResource QRCODE1 = new LinkedResource(QRCODE)
                {
                    ContentId = "QRCODE"
                };
              
                string top_image = $"{AppDomain.CurrentDomain.BaseDirectory}\\Template\\slices\\top_image.png";
                LinkedResource siteheader = new LinkedResource(top_image)
                {
                    ContentId = "top_image"
                };
                string login_image = $"{AppDomain.CurrentDomain.BaseDirectory}\\Template\\slices\\login_image.png";
                LinkedResource sitesircle1 = new LinkedResource(login_image)
                {
                    ContentId = "login_image"
                };
                string add_employee_image = $"{AppDomain.CurrentDomain.BaseDirectory}\\Template\\slices\\add_employee_image.png";
                LinkedResource sircle3Logo = new LinkedResource(add_employee_image)
                {
                    ContentId = "add_employee_image"
                };
                string leadbyexample_image = $"{AppDomain.CurrentDomain.BaseDirectory}\\Template\\slices\\leadbyexample_image.png";
                LinkedResource circle2Logo = new LinkedResource(leadbyexample_image)
                {
                    ContentId = "leadbyexample_image"
                };
                string encourage_image = $"{AppDomain.CurrentDomain.BaseDirectory}\\Template\\slices\\encourage_image.png";
                LinkedResource adminLogo = new LinkedResource(encourage_image)
                {
                    ContentId = "encourage_image"
                };
                string competition_image = $"{AppDomain.CurrentDomain.BaseDirectory}\\Template\\slices\\competition_image.png";
                LinkedResource hrlineLogo = new LinkedResource(competition_image)
                {
                    ContentId = "competition_image"

                };
                string synergy_image = $"{AppDomain.CurrentDomain.BaseDirectory}\\Template\\slices\\synergy_image.png";
                LinkedResource bottomLogo = new LinkedResource(synergy_image)
                {
                    ContentId = "synergy_image"
                };
                string quote_image = $"{AppDomain.CurrentDomain.BaseDirectory}\\Template\\slices\\quote_image.png";
                LinkedResource addressLogo = new LinkedResource(quote_image)
                {
                    ContentId = "quote_image"
                };
                string Group5 = $"{AppDomain.CurrentDomain.BaseDirectory}\\Template\\slices\\Group 5.png";
                LinkedResource cellLogo = new LinkedResource(Group5)
                {
                    ContentId = "Group5"
                };
                string fb = $"{AppDomain.CurrentDomain.BaseDirectory}\\Template\\slices\\fb.png";
                LinkedResource emailLogo = new LinkedResource(fb)
                {
                    ContentId = "fb"
                };
                string amal_top_logo = $"{AppDomain.CurrentDomain.BaseDirectory}\\Template\\slices\\amal_top_logo.png";
                LinkedResource logo1 = new LinkedResource(amal_top_logo)
                {
                    ContentId = "amal_top_logo"
                };

                string inss = $"{AppDomain.CurrentDomain.BaseDirectory}\\Template\\slices\\in.png";
                LinkedResource ins = new LinkedResource(inss)
                {
                    ContentId = "amal_top_logo"
                };
                string amal_small = $"{AppDomain.CurrentDomain.BaseDirectory}\\Template\\slices\\amal_small.png";
                LinkedResource amal_smalls = new LinkedResource(amal_small)
                {
                    ContentId = "amal_small"
                };

                string login = $"{AppDomain.CurrentDomain.BaseDirectory}\\Template\\slices\\login.jpg";
                LinkedResource loginPro = new LinkedResource(login)
                {
                    ContentId = "loginPro"
                };

                htmlView.LinkedResources.Add(adminLogo);
                htmlView.LinkedResources.Add(hrlineLogo);
                htmlView.LinkedResources.Add(bottomLogo);
                htmlView.LinkedResources.Add(addressLogo);
                htmlView.LinkedResources.Add(cellLogo);
                htmlView.LinkedResources.Add(siteheader);
                htmlView.LinkedResources.Add(sitesircle1);
                htmlView.LinkedResources.Add(sircle3Logo);
                htmlView.LinkedResources.Add(circle2Logo);
                htmlView.LinkedResources.Add(emailLogo);
                htmlView.LinkedResources.Add(logo1); 
               htmlView.LinkedResources.Add(QRCODE1);
                htmlView.LinkedResources.Add(ins);
                htmlView.LinkedResources.Add(amal_smalls);
                msg.AlternateViews.Add(htmlView);
            }

            SmtpClient client = new SmtpClient
            {
               
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(from, password),
                Port = Convert.ToInt32(port), // You can use Port 25 if 587 is blocked (mine is!)
                Host =host,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,

            };
            try
            {
                client.Send(msg);
                //var filePath = HttpContext.Current.Server.MapPath(OriginalPath) ;
                //if (File.Exists(filePath))
                //{
                //    File.Delete(filePath);
                //}
                //string fileSavePath = OriginalPath; //get path
                //FileInfo info = new FileInfo(fileSavePath);//get info file
                //                                           //the problem ocurred because this, 
                //FileStream s = new FileStream(fileSavePath, FileMode.Open); //openning stream, them file in use by a process
                //System.IO.File.Delete(fileSavePath); //Generete a error
                //                                     //problem solved here...
                //s.Close();
                //s.Dispose();
                //System.IO.File.Delete(fileSavePath);

            }
            catch (Exception ex)
            {
            }
        }
    }
}
