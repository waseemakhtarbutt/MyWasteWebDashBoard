using DrTech.Common.Helpers;
using DrTech.DAL;
using DrTech.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using static DrTech.Common.Extentions.Constants;

namespace DrTech.Notifications.Sender
{
    public class Sender
    {
        static void Main(string[] args)
        {
           SendEmails();
        }
        public static void SendEmails()
        {
            //Console.WriteLine("--- SENDING EMAILS ---");
            //MongoDAL NotificationDAL = new MongoDAL();
            //List<FilterHelper> filter = new List<FilterHelper>
            //    {
            //        new FilterHelper
            //        {
            //            Field = "Status",
            //            Value = "0"
            //        }
            //    };

            //List<EmailNotification> lstEmail = NotificationDAL.GetModelData<EmailNotification>(filter, CollectionNames.EmailNotification);
            //Console.WriteLine("Pending Emails Found: " + lstEmail.Count);
            //foreach (EmailNotification mdlEmail in lstEmail)
            //{
            //    try
            //    {
            //        EmailHelper.SendEmail(mdlEmail.EmailSubject, mdlEmail.EmailBody, mdlEmail.EmailTo);
            //        mdlEmail.Status = 1;
            //        var update = Builders<EmailNotification>.Update
            //                      .Set(o => o.Status, mdlEmail.Status);
            //        string Id = Convert.ToString(mdlEmail.Id);
            //        bool Status = NotificationDAL.UpdateStatus(Id, update, CollectionNames.EmailNotification);
            //        Console.WriteLine("------ Email Sent ------");
            //    }
            //    catch (Exception exp)
            //    {
            //        Console.WriteLine("ERROR: Sending Email Failed To : " + mdlEmail.EmailTo);
            //    }
            //}

        }


    }
}
