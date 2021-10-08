using DrTech.Amal.Common.Extentions;
using DrTech.Amal.SQLDataAccess;
using DrTech.Amal.SQLModels;
//using DrTech.Models.ViewModels;
using DrTech.Amal.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static DrTech.Amal.Common.Extentions.Constants;
using DrTech.Amal.SQLDataAccess.CustomModels;
using DrTech.Amal.Common.Helpers;

namespace DrTech.Amal.Notifications
{
    public class NotifyEvent : BaseNotifyEvent
    {

        ContextDB db;
        public Dictionary<string, object> Parameters { get; set; }
        public NotifyEvent()
        {
            this.Parameters = new Dictionary<string, object>();
            db = new ContextDB();
        }
        public bool AddNotifyEvent(long EventID, string Parameter)
        {
            try
            {
                bool isAdded = false;
                string EventId = Convert.ToString(EventID);
                string AdminEmail = System.Configuration.ConfigurationManager.AppSettings[AppSettings.EMAIL_SECTION_TO_EMAIL].ToString();

                List<EmailNotificationEvent> lstTemplate = new List<EmailNotificationEvent>();
                lstTemplate = db.Repository<EmailNotificationEvent>().GetAll().Where(x => x.EventId == EventID && x.IsActive == true).ToList();

                if (lstTemplate.Count > 0)
                {
                    if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Recycle.EmailSendToAdminRecycleInfo)
                    {


                        Recycle mdlRecycle = db.Repository<Recycle>().GetAll().Where(x => x.FileName == Parameter).FirstOrDefault();

                        var UersDetails = db.Repository<User>().FindById(mdlRecycle.UserID);
                        if (UersDetails != null)
                        {
                            User mdluser = new User
                            {
                                FullName = UersDetails.FullName,
                                Address = UersDetails.Address,
                                FileName = mdlRecycle.FileName,
                                Phone = UersDetails.Phone,
                                CreatedDate = mdlRecycle.CollectorDateTime,
                                Longitude = UersDetails.Longitude,
                                Latitude = UersDetails.Latitude,

                            };

                            if (mdluser != null)
                            {
                                EmailNotification EmailNotification = PrepareEmailNotification<User>(lstTemplate, mdluser, Parameter, AdminEmail);
                                EmailHelper.SendEmail(EmailNotification.EmailSubject, EmailNotification.EmailBody, "request@Amalforlife.com");
                                db.Repository<EmailNotification>().Insert(EmailNotification);
                                db.Save();
                            }
                        }
                    }
                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Recycle.EmailSendToUserCollectionTime)
                    {
                        // var RecycleItemsDetails = _IUWork.FindOneByID<MrClean>(Parameter, CollectionNames.RECYCLE).Result; 
                        Int32 ID = Convert.ToInt32(Parameter);
                        var Users = db.Repository<User>().FindById(ID); 

                        if (Users != null)
                        {


                            User mdlUser = new User
                            {
                                FullName = Users.FullName,
                                Address = Users.Address,
                                GreenPoints = Users.GreenPoints
                            };

                            if (mdlUser != null)
                            {
                                EmailNotification EmailNotification = PrepareEmailNotification<User>(lstTemplate, mdlUser, Parameter, mdlUser.Email);
                                EmailHelper.SendEmail(EmailNotification.EmailSubject, EmailNotification.EmailBody, "request@Amalforlife.com");
                                db.Repository<EmailNotification>().Insert(EmailNotification);
                                db.Save();
                            }
                        }
                    }
                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.RequestaBin.EmailSentoAdminForServerBin)
                    {
                        Int32 ID = Convert.ToInt32(Parameter);
                        var Users = db.Repository<User>().FindById(ID);

                        CommonViewModelForEmail CommonInfo = new CommonViewModelForEmail
                        {
                            Name = Users.FullName,
                            Address = Users.Address,
                            Phone = Users.Phone,
                            Longitude = Convert.ToDecimal(Users.Longitude),
                            Latitude = Convert.ToDecimal(Users.Latitude),
                            TrackingNumber = Parameters["TrackingNumber"].ToString(),
                            BinName = Parameters["BinName"].ToString()

                        };

                        if (CommonInfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<CommonViewModelForEmail>(lstTemplate, CommonInfo, Parameter, AdminEmail);
                            EmailHelper.SendEmail(EmailNotification.EmailSubject, EmailNotification.EmailBody, EmailNotification.EmailTo);
                            db.Repository<EmailNotification>().Insert(EmailNotification);
                            db.Save();
                        }
                    }
                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.RePlant.EmailSendToAdminForReplantInfo)
                    {
                        Int32 ID = Convert.ToInt32(Parameter);
                        var user = db.Repository<User>().FindById(ID);

                        //user.Replant.ToList()
                        CommonViewModelForEmail CommonInfo = new CommonViewModelForEmail
                        {
                            Name = user.FullName,
                            Address = user.Address,
                            Phone = user.Phone,
                            FileName = Parameters["FileName"].ToString(),
                            Longitude = Convert.ToDecimal(user.Longitude),
                            Latitude = Convert.ToDecimal(user.Latitude),
                            PlantName = Parameters["PlantName"].ToString(),
                            TreeCount = Convert.ToInt32(Parameters["TreeCount"]),
                            Height = Parameters["Height"].ToString()
                        };

                        if (CommonInfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<CommonViewModelForEmail>(lstTemplate, CommonInfo, Parameter, AdminEmail);
                            EmailHelper.SendEmail(EmailNotification.EmailSubject, EmailNotification.EmailBody, EmailNotification.EmailTo);
                            db.Repository<EmailNotification>().Insert(EmailNotification);
                            db.Save();
                        }
                    }
                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.RePlant.EmailSentToUserForGreenPoints)
                    {
                        Int32 ID = Convert.ToInt32(Parameter);
                        var user = db.Repository<User>().FindById(ID);

                        //user.Replant.ToList()
                        User userinfo = new User
                        {
                            FullName = user.FullName,
                            Address = user.Address,
                            GreenPoints = user.GreenPoints,
                            Email = user.Email
                        };

                        if (userinfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<User>(lstTemplate, userinfo, Parameter, userinfo.Email);
                            EmailHelper.SendEmail(EmailNotification.EmailSubject, EmailNotification.EmailBody, EmailNotification.EmailTo);
                            db.Repository<EmailNotification>().Insert(EmailNotification);
                            db.Save();
                        }
                    }
                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Regift.EmailSendToAdminForApproval)
                    {
                        Int32 ID = Convert.ToInt32(Parameter);
                        var user = db.Repository<User>().FindById(ID);

                        CommonViewModelForEmail CommonInfo = new CommonViewModelForEmail
                        {
                            FileName = Parameters["FileName"].ToString(),

                            Longitude = Convert.ToDecimal(Parameters["Longitude"]),
                            Latitude = Convert.ToDecimal(Parameters["Latitude"]),
                            TypeDescription = Parameters["TypeDescription"].ToString(),
                            SubTypeDescription = Parameters["SubTypeDescription"].ToString(),
                            DonateToDescription = Parameters["DonateToDescription"].ToString(),
                            Name = user.FullName,
                            Address = user.Address,
                            Phone = user.Phone

                        };

                        //Users userinfo = new Users
                        //{
                        //    FullName = user.FullName,
                        //    Address = user.Address,
                        //    FileName = Parameters["FileName"].ToString(),
                        //    Longitude = user.Longitude,
                        //    Latitude = user.Latitude

                        //};

                        if (CommonInfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<CommonViewModelForEmail>(lstTemplate, CommonInfo, Parameter, AdminEmail);
                            db.Repository<EmailNotification>().Insert(EmailNotification);
                            db.Save();
                        }
                    }
                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Regift.EmailSendToUser)
                    {
                        Int32 ID = Convert.ToInt32(Parameter);
                        var user = db.Repository<User>().FindById(ID);

                        //user.Replant.ToList()
                        User userinfo = new User
                        {
                            FullName = user.FullName,
                        };

                        if (userinfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<User>(lstTemplate, userinfo, Parameter, userinfo.Email);
                            db.Repository<EmailNotification>().Insert(EmailNotification);
                            db.Save();
                        }
                    }
                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Users.EmailSendtoUserForgotPassword)
                    {
                        Int32 ID = Convert.ToInt32(Parameter);
                        var user = db.Repository<User>().FindById(ID);

                        //user.Replant.ToList()
                        User userinfo = new User
                        {
                            FullName = user.FullName,
                            Email = user.Email,
                            Phone = user.Phone
                        };
                        if (userinfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<User>(lstTemplate, userinfo, Parameter, AdminEmail);
                            db.Repository<EmailNotification>().Insert(EmailNotification);
                            db.Save();
                        }
                    }


                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Users.RegistrationProcessEmailSendToAdmin)
                    {
                        // var user = _IUWork.FindOneByID<Users>(Parameter, CollectionNames.USERS).Result;

                        User mdlUser = db.Repository<User>().GetAll().Where(x => x.Phone == Parameter).FirstOrDefault();

                        User userinfo = new User
                        {
                            Phone = mdlUser.Phone,
                        };
                        if (userinfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<User>(lstTemplate, userinfo, Parameter, AdminEmail);
                            db.Repository<EmailNotification>().Insert(EmailNotification);
                            db.Save();
                        }
                    }

                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Users.RegistrationCompletedEmailSendToAdmin)
                    {
                        Int32 ID = Convert.ToInt32(Parameter);
                        var user = db.Repository<User>().FindById(ID);

                        User userinfo = new User
                        {
                            FullName = user.FullName,
                            Phone = user.Phone,
                            Password = user.Password,
                            Email = user.Email,
                            Address = user.Address,
                            City = user.City,
                            Latitude = user.Latitude,
                            Longitude = user.Longitude

                        };
                        if (userinfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<User>(lstTemplate, userinfo, Parameter, AdminEmail);
                            db.Repository<EmailNotification>().Insert(EmailNotification);
                            db.Save();
                        }
                    }


                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Common.EmailSendtoUserReportAProblem)
                    {


                        ReportProblemViewModel userinfo = new ReportProblemViewModel
                        {
                            Email = Parameters["Email"].ToString(),
                            Phone = Parameters["Phone"].ToString(),
                            Problem = Parameters["Description"].ToString(),
                            Subject = Parameters["IssueType"].ToString(),

                        };
                        if (userinfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<ReportProblemViewModel>(lstTemplate, userinfo, Parameter, userinfo.Email);
                            EmailHelper.SendEmail(EmailNotification.EmailSubject, EmailNotification.EmailBody, EmailNotification.EmailTo);
                            db.Repository<EmailNotification>().Insert(EmailNotification);
                            db.Save();
                        }
                    }


                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Common.EmailSendtoAdminReportAProblem)
                    {


                        ReportProblemViewModel userinfo = new ReportProblemViewModel
                        {
                            Email = Parameters["Email"].ToString(),
                            Phone = Parameters["Phone"].ToString(),
                            Problem = Parameters["Description"].ToString(),
                            Subject = Parameters["IssueType"].ToString(),

                        };
                        if (userinfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<ReportProblemViewModel>(lstTemplate, userinfo, Parameter, userinfo.Email);
                            EmailHelper.SendEmail(EmailNotification.EmailSubject, EmailNotification.EmailBody, EmailNotification.EmailTo);
                            db.Repository<EmailNotification>().Insert(EmailNotification);
                            db.Save();
                        }
                    }

                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.InviteFriend.EmailSendtoFriend)
                    {
                        ReportProblemViewModel userinfo = new ReportProblemViewModel
                        {
                            Email = Parameter

                        };
                        if (userinfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<ReportProblemViewModel>(lstTemplate, userinfo, Parameter, userinfo.Email);
                            db.Repository<EmailNotification>().Insert(EmailNotification);
                            db.Save();
                        }
                    }                 

                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Refuse.RefuseEmailSendtoAdmin)
                    {
                        Int32 ID = Convert.ToInt32(Parameter);
                        var user = db.Repository<User>().FindById(ID);


                        CommonViewModelForEmail RefuseInfo = new CommonViewModelForEmail
                        {
                            FileName = Parameters["FileName"].ToString(),
                            Idea = Parameters["Idea"].ToString(),
                            Longitude = Convert.ToDecimal(Parameters["Longitude"]),
                            Latitude = Convert.ToDecimal(Parameters["Latitude"]),
                            Name = user.FullName,
                            Address = user.Address,
                            Phone = user.Phone

                        };
                        if (RefuseInfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<CommonViewModelForEmail>(lstTemplate, RefuseInfo, Parameter, AdminEmail);
                            db.Repository<EmailNotification>().Insert(EmailNotification);
                            db.Save();
                        }
                    }


                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Reduce.ReduceEmailSendtoAdmin)
                    {
                        Int32 ID = Convert.ToInt32(Parameter);
                        var user = db.Repository<User>().FindById(ID);


                        CommonViewModelForEmail RefuseInfo = new CommonViewModelForEmail
                        {
                            FileName = Parameters["FileName"].ToString(),
                            Idea = Parameters["Idea"].ToString(),
                            Longitude = Convert.ToDecimal(Parameters["Longitude"]),
                            Latitude = Convert.ToDecimal(Parameters["Latitude"]),
                            Name = user.FullName,
                            Address = user.Address,
                            Phone = user.Phone

                        };
                        if (RefuseInfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<CommonViewModelForEmail>(lstTemplate, RefuseInfo, Parameter, AdminEmail);
                            db.Repository<EmailNotification>().Insert(EmailNotification);
                            db.Save();
                        }
                    }


                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Reuse.ReuseEmailSendtoAdmin)
                    {
                        Int32 ID = Convert.ToInt32(Parameter);
                        var user = db.Repository<User>().FindById(ID);


                        CommonViewModelForEmail RefuseInfo = new CommonViewModelForEmail
                        {
                            FileName = Parameters["FileName"].ToString(),
                            Idea = Parameters["Idea"].ToString(),
                            Longitude = Convert.ToDecimal(Parameters["Longitude"]),
                            Latitude = Convert.ToDecimal(Parameters["Latitude"]),
                            Name = user.FullName,
                            Address = user.Address,
                            Phone = user.Phone

                        };
                        if (RefuseInfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<CommonViewModelForEmail>(lstTemplate, RefuseInfo, Parameter, AdminEmail);
                            db.Repository<EmailNotification>().Insert(EmailNotification);
                            db.Save();
                        }
                    }


                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Children.SendEmailToAdmin)
                    {
                        Int32 ID = Convert.ToInt32(Parameter);
                        var user = db.Repository<User>().FindById(ID);


                        CommonViewModelForEmail ChildInfo = new CommonViewModelForEmail
                        {
                            ChildName = Parameters["ChildName"].ToString(),
                            Gender = Parameters["Gender"].ToString(),
                            RollNo = Parameters["RollNo"].ToString(),
                            SchoolName = Parameters["SchoolName"].ToString(),
                            Branch = Parameters["Branch"].ToString(),
                            Name = user.FullName,
                            Address = user.Address,
                            Phone = user.Phone
                        };
                        if (ChildInfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<CommonViewModelForEmail>(lstTemplate, ChildInfo, Parameter, AdminEmail);
                            db.Repository<EmailNotification>().Insert(EmailNotification);
                            db.Save();
                        }
                    }

                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Organization.SendEmailToAdmin)
                    {
                        Int32 ID = Convert.ToInt32(Parameter);
                        var user = db.Repository<User>().FindById(ID);


                        CommonViewModelForEmail ChildInfo = new CommonViewModelForEmail
                        {
                            OrgName = Parameters["OrgName"].ToString(),
                            EmployeeID = Parameters["EmployeeID"].ToString(),
                            Name = user.FullName,
                            Address = user.Address
                        };
                        if (ChildInfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<CommonViewModelForEmail>(lstTemplate, ChildInfo, Parameter, AdminEmail);
                            db.Repository<EmailNotification>().Insert(EmailNotification);
                            db.Save();
                        }
                    }

                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.NGO.SendEmailToAdmin)
                    {
                        Int32 ID = Convert.ToInt32(Parameter);
                        var user = db.Repository<User>().FindById(ID);


                        CommonViewModelForEmail ChildInfo = new CommonViewModelForEmail
                        {
                            OrgName = Parameters["NGOName"].ToString(),
                            EmployeeID = Parameters["EmployeeID"].ToString(),
                            Name = user.FullName,
                            Address = user.Address
                        };
                        if (ChildInfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<CommonViewModelForEmail>(lstTemplate, ChildInfo, Parameter, AdminEmail);
                            db.Repository<EmailNotification>().Insert(EmailNotification);
                            db.Save();
                        }
                    }


                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.School.SendEmailToAdmin)
                    {
                        Int32 ID = Convert.ToInt32(Parameter);
                        var user = db.Repository<User>().FindById(ID);

                        CommonViewModelForEmail SchoolInfo = new CommonViewModelForEmail
                        {
                            Name = Parameters["ContactPerson"].ToString(),
                            SchoolName = Parameters["Name"].ToString(),
                            Branch = Parameters["Branch"].ToString(),
                            Email = Parameters["Email"].ToString(),
                            Phone = Parameters["Phone"].ToString(),
                        };
                        if (SchoolInfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<CommonViewModelForEmail>(lstTemplate, SchoolInfo, Parameter, AdminEmail);
                            if (EmailNotification.EmailTo == "true")
                                EmailNotification.EmailTo = AdminEmail;
                            db.Repository<EmailNotification>().Insert(EmailNotification);
                            db.Save();
                        }
                    }

                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Driver.SendtoAdminPin)
                    {
                        string Phone = Parameters["Phone"].ToString();
                        string PIN = Parameters["PIN"].ToString();
                        Driver userinfo = new Driver();
                        userinfo.Phone = Phone;
                        userinfo.PIN = PIN;

                        if (userinfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<Driver>(lstTemplate, userinfo, Parameter, AdminEmail);
                            db.Repository<EmailNotification>().Insert(EmailNotification);
                            db.Save();
                        }
                    }

                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Report.ReportEmailSendtoAdmin)
                    {
                        string Description = Parameters["Description"].ToString();
                        string Longitude = Parameters["Longitude"].ToString();
                        string Latitude = Parameters["Latitude"].ToString();


                        Int32 ID = Convert.ToInt32(Parameter);
                        User userinfo = db.Repository<User>().FindById(ID);

                        userinfo.Latitude = Convert.ToDecimal(Latitude);
                        userinfo.Longitude = Convert.ToDecimal(Longitude);
                        userinfo.Status = Description;

                        if (userinfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<User>(lstTemplate, userinfo, Parameter, AdminEmail);
                            EmailHelper.SendEmail(EmailNotification.EmailSubject, EmailNotification.EmailBody, EmailNotification.EmailTo);
                            db.Repository<EmailNotification>().Insert(EmailNotification);
                            db.Save();
                        }
                    }
                    //Send email template for how to use GPN
                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Users.SendEmailToSchoolUserForFurtherProcess)
                    {
                        // var RecycleItemsDetails = _IUWork.FindOneByID<MrClean>(Parameter, CollectionNames.RECYCLE).Result; 
                        Int32 ID = Convert.ToInt32(Parameter);
                        var Users = db.Repository<User>().FindById(ID);

                        if (Users != null)
                        {


                            User mdlUser = new User
                            {
                                FullName = Users.FullName,
                                Address = Users.Address,
                                GreenPoints = Users.GreenPoints
                            };

                            if (mdlUser != null)
                            {
                                EmailNotification EmailNotification = PrepareEmailNotification<User>(lstTemplate, mdlUser, Parameter, mdlUser.Email);
                                db.Repository<EmailNotification>().Insert(EmailNotification);
                                db.Save();
                            }
                        }
                    }
                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Users.SendEmailToBusinessUserForFurtherProcess)
                    {
                        // var RecycleItemsDetails = _IUWork.FindOneByID<MrClean>(Parameter, CollectionNames.RECYCLE).Result; 
                        Int32 ID = Convert.ToInt32(Parameter);
                        var Users = db.Repository<User>().FindById(ID);

                        if (Users != null)
                        {


                            User mdlUser = new User
                            {
                                FullName = Users.FullName,
                                Address = Users.Address,
                                GreenPoints = Users.GreenPoints
                            };

                            if (mdlUser != null)
                            {
                                EmailNotification EmailNotification = PrepareEmailNotification<User>(lstTemplate, mdlUser, Parameter, mdlUser.Email);
                                db.Repository<EmailNotification>().Insert(EmailNotification);
                                db.Save();
                            }
                        }
                    }
                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Users.SendEmailToOrgUserForFurtherProcess)
                    {
                        // var RecycleItemsDetails = _IUWork.FindOneByID<MrClean>(Parameter, CollectionNames.RECYCLE).Result; 
                        Int32 ID = Convert.ToInt32(Parameter);
                        var Users = db.Repository<User>().FindById(ID);

                        if (Users != null)
                        {


                            User mdlUser = new User
                            {
                                FullName = Users.FullName,
                                Address = Users.Address,
                                GreenPoints = Users.GreenPoints
                            };

                            if (mdlUser != null)
                            {
                                EmailNotification EmailNotification = PrepareEmailNotification<User>(lstTemplate, mdlUser, Parameter, mdlUser.Email);
                                db.Repository<EmailNotification>().Insert(EmailNotification);
                                db.Save();
                            }
                        }
                    }
                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Email.SendEmailToUser)
                    {
                        Int32 ID = Convert.ToInt32(Parameter);
                        var user = db.Repository<User>().FindById(ID);

                        //user.Replant.ToList()
                        User userinfo = new User
                        {
                            FullName = user.FullName,
                            Email = user.Email
                        };

                        if (userinfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<User>(lstTemplate, userinfo, Parameter, userinfo.Email);
                            db.Repository<EmailNotification>().Insert(EmailNotification);
                            db.Save();
                        }
                    }

                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Email.SendEmailOfPhoneConfirmation)
                    {
                        Int32 ID = Convert.ToInt32(Parameter);
                        var user = db.Repository<User>().FindById(ID);
                        string SMSCode = Parameters["SMSCode"].ToString();

                        //user.Replant.ToList()
                        UserTemplateViewModel userinfo = new UserTemplateViewModel
                        {
                            SMSCode = SMSCode
                        };
                       

                        if (userinfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<UserTemplateViewModel>(lstTemplate, userinfo, Parameter, user.Email);
                            db.Repository<EmailNotification>().Insert(EmailNotification);
                            db.Save();
                        }
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


    }
}
