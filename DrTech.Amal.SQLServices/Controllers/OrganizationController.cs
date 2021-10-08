using DrTech.Amal.Common.Enums;
using DrTech.Amal.Common.Helpers;
using DrTech.Amal.Common.ServerResponse;
using DrTech.Amal.Notifications;
using DrTech.Amal.SQLDataAccess.Repository;
using DrTech.Amal.SQLModels;
using DrTech.Amal.SQLServices.Auth;
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
using DrTech.Amal.SQLDataAccess;
using System.Text;
using DrTech.Amal.SQLDataAccess.CustomModels;

namespace DrTech.Amal.SQLServices.Controllers
{
    [Authorize]
    public class OrganizationController : BaseController
    {

        [HttpPost]
        public async Task<ResponseObject<bool>> AddOrganizationInformation()
        {
            try
            {
               STG_Organization mdlOrganization = new STG_Organization();
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                //string FileName = string.Empty;
                //HttpPostedFile file = HttpContext.Current.Request.Files[0];
                //FileName = await FileOpsHelper.UploadFileNew(file, ContainerName.RECYCLE);

                mdlOrganization.FileName = "";

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["name"]))
                    mdlOrganization.Name = HttpContext.Current.Request.Form["name"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["address"]))
                    mdlOrganization.Address = HttpContext.Current.Request.Form["address"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["phone"]))
                    mdlOrganization.Phone = HttpContext.Current.Request.Form["phone"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["email"]))
                    mdlOrganization.Email = HttpContext.Current.Request.Form["email"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["cityID"]))
                    mdlOrganization.CityID = Convert.ToInt32(HttpContext.Current.Request.Form["cityID"]);
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["areaID"]))
                    mdlOrganization.AreaID = Convert.ToInt32(HttpContext.Current.Request.Form["areaID"]);
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["officeName"]))
                    mdlOrganization.SiteOffice = HttpContext.Current.Request.Form["officeName"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["contactPerson"]))
                    mdlOrganization.ContactPerson = HttpContext.Current.Request.Form["contactPerson"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["orgTypeID"]))
                    mdlOrganization.OrgTypeID = Convert.ToInt32(HttpContext.Current.Request.Form["orgTypeID"]);
                

                mdlOrganization.IsVerified = false;
                mdlOrganization.IsActive = true;
                mdlOrganization.UserID = UserID;
                mdlOrganization.CreatedBy = (int)UserID;
                mdlOrganization.CreatedDate = DateTime.Now;              
                mdlOrganization.UserID = UserID;
                

                db.Repository<STG_Organization>().Insert(mdlOrganization);
                db.Save();

                return ServiceResponse.SuccessReponse(true, MessageEnum.OrgAddedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<List<Organization>> GetOrganizationDropdowns()
        {
            try
            {
                var dorpdowns = db.Repository<Organization>().GetAll().ToList();
                return ServiceResponse.SuccessReponse(dorpdowns, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Organization>>(exp);
            }

        }


        [HttpGet]
        public ResponseObject<List<object>> GetOrganizationsWithRegistrationStatus()
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                List<object> list = db.ExtRepositoryFor<OrganizationRepository>().GetAllOrganizationWithRegistrationStatus(UserID);
                return ServiceResponse.SuccessReponse(list, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<List<object>> GetSubOfficessWithRegistrationStatus(int ID)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                List<object> list = db.ExtRepositoryFor<OrganizationRepository>().GetAllSubOfficesWithRegistrationStatus(UserID, ID);
                return ServiceResponse.SuccessReponse(list, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<List<object>> GetSubOfficesDropdown(int ID)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                var dorpdowns = db.ExtRepositoryFor<OrganizationRepository>().GetOrgBranchesByID(UserID, ID);
                return ServiceResponse.SuccessReponse(dorpdowns, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<List<STG_Organization>> GetStgOrganizationList()
        {
            try
            {
                List<STG_Organization> regOrganization = db.Repository<STG_Organization>().GetAll().Where(x => x.IsVerified == false && x.IsActive != false).ToList();
                return ServiceResponse.SuccessReponse(regOrganization, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<STG_Organization>>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<object> GetStgOrganizationByID(string Param)
        {
            try
            {
                string[] requestParam = Param.Split('-');
                int id = Convert.ToInt32(requestParam[0]);
                if (requestParam[1] != "Edit")
                {
                    object stgSchool = db.Repository<STG_Organization>().GetAll().Where(x => x.ID == id).FirstOrDefault();
                    return ServiceResponse.SuccessReponse(stgSchool, MessageEnum.DefaultSuccessMessage);
                }
                else
                {
                    object school = db.Repository<Organization>().GetAll().Where(x => x.ID == id).FirstOrDefault();
                    return ServiceResponse.SuccessReponse(school, MessageEnum.DefaultSuccessMessage);
                }
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<List<RegOrganization>> GetRegOrganizationDropdown()
        {
            try
            {
                List<RegOrganization> regOrganization = db.Repository<RegOrganization>().GetAll().ToList();
                regOrganization.Add(new RegOrganization { ID = 0, Name = "Not Registered" });
                return ServiceResponse.SuccessReponse(regOrganization, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<RegOrganization>>(exp);
            }
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ResponseObject<bool>> AddOrganization()
        {
            try
            {
                Organization mdlOrganization = new Organization();
                RegOrganization regOrganization = new RegOrganization();
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
                        mdlOrganization = db.Repository<Organization>().FindById(Convert.ToInt32(HttpContext.Current.Request.Form["id"].ToString()));
                }

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["imageToUpload"]))
                {
                    HttpPostedFile file = HttpContext.Current.Request.Files[0];
                    logoFileName = await FileOpsHelper.UploadFileNew(file, ContainerName.ORGANIZATION);
                    mdlOrganization.FileName = logoFileName;
                }
                else
                {
                    if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["fileName"]))
                        mdlOrganization.FileName = HttpContext.Current.Request.Form["fileName"].ToString();
                }

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["fileToUpload"]))
                {
                    if (HttpContext.Current.Request.Files.Count == 1)
                    {
                        requiredDocuments = HttpContext.Current.Request.Files[0];
                        documentFileName = await FileOpsHelper.UploadFileNew(requiredDocuments, ContainerName.ORGANIZATION);
                    }
                    else
                    {
                        requiredDocuments = HttpContext.Current.Request.Files[1];
                        documentFileName = await FileOpsHelper.UploadFileNew(requiredDocuments, ContainerName.ORGANIZATION);
                    }
                    mdlOrganization.DocumentFileName = documentFileName;
                }
                else if (requestType == "Edit")
                {
                    if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["DocumentFileName"]))
                        mdlOrganization.DocumentFileName = HttpContext.Current.Request.Form["DocumentFileName"].ToString();
                }

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["name"]))
                    mdlOrganization.Name = HttpContext.Current.Request.Form["name"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["address"]))
                    mdlOrganization.Address = HttpContext.Current.Request.Form["address"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["phone"]))
                    mdlOrganization.Phone = HttpContext.Current.Request.Form["phone"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["parentId"]))
                    mdlOrganization.ParentID = Convert.ToInt32(HttpContext.Current.Request.Form["parentId"]);
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["level"]))
                    mdlOrganization.Level = HttpContext.Current.Request.Form["level"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["siteOffice"]))
                    mdlOrganization.SiteOffice = HttpContext.Current.Request.Form["siteOffice"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["contactPerson"]))
                    mdlOrganization.ContactPerson = HttpContext.Current.Request.Form["contactPerson"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["email"]))
                    mdlOrganization.Email = HttpContext.Current.Request.Form["email"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["orgTypeID"]))
                    mdlOrganization.OrgTypeID = Convert.ToInt32(HttpContext.Current.Request.Form["orgTypeID"].ToString());
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["AreaID"]))
                    mdlOrganization.AreaID = Convert.ToInt32(HttpContext.Current.Request.Form["AreaID"].ToString());

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["cityid"]))
                    mdlOrganization.CityID = Convert.ToInt32(HttpContext.Current.Request.Form["cityid"]);

                mdlOrganization.IsVerified = true;
                mdlOrganization.IsActive = true;
                // mdlSchool.UserID = UserID;

                if (requestType != "Edit")
                {
                    if (mdlOrganization.ParentID <= 0)
                    {

                        if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["name"]))
                            regOrganization.Name = HttpContext.Current.Request.Form["name"].ToString();
                        regOrganization.IsActive = true;
                        regOrganization.CreatedBy = UserID;
                        regOrganization.CreatedDate = DateTime.Now;
                        User user = new User
                        {
                            FullName = mdlOrganization.ContactPerson,
                            Phone = mdlOrganization.Phone,
                            Email = "admin" + mdlOrganization.Email,
                            Password = "admin@1234",
                            UserTypeID = (int)UserTypeEnum.Web,
                            RoleID = (int)UserRoleTypeEnum.OrganizationAdmin,
                            CreatedBy = db.Repository<User>().GetAll().Where(x => x.RoleID == 1).Select(x => x.ID).FirstOrDefault(),
                        };
                        regOrganization.User = user;
                        mdlOrganization.RegOrganization = regOrganization;
                        //db.Repository<RegOrganization>().Insert(regOrganization);
                        //db.Save();
                        //mdlOrganization.ParentID = regOrganization.ID;
                    }

                    User mdlUsers = new User
                    {
                        FullName = mdlOrganization.ContactPerson,
                        Phone = mdlOrganization.Phone,
                        Email = mdlOrganization.Email,
                        Password = "abcd@1234",
                        UserTypeID = (int)UserTypeEnum.Web,
                        RoleID = (int)UserRoleTypeEnum.SubOrganizationAdmin,
                        CreatedBy = db.Repository<User>().GetAll().Where(x => x.RoleID == 1).Select(x => x.ID).FirstOrDefault(),
                    };
                    //db.Repository<User>().Insert(mdlUsers);
                    //db.Save();
                    //mdlSchool.UserID = mdlUsers.ID;
                    mdlOrganization.User = mdlUsers;
                }


                if (requestType != "Edit")
                {
                    mdlOrganization.EmployeeGreenPoints = 0;
                    mdlOrganization.GreenPoints = 0;
                    mdlOrganization.CreatedBy = (int)UserID;
                    mdlOrganization.CreatedDate = DateTime.Now;
                    db.Repository<Organization>().Insert(mdlOrganization);
                    db.Save();
                }

                else
                {
                    mdlOrganization.EmployeeGreenPoints = 0;
                    mdlOrganization.GreenPoints = 0;
                    mdlOrganization.UpdatedBy = (int)UserID;
                    mdlOrganization.UpdatedDate = DateTime.Now;
                    db.Repository<Organization>().Update(mdlOrganization);
                    db.Save();
                }


                int stgSchoolID = 0;
                if (requestType == "S")
                {
                    if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["stgId"]))
                    {
                        stgSchoolID = Convert.ToInt32(HttpContext.Current.Request.Form["stgId"]);
                        STG_Organization iSVerifiedOrganization = db.Repository<STG_Organization>().GetAll().Where(x => x.ID == stgSchoolID).FirstOrDefault();
                        iSVerifiedOrganization.IsVerified = true;
                        db.Repository<STG_Organization>().Update(iSVerifiedOrganization);
                        db.Save();
                    }
                }


                NotifyEvent _event = new NotifyEvent();
                _event.Parameters.Add("ContactPerson", mdlOrganization.ContactPerson);
                _event.Parameters.Add("Name", mdlOrganization.Name);
                _event.Parameters.Add("Branch", mdlOrganization.SiteOffice);
                _event.Parameters.Add("Email", mdlOrganization.Email);
                _event.Parameters.Add("Phone", mdlOrganization.Phone);
                _event.AddNotifyEvent((long)NotificationEventConstants.School.SendEmailToAdmin, Convert.ToString(UserID));


                SMSNotifyEvent _events = new SMSNotifyEvent();
                _events.Parameters.Add("ContactPerson", mdlOrganization.ContactPerson);
                _events.Parameters.Add("Phone", mdlOrganization.Phone);
                _events.Parameters.Add("Name", mdlOrganization.Name);
                _events.AddSMSNotifyEvent((long)NotificationEventConstants.School.SendSMSToUser, mdlOrganization.Phone);

                StringBuilder CodeModel = new StringBuilder();
                CodeModel.Append(mdlOrganization.ID + ";");
                CodeModel.Append(mdlOrganization.Email + ";");
                CodeModel.Append(mdlOrganization.Phone + ";");
                CodeModel.Append(mdlOrganization.Name + ";");
                string QRPath = string.Empty;
                QRPath = QRCodeTagHelper.QRCodeGeneratorImage(CodeModel);
                EmailHelper.SendEmailByTemplate(mdlOrganization.Name, mdlOrganization.Email, QRPath);
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
        public async Task<ResponseObject<List<object>>> GetOrganizationList(OrganizationRequestDto model)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                int? RoleID = JwtDecoder.GetUserRoleFromToken(Request.Headers.Authorization.Parameter);
                int roleID = Convert.ToInt32(UserRoleTypeEnum.Admin);

                //  List<Organization> organizationList = db.Repository<Organization>().GetAll().Where(x => x.UserID == UserID || RoleID == roleID && x.IsVerified==true).ToList();
                //return ServiceResponse.SuccessReponse(organizationList, MessageEnum.DefaultSuccessMessage);
                var approvedOrgs = db.ExtRepositoryFor<OrganizationRepository>().GetApprovedOrganizationList(model,UserID);

                if (approvedOrgs.Count == 0)
                    return ServiceResponse.SuccessReponse(approvedOrgs, MessageEnum.RegiftItemsNotFound);
                else
                    return ServiceResponse.SuccessReponse(approvedOrgs, MessageEnum.RegiftItemGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }

        public async Task<ResponseObject<List<object>>> GetSuspendedOrganizationList()
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                int? RoleID = JwtDecoder.GetUserRoleFromToken(Request.Headers.Authorization.Parameter);
                int roleID = Convert.ToInt32(UserRoleTypeEnum.Admin);

                //  List<Organization> organizationList = db.Repository<Organization>().GetAll().Where(x => x.UserID == UserID || RoleID == roleID && x.IsVerified==true).ToList();
                //return ServiceResponse.SuccessReponse(organizationList, MessageEnum.DefaultSuccessMessage);
                var approvedOrgs = db.ExtRepositoryFor<OrganizationRepository>().GetSuspendedOrganizationList(UserID);

                if (approvedOrgs.Count == 0)
                    return ServiceResponse.SuccessReponse(approvedOrgs, MessageEnum.RegiftItemsNotFound);
                else
                    return ServiceResponse.SuccessReponse(approvedOrgs, MessageEnum.RegiftItemGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<List<Member>> GetMemberList()
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                int? RoleID = JwtDecoder.GetUserRoleFromToken(Request.Headers.Authorization.Parameter);
                int roleID = Convert.ToInt32(UserRoleTypeEnum.Admin);

                List<Member> memberList = db.Repository<Member>().GetAll().Where(x => x.Organization.UserID == UserID || roleID == RoleID).ToList();
                return ServiceResponse.SuccessReponse(memberList, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Member>>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<Member> GetMemberDetail(int ID)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                int roleID = Convert.ToInt32(UserRoleTypeEnum.Admin);

                Member memberDetail = db.Repository<Member>().GetAll().Where(x => x.ID == ID).FirstOrDefault();
                return ServiceResponse.SuccessReponse(memberDetail, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<Member>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<Member> GetMemberStatus(Member member)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                int roleID = Convert.ToInt32(UserRoleTypeEnum.Admin);

                Member memberDetail = db.Repository<Member>().GetAll().Where(x => x.ID == member.ID).FirstOrDefault();
                memberDetail.IsActive = member.IsActive;
                return ServiceResponse.SuccessReponse(memberDetail, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<Member>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<List<object>> GetMemberListByRole(bool IsSuspended)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                int? RoleID = JwtDecoder.GetUserRoleFromToken(Request.Headers.Authorization.Parameter);
                
                List<object> memberList = db.ExtRepositoryFor<OrganizationRepository>().GetMemberListByRole(UserID, IsSuspended, RoleID);
                return ServiceResponse.SuccessReponse(memberList, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<List<object>> GetMemberListByRoleWithMemberProgress(bool IsSuspended)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                int? RoleID = JwtDecoder.GetUserRoleFromToken(Request.Headers.Authorization.Parameter);

                List<object> memberList = db.ExtRepositoryFor<OrganizationRepository>().GetMemberListByRoleWithMemberProgress(UserID, IsSuspended, RoleID);
                return ServiceResponse.SuccessReponse(memberList, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }
        [HttpPost]
        public ResponseObject<bool> SuspendMember(int id)
        {
            try
            {
                Member member = db.Repository<Member>().GetAll().Where(x => x.ID == id).FirstOrDefault();

                member.IsActive = false;

                db.Repository<Member>().Update(member);
                db.Save();
                PushNotificationEvent _event = new PushNotificationEvent();
                _event.Parameters.Add("GPN", member.Organization.Name);
                _event.AddPushNotifyEvent((long)NotificationEventConstants.PushNotification.NotifyMemberOnSuspention, member.UserID.ToString());

                //send sms to user for to notification.
                //SMSNotifyEvent _events = new SMSNotifyEvent();
                //_events.Parameters.Add("SMSCode", "");
                //_events.AddSMSNotifyEvent((long)NotificationEventConstants.SuspendUsers.SMSSendtoMember, member.User.Phone);



                return ServiceResponse.SuccessReponse(true, MessageEnum.SuspendSuccessfully);
            }
            catch (Exception exp)
            {

                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
        [HttpGet]
        public ResponseObject<bool> InactiveStgOrg(int ID)
        {
            try
            {
                STG_Organization org = db.Repository<STG_Organization>().GetAll().Where(x => x.ID == ID).FirstOrDefault();
                org.IsActive = false;
                db.Repository<STG_Organization>().Update(org);
                db.Save();
                return ServiceResponse.SuccessReponse(true, MessageEnum.OrgSuspended);
            }
            catch (Exception exp)
            {

                return ServiceResponse.ErrorReponse<bool>(exp);
            }

        }

        [HttpGet]
        public ResponseObject<bool> SuspendOrg(int ID)
        {
            try
            {
                Organization org = db.Repository<Organization>().GetAll().Where(x => x.ID == ID).FirstOrDefault();
                org.IsActive = false;
                db.Repository<Organization>().Update(org);
                db.Save();
                //Suspend member of the orgs
                var membersList = db.Repository<Member>().GetAll().Where(x => x.OrgId == org.ID).ToList();
                foreach (var membr in membersList)
                {
                    membr.IsActive = false;
                }
                return ServiceResponse.SuccessReponse(true, MessageEnum.OrgSuspended);
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
                Organization org = db.Repository<Organization>().GetAll().Where(x => x.ID == ID).FirstOrDefault();
                org.IsActive = true;
                db.Repository<Organization>().Update(org);
                db.Save();
                //Suspend member of the orgs
                var membersList = db.Repository<Member>().GetAll().Where(x => x.OrgId == org.ID).ToList();
                foreach (var membr in membersList)
                {
                    membr.IsActive = true;
                }
                return ServiceResponse.SuccessReponse(true, MessageEnum.OrgRestored);
            }
            catch (Exception exp)
            {

                return ServiceResponse.ErrorReponse<bool>(exp);
            }

        }

        public ResponseObject<bool> ActivateMember(int Id)
        {
            try
            {
                var member = db.Repository<Member>().FindById(Id);
                member.IsActive = true;
                db.Repository<Member>().Update(member);
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

                List<object> sectionsList = db.ExtRepositoryFor<OrganizationRepository>().GetDepartmentsByRole(UserID, RoleID);
                return ServiceResponse.SuccessReponse(sectionsList, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Object>>(exp);
            }
        }
        [HttpGet]
        public ResponseObject<List<Object>> GetOrganizationComparison(string dpt, string org, string fd, string td)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                int? RoleID = JwtDecoder.GetUserRoleFromToken(Request.Headers.Authorization.Parameter);

                int organizationId = 0;

                if (RoleID == (int)UserRoleTypeEnum.SubOrganizationAdmin)
                    organizationId = db.Repository<Organization>().GetAll().Where(x => x.UserID == UserID).FirstOrDefault().ID;

                List<object> resultList = new List<object>();

                if ((organizationId != 0 && RoleID == (int)UserRoleTypeEnum.SubOrganizationAdmin) || RoleID == (int)UserRoleTypeEnum.OrganizationAdmin)
                {
                    DateTime fromDate = Utility.GetParsedDate(fd).Date;
                    DateTime toDate = Utility.GetParsedDate(td).Date.AddDays(1).AddSeconds(-1);

                    resultList = db.ExtRepositoryFor<OrganizationRepository>().GetOrganizationComparison(organizationId, dpt, org, fromDate, toDate, RoleID);
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
        public ResponseObject<List<Object>> GetBranchesByOrganizationAdmin()
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                List<object> branchesList = db.ExtRepositoryFor<OrganizationRepository>().GetBranchesByOrganizationAdmin(UserID);
                return ServiceResponse.SuccessReponse(branchesList, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Object>>(exp);
            }
        }
    }
}
