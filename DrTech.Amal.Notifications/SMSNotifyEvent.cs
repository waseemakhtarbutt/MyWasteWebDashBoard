using DrTech.Amal.Common.Helpers;
using DrTech.Amal.SQLDataAccess;
using DrTech.Amal.SQLDataAccess.CustomModels;
using DrTech.Amal.SQLModels;
//using DrTech.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using static DrTech.Amal.Common.Extentions.Constants;

namespace DrTech.Amal.Notifications
{
    public class SMSNotifyEvent : BaseNotifyEvent
    {
        ContextDB db;
        public Dictionary<string, object> Parameters { get; set; }
        public SMSNotifyEvent()
        {
            this.Parameters = new Dictionary<string, object>();
            db = new ContextDB();
        }

        public bool AddSMSNotifyEvent(long EventID, string Parameter)
        {
            try
            {
                bool isAdded = false;



                List<SMSNotificationEvent> lstTemplate = db.Repository<SMSNotificationEvent>().GetAll().Where(x => x.EventID == EventID && x.IsActive == true).ToList();

                if (lstTemplate.Count > 0)
                {
                    if (Convert.ToUInt64(lstTemplate[0].EventID) == (long)NotificationEventConstants.Users.SendSMSToUserForEntrolment)
                    {
                        string Mobile = Parameter.ToString();
                        //string SMSCode = Parameters["SMSCode"].ToString();
                        User userinfo = new User();

                        SMSNotification SMSNotification = PrepareSMSNotification<User>(lstTemplate, userinfo, Mobile);
                        SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);
                    }
                    else if (Convert.ToUInt64(lstTemplate[0].EventID) == (long)NotificationEventConstants.Users.SendVerificationCodeSMS)
                    {
                        string Phone = Parameter.ToString();
                        string SMSCode = Parameters["SMSCode"].ToString();


                        var UserDetail = db.Repository<User>().GetAll().Where(x => x.Phone == Phone).FirstOrDefault();

                        // QRCode use as SMS Code
                        UserDetail.QRCode = SMSCode;

                        SMSNotification SMSNotification = PrepareSMSNotification<User>(lstTemplate, UserDetail, Phone);
                        SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);

                    }

                    else if (Convert.ToUInt64(lstTemplate[0].EventID) == (long)NotificationEventConstants.Users.UpdateAfterSendToSMS)
                    {
                        string Phone = Parameter.ToString();
                        string SMSCode = Parameters["SMSCode"].ToString();

                        var UserDetail = db.Repository<User>().GetAll().Where(x => x.Phone == Phone).FirstOrDefault();

                        SMSNotification SMSNotification = PrepareSMSNotification<User>(lstTemplate, UserDetail, Phone);
                        SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);

                    }

                    else if (Convert.ToUInt64(lstTemplate[0].EventID) == (long)NotificationEventConstants.Users.SMSSendtoUserForgotPassword)
                    {
                        string Phone = Parameter.ToString();
                        string Email = Parameters["Email"].ToString();

                       

                        var UserDetail = db.Repository<User>().GetAll().Where(x => x.Phone == Phone).FirstOrDefault();

                        SMSNotification SMSNotification = PrepareSMSNotification<User>(lstTemplate, UserDetail, Phone);
                        SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);

                    }

                    else if (Convert.ToUInt64(lstTemplate[0].EventID) == (long)NotificationEventConstants.Regift.SMSSendToUser)
                    {
                        Int32 ID = Convert.ToInt32(Parameter);
                        var UserDetail = db.Repository<User>().FindById(ID);

                        SMSNotification SMSNotification = PrepareSMSNotification<User>(lstTemplate, UserDetail, UserDetail.Phone);
                        SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);

                    }

                    else if (Convert.ToUInt64(lstTemplate[0].EventID) == (long)NotificationEventConstants.Regift.Updated)
                    {
                        Int32 ID = Convert.ToInt32(Parameter);
                        var UserDetail = db.Repository<User>().FindById(ID);

                        SMSNotification SMSNotification = PrepareSMSNotification<User>(lstTemplate, UserDetail, UserDetail.Phone);

                        string Comments = Parameters["Comments"].ToString();

                        if (string.IsNullOrEmpty(Comments))
                            SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);
                        else
                            SendSMS(SMSNotification.MobileNumber, Comments);
                    }

                    else if (Convert.ToUInt64(lstTemplate[0].EventID) == (long)NotificationEventConstants.Recycle.Updated)
                    {
                        Int32 ID = Convert.ToInt32(Parameter);
                        var UserDetail = db.Repository<User>().FindById(ID);

                        SMSNotification SMSNotification = PrepareSMSNotification<User>(lstTemplate, UserDetail, UserDetail.Phone);

                        string Comments = Parameters["Comments"].ToString();

                        if (string.IsNullOrEmpty(Comments))
                            SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);
                        else
                            SendSMS(SMSNotification.MobileNumber, Comments);
                    }

                    else if (Convert.ToUInt64(lstTemplate[0].EventID) == (long)NotificationEventConstants.RequestaBin.Updated)
                    {
                        Int32 ID = Convert.ToInt32(Parameter);
                        var UserDetail = db.Repository<User>().FindById(ID);

                        SMSNotification SMSNotification = PrepareSMSNotification<User>(lstTemplate, UserDetail, UserDetail.Phone);

                        string Comments = Parameters["Comments"].ToString();

                        if (string.IsNullOrEmpty(Comments))
                            SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);
                        else
                            SendSMS(SMSNotification.MobileNumber, Comments);
                    }

                    else if (Convert.ToUInt64(lstTemplate[0].EventID) == (long)NotificationEventConstants.Recycle.SMSSentoUser)
                    {
                        Int32 ID = Convert.ToInt32(Parameter);
                        var UserDetail = db.Repository<User>().FindById(ID);

                        SMSNotification SMSNotification = PrepareSMSNotification<User>(lstTemplate, UserDetail, UserDetail.Phone);
                        SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);

                    }



                    else if (Convert.ToUInt64(lstTemplate[0].EventID) == (long)NotificationEventConstants.RePlant.SMSSendToUser)
                    {
                        Int32 ID = Convert.ToInt32(Parameter);
                        var UserDetail = db.Repository<User>().FindById(ID);

                        SMSNotification SMSNotification = PrepareSMSNotification<User>(lstTemplate, UserDetail, UserDetail.Phone);
                        SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);

                    }

                    else if (Convert.ToUInt64(lstTemplate[0].EventID) == (long)NotificationEventConstants.Report.SendSMSToUser)
                    {
                        string Description = Parameters["Description"].ToString();
                        Int32 ID = Convert.ToInt32(Parameter);
                        var UserDetail = db.Repository<User>().FindById(ID);
                        UserDetail.Status = Description;

                        SMSNotification SMSNotification = PrepareSMSNotification<User>(lstTemplate, UserDetail, UserDetail.Phone);
                        SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);

                    }

                    else if (Convert.ToUInt64(lstTemplate[0].EventID) == (long)NotificationEventConstants.RequestaBin.SendSMSToUser)
                    {
                        string TrackingNumber = Parameters["TrackingNumber"].ToString();
                        Int32 ID = Convert.ToInt32(Parameter);
                        var UserDetail = db.Repository<User>().FindById(ID);

                        UserDetail.Status = TrackingNumber;

                        SMSNotification SMSNotification = PrepareSMSNotification<User>(lstTemplate, UserDetail, UserDetail.Phone);
                        SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);

                    }
                    else if (Convert.ToUInt64(lstTemplate[0].EventID) == (long)NotificationEventConstants.Common.SMSSendtoUserForReportAProblem)
                    {
                        string Phone = Parameters["Phone"].ToString();

                        User user = new User();

                        SMSNotification SMSNotification = PrepareSMSNotification<User>(lstTemplate, user, Phone);
                        SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);

                    }

                    else if (Convert.ToUInt64(lstTemplate[0].EventID) == (long)NotificationEventConstants.Children.SendSMSToUser)
                    {
                        string ChildName = Parameters["ChildName"].ToString();

                        string SchoolName = Parameters["SchoolName"].ToString();

                        Int32 ID = Convert.ToInt32(Parameter);
                        var UserDetail = db.Repository<User>().FindById(ID);

                        GPNViewModelForSMS ChildInfo = new GPNViewModelForSMS
                        {
                            Name = UserDetail.FullName,
                            ChildName = ChildName,
                            SchoolName = SchoolName
                        };

                        SMSNotification SMSNotification = PrepareSMSNotification<GPNViewModelForSMS>(lstTemplate, ChildInfo, UserDetail.Phone);
                        SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);

                    }

                    else if (Convert.ToUInt64(lstTemplate[0].EventID) == (long)NotificationEventConstants.Organization.SendSMSToUser)
                    {
                        string OrgName = Parameters["OrgName"].ToString();
                        Int32 ID = Convert.ToInt32(Parameter);
                        var UserDetail = db.Repository<User>().FindById(ID);

                        GPNViewModelForSMS ChildInfo = new GPNViewModelForSMS
                        {
                            OrgName = OrgName,
                            Name = UserDetail.FullName
                        };

                        SMSNotification SMSNotification = PrepareSMSNotification<GPNViewModelForSMS>(lstTemplate, ChildInfo, UserDetail.Phone);
                        SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);
                    }

                    else if (Convert.ToUInt64(lstTemplate[0].EventID) == (long)NotificationEventConstants.NGO.SendSMSToUser)
                    {
                        string NGOName = Parameters["NGOName"].ToString();
                        Int32 ID = Convert.ToInt32(Parameter);
                        var UserDetail = db.Repository<User>().FindById(ID);

                        GPNViewModelForSMS ChildInfo = new GPNViewModelForSMS
                        {
                            NGOName = NGOName,
                            Name = UserDetail.FullName
                        };

                        SMSNotification SMSNotification = PrepareSMSNotification<GPNViewModelForSMS>(lstTemplate, ChildInfo, UserDetail.Phone);
                        SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);
                    }

                    else if (Convert.ToUInt64(lstTemplate[0].EventID) == (long)NotificationEventConstants.School.SendSMSToUser)
                    {
                        string ContactPerson = Parameters["ContactPerson"].ToString();
                        string Phone = Parameters["Phone"].ToString();
                        string School = Parameters["Name"].ToString();
                        User user = new User();
                        user.FullName = ContactPerson;
                        user.Phone = Phone;
                        user.Status = School;
                        SMSNotification SMSNotification = PrepareSMSNotification<User>(lstTemplate, user, user.Phone);
                        SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);
                    }
                    else if (Convert.ToUInt64(lstTemplate[0].EventID) == (long)NotificationEventConstants.InviteFriend.SMSSendtoFriend)
                    {
                        string Phone = Parameter;
                        User user = new User();
                        SMSNotification SMSNotification = PrepareSMSNotification<User>(lstTemplate, user, Phone);
                        SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);

                    }
                    else if (Convert.ToUInt64(lstTemplate[0].EventID) == (long)NotificationEventConstants.SuspendUsers.SMSSendtoEmployee)
                    {
                        string Phone = Parameter;
                        User user = new User();
                        SMSNotification SMSNotification = PrepareSMSNotification<User>(lstTemplate, user, Phone);
                        SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);

                    }
                    else if (Convert.ToUInt64(lstTemplate[0].EventID) == (long)NotificationEventConstants.SuspendUsers.SMSSendtoMember)
                    {
                        string Phone = Parameter;
                        User user = new User();
                       
                        SMSNotification SMSNotification = PrepareSMSNotification<User>(lstTemplate, user, Phone);
                        SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);

                    }
                    else if (Convert.ToUInt64(lstTemplate[0].EventID) == (long)NotificationEventConstants.SuspendUsers.SMSSendtoStudent)
                    {
                        string Phone = Parameter;
                        User user = new User();
                        SMSNotification SMSNotification = PrepareSMSNotification<User>(lstTemplate, user, Phone);
                        SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);

                    }
                    else if (Convert.ToUInt64(lstTemplate[0].EventID) == (long)NotificationEventConstants.SuspendUsers.SMSSendtoStaff)
                    {
                        string Phone = Parameter;
                        User user = new User();
                        SMSNotification SMSNotification = PrepareSMSNotification<User>(lstTemplate, user, Phone);
                        SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);

                    }
                    else if (Convert.ToUInt64(lstTemplate[0].EventID) == (long)NotificationEventConstants.GOI.SMSSendToLocationAdmin)
                    {
                        Int32 ID = Convert.ToInt32(Parameter);
                        string Company = Parameters["Company"].ToString();
                        string Location = Parameters["Location"].ToString();
                        string Date = Parameters["Date"].ToString();
                        string Weight = Parameters["Weight"].ToString();
                        var UserDetail = db.Repository<User>().FindById(ID);

                        GOILocationViewModelForSMS adminDetails = new GOILocationViewModelForSMS
                        {
                            Company = Company,
                            Location = Location,
                            Date = Date,
                            Weight = Weight
                        };

                        SMSNotification SMSNotification = PrepareSMSNotification<GOILocationViewModelForSMS>(lstTemplate, adminDetails, UserDetail.Phone);
                        SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);

                    }

                }
                return isAdded;
            }
            catch (Exception ex)
            {
                Common.ServerResponse.ServiceResponse.LogError(ex);
                return false;
            }

        }


        public void SendSMS(string To, string Message)
        {
            try
            {

                string SendEmail = System.Configuration.ConfigurationManager.AppSettings[AppSettings.SMS_SECTION_SENT].ToString();
                if (SendEmail != "true")
                {
                    return;
                }


                string UserName = "Amal";
                string Password = "Amal!@#123";
                string ClientID = "Amal";
                string Mask = "Amal";

                string URI = "http://www.smspoint.pk/api/smsapi/";

                string res = "";

                //  string Message = "Dear AR - Congratulation!\n<br>You have earned 500 Green Points due to your recyclable material of 50Kg.\n<br>Thanks\nAmal";
                //  string To = "923214416412";
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
