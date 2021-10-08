using System;
using System.Collections.Generic;
using DrTech.DAL;
using Microsoft.AspNetCore.Mvc;
using DrTech.Models;
using static DrTech.Common.Extentions.Constants;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using DrTech.Common.Helpers;
using DrTech.Common.ServerResponse;
using DrTech.Common.Enums;
using System.Security.Claims;
using DrTech.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using DrTech.Common.Extentions;
using MongoDB.Driver;
using System.Linq;
using DrTech.Services.Attribute;
using DrTech.Models.Common;
using DrTech.Notifications;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.ReportingServices.ReportProcessing.OnDemandReportObjectModel;

namespace DrTech.Services.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : BaseControllerBase
    {

        [HttpPost("RegistrationProcess")]
        public ResponseObject<Users> RegistrationProcess([FromBody]Users mdlUser)
        {
            try
            {
                List<FilterHelper> filter = new List<FilterHelper>
                {
                     new FilterHelper
                    {
                        Field = "Phone",
                        Value = mdlUser.Phone
                    }
                };
                var UserDetails = _IUWork.GetModelData<Users>(filter, CollectionNames.USERS).FirstOrDefault();
                if (UserDetails != null)
                {
                    if (UserDetails.Address != "" && UserDetails.Email != "")
                        return ServiceResponse.SuccessReponse(UserDetails, UserTypeEnum.RegisteredUser.GetDescription());
                    else
                        return ServiceResponse.SuccessReponse(UserDetails, UserTypeEnum.BasicUser.GetDescription());
                }
                else
                {
                    mdlUser.GreenPoints = 0;
                    mdlUser.IsActive = true;
                    mdlUser.Cash = 0;
                    mdlUser.UserRole = UserRoleTypeEnum.Mobile.GetDescription();
                    _IUWork.InsertOne(mdlUser, "Users");
                    SMSNotifyEvent _events = new SMSNotifyEvent();
                    _events.Parameters.Add("Email", "");
                    _events.AddSMSNotifyEvent((long)NotificationEventConstants.Users.SendSMSToUserForEntrolment, mdlUser.Phone);
                    NotifyEvent _event = new NotifyEvent();
                    _event.Parameters.Add("Email", "");
                    _event.AddNotifyEvent((long)NotificationEventConstants.Users.RegistrationProcessEmailSendToAdmin, mdlUser.Phone);
                    return ServiceResponse.SuccessReponse(mdlUser, UserTypeEnum.BasicUser.GetDescription());
                }
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<Users>(exp);
            }
        }
        [HttpPost("RegistrationForm")]
        public ResponseObject<bool> RegistrationForm([FromBody]Users mdlUser)
        {
            try
            {
                List<FilterHelper> filter = new List<FilterHelper>
                {
                     new FilterHelper
                    {
                        Field = "Email",
                        Value = mdlUser.Email
                    }
                };

                List<Users> lstUsers = _IUWork.GetModelData<Users>(filter, "Users");

                if (lstUsers.Count > 0)
                {
                    return ServiceResponse.ErrorReponse<bool>(MessageEnum.UserEmailAlreadyExists);
                }
                else
                {
                    if (mdlUser != null)
                    {
                        StringBuilder CodeModel = new StringBuilder();
                        CodeModel.Append(mdlUser.Email + ";");
                        CodeModel.Append(mdlUser.Phone + ";");
                        CodeModel.Append(mdlUser.FullName);
                        string QRCode = ""; //QRCodeTagHelper.QRCodeGenerator(CodeModel);

                        mdlUser.GreenPoints = 0;
                        mdlUser.IsActive = true;
                        mdlUser.Cash = 0;
                        mdlUser.QRCode = QRCode;
                        _IUWork.InsertOne(mdlUser, "Users");
                        return ServiceResponse.SuccessReponse(true, MessageEnum.DefaultSuccessMessage);
                    }
                    else
                    {
                        return ServiceResponse.ErrorReponse<bool>(MessageEnum.DefaultErrorMessage);
                    }
                }
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
        [HttpPost("UpdateRegistrationFormByUser")]
        public async Task<ResponseObject<Users>> UpdateRegistrationFormByUser(UserViewModel mdlUser)
        {
            try
            {
                Users mdlUsers = new Users();
                string fileName = "";

                if (mdlUser.File != null)
                    fileName = await SaveFile(mdlUser.File);

                var lstUsers = await _IUWork.FindOneByID<Users>(mdlUser.UserId, CollectionNames.USERS);

                if (lstUsers != null)
                {
                    if (string.IsNullOrEmpty(mdlUser.FullName))
                        mdlUser.FullName = lstUsers.FullName;
                    if (string.IsNullOrEmpty(mdlUser.Email))
                        mdlUser.Email = lstUsers.Email;
                    if (string.IsNullOrEmpty(mdlUser.Password))
                        mdlUser.Password = lstUsers.Password;
                    if (string.IsNullOrEmpty(mdlUser.Phone))
                        mdlUser.Phone = lstUsers.Phone;
                    if (string.IsNullOrEmpty(mdlUser.FileName))
                        mdlUser.FileName = lstUsers.FileName;
                    if (mdlUser.City == 0)
                        mdlUser.City = lstUsers.City;

                    if (string.IsNullOrEmpty(mdlUser.CityDescription))
                        mdlUser.CityDescription = lstUsers.CityDescription;
                }


                if (mdlUser.File != null)
                    mdlUser.FileName = fileName;

                StringBuilder CodeModel = new StringBuilder();
                // CodeModel.Append(mdlUser.Email + ";");
                CodeModel.Append(mdlUser.Phone);
                //CodeModel.Append(mdlUser.FullName);
                string QRCode = ""; //QRCodeTagHelper.QRCodeGenerator(CodeModel);

                string UserId = lstUsers?.Id.ToString();

                if (UserId == null)
                {
                    var model = new Users
                    {
                        FullName = mdlUser.FullName,
                        Email = mdlUser.Email,
                        Password = mdlUser.Password,
                        Address = mdlUser.Address,
                        FileName = mdlUser.FileName,
                        Phone = mdlUser.Phone,
                        QRCode = QRCode,
                        Latitude = mdlUser.Latitude,
                        Longitude = mdlUser.Longitude,
                        City = mdlUser.City,
                        CityDescription = mdlUser.CityDescription
                    };
                    await _IUWork.InsertOne(model, CollectionNames.USERS);

                    return ServiceResponse.SuccessReponse(model, MessageEnum.UserProfileUpdated);
                }
                else
                {
                    var update = Builders<Users>.Update
                          .Set(o => o.FullName, mdlUser.FullName)
                          .Set(g => g.Email, mdlUser.Email)
                          .Set(d => d.Password, mdlUser.Password)
                          .Set(a => a.Address, mdlUser.Address)
                          .Set(p => p.FileName, mdlUser.FileName)
                          .Set(q => q.Phone, mdlUser.Phone)
                          .Set(q => q.QRCode, QRCode)
                          .Set(q => q.Longitude, mdlUser.Longitude)
                          .Set(q => q.Latitude, mdlUser.Latitude)
                          .Set(i => i.CityDescription, mdlUser.CityDescription);

                    bool counts = _IUWork.UpdateStatus(UserId, update, CollectionNames.USERS);

                    var UpdatedUsers = _IUWork.FindOneByID<Users>(UserId, CollectionNames.USERS).Result;


                    int _min = 1000;
                    int _max = 9999;
                    Random random = new Random();
                    Int32 number = random.Next(_min, _max);

                    UpdatedUsers.SMSCode = number;

                    SMSNotifyEvent _events = new SMSNotifyEvent();
                    _events.Parameters.Add("SMSCode", number);
                    _events.AddSMSNotifyEvent((long)NotificationEventConstants.Users.SendVerificationCodeSMS, UpdatedUsers.Phone);

                    NotifyEvent _event = new NotifyEvent();
                    _event.Parameters.Add("Email", UpdatedUsers.Email);
                    _event.AddNotifyEvent((long)NotificationEventConstants.Users.RegistrationCompletedEmailSendToAdmin, UpdatedUsers.Id.ToString());

                    return ServiceResponse.SuccessReponse(UpdatedUsers, MessageEnum.UserProfileUpdated);
                }
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<Users>(exp);
            }
        }
        [HttpPost("Login")]
        public ResponseObject<TokenViewModel> Login([FromBody]Users mdlUser)
        {
            try
            {
                if (mdlUser.Email == null || mdlUser.Password == "")
                {
                    return ServiceResponse.ErrorReponse<TokenViewModel>(MessageEnum.DefaultParametersCanNotBeNull);

                }
                else
                {
                    List<FilterHelper> filter = new List<FilterHelper>
                    {
                        new FilterHelper
                        {
                            Field = "Email",
                            Value = mdlUser.Email
                        },
                        new FilterHelper
                        {
                            Field = "Password",
                            Value = mdlUser.Password
                        }
                    };

                    var user = _IUWork.GetModelData<Users>(filter, CollectionNames.USERS)?.FirstOrDefault();

                    if (user != null && (user.UserRole == UserRoleTypeEnum.Admin.GetDescription() || user.UserRole == UserRoleTypeEnum.NGO.GetDescription()))
                    {
                        var token = JwtTokenBuilder(user);
                        return ServiceResponse.SuccessReponse(new TokenViewModel
                        {
                            Token = token,
                            UserId = user.Id.ToString(),
                            FullName = user.FullName,
                            Address = user.Address,
                            Email = user.Email,
                            Phone = user.Phone,
                            FileName = user.FileName,
                            Longitude = user.Longitude,
                            Latitude = user.Latitude,
                            QRCode = user.QRCode,
                            GreenPoints = user.GreenPoints,
                            Role = user.UserRole,
                            IsVerified = user.IsVerified
                        }, MessageEnum.UserAuthorizedSuccessFully);
                    }
                    else
                    {
                        return ServiceResponse.ErrorReponse<TokenViewModel>(MessageEnum.UserCredentialsAreNotCorrect);
                    }
                }
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<TokenViewModel>(exp);
            }
        }
        private string JwtTokenBuilder(Users user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("Id", user.Id.ToString()),
                new Claim("Email", user.Email),
                new Claim("Role", user.UserRole)
            };

            var dd = AppSettingsHelper.GetAttributeValue(AppSettings.SECURITY_SECTION, AppSettings.SECURITY_KEY);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(dd));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(issuer: AppSettingsHelper.GetAttributeValue(AppSettings.SECURITY_SECTION, AppSettings.SECURITY_ISSUER),
                                                audience: AppSettingsHelper.GetAttributeValue(AppSettings.SECURITY_SECTION, AppSettings.SECURITY_AUDIENCE),
                                                signingCredentials: credentials,
                                                expires: DateTime.Now.AddYears(1),
                                                claims: claims);

            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return token;

        }
        [HttpGet("GetUsersGreenPointStatus")]
        public ResponseObject<List<MapMarker>> GetUsersGreenPointStatus()
        {
            try
            {
                var lstUsersDetails = _IUWork.GetModelData<Users>("Users")?.ConvertAll(p => new MapMarker
                {
                    Cash = p.Cash,
                    GreenPoints = p.GreenPoints,
                    Latitude = p.Latitude,
                    Longitude = p.Longitude,
                    Status = p.Status
                });
                if (lstUsersDetails?.Count > 0)
                {
                    return ServiceResponse.SuccessReponse(lstUsersDetails, MessageEnum.DefaultSuccessMessage);
                }
                else
                {
                    return ServiceResponse.SuccessReponse(lstUsersDetails, MessageEnum.DefaultErrorMessage);
                }
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<MapMarker>>(exp);
            }
        }
        [HttpGet("ForgotPassword")]
        public ResponseObject<bool> ForgotPassword(string Phone)
        {
            try
            {
                if (Phone == "")
                {
                    return ServiceResponse.ErrorReponse<bool>(MessageEnum.UserForgotParameterNotnull);
                }

                else
                {
                    List<FilterHelper> filter = new List<FilterHelper>
                    {
                        new FilterHelper
                        {
                            Field = "Phone",
                            Value = Phone
                        }
                    };

                    List<Users> lstUsers = _IUWork.GetModelData<Users>(filter, CollectionNames.USERS);

                    if (lstUsers.Count > 0)
                    {
                        foreach (Users mdlUser in lstUsers)
                        {

                            SMSNotifyEvent _events = new SMSNotifyEvent();
                            _events.Parameters.Add("Email", mdlUser.Email);
                            _events.AddSMSNotifyEvent((long)NotificationEventConstants.Users.SMSSendtoUserForgotPassword, mdlUser.Phone);

                            NotifyEvent _event = new NotifyEvent();
                            _event.Parameters.Add("Email", mdlUser.Email);
                            _event.AddNotifyEvent((long)NotificationEventConstants.Users.EmailSendtoUserForgotPassword, Convert.ToString(mdlUser.Id));
                        }
                        return ServiceResponse.SuccessReponse(true, MessageEnum.PasswordSentToUserViaEmail);
                    }
                    else
                    {
                        return ServiceResponse.ErrorReponse<bool>(MessageEnum.EmailNotFound);
                    }

                    //return ServiceResponse.SuccessReponse(true, MessageEnum.PasswordSentToUserViaEmail);
                }
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }

        }
        [Authorize]
        [HttpPost("UpdateRegistrationForm")]
        public async Task<ResponseObject<TokenViewModel>> UpdateRegistrationForm(UserViewModel mdlUser)
        {
            try
            {
                string fileName = string.Empty;

                if (mdlUser.File != null)
                    fileName = await SaveFile(mdlUser.File);

                // GetLoggedInUserId()
                var User = _IUWork.FindOneByID<Users>(GetLoggedInUserId(), CollectionNames.USERS).Result;

                string OldPhone = User.Phone;

                if (string.IsNullOrEmpty(mdlUser.FullName))
                    mdlUser.FullName = User.FullName;
                if (string.IsNullOrEmpty(mdlUser.Email))
                    mdlUser.Email = User.Email;
                if (string.IsNullOrEmpty(mdlUser.Password))
                    mdlUser.Password = User.Password;
                if (string.IsNullOrEmpty(mdlUser.Phone))
                    mdlUser.Phone = User.Phone;

                if (mdlUser.File != null)
                    mdlUser.FileName = fileName;
                else if (string.IsNullOrEmpty(mdlUser.FileName))
                    mdlUser.FileName = User.FileName;

                if (mdlUser.Latitude == 0)
                    mdlUser.Latitude = User.Latitude;

                if (mdlUser.Longitude == 0)
                    mdlUser.Longitude = User.Longitude;

                if (mdlUser.City == 0)
                    mdlUser.City = User.City;

                if (string.IsNullOrEmpty(mdlUser.CityDescription))
                    mdlUser.CityDescription = User.CityDescription;

                //StringBuilder CodeModel = new StringBuilder();
                //CodeModel.Append(mdlUser.Email + ";");
                //CodeModel.Append(mdlUser.Phone + ";");
                //CodeModel.Append(mdlUser.FullName);
                //string QRCode = QRCodeTagHelper.QRCodeGenerator(CodeModel);


                var update = Builders<Users>.Update
                    .Set(o => o.FullName, mdlUser.FullName)
                    .Set(g => g.Email, mdlUser.Email)
                    .Set(d => d.Password, mdlUser.Password)
                    .Set(a => a.Address, mdlUser.Address)
                    .Set(p => p.FileName, mdlUser.FileName)
                    .Set(q => q.Phone, mdlUser.Phone)
                    .Set(q => q.Longitude, mdlUser.Longitude)
                    .Set(q => q.Latitude, mdlUser.Latitude)
                    .Set(l => l.City, mdlUser.City)
                    .Set(i => i.CityDescription, mdlUser.CityDescription);

                bool counts = _IUWork.UpdateStatus(GetLoggedInUserId(), update, CollectionNames.USERS);
                var UpdatedUsers = _IUWork.FindOneByID<Users>(GetLoggedInUserId(), CollectionNames.USERS).Result;

                int _min = 1000;
                int _max = 9999;
                Random random = new Random();
                Int32 number = random.Next(_min, _max);



                if (mdlUser.SMSCodeRequired == true)
                {
                    UpdatedUsers.SMSCode = number;

                    SMSNotifyEvent _events = new SMSNotifyEvent();
                    _events.Parameters.Add("SMSCode", UpdatedUsers.SMSCode);
                    _events.AddSMSNotifyEvent((long)NotificationEventConstants.Users.SendVerificationCodeSMS, UpdatedUsers.Phone);
                }

                if (UpdatedUsers != null)
                {
                    var token = JwtTokenBuilder(UpdatedUsers);

                    //   return ServiceResponse.SuccessReponse(new TokenViewModel { UserId = user.Id.ToString(), UserName = user.FullName, FileName = user.ImagePath, Token = token }, MessageEnum.UserAuthorizedSuccessFully);
                    return ServiceResponse.SuccessReponse(new TokenViewModel
                    {
                        Token = token,
                        UserId = UpdatedUsers.Id.ToString(),
                        FullName = UpdatedUsers.FullName,
                        Address = UpdatedUsers.Address,
                        Email = UpdatedUsers.Email,
                        Phone = UpdatedUsers.Phone,
                        FileName = UpdatedUsers.FileName,
                        Longitude = UpdatedUsers.Longitude,
                        Latitude = UpdatedUsers.Latitude,
                        QRCode = UpdatedUsers.QRCode,
                        GreenPoints = UpdatedUsers.GreenPoints,
                        City = UpdatedUsers.City,
                        CityDescription = UpdatedUsers.CityDescription,
                        Password = UpdatedUsers.Password,
                        SMSCode = UpdatedUsers.SMSCode
                    }, MessageEnum.UserProfileUpdated);
                }
                else
                {
                    return ServiceResponse.ErrorReponse<TokenViewModel>(MessageEnum.UserDetailNotFoundSuccessfully);
                }

                // return ServiceResponse.SuccessReponse(UpdatedUsers, MessageEnum.UserProfileUpdated);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<TokenViewModel>(exp);
            }
        }
        [HttpPost("LoginUser")]
        public ResponseObject<TokenViewModel> LoginUser([FromBody]Users mdlUser)
        {
            try
            {
                if (mdlUser.Phone == "" || mdlUser.Password == "")
                {
                    return ServiceResponse.ErrorReponse<TokenViewModel>(MessageEnum.DefaultParametersCanNotBeNull);

                }
                else
                {
                    List<FilterHelper> filter = new List<FilterHelper>
                    {
                        new FilterHelper
                        {
                            Field = "Phone",
                            Value = mdlUser.Phone
                        },
                        new FilterHelper
                        {
                            Field = "Password",
                            Value = mdlUser.Password
                        }
                    };

                    var user = _IUWork.GetModelData<Users>(filter, CollectionNames.USERS)?.FirstOrDefault();

                    if (user != null)
                    {
                        var token = JwtTokenBuilder(user);
                       
                        //   return ServiceResponse.SuccessReponse(new TokenViewModel { UserId = user.Id.ToString(), UserName = user.FullName, FileName = user.ImagePath, Token = token }, MessageEnum.UserAuthorizedSuccessFully);
                        return ServiceResponse.SuccessReponse(new TokenViewModel
                        {
                            Token = token,
                            UserId = user.Id.ToString(),
                            FullName = user.FullName,
                            Address = user.Address,
                            Email = user.Email,
                            Phone = user.Phone,
                            FileName = user.FileName,
                            Longitude = user.Longitude,
                            Latitude = user.Latitude,
                            QRCode = user.QRCode,
                            GreenPoints = user.GreenPoints,
                            City = user.City,
                            CityDescription = user.CityDescription,
                            IsVerified = user.IsVerified
                        }, MessageEnum.UserAuthorizedSuccessFully);

                     
                    }
                    else
                    {
                        return ServiceResponse.ErrorReponse<TokenViewModel>(MessageEnum.UserCredentialsAreNotCorrect);
                    }
                }
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<TokenViewModel>(exp);
            }

        }

        [Auth(UserRoleTypeEnum.Admin)]
        [HttpGet("GetUserList")]
        public async Task<ResponseObject<List<UserListViewModel>>> GetUserList(string type)
        {
            try
            {
                var users = _IUWork.GetModelData<Users>(CollectionNames.USERS);

                users = type == "registered" ? users?.FindAll(p => (p.UserRole != UserRoleTypeEnum.Admin.GetDescription() && p.UserRole != UserRoleTypeEnum.NGO.GetDescription()) && (!string.IsNullOrEmpty(p.Email) && !string.IsNullOrEmpty(p.Phone)))
                                            : users?.FindAll(p => (p.UserRole != UserRoleTypeEnum.Admin.GetDescription() && p.UserRole != UserRoleTypeEnum.NGO.GetDescription()) && (string.IsNullOrEmpty(p.Email) || string.IsNullOrEmpty(p.Phone)));

                users = users?.ToSortByCreationDateDescendingOrder();

                var list = users?.ConvertAll(p => new UserListViewModel
                {
                    MemberSince = p.CreatedAt.ToString("mon dd, yyyy"),
                    FullName = p.FullName,
                    Email = p.Email,
                    Phone = p.Phone,
                    Latitude = p.Latitude,
                    Longitude = p.Longitude,
                    UserId = p.Id.ToString(),
                    Address = p.Address,
                    GreenPoints = p.GreenPoints,
                    BinCount = (int)p.BuyBinDetails?.Count(),
                    RecycleCount = 0,
                    ReduceCount = (int)p.Reduce?.Count(),
                    RefuseCount = (int)p.Refuse?.Count(),
                    ReplantCount = (int)p.Replant?.Count(),
                    ReportCount = 0,
                    ReuseCount = (int)p.Reuse?.Count(),
                    RegiftCount = 0
                });


                //if (type == "registered")
                //{
                //    foreach (var item in list)
                //    {
                //        var regiftCount = (int)_IUWork.GetModelByUserID<Regift>(item.UserId, CollectionNames.REGIFT)?.Count();
                //        var reportCount = (int)_IUWork.GetModelByUserID<Report>(item.UserId, CollectionNames.Report)?.Count();
                //        var recycleCount = (int)_IUWork.GetModelByUserID<MrClean>(item.UserId, CollectionNames.RECYCLE)?.Count();

                //        item.RecycleCount = recycleCount;
                //        item.ReportCount = reportCount;
                //        item.RegiftCount = regiftCount;
                //    }

                //}
                return ServiceResponse.SuccessReponse(list, MessageEnum.UserProfileUpdated);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<UserListViewModel>>(exp);
            }
        }


        [HttpPost("ReportAProblem")]
        public async Task<ResponseObject<bool>> ReportAProblem(ReportProblemViewModel mdlUser)
        {
            try
            {

                //NotifyEvent _event = new NotifyEvent();
                //_event.Parameters.Add("Email", mdlUser.Email);
                //_event.Parameters.Add("Phone", mdlUser.Phone);
                //_event.Parameters.Add("Problem", mdlUser.Problem);
                //_event.Parameters.Add("Subject", mdlUser.Subject);
                //_event.AddNotifyEvent((long)NotificationEventConstants.Common.EmailSendtoUserReportAProblem, "");



                NotifyEvent _events = new NotifyEvent();
                _events.Parameters.Add("Email", "info@amalforlife.com");
                _events.Parameters.Add("Phone", mdlUser.Phone);
                _events.Parameters.Add("Description", mdlUser.Problem);
                _events.Parameters.Add("IssueType", mdlUser.Subject);
                _events.AddNotifyEvent((long)NotificationEventConstants.Common.EmailSendtoAdminReportAProblem, "");


                SMSNotifyEvent _event = new SMSNotifyEvent();
                _event.Parameters.Add("Phone", mdlUser.Phone);
                _event.AddSMSNotifyEvent((long)NotificationEventConstants.Common.SMSSendtoUserForReportAProblem, GetLoggedInUserId());

                return ServiceResponse.SuccessReponse(true, MessageEnum.DefaultSuccessMessage);

            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpPost("InviteAFriend")]
        public async Task<ResponseObject<bool>> InviteAFriend(string email)
        {
            try
            {
                var User = _IUWork.FindOneByID<Users>(GetLoggedInUserId(), CollectionNames.USERS).Result;

                NotifyEvent _event = new NotifyEvent();
                _event.AddNotifyEvent((long)NotificationEventConstants.InviteFriend.EmailSendtoFriend, email);
                return ServiceResponse.SuccessReponse(true, MessageEnum.DefaultSuccessMessage);

            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [Authorize]
        [HttpGet("GetUserDetail")]
        public async Task<ResponseObject<UserDetailViewModel>> GetUserDetail(string id)
        {
            try
            {
                var user = await _IUWork.FindOneByID<Users>(id, CollectionNames.USERS);

                if (user != null)
                {
                    var regiftCount = (int)_IUWork.GetModelByUserID<Regift>(user.Id.ToString(), CollectionNames.REGIFT)?.Count();
                    var reportCount = (int)_IUWork.GetModelByUserID<Report>(user.Id.ToString(), CollectionNames.Report)?.Count();
                    var recycleCount = (int)_IUWork.GetModelByUserID<MrClean>(user.Id.ToString(), CollectionNames.RECYCLE)?.Count();

                    return ServiceResponse.SuccessReponse(new UserDetailViewModel
                    {
                        MemberSince = user.CreatedAt.ToString("MMM dd, yyyy"),
                        FullName = user.FullName,
                        Email = user.Email,
                        Address = user.Address,
                        FileName = user.FileName,
                        Latitude = user.Latitude,
                        Longitude = user.Longitude,
                        Phone = user.Phone,
                        UserType = string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Phone) ? "Basic" : "Registered",

                        GreenPoints = user.GreenPoints,
                        BinCount = (int)user.BuyBinDetails?.Count(),
                        RecycleCount = recycleCount,
                        ReduceCount = (int)user.Reduce?.Count(),
                        RefuseCount = (int)user.Refuse?.Count(),
                        ReplantCount = (int)user.Replant?.Count(),
                        ReportCount = reportCount,
                        ReuseCount = (int)user.Reuse?.Count(),
                        RegiftCount = regiftCount

                    }, MessageEnum.UserDetailFoundSuccessfully);
                }
                return ServiceResponse.SuccessReponse<UserDetailViewModel>(null, MessageEnum.UserDetailNotFoundSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<UserDetailViewModel>(exp);
            }
        }


        [HttpPost("LoginThroughSocialMedia")]
        public async Task<ResponseObject<TokenViewModel>> LoginThroughSocialMedia(UserViewModel mdlUser)
        {
            try
            {
                bool IsTrue = false;
                Users user = new Users();
                SocialMedia sm = new SocialMedia();
                if (mdlUser != null)
                {
                    sm.SocialMediaType = mdlUser.SocialMediaType;
                    if (mdlUser.OSType == OSTypeEnum.Andriod.GetDescription())
                        mdlUser.OSType = OSTypeEnum.Andriod.GetDescription();
                    else
                        mdlUser.OSType = OSTypeEnum.ios.GetDescription();

                    List<FilterHelper> filter = new List<FilterHelper>
                    {
                        new FilterHelper
                        {
                            Field = "SocialMediaId",
                            Value = mdlUser.SocialMediaId
                        }
                    };

                    user = _IUWork.GetModelData<Users>(filter, CollectionNames.USERS)?.FirstOrDefault();

                    if (user != null)
                    {

                        string UserId = user?.Id.ToString();


                        if (string.IsNullOrEmpty(mdlUser.FullName))
                            mdlUser.FullName = user.FullName;

                        if (string.IsNullOrEmpty(mdlUser.Email))
                            mdlUser.Email = user.Email;

                        if (string.IsNullOrEmpty(mdlUser.FileName))
                            mdlUser.FileName = user.FileName;

                        if (string.IsNullOrEmpty(mdlUser.Phone))
                            mdlUser.Phone = user.Phone;

                        if (string.IsNullOrEmpty(mdlUser.SocialMediaId))
                            mdlUser.SocialMediaId = user.SocialMediaId;

                        if (string.IsNullOrEmpty(mdlUser.DeviceId))
                            mdlUser.DeviceId = user.DeviceId;

                        if (mdlUser.Latitude == 0)
                            mdlUser.Latitude = user.Latitude;

                        if (mdlUser.Longitude == 0)
                            mdlUser.Longitude = user.Longitude;

                        if (mdlUser.City == 0)
                            mdlUser.City = user.City;

                        if (string.IsNullOrEmpty(mdlUser.CityDescription))
                            mdlUser.CityDescription = user.CityDescription;

                        var update = Builders<Users>.Update
                          .Set(o => o.FullName, mdlUser.FullName)
                          .Set(g => g.Email, mdlUser.Email)
                          .Set(p => p.FileName, mdlUser.FileName)
                          .Set(q => q.Phone, mdlUser.Phone)
                          .Set(u => u.OSType, mdlUser.OSType)
                          .Set(l => l.Longitude, mdlUser.Longitude)
                          .Set(m => m.Latitude, mdlUser.Latitude)
                          .Set(x => x.SocialMediaId, mdlUser.SocialMediaId)
                          .Set(t => t.DeviceId, mdlUser.DeviceId)
                          .Set(b => b.City, mdlUser.City)
                          .Set(i => i.CityDescription, mdlUser.CityDescription);
                        bool counts = _IUWork.UpdateStatus(UserId, update, CollectionNames.USERS);
                    }
                    else
                    {

                        List<FilterHelper> Phone = new List<FilterHelper>
                    {
                        new FilterHelper
                        {
                            Field = "Phone",
                            Value = mdlUser.Phone
                        }
                    };

                        user = _IUWork.GetModelData<Users>(filter, CollectionNames.USERS)?.FirstOrDefault();

                        if (user != null)
                        {
                            string UserId = user?.Id.ToString();

                            var update = Builders<Users>.Update
                                                    .Set(o => o.FullName, mdlUser.FullName)
                                                    .Set(g => g.Email, mdlUser.Email)
                                                    .Set(p => p.FileName, mdlUser.FileName)
                                                    .Set(q => q.Phone, mdlUser.Phone)
                                                    .Set(u => u.OSType, mdlUser.OSType)
                                                    .Set(l => l.Longitude, mdlUser.Longitude)
                                                    .Set(m => m.Latitude, mdlUser.Latitude)
                                                    .Set(x => x.SocialMediaId, mdlUser.SocialMediaId)
                                                    .Set(t => t.DeviceId, mdlUser.DeviceId)
                                                    .Set(b => b.City, mdlUser.City)
                                                    .Set(i => i.CityDescription, mdlUser.CityDescription);
                            bool counts = _IUWork.UpdateStatus(UserId, update, CollectionNames.USERS);

                        }
                        else
                        {
                            IsTrue = true;
                            Users UserProperty = new Users();
                            UserProperty.FullName = mdlUser.FullName;
                            UserProperty.FileName = mdlUser.FileName;
                            UserProperty.DeviceId = mdlUser.DeviceId;
                            UserProperty.OSType = mdlUser.OSType;
                            UserProperty.Email = mdlUser.Email;
                            UserProperty.Phone = mdlUser.Phone;
                            UserProperty.Longitude = mdlUser.Longitude;
                            UserProperty.Latitude = mdlUser.Latitude;
                            UserProperty.SocialMediaId = mdlUser.SocialMediaId;
                            UserProperty.City = mdlUser.City;
                            UserProperty.CityDescription = mdlUser.CityDescription;
                            await _IUWork.InsertOne(UserProperty, "Users");
                        }
                    }


                }
                else
                    return ServiceResponse.ErrorReponse<TokenViewModel>(MessageEnum.DefaultParametersCanNotBeNull);


                List<FilterHelper> filters = new List<FilterHelper>
                    {
                        new FilterHelper
                        {
                            Field = "SocialMediaId",
                            Value = mdlUser.SocialMediaId
                        }
                    };

                user = _IUWork.GetModelData<Users>(filters, CollectionNames.USERS)?.FirstOrDefault();
                string ID = user?.Id.ToString();
                if (IsTrue == true)
                    await _IUWork.AddSubDocument<Users, SocialMedia>(ID, sm, CollectionNames.USERS, CollectionNames.SocialMedia);

                var token = JwtTokenBuilder(user);
                return ServiceResponse.SuccessReponse(new TokenViewModel
                {
                    Token = token,
                    UserId = user.Id.ToString(),
                    FullName = user.FullName,
                    Address = user.Address,
                    Email = user.Email,
                    Phone = user.Phone,
                    FileName = user.FileName,
                    Longitude = user.Longitude,
                    Latitude = user.Latitude,
                    QRCode = user.QRCode,
                    GreenPoints = user.GreenPoints,
                    City = user.City,
                    CityDescription = user.CityDescription,
                    Password = user.Password,
                    IsVerified = user.IsVerified
                }, MessageEnum.UserAuthorizedSuccessFully);

            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<TokenViewModel>(exp);
            }
        }


        [HttpGet("MigrateDataMongotoSQL")]
        public async Task<ResponseObject<string>> MigrateDataMongotoSQL()
        {
            int count = 0;
            List<string> missingType = new List<string>();
            List<string> subMissingType = new List<string>();
            try
            {

                string apiUrl = "https://amalforlife.azurewebsites.net/api/Users/GetUsersForWeb";
                //string insert = "http://10.200.10.33:2018/api/Users/InsertUsers";

                //string recycleurl = "http://10.200.10.33:2018/api/users/getallrecycle";

                //string reporturl = "http://10.200.10.33:2018/api/users/getallreport";

                //string regifteurl = "http://10.200.10.33:2018/api/users/getallregift";

                //string emailurl = "http://10.200.10.33:2018/api/users/getallemails";


                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization
                             = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjVjNTgzYThiNzRjNWZjZDY5MDBmNTljNyIsIkVtYWlsIjoiYW1pci5uYXppckBkcnRlY2hway5jb20iLCJSb2xlIjoiTW9iaWxlIFVzZXIiLCJleHAiOjE1ODA4MjIxMDQsImlzcyI6Imh0dHBzOi8vYW1hbGZvcmxpZmUuY29tLyIsImF1ZCI6Imh0dHBzOi8vYW1hbGZvcmxpZmUuY29tLyJ9.Kn7tTjq7xVF-qIjZZRO0mMVEcNEcECBUDE-yRYd-G70");
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    //HttpResponseMessage Recycleresponse = await client.GetAsync(recycleurl);

                    //HttpResponseMessage Regiftresponse = await client.GetAsync(regifteurl);

                    //HttpResponseMessage Reportresponse = await client.GetAsync(reporturl);

                    //HttpResponseMessage Emailresponse = await client.GetAsync(emailurl);

                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        // var recdata = await Recycleresponse.Content.ReadAsStringAsync();
                        //  var regiftdata = await Regiftresponse.Content.ReadAsStringAsync();
                        //   var reportdata = await Reportresponse.Content.ReadAsStringAsync();
                        //  var Emaildata = await Emailresponse.Content.ReadAsStringAsync();

                        //var userlist = JsonConvert.DeserializeObject<Response>(data);

                        //List<UserDataViewModel> UserList = JsonConvert.DeserializeObject<List<UserDataViewModel>>(userlist.Data.ToString());

                        //foreach (var item in UserList)
                        //{
                        //    Users mdlUser = new Users();
                        //    await _IUWork.InsertOne(item, "Users");
                        //}
                        List<Users> users = _Source.GetModelData<Users>(CollectionNames.USERS);


                        string str = "df";
                        new BaseControllerBase(str);
                        foreach (var item in users)
                        {
                            Users mdlUser = new Users();
                            mdlUser.FullName = item.FullName;
                            mdlUser.Address = item.Address;
                            mdlUser.Cash = item.Cash;
                            mdlUser.City = item.City;
                            mdlUser.CityDescription = item.CityDescription;
                            mdlUser.CreatedAt = item.CreatedAt;
                            mdlUser.datetime = item.datetime;
                            mdlUser.DeviceId = item.DeviceId;
                           // mdlUser.Email = item.Email;
                           // mdlUser.FileName = item.FileName;
                           // mdlUser.GreenPoints = item.GreenPoints;
                           // mdlUser.IsActive = item.IsActive;
                           // mdlUser.IsVerified = item.IsVerified;
                           // mdlUser.Latitude = item.Latitude;
                           // mdlUser.Longitude = item.Longitude;
                           // mdlUser.Password = item.Password;
                           // mdlUser.Phone = item.Phone;
                           // mdlUser.UserRole = item.UserRole;
                           // mdlUser.UserTypeId = item.UserTypeId;
                           // mdlUser.Status = item.Status;
                           // mdlUser.SocialMediaId = item.SocialMediaId;
                           //// mdlUser.SocialMedia = item.SocialMedia;
                           // mdlUser.SMSCodeRequired = item.SMSCodeRequired;
                           // mdlUser.SMSCode = item.SMSCode;
                           // mdlUser.QRCode = item.QRCode;
                           // mdlUser.Id = item.Id;
                             await _IUWork.InsertOne(mdlUser, "LiveUserData");

                            foreach (var reuse in item.Reuse)
                            {
                                await _IUWork.AddSubDocument<Users, Reuse>(reuse.UserId, reuse, CollectionNames.USERS, CollectionNames.REUSE);
                            }

                            foreach (var redue in item.Reduce)
                            {
                                await _IUWork.AddSubDocument<Users, Reduce>(redue.UserId, redue, CollectionNames.USERS, CollectionNames.REUSE);
                            }

                            foreach (var refuse in item.Refuse)
                            {
                                await _IUWork.AddSubDocument<Users, Refuse>(refuse.UserId, refuse, CollectionNames.USERS, CollectionNames.REUSE);
                            }

                            foreach (var replant in item.Replant)
                            {
                                await _IUWork.AddSubDocument<Users, Replant>(replant.UserId, replant, CollectionNames.USERS, CollectionNames.REUSE);
                            }

                            foreach (var report in item.Report)
                            {
                                await _IUWork.AddSubDocument<Users, Report>(report.UserId, report, CollectionNames.USERS, CollectionNames.REUSE);
                            }


                        }

                    }


                }
                return ServiceResponse.SuccessReponse("", MessageEnum.UserProfileUpdated);
            }

            catch (Exception exp)
            {

                return ServiceResponse.ErrorReponse<string>(exp + count.ToString());
            }

        }
        public class Response
        {
            public int StatusCode { get; set; }
            public string StatusMessage { get; set; }
            //public bool IsSuccess { get; set; }
            public object Data { get; set; }
        }


        [HttpGet("GetUsersForWeb")]
        public async Task<ResponseObject<List<Users>>> GetUsersForWeb()
        {
            try
            {
                var users = _IUWork.GetModelData<Users>(CollectionNames.USERS);

                //users = users?.ToSortByCreationDateDescendingOrder();
                //var list = users?.ConvertAll(p => new UserViewModel
                //{
                //    FullName = p.FullName,
                //    Email = p.Email,
                //    Phone = p.Phone,
                //    Latitude = p.Latitude,
                //    Longitude = p.Longitude,
                //    UserId = p.Id.ToString(),
                //    Address = p.Address,
                //    GreenPoints = p.GreenPoints
                //});
                return ServiceResponse.SuccessReponse(users, MessageEnum.UserProfileUpdated);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Users>>(exp);
            }
        }

        [AllowAnonymous]
        [HttpGet("GetMyGPN")]
        public async Task<ResponseObject<List<MyGPNModel>>> GetMyGPN()
        {
            try
            {
                string UserID = GetLoggedInUserId();

                Users user = await _IUWork.FindOneByID<Users>(UserID, Constants.CollectionNames.USERS);

                List<MyGPNModel> lstMyGPNModel = new List<MyGPNModel>();

                if (user.children != null)
                {
                    foreach (Child child in user.children)
                    {

                        string SchoolID = child.SchoolId;

                        if (lstMyGPNModel.Any(d => d.ID == SchoolID && d.Type == GPNTypes.SCHOOL))
                            continue;


                        Schools school = await _IUWork.FindOneByID<Schools>(SchoolID, Constants.CollectionNames.SCHOOL);

                        MyGPNModel gpn = new MyGPNModel();
                        gpn.ID = school.Id.ToString();
                        gpn.FileName = school.FileName;
                        gpn.Name = school.Name;
                        gpn.GreenPoints = school.GreenPoints;
                        gpn.Level = "";
                        gpn.Type = Constants.GPNTypes.SCHOOL;

                        lstMyGPNModel.Add(gpn);

                    }
                }


                if (user.Employments != null)
                {
                    foreach (Employment emp in user.Employments)
                    {

                        if (lstMyGPNModel.Any(d => d.ID == emp.OrgnizationId && d.Type == GPNTypes.ORGANIZATION))
                            continue;

                        Organization org = await _IUWork.FindOneByID<Organization>(emp.OrgnizationId, Constants.CollectionNames.Organization);

                        MyGPNModel gpn = new MyGPNModel();
                        gpn.ID = org.Id.ToString();
                        gpn.FileName = org.FileName;
                        gpn.Name = org.OrgnizationName;
                        gpn.GreenPoints = org.OrgGreenPoints;
                        gpn.Level = "";
                        gpn.Type = Constants.GPNTypes.ORGANIZATION;

                        lstMyGPNModel.Add(gpn);


                    }
                }

                if (user.Memberships != null)
                {
                    foreach (Members member in user.Memberships)
                    {
                        if (lstMyGPNModel.Any(d => d.ID == member.NGOId && d.Type == GPNTypes.NGO))
                            continue;

                        if (member.NGOId != "undefined" && member.NGOId != "")
                        {

                            NGO ngo = await _IUWork.FindOneByID<NGO>(member.NGOId, Constants.CollectionNames.NGO);

                            MyGPNModel gpn = new MyGPNModel();
                            gpn.ID = ngo.Id.ToString();
                            gpn.FileName = ngo.FileName;
                            gpn.Name = ngo.Name;
                            gpn.GreenPoints = ngo.GreenPoints;
                            gpn.Level = "";
                            gpn.Type = Constants.GPNTypes.NGO;

                            lstMyGPNModel.Add(gpn);
                        }
                    }

                }

                lstMyGPNModel = lstMyGPNModel.Distinct().ToList<MyGPNModel>();

                return ServiceResponse.SuccessReponse(lstMyGPNModel, MessageEnum.UserProfileUpdated);

            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<MyGPNModel>>(exp);
            }

        }

        [AllowAnonymous]
        [HttpGet("GetGPNContacts")]
        public ResponseObject<List<Users>> GetGPNContacts(string ID, string GPNType)
        {
            try
            {

                List<Users> lstUsers = _IUWork.GetModelData<Users>(Constants.CollectionNames.USERS);

                if (GPNType == Constants.GPNTypes.SCHOOL)
                    lstUsers = lstUsers.Where(d => d.children.Any(c => c.SchoolId == ID)).ToList();
                else if (GPNType == Constants.GPNTypes.ORGANIZATION)
                    lstUsers = lstUsers.Where(d => d.Employments.Any(c => c.OrgnizationId == ID)).ToList();
                else if (GPNType == Constants.GPNTypes.NGO)
                    lstUsers = lstUsers.Where(d => d.Memberships.Any(c => c.NGOId == ID)).ToList();

                lstUsers = lstUsers.OrderByDescending(d => d.GreenPoints).ToList();

                return ServiceResponse.SuccessReponse(lstUsers, MessageEnum.UserProfileUpdated);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Users>>(exp);
            }
        }


        [HttpPost("InsertUsers")]
        public async Task<ResponseObject<Users>> InsertUsers(Users mdlUser)
        {
            try
            {
                var model = new Users
                {
                    FullName = mdlUser.FullName,
                    Email = mdlUser.Email,
                    Password = mdlUser.Password,
                    Address = mdlUser.Address,
                    FileName = mdlUser.FileName,
                    Phone = mdlUser.Phone,
                    Latitude = mdlUser.Latitude,
                    Longitude = mdlUser.Longitude,
                    City = mdlUser.City,
                    CityDescription = mdlUser.CityDescription
                };
                await _IUWork.InsertOne(model, "Users");

                return ServiceResponse.SuccessReponse(model, MessageEnum.UserProfileUpdated);


            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<Users>(exp);
            }
        }


        //   [HttpPost("SMSCodeGenerator")]
        public bool SMSCodeGenerator(string Phone)
        {
            try
            {
                List<FilterHelper> filter = new List<FilterHelper>
                {
                     new FilterHelper
                    {
                        Field = "Phone",
                        Value = Phone
                    }
                };
                var UserDetails = _IUWork.GetModelData<Users>(filter, CollectionNames.USERS).FirstOrDefault();

                string UserId = UserDetails.Id.ToString();

                Random random = new Random();
                Int32 number = random.Next(100, 500);


                if (UserDetails != null)
                {
                    var update = Builders<Users>.Update
                  .Set(o => o.SMSCode, number);
                    bool counts = _IUWork.UpdateStatus(UserId, update, CollectionNames.USERS);
                    return true;
                }
                else
                    return false;



            }
            catch (Exception exp)
            {
                return false;
            }
        }

        [HttpPost("VerifiedSMSCode")]
        public async Task<ResponseObject<bool>> VerifiedSMSCode(string UserId)
        {
            try
            {

                var lstUsers = await _IUWork.FindOneByID<Users>(UserId, CollectionNames.USERS);


                var update = Builders<Users>.Update
                   .Set(o => o.IsVerified, true);


                bool IsUpdated = _IUWork.UpdateStatus(lstUsers.Id.ToString(), update, CollectionNames.USERS);


                if (IsUpdated)
                {
                    SMSNotifyEvent _events = new SMSNotifyEvent();
                    _events.Parameters.Add("SMSCode", lstUsers.SMSCode);
                    _events.AddSMSNotifyEvent((long)NotificationEventConstants.Users.UpdateAfterSendToSMS, lstUsers.Phone);

                    NotifyEvent _event = new NotifyEvent();
                    _event.Parameters.Add("Email", lstUsers.Email);
                    _event.AddNotifyEvent((long)NotificationEventConstants.Users.RegistrationCompletedEmailSendToAdmin, lstUsers.Id.ToString());

                    return ServiceResponse.SuccessReponse(true, MessageEnum.SMSCodeVerify);
                }
                    
                else
                    return ServiceResponse.SuccessReponse(false, MessageEnum.SMSCodeNotVerify);


            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpGet("GetDisclaimerText")]
        public async Task<ResponseObject<List<Disclaimer>>> GetDisclaimerText()
        {
            try
            {
                var users = _IUWork.GetModelData<Disclaimer>(CollectionNames.Disclaimer);


                return ServiceResponse.SuccessReponse(users, MessageEnum.DisclaimerText);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Disclaimer>>(exp);
            }
        }


        [HttpPost("InsertDisclaimerText")]
        public async Task<ResponseObject<bool>> InsertDisclaimerText(string heading, string text)
        {
            try
            {
                Disclaimer disclaimer = new Disclaimer();

                disclaimer.DisclaimerHeading = heading;
                disclaimer.DisclaimerText = text;

                await _IUWork.InsertOne(disclaimer, CollectionNames.Disclaimer);
                return ServiceResponse.SuccessReponse(true, MessageEnum.DisclaimerText);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }


        [HttpPost("AddSMSTemplate")]
        public async Task<ResponseObject<bool>> AddSMSTemplate(SMSNotificationEvents FileInfo)
        {
            if (FileInfo == null)
                return ServiceResponse.ErrorReponse<bool>(MessageEnum.RecycleModelNotNull);

            try
            {
                await _IUWork.InsertOne(FileInfo, "SMSNotificationEvents");
                return ServiceResponse.SuccessReponse(true, MessageEnum.RecycleItemsAdded);

            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpGet("GetLatestAppVersion")]
        public async Task<ResponseObject<string>> GetLatestAppVersion()
        {
            string Version = AppSettingsHelper.GetAttributeValue(Constants.AppSettings.APP_SECTION, Constants.AppSettings.APP_SECTION_VERSION);

            return ServiceResponse.SuccessReponse(Version, MessageEnum.AppVersion);
        }

        [HttpGet("GetAllRecycle")]
        public async Task<ResponseObject<List<MrClean>>> GetAllRecycle()
        {
            try
            {
                var lstRecycle = _IUWork.GetModelData<MrClean>(CollectionNames.RECYCLE);


                return ServiceResponse.SuccessReponse(lstRecycle, MessageEnum.DisclaimerText);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<MrClean>>(exp);
            }
        }



        [HttpGet("GetAllReport")]
        public async Task<ResponseObject<List<Report>>> GetAllReport()
        {
            try
            {
                var lstRecycle = _IUWork.GetModelData<Report>(CollectionNames.Report);


                return ServiceResponse.SuccessReponse(lstRecycle, MessageEnum.DisclaimerText);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Report>>(exp);
            }
        }


        [HttpGet("GetAllRegift")]
        public async Task<ResponseObject<List<Regift>>> GetAllRegift()
        {
            try
            {
                var lstRecycle = _IUWork.GetModelData<Regift>(CollectionNames.REGIFT);


                return ServiceResponse.SuccessReponse(lstRecycle, MessageEnum.DisclaimerText);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Regift>>(exp);
            }
        }



        [HttpGet("GetAllEmails")]
        public async Task<ResponseObject<List<EmailNotification>>> GetAllEmails()
        {
            try
            {
                var lstRecycle = _IUWork.GetModelData<EmailNotification>(CollectionNames.EmailNotification);


                return ServiceResponse.SuccessReponse(lstRecycle, MessageEnum.DisclaimerText);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<EmailNotification>>(exp);
            }
        }

        [HttpGet("CopyData")]
        public async Task<ResponseObject<bool>> CopyData()

        {
            try
            {
                var users = _Source.GetModelData<Lookups>(Constants.CollectionNames.Lookups);


                IMongoDAL des = new MongoDAL("mongodb://10.200.10.33:27017/");


                foreach (Lookups u in users)
                {
                    await des.InsertOne<Lookups>(u, Constants.CollectionNames.Lookups);
                }

                
                return ServiceResponse.SuccessReponse(true, MessageEnum.AppVersion);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
    }
} 