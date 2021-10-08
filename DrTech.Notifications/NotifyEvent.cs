using DrTech.Common.Extentions;
using DrTech.DAL;
using DrTech.Models;
using DrTech.Models.ViewModels;
using DrTech.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static DrTech.Common.Extentions.Constants;

namespace DrTech.Common.Helpers
{
    public class NotifyEvent : BaseNotifyEvent
    {

        IMongoDAL _IUWork;
        public Dictionary<string, object> Parameters { get; set; }
        public NotifyEvent()
        {
            this.Parameters = new Dictionary<string, object>();
            _IUWork = new MongoDAL();
        }
        public bool AddNotifyEvent(long EventID, string Parameter)
        {
            try
            {
                bool isAdded = false;
                string EventId = Convert.ToString(EventID);
                string AdminEmail = AppSettingsHelper.GetAttributeValue(Constants.AppSettings.EMAIL_SECTION, Constants.AppSettings.EMAIL_SECTION_TO_EMAIL);
                List<FilterHelper> filter = new List<FilterHelper>
                {
                    new FilterHelper
                    {
                        Field = "EventId",
                        Value = EventId
                    }
                };

                List<NotificationEvents> lstTemplate = _IUWork.GetModelData<NotificationEvents>(filter, CollectionNames.NotificationEvents);
                if (lstTemplate.Count > 0)
                {
                    if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Recycle.EmailSendToAdminRecycleInfo)
                    {
                        List<FilterHelper> filters = new List<FilterHelper>
                {
                    new FilterHelper
                    {
                        Field = "FileName",
                        Value = Parameter
                    }
                };

                        List<MrClean> lstItems = _IUWork.GetModelData<MrClean>(filters, lstTemplate[0].CollectionName);
                        var UersDetails = _IUWork.FindOneByID<Users>(lstItems[0].UserId, CollectionNames.USERS).Result;
                        if (UersDetails != null)
                        {
                            Users mdluser = new Users
                            {
                                FullName = UersDetails.FullName,
                                Address = UersDetails.Address,
                                FileName = lstItems[0].FileName,
                                Phone = UersDetails.Phone,
                                datetime = lstItems[0].CollectorDateTime,
                                Longitude = UersDetails.Longitude,
                                Latitude = UersDetails.Latitude,

                            };

                            if (mdluser != null)
                            {
                                EmailNotification EmailNotification = PrepareEmailNotification<Users>(lstTemplate, mdluser, Parameter, AdminEmail);
                                _IUWork.InsertOne<EmailNotification>(EmailNotification, Constants.CollectionNames.EmailNotification);
                            }
                        }
                    }
                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Recycle.EmailSendToUserCollectionTime)
                    {
                        // var RecycleItemsDetails = _IUWork.FindOneByID<MrClean>(Parameter, CollectionNames.RECYCLE).Result; 
                        var Users = _IUWork.FindOneByID<Users>(Parameter, CollectionNames.USERS).Result;

                        if (Users != null)
                        {


                            Users mdlUser = new Users
                            {
                                FullName = Users.FullName,
                                Address = Users.Address,
                                GreenPoints = Users.GreenPoints
                            };

                            if (mdlUser != null)
                            {
                                EmailNotification EmailNotification = PrepareEmailNotification<Users>(lstTemplate, mdlUser, Parameter, mdlUser.Email);
                                _IUWork.InsertOne<EmailNotification>(EmailNotification, Constants.CollectionNames.EmailNotification);

                            }
                        }
                    }
                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.RequestaBin.EmailSentoAdminForServerBin)
                    {
                        var user = _IUWork.FindOneByID<Users>(Parameter, CollectionNames.USERS).Result;

                        CommonViewModelForEmail CommonInfo = new CommonViewModelForEmail
                        {
                            Name = user.FullName,
                            Address = user.Address,
                            Phone = user.Phone,
                            Longitude = user.Longitude,
                            Latitude = user.Latitude,
                            TrackingNumber = Parameters["TrackingNumber"].ToString(),
                            BinName = Parameters["BinName"].ToString()

                        };

                        if (CommonInfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<CommonViewModelForEmail>(lstTemplate, CommonInfo, Parameter, AdminEmail);
                            _IUWork.InsertOne<EmailNotification>(EmailNotification, Constants.CollectionNames.EmailNotification);
                        }
                    }
                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.RePlant.EmailSendToAdminForReplantInfo)
                    {
                        var user = _IUWork.FindOneByID<Users>(Parameter, CollectionNames.USERS).Result;

                        //user.Replant.ToList()
                        CommonViewModelForEmail CommonInfo = new CommonViewModelForEmail
                        {
                            Name = user.FullName,
                            Address = user.Address,
                            Phone = user.Phone,
                            FileName = Parameters["FileName"].ToString(),
                            Longitude = user.Longitude,
                            Latitude = user.Latitude,
                            PlantName = Parameters["PlantName"].ToString(),
                            TreeCount = Convert.ToInt32(Parameters["TreeCount"]),
                            Height = Parameters["Height"].ToString()
                        };

                        if (CommonInfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<CommonViewModelForEmail>(lstTemplate, CommonInfo, Parameter, AdminEmail);
                            _IUWork.InsertOne<EmailNotification>(EmailNotification, Constants.CollectionNames.EmailNotification);
                        }
                    }
                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.RePlant.EmailSentToUserForGreenPoints)
                    {
                        var user = _IUWork.FindOneByID<Users>(Parameter, CollectionNames.USERS).Result;

                        //user.Replant.ToList()
                        Users userinfo = new Users
                        {
                            FullName = user.FullName,
                            Address = user.Address,
                            GreenPoints = user.GreenPoints,
                            Email = user.Email
                        };

                        if (userinfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<Users>(lstTemplate, userinfo, Parameter, userinfo.Email);
                            _IUWork.InsertOne<EmailNotification>(EmailNotification, Constants.CollectionNames.EmailNotification);
                        }
                    }
                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Regift.EmailSendToAdminForApproval)
                    {

                        var user = _IUWork.FindOneByID<Users>(Parameter, CollectionNames.USERS).Result;

                        CommonViewModelForEmail CommonInfo = new CommonViewModelForEmail
                        {
                            FileName = Parameters["FileName"].ToString(),

                            Longitude = Convert.ToDouble(Parameters["Longitude"]),
                            Latitude = Convert.ToDouble(Parameters["Latitude"]),
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
                            _IUWork.InsertOne<EmailNotification>(EmailNotification, Constants.CollectionNames.EmailNotification);
                        }
                    }
                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Regift.EmailSendToUser)
                    {
                        var user = _IUWork.FindOneByID<Users>(Parameter, CollectionNames.USERS).Result;

                        //user.Replant.ToList()
                        Users userinfo = new Users
                        {
                            FullName = user.FullName,
                        };

                        if (userinfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<Users>(lstTemplate, userinfo, Parameter, userinfo.Email);
                            _IUWork.InsertOne<EmailNotification>(EmailNotification, Constants.CollectionNames.EmailNotification);
                        }
                    }
                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Users.EmailSendtoUserForgotPassword)
                    {
                        var user = _IUWork.FindOneByID<Users>(Parameter, CollectionNames.USERS).Result;

                        //user.Replant.ToList()
                        Users userinfo = new Users
                        {
                            FullName = user.FullName,
                            Email = user.Email,
                            Phone = user.Phone
                        };
                        if (userinfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<Users>(lstTemplate, userinfo, Parameter, AdminEmail);
                            _IUWork.InsertOne<EmailNotification>(EmailNotification, Constants.CollectionNames.EmailNotification);
                        }
                    }


                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Users.RegistrationProcessEmailSendToAdmin)
                    {
                        // var user = _IUWork.FindOneByID<Users>(Parameter, CollectionNames.USERS).Result;

                        List<FilterHelper> filters = new List<FilterHelper>
                {
                    new FilterHelper
                    {
                        Field = "Phone",
                        Value = Parameter
                    }
                };

                        List<Users> lstItems = _IUWork.GetModelData<Users>(filters, lstTemplate[0].CollectionName);

                        Users userinfo = new Users
                        {
                            Phone = lstItems[0].Phone,
                        };
                        if (userinfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<Users>(lstTemplate, userinfo, Parameter, AdminEmail);
                            _IUWork.InsertOne<EmailNotification>(EmailNotification, Constants.CollectionNames.EmailNotification);
                        }
                    }

                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Users.RegistrationCompletedEmailSendToAdmin)
                    {
                        var user = _IUWork.FindOneByID<Users>(Parameter, CollectionNames.USERS).Result;

                        Users userinfo = new Users
                        {
                            FullName = user.FullName,
                            Phone = user.Phone,
                            Password = user.Password,
                            Email = user.Email,
                            Address = user.Address,
                            CityDescription = user.CityDescription,
                            Latitude = user.Latitude,
                            Longitude = user.Longitude

                        };
                        if (userinfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<Users>(lstTemplate, userinfo, Parameter, AdminEmail);
                            _IUWork.InsertOne<EmailNotification>(EmailNotification, Constants.CollectionNames.EmailNotification);
                        }
                    }


                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Common.EmailSendtoUserReportAProblem)
                    {


                        ReportProblemViewModel userinfo = new ReportProblemViewModel
                        {
                            Email = Parameters["Email"].ToString(),
                            Phone = Parameters["Phone"].ToString(),
                            Problem = Parameters["Problem"].ToString()

                        };
                        if (userinfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<ReportProblemViewModel>(lstTemplate, userinfo, Parameter, userinfo.Email);
                            _IUWork.InsertOne<EmailNotification>(EmailNotification, Constants.CollectionNames.EmailNotification);
                        }
                    }


                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Common.EmailSendtoAdminReportAProblem)
                    {


                        ReportProblemViewModel userinfo = new ReportProblemViewModel
                        {
                            Email = Parameters["Email"].ToString(),
                            Phone = Parameters["Phone"].ToString(),
                            Problem = Parameters["Problem"].ToString(),
                            Subject = Parameters["Subject"].ToString(),

                        };
                        if (userinfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<ReportProblemViewModel>(lstTemplate, userinfo, Parameter, userinfo.Email);
                            _IUWork.InsertOne<EmailNotification>(EmailNotification, Constants.CollectionNames.EmailNotification);
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
                            _IUWork.InsertOne<EmailNotification>(EmailNotification, Constants.CollectionNames.EmailNotification);
                        }
                    }

                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Refuse.RefuseEmailSendtoAdmin)
                    {
                        var user = _IUWork.FindOneByID<Users>(Parameter, CollectionNames.USERS).Result;


                        CommonViewModelForEmail RefuseInfo = new CommonViewModelForEmail
                        {
                            FileName = Parameters["FileName"].ToString(),
                            Idea = Parameters["Idea"].ToString(),
                            Longitude = Convert.ToDouble(Parameters["Longitude"]),
                            Latitude = Convert.ToDouble(Parameters["Latitude"]),
                            Name = user.FullName,
                            Address = user.Address,
                            Phone = user.Phone

                        };
                        if (RefuseInfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<CommonViewModelForEmail>(lstTemplate, RefuseInfo, Parameter, AdminEmail);
                            _IUWork.InsertOne<EmailNotification>(EmailNotification, Constants.CollectionNames.EmailNotification);
                        }
                    }


                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Reduce.ReduceEmailSendtoAdmin)
                    {
                        var user = _IUWork.FindOneByID<Users>(Parameter, CollectionNames.USERS).Result;


                        CommonViewModelForEmail RefuseInfo = new CommonViewModelForEmail
                        {
                            FileName = Parameters["FileName"].ToString(),
                            Idea = Parameters["Idea"].ToString(),
                            Longitude = Convert.ToDouble(Parameters["Longitude"]),
                            Latitude = Convert.ToDouble(Parameters["Latitude"]),
                            Name = user.FullName,
                            Address = user.Address,
                            Phone = user.Phone

                        };
                        if (RefuseInfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<CommonViewModelForEmail>(lstTemplate, RefuseInfo, Parameter, AdminEmail);
                            _IUWork.InsertOne<EmailNotification>(EmailNotification, Constants.CollectionNames.EmailNotification);
                        }
                    }


                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Reuse.ReuseEmailSendtoAdmin)
                    {
                        var user = _IUWork.FindOneByID<Users>(Parameter, CollectionNames.USERS).Result;


                        CommonViewModelForEmail RefuseInfo = new CommonViewModelForEmail
                        {
                            FileName = Parameters["FileName"].ToString(),
                            Idea = Parameters["Idea"].ToString(),
                            Longitude = Convert.ToDouble(Parameters["Longitude"]),
                            Latitude = Convert.ToDouble(Parameters["Latitude"]),
                            Name = user.FullName,
                            Address = user.Address,
                            Phone = user.Phone

                        };
                        if (RefuseInfo != null)
                        {
                            EmailNotification EmailNotification = PrepareEmailNotification<CommonViewModelForEmail>(lstTemplate, RefuseInfo, Parameter, AdminEmail);
                            _IUWork.InsertOne<EmailNotification>(EmailNotification, Constants.CollectionNames.EmailNotification);
                        }
                    }


                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Children.SendEmailToAdmin)
                    {
                        var user = _IUWork.FindOneByID<Users>(Parameter, CollectionNames.USERS).Result;


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
                            _IUWork.InsertOne<EmailNotification>(EmailNotification, Constants.CollectionNames.EmailNotification);
                        }
                    }

                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.Organization.SendEmailToAdmin)
                    {
                        var user = _IUWork.FindOneByID<Users>(Parameter, CollectionNames.USERS).Result;


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
                            _IUWork.InsertOne<EmailNotification>(EmailNotification, Constants.CollectionNames.EmailNotification);
                        }
                    }

                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.NGO.SendEmailToAdmin)
                    {
                        var user = _IUWork.FindOneByID<Users>(Parameter, CollectionNames.USERS).Result;


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
                            _IUWork.InsertOne<EmailNotification>(EmailNotification, Constants.CollectionNames.EmailNotification);
                        }
                    }


                    else if (Convert.ToUInt64(lstTemplate[0].EventId) == (long)NotificationEventConstants.School.SendEmailToAdmin)
                    {
                        var user = _IUWork.FindOneByID<Users>(Parameter, CollectionNames.USERS).Result;

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
                            _IUWork.InsertOne<EmailNotification>(EmailNotification, Constants.CollectionNames.EmailNotification);
                        }
                    }

                }
                return isAdded;
            }
            catch (Exception ex)
            {
                ServerResponse.ServiceResponse.LogError(ex);
                return false;
            }

        }


    }
}
