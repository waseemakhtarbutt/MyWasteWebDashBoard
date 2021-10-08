using DrTech.Common.Helpers;
using DrTech.DAL;
using DrTech.Models;
using DrTech.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using static DrTech.Common.Extentions.Constants;

namespace DrTech.Notifications
{
    public class SMSNotifyEvent : BaseNotifyEvent
    {
        IMongoDAL _IUWork;
        public Dictionary<string, object> Parameters { get; set; }
        public SMSNotifyEvent()
        {
            this.Parameters = new Dictionary<string, object>();
            _IUWork = new MongoDAL();
        }

        public bool AddSMSNotifyEvent(long EventID, string Parameter)
        {
            try
            {
                bool isAdded = false;
                string EventId = Convert.ToString(EventID);
                //string AdminEmail = AppSettingsHelper.GetAttributeValue(Constants.AppSettings.EMAIL_SECTION, Constants.AppSettings.EMAIL_SECTION_TO_EMAIL);
                List<FilterHelper> filter = new List<FilterHelper>
                {
                    new FilterHelper
                    {
                        Field = "EventId",
                        Value = EventId
                    }
                };

                List<SMSNotificationEvents> lstTemplate = _IUWork.GetModelData<SMSNotificationEvents>(filter, CollectionNames.SMSNotificationEvents);
                if (lstTemplate.Count > 0)
                {
                    if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Users.SendSMSToUserForEntrolment)
                    {
                        string Mobile = Parameter.ToString();
                        string SMSCode = Parameters["SMSCode"].ToString();
                        Users userinfo = new Users();

                        SMSNotifications SMSNotification = PrepareSMSNotification<Users>(lstTemplate, userinfo, Mobile);
                        SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);
                    }
                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Users.SendVerificationCodeSMS)
                    {
                        string Phone = Parameter.ToString();
                        string SMSCode = Parameters["SMSCode"].ToString();

                        List<FilterHelper> filters = new List<FilterHelper>
                    {
                     new FilterHelper
                    {
                        Field = "Phone",
                        Value = Phone
                    }
                    };

                        var UserDetail = _IUWork.GetModelData<Users>(filters, CollectionNames.USERS).FirstOrDefault();

                        UserDetail.SMSCode = Convert.ToInt32(SMSCode);

                        SMSNotifications SMSNotification = PrepareSMSNotification<Users>(lstTemplate, UserDetail, Phone);
                        SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);

                    }

                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Users.UpdateAfterSendToSMS)
                    {
                        string Phone = Parameter.ToString();
                        string SMSCode = Parameters["SMSCode"].ToString();

                        List<FilterHelper> filters = new List<FilterHelper>
                    {
                     new FilterHelper
                    {
                        Field = "Phone",
                        Value = Phone
                    }
                    };

                        var UserDetail = _IUWork.GetModelData<Users>(filters, CollectionNames.USERS).FirstOrDefault();

                        SMSNotifications SMSNotification = PrepareSMSNotification<Users>(lstTemplate, UserDetail, Phone);
                        SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);

                    }

                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Users.SMSSendtoUserForgotPassword)
                    {
                        string Phone = Parameter.ToString();
                        string Email = Parameters["Email"].ToString();

                        List<FilterHelper> filters = new List<FilterHelper>
                    {
                     new FilterHelper
                    {
                        Field = "Phone",
                        Value = Phone
                    }
                    };

                        var UserDetail = _IUWork.GetModelData<Users>(filters, CollectionNames.USERS).FirstOrDefault();

                        SMSNotifications SMSNotification = PrepareSMSNotification<Users>(lstTemplate, UserDetail, Phone);
                        SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);

                    }

                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Regift.SMSSendToUser)
                    {
                        var UserDetail = _IUWork.FindOneByID<Users>(Parameter, CollectionNames.USERS).Result;

                        SMSNotifications SMSNotification = PrepareSMSNotification<Users>(lstTemplate, UserDetail, UserDetail.Phone);
                        SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);

                    }


                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Recycle.SMSSentoUser)
                    {
                        var UserDetail = _IUWork.FindOneByID<Users>(Parameter, CollectionNames.USERS).Result;

                        SMSNotifications SMSNotification = PrepareSMSNotification<Users>(lstTemplate, UserDetail, UserDetail.Phone);
                        SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);

                    }



                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.RePlant.SMSSendToUser)
                    {
                        var UserDetail = _IUWork.FindOneByID<Users>(Parameter, CollectionNames.USERS).Result;

                        SMSNotifications SMSNotification = PrepareSMSNotification<Users>(lstTemplate, UserDetail, UserDetail.Phone);
                        SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);

                    }

                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Report.SendSMSToUser)
                    {
                        string Description = Parameters["Description"].ToString();

                        var UserDetail = _IUWork.FindOneByID<Users>(Parameter, CollectionNames.USERS).Result;
                        UserDetail.Status = Description;

                        SMSNotifications SMSNotification = PrepareSMSNotification<Users>(lstTemplate, UserDetail, UserDetail.Phone);
                        SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);

                    }

                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.RequestaBin.SendSMSToUser)
                    {
                        string TrackingNumber = Parameters["TrackingNumber"].ToString();

                        var UserDetail = _IUWork.FindOneByID<Users>(Parameter, CollectionNames.USERS).Result;

                        UserDetail.Status = TrackingNumber;

                        SMSNotifications SMSNotification = PrepareSMSNotification<Users>(lstTemplate, UserDetail, UserDetail.Phone);
                        SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);

                    }
                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Common.SMSSendtoUserForReportAProblem)
                    {
                        string Phone = Parameters["Phone"].ToString();

                        Users user = new Users();

                        SMSNotifications SMSNotification = PrepareSMSNotification<Users>(lstTemplate, user, Phone);
                        SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);

                    }

                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Children.SendSMSToUser)
                    {
                        string ChildName = Parameters["ChildName"].ToString();

                        string SchoolName = Parameters["SchoolName"].ToString();


                        var UserDetail = _IUWork.FindOneByID<Users>(Parameter, CollectionNames.USERS).Result;

                        GPNViewModelForSMS ChildInfo = new GPNViewModelForSMS
                        {
                            Name = UserDetail.FullName,
                            ChildName = ChildName,
                            SchoolName = SchoolName
                        };

                        SMSNotifications SMSNotification = PrepareSMSNotification<GPNViewModelForSMS>(lstTemplate, ChildInfo, UserDetail.Phone);
                        SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);

                    }

                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Organization.SendSMSToUser)
                    {
                        string OrgName = Parameters["OrgName"].ToString();
                        var UserDetail = _IUWork.FindOneByID<Users>(Parameter, CollectionNames.USERS).Result;

                        GPNViewModelForSMS ChildInfo = new GPNViewModelForSMS
                        {
                            OrgName = OrgName,
                            Name = UserDetail.FullName
                        };

                        SMSNotifications SMSNotification = PrepareSMSNotification<GPNViewModelForSMS>(lstTemplate, ChildInfo, UserDetail.Phone);
                        SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);
                    }

                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.NGO.SendSMSToUser)
                    {
                        string NGOName = Parameters["NGOName"].ToString();
                        var UserDetail = _IUWork.FindOneByID<Users>(Parameter, CollectionNames.USERS).Result;

                        GPNViewModelForSMS ChildInfo = new GPNViewModelForSMS
                        {
                            NGOName = NGOName,
                            Name = UserDetail.FullName
                        };

                        SMSNotifications SMSNotification = PrepareSMSNotification<GPNViewModelForSMS>(lstTemplate, ChildInfo, UserDetail.Phone);
                        SendSMS(SMSNotification.MobileNumber, SMSNotification.SMSText);
                    }

                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.School.SendSMSToUser)
                    {
                        string ContactPerson = Parameters["ContactPerson"].ToString();
                        string Phone = Parameters["Phone"].ToString();
                        string School = Parameters["Name"].ToString();
                        Users user = new Users();
                        user.FullName = ContactPerson;
                        user.Phone = Phone;
                        user.Status = School;
                        SMSNotifications SMSNotification = PrepareSMSNotification<Users>(lstTemplate, user, user.Phone);
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
