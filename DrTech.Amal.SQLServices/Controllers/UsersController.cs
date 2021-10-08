using AmalForLife.Models;
using DrTech.Amal.Common.Enums;
using DrTech.Amal.Common.Extentions;
using DrTech.Amal.Common.Helpers;
using DrTech.Amal.Common.ServerResponse;
using DrTech.Amal.Notifications;
using DrTech.Amal.SQLDataAccess.CustomModels;
using DrTech.Amal.SQLDataAccess.Repository; 
using DrTech.Amal.SQLModels;
using DrTech.Amal.SQLServices.Auth;
using DrTech.Amal.SQLServices.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
//using System.IdentityModel.Tokens.Jwt;
using System.IO;   
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using static DrTech.Amal.Common.Extentions.Constants;
using static DrTech.Amal.SQLDataAccess.Repository.BusinessRepository;

namespace DrTech.Amal.SQLServices.Controllers
{
    //Route("api/[controller]")]
    public class UsersController : BaseController
    {
        //  [Route("api/Users/{RegistrationProcess}")]
        [HttpPost]
        public ResponseObject<User> RegistrationProcess([FromBody] User mdlUser)
        {
            try
            {
                User mdlUserDetails = db.ExtRepositoryFor<UsersRepository>().GetBasicUserDetails(mdlUser.Phone);

                // var json = new JavaScriptSerializer().Serialize(mdlUserDetails);
                //  var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(mdlUserDetails);
                Company mdlCompany = db.Repository<Company>().GetAll().Where(x => x.CompanyName == "AmalForLife").FirstOrDefault();

                if (mdlUserDetails != null)
                {
                    if (mdlUserDetails.Address != null && mdlUserDetails.Email != null)
                        return ServiceResponse.SuccessReponse(mdlUserDetails, "RegisteredUser");
                    else
                        return ServiceResponse.SuccessReponse(mdlUserDetails, "BasicUser");
                }
                else
                {
                    mdlUser.GreenPoints = 0;
                    mdlUser.IsActive = true;
                    mdlUser.RoleID = (int)UserRoleTypeEnum.MobileUser;
                    mdlUser.CreatedDate = DateTime.Now;
                    mdlUser.CreatedBy = 0;
                    mdlUser.IsVerified = false;
                    mdlUser.CompanyID = mdlCompany.ID;
                    mdlUser.Type = "A";
                    db.Repository<User>().Insert(mdlUser);
                    db.Save();

                    //  var mdlUserString = Newtonsoft.Json.JsonConvert.SerializeObject(mdlUser);

                    SMSNotifyEvent _events = new SMSNotifyEvent();
                    _events.Parameters.Add("SMSCode", "");
                    _events.AddSMSNotifyEvent((long)NotificationEventConstants.Users.SendSMSToUserForEntrolment, mdlUser.Phone);
                    NotifyEvent _event = new NotifyEvent();
                    _event.Parameters.Add("SMSCode", "");
                    _event.AddNotifyEvent((long)NotificationEventConstants.Users.RegistrationProcessEmailSendToAdmin, mdlUser.Phone);
                    return ServiceResponse.SuccessReponse(mdlUser, UserTypeEnum.BasicUser.GetDescription());
                }

                // return mdlUser;
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<User>(exp);
            }
        }

        //[Route("api/Users/{UpdateUserRegistrationByID}")]
        //   [Authorize]
        #region|Already working code for UpdateUserRegistrationByID|
        [HttpPost]
        public async Task<ResponseObject<TokenViewModel>> UpdateUserRegistrationByID()
        {
            try
            {
                //  int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                User _mdlUser = new User();

                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    HttpPostedFile file = HttpContext.Current.Request.Files[0];
                    _mdlUser.FileName = await FileOpsHelper.UploadFileNew(file, ContainerName.USER);
                }
                else
                {
                    if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["file"]))
                        _mdlUser.FileName = HttpContext.Current.Request.Form["file"].ToString();
                }
                string BusinessKey = string.Empty;
                string AreaName = string.Empty;
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["fullName"]))
                    _mdlUser.FullName = HttpContext.Current.Request.Form["fullName"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["email"]))
                    _mdlUser.Email = HttpContext.Current.Request.Form["email"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["address"]))
                    _mdlUser.Address = HttpContext.Current.Request.Form["address"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["phone"]))
                    _mdlUser.Phone = HttpContext.Current.Request.Form["phone"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["password"]))
                    _mdlUser.Password = HttpContext.Current.Request.Form["password"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["longitude"]))
                    _mdlUser.Longitude = Convert.ToDecimal(HttpContext.Current.Request.Form["longitude"]);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["latitude"]))
                    _mdlUser.Latitude = Convert.ToDecimal(HttpContext.Current.Request.Form["latitude"]);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["city"]))
                    _mdlUser.City = HttpContext.Current.Request.Form["city"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["ID"]))
                    _mdlUser.ID = Convert.ToInt32(HttpContext.Current.Request.Form["ID"]);


                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["cityID"]))
                    _mdlUser.CityId = Convert.ToInt32(HttpContext.Current.Request.Form["cityID"]);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["deviceToken"]))
                    _mdlUser.DeviceToken = HttpContext.Current.Request.Form["deviceToken"];

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["smsCodeRequired"]))
                    _mdlUser.SMSCodeRequired = Convert.ToBoolean(HttpContext.Current.Request.Form["smsCodeRequired"]);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["areaID"]))
                    _mdlUser.AreaID = Convert.ToInt32(HttpContext.Current.Request.Form["areaID"]);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["unionCouncil"]))
                    _mdlUser.UnionCouncil = HttpContext.Current.Request.Form["unionCouncil"];

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["businessKey"]))
                    BusinessKey = HttpContext.Current.Request.Form["businessKey"];

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["areaname"]))
                    AreaName = HttpContext.Current.Request.Form["areaname"];

                _mdlUser.CompanyID = db.ExtRepositoryFor<CommonRepository>().GetCompanyIdFromBusinessKey(BusinessKey);


                User User = new User();
                User = db.ExtRepositoryFor<UsersRepository>().GetUserDetailsByID(_mdlUser.ID);
                string OldPhone = User.Phone;
                if (string.IsNullOrEmpty(_mdlUser.FullName))
                    _mdlUser.FullName = User.FullName;
                if (string.IsNullOrEmpty(_mdlUser.Email))
                    _mdlUser.Email = User.Email;
                if (string.IsNullOrEmpty(_mdlUser.Password))
                    _mdlUser.Password = User.Password;
                if (string.IsNullOrEmpty(_mdlUser.Phone))
                    _mdlUser.Phone = User.Phone;
                if (_mdlUser.Latitude == 0) 
                    _mdlUser.Latitude = User.Latitude;
                if (_mdlUser.Longitude == 0)
                    _mdlUser.Longitude = User.Longitude;

                if (string.IsNullOrEmpty(_mdlUser.City))
                    _mdlUser.City = User.City;

                if (string.IsNullOrEmpty(_mdlUser.FileName))
                    _mdlUser.FileName = User.FileName;

                if (_mdlUser.SMSCodeRequired == null)
                    _mdlUser.SMSCodeRequired = false;

                if (_mdlUser.AreaID == 0)
                    _mdlUser.AreaID = User.AreaID;

                if (string.IsNullOrEmpty(_mdlUser.UnionCouncil))
                    _mdlUser.UnionCouncil = User.UnionCouncil;




                StringBuilder CodeModel = new StringBuilder();
                CodeModel.Append(_mdlUser.ID + ";");
                CodeModel.Append(_mdlUser.Email + ";");
                CodeModel.Append(_mdlUser.Phone + ";");
                CodeModel.Append(_mdlUser.FullName);

                string QRCode = string.Empty;

                User mdlUser = db.Repository<User>().FindById(_mdlUser.ID);
                if (AreaName != null)
                {
                    var UserArea = db.Repository<Area>().GetAll().Where(x => x.Name == AreaName).FirstOrDefault();
                    if (UserArea == null)
                    {

                        Area area = new Area();
                        area.Name = AreaName;
                        area.CityID = _mdlUser.CityId;
                        area.IsActive = true;
                        area.CreatedBy = mdlUser.ID;
                        area.CreatedDate = DateTime.UtcNow;
                        db.Repository<Area>().Insert(area);
                        db.Save();
                        UserArea = db.Repository<Area>().GetAll().Where(x => x.Name == AreaName).FirstOrDefault();
                        _mdlUser.AreaID = UserArea.ID;
                    }
                }
                else
                {
                    return ServiceResponse.ErrorReponse<TokenViewModel>("This Area is Already Exits");

                }
                if (string.IsNullOrEmpty(mdlUser.QRCode))
                {
                    QRCode = "";           // QRCodeTagHelper.QRCodeGenerator(CodeModel);
                }
                else
                    // QRCode = mdlUser.QRCode;
                    QRCode = "";
                mdlUser.FullName = _mdlUser.FullName;
                mdlUser.Address = _mdlUser.Address;
                mdlUser.Phone = _mdlUser.Phone;
                mdlUser.Password = _mdlUser.Password;
                mdlUser.Email = _mdlUser.Email;
                mdlUser.Longitude = _mdlUser.Longitude;
                mdlUser.Latitude = _mdlUser.Latitude;
                mdlUser.City = _mdlUser.City;
                mdlUser.FileName = _mdlUser.FileName;
                mdlUser.DeviceToken = _mdlUser.DeviceToken;
                mdlUser.QRCode = QRCode;
                mdlUser.UnionCouncil = _mdlUser.UnionCouncil;
                mdlUser.AreaID = _mdlUser.AreaID;
                mdlUser.CompanyID = _mdlUser.CompanyID;
                mdlUser.CityId = _mdlUser.CityId;
                db.Repository<User>().Update(mdlUser);
                db.Save();

                #region|Connect User with the User Area GPN if exist|
                var CheckGPNInUserArea = db.Repository<Organization>().GetAll().Where(x => x.AreaID == mdlUser.AreaID).FirstOrDefault();
                bool IsConnectedWithGPN = false;
                string GPNConnectionMessage = "";
                if (CheckGPNInUserArea != null)
                {
                    var CheckUserMembershipInGPN = db.Repository<Member>().GetAll().Where(x => x.OrgId == CheckGPNInUserArea.ID && x.UserID == mdlUser.ID && x.IsActive == true).FirstOrDefault();
                    if (CheckUserMembershipInGPN == null)
                    {

                        Member mdlMember = new Member();
                        mdlMember.Name = mdlUser.FullName;
                        mdlMember.Designation = "Membership";
                        mdlMember.UserID = mdlUser.ID;
                        mdlMember.CreatedBy = mdlUser.ID;
                        mdlMember.OrgId = CheckGPNInUserArea.ID;
                        mdlMember.IsCurrentlyWorking = true;
                        mdlMember.FileName = "";
                        mdlMember.IsActive = true;
                        mdlMember.IsVerified = true;
                        db.Repository<Member>().Insert(mdlMember);
                        db.Save();
                        // set flags
                        IsConnectedWithGPN = true;
                        //   GPNConnectionMessage = string.Format("Congratulations! You are now connected with {GPN} GPN".Replace("{GPN}", CheckGPNInUserArea.Name);
                        GPNConnectionMessage = String.Format("Congratulations! You are now connected with {0} GPN",
                              CheckGPNInUserArea.Name);
                    }

                }
                #endregion

                var UpdatedUsers = db.Repository<User>().FindById(_mdlUser.ID);

                object TotalGP = db.ExtRepositoryFor<UsersRepository>().GetUserTotalGP(UpdatedUsers.ID);
                int GP = 0;
                decimal? GPRedeemed = 0;
                decimal? GPRedeemable = 0;
                if (TotalGP != null)
                {
                    GP = Convert.ToInt32(TotalGP.GetType().GetProperty("TotalGP").GetValue(TotalGP));

                    GPRedeemed = db.Repository<GCRedeem>().GetAll().Where(x => x.UserID == UpdatedUsers.ID).ToList().Sum(y => y.GCRedeemed);
                    GPRedeemable = GP - GPRedeemed;

                }

                UpdatedUsers.GreenPoints = GP;
                int _min = 1000;
                int _max = 9999;
                Random random = new Random();
                Int32 number = random.Next(_min, _max);
                if (_mdlUser.SMSCodeRequired == true)
                {
                    _mdlUser.IsVerified = false;
                    SMSNotifyEvent _events = new SMSNotifyEvent();
                    _events.Parameters.Add("SMSCode", number);
                    _events.AddSMSNotifyEvent((long)NotificationEventConstants.Users.SendVerificationCodeSMS, UpdatedUsers.Phone);
                    //Also email the code.
                    NotifyEvent _eventEmail = new NotifyEvent();
                    _eventEmail.Parameters.Add("SMSCode", number);
                    _eventEmail.AddNotifyEvent((long)NotificationEventConstants.Email.SendEmailOfPhoneConfirmation, Convert.ToString(mdlUser.ID));
                }



                if (UpdatedUsers != null)
                {
                    // LookupType mdllookup = db.Repository<LookupType>().GetAll().Where(x => x.ID == UpdatedUsers.CityId).FirstOrDefault();
                    City city = db.Repository<City>().GetAll().Where(x => x.ID == UpdatedUsers.CityId).FirstOrDefault();

                    String CityName = "";
                    if (city != null)
                    {
                        CityName = city.CityName;
                    }
                    // var token = JwtTokenBuilder(UpdatedUsers);
                    object mdlnew = new object();
                    var token = JwtManager.CreateToken(UpdatedUsers, out mdlnew);
                    return ServiceResponse.SuccessReponse(new TokenViewModel
                    {
                        Token = token,
                        ID = UpdatedUsers.ID,
                        FullName = UpdatedUsers.FullName,
                        Address = UpdatedUsers.Address,
                        Email = UpdatedUsers.Email,
                        Phone = UpdatedUsers.Phone,
                        FileName = UpdatedUsers.FileName,
                        Longitude = Convert.ToDecimal(UpdatedUsers.Longitude),
                        Latitude = Convert.ToDecimal(UpdatedUsers.Latitude),
                        GreenPoints = Convert.ToInt32(UpdatedUsers.GreenPoints),
                        Password = UpdatedUsers.Password,
                        SMSCode = number,
                        City = CityName,
                        DeviceToken = UpdatedUsers.DeviceToken,
                        AreaID = UpdatedUsers.AreaID,
                        UnionCouncil = UpdatedUsers.UnionCouncil,
                        CompanyID = UpdatedUsers.CompanyID,
                        CityId = UpdatedUsers.CityId,
                        BusinessKey = db.ExtRepositoryFor<CommonRepository>().GetBusinessKeyCompanyIdFrom(UpdatedUsers.CompanyID),
                        IsConnectedWithGPN = IsConnectedWithGPN,
                        GPNConnectionMessage = GPNConnectionMessage,
                        RedeemedPoints = GPRedeemed,
                        RedeemablePoints = GPRedeemable,
                        WalletBalance = UpdatedUsers.WalletBalance ?? 0,
                        QRCode = UpdatedUsers.QRCode,
                        //AreaName=


                    }, MessageEnum.UserProfileUpdated);

                }
                else
                {
                    return ServiceResponse.ErrorReponse<TokenViewModel>(MessageEnum.UserDetailNotFoundSuccessfully);
                }
            }
            catch (DbEntityValidationException e)
            {
                String errorMessage = string.Empty;
                foreach (var eve in e.EntityValidationErrors)
                {
                    errorMessage = string.Format("Entity of type {0} in state {1} has the following validation errors {2}: ",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State, 0) + Environment.NewLine;
                    foreach (var ve in eve.ValidationErrors)
                    {
                        errorMessage = errorMessage + string.Format("- Property: {0}, Error: {1}",
                            ve.PropertyName, ve.ErrorMessage) + Environment.NewLine;
                    }
                }
                return ServiceResponse.ErrorReponse<TokenViewModel>(errorMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<TokenViewModel>(exp);
            }
        }
        #endregion

        #region|New for version 5.1.4 UpdateUserRegistrationByID|
        [HttpPost]
        public async Task<ResponseObject<TokenViewModel>> kkUpdateUserRegistrationByID()
        {
            try
            {
                bool flag = false;
                int UpdatedUserID = 0;
                int NewUserID = 0;

                bool IsFacebook = false;
                bool IsGoogle = false;
                //  int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                User _mdlUser = new User();

                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    HttpPostedFile file = HttpContext.Current.Request.Files[0];
                    _mdlUser.FileName = await FileOpsHelper.UploadFileNew(file, ContainerName.USER);
                }
                else
                {
                    if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["file"]))
                        _mdlUser.FileName = HttpContext.Current.Request.Form["file"].ToString();
                }
                string BusinessKey = string.Empty;
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["fullName"]))
                    _mdlUser.FullName = HttpContext.Current.Request.Form["fullName"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["email"]))
                    _mdlUser.Email = HttpContext.Current.Request.Form["email"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["address"]))
                    _mdlUser.Address = HttpContext.Current.Request.Form["address"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["phone"]))
                    _mdlUser.Phone = HttpContext.Current.Request.Form["phone"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["password"]))
                    _mdlUser.Password = HttpContext.Current.Request.Form["password"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["longitude"]))
                    _mdlUser.Longitude = Convert.ToDecimal(HttpContext.Current.Request.Form["longitude"]);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["latitude"]))
                    _mdlUser.Latitude = Convert.ToDecimal(HttpContext.Current.Request.Form["latitude"]);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["city"]))
                    _mdlUser.City = HttpContext.Current.Request.Form["city"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["ID"]))
                    _mdlUser.ID = Convert.ToInt32(HttpContext.Current.Request.Form["ID"]);


                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["cityID"]))
                    _mdlUser.CityId = Convert.ToInt32(HttpContext.Current.Request.Form["cityID"]);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["deviceToken"]))
                    _mdlUser.DeviceToken = HttpContext.Current.Request.Form["deviceToken"];

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["smsCodeRequired"]))
                    _mdlUser.SMSCodeRequired = Convert.ToBoolean(HttpContext.Current.Request.Form["smsCodeRequired"]);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["areaID"]))
                    _mdlUser.AreaID = Convert.ToInt32(HttpContext.Current.Request.Form["areaID"]);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["unionCouncil"]))
                    _mdlUser.UnionCouncil = HttpContext.Current.Request.Form["unionCouncil"];

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["facebookKey"]))
                    _mdlUser.SocialMediaKey = HttpContext.Current.Request.Form["facebookKey"];

                //if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["googleKey"]))
                //    _mdlUser.GoogleKey = HttpContext.Current.Request.Form["googleKey"];

                if (_mdlUser.SocialMediaKey != null)
                {
                    IsFacebook = true;
                }

                //if (_mdlUser.GoogleKey != null)
                //{
                //    IsGoogle = true;
                //}

                #region|New Block|
                int _min = 1000;
                int _max = 9999;
                Random random = new Random();
                Int32 number = random.Next(_min, _max);
                if (_mdlUser.SMSCodeRequired == true)
                {
                    User usrModl = new User();
                    usrModl = db.Repository<User>().GetAll().Where(x => x.Phone == _mdlUser.Phone).FirstOrDefault();
                    if (usrModl == null)
                    {
                        User dbUser = new User();
                        dbUser.AreaID = _mdlUser.AreaID;
                        dbUser.CityId = _mdlUser.CityId;
                        dbUser.DeviceToken = _mdlUser.DeviceToken;
                        dbUser.Email = _mdlUser.Email;
                        dbUser.Phone = _mdlUser.Phone;
                        if (_mdlUser.SMSCodeRequired == null)
                        {
                            _mdlUser.SMSCodeRequired = false;
                        }
                        dbUser.RemainingAmount = 1000;
                       
                        dbUser.SMSCodeRequired = _mdlUser.SMSCodeRequired;

                        if (IsFacebook == true)
                        {
                            dbUser.SocialMediaKey = _mdlUser.SocialMediaKey;
                        }
                        if (IsGoogle == true)
                        {
                            //  dbUser.GoogleKey = _mdlUser.GoogleKey;
                        }

                        dbUser.FileName = _mdlUser.FileName;
                        dbUser.Type = "A";
                        dbUser.Address = _mdlUser.Address;
                        dbUser.FullName = _mdlUser.FullName;
                        dbUser.Latitude = _mdlUser.Latitude;
                        dbUser.Longitude = _mdlUser.Longitude;
                        dbUser.Password = _mdlUser.Password;
                        dbUser.CompanyID = null;
                        dbUser.RoleID = 6;

                        dbUser.IsVerified = false;
                        db.Repository<User>().Insert(dbUser);
                        db.Save();

                        //  flag = true;
                        //  NewUserID = dbUser.ID;

                        SMSNotifyEvent _eventsusr = new SMSNotifyEvent();
                        _eventsusr.Parameters.Add("SMSCode", number);
                        _eventsusr.AddSMSNotifyEvent((long)NotificationEventConstants.Users.SendVerificationCodeSMS, dbUser.Phone);
                        //Also email the code.
                        NotifyEvent _eventEmailusr = new NotifyEvent();
                        _eventEmailusr.Parameters.Add("SMSCode", number);
                        _eventEmailusr.AddNotifyEvent((long)NotificationEventConstants.Email.SendEmailOfPhoneConfirmation, Convert.ToString(dbUser.ID));

                        //Create response object
                        object TotalGPOfUser = db.ExtRepositoryFor<UsersRepository>().GetUserTotalGP(dbUser.ID);
                        int GC = 0;
                        decimal? GPRedeemedOfUser = 0;
                        decimal? GPRedeemableOfUser = 0;
                        if (TotalGPOfUser != null)
                        {
                            GC = Convert.ToInt32(TotalGPOfUser.GetType().GetProperty("TotalGP").GetValue(TotalGPOfUser));
                            GPRedeemedOfUser = db.Repository<GCRedeem>().GetAll().Where(x => x.UserID == dbUser.ID).ToList().Sum(y => y.GCRedeemed);
                            GPRedeemableOfUser = GC - GPRedeemedOfUser;
                        }
                        var CheckGPNInUserAreaUser = db.Repository<Organization>().GetAll().Where(x => x.AreaID == dbUser.AreaID).FirstOrDefault();
                        bool IsConnectedWithGPNUser = false;
                        string GPNConnectionMessageUser = "";
                        if (CheckGPNInUserAreaUser != null)
                        {
                            var CheckUserMembershipInGPN = db.Repository<Member>().GetAll().Where(x => x.OrgId == CheckGPNInUserAreaUser.ID && x.UserID == dbUser.ID && x.IsActive == true).FirstOrDefault();
                            if (CheckUserMembershipInGPN == null)
                            {

                                Member mdlMember = new Member();
                                mdlMember.Name = dbUser.FullName;
                                mdlMember.Designation = "Membership";
                                mdlMember.UserID = dbUser.ID;
                                mdlMember.CreatedBy = dbUser.ID;
                                mdlMember.OrgId = CheckGPNInUserAreaUser.ID;
                                mdlMember.IsCurrentlyWorking = true;
                                mdlMember.FileName = "";
                                mdlMember.IsActive = true;
                                mdlMember.IsVerified = true;
                                db.Repository<Member>().Insert(mdlMember);
                                db.Save();
                                // set flags
                                IsConnectedWithGPNUser = true;
                                //   GPNConnectionMessage = string.Format("Congratulations! You are now connected with {GPN} GPN".Replace("{GPN}", CheckGPNInUserArea.Name);
                                GPNConnectionMessageUser = String.Format("Congratulations! You are now connected with {0} GPN",
                                      CheckGPNInUserAreaUser.Name);
                            }

                        }

                        City city = db.Repository<City>().GetAll().Where(x => x.ID == dbUser.CityId).FirstOrDefault();

                        String CityName = "";
                        if (city != null)
                        {
                            CityName = city.CityName;
                        }
                        object mdlnewusr = new object();
                        var usrtoken = JwtManager.CreateToken(dbUser, out mdlnewusr);

                        return ServiceResponse.SuccessReponse(new TokenViewModel
                        {
                            Token = usrtoken,
                            SMSCode = number,
                            ID = dbUser.ID,
                            FullName = dbUser.FullName,
                            Address = dbUser.Address,
                            Email = dbUser.Email,
                            Phone = dbUser.Phone,
                            FileName = dbUser.FileName,
                            Longitude = Convert.ToDecimal(dbUser.Longitude),
                            Latitude = Convert.ToDecimal(dbUser.Latitude),
                            GreenPoints = Convert.ToInt32(dbUser.GreenPoints),
                            Password = dbUser.Password,
                            City = CityName,
                            DeviceToken = dbUser.DeviceToken,
                            AreaID = dbUser.AreaID,
                            UnionCouncil = dbUser.UnionCouncil,
                            CompanyID = dbUser.CompanyID,
                            CityId = dbUser.CityId,
                            BusinessKey = db.ExtRepositoryFor<CommonRepository>().GetBusinessKeyCompanyIdFrom(dbUser.CompanyID),
                            IsConnectedWithGPN = IsConnectedWithGPNUser,
                            GPNConnectionMessage = GPNConnectionMessageUser,
                            RedeemedPoints = GPRedeemedOfUser,
                            RedeemablePoints = GPRedeemableOfUser,
                            WalletBalance = dbUser.WalletBalance ?? 0,
                            QRCode = dbUser.QRCode

                        },

                       MessageEnum.RecordFoundSuccessfully);

                    }
                    else
                    {


                        object TotalGPOfUser = db.ExtRepositoryFor<UsersRepository>().GetUserTotalGP(usrModl.ID);
                        int GC = 0;
                        decimal? GPRedeemedOfUser = 0;
                        decimal? GPRedeemableOfUser = 0;
                        if (TotalGPOfUser != null)
                        {
                            GC = Convert.ToInt32(TotalGPOfUser.GetType().GetProperty("TotalGP").GetValue(TotalGPOfUser));

                            GPRedeemedOfUser = db.Repository<GCRedeem>().GetAll().Where(x => x.UserID == usrModl.ID).ToList().Sum(y => y.GCRedeemed);
                            GPRedeemableOfUser = GC - GPRedeemedOfUser;

                        }

                        var CheckGPNInUserAreaUser = db.Repository<Organization>().GetAll().Where(x => x.AreaID == usrModl.AreaID).FirstOrDefault();
                        bool IsConnectedWithGPNUser = false;
                        string GPNConnectionMessageUser = "";
                        if (CheckGPNInUserAreaUser != null)
                        {
                            var CheckUserMembershipInGPN = db.Repository<Member>().GetAll().Where(x => x.OrgId == CheckGPNInUserAreaUser.ID && x.UserID == usrModl.ID && x.IsActive == true).FirstOrDefault();
                            if (CheckUserMembershipInGPN == null)
                            {

                                Member mdlMember = new Member();
                                mdlMember.Name = usrModl.FullName;
                                mdlMember.Designation = "Membership";
                                mdlMember.UserID = usrModl.ID;
                                mdlMember.CreatedBy = usrModl.ID;
                                mdlMember.OrgId = CheckGPNInUserAreaUser.ID;
                                mdlMember.IsCurrentlyWorking = true;
                                mdlMember.FileName = "";
                                mdlMember.IsActive = true;
                                mdlMember.IsVerified = true;
                                db.Repository<Member>().Insert(mdlMember);
                                db.Save();
                                // set flags
                                IsConnectedWithGPNUser = true;
                                //   GPNConnectionMessage = string.Format("Congratulations! You are now connected with {GPN} GPN".Replace("{GPN}", CheckGPNInUserArea.Name);
                                GPNConnectionMessageUser = String.Format("Congratulations! You are now connected with {0} GPN",
                                      CheckGPNInUserAreaUser.Name);
                            }

                        }

                        City city = db.Repository<City>().GetAll().Where(x => x.ID == usrModl.CityId).FirstOrDefault();

                        String CityName = "";
                        if (city != null)
                        {
                            CityName = city.CityName;
                        }
                        object mdlnewusr = new object();
                        var usrtoken = JwtManager.CreateToken(usrModl, out mdlnewusr);

                        SMSNotifyEvent _eventsusr = new SMSNotifyEvent();
                        _eventsusr.Parameters.Add("SMSCode", number);
                        _eventsusr.AddSMSNotifyEvent((long)NotificationEventConstants.Users.SendVerificationCodeSMS, usrModl.Phone);
                        //Also email the code.
                        NotifyEvent _eventEmailusr = new NotifyEvent();
                        _eventEmailusr.Parameters.Add("SMSCode", number);
                        _eventEmailusr.AddNotifyEvent((long)NotificationEventConstants.Email.SendEmailOfPhoneConfirmation, Convert.ToString(usrModl.ID));

                        return ServiceResponse.SuccessReponse(new TokenViewModel
                        {
                            //Addd SMS parameter.
                            SMSCode = number,
                            Token = usrtoken,
                            ID = usrModl.ID,
                            FullName = usrModl.FullName,
                            Address = usrModl.Address,
                            Email = usrModl.Email,
                            Phone = usrModl.Phone,
                            FileName = usrModl.FileName,
                            Longitude = Convert.ToDecimal(usrModl.Longitude),
                            Latitude = Convert.ToDecimal(usrModl.Latitude),
                            GreenPoints = Convert.ToInt32(usrModl.GreenPoints),
                            Password = usrModl.Password,
                            City = CityName,
                            DeviceToken = usrModl.DeviceToken,
                            AreaID = usrModl.AreaID,
                            UnionCouncil = usrModl.UnionCouncil,
                            CompanyID = usrModl.CompanyID,
                            CityId = usrModl.CityId,
                            BusinessKey = db.ExtRepositoryFor<CommonRepository>().GetBusinessKeyCompanyIdFrom(usrModl.CompanyID),
                            IsConnectedWithGPN = IsConnectedWithGPNUser,
                            GPNConnectionMessage = GPNConnectionMessageUser,
                            RedeemedPoints = GPRedeemedOfUser,
                            RedeemablePoints = GPRedeemableOfUser,
                            WalletBalance = usrModl.WalletBalance ?? 0,
                            QRCode = usrModl.QRCode

                        },

                       MessageEnum.RecordFoundSuccessfully);

                        //if (IsFacebook == true)
                        //{
                        //    usrModl.SocialMediaKey = _mdlUser.SocialMediaKey;
                        //}
                        //if (IsGoogle == true)
                        //{
                        //    usrModl.GoogleKey = _mdlUser.GoogleKey;
                        //}
                        //usrModl.Type = "A";
                        //usrModl.FullName = _mdlUser.FullName;
                        //usrModl.Phone = _mdlUser.Phone;
                        //usrModl.Email = _mdlUser.Email;
                        //usrModl.Address = _mdlUser.Address;                      
                        //usrModl.Latitude = _mdlUser.Latitude;
                        //usrModl.Longitude = _mdlUser.Longitude;
                        //usrModl.AreaID = _mdlUser.AreaID;
                        //usrModl.CityId = _mdlUser.CityId;
                        //db.Repository<User>().Update(usrModl);
                        //db.Save();
                        //flag = false;
                        //UpdatedUserID = usrModl.ID;

                    }






                    //////var usrfromDB = new User();
                    //////if (flag == true)
                    //////{
                    //////    usrfromDB = db.Repository<User>().GetAll().Where(x => x.ID == NewUserID).FirstOrDefault();
                    //////}
                    //////else
                    //////{
                    //////    usrfromDB = db.Repository<User>().GetAll().Where(x => x.ID == UpdatedUserID).FirstOrDefault();

                    //////}
                    //////_mdlUser.IsVerified = false;
                    //////SMSNotifyEvent _events = new SMSNotifyEvent();
                    //////_events.Parameters.Add("SMSCode", number);
                    //////_events.AddSMSNotifyEvent((long)NotificationEventConstants.Users.SendVerificationCodeSMS, usrfromDB.Phone);
                    ////////Also email the code.
                    //////NotifyEvent _eventEmail = new NotifyEvent();
                    //////_eventEmail.Parameters.Add("SMSCode", number);
                    //////_eventEmail.AddNotifyEvent((long)NotificationEventConstants.Email.SendEmailOfPhoneConfirmation, Convert.ToString(usrfromDB.ID));


                    //////////////////////object TotalGPOfUser = db.ExtRepositoryFor<UsersRepository>().GetUserTotalGP(usrfromDB.ID);
                    //////////////////////int GC = 0;
                    //////////////////////decimal? GPRedeemedOfUser = 0;
                    //////////////////////decimal? GPRedeemableOfUser = 0;
                    //////////////////////if (TotalGPOfUser != null)
                    //////////////////////{
                    //////////////////////    GC = Convert.ToInt32(TotalGPOfUser.GetType().GetProperty("TotalGP").GetValue(TotalGPOfUser));

                    //////////////////////    GPRedeemedOfUser = db.Repository<GCRedeem>().GetAll().Where(x => x.UserID == usrfromDB.ID).ToList().Sum(y => y.GCRedeemed);
                    //////////////////////    GPRedeemableOfUser = GC - GPRedeemedOfUser;

                    //////////////////////}

                    //////////////////////var CheckGPNInUserAreaUser = db.Repository<Organization>().GetAll().Where(x => x.AreaID == _mdlUser.AreaID).FirstOrDefault();
                    //////////////////////bool IsConnectedWithGPNUser = false;
                    //////////////////////string GPNConnectionMessageUser = "";
                    //////////////////////if (CheckGPNInUserAreaUser != null)
                    //////////////////////{
                    //////////////////////    var CheckUserMembershipInGPN = db.Repository<Member>().GetAll().Where(x => x.OrgId == CheckGPNInUserAreaUser.ID && x.UserID == _mdlUser.ID && x.IsActive == true).FirstOrDefault();
                    //////////////////////    if (CheckUserMembershipInGPN == null)
                    //////////////////////    {

                    //////////////////////        Member mdlMember = new Member();
                    //////////////////////        mdlMember.Name = _mdlUser.FullName;
                    //////////////////////        mdlMember.Designation = "Membership";
                    //////////////////////        mdlMember.UserID = _mdlUser.ID;
                    //////////////////////        mdlMember.CreatedBy = _mdlUser.ID;
                    //////////////////////        mdlMember.OrgId = CheckGPNInUserAreaUser.ID;
                    //////////////////////        mdlMember.IsCurrentlyWorking = true;
                    //////////////////////        mdlMember.FileName = "";
                    //////////////////////        mdlMember.IsActive = true;
                    //////////////////////        mdlMember.IsVerified = true;
                    //////////////////////        db.Repository<Member>().Insert(mdlMember);
                    //////////////////////        db.Save();
                    //////////////////////        // set flags
                    //////////////////////        IsConnectedWithGPNUser = true;
                    //////////////////////        //   GPNConnectionMessage = string.Format("Congratulations! You are now connected with {GPN} GPN".Replace("{GPN}", CheckGPNInUserArea.Name);
                    //////////////////////        GPNConnectionMessageUser = String.Format("Congratulations! You are now connected with {0} GPN",
                    //////////////////////              CheckGPNInUserAreaUser.Name);
                    //////////////////////    }

                    //////////////////////}

                    //////////////////////City city = db.Repository<City>().GetAll().Where(x => x.ID == usrfromDB.CityId).FirstOrDefault();

                    //////////////////////String CityName = "";
                    //////////////////////if (city != null)
                    //////////////////////{
                    //////////////////////    CityName = city.CityName;
                    //////////////////////}
                    //


                    ////    object mdlnew = new object();
                    ////    var token = JwtManager.CreateToken(usrfromDB, out mdlnew);

                    ////    return ServiceResponse.SuccessReponse(new TokenViewModel
                    ////    {
                    ////        SMSCode = number,

                    ////        Token = token,
                    ////        ID = usrfromDB.ID,
                    ////        FullName = usrfromDB.FullName,
                    ////        Address = usrfromDB.Address,
                    ////        Email = usrfromDB.Email,
                    ////        Phone = usrfromDB.Phone,
                    ////        FileName = usrfromDB.FileName,
                    ////        Longitude = Convert.ToDecimal(usrfromDB.Longitude),
                    ////        Latitude = Convert.ToDecimal(usrfromDB.Latitude),
                    ////        GreenPoints = Convert.ToInt32(usrfromDB.GreenPoints),
                    ////        Password = usrfromDB.Password,
                    ////        City = CityName,
                    ////        DeviceToken = usrfromDB.DeviceToken,
                    ////        AreaID = usrfromDB.AreaID,
                    ////        UnionCouncil = usrfromDB.UnionCouncil,
                    ////        CompanyID = usrfromDB.CompanyID,
                    ////        CityId = usrfromDB.CityId,
                    ////        BusinessKey = db.ExtRepositoryFor<CommonRepository>().GetBusinessKeyCompanyIdFrom(usrfromDB.CompanyID),
                    ////        IsConnectedWithGPN = IsConnectedWithGPNUser,
                    ////        GPNConnectionMessage = GPNConnectionMessageUser,
                    ////        RedeemedPoints = GPRedeemedOfUser,
                    ////        RedeemablePoints = GPRedeemableOfUser,
                    ////        WalletBalance = usrfromDB.WalletBalance ?? 0,
                    ////        QRCode = usrfromDB.QRCode

                    ////    },

                    ////        MessageEnum.UserDetailNotFoundSuccessfully);
                    ////}
                    #endregion








                    ////////StringBuilder CodeModel = new StringBuilder();
                    ////////CodeModel.Append(_mdlUser.ID + ";");
                    ////////CodeModel.Append(_mdlUser.Email + ";");
                    ////////CodeModel.Append(_mdlUser.Phone + ";");
                    ////////CodeModel.Append(_mdlUser.FullName);

                    ////////string QRCode = string.Empty;

                    ////////#region|Connect User with the User Area GPN if exist|
                    ////////var CheckGPNInUserArea = db.Repository<Organization>().GetAll().Where(x => x.AreaID == _mdlUser.AreaID).FirstOrDefault();
                    ////////bool IsConnectedWithGPN = false;
                    ////////string GPNConnectionMessage = "";
                    ////////if (CheckGPNInUserArea != null)
                    ////////{
                    ////////    var CheckUserMembershipInGPN = db.Repository<Member>().GetAll().Where(x => x.OrgId == CheckGPNInUserArea.ID && x.UserID == _mdlUser.ID && x.IsActive == true).FirstOrDefault();
                    ////////    if (CheckUserMembershipInGPN == null)
                    ////////    {

                    ////////        Member mdlMember = new Member();
                    ////////        mdlMember.Name = _mdlUser.FullName;
                    ////////        mdlMember.Designation = "Membership";
                    ////////        mdlMember.UserID = _mdlUser.ID;
                    ////////        mdlMember.CreatedBy = _mdlUser.ID;
                    ////////        mdlMember.OrgId = CheckGPNInUserArea.ID;
                    ////////        mdlMember.IsCurrentlyWorking = true;
                    ////////        mdlMember.FileName = "";
                    ////////        mdlMember.IsActive = true;
                    ////////        mdlMember.IsVerified = true;
                    ////////        db.Repository<Member>().Insert(mdlMember);
                    ////////        db.Save();
                    ////////        // set flags
                    ////////        IsConnectedWithGPN = true;
                    ////////        //   GPNConnectionMessage = string.Format("Congratulations! You are now connected with {GPN} GPN".Replace("{GPN}", CheckGPNInUserArea.Name);
                    ////////        GPNConnectionMessage = String.Format("Congratulations! You are now connected with {0} GPN",
                    ////////              CheckGPNInUserArea.Name);
                    ////////    }

                    ////////}
                    ////////#endregion
                    ////////var UpdatedUsers = new User();
                    ////////if(flag == true)
                    ////////{
                    ////////    UpdatedUsers = db.Repository<User>().FindById(NewUserID);
                    ////////}
                    ////////else
                    ////////{
                    ////////    UpdatedUsers = db.Repository<User>().FindById(UpdatedUserID);

                    ////////}

                    ////////// = db.Repository<User>().FindById(_mdlUser.ID);

                    ////////object TotalGP = db.ExtRepositoryFor<UsersRepository>().GetUserTotalGP(UpdatedUsers.ID);
                    ////////int GP = 0;
                    ////////decimal? GPRedeemed = 0;
                    ////////decimal? GPRedeemable = 0;
                    ////////if (TotalGP != null)
                    ////////{
                    ////////    GP = Convert.ToInt32(TotalGP.GetType().GetProperty("TotalGP").GetValue(TotalGP));

                    ////////    GPRedeemed = db.Repository<GCRedeem>().GetAll().Where(x => x.UserID == UpdatedUsers.ID).ToList().Sum(y => y.GCRedeemed);
                    ////////    GPRedeemable = GP - GPRedeemed;

                    ////////}

                    ////////UpdatedUsers.GreenPoints = GP;             
                    ////////if (UpdatedUsers != null)
                    ////////{
                    ////////    // LookupType mdllookup = db.Repository<LookupType>().GetAll().Where(x => x.ID == UpdatedUsers.CityId).FirstOrDefault();
                    ////////    City city = db.Repository<City>().GetAll().Where(x => x.ID == UpdatedUsers.CityId).FirstOrDefault();

                    ////////    String CityName = "";
                    ////////    if (city != null)
                    ////////    {
                    ////////        CityName = city.CityName;
                    ////////    }
                    ////////    // var token = JwtTokenBuilder(UpdatedUsers);
                    ////////    object mdlnew = new object();
                    ////////    var token = JwtManager.CreateToken(UpdatedUsers, out mdlnew);
                    ////////    return ServiceResponse.SuccessReponse(new TokenViewModel
                    ////////    {
                    ////////        Token = token,
                    ////////        ID = UpdatedUsers.ID,
                    ////////        FullName = UpdatedUsers.FullName,
                    ////////        Address = UpdatedUsers.Address,
                    ////////        Email = UpdatedUsers.Email,
                    ////////        Phone = UpdatedUsers.Phone,
                    ////////        FileName = UpdatedUsers.FileName,
                    ////////        Longitude = Convert.ToDecimal(UpdatedUsers.Longitude),
                    ////////        Latitude = Convert.ToDecimal(UpdatedUsers.Latitude),
                    ////////        GreenPoints = Convert.ToInt32(UpdatedUsers.GreenPoints),
                    ////////        Password = UpdatedUsers.Password,
                    ////////        SMSCode = number,
                    ////////        City = CityName,
                    ////////        DeviceToken = UpdatedUsers.DeviceToken,
                    ////////        AreaID = UpdatedUsers.AreaID,
                    ////////        UnionCouncil = UpdatedUsers.UnionCouncil,
                    ////////        CompanyID = UpdatedUsers.CompanyID,
                    ////////        CityId = UpdatedUsers.CityId,
                    ////////        BusinessKey = db.ExtRepositoryFor<CommonRepository>().GetBusinessKeyCompanyIdFrom(UpdatedUsers.CompanyID),
                    ////////        IsConnectedWithGPN = IsConnectedWithGPN,
                    ////////        GPNConnectionMessage = GPNConnectionMessage,
                    ////////        RedeemedPoints = GPRedeemed,
                    ////////        RedeemablePoints = GPRedeemable,
                    ////////        WalletBalance = UpdatedUsers.WalletBalance ?? 0,
                    ////////        QRCode = UpdatedUsers.QRCode



                    ////////    }, MessageEnum.UserProfileUpdated);

                    ////////}
                    ////////else
                    ////////{
                    ////////    return ServiceResponse.ErrorReponse<TokenViewModel>(MessageEnum.UserDetailNotFoundSuccessfully);
                    ////////}
                }
                else
                {
                    var usrToUpdate = db.Repository<User>().GetAll().Where(x => x.Phone == _mdlUser.Phone).FirstOrDefault();

                    usrToUpdate.AreaID = _mdlUser.AreaID;
                    usrToUpdate.CityId = _mdlUser.CityId;
                    usrToUpdate.DeviceToken = _mdlUser.DeviceToken;
                    usrToUpdate.Email = _mdlUser.Email;
                    usrToUpdate.Phone = _mdlUser.Phone;
                    if (_mdlUser.SMSCodeRequired == null)
                    {
                        usrToUpdate.SMSCodeRequired = false;
                    }
                    usrToUpdate.SMSCodeRequired = _mdlUser.SMSCodeRequired;

                    if (IsFacebook == true)
                    {
                        usrToUpdate.SocialMediaKey = _mdlUser.SocialMediaKey;
                    }
                    if (IsGoogle == true)
                    {
                        // usrToUpdate.GoogleKey = _mdlUser.GoogleKey;
                    }

                    usrToUpdate.FileName = _mdlUser.FileName;
                    usrToUpdate.Type = "A";
                    usrToUpdate.Address = _mdlUser.Address;
                    usrToUpdate.FullName = _mdlUser.FullName;
                    usrToUpdate.Latitude = _mdlUser.Latitude;
                    usrToUpdate.Longitude = _mdlUser.Longitude;
                    usrToUpdate.Password = _mdlUser.Password;
                    usrToUpdate.CompanyID = null;
                    usrToUpdate.RoleID = 6;
                    usrToUpdate.IsVerified = false;
                    db.Repository<User>().Update(usrToUpdate);


                    object TotalGPOfUser = db.ExtRepositoryFor<UsersRepository>().GetUserTotalGP(usrToUpdate.ID);
                    int GC = 0;
                    decimal? GPRedeemedOfUser = 0;
                    decimal? GPRedeemableOfUser = 0;
                    if (TotalGPOfUser != null)
                    {
                        GC = Convert.ToInt32(TotalGPOfUser.GetType().GetProperty("TotalGP").GetValue(TotalGPOfUser));

                        GPRedeemedOfUser = db.Repository<GCRedeem>().GetAll().Where(x => x.UserID == usrToUpdate.ID).ToList().Sum(y => y.GCRedeemed);
                        GPRedeemableOfUser = GC - GPRedeemedOfUser;

                    }

                    var CheckGPNInUserAreaUser = db.Repository<Organization>().GetAll().Where(x => x.AreaID == usrToUpdate.AreaID).FirstOrDefault();
                    bool IsConnectedWithGPNUser = false;
                    string GPNConnectionMessageUser = "";
                    if (CheckGPNInUserAreaUser != null)
                    {
                        var CheckUserMembershipInGPN = db.Repository<Member>().GetAll().Where(x => x.OrgId == CheckGPNInUserAreaUser.ID && x.UserID == usrToUpdate.ID && x.IsActive == true).FirstOrDefault();
                        if (CheckUserMembershipInGPN == null)
                        {

                            Member mdlMember = new Member();
                            mdlMember.Name = usrToUpdate.FullName;
                            mdlMember.Designation = "Membership";
                            mdlMember.UserID = usrToUpdate.ID;
                            mdlMember.CreatedBy = usrToUpdate.ID;
                            mdlMember.OrgId = CheckGPNInUserAreaUser.ID;
                            mdlMember.IsCurrentlyWorking = true;
                            mdlMember.FileName = "";
                            mdlMember.IsActive = true;
                            mdlMember.IsVerified = true;
                            db.Repository<Member>().Insert(mdlMember);
                            db.Save();
                            // set flags
                            IsConnectedWithGPNUser = true;
                            //   GPNConnectionMessage = string.Format("Congratulations! You are now connected with {GPN} GPN".Replace("{GPN}", CheckGPNInUserArea.Name);
                            GPNConnectionMessageUser = String.Format("Congratulations! You are now connected with {0} GPN",
                                  CheckGPNInUserAreaUser.Name);
                        }

                    }

                    City city = db.Repository<City>().GetAll().Where(x => x.ID == usrToUpdate.CityId).FirstOrDefault();

                    String CityName = "";
                    if (city != null)
                    {
                        CityName = city.CityName;
                    }
                    object mdlnewusr = new object();
                    var usrtoken = JwtManager.CreateToken(usrToUpdate, out mdlnewusr);
                    return ServiceResponse.SuccessReponse(new TokenViewModel
                    {
                        Token = usrtoken,
                        ID = usrToUpdate.ID,
                        FullName = usrToUpdate.FullName,
                        Address = usrToUpdate.Address,
                        Email = usrToUpdate.Email,
                        Phone = usrToUpdate.Phone,
                        FileName = usrToUpdate.FileName,
                        Longitude = Convert.ToDecimal(usrToUpdate.Longitude),
                        Latitude = Convert.ToDecimal(usrToUpdate.Latitude),
                        GreenPoints = Convert.ToInt32(usrToUpdate.GreenPoints),
                        Password = usrToUpdate.Password,
                        City = CityName,
                        DeviceToken = usrToUpdate.DeviceToken,
                        AreaID = usrToUpdate.AreaID,
                        UnionCouncil = usrToUpdate.UnionCouncil,
                        CompanyID = usrToUpdate.CompanyID,
                        CityId = usrToUpdate.CityId,
                        BusinessKey = db.ExtRepositoryFor<CommonRepository>().GetBusinessKeyCompanyIdFrom(usrToUpdate.CompanyID),
                        IsConnectedWithGPN = IsConnectedWithGPNUser,
                        GPNConnectionMessage = GPNConnectionMessageUser,
                        RedeemedPoints = GPRedeemedOfUser,
                        RedeemablePoints = GPRedeemableOfUser,
                        WalletBalance = usrToUpdate.WalletBalance ?? 0,
                        QRCode = usrToUpdate.QRCode

                    },

                   MessageEnum.RecordFoundSuccessfully);

                }
            }

            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<TokenViewModel>(exp);
            }
        }
        #endregion

        //private string JwtTokenBuilder(User user)
        //{
        //    List<Claim> claims = new List<Claim>
        //    {
        //        new Claim("Id", user.ID.ToString()),
        //        new Claim("Email", user.Email),
        //        new Claim("Role", user.RoleID.ToString())
        //    };

        //    var dd = System.Configuration.ConfigurationManager.AppSettings[AppSettings.SECURITY_KEY].ToString();
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(dd));
        //    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var jwtToken = new JwtSecurityToken(issuer: System.Configuration.ConfigurationManager.AppSettings[AppSettings.SECURITY_ISSUER].ToString(),
        //                                        audience: System.Configuration.ConfigurationManager.AppSettings[AppSettings.SECURITY_AUDIENCE].ToString(),
        //                                        signingCredentials: credentials,
        //                                        expires: DateTime.Now.AddYears(1),
        //                                        claims: claims);

        //    var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        //    return token;


        //}
        [HttpPost]
        public ResponseObject<TokenViewModel> LoginUser([FromBody] User mdlUser)
        {
            try
            {
                if (mdlUser.Phone == "" || mdlUser.Password == "")
                {
                    return ServiceResponse.ErrorReponse<TokenViewModel>(MessageEnum.DefaultParametersCanNotBeNull);
                }
                else
                {
                    var user = db.ExtRepositoryFor<UsersRepository>().GetUserByPhoneandPassword(mdlUser.Phone, mdlUser.Password);


                    int GP = 0;
                    decimal? GPRedeemed = 0;
                    decimal? GPRedeemable = 0;

                    int MonthWiseGP = 0;

                    if (user != null)
                    {
                        if (user.RoleID != (int)UserRoleTypeEnum.Admin)
                        {
                            object TotalGP = db.ExtRepositoryFor<UsersRepository>().GetUserTotalGP(user.ID);

                            if (TotalGP != null)
                            {
                                GP = Convert.ToInt32(TotalGP.GetType().GetProperty("TotalGP").GetValue(TotalGP));

                                GPRedeemed = db.Repository<GCRedeem>().GetAll().Where(x => x.UserID == user.ID).ToList().Sum(y => y.GCRedeemed);
                                GPRedeemable = GP - GPRedeemed;

                                //Minus the redeemed GC of user
                                //var UserRedeems = db.Repository<GCRedeem>().GetAll().Where(x => x.UserID == user.ID).ToList();
                                //decimal? RedeemedGC = 0;
                                //int RedemmUserGC = 0;
                                //if(UserRedeems.Count > 0)
                                //{
                                //    RedeemedGC =   UserRedeems.Sum(x => x.GCRedeemed);
                                //    RedemmUserGC = Convert.ToInt32(RedeemedGC);
                                //    GP = GP - RedemmUserGC;
                                //}
                                //End
                            }

                            object lst = db.ExtRepositoryFor<UsersRepository>().GetUserCurrentMonthGP(user.ID);
                            if (lst != null)
                            {
                                MonthWiseGP = Convert.ToInt32(lst.GetType().GetProperty("MonthlyGP").GetValue(lst));
                            }
                        }
                        object counts = db.ExtRepositoryFor<UsersRepository>().GetCountsByUserID(user.ID);

                        int ChildrenCount = 0;
                        int EmployeesCount = 0;
                        int MembersCount = 0;
                        if (counts != null)
                        {
                            ChildrenCount = Convert.ToInt32(counts.GetType().GetProperty("ChildrenCount").GetValue(counts));
                            EmployeesCount = Convert.ToInt32(counts.GetType().GetProperty("EmployeesCount").GetValue(counts));
                            MembersCount = Convert.ToInt32(counts.GetType().GetProperty("MembersCount").GetValue(counts));
                        }

                        object mdlnew = new object();
                        var token = JwtManager.CreateToken(user, out mdlnew);

                        City mdlCity = db.Repository<City>().GetAll().Where(x => x.ID == user.CityId).FirstOrDefault();

                        String CityName = "";
                        if (mdlCity != null)
                        {
                            CityName = mdlCity.CityName;
                        }
                        //  int TotalGP = db.ExtRepositoryFor<UsersRepository>().get

                        //   return ServiceResponse.SuccessReponse(new TokenViewModel { UserId = user.Id.ToString(), UserName = user.FullName, FileName = user.ImagePath, Token = token }, MessageEnum.UserAuthorizedSuccessFully);

                        return ServiceResponse.SuccessReponse(new TokenViewModel
                        {
                            Token = token,
                            ID = user.ID,
                            FullName = user.FullName,
                            Address = user.Address,
                            Email = user.Email,
                            Phone = user.Phone,
                            FileName = user.FileName,
                            Longitude = Convert.ToDecimal(user.Longitude),
                            Latitude = Convert.ToDecimal(user.Latitude),
                            GreenPoints = GP,
                            City = CityName,
                            IsVerified = Convert.ToBoolean(user.IsVerified),
                            DeviceToken = user.DeviceToken,
                            ChildrenCount = ChildrenCount,
                            MembersCount = MembersCount,
                            EmployeesCount = EmployeesCount,
                            CurrentMonthGP = MonthWiseGP,
                            AreaID = user.AreaID,
                            UnionCouncil = user.UnionCouncil,
                            CompanyID = user.CompanyID,
                            CityId = user.CityId,
                            RedeemedPoints = GPRedeemed,
                            RedeemablePoints = GPRedeemable,
                            WalletBalance = user.WalletBalance ?? 0,
                            BusinessKey = db.ExtRepositoryFor<CommonRepository>().GetBusinessKeyCompanyIdFrom(user.CompanyID),
                            QRCode = user.QRCode,
                            RemainingAmount = user.RemainingAmount,
                            PaidAmount = Convert.ToDecimal( user.WalletBalance),
                            DueDate = DateTime.UtcNow

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



        [HttpPost]
        public ResponseObject<TokenViewModel> Login([FromBody] User mdlUser)
        {
            try
            {
                if (mdlUser.Email == null || mdlUser.Password == "")
                {
                    return ServiceResponse.ErrorReponse<TokenViewModel>(MessageEnum.DefaultParametersCanNotBeNull);

                }
                else
                {
                    var user = db.Repository<User>().GetAll().Where(x => x.Email == mdlUser.Email && x.Password == mdlUser.Password).FirstOrDefault();
                    //  var user = db.ExtRepositoryFor<UsersRepository>().GetUserDetaisForWeb(mdlUser.Email, mdlUser.Password);

                    if (user != null && (user.RoleID == (int)UserRoleTypeEnum.Admin || user.RoleID == (int)UserRoleTypeEnum.BusinessAdmin || user.RoleID == (int)UserRoleTypeEnum.OrganizationAdmin || user.RoleID == (int)UserRoleTypeEnum.SchoolAdmin || user.RoleID == (int)UserRoleTypeEnum.SubSchoolAdmin || user.RoleID == (int)UserRoleTypeEnum.OrganizationAdmin || user.RoleID == (int)UserRoleTypeEnum.SubOrganizationAdmin || user.RoleID == (int)UserRoleTypeEnum.BusinessAdmin || user.RoleID == (int)UserRoleTypeEnum.SubBusinessAdmin || user.RoleID == (int)UserRoleTypeEnum.CompanyAdmin || user.RoleID == (int)UserRoleTypeEnum.GOIBusinessStaffAdmin || user.RoleID == (int)UserRoleTypeEnum.WWF))
                    {
                        object mdlnew = new object();
                        var token = JwtManager.CreateToken(user, out mdlnew);

                        return ServiceResponse.SuccessReponse(new TokenViewModel
                        {
                            Token = token,
                            ID = user.ID,
                            FullName = user.FullName,
                            Address = user.Address,
                            Email = user.Email,
                            Phone = user.Phone,
                            FileName = user.FileName,
                            Longitude = Convert.ToDecimal(user.Longitude),
                            Latitude = Convert.ToDecimal(user.Latitude),
                            QRCode = user.QRCode,
                            GreenPoints = Convert.ToInt32(user.GreenPoints),
                            City = user.City,
                            IsVerified = Convert.ToBoolean(user.IsVerified),
                            DeviceToken = user.DeviceToken,
                            RoleID = (int)user.RoleID,
                            CompanyID = user.CompanyID,
                            CityId = user.CityId,
                            // Type = user.Type
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

        //[HttpGet]
        //public ResponseObject<List<MapMarker>> GetUsersGreenPointStatus()
        //{
        //    try
        //    {
        //        var lstUsersDetails = _IUWork.GetModelData<Users>("Users")?.ConvertAll(p => new MapMarker
        //        {
        //            Cash = p.Cash,
        //            GreenPoints = p.GreenPoints,
        //            Latitude = p.Latitude,
        //            Longitude = p.Longitude,
        //            Status = p.Status
        //        });
        //        if (lstUsersDetails?.Count > 0)
        //        {
        //            return ServiceResponse.SuccessReponse(lstUsersDetails, MessageEnum.DefaultSuccessMessage);
        //        }
        //        else
        //        {
        //            return ServiceResponse.SuccessReponse(lstUsersDetails, MessageEnum.DefaultErrorMessage);
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        return ServiceResponse.ErrorReponse<List<MapMarker>>(exp);
        //    }
        //}


        public async Task<ResponseObject<bool>> ChangePassword(ChangePasswordveiwModel model)
        {
            int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
            var usrFromDB = db.Repository<User>().FindById(UserID);
            if (usrFromDB != null)
            {
                if (usrFromDB.Password.ToLower() == model.OldPassword.ToLower())
                {
                    usrFromDB.Password = model.NewPassword;
                    db.Repository<User>().Update(usrFromDB);
                    db.Save();
                    return ServiceResponse.SuccessReponse(true, MessageEnum.DefaultSuccessMessage);
                }
                else
                {
                    return ServiceResponse.SuccessReponse(false, MessageEnum.DefaultErrorMessage);
                }
            }
            else
            {
                return ServiceResponse.SuccessReponse(false, MessageEnum.RecordNotFound);
            }


        }


        [HttpGet()]
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


                    User mdlUser = db.ExtRepositoryFor<UsersRepository>().GetBasicUserDetails(Phone);

                    if (mdlUser != null)
                    {
                        SMSNotifyEvent _events = new SMSNotifyEvent();
                        _events.Parameters.Add("Email", mdlUser.Email);
                        _events.AddSMSNotifyEvent((long)NotificationEventConstants.Users.SMSSendtoUserForgotPassword, mdlUser.Phone);

                        NotifyEvent _event = new NotifyEvent();
                        _event.Parameters.Add("Email", mdlUser.Email);
                        _event.AddNotifyEvent((long)NotificationEventConstants.Users.EmailSendtoUserForgotPassword, Convert.ToString(mdlUser.ID));

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

        [HttpPost]
        public async Task<ResponseObject<bool>> ReportAProblem(ReportProblemViewModel mdlUser)
        {
            try
            {

                NotifyEvent _events = new NotifyEvent();
                _events.Parameters.Add("Email", "infoamalfrolife@gmail.com");
                _events.Parameters.Add("Phone", mdlUser.Phone);
                _events.Parameters.Add("Description", mdlUser.Problem);
                _events.Parameters.Add("IssueType", mdlUser.Subject);
                _events.AddNotifyEvent((long)NotificationEventConstants.Common.EmailSendtoAdminReportAProblem, "");
                SMSNotifyEvent _event = new SMSNotifyEvent();
                _event.Parameters.Add("Phone", mdlUser.Phone);
                _event.AddSMSNotifyEvent((long)NotificationEventConstants.Common.SMSSendtoUserForReportAProblem, Convert.ToString(""));
                return ServiceResponse.SuccessReponse(true, MessageEnum.ProblemReportedSuccessfully);

            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpPost]
        public async Task<ResponseObject<bool>> InviteAFriend(string Email = null, string Phone = null)
        {
            try
            {
                if (string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(Phone))
                    return ServiceResponse.SuccessReponse(true, MessageEnum.EmailPhoneMissingMessage);


                if (!string.IsNullOrEmpty(Email))
                {
                    NotifyEvent _event = new NotifyEvent();
                    _event.AddNotifyEvent((long)NotificationEventConstants.InviteFriend.EmailSendtoFriend, Email);

                }

                if (!string.IsNullOrEmpty(Phone))
                {
                    SMSNotifyEvent _events = new SMSNotifyEvent();
                    _events.AddSMSNotifyEvent((long)NotificationEventConstants.InviteFriend.SMSSendtoFriend, Phone);
                }

                return ServiceResponse.SuccessReponse(true, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        //[Authorize]
        //[HttpGet("GetUserDetail")]
        //public async Task<ResponseObject<UserDetailViewModel>> GetUserDetail(string id)
        //{
        //    try
        //    {
        //        var user = await _IUWork.FindOneByID<Users>(id, CollectionNames.USERS);

        //        if (user != null)
        //        {
        //            var regiftCount = (int)_IUWork.GetModelByUserID<Regift>(user.Id.ToString(), CollectionNames.REGIFT)?.Count();
        //            var reportCount = (int)_IUWork.GetModelByUserID<Report>(user.Id.ToString(), CollectionNames.Report)?.Count();
        //            var recycleCount = (int)_IUWork.GetModelByUserID<MrClean>(user.Id.ToString(), CollectionNames.RECYCLE)?.Count();

        //            return ServiceResponse.SuccessReponse(new UserDetailViewModel
        //            {
        //                MemberSince = user.CreatedAt.ToString("MMM dd, yyyy"),
        //                FullName = user.FullName,
        //                Email = user.Email,
        //                Address = user.Address,
        //                FileName = user.FileName,
        //                Latitude = user.Latitude,
        //                Longitude = user.Longitude,
        //                Phone = user.Phone,
        //                UserType = string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Phone) ? "Basic" : "Registered",

        //                GreenPoints = user.GreenPoints,
        //                BinCount = (int)user.BuyBinDetails?.Count(),
        //                RecycleCount = recycleCount,
        //                ReduceCount = (int)user.Reduce?.Count(),
        //                RefuseCount = (int)user.Refuse?.Count(),
        //                ReplantCount = (int)user.Replant?.Count(),
        //                ReportCount = reportCount,
        //                ReuseCount = (int)user.Reuse?.Count(),
        //                RegiftCount = regiftCount

        //            }, MessageEnum.UserDetailFoundSuccessfully);
        //        }
        //        return ServiceResponse.SuccessReponse<UserDetailViewModel>(null, MessageEnum.UserDetailNotFoundSuccessfully);
        //    }
        //    catch (Exception exp)
        //    {
        //        return ServiceResponse.ErrorReponse<UserDetailViewModel>(exp);
        //    }
        //}


        [HttpPost]
        public async Task<ResponseObject<TokenViewModel>> LoginThroughSocialMedia()
        {
            try
            {
                decimal? GPRedeemed = 0;
                decimal? GPRedeemable = 0;
                bool IsTrue = false;
                string socialMediaKey = string.Empty;
                string socialMediaName = string.Empty;
                bool flag = false;
                string SMediaKey = string.Empty;
                //  User mdlUserDetail = new User();

                User mdlUser = new User();
                string FileName = string.Empty;
                mdlUser.RoleID = (int)UserRoleTypeEnum.MobileUser;

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["fullName"]))
                    mdlUser.FullName = HttpContext.Current.Request.Form["fullName"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["email"]))
                    mdlUser.Email = HttpContext.Current.Request.Form["email"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["socialMediaKey"]))
                    mdlUser.SocialMediaKey = HttpContext.Current.Request.Form["socialMediaKey"];
                socialMediaKey = mdlUser.SocialMediaKey;

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["socialMediaName"]))
                    mdlUser.SocialMediaName = HttpContext.Current.Request.Form["socialMediaName"];
                socialMediaName = mdlUser.SocialMediaName;

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["fileName"]))
                    mdlUser.FileName = HttpContext.Current.Request.Form["fileName"];

                if (mdlUser.SocialMediaName.ToLower() == "google")
                {
                    flag = true;
                    SMediaKey = mdlUser.SocialMediaKey;

                    // mdlUser.GoogleName = mdlUser.SocialMediaName;
                    //  mdlUser.GoogleKey = mdlUser.SocialMediaKey;
                    //   mdlUser.SocialMediaName = "";
                    // mdlUser.SocialMediaKey = "";
                }
                if (mdlUser.SocialMediaName.ToLower() == "facebook")
                {
                    flag = false;
                    SMediaKey = mdlUser.SocialMediaKey;

                    //  mdlUser.SocialMediaName = mdlUser.SocialMediaName;
                    //   mdlUser.SocialMediaKey = mdlUser.SocialMediaKey;
                    //  mdlUser.GoogleName = "";
                    // mdlUser.GoogleKey = "";
                }


                User mdlUserKey = new User();
                //if (mdlUser.GoogleName != "")
                //{
                //    mdlUserKey = db.Repository<User>().GetAll().Where(x => x.GoogleKey == mdlUser.GoogleKey).FirstOrDefault();
                //}
                //else if (mdlUser.SocialMediaName != "")
                //{
                //    mdlUserKey = db.Repository<User>().GetAll().Where(x => x.SocialMediaKey == mdlUser.SocialMediaKey).FirstOrDefault();
                //}
                if (flag == true)
                {
                    //  mdlUserKey = db.Repository<User>().GetAll().Where(x => x.GoogleKey == SMediaKey).FirstOrDefault();
                }
                else
                {
                    mdlUserKey = db.Repository<User>().GetAll().Where(x => x.SocialMediaKey == SMediaKey).FirstOrDefault();
                }

                if (mdlUserKey != null)
                {
                    int GP = 0;
                    object TotalGP = db.ExtRepositoryFor<UsersRepository>().GetUserTotalGP(mdlUserKey.ID);

                    if (TotalGP != null)
                    {
                        GP = Convert.ToInt32(TotalGP.GetType().GetProperty("TotalGP").GetValue(TotalGP));

                        GPRedeemed = db.Repository<GCRedeem>().GetAll().Where(x => x.UserID == mdlUserKey.ID).ToList().Sum(y => y.GCRedeemed);
                        GPRedeemable = GP - GPRedeemed;
                    }

                    Int32 ID = mdlUserKey.ID;
                    mdlUserKey.RoleID = mdlUser.RoleID;
                    mdlUserKey.Type = "A";
                    object mdlnew = new object();
                    // var token = JwtTokenBuilder(mdlUserDetail);
                    var token = JwtManager.CreateToken(mdlUserKey, out mdlnew);

                    City city = db.Repository<City>().GetAll().Where(x => x.ID == mdlUserKey.CityId).FirstOrDefault();

                    String CityName = "";
                    if (city != null)
                    {
                        CityName = city.CityName;
                    }

                    return ServiceResponse.SuccessReponse(new TokenViewModel
                    {
                        Token = token,
                        ID = mdlUserKey.ID,
                        FullName = mdlUserKey.FullName,
                        Address = mdlUserKey.Address,
                        Email = mdlUserKey.Email,
                        Phone = mdlUserKey.Phone,
                        FileName = mdlUserKey.FileName,
                        Longitude = Convert.ToDecimal(mdlUserKey.Longitude),
                        Latitude = Convert.ToDecimal(mdlUserKey.Latitude),
                        QRCode = mdlUserKey.QRCode,
                        GreenPoints = GP,
                        Password = mdlUserKey.Password,
                        IsVerified = Convert.ToBoolean(mdlUserKey.IsVerified),
                        City = CityName,
                        CompanyID = mdlUserKey.CompanyID,
                        CityId = mdlUserKey.CityId,
                        RedeemedPoints = GPRedeemed,
                        AreaID = mdlUserKey.AreaID,
                        RedeemablePoints = GPRedeemable,
                        WalletBalance = mdlUserKey.WalletBalance ?? 0,
                        FacebookKey = mdlUserKey.SocialMediaKey,
                        //   GoogleKey = mdlUserKey.GoogleKey

                    }, MessageEnum.UserAuthorizedSuccessFully);

                }
                else
                {
                    string facebookKey;
                    string googleKey;
                    if (socialMediaName.ToLower() == "facebook")
                    {
                        facebookKey = socialMediaKey;
                        googleKey = null;
                    }
                    else
                    {
                        facebookKey = null;
                        // googleKey = socialMediaKey;
                    }

                    return ServiceResponse.SuccessReponse(new TokenViewModel
                    {

                        FullName = mdlUser.FullName,
                        Email = mdlUser.Email,
                        Phone = mdlUser.Phone,
                        FileName = mdlUser.FileName,
                        FacebookKey = facebookKey,
                        //GoogleKey = googleKey

                    }, MessageEnum.UserAuthorizedSuccessFully);
                }

            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<TokenViewModel>(exp);
            }

        }
        #region|LoginThroughSocailMedia Already working code|
        [HttpPost]
        public async Task<ResponseObject<TokenViewModel>> KKLoginThroughSocialMedia1()
        {
            try
            {
                decimal? GPRedeemed = 0;
                decimal? GPRedeemable = 0;
                bool IsTrue = false;
                User mdlUserDetail = new User();

                User mdlUser = new User();
                //if (!Request.Content.IsMimeMultipartContent())
                //{
                //    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                //}
                string FileName = string.Empty;
                //HttpPostedFile file = HttpContext.Current.Request.Files[0];
                //FileName = await FileOpsHelper.UploadFileNew(file);
                mdlUser.RoleID = (int)UserRoleTypeEnum.MobileUser;

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["fullName"]))
                    mdlUser.FullName = HttpContext.Current.Request.Form["fullName"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["email"]))
                    mdlUser.Email = HttpContext.Current.Request.Form["email"].ToString();

                //if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["address"]))
                //    mdlUser.Address = HttpContext.Current.Request.Form["address"].ToString();

                //if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["phone"]))
                //    mdlUser.Phone = HttpContext.Current.Request.Form["phone"].ToString();

                //if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["password"]))
                //    mdlUser.Password = HttpContext.Current.Request.Form["password"].ToString();

                //if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["longitude"]))
                //    mdlUser.Longitude = Convert.ToDecimal(HttpContext.Current.Request.Form["longitude"]);

                //if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["latitude"]))
                //    mdlUser.Latitude = Convert.ToDecimal(HttpContext.Current.Request.Form["latitude"]);

                //if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["city"]))
                //    mdlUser.City = HttpContext.Current.Request.Form["city"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["socialMediaKey"]))
                    mdlUser.SocialMediaKey = HttpContext.Current.Request.Form["socialMediaKey"];


                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["socialMediaName"]))
                    mdlUser.SocialMediaName = HttpContext.Current.Request.Form["socialMediaName"];


                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["fileName"]))
                    mdlUser.FileName = HttpContext.Current.Request.Form["fileName"];

                if (mdlUser != null)
                {

                    //if (mdlUser.OSType == OSTypeEnum.Andriod.GetDescription())
                    //    mdlUser.OSType = OSTypeEnum.Andriod.GetDescription();
                    //else
                    //    mdlUser.OSType = OSTypeEnum.ios.GetDescription();

                    User mdlUserKey = db.ExtRepositoryFor<UsersRepository>().GetUserDetailsBySocialMediaKey(mdlUser.SocialMediaKey);

                    if (mdlUserKey != null)
                    {

                        Int32 UserId = mdlUserKey.ID;


                        if (string.IsNullOrEmpty(mdlUser.FullName))
                            mdlUser.FullName = mdlUserKey.FullName;

                        if (string.IsNullOrEmpty(mdlUser.Email))
                            mdlUser.Email = mdlUserKey.Email;

                        if (string.IsNullOrEmpty(mdlUser.FileName))
                            mdlUser.FileName = mdlUserKey.FileName;

                        //if (string.IsNullOrEmpty(mdlUser.Phone))
                        //    mdlUser.Phone = mdlUserDetail.Phone;

                        if (string.IsNullOrEmpty(mdlUser.SocialMediaKey))
                            mdlUser.SocialMediaKey = mdlUserKey.SocialMediaKey;

                        if (string.IsNullOrEmpty(mdlUser.SocialMediaName))
                            mdlUser.SocialMediaName = mdlUserKey.SocialMediaName;

                        //if (string.IsNullOrEmpty(mdlUser.DeviceID))
                        //    mdlUser.DeviceID = mdlUserDetail.DeviceID;

                        //if (mdlUser.Latitude == 0)
                        //    mdlUser.Latitude = mdlUserDetail.Latitude;

                        //if (mdlUser.Longitude == 0)
                        //    mdlUser.Longitude = mdlUserDetail.Longitude;

                        //if (string.IsNullOrEmpty(mdlUser.City))
                        //    mdlUser.City = mdlUserDetail.City;

                        //if (string.IsNullOrEmpty(mdlUser.Address))
                        //    mdlUser.Address = mdlUserDetail.Address;

                        //if (string.IsNullOrEmpty(mdlUser.Password))
                        //    mdlUser.Password = mdlUserDetail.Password;


                        mdlUserKey.FullName = mdlUser.FullName;
                        mdlUserKey.Email = mdlUser.Email;
                        mdlUserKey.FileName = mdlUser.FileName;
                        mdlUserKey.Type = "A";


                        //mdlUserDetail.OSType = mdlUser.OSType;
                        mdlUserKey.SocialMediaKey = mdlUser.SocialMediaKey;
                        mdlUserKey.SocialMediaName = mdlUser.SocialMediaName;

                        mdlUserKey.RoleID = (int)UserRoleTypeEnum.MobileUser;
                        db.Repository<User>().Update(mdlUserKey);
                        db.Save();

                    }
                    else
                    {

                        User mdlUserPhone = db.ExtRepositoryFor<UsersRepository>().GetBasicUserDetails(mdlUser.Phone);

                        if (mdlUserPhone != null)
                        {
                            Int32 UserId = mdlUserPhone.ID;

                            mdlUserPhone.FullName = mdlUser.FullName;
                            //mdlUserDetail.Address = mdlUser.Address;
                            //mdlUserDetail.Phone = mdlUser.Phone;
                            //mdlUserDetail.Password = mdlUser.Password;
                            mdlUserPhone.Email = mdlUser.Email;
                            //mdlUserDetail.Longitude = mdlUser.Longitude;
                            //mdlUserDetail.Latitude = mdlUser.Latitude;
                            //mdlUserDetail.City = mdlUser.City;
                            mdlUserPhone.FileName = mdlUser.FileName;
                            mdlUserPhone.SocialMediaKey = mdlUser.SocialMediaKey;
                            mdlUserPhone.SocialMediaName = mdlUser.SocialMediaName;
                            // mdlUserDetail.OSType = mdlUser.OSType;
                            mdlUserPhone.RoleID = (int)UserRoleTypeEnum.MobileUser;
                            db.Repository<User>().Update(mdlUserPhone);
                            db.Save();

                        }
                        else
                        {
                            IsTrue = true;
                            User UserProperty = new User();
                            UserProperty.FullName = mdlUser.FullName;
                            UserProperty.FileName = mdlUser.FileName;
                            //UserProperty.DeviceID = mdlUser.DeviceID;
                            //UserProperty.OSType = mdlUser.OSType;
                            UserProperty.Email = mdlUser.Email;
                            UserProperty.Type = "A";
                            //UserProperty.Phone = mdlUser.Phone;
                            //UserProperty.Longitude = mdlUser.Longitude;
                            //UserProperty.Latitude = mdlUser.Latitude;
                            UserProperty.SocialMediaKey = mdlUser.SocialMediaKey;
                            //  UserProperty.City = mdlUser.City;
                            //  UserProperty.Password = mdlUser.Password;
                            UserProperty.SocialMediaName = mdlUser.SocialMediaName;
                            UserProperty.RoleID = (int)UserRoleTypeEnum.MobileUser;
                            db.Repository<User>().Insert(UserProperty);
                            db.Save();
                        }
                    }


                }
                else
                    return ServiceResponse.ErrorReponse<TokenViewModel>(MessageEnum.DefaultParametersCanNotBeNull);


                mdlUserDetail = db.ExtRepositoryFor<UsersRepository>().GetUserDetailsBySocialMediaKey(mdlUser.SocialMediaKey);
                int GP = 0;
                object TotalGP = db.ExtRepositoryFor<UsersRepository>().GetUserTotalGP(mdlUserDetail.ID);

                if (TotalGP != null)
                {
                    GP = Convert.ToInt32(TotalGP.GetType().GetProperty("TotalGP").GetValue(TotalGP));

                    GPRedeemed = db.Repository<GCRedeem>().GetAll().Where(x => x.UserID == mdlUserDetail.ID).ToList().Sum(y => y.GCRedeemed);
                    GPRedeemable = GP - GPRedeemed;
                }


                Int32 ID = mdlUserDetail.ID;
                mdlUserDetail.RoleID = mdlUser.RoleID;
                mdlUserDetail.Type = "A";
                object mdlnew = new object();
                // var token = JwtTokenBuilder(mdlUserDetail);
                var token = JwtManager.CreateToken(mdlUserDetail, out mdlnew);

                City city = db.Repository<City>().GetAll().Where(x => x.ID == mdlUserDetail.CityId).FirstOrDefault();

                String CityName = "";
                if (city != null)
                {
                    CityName = city.CityName;
                }

                return ServiceResponse.SuccessReponse(new TokenViewModel
                {
                    Token = token,
                    ID = mdlUserDetail.ID,
                    FullName = mdlUserDetail.FullName,
                    Address = mdlUserDetail.Address,
                    Email = mdlUserDetail.Email,
                    Phone = mdlUserDetail.Phone,
                    FileName = mdlUserDetail.FileName,
                    Longitude = Convert.ToDecimal(mdlUserDetail.Longitude),
                    Latitude = Convert.ToDecimal(mdlUserDetail.Latitude),
                    QRCode = mdlUserDetail.QRCode,
                    GreenPoints = GP,
                    Password = mdlUserDetail.Password,
                    IsVerified = Convert.ToBoolean(mdlUserDetail.IsVerified),
                    City = CityName,
                    CompanyID = mdlUserDetail.CompanyID,
                    CityId = mdlUserDetail.CityId,
                    RedeemedPoints = GPRedeemed,
                    RedeemablePoints = GPRedeemable,
                    WalletBalance = mdlUserDetail.WalletBalance ?? 0,
                }, MessageEnum.UserAuthorizedSuccessFully);

            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<TokenViewModel>(exp);
            }
        }
        #endregion



        [HttpGet]
        public async Task<ResponseObject<List<User>>> GetUsersForWeb()
        {
            try
            {
                var users = db.Repository<User>().GetAll().ToList();

                return ServiceResponse.SuccessReponse(users, MessageEnum.UserProfileUpdated);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<User>>(exp);
            }
        }

        [HttpGet]
        public async Task<ResponseObject<List<User>>> GetAdminUsers()
        {
            try
            {
                var users = db.Repository<User>().GetAll().Where(x => x.RoleID != (int)UserRoleTypeEnum.MobileUser).ToList();

                return ServiceResponse.SuccessReponse(users, MessageEnum.RecordFoundSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<User>>(exp);
            }
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<ResponseObject<List<MyGPNModel>>> GetMyGPN(string Type = "")
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                List<MyGPNModel> lstMyGPNModel = new List<MyGPNModel>();

                if (Type == Constants.GPNTypes.SCHOOL)
                {
                    List<School> mdlSchool = db.ExtRepositoryFor<SchoolRepository>().GetChildSchoolByUserID(UserID);

                    foreach (School school in mdlSchool)
                    {
                        MyGPNModel gpn = new MyGPNModel();
                        gpn.ID = school.ID;
                        gpn.FileName = school.FileName;
                        gpn.Name = school.Name;
                        gpn.GreenPoints = school.GreenPoints;
                        gpn.Level = school.Level;
                        gpn.Type = Constants.GPNTypes.SCHOOL;

                        lstMyGPNModel.Add(gpn);
                    }
                }
                else if (Type == Constants.GPNTypes.ORGANIZATION)
                {
                    List<Organization> mdlOrganization = db.ExtRepositoryFor<OrganizationRepository>().GetEmployeeOrganizationByUserID(UserID);
                    foreach (Organization org in mdlOrganization)
                    {

                        MyGPNModel gpn = new MyGPNModel();
                        gpn.ID = org.ID;
                        gpn.FileName = org.FileName;
                        gpn.Name = org.Name;
                        gpn.GreenPoints = (int)org.GreenPoints;
                        gpn.Level = org.Level;
                        gpn.Type = Constants.GPNTypes.ORGANIZATION;

                        lstMyGPNModel.Add(gpn);
                    }
                }

                else if (Type == Constants.GPNTypes.Business)
                {

                    List<Business> mdlNGO = db.ExtRepositoryFor<NGORepository>().GetEmployeeNGOByUserID(UserID);
                    foreach (Business ngo in mdlNGO)
                    {
                        MyGPNModel gpn = new MyGPNModel();
                        gpn.ID = ngo.ID;
                        gpn.FileName = ngo.FileName;
                        gpn.Name = ngo.Name;
                        gpn.GreenPoints = Convert.ToInt32(ngo.GreenPoints);
                        gpn.Level = ngo.Level;
                        gpn.Type = Constants.GPNTypes.Business;
                        lstMyGPNModel.Add(gpn);
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

        [HttpGet]
        public ResponseObject<List<object>> GetGPNContacts(int ID, string GPNType)
        {
            try
            {
                List<object> lstUsers = new List<object>();

                if (GPNType == Constants.GPNTypes.SCHOOL)
                    lstUsers = db.ExtRepositoryFor<UsersRepository>().GetChildrenBySchoolID(ID);
                else if (GPNType == Constants.GPNTypes.Business)
                    lstUsers = db.ExtRepositoryFor<UsersRepository>().GetEmployeesByBusinessID(ID);
                else if (GPNType == Constants.GPNTypes.ORGANIZATION)
                    lstUsers = db.ExtRepositoryFor<UsersRepository>().GetMembersByOrgID(ID);

                return ServiceResponse.SuccessReponse(lstUsers, MessageEnum.UserProfileUpdated);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }


        #region|VerifiedSMSCode Already functional cod|
        [HttpPost]
        public async Task<ResponseObject<bool>> VerifiedSMSCode(int ID)
        {
            try
            {
                // int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                User mdlUser = db.Repository<User>().FindById(ID);
                mdlUser.IsVerified = true;
                db.Repository<User>().Update(mdlUser);
                db.Save();

                SMSNotifyEvent _events = new SMSNotifyEvent();
                _events.Parameters.Add("SMSCode", "");
                _events.AddSMSNotifyEvent((long)NotificationEventConstants.Users.UpdateAfterSendToSMS, mdlUser.Phone);

                NotifyEvent _event = new NotifyEvent();
                _event.Parameters.Add("Email", mdlUser.Email);
                _event.AddNotifyEvent((long)NotificationEventConstants.Users.RegistrationCompletedEmailSendToAdmin, mdlUser.ID.ToString());

                return ServiceResponse.SuccessReponse(true, MessageEnum.SMSCodeVerify);

            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
        #endregion
        #region|VerifiedSMSCode New Version 5.1.4 changes|
        [HttpPost]
        public async Task<ResponseObject<TokenViewModel>> kkVerifiedSMSCode()
        {

            try
            {
                //  int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                User _mdlUser = new User();

                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    HttpPostedFile file = HttpContext.Current.Request.Files[0];
                    _mdlUser.FileName = await FileOpsHelper.UploadFileNew(file, ContainerName.USER);
                }
                else
                {
                    if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["file"]))
                        _mdlUser.FileName = HttpContext.Current.Request.Form["file"].ToString();
                }
                string BusinessKey = string.Empty;
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["fullName"]))
                    _mdlUser.FullName = HttpContext.Current.Request.Form["fullName"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["email"]))
                    _mdlUser.Email = HttpContext.Current.Request.Form["email"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["address"]))
                    _mdlUser.Address = HttpContext.Current.Request.Form["address"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["phone"]))
                    _mdlUser.Phone = HttpContext.Current.Request.Form["phone"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["password"]))
                    _mdlUser.Password = HttpContext.Current.Request.Form["password"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["longitude"]))
                    _mdlUser.Longitude = Convert.ToDecimal(HttpContext.Current.Request.Form["longitude"]);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["latitude"]))
                    _mdlUser.Latitude = Convert.ToDecimal(HttpContext.Current.Request.Form["latitude"]);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["city"]))
                    _mdlUser.City = HttpContext.Current.Request.Form["city"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["ID"]))
                    _mdlUser.ID = Convert.ToInt32(HttpContext.Current.Request.Form["ID"]);


                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["cityID"]))
                    _mdlUser.CityId = Convert.ToInt32(HttpContext.Current.Request.Form["cityID"]);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["deviceToken"]))
                    _mdlUser.DeviceToken = HttpContext.Current.Request.Form["deviceToken"];

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["smsCodeRequired"]))
                    _mdlUser.SMSCodeRequired = Convert.ToBoolean(HttpContext.Current.Request.Form["smsCodeRequired"]);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["areaID"]))
                    _mdlUser.AreaID = Convert.ToInt32(HttpContext.Current.Request.Form["areaID"]);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["unionCouncil"]))
                    _mdlUser.UnionCouncil = HttpContext.Current.Request.Form["unionCouncil"];

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["businessKey"]))
                    BusinessKey = HttpContext.Current.Request.Form["businessKey"];



                _mdlUser.CompanyID = db.ExtRepositoryFor<CommonRepository>().GetCompanyIdFromBusinessKey(BusinessKey);



                User usrMdod = new User();

                usrMdod = db.Repository<User>().GetAll().Where(x => x.Phone == _mdlUser.Phone).FirstOrDefault();

                if (usrMdod == null)
                {

                    string OldPhone = usrMdod.Phone;
                    if (string.IsNullOrEmpty(_mdlUser.FullName))
                        _mdlUser.FullName = usrMdod.FullName;
                    if (string.IsNullOrEmpty(_mdlUser.Email))
                        _mdlUser.Email = usrMdod.Email;
                    if (string.IsNullOrEmpty(_mdlUser.Password))
                        _mdlUser.Password = usrMdod.Password;
                    if (string.IsNullOrEmpty(_mdlUser.Phone))
                        _mdlUser.Phone = usrMdod.Phone;
                    if (_mdlUser.Latitude == 0)
                        _mdlUser.Latitude = usrMdod.Latitude;
                    if (_mdlUser.Longitude == 0)
                        _mdlUser.Longitude = usrMdod.Longitude;

                    if (string.IsNullOrEmpty(_mdlUser.City))
                        _mdlUser.City = usrMdod.City;

                    if (string.IsNullOrEmpty(_mdlUser.FileName))
                        _mdlUser.FileName = usrMdod.FileName;

                    if (_mdlUser.SMSCodeRequired == null)
                        _mdlUser.SMSCodeRequired = false;

                    if (_mdlUser.AreaID == 0)
                        _mdlUser.AreaID = usrMdod.AreaID;

                    if (string.IsNullOrEmpty(_mdlUser.UnionCouncil))
                        _mdlUser.UnionCouncil = usrMdod.UnionCouncil;

                    User mdlser = new User();
                    mdlser.FullName = _mdlUser.FullName;
                    mdlser.Address = _mdlUser.Address;
                    mdlser.Phone = _mdlUser.Phone;
                    mdlser.Password = _mdlUser.Password;
                    mdlser.Email = _mdlUser.Email;
                    mdlser.Longitude = _mdlUser.Longitude;
                    mdlser.Latitude = _mdlUser.Latitude;
                    mdlser.City = _mdlUser.City;
                    mdlser.FileName = _mdlUser.FileName;
                    mdlser.DeviceToken = _mdlUser.DeviceToken;
                    mdlser.QRCode = "";
                    mdlser.UnionCouncil = _mdlUser.UnionCouncil;
                    mdlser.AreaID = _mdlUser.AreaID;
                    mdlser.CompanyID = _mdlUser.CompanyID;
                    mdlser.CityId = _mdlUser.CityId;
                    db.Repository<User>().Insert(mdlser);
                    db.Save();

                    object TotalGP = db.ExtRepositoryFor<UsersRepository>().GetUserTotalGP(usrMdod.ID);
                    int GP = 0;
                    decimal? GPRedeemed = 0;
                    decimal? GPRedeemable = 0;
                    if (TotalGP != null)
                    {
                        GP = Convert.ToInt32(TotalGP.GetType().GetProperty("TotalGP").GetValue(TotalGP));

                        GPRedeemed = db.Repository<GCRedeem>().GetAll().Where(x => x.UserID == usrMdod.ID).ToList().Sum(y => y.GCRedeemed);
                        GPRedeemable = GP - GPRedeemed;

                    }
                    City city = db.Repository<City>().GetAll().Where(x => x.ID == usrMdod.CityId).FirstOrDefault();

                    String CityName = "";
                    if (city != null)
                    {
                        CityName = city.CityName;
                    }

                    usrMdod.GreenPoints = GP;
                    //int _min = 1000;
                    //int _max = 9999;
                    //Random random = new Random();
                    //Int32 number = random.Next(_min, _max);
                    //if (_mdlUser.SMSCodeRequired == true)
                    //{
                    //    _mdlUser.IsVerified = false;
                    //    SMSNotifyEvent _events = new SMSNotifyEvent();
                    //    _events.Parameters.Add("SMSCode", number);
                    //    _events.AddSMSNotifyEvent((long)NotificationEventConstants.Users.SendVerificationCodeSMS, usrMdod.Phone);
                    //    //Also email the code.
                    //    NotifyEvent _eventEmail = new NotifyEvent();
                    //    _eventEmail.Parameters.Add("SMSCode", number);
                    //    _eventEmail.AddNotifyEvent((long)NotificationEventConstants.Email.SendEmailOfPhoneConfirmation, Convert.ToString(usrMdod.ID));
                    //}

                    object mdlnew = new object();
                    var token = JwtManager.CreateToken(mdlser, out mdlnew);
                    return ServiceResponse.SuccessReponse(new TokenViewModel
                    {
                        Token = token,
                        ID = mdlser.ID,
                        FullName = mdlser.FullName,
                        Address = mdlser.Address,
                        Email = mdlser.Email,
                        Phone = mdlser.Phone,
                        FileName = mdlser.FileName,
                        Longitude = Convert.ToDecimal(mdlser.Longitude),
                        Latitude = Convert.ToDecimal(mdlser.Latitude),
                        GreenPoints = Convert.ToInt32(mdlser.GreenPoints),
                        Password = mdlser.Password,
                        City = CityName,
                        DeviceToken = mdlser.DeviceToken,
                        AreaID = mdlser.AreaID,
                        UnionCouncil = mdlser.UnionCouncil,
                        CompanyID = mdlser.CompanyID,
                        CityId = mdlser.CityId,
                        BusinessKey = db.ExtRepositoryFor<CommonRepository>().GetBusinessKeyCompanyIdFrom(usrMdod.CompanyID),
                        IsConnectedWithGPN = false,     //IsConnectedWithGPN,
                        GPNConnectionMessage = "", // GPNConnectionMessage,
                        RedeemedPoints = GPRedeemed,
                        RedeemablePoints = GPRedeemable,
                        WalletBalance = mdlser.WalletBalance ?? 0,
                        QRCode = usrMdod.QRCode



                    }, MessageEnum.UserProfileUpdated);
                }
                else
                {
                    usrMdod.FullName = _mdlUser.FullName;
                    usrMdod.Address = _mdlUser.Address;
                    usrMdod.Phone = _mdlUser.Phone;
                    usrMdod.Password = _mdlUser.Password;
                    usrMdod.Email = _mdlUser.Email;
                    usrMdod.Longitude = _mdlUser.Longitude;
                    usrMdod.Latitude = _mdlUser.Latitude;
                    usrMdod.City = _mdlUser.City;
                    usrMdod.FileName = _mdlUser.FileName;
                    usrMdod.DeviceToken = _mdlUser.DeviceToken;
                    usrMdod.QRCode = "";
                    usrMdod.UnionCouncil = _mdlUser.UnionCouncil;
                    usrMdod.AreaID = _mdlUser.AreaID;
                    usrMdod.CompanyID = _mdlUser.CompanyID;
                    usrMdod.CityId = _mdlUser.CityId;
                    usrMdod.IsVerified = true;
                    db.Repository<User>().Update(usrMdod);
                    db.Save();

                    object TotalGP = db.ExtRepositoryFor<UsersRepository>().GetUserTotalGP(usrMdod.ID);
                    int GP = 0;
                    decimal? GPRedeemed = 0;
                    decimal? GPRedeemable = 0;
                    if (TotalGP != null)
                    {
                        GP = Convert.ToInt32(TotalGP.GetType().GetProperty("TotalGP").GetValue(TotalGP));

                        GPRedeemed = db.Repository<GCRedeem>().GetAll().Where(x => x.UserID == usrMdod.ID).ToList().Sum(y => y.GCRedeemed);
                        GPRedeemable = GP - GPRedeemed;

                    }

                    usrMdod.GreenPoints = GP;
                    //int _min = 1000;
                    //int _max = 9999;
                    //Random random = new Random();
                    //Int32 number = random.Next(_min, _max);
                    //if (_mdlUser.SMSCodeRequired == true)
                    //{
                    //    _mdlUser.IsVerified = false;
                    //    SMSNotifyEvent _events = new SMSNotifyEvent();
                    //    _events.Parameters.Add("SMSCode", number);
                    //    _events.AddSMSNotifyEvent((long)NotificationEventConstants.Users.SendVerificationCodeSMS, usrMdod.Phone);
                    //    //Also email the code.
                    //    NotifyEvent _eventEmail = new NotifyEvent();
                    //    _eventEmail.Parameters.Add("SMSCode", number);
                    //    _eventEmail.AddNotifyEvent((long)NotificationEventConstants.Email.SendEmailOfPhoneConfirmation, Convert.ToString(usrMdod.ID));
                    //}


                    City city = db.Repository<City>().GetAll().Where(x => x.ID == usrMdod.CityId).FirstOrDefault();

                    String CityName = "";
                    if (city != null)
                    {
                        CityName = city.CityName;
                    }
                    // var token = JwtTokenBuilder(UpdatedUsers);
                    object mdlnew = new object();
                    var token = JwtManager.CreateToken(usrMdod, out mdlnew);
                    return ServiceResponse.SuccessReponse(new TokenViewModel
                    {
                        Token = token,
                        ID = usrMdod.ID,
                        FullName = usrMdod.FullName,
                        Address = usrMdod.Address,
                        Email = usrMdod.Email,
                        Phone = usrMdod.Phone,
                        FileName = usrMdod.FileName,
                        Longitude = Convert.ToDecimal(usrMdod.Longitude),
                        Latitude = Convert.ToDecimal(usrMdod.Latitude),
                        GreenPoints = Convert.ToInt32(usrMdod.GreenPoints),
                        Password = usrMdod.Password,
                        // SMSCode = number,
                        City = CityName,
                        DeviceToken = usrMdod.DeviceToken,
                        AreaID = usrMdod.AreaID,
                        UnionCouncil = usrMdod.UnionCouncil,
                        CompanyID = usrMdod.CompanyID,
                        CityId = usrMdod.CityId,
                        BusinessKey = db.ExtRepositoryFor<CommonRepository>().GetBusinessKeyCompanyIdFrom(usrMdod.CompanyID),
                        IsConnectedWithGPN = false,     //IsConnectedWithGPN,
                        GPNConnectionMessage = "", // GPNConnectionMessage,
                        RedeemedPoints = GPRedeemed,
                        RedeemablePoints = GPRedeemable,
                        WalletBalance = usrMdod.WalletBalance ?? 0,
                        QRCode = usrMdod.QRCode



                    }, MessageEnum.UserProfileUpdated);

                }








                //////#region|Connect User with the User Area GPN if exist|
                //////var CheckGPNInUserArea = db.Repository<Organization>().GetAll().Where(x => x.AreaID == mdlUser.AreaID).FirstOrDefault();
                //////bool IsConnectedWithGPN = false;
                //////string GPNConnectionMessage = "";
                //////if (CheckGPNInUserArea != null)
                //////{
                //////    var CheckUserMembershipInGPN = db.Repository<Member>().GetAll().Where(x => x.OrgId == CheckGPNInUserArea.ID && x.UserID == mdlUser.ID && x.IsActive == true).FirstOrDefault();
                //////    if (CheckUserMembershipInGPN == null)
                //////    {

                //////        Member mdlMember = new Member();
                //////        mdlMember.Name = mdlUser.FullName;
                //////        mdlMember.Designation = "Membership";
                //////        mdlMember.UserID = mdlUser.ID;
                //////        mdlMember.CreatedBy = mdlUser.ID;
                //////        mdlMember.OrgId = CheckGPNInUserArea.ID;
                //////        mdlMember.IsCurrentlyWorking = true;
                //////        mdlMember.FileName = "";
                //////        mdlMember.IsActive = true;
                //////        mdlMember.IsVerified = true;
                //////        db.Repository<Member>().Insert(mdlMember);
                //////        db.Save();
                //////        // set flags
                //////        IsConnectedWithGPN = true;
                //////        //   GPNConnectionMessage = string.Format("Congratulations! You are now connected with {GPN} GPN".Replace("{GPN}", CheckGPNInUserArea.Name);
                //////        GPNConnectionMessage = String.Format("Congratulations! You are now connected with {0} GPN",
                //////              CheckGPNInUserArea.Name);
                //////    }

                //////}
                #endregion

                //var UpdatedUsers = db.Repository<User>().FindById(_mdlUser.ID);

                //object TotalGP = db.ExtRepositoryFor<UsersRepository>().GetUserTotalGP(UpdatedUsers.ID);
                //int GP = 0;
                //decimal? GPRedeemed = 0;
                //decimal? GPRedeemable = 0;
                //if (TotalGP != null)
                //{
                //    GP = Convert.ToInt32(TotalGP.GetType().GetProperty("TotalGP").GetValue(TotalGP));

                //    GPRedeemed = db.Repository<GCRedeem>().GetAll().Where(x => x.UserID == UpdatedUsers.ID).ToList().Sum(y => y.GCRedeemed);
                //    GPRedeemable = GP - GPRedeemed;

                //}

                //UpdatedUsers.GreenPoints = GP;
                //int _min = 1000;
                //int _max = 9999;
                //Random random = new Random();
                //Int32 number = random.Next(_min, _max);
                //if (_mdlUser.SMSCodeRequired == true)
                //{
                //    _mdlUser.IsVerified = false;
                //    SMSNotifyEvent _events = new SMSNotifyEvent();
                //    _events.Parameters.Add("SMSCode", number);
                //    _events.AddSMSNotifyEvent((long)NotificationEventConstants.Users.SendVerificationCodeSMS, UpdatedUsers.Phone);
                //    //Also email the code.
                //    NotifyEvent _eventEmail = new NotifyEvent();
                //    _eventEmail.Parameters.Add("SMSCode", number);
                //    _eventEmail.AddNotifyEvent((long)NotificationEventConstants.Email.SendEmailOfPhoneConfirmation, Convert.ToString(mdlUser.ID));
                //}



            }
            catch (DbEntityValidationException e)
            {
                String errorMessage = string.Empty;
                foreach (var eve in e.EntityValidationErrors)
                {
                    errorMessage = string.Format("Entity of type {0} in state {1} has the following validation errors {2}: ",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State, 0) + Environment.NewLine;
                    foreach (var ve in eve.ValidationErrors)
                    {
                        errorMessage = errorMessage + string.Format("- Property: {0}, Error: {1}",
                            ve.PropertyName, ve.ErrorMessage) + Environment.NewLine;
                    }
                }
                return ServiceResponse.ErrorReponse<TokenViewModel>(errorMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<TokenViewModel>(exp);
            }
        }



        [HttpGet]
        public async Task<ResponseObject<string>> GetLatestAppVersion()
        {
            string Version = System.Configuration.ConfigurationManager.AppSettings[AppSettings.APP_SECTION_VERSION].ToString();
            return ServiceResponse.SuccessReponse(Version, MessageEnum.AppVersion);
        }

        [HttpGet]
        public async Task<ResponseObject<GPNAverageViewModel>> GetGPNAverageByUser(int UserId)
        {
            try
            {

                GPNAverageViewModel gPNAverageViewModel = db.ExtRepositoryFor<UsersRepository>().GetUserTotalGP(UserId);
                int GP = 0;
                if (gPNAverageViewModel != null)
                {
                    // GP = Convert.ToInt32(TotalGP.GetType().GetProperty("TotalGP").GetValue(TotalGP));
                    GP = Convert.ToInt32(gPNAverageViewModel.TotalGP);

                }

                GPNAverageViewModel result = new GPNAverageViewModel();
                result.TotalGW = GP;
                result.RedeemedPoints = db.Repository<GCRedeem>().GetAll().Where(x => x.UserID == UserId).ToList().Sum(y => y.GCRedeemed);
                result.RedeemablePoints = GP - result.RedeemedPoints;

                gPNAverageViewModel.RedeemedPoints = result.RedeemedPoints;
                gPNAverageViewModel.TotalGW = result.TotalGW;
                gPNAverageViewModel.RedeemablePoints = result.RedeemablePoints;

                // var result = db.ExtRepositoryFor<UsersRepository>().GetGPNAverageByUser(UserId);

                return ServiceResponse.SuccessReponse(gPNAverageViewModel, MessageEnum.GPFoundSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<GPNAverageViewModel>(exp);
            }
        }

        [HttpPost]
        public async Task<ResponseObject<IEnumerable<Object>>> GetUserList(UserRequestDto model)
        {
            try
            {
                IEnumerable<Object> users = db.ExtRepositoryFor<UsersRepository>().GetUserList(model);

                if (users.Count() == 0)
                    return ServiceResponse.SuccessReponse(users, MessageEnum.UserListNotFound);
                else
                    return ServiceResponse.SuccessReponse(users, MessageEnum.UserListGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<IEnumerable<Object>>(exp);
            }
        }

        [HttpGet]
        public async Task<ResponseObject<object>> GetUserDetail(int id)
        {
            try
            {
                object user = db.ExtRepositoryFor<UsersRepository>().GetUserDetail(id);

                if (user == null)
                    return ServiceResponse.SuccessReponse(user, MessageEnum.UserDetailNotFoundSuccessfully);
                else
                    return ServiceResponse.SuccessReponse(user, MessageEnum.UserDetailFoundSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }
        [HttpGet]
        public async Task<ResponseObject<object>> GetUserDetailAssoList(int id)
        {
            try
            {
                object user = db.ExtRepositoryFor<UsersRepository>().UserAssocations(id);

                if (user == null)
                    return ServiceResponse.SuccessReponse(user, MessageEnum.UserDetailNotFoundSuccessfully);
                else
                    return ServiceResponse.SuccessReponse(user, MessageEnum.UserDetailFoundSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }
        //[HttpGet]
        //public async Task<ResponseObject<IEnumerable<object>>> GetContactPersonDetail(int id)
        //{
        //    try
        //    {
        //        IEnumerable<object> user = db.ExtRepositoryFor<UsersRepository>().GetContactPersonDetail(id);

        //        if (user.Count() == 0)
        //            return ServiceResponse.SuccessReponse(user, MessageEnum.ContactPersonNotFound);
        //        else
        //            return ServiceResponse.SuccessReponse(user, MessageEnum.ContactPersonFoundSuccessfully);
        //    }
        //    catch (Exception exp)
        //    {
        //        return ServiceResponse.ErrorReponse<IEnumerable<object>>(exp);
        //    }
        //}


        [HttpGet]
        public ResponseObject<object> GetAllRsCount()
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                var userStats = db.ExtRepositoryFor<UsersRepository>().GetUserRsCount(UserID);
                return ServiceResponse.SuccessReponse(userStats, MessageEnum.GotUserRsCountSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<object> GetAllMapsPins()
        {
            try
            {
                var RsPins = db.ExtRepositoryFor<UsersRepository>().GetAllMapPins();
                return ServiceResponse.SuccessReponse<object>(RsPins, "Rs pins found successfully");
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }

        public ResponseObject<object> GetRByID(int rID, string rName)
        {
            try
            {
                object rItem = new object();

                if (rName == "Refuse")
                {
                    rItem = db.Repository<Refuse>().FindById(rID);
                }
                else if (rName == "Recycle")
                {
                    rItem = db.Repository<Recycle>().FindById(rID);
                }
                else if (rName == "Reduce")
                {
                    rItem = db.Repository<Reduce>().FindById(rID);
                }
                else if (rName == "Reuse")
                {
                    rItem = db.Repository<Reuse>().FindById(rID);
                }
                else if (rName == "Regift")
                {
                    rItem = db.Repository<Regift>().FindById(rID);
                }
                else if (rName == "Replant")
                {
                    rItem = db.Repository<Replant>().FindById(rID);
                }
                else if (rName == "Report")
                {
                    rItem = db.Repository<Report>().FindById(rID);
                }

                return ServiceResponse.SuccessReponse(rItem, "Amal R found successfully");
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<int> SentGeneralMessageToAllUsers()
        {
            try
            {
                PushNotificationEvent _event = new PushNotificationEvent();
                _event.AddPushNotifyEvent((long)NotificationEventConstants.PushNotification.General, "1");
                return ServiceResponse.SuccessReponse(1, MessageEnum.ReduceUpdatedSuccessfully);
            }
            catch (Exception exp)
            {

                return ServiceResponse.ErrorReponse<int>(exp);
            }
        }

        [HttpGet]
        public async Task<ResponseObject<string>> MigrateDataMongotoSQL()
        {
            int count = 0;
            List<string> missingType = new List<string>();
            List<string> subMissingType = new List<string>();
            try
            {

                string apiUrl = "https://amalforlife.azurewebsites.net/api/Map/GetUsersGreenPointStatus";
                string insert = "http://10.200.10.33:2018/api/Users/InsertUsers";

                string recycleurl = "http://10.200.10.33:2018/api/users/getallrecycle";

                string reporturl = "http://10.200.10.33:2018/api/users/getallreport";

                string regifteurl = "http://10.200.10.33:2018/api/users/getallregift";

                string emailurl = "http://10.200.10.33:2018/api/users/getallemails";


                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization
                             = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjVjNTgzYThiNzRjNWZjZDY5MDBmNTljNyIsIkVtYWlsIjoiYW1pci5uYXppckBkcnRlY2hway5jb20iLCJSb2xlIjoiTW9iaWxlIFVzZXIiLCJleHAiOjE1ODA4MjIxMDQsImlzcyI6Imh0dHBzOi8vYW1hbGZvcmxpZmUuY29tLyIsImF1ZCI6Imh0dHBzOi8vYW1hbGZvcmxpZmUuY29tLyJ9.Kn7tTjq7xVF-qIjZZRO0mMVEcNEcECBUDE-yRYd-G70");
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    HttpResponseMessage Recycleresponse = await client.GetAsync(recycleurl);

                    HttpResponseMessage Regiftresponse = await client.GetAsync(regifteurl);

                    HttpResponseMessage Reportresponse = await client.GetAsync(reporturl);

                    HttpResponseMessage Emailresponse = await client.GetAsync(emailurl);

                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        var recdata = await Recycleresponse.Content.ReadAsStringAsync();
                        var regiftdata = await Regiftresponse.Content.ReadAsStringAsync();
                        var reportdata = await Reportresponse.Content.ReadAsStringAsync();
                        var Emaildata = await Emailresponse.Content.ReadAsStringAsync();

                        var userlist = JsonConvert.DeserializeObject<Response>(data);

                        List<MUser> UserList = JsonConvert.DeserializeObject<List<MUser>>(userlist.Data.ToString());


                        var userrecycle = JsonConvert.DeserializeObject<Response>(recdata);

                        List<MMrClean> Userrecy = JsonConvert.DeserializeObject<List<MMrClean>>(userrecycle.Data.ToString());


                        var userregift = JsonConvert.DeserializeObject<Response>(regiftdata);

                        List<MRegift> Userreg = JsonConvert.DeserializeObject<List<MRegift>>(userregift.Data.ToString());


                        var userreport = JsonConvert.DeserializeObject<Response>(reportdata);

                        List<MReport> Userrep = JsonConvert.DeserializeObject<List<MReport>>(userreport.Data.ToString());

                        var useremail = JsonConvert.DeserializeObject<Response>(Emaildata);

                        List<MEmail> Usermail = JsonConvert.DeserializeObject<List<MEmail>>(useremail.Data.ToString());



                        List<User> sqlUserList = new List<User>();
                        User sqlUser;
                        Refuse sqlRefuse;
                        Reduce sqlReduce;
                        Reuse sqlReuse;
                        Replant sqlReplant;
                        BuyBin sqlBuyBin;

                        Recycle sqlRecycle;
                        RecycleSubItem sqlrecitems;

                        Regift sqlRegift;
                        RegiftSubItem sqlregitems;
                        OrderTracking sqlOrderTracking;
                        Report sqlReport;
                        EmailNotification sqlEmail;



                        List<Reuse> sqlReuseList;
                        List<Reduce> sqlReduceList;
                        List<Refuse> sqlRefuseList;
                        List<Replant> sqlReplantList;
                        List<BuyBin> sqlBuyBinList;

                        List<Recycle> sqlrecycleList;
                        List<RecycleSubItem> sqlRecSubList;
                        List<Regift> sqlRegiftList;
                        List<RegiftSubItem> sqlRegSubList;
                        List<Report> sqlReportList;

                        List<EmailNotification> sqlEmailList = null;


                        //foreach (var email in Usermail)
                        //{
                        //    sqlEmail = new EmailNotification();
                        //    sqlEmail.EmailTo = email.EmailTo;
                        //    sqlEmail.EmailSubject = email.EmailSubject;
                        //    sqlEmail.EmailBody = email.EmailBody;
                        //    sqlEmail.Status = Convert.ToInt32(email.Status);

                        //    db.Repository<EmailNotification>().Insert(sqlEmail);
                        //    db.Save();

                        //}

                        foreach (var user in UserList)
                        {
                            count++;

                            sqlRefuseList = new List<Refuse>();
                            sqlReduceList = new List<Reduce>();
                            sqlReuseList = new List<Reuse>();
                            sqlReplantList = new List<Replant>();
                            sqlBuyBinList = new List<BuyBin>();

                            sqlrecycleList = new List<Recycle>();
                            sqlRecSubList = new List<RecycleSubItem>();
                            sqlRegiftList = new List<Regift>();
                            sqlRegSubList = new List<RegiftSubItem>();
                            sqlReportList = new List<Report>();
                            sqlEmailList = new List<EmailNotification>();

                            sqlUser = new User();
                            sqlUser.FullName = user.FullName;
                            sqlUser.Address = user.Address;
                            sqlUser.Phone = user.Phone;
                            sqlUser.Email = user.Email;
                            sqlUser.UserTypeID = 1;
                            sqlUser.RoleID = 6;
                            sqlUser.Password = user.Password;
                            sqlUser.Latitude = (decimal?)user.Latitude;
                            sqlUser.Longitude = (decimal?)user.Longitude;
                            sqlUser.GreenPoints = (int?)user.GreenPoints;
                            sqlUser.FileName = user.FileName;
                            sqlUser.QRCode = user.QRCode;
                            sqlUser.DeviceID = user.DeviceId;
                            sqlUser.OSType = user.OSType;
                            sqlUser.SocialMediaKey = user.SocialMediaId;
                            sqlUser.SocialMediaName = user.SocialMedia.Count > 0 ? user.SocialMedia[0].SocialMediaType : "";
                            sqlUser.Status = user.Status;
                            sqlUser.City = user.CityDescription;
                            sqlUser.CityId = user.City;
                            sqlUser.IsVerified = user.IsVerified;
                            sqlUser.IsActive = user.IsActive;
                            sqlUser.SMSCodeRequired = user.SMSCodeRequired;
                            //;sqlUser.Refuses.ToList().AddRange(sqlRefuseList);
                            foreach (var refuse in user.Refuse)
                            {
                                sqlRefuse = new Refuse();
                                sqlRefuse.FileName = refuse.FileName;
                                sqlRefuse.Idea = refuse.Idea;
                                sqlRefuse.Latitude = (decimal)refuse.Latitude;
                                sqlRefuse.Longitude = (decimal)refuse.Longitude;
                                sqlRefuse.StatusID = (int)refuse.Status;
                                sqlRefuse.GreenPoints = refuse.GreenPoints;
                                sqlRefuseList.Add(sqlRefuse);

                            }
                            foreach (var reduse in user.Reduse)
                            {
                                sqlReduce = new Reduce();
                                sqlReduce.FileName = reduse.FileName;
                                sqlReduce.Idea = reduse.Idea;
                                sqlReduce.Latitude = (decimal)reduse.Latitude;
                                sqlReduce.Longitude = (decimal)reduse.Longitude;
                                sqlReduce.StatusID = (int)reduse.Status;
                                sqlReduce.GreenPoints = reduse.GreenPoints;
                                sqlReduceList.Add(sqlReduce);
                            }
                            foreach (var replant in user.Replant)
                            {
                                sqlReplant = new Replant();
                                sqlReplant.FileName = replant.FileName;
                                sqlReplant.PlantID = db.Repository<LookupType>().GetAll().Where(x => x.Name.ToLower() == replant.PlantNameDescription.ToString().ToLower()).Select(x => x.ID).FirstOrDefault();
                                sqlReplant.Description = replant.Description;
                                sqlReplant.TreeCount = replant.TreeCount;
                                //sqlReplant.Height = (Double) replant.Height;
                                //sqlReplant.Reminder = replant.Reminder;
                                sqlReplant.Latitude = (decimal)replant.Latitude;
                                sqlReplant.Longitude = (decimal)replant.Longitude;
                                sqlReplant.StatusID = (int)replant.Status;
                                sqlReplant.GreenPoints = replant.GreenPoints;
                                sqlReplantList.Add(sqlReplant);
                            }
                            foreach (var reuse in user.Reuse)
                            {
                                sqlReuse = new Reuse();
                                sqlReuse.FileName = reuse.FileName;
                                sqlReuse.Idea = reuse.Idea;
                                sqlReuse.Description = reuse.Description;
                                sqlReuse.Latitude = (decimal)reuse.Latitude;
                                sqlReuse.Longitude = (decimal)reuse.Longitude;
                                sqlReuse.StatusID = (int)reuse.Status;
                                sqlReuse.GreenPoints = reuse.GreenPoints;
                                sqlReuseList.Add(sqlReuse);

                            }
                            foreach (var buyBin in user.BuyBinDetails)
                            {
                                sqlBuyBin = new BuyBin();
                                sqlBuyBin.FileName = buyBin.FileName;
                                sqlBuyBin.BinID = 6; // db.Repository<BinDetail>().GetAll().Where(x => x.FileName == buyBin.FileName.ToString()).Select(x => x.ID).FirstOrDefault(); ;                                
                                sqlBuyBin.StatusID = (int)buyBin.Status;
                                sqlBuyBin.Qty = buyBin.Qty;
                                sqlBuyBin.Price = (decimal)buyBin.Price;
                                sqlBuyBin.TrackingNumber = buyBin.TrackingNumber;

                                sqlBuyBinList.Add(sqlBuyBin);
                            }

                            List<MMrClean> recycle = Userrecy.Where(x => x.UserId == user.Id).ToList();
                            foreach (MMrClean item in recycle)
                            {
                                sqlRecycle = new Recycle();
                                sqlRecycle.FileName = item.FileName;
                                //sqlRecycle.CollectorDateTime = Common.Helpers.Utility.GetParsedDate(item.CollectorDateTime);
                                sqlRecycle.StatusID = 1;
                                sqlRecycle.GreenPoints = item.GreenPoints;
                                sqlrecitems = new RecycleSubItem();
                                sqlrecitems.Description = item.Description;
                                sqlrecitems.Weight = (decimal?)item.Weight;
                                sqlrecitems.IsParent = true;
                            
                                sqlrecycleList.Add(sqlRecycle);
                            }

                            List<MRegift> regift = Userreg.Where(x => x.UserId == user.Id).ToList();
                            foreach (MRegift item in regift)
                            {
                                if (count == 294)
                                {

                                }
                                sqlRegift = new Regift();
                                sqlRegift.FileName = item.FileName;
                                sqlRegift.Longitude = (decimal)item.Longitude;
                                sqlRegift.Latitude = (decimal)item.Latitude;
                                //sqlRegift.PickupDate = Common.Helpers.Utility.GetParsedDate(item.DeliveryDate);
                                sqlRegift.DonateToID = 19;
                                sqlRegift.StatusID = 1;
                                sqlRegift.CityID = 3;
                                sqlRegift.GreenPoints = item.GreenPoints;
                                sqlRegift.Description = item.Description;

                                sqlregitems = new RegiftSubItem();

                                if (item.SubTypeDescription.Contains("Age:"))
                                {
                                    item.SubTypeDescription = item.SubTypeDescription.Substring(5, item.SubTypeDescription.Length - 5).Trim();
                                }
                                var tId = db.Repository<LookupType>().GetAll().Where(x => x.Name.ToLower().Trim() == item.TypeDescription.ToLower().Trim()).Select(x => x.ID).FirstOrDefault();
                                var stId = db.Repository<LookupType>().GetAll().Where(x => x.Name.ToLower().Trim() == item.SubTypeDescription.ToLower().Trim()).Select(x => x.ID).FirstOrDefault();
                                if (tId == 0)
                                {
                                    missingType.Add(item.TypeDescription);
                                }
                                if (stId == 0)
                                {
                                    subMissingType.Add(item.SubTypeDescription);
                                }

                                sqlregitems.TypeID = tId;
                                sqlregitems.SubTypeID = stId;
                                sqlregitems.Qty = 0;
                                sqlregitems.IsParent = true;
                                sqlRegift.RegiftSubItems.Add(sqlregitems);
                                sqlRegiftList.Add(sqlRegift);

                            }

                            List<MReport> report = Userrep.Where(x => x.UserId == user.Id).ToList();
                            foreach (MReport item in report)
                            {
                                sqlReport = new Report();
                                sqlReport.FileName = item.FileName;
                                sqlReport.Longitude = (decimal)item.Longitude;
                                sqlReport.Latitude = (decimal)item.Latitude;
                                sqlReport.StatusID = 1;
                                sqlReport.GreenPoints = item.GreenPoints;
                                sqlReport.Description = item.Description;
                                sqlReportList.Add(sqlReport);
                            }

                          
                            sqlUser.Regifts = sqlRegiftList;

                            db.Repository<User>().Insert(sqlUser);
                            db.Save();
                        }
                    }

                }
                var t = missingType;
                var s = subMissingType;
                return ServiceResponse.SuccessReponse("true" + count.ToString(), MessageEnum.ReduceUpdatedSuccessfully);
            }
            catch (DbEntityValidationException e)
            {
                String errorMessage = string.Empty;
                foreach (var eve in e.EntityValidationErrors)
                {
                    errorMessage = string.Format("Entity of type {0} in state {1} has the following validation errors {2}: ",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State, count) + Environment.NewLine;
                    foreach (var ve in eve.ValidationErrors)
                    {
                        errorMessage = errorMessage + string.Format("- Property: {0}, Error: {1}",
                            ve.PropertyName, ve.ErrorMessage) + Environment.NewLine;
                    }
                }
                return ServiceResponse.ErrorReponse<string>(errorMessage);
            }
            catch (System.IndexOutOfRangeException e)  // CS0168
            {
                System.Console.WriteLine(e.Message);
                // Set IndexOutOfRangeException to the new exception's InnerException.

                return ServiceResponse.ErrorReponse<string>(e);
            }
            catch (Exception exp)
            {

                return ServiceResponse.ErrorReponse<string>(exp + count.ToString());
            }

        }

        [HttpGet]
        public async Task<ResponseObject<List<object>>> TopGPUsers()
        {
            int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
            int SchoolID = db.ExtRepositoryFor<UsersRepository>().GetSchoolIDFromUserID(UserID);
            //  int SchoolID1 = 34;
            List<object> list = db.ExtRepositoryFor<UsersRepository>().TopGPUsers(SchoolID);
            return ServiceResponse.SuccessReponse(list, MessageEnum.RecordFoundSuccessfully);
        }


        //[HttpGet]
        //public async Task<List<GetGOIGreenPoints_Result>> GetGreenCreditsForGOI(int GOI1, int GOI2, int GOI3)
        //{
        //    return db.ExtRepositoryFor<UsersRepository>().GetGreenCreditsForGOI(GOI1, GOI2, GOI3);
        //}

        [HttpPost]
        public async Task<ResponseObject<List<GetGOIGreenPoints_Result>>> GetGreenCreditsForGOI(GOIForGrapViewModel model)
        {

            List<GetGOIGreenPoints_Result> lisst = db.ExtRepositoryFor<UsersRepository>().GetGreenCreditsForGOI(model.companyID1, model.companyID2, model.companyID3);

            return ServiceResponse.SuccessReponse(lisst, MessageEnum.RecordFoundSuccessfully);

        }



        [HttpGet]
        public async Task<ResponseObject<object>> RsCountForGPN()
        {
            //  int SchoolID = 34;
            int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
            int SchoolID = db.ExtRepositoryFor<UsersRepository>().GetSchoolIDFromUserID(UserID);
            object o = db.ExtRepositoryFor<UsersRepository>().RsCountForGPN(SchoolID);
            return ServiceResponse.SuccessReponse(o, MessageEnum.RecordFoundSuccessfully);
            //  return o;
        }

        [HttpGet]
        public async Task<ResponseObject<List<RecycleDetailChartVM>>> GetDataForRecycleDetailChartByAdmin()
        {
            int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
            List<RecycleDetailChartVM> lisst = db.ExtRepositoryFor<BusinessRepository>().GetDataForRecycleDetailChartByAdmin(Convert.ToInt32(UserID));
            return ServiceResponse.SuccessReponse(lisst, MessageEnum.RecordFoundSuccessfully);

        }

        [HttpGet]
        public async Task<ResponseObject<string>> GetLoggedInAdminBusinessName()
        {
            int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
            string businessName = db.ExtRepositoryFor<CompanyRepository>().GetLoggedInAdminBusinessName(Convert.ToInt32(UserID));
            return ServiceResponse.SuccessReponse(businessName, MessageEnum.RecordFoundSuccessfully);

        }

        [HttpGet]
        public async Task<ResponseObject<List<object>>> GetCircularGraph()
        {
            int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
            List<object> lisst = db.ExtRepositoryFor<BusinessRepository>().GetCircularChartData(Convert.ToInt32(UserID));
            return ServiceResponse.SuccessReponse(lisst, MessageEnum.RecordFoundSuccessfully);

        }
        [HttpGet]
        public async Task<ResponseObject<List<Area>>> GetAllArea()
        {

            //try
            //{
            List<Area> lstArea = db.Repository<Area>().GetAll().ToList();


            return ServiceResponse.SuccessReponse(lstArea, MessageEnum.DefaultSuccessMessage);
            //}
            //catch (Exception exp)
            //{
            //    return ServiceResponse.ErrorReponse<List<Area>>(exp);
            //}
        }



        [HttpGet]
        public async Task<ResponseObject<List<GetGOIChart_Result>>> GetGOIGraph()
        {
            int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
            List<GetGOIChart_Result> lisst = db.ExtRepositoryFor<UsersRepository>().GetGOIGraph(Convert.ToInt32(UserID));
            return ServiceResponse.SuccessReponse(lisst, MessageEnum.RecordFoundSuccessfully);
        }

        [HttpGet]
        public async Task<ResponseObject<List<spGetDailyGreenPoints_Result>>> GetDailyGreenPointsGraph()
        {
            int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
            List<spGetDailyGreenPoints_Result> lisst = db.ExtRepositoryFor<UsersRepository>().GetDailyGreenPointsGraph(Convert.ToInt32(UserID));
            return ServiceResponse.SuccessReponse(lisst, MessageEnum.RecordFoundSuccessfully);
        }
        [HttpGet]
        public async Task<ResponseObject<List<spGetWasteWeightDaily_Result>>> GetDailyWasteWeightGraph()
        {
            int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
            List<spGetWasteWeightDaily_Result> lisst = db.ExtRepositoryFor<UsersRepository>().GetDailyWasteWeightGraph(Convert.ToInt32(UserID));
            return ServiceResponse.SuccessReponse(lisst, MessageEnum.RecordFoundSuccessfully);
        }


        [HttpGet]
        public async Task<ResponseObject<List<GetGreenPointsYearWise_Result>>> GetGOIGraphYearWise()
        {
            int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
            List<GetGreenPointsYearWise_Result> lisst = db.ExtRepositoryFor<UsersRepository>().GetGOIGraphYearWise(Convert.ToInt32(UserID));
            return ServiceResponse.SuccessReponse(lisst, MessageEnum.RecordFoundSuccessfully);
        }

        [HttpGet]
        public async Task<ResponseObject<List<GetGreenPointsMonthWise_Result>>> GetGOIGraphMonthWise()
        {
            int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
            List<GetGreenPointsMonthWise_Result> lisst = db.ExtRepositoryFor<UsersRepository>().GetGOIGraphMonthWise(Convert.ToInt32(UserID));
            return ServiceResponse.SuccessReponse(lisst, MessageEnum.RecordFoundSuccessfully);
        }

        public class Response
        {
            public int StatusCode { get; set; }
            public string StatusMessage { get; set; }
            //public bool IsSuccess { get; set; }
            public object Data { get; set; }
        }

        [HttpPost]
        public async Task<ResponseObject<bool>> AddContactUsData(ContactU _mdlContactUs)
        {
            try
            {
                if (_mdlContactUs != null)
                {
                    _mdlContactUs.IsActive = true;
                    db.Repository<ContactU>().Insert(_mdlContactUs);
                    db.Save();
                    return ServiceResponse.SuccessReponse(true, MessageEnum.DefaultSuccessMessage);
                }
                else
                    return ServiceResponse.SuccessReponse(false, MessageEnum.DefaultParametersCanNotBeNull);


            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

    }
}

