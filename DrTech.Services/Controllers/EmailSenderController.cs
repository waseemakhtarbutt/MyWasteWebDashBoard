using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DrTech.Common.Enums;
using DrTech.Common.Helpers;
using DrTech.Common.ServerResponse;
using DrTech.DAL;
using DrTech.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using static DrTech.Common.Extentions.Constants;

namespace DrTech.Services.Controllers
{
    // [Produces("application/json")]
    [Route("api/EmailSender")]
    public class EmailSenderController : BaseControllerBase
    {
        [HttpGet("SendEmails")]
        public async Task<ResponseObject<bool>> SendEmails()
        {
            try
            {
                List<FilterHelper> filter = new List<FilterHelper>
                {
                    new FilterHelper
                    {
                        Field = "Status",
                        Value = "0"
                    }
                };

                List<EmailNotification> lstEmailNotifications = _IUWork.GetModelData<EmailNotification>(filter, CollectionNames.EmailNotification);
                foreach (EmailNotification mdlEmail in lstEmailNotifications)
                {
                    if (mdlEmail == null) 
                        continue;

                    if (mdlEmail.EmailTo == string.Empty)
                        //{
                        //return ServiceResponse.SuccessReponse(false, "");
                        continue;
                    //}
                    if (!mdlEmail.EmailTo.Contains("@"))
                        continue;
                    
                            //else
                    
                    EmailHelper.SendEmail(mdlEmail.EmailSubject, mdlEmail.EmailBody, mdlEmail.EmailTo);
                    mdlEmail.Status = 1;
                    var update = Builders<EmailNotification>.Update
                                    .Set(o => o.Status, mdlEmail.Status);

                    string Id = Convert.ToString(mdlEmail.Id);
                    bool Status = _IUWork.UpdateStatus(Id, update, CollectionNames.EmailNotification);
                    
               }

                return ServiceResponse.SuccessReponse(true, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }


        public void SendSMS()
        {
            try
            {
                //Console.WriteLine("Hello World!");

                //System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                //mailMessage.From = new System.Net.Mail.MailAddress("admin@drtechpk.com", "DrTech Admin");
                //mailMessage.To.Add(new System.Net.Mail.MailAddress("safiullah@drtechpk.com", "Safi Ullah Bhatti"));
                //mailMessage.Subject = "Subject";
                //mailMessage.Body = "Body";

                //System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient("smtp.office365.com");
                //smtpClient.UseDefaultCredentials = false;
                //smtpClient.Credentials = new System.Net.NetworkCredential("admin@drtechpk.com", "drt3ch@dm!n");


                //smtpClient.Port = 587;

                //smtpClient.EnableSsl = true;





                //smtpClient.Send(mailMessage);

                ////smtpClient.SendCompleted += SmtpClient_SendCompleted;



                ////smtpClient.SendMailAsync(mailMessage);

                string UserName = "Amal";
                string Password = "Amal!@#123";
                string ClientID = "Amal";
                string Mask = "Amal";

                //string UserName = "iqbal";
                //string Password = "iqbal!@#123";
                //string ClientID = "iqbal";
                //string Mask = "SMS POINT";


                string URI = "http://www.smspoint.pk/api/smsapi/";


                string res = "";

                string Message = "Dear AR - Congratulation!\n<br>You have earned 500 Green Points due to your recyclable material of 50Kg.\n<br>Thanks\nAmal";
                string To = "923214416412";
                string Language = "English";

                string myParameters = string.Format("username={0}&password={1}&clientid={2}&mask={3}&msg={4}&to={5}&language={6}", UserName, Password, ClientID, Mask, Message, To, Language);

                var byteArray = Encoding.UTF8.GetBytes(myParameters);

                var client = WebRequest.Create(URI);
                client.Method = "POST";
                client.ContentType = "application/x-www-form-urlencoded";
                client.ContentLength = byteArray.Length;
                var stream = client.GetRequestStream();
                stream.Write(byteArray, 0, byteArray.Length);
                stream.Close();

                var response = client.GetResponse().GetResponseStream();
                var resp = new StreamReader(response);

                res = resp.ReadToEnd();

                if (res.ToUpper() == "SENT SUCCESSFULLY")
                {
                    // Do whatever to do on sent 
                }
                else
                {
                    // all other errors
                }


            }
            catch (Exception e)
            {

            }
        }


    }
}