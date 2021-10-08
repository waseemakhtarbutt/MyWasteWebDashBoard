using DrTech.Amal.Common.Enums;
using DrTech.Amal.Common.Extentions;
using DrTech.Amal.Common.Helpers;
using DrTech.Amal.Common.ServerResponse;
using DrTech.Amal.Notifications;
using DrTech.Amal.SQLModels;
using DrTech.Amal.SQLServices.Auth;
using DrTech.Amal.SQLDataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using static DrTech.Amal.Common.Extentions.Constants;
using System.Text;
using DrTech.Amal.SQLDataAccess.CustomModels;

namespace DrTech.Amal.SQLServices.Controllers
{
    [Authorize]
    public class BusinessController : BaseController
    {
        [HttpPost]
        public async Task<ResponseObject<bool>> AddBusinessInformation()
        {
            try
            {
                STG_Business mdlBusiness = new STG_Business();
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                //string FileName = string.Empty;
                //HttpPostedFile file = HttpContext.Current.Request.Files[0];
                //FileName = await FileOpsHelper.UploadFileNew(file, ContainerName.Business);

                mdlBusiness.FileName = "";

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["name"]))
                    mdlBusiness.Name = HttpContext.Current.Request.Form["name"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["address"]))
                    mdlBusiness.Address = HttpContext.Current.Request.Form["address"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["phone"]))
                    mdlBusiness.Phone = HttpContext.Current.Request.Form["phone"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["email"]))
                    mdlBusiness.Email = HttpContext.Current.Request.Form["email"].ToString();


                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["cityid"]))
                    mdlBusiness.CityID = Convert.ToInt32(HttpContext.Current.Request.Form["cityid"]);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["officeName"]))
                    mdlBusiness.OfficeName = HttpContext.Current.Request.Form["officeName"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["contactPerson"]))
                    mdlBusiness.ContactPerson = HttpContext.Current.Request.Form["contactPerson"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["busTypeID"]))
                    mdlBusiness.BusTypeID = Convert.ToInt32(HttpContext.Current.Request.Form["busTypeID"]);

                mdlBusiness.IsVerified = false;
                mdlBusiness.IsActive = false;
                mdlBusiness.CreatedBy = (int)UserID;
                mdlBusiness.CreatedDate = DateTime.Now;
                mdlBusiness.UserID = UserID;
                db.Repository<STG_Business>().Insert(mdlBusiness);
                db.Save();

                return ServiceResponse.SuccessReponse(true, MessageEnum.NGOAdded);
            }
            catch (DbEntityValidationException e)
            {
                String errorMessage = string.Empty;
                foreach (var eve in e.EntityValidationErrors)
                {
                    errorMessage = string.Format("Entity of type {0} in state {1} has the following validation errors: ",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State) + Environment.NewLine;
                    foreach (var ve in eve.ValidationErrors)
                    {
                        errorMessage = errorMessage + string.Format("- Property: {0}, Error: {1}",
                            ve.PropertyName, ve.ErrorMessage) + Environment.NewLine;
                    }
                }
                return ServiceResponse.ErrorReponse<bool>(errorMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<List<Business>> GetBusinessDropdowns()
        {
            try
            {
                var dorpdowns = db.Repository<Business>().GetAll().ToList();
                return ServiceResponse.SuccessReponse(dorpdowns, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Business>>(exp);
            }
        }

      

        [HttpGet]
        public ResponseObject<List<object>> GetBusinessesWithRegistrationStatus()
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                List<object> list = db.ExtRepositoryFor<BusinessRepository>().GetAllBusinessWithRegistrationStatus(UserID);
                return ServiceResponse.SuccessReponse(list, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<List<object>> GetSubBusinessesWithRegistrationStatus(int ID)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                List<object> list = db.ExtRepositoryFor<BusinessRepository>().GetAllSubBusinessWithRegistrationStatus(UserID, ID);
                return ServiceResponse.SuccessReponse(list, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<List<object>> GetBusinessSubOfficesDropdown(int ID)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                var dorpdowns = db.ExtRepositoryFor<BusinessRepository>().GetBusinessBranchesByID(UserID, ID);
                return ServiceResponse.SuccessReponse(dorpdowns, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }  

        [HttpGet]
        public ResponseObject<List<STG_Business>> GetStgBusinessList()
        {
            try
            {
                List<STG_Business> regBusiness = db.Repository<STG_Business>().GetAll().Where(x => x.IsVerified == false && x.IsActive == false).ToList();
                return ServiceResponse.SuccessReponse(regBusiness, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<STG_Business>>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<object> GetStgBusinessByID(string Param)
        {
            try
            {
                string[] requestParam = Param.Split('-');
                int id = Convert.ToInt32(requestParam[0]);
                if (requestParam[1] != "Edit")
                {
                    object stgSchool = db.Repository<STG_Business>().GetAll().Where(x => x.ID == id).FirstOrDefault();
                    return ServiceResponse.SuccessReponse(stgSchool, MessageEnum.DefaultSuccessMessage);
                }
                else
                {
                    object school = db.Repository<Business>().GetAll().Where(x => x.ID == id).FirstOrDefault();
                    return ServiceResponse.SuccessReponse(school, MessageEnum.DefaultSuccessMessage);
                }
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<List<RegBusiness>> GetRegBusinessDropdown()
        {
            try
            {
                List<RegBusiness> regBusiness = db.Repository<RegBusiness>().GetAll().ToList();
                regBusiness.Add(new RegBusiness { ID = 0, Name = "Not Registered" });
                return ServiceResponse.SuccessReponse(regBusiness, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<RegBusiness>>(exp);
            }
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ResponseObject<bool>> AddBusiness()
        {
            try
            {
                Business mdlBusiness = new Business();
                RegBusiness regBusiness = new RegBusiness();
                HttpPostedFile requiredDocuments;
                string requestType = string.Empty;
                string logoFileName = string.Empty;
                string documentFileName = string.Empty;
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["requestType"]))
                    requestType = HttpContext.Current.Request.Form["requestType"].ToString();

                //school edit request 
                if (requestType == "Edit")
                {
                    if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["id"]))
                        mdlBusiness = db.Repository<Business>().FindById(Convert.ToInt32(HttpContext.Current.Request.Form["id"].ToString()));
                }

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["imageToUpload"]))
                {
                    HttpPostedFile file = HttpContext.Current.Request.Files[0];
                    logoFileName = await FileOpsHelper.UploadFileNew(file, ContainerName.Business);
                    mdlBusiness.FileName = logoFileName;
                }
                else
                {
                    if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["fileName"]))
                        mdlBusiness.FileName = HttpContext.Current.Request.Form["fileName"].ToString();
                }

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["fileToUpload"]))
                {
                    if (HttpContext.Current.Request.Files.Count == 1)
                    {
                        requiredDocuments = HttpContext.Current.Request.Files[0];
                        documentFileName = await FileOpsHelper.UploadFileNew(requiredDocuments, ContainerName.Business);
                    }
                    else
                    {
                        requiredDocuments = HttpContext.Current.Request.Files[1];
                        documentFileName = await FileOpsHelper.UploadFileNew(requiredDocuments, ContainerName.Business);
                    }
                    mdlBusiness.DocumentFileName = documentFileName;
                }
                else if (requestType == "Edit")
                {
                    if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["DocumentFileName"]))
                        mdlBusiness.DocumentFileName = HttpContext.Current.Request.Form["DocumentFileName"].ToString();
                }

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["name"]))
                    mdlBusiness.Name = HttpContext.Current.Request.Form["name"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["address"]))
                    mdlBusiness.Address = HttpContext.Current.Request.Form["address"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["phone"]))
                    mdlBusiness.Phone = HttpContext.Current.Request.Form["phone"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["parentId"]))
                    mdlBusiness.ParentId = Convert.ToInt32(HttpContext.Current.Request.Form["parentId"]);
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["level"]))
                    mdlBusiness.Level = HttpContext.Current.Request.Form["level"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["officeName"]))
                    mdlBusiness.OfficeName = HttpContext.Current.Request.Form["officeName"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["contactPerson"]))
                    mdlBusiness.ContactPerson = HttpContext.Current.Request.Form["contactPerson"].ToString();                
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["email"]))
                    mdlBusiness.Email = HttpContext.Current.Request.Form["email"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["busTypeID"]))
                    mdlBusiness.BusTypeID = Convert.ToInt32(HttpContext.Current.Request.Form["busTypeID"].ToString());


                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["cityid"]))
                    mdlBusiness.CityID = Convert.ToInt32(HttpContext.Current.Request.Form["cityid"]);

                mdlBusiness.IsVerified = true;
                mdlBusiness.IsActive = true;
                // mdlSchool.UserID = UserID;

                if (requestType != "Edit")
                {
                    if (mdlBusiness.ParentId <= 0)
                    {

                        if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["name"]))
                            regBusiness.Name = HttpContext.Current.Request.Form["name"].ToString();
                        regBusiness.IsActive = true;
                        regBusiness.CreatedBy = UserID;
                        regBusiness.CreatedDate = DateTime.Now;
                        User user = new User
                        {
                            FullName = mdlBusiness.ContactPerson,
                            Phone = mdlBusiness.Phone,
                            Email = "admin" + mdlBusiness.Email,
                            Password = "admin@1234",
                            UserTypeID = (int)UserTypeEnum.Web,
                            RoleID = (int)UserRoleTypeEnum.BusinessAdmin,
                            CreatedBy = db.Repository<User>().GetAll().Where(x => x.RoleID == 1).Select(x => x.ID).FirstOrDefault(),
                        };
                        regBusiness.User = user;
                        mdlBusiness.RegBusiness = regBusiness;
                        //db.Repository<RegBusiness>().Insert(regBusiness);
                        //db.Save();
                        //mdlBusiness.ParentId = regBusiness.ID;
                    }

                    User mdlUsers = new User
                    {
                        FullName = mdlBusiness.ContactPerson,
                        Phone = mdlBusiness.Phone,
                        Email = mdlBusiness.Email,
                        Password = "abcd@1234",
                        UserTypeID = (int)UserTypeEnum.Web,
                        RoleID = (int)UserRoleTypeEnum.SubBusinessAdmin,
                        CreatedBy = db.Repository<User>().GetAll().Where(x => x.RoleID == 1).Select(x => x.ID).FirstOrDefault(),
                    };
                    //db.Repository<User>().Insert(mdlUsers);
                    //db.Save();
                    //mdlSchool.UserID = mdlUsers.ID;
                    mdlBusiness.User = mdlUsers;
                }


                if (requestType != "Edit")
                {
                    mdlBusiness.EmployeeGreenPoints = 0;
                    mdlBusiness.GreenPoints = 0;
                    mdlBusiness.CreatedBy = (int)UserID;
                    mdlBusiness.CreatedDate = DateTime.Now;
                    db.Repository<Business>().Insert(mdlBusiness);
                    db.Save();
                }

                else
                {
                    mdlBusiness.EmployeeGreenPoints = 0;
                    mdlBusiness.GreenPoints = 0;
                    mdlBusiness.UpdatedBy = (int)UserID;
                    mdlBusiness.UpdatedDate = DateTime.Now;
                    db.Repository<Business>().Update(mdlBusiness);
                    db.Save();
                }


                int stgSchoolID = 0;
                if (requestType == "S")
                {
                    if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["stgId"]))
                    {
                        stgSchoolID = Convert.ToInt32(HttpContext.Current.Request.Form["stgId"]);
                        STG_Business iSVerifiedBusiness = db.Repository<STG_Business>().GetAll().Where(x => x.ID == stgSchoolID).FirstOrDefault();
                        iSVerifiedBusiness.IsVerified = true;
                        db.Repository<STG_Business>().Update(iSVerifiedBusiness);
                        db.Save();
                    }
                }


                NotifyEvent _event = new NotifyEvent();
                _event.Parameters.Add("ContactPerson", mdlBusiness.ContactPerson);
                _event.Parameters.Add("Name", mdlBusiness.Name);
                _event.Parameters.Add("Branch", mdlBusiness.OfficeName);
                _event.Parameters.Add("Email", mdlBusiness.Email);
                _event.Parameters.Add("Phone", mdlBusiness.Phone);
                _event.AddNotifyEvent((long)NotificationEventConstants.School.SendEmailToAdmin, Convert.ToString(UserID));


                SMSNotifyEvent _events = new SMSNotifyEvent();
                _events.Parameters.Add("ContactPerson", mdlBusiness.ContactPerson);
                _events.Parameters.Add("Phone", mdlBusiness.Phone);
                _events.Parameters.Add("Name", mdlBusiness.Name);
                _events.AddSMSNotifyEvent((long)NotificationEventConstants.School.SendSMSToUser, mdlBusiness.Phone);

                StringBuilder CodeModel = new StringBuilder();
                CodeModel.Append(mdlBusiness.ID + ";");
                CodeModel.Append(mdlBusiness.Email + ";");
                CodeModel.Append(mdlBusiness.Phone + ";");
                CodeModel.Append(mdlBusiness.Name + ";");
                string QRPath = string.Empty;
                QRPath = QRCodeTagHelper.QRCodeGeneratorImage(CodeModel);
                EmailHelper.SendEmailByTemplate(mdlBusiness.Name, mdlBusiness.Email, QRPath);
                if (requestType != "Edit")
                {
                    return ServiceResponse.SuccessReponse(true, MessageEnum.SchoolAddedSuccessfully);
                }
                {
                    return ServiceResponse.SuccessReponse(true, MessageEnum.SchoolInfoUpdated);
                }


            }
            catch (DbEntityValidationException e)
            {
                String errorMessage = string.Empty;
                foreach (var eve in e.EntityValidationErrors)
                {
                    errorMessage = string.Format("Entity of type {0} in state {1} has the following validation errors: ",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State) + Environment.NewLine;
                    foreach (var ve in eve.ValidationErrors)
                    {
                        errorMessage = errorMessage + string.Format("- Property: {0}, Error: {1}",
                            ve.PropertyName, ve.ErrorMessage) + Environment.NewLine;
                    }
                }
                return ServiceResponse.ErrorReponse<bool>(errorMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
        [HttpPost]
        public ResponseObject<List<Business>> GetBusinessList(BusinesssRequestDto model)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                int? RoleID = JwtDecoder.GetUserRoleFromToken(Request.Headers.Authorization.Parameter);
                int roleID = Convert.ToInt32(UserRoleTypeEnum.Admin);

                List<Business> businessList = db.Repository<Business>().GetAll().Where(x => x.UserID == UserID || RoleID == roleID && x.IsVerified==true && x.IsActive == true).ToList();
                if (model?.StartDate != null && model?.EndDate != null)
                {
                    businessList = businessList.Where(x => x.CreatedDate >= Utility.GetDateFromString(model.StartDate) && x.CreatedDate <= Utility.GetDateFromString(model.EndDate)).OrderByDescending(x => x.CreatedDate).ToList();
                }
                return ServiceResponse.SuccessReponse(businessList, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Business>>(exp);
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public ResponseObject<List<object>> GetHeadOfficesOFBusinessForGOI()
        {
            try
            {
                List<object> businessList = new List<object>();
                // businessList = db.Repository<RegBusiness>().GetAll().Where(x => x.IsActive == true).ToList<object>(); 
                businessList = db.ExtRepositoryFor<BusinessRepository>().GetHeadOfficesOFBusinessForGOI();
                    return ServiceResponse.SuccessReponse(businessList, MessageEnum.DefaultSuccessMessage);               
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public ResponseObject<List<object>> GetBusinessBranchesByIdForGOI(int ID)
        {
            try
            {
                List<object> businessList = new List<object>();
                businessList = db.ExtRepositoryFor<BusinessRepository>().GetBusinessListForGOI(ID);
                return ServiceResponse.SuccessReponse(businessList, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }
        public ResponseObject<List<object>> GetBusinessListForGOI()
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                 int? RoleID = JwtDecoder.GetUserRoleFromToken(Request.Headers.Authorization.Parameter);
                List<object> businessList = new List<object>();
                if(RoleID == (int)UserRoleTypeEnum.BusinessAdmin)
                {
                    RegBusiness mdlbusiness = db.Repository<RegBusiness>().GetAll().Where(x => x.IsActive == true && x.UserID == UserID).FirstOrDefault();
                    businessList = db.ExtRepositoryFor<BusinessRepository>().GetBusinessListForGOI(mdlbusiness.ID);
                    return ServiceResponse.SuccessReponse(businessList, MessageEnum.DefaultSuccessMessage);
                }
                else
                {
                    Business mdlbusiness = db.Repository<Business>().GetAll().Where(x => x.IsVerified == true && x.IsActive == true && x.UserID == UserID).FirstOrDefault();
                     businessList = db.ExtRepositoryFor<BusinessRepository>().GetBusinessListForGOI(mdlbusiness.ParentId);
                    return ServiceResponse.SuccessReponse(businessList, MessageEnum.DefaultSuccessMessage);
                }


            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }

        public ResponseObject<List<Business>> GetSuspendedBusinessList()
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                int? RoleID = JwtDecoder.GetUserRoleFromToken(Request.Headers.Authorization.Parameter);
                int roleID = Convert.ToInt32(UserRoleTypeEnum.Admin);

                List<Business> businessList = db.Repository<Business>().GetAll().Where(x => x.UserID == UserID || RoleID == roleID && x.IsActive == false).ToList();
                return ServiceResponse.SuccessReponse(businessList, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Business>>(exp);
            }
        }



        [HttpGet]
        public ResponseObject<List<Employment>> GetEmployeeList()
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                int? RoleID = JwtDecoder.GetUserRoleFromToken(Request.Headers.Authorization.Parameter);
                int roleID = Convert.ToInt32(UserRoleTypeEnum.Admin);

                List<Employment> employmentList = db.Repository<Employment>().GetAll().Where(x => x.Business.UserID == UserID || roleID == RoleID).ToList();
                return ServiceResponse.SuccessReponse(employmentList, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Employment>>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<Employment> GetEmployeeDetail(int ID)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                int roleID = Convert.ToInt32(UserRoleTypeEnum.Admin);

                Employment employeeDetail = db.Repository<Employment>().GetAll().Where(x => x.ID == ID).FirstOrDefault();
                return ServiceResponse.SuccessReponse(employeeDetail, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<Employment>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<Employment> GetBusinessStatus(Employment employee)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                int roleID = Convert.ToInt32(UserRoleTypeEnum.Admin);

                Employment employmentDetail = db.Repository<Employment>().GetAll().Where(x => x.ID == employee.ID).FirstOrDefault();
                employmentDetail.IsActive = employee.IsActive;
                return ServiceResponse.SuccessReponse(employmentDetail, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<Employment>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<bool> InactiveStgBusiness(int ID)
        {
            try
            {
                STG_Business business = db.Repository<STG_Business>().GetAll().Where(x => x.ID == ID).FirstOrDefault();
                business.IsActive = false;
                db.Repository<STG_Business>().Update(business);
                db.Save();
                return ServiceResponse.SuccessReponse(true, MessageEnum.BusinessSuspended);
            }
            catch (Exception exp)
            {

                return ServiceResponse.ErrorReponse<bool>(exp);
            }

        }

        [HttpGet]
        public ResponseObject<bool> SuspendBusiness(int ID)
        {
            try
            {
                Business business = db.Repository<Business>().GetAll().Where(x => x.ID == ID).FirstOrDefault();
                business.IsActive = false;
                db.Repository<Business>().Update(business);
                db.Save();
                var employeesList = db.Repository<Employment>().GetAll().Where(x => x.BusId == business.ID).ToList();
                foreach (var employee in employeesList)
                {
                    employee.IsActive = false;
                }
                return ServiceResponse.SuccessReponse(true, MessageEnum.BusinessSuspended);
            }
            catch (Exception exp)
            {

                return ServiceResponse.ErrorReponse<bool>(exp);
            }

        }

        [HttpGet]
        public ResponseObject<bool> ActivateInstance(int ID)
        {
            try
            {
                Business business = db.Repository<Business>().GetAll().Where(x => x.ID == ID).FirstOrDefault();
                business.IsActive = true;
                db.Repository<Business>().Update(business);
                db.Save();
                var employeesList = db.Repository<Employment>().GetAll().Where(x => x.BusId == business.ID).ToList();
                foreach (var employee in employeesList)
                {
                    employee.IsActive = true;
                }
                return ServiceResponse.SuccessReponse(true, MessageEnum.BusinessRestored);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }

        }
        [HttpGet]
        public ResponseObject<List<object>> GetEmployListByRole(bool IsSuspended)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                int? RoleID = JwtDecoder.GetUserRoleFromToken(Request.Headers.Authorization.Parameter);

                List<object> employList = db.ExtRepositoryFor<BusinessRepository>().GetEmployListByRole(UserID, IsSuspended, RoleID);
                return ServiceResponse.SuccessReponse(employList, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<List<object>> GetEmployListByRoleWithEmployeeProgress(bool IsSuspended)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                int? RoleID = JwtDecoder.GetUserRoleFromToken(Request.Headers.Authorization.Parameter);

                List<object> employList = db.ExtRepositoryFor<BusinessRepository>().GetEmployListByRoleWithEmployeeProgress(UserID, IsSuspended, RoleID);
                return ServiceResponse.SuccessReponse(employList, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }
        [HttpPost]
        public ResponseObject<bool> SuspendEmploy(int id)
        {
            try
            {
                Employment employee = db.Repository<Employment>().GetAll().Where(x => x.ID == id).FirstOrDefault();

                employee.IsActive = false;
                db.Repository<Employment>().Update(employee);
                db.Save();
                PushNotificationEvent _event = new PushNotificationEvent();
                _event.Parameters.Add("GPN", employee.Business.Name);
                _event.AddPushNotifyEvent((long)NotificationEventConstants.PushNotification.NotifyEmployeeOnSuspention, employee.UserID.ToString());
                //send sms to user for to notification.
                //SMSNotifyEvent _events = new SMSNotifyEvent();
                //_events.Parameters.Add("SMSCode", "");

                //_events.AddSMSNotifyEvent((long)NotificationEventConstants.SuspendUsers.SMSSendtoEmployee, employ.User.Phone);



                return ServiceResponse.SuccessReponse(true, MessageEnum.SuspendSuccessfully);
            }
            catch (Exception exp)
            {

                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
        [HttpPost]
        public ResponseObject<bool> ActivateEmployee(int Id)
        {
            try
            {
                var employee = db.Repository<Employment>().FindById(Id);
                employee.IsActive = true;
                db.Repository<Employment>().Update(employee);
                db.Save();

                return ServiceResponse.SuccessReponse(true, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<List<Object>> GetDepartmentsByRole()
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                int? RoleID = JwtDecoder.GetUserRoleFromToken(Request.Headers.Authorization.Parameter);

                List<object> sectionsList = db.ExtRepositoryFor<BusinessRepository>().GetDepartmentsByRole(UserID, RoleID);
                return ServiceResponse.SuccessReponse(sectionsList, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Object>>(exp);
            }
        }
        [HttpGet]
        public ResponseObject<List<Object>> GetBusinessComparison(string dpt, string bus, string fd, string td)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                int? RoleID = JwtDecoder.GetUserRoleFromToken(Request.Headers.Authorization.Parameter);

                int businessId = 0;

                if (RoleID == (int)UserRoleTypeEnum.SubBusinessAdmin)
                    businessId = db.Repository<Business>().GetAll().Where(x => x.UserID == UserID).FirstOrDefault().ID;

                List<object> resultList = new List<object>();

                if ((businessId != 0 && RoleID == (int)UserRoleTypeEnum.SubBusinessAdmin) || RoleID == (int)UserRoleTypeEnum.BusinessAdmin)
                {
                    DateTime fromDate = Utility.GetParsedDate(fd).Date;
                    DateTime toDate = Utility.GetParsedDate(td).Date.AddDays(1).AddSeconds(-1);

                    resultList = db.ExtRepositoryFor<BusinessRepository>().GetBusinessComparison(businessId, dpt, bus, fromDate, toDate, RoleID);
                    return ServiceResponse.SuccessReponse(resultList, MessageEnum.DefaultSuccessMessage);
                }
                else
                    return ServiceResponse.SuccessReponse(resultList, MessageEnum.DefaultErrorMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Object>>(exp);
            }
        }
        [HttpGet]
        public ResponseObject<List<Object>> GetBranchesByBusinessAdmin()
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                List<object> branchesList = db.ExtRepositoryFor<BusinessRepository>().GetBranchesByBusinessAdmin(UserID);
                return ServiceResponse.SuccessReponse(branchesList, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Object>>(exp);
            }
        }
    }
}
