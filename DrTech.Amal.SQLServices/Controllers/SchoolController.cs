using DrTech.Amal.Common.Enums;
using DrTech.Amal.Common.Helpers;
using DrTech.Amal.Common.ServerResponse;
using DrTech.Amal.Notifications;
using DrTech.Amal.SQLDataAccess.CustomModels;
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
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using static DrTech.Amal.Common.Extentions.Constants;

namespace DrTech.Amal.SQLServices.Controllers
{
    [Authorize]
    public class SchoolController : BaseController
    {
        [HttpPost]
        public async System.Threading.Tasks.Task<ResponseObject<bool>> AddSchoolInformation()
        {
            try
            {
                STG_School mdlSchool = new STG_School();
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                string AreaName = string.Empty;
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                //string FileName = string.Empty;
                //HttpPostedFile file = HttpContext.Current.Request.Files[0];
                //FileName = await FileOpsHelper.UploadFileNew(file, ContainerName.SCHOOL);                

                mdlSchool.FileName = "";

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["name"]))
                    mdlSchool.Name = HttpContext.Current.Request.Form["name"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["address"]))
                    mdlSchool.Address = HttpContext.Current.Request.Form["address"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["phone"]))
                    mdlSchool.Phone = HttpContext.Current.Request.Form["phone"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["parentId"]))
                    mdlSchool.ParentID = Convert.ToInt32(HttpContext.Current.Request.Form["parentId"]);
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["level"]))
                    mdlSchool.Level = HttpContext.Current.Request.Form["level"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["branchName"]))
                    mdlSchool.BranchName = HttpContext.Current.Request.Form["branchName"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["contactPerson"]))
                    mdlSchool.ContactPerson = HttpContext.Current.Request.Form["contactPerson"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["contactPersonPhone"]))
                    mdlSchool.ContactPersonPhone = HttpContext.Current.Request.Form["contactPersonPhone"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["email"]))
                    mdlSchool.Email = HttpContext.Current.Request.Form["email"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["cityid"]))
                    mdlSchool.CityID = Convert.ToInt32(HttpContext.Current.Request.Form["cityid"]);
                //if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["areaid"]))
                //    mdlSchool.AreaID = Convert.ToInt32(HttpContext.Current.Request.Form["areaid"]);
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["areaname"]))
                    AreaName = HttpContext.Current.Request.Form["areaname"];

                mdlSchool.IsVerified = false;
                mdlSchool.IsActive = true;
                mdlSchool.UserID = UserID;
                mdlSchool.ParentsGreenPoints = 0;
                mdlSchool.GreenPoints = 0;
                mdlSchool.CreatedBy = (int)UserID;
                mdlSchool.CreatedDate = DateTime.Now;
                //  mdlSchool.UserID = UserID;
                //if (AreaName != null)
                //{
                //    var UserArea = db.Repository<Area>().GetAll().Where(x => x.Name == AreaName).FirstOrDefault();
                //    if (UserArea == null)
                //    {

                //        Area area = new Area();
                //        area.Name = AreaName;
                //        area.CityID = mdlSchool.CityID;
                //        area.IsActive = true;
                //        area.CreatedBy = mdlSchool.ID;
                //        area.CreatedDate = DateTime.UtcNow;
                //        db.Repository<Area>().Insert(area);
                //        db.Save();
                //        UserArea = db.Repository<Area>().GetAll().Where(x => x.Name == AreaName).FirstOrDefault();
                //        mdlSchool.AreaID = UserArea.ID;
                //    }
                //}

                db.Repository<STG_School>().Insert(mdlSchool);
                db.Save();

                NotifyEvent _event = new NotifyEvent();
                _event.Parameters.Add("ContactPerson", mdlSchool.ContactPerson);
                _event.Parameters.Add("Name", mdlSchool.Name);
                _event.Parameters.Add("Branch", mdlSchool.BranchName);
                _event.Parameters.Add("Email", mdlSchool.Email);
                _event.Parameters.Add("Phone", mdlSchool.Phone);
                _event.AddNotifyEvent((long)NotificationEventConstants.School.SendEmailToAdmin, Convert.ToString(UserID));

                SMSNotifyEvent _events = new SMSNotifyEvent();
                _events.Parameters.Add("ContactPerson", mdlSchool.ContactPerson);
                _events.Parameters.Add("Phone", mdlSchool.Phone);
                _events.Parameters.Add("Name", mdlSchool.Name);
                _events.AddSMSNotifyEvent((long)NotificationEventConstants.School.SendSMSToUser, mdlSchool.Phone);

                return ServiceResponse.SuccessReponse(true, MessageEnum.SchoolAddedSuccessfully);
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
        public ResponseObject<List<School>> GetSchoolsDropdowns()
        {
            try
            {
                var dropdowns = db.Repository<School>().GetAll().ToList();
                return ServiceResponse.SuccessReponse(dropdowns, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<School>>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<List<object>> GetSchoolsListWithChild()
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                List<object> dropdowns = db.ExtRepositoryFor<SchoolRepository>().GetAllSchoolsWithRegisteredChildren(UserID);
                return ServiceResponse.SuccessReponse(dropdowns, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<List<object>> GetBranchesWithRegistrationStatus(int ID)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                List<object> list = db.ExtRepositoryFor<SchoolRepository>().GetAllSchoolBranchesWithRegistrationStatus(UserID, ID);
                return ServiceResponse.SuccessReponse(list, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<List<object>> GetSchoolBranchDropdown(int ID)
        {
            try
            {
                var dorpdowns = db.ExtRepositoryFor<SchoolRepository>().GetSchoolBranchesByID(ID);
                return ServiceResponse.SuccessReponse(dorpdowns, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<List<STG_School>> GetStgSchoolList()
        {
            try
            {
                List<STG_School> regSchool = db.Repository<STG_School>().GetAll().Where(x => x.IsVerified == false && x.IsActive != false).ToList();
                return ServiceResponse.SuccessReponse(regSchool, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<STG_School>>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<object> GetStgSchoolByID(string Param)
        {
            try
            {
                string[] requestParam = Param.Split('-');
                int id = Convert.ToInt32(requestParam[0]);
                if (requestParam[1] != "Edit")
                {
                    object stgSchool = db.Repository<STG_School>().GetAll().Where(x => x.ID == id).FirstOrDefault();
                    return ServiceResponse.SuccessReponse(stgSchool, MessageEnum.DefaultSuccessMessage);
                }
                else
                {
                    object school = db.Repository<School>().GetAll().Where(x => x.ID == id).FirstOrDefault();
                    return ServiceResponse.SuccessReponse(school, MessageEnum.DefaultSuccessMessage);
                }
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<List<RegSchool>> GetRegSchoolDropdown()
        {
            try
            {
                List<RegSchool> regSchool = db.Repository<RegSchool>().GetAll().Where(x => x.IsActive == true).ToList();
                regSchool.Add(new RegSchool { ID = 0, Name = "Not Registered" });
                return ServiceResponse.SuccessReponse(regSchool, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<RegSchool>>(exp);
            }
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ResponseObject<bool>> AddSchool()
        {
            try
            {
                School mdlSchool = new School();
                RegSchool regSchool = new RegSchool();
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
                        mdlSchool = db.Repository<School>().FindById(Convert.ToInt32(HttpContext.Current.Request.Form["id"].ToString()));
                }

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["imageToUpload"]))
                {
                    HttpPostedFile file = HttpContext.Current.Request.Files[0];
                    logoFileName = await FileOpsHelper.UploadFileNew(file, ContainerName.SCHOOL);
                    mdlSchool.FileName = logoFileName;
                }
                else
                {
                    if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["fileName"]))
                        mdlSchool.FileName = HttpContext.Current.Request.Form["fileName"].ToString();
                }
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["fileToUpload"]))
                {
                    if (HttpContext.Current.Request.Files.Count == 1)
                    {
                        requiredDocuments = HttpContext.Current.Request.Files[0];
                        documentFileName = await FileOpsHelper.UploadFileNew(requiredDocuments, ContainerName.SCHOOL);
                    }
                    else
                    {
                        requiredDocuments = HttpContext.Current.Request.Files[1];
                        documentFileName = await FileOpsHelper.UploadFileNew(requiredDocuments, ContainerName.SCHOOL);
                    }
                    mdlSchool.DocumentFileName = documentFileName;
                }
                else if (requestType == "Edit")
                {
                    if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["DocumentFileName"]))
                        mdlSchool.DocumentFileName = HttpContext.Current.Request.Form["DocumentFileName"].ToString();
                }
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["name"]))
                    mdlSchool.Name = HttpContext.Current.Request.Form["name"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["address"]))
                    mdlSchool.Address = HttpContext.Current.Request.Form["address"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["phone"]))
                    mdlSchool.Phone = HttpContext.Current.Request.Form["phone"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["parentId"]))
                    mdlSchool.ParentID = Convert.ToInt32(HttpContext.Current.Request.Form["parentId"]);
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["level"]))
                    mdlSchool.Level = HttpContext.Current.Request.Form["level"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["branchName"]))
                    mdlSchool.BranchName = HttpContext.Current.Request.Form["branchName"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["contactPerson"]))
                    mdlSchool.ContactPerson = HttpContext.Current.Request.Form["contactPerson"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["contactPersonPhone"]))
                    mdlSchool.ContactPersonPhone = HttpContext.Current.Request.Form["contactPersonPhone"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["email"]))
                    mdlSchool.Email = HttpContext.Current.Request.Form["email"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["regFormat"]))
                    mdlSchool.RegFormat = HttpContext.Current.Request.Form["regFormat"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["cityid"]))
                    mdlSchool.CityID = Convert.ToInt32(HttpContext.Current.Request.Form["cityid"]);
                mdlSchool.IsVerified = true;
                mdlSchool.IsActive = true;
                // mdlSchool.UserID = UserID;
                if (requestType != "Edit")
                {
                    if (mdlSchool.ParentID <= 0)
                    {

                        if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["name"]))
                            regSchool.Name = HttpContext.Current.Request.Form["name"].ToString();
                        regSchool.IsActive = true;
                        regSchool.CreatedBy = UserID;
                        regSchool.CreatedDate = DateTime.Now;
                        User user = new User
                        {
                            FullName = mdlSchool.ContactPerson,
                            Phone = mdlSchool.Phone,
                            Email = "admin" + mdlSchool.Email,
                            Password = PasswordGenerator.Generate(12),
                            UserTypeID = (int)UserTypeEnum.Web,
                            RoleID = (int)UserRoleTypeEnum.SchoolAdmin,
                            CreatedBy = db.Repository<User>().GetAll().Where(x => x.RoleID == 1).Select(x => x.ID).FirstOrDefault(),
                        };
                        regSchool.User = user;
                        mdlSchool.RegSchool = regSchool;
                        //db.Repository<RegSchool>().Insert(regSchool);
                        //db.Save();
                        //mdlSchool.ParentID = regSchool.ID;
                    }

                    User mdlUsers = new User
                    {
                        FullName = mdlSchool.ContactPerson,
                        Phone = mdlSchool.Phone,
                        Email = mdlSchool.Email,
                        Password = PasswordGenerator.Generate(15),
                        UserTypeID = (int)UserTypeEnum.Web,
                        RoleID = (int)UserRoleTypeEnum.SubSchoolAdmin,
                        CreatedBy = db.Repository<User>().GetAll().Where(x => x.RoleID == 1).Select(x => x.ID).FirstOrDefault(),
                    };
                    //db.Repository<User>().Insert(mdlUsers);
                    //db.Save();
                    //mdlSchool.UserID = mdlUsers.ID;
                    mdlSchool.User = mdlUsers;
                }
                if (requestType != "Edit")
                {
                    mdlSchool.ParentsGreenPoints = 0;
                    mdlSchool.GreenPoints = 0;
                    mdlSchool.CreatedBy = (int)UserID;
                    mdlSchool.CreatedDate = DateTime.Now;
                    db.Repository<School>().Insert(mdlSchool);
                    db.Save();
                }
                else
                {
                    mdlSchool.ParentsGreenPoints = 0;
                    mdlSchool.GreenPoints = 0;
                    mdlSchool.UpdatedBy = (int)UserID;
                    mdlSchool.UpdatedDate = DateTime.Now;
                    db.Repository<School>().Update(mdlSchool);
                    db.Save();
                }
                int stgSchoolID = 0;
                if (requestType == "S")
                {
                    if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["stgId"]))
                    {
                        stgSchoolID = Convert.ToInt32(HttpContext.Current.Request.Form["stgId"]);
                        STG_School iSVerifiedSchool = db.Repository<STG_School>().GetAll().Where(x => x.ID == stgSchoolID).FirstOrDefault();
                        iSVerifiedSchool.IsVerified = true;
                        db.Repository<STG_School>().Update(iSVerifiedSchool);
                        db.Save();
                    }
                }
                NotifyEvent _event = new NotifyEvent();
                _event.Parameters.Add("ContactPerson", mdlSchool.ContactPerson);
                _event.Parameters.Add("Name", mdlSchool.Name);
                _event.Parameters.Add("Branch", mdlSchool.BranchName);
                _event.Parameters.Add("Email", mdlSchool.Email);
                _event.Parameters.Add("Phone", mdlSchool.Phone);
                _event.AddNotifyEvent((long)NotificationEventConstants.School.SendEmailToAdmin, Convert.ToString(UserID));
                SMSNotifyEvent _events = new SMSNotifyEvent();
                _events.Parameters.Add("ContactPerson", mdlSchool.ContactPerson);
                _events.Parameters.Add("Phone", mdlSchool.Phone);
                _events.Parameters.Add("Name", mdlSchool.Name);
                _events.AddSMSNotifyEvent((long)NotificationEventConstants.School.SendSMSToUser, mdlSchool.Phone);
                StringBuilder CodeModel = new StringBuilder();
                CodeModel.Append(mdlSchool.ID + ";");
                CodeModel.Append(mdlSchool.Email + ";");
                CodeModel.Append(mdlSchool.Phone + ";");
                CodeModel.Append(mdlSchool.Name + ";");
                string QRPath = string.Empty;
                QRPath = QRCodeTagHelper.QRCodeGeneratorImage(CodeModel);
                EmailHelper.SendEmailByTemplate(mdlSchool.Name, mdlSchool.Email, QRPath);
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
        public ResponseObject<List<School>> GetSchoolList(SchoolRequestDto model)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                int? RoleID = JwtDecoder.GetUserRoleFromToken(Request.Headers.Authorization.Parameter);
                int roleID = Convert.ToInt32(UserRoleTypeEnum.Admin);

                List<School> schoolList = db.Repository<School>().GetAll().Where(x => x.UserID == UserID || RoleID == roleID && x.IsVerified == true && x.IsActive == true).ToList();
                if(model.StartDate != null && model.EndDate != null)
                {
                schoolList = schoolList.Where(x => x.CreatedDate >= Utility.GetDateFromString(model.StartDate) && x.CreatedDate <= Utility.GetDateFromString(model.EndDate)).OrderByDescending(x => x.CreatedDate).ToList();
                }
                return ServiceResponse.SuccessReponse(schoolList, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<School>>(exp);
            }
        }
        [HttpGet]
        public ResponseObject<List<School>> GetSuspendedSchoolList()
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                int? RoleID = JwtDecoder.GetUserRoleFromToken(Request.Headers.Authorization.Parameter);
                int roleID = Convert.ToInt32(UserRoleTypeEnum.Admin);

                List<School> schoolList = db.Repository<School>().GetAll().Where(x => x.UserID == UserID || RoleID == roleID && x.IsActive == false).ToList();
                return ServiceResponse.SuccessReponse(schoolList, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<School>>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<List<Child>> GetStudentList()
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                int? RoleID = JwtDecoder.GetUserRoleFromToken(Request.Headers.Authorization.Parameter);
                int roleID = Convert.ToInt32(UserRoleTypeEnum.Admin);

                List<Child> studentList = db.Repository<Child>().GetAll().Where(x => x.School.UserID == UserID || roleID == RoleID).ToList();
                return ServiceResponse.SuccessReponse(studentList, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Child>>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<Child> GetStudentDetail(int ID)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                int roleID = Convert.ToInt32(UserRoleTypeEnum.Admin);

                Child studentDetail = db.Repository<Child>().GetAll().Where(x => x.ID == ID).FirstOrDefault();
                return ServiceResponse.SuccessReponse(studentDetail, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<Child>(exp);
            }
        }

        [HttpPost]
        public ResponseObject<bool> UpdateStudentStatus(Child Student)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                int roleID = Convert.ToInt32(UserRoleTypeEnum.Admin);

                Child studentDetail = db.Repository<Child>().GetAll().Where(x => x.ID == Student.ID).FirstOrDefault();
                studentDetail.IsActive = Student.IsActive;
                db.Repository<Child>().Update(studentDetail);
                db.Save();
                return ServiceResponse.SuccessReponse(true, MessageEnum.KidsInfoUpdated);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<School> GetSchoolDetail(int ID)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                int roleID = Convert.ToInt32(UserRoleTypeEnum.Admin);

                School schoolDetail = db.Repository<School>().GetAll().Where(x => x.ID == ID).FirstOrDefault();
                return ServiceResponse.SuccessReponse(schoolDetail, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<School>(exp);
            }
        }

        [HttpPost]
        public ResponseObject<bool> UpdateSchoolStatus(School school)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                int roleID = Convert.ToInt32(UserRoleTypeEnum.Admin);

                School schoolDetail = db.Repository<School>().GetAll().Where(x => x.ID == school.ID).FirstOrDefault();
                schoolDetail.IsActive = school.IsActive;
                db.Repository<School>().Update(schoolDetail);
                db.Save();
                return ServiceResponse.SuccessReponse(true, MessageEnum.SchoolInfoUpdated);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<bool> InactiveStgSchool(int ID)
        {
            try
            {
                STG_School iSVerifiedSchool = db.Repository<STG_School>().GetAll().Where(x => x.ID == ID).FirstOrDefault();
                iSVerifiedSchool.IsActive = false;
                db.Repository<STG_School>().Update(iSVerifiedSchool);
                db.Save();
                return ServiceResponse.SuccessReponse(true, MessageEnum.SchoolSuspended);
            }
            catch (Exception exp)
            {

                return ServiceResponse.ErrorReponse<bool>(exp);
            }

        }

        [HttpGet]
        public ResponseObject<bool> SuspendSchool(int ID)
        {
            try
            {
                School school = db.Repository<School>().GetAll().Where(x => x.ID == ID).FirstOrDefault();
                school.IsActive = false;
                db.Repository<School>().Update(school);
                db.Save();
                //Also suspend the childs of this school.
                var listChilds = db.Repository<Child>().GetAll().Where(x => x.SchoolID == school.ID).ToList();
                foreach (var child in listChilds)
                {
                    child.IsActive = false;
                    // item.User.IsActive = false;
                    db.Save();
                }
                //Also suspend the staff of this school.
                var schoolStaffList = db.Repository<SchoolStaff>().GetAll().Where(x => x.SchoolID == school.ID).ToList();
                foreach (var staff in schoolStaffList)
                {
                    staff.IsActive = false;
                    db.Save();
                }

                return ServiceResponse.SuccessReponse(true, MessageEnum.SchoolSuspended);
            }
            catch (Exception exp)
            {

                return ServiceResponse.ErrorReponse<bool>(exp);
            }

        }

        [HttpGet]
        public ResponseObject<List<object>> GetStudentListByRole(bool IsSuspended)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                int? RoleID = JwtDecoder.GetUserRoleFromToken(Request.Headers.Authorization.Parameter);

                List<object> studentList = db.ExtRepositoryFor<SchoolRepository>().GetStudentListByRole(UserID, IsSuspended, RoleID);
                return ServiceResponse.SuccessReponse(studentList, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<List<object>> GetStudentListByRoleWithPointsProgress(bool IsSuspended)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                int? RoleID = JwtDecoder.GetUserRoleFromToken(Request.Headers.Authorization.Parameter);

                List<object> studentList = db.ExtRepositoryFor<SchoolRepository>().GetStudentListByRoleWithPointsProgress(UserID, IsSuspended, RoleID);
                return ServiceResponse.SuccessReponse(studentList, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }
        [HttpGet]
        public ResponseObject<List<object>> GetStaffListByRole(bool IsSuspended)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                int? RoleID = JwtDecoder.GetUserRoleFromToken(Request.Headers.Authorization.Parameter);

                List<object> staffList = db.ExtRepositoryFor<SchoolRepository>().GetStaffListByRole(UserID, IsSuspended, RoleID);
                return ServiceResponse.SuccessReponse(staffList, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }
        [HttpPost]
        public ResponseObject<bool> SuspendChild(int id)
        {
            try
            {
                Child child = db.Repository<Child>().GetAll().Where(x => x.ID == id).FirstOrDefault();

                child.IsActive = false;

                db.Repository<Child>().Update(child);
                db.Save();
                PushNotificationEvent _event = new PushNotificationEvent();
                _event.Parameters.Add("GPN", child.School.Name);
                _event.AddPushNotifyEvent((long)NotificationEventConstants.PushNotification.NotifyStudentOnSuspention, child.UserID.ToString());

                //send sms to user for to notification.
                //SMSNotifyEvent _events = new SMSNotifyEvent();
                //_events.Parameters.Add("SMSCode", "");

                //_events.AddSMSNotifyEvent((long)NotificationEventConstants.SuspendUsers.SMSSendtoStudent, child.User.Phone);

                return ServiceResponse.SuccessReponse(true, MessageEnum.SchoolAddedSuccessfully);
            }
            catch (Exception exp)
            {

                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpPost]
        public ResponseObject<bool> SuspendStaff(int id)
        {
            try
            {
                SchoolStaff schoolStaff = db.Repository<SchoolStaff>().GetAll().Where(x => x.ID == id).FirstOrDefault();

                schoolStaff.IsActive = false;
                db.Repository<SchoolStaff>().Update(schoolStaff);
                db.Save();
                PushNotificationEvent _event = new PushNotificationEvent();
                _event.Parameters.Add("GPN", schoolStaff.School.Name);
                _event.AddPushNotifyEvent((long)NotificationEventConstants.PushNotification.NotifyStaffOnSuspention, schoolStaff.UserID.ToString());
                //send sms to user for to notification.
                //SMSNotifyEvent _events = new SMSNotifyEvent();
                //_events.Parameters.Add("SMSCode", "");

                //_events.AddSMSNotifyEvent((long)NotificationEventConstants.SuspendUsers.SMSSendtoStaff, schoolStaff.User.Phone);



                return ServiceResponse.SuccessReponse(true, MessageEnum.SuspendSuccessfully);
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
                School school = db.Repository<School>().GetAll().Where(x => x.ID == ID).FirstOrDefault();
                school.IsActive = true;
                db.Repository<School>().Update(school);
                db.Save();
                var listChilds = db.Repository<Child>().GetAll().Where(x => x.SchoolID == school.ID).ToList();
                foreach (var child in listChilds)
                {
                    child.IsActive = true;
                    // item.User.IsActive = true;
                    db.Save();
                }
                //Also suspend the staff of this school.
                var schoolStaffList = db.Repository<SchoolStaff>().GetAll().Where(x => x.SchoolID == school.ID).ToList();
                foreach (var staff in schoolStaffList)
                {
                    staff.IsActive = true;
                    db.Save();
                }
                return ServiceResponse.SuccessReponse(true, MessageEnum.BusinessRestored);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }

        }

        [HttpGet]
        public ResponseObject<List<Object>> GetClassBySchool()
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                int? RoleID = JwtDecoder.GetUserRoleFromToken(Request.Headers.Authorization.Parameter);

                List<object> classesList = db.ExtRepositoryFor<SchoolRepository>().GetClassesBySchool(UserID, RoleID);
                return ServiceResponse.SuccessReponse(classesList, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Object>>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<List<Object>> GetSectionByClass(string Class)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                int? RoleID = JwtDecoder.GetUserRoleFromToken(Request.Headers.Authorization.Parameter);

                List<object> sectionsList = db.ExtRepositoryFor<SchoolRepository>().GetSectionByClass(UserID, RoleID, Class);
                return ServiceResponse.SuccessReponse(sectionsList, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Object>>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<List<Object>> GetBranchesBySchoolAdmin()
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                List<object> branchesList = db.ExtRepositoryFor<SchoolRepository>().GetBranchesBySchoolAdmin(UserID);
                return ServiceResponse.SuccessReponse(branchesList, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Object>>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<List<Object>> GetSchoolComparison(string clas, string sct, string sch, string fd, string td)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                int? RoleID = JwtDecoder.GetUserRoleFromToken(Request.Headers.Authorization.Parameter);

                int branchId = 0;

                if (RoleID == (int)UserRoleTypeEnum.SubSchoolAdmin)
                    branchId = db.Repository<School>().GetAll().Where(x => x.UserID == UserID).FirstOrDefault().ID;

                List<object> resultList = new List<object>();

                if ((branchId != 0 && RoleID == (int)UserRoleTypeEnum.SubSchoolAdmin) || RoleID == (int)UserRoleTypeEnum.SchoolAdmin)
                {
                    DateTime fromDate = Utility.GetParsedDate(fd).Date;
                    DateTime toDate = Utility.GetParsedDate(td).Date.AddDays(1).AddSeconds(-1);

                    resultList = db.ExtRepositoryFor<SchoolRepository>().GetSchoolComparison(branchId, clas, sct, sch, fromDate, toDate, RoleID);
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

        public ResponseObject<bool> ActivateStudent(int Id)
        {
            try
            {
                var child = db.Repository<Child>().FindById(Id);
                child.IsActive = true;
                db.Repository<Child>().Update(child);
                db.Save();

                return ServiceResponse.SuccessReponse(true, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
        public ResponseObject<bool> ActivateSchoolStaff(int Id)
        {
            try
            {
                var schoolStaff = db.Repository<SchoolStaff>().FindById(Id);
                schoolStaff.IsActive = true;
                db.Repository<SchoolStaff>().Update(schoolStaff);
                db.Save();

                return ServiceResponse.SuccessReponse(true, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ResponseObject<List<SchoolsComparisionResult>>> GetSchoolsBranchesComparisionChartBySchoolAdmin(SchoolsComparisionCriteria model)
        {
            int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
            var reuslt = db.ExtRepositoryFor<SchoolRepository>().GetSchoolsBranchesComparisionChartBySchoolAdmin(model, Convert.ToInt32(UserID));
            return ServiceResponse.SuccessReponse(reuslt, MessageEnum.RecordFoundSuccessfully);

        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ResponseObject<List<Records>>> GetSchoolsBranchesComparisionPieChartBySchoolAdmin()
        {
            int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
            var reuslt = db.ExtRepositoryFor<SchoolRepository>().GetSchoolsBranchesComparisionPieChartBySchoolAdmin(Convert.ToInt32(UserID));
            return ServiceResponse.SuccessReponse(reuslt, MessageEnum.RecordFoundSuccessfully);

        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ResponseObject<List<Records>>> GetSchoolsBranchesStudentsPieChartBySchoolAdmin()
        {
            int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
            var reuslt = db.ExtRepositoryFor<SchoolRepository>().GetSchoolsBranchesStudentsPieChartBySchoolAdmin(Convert.ToInt32(UserID));
            return ServiceResponse.SuccessReponse(reuslt, MessageEnum.RecordFoundSuccessfully);

        }
        [HttpGet]
        public async Task<ResponseObject<List<Object>>> GetSchoolBranchesByUserId()
        {
            int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
            var reuslt = db.ExtRepositoryFor<SchoolRepository>().GetSchoolBranchesByUserId(Convert.ToInt32(UserID));
            return ServiceResponse.SuccessReponse(reuslt, MessageEnum.RecordFoundSuccessfully);

        }
        [HttpPost]
        public async Task<ResponseObject<List<Object>>> StudentsBySchool(BranchRequest model)
        {
            int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
            var reuslt = db.ExtRepositoryFor<SchoolRepository>().GetSchoolStudentsBySchoolId(model,UserID);
            return ServiceResponse.SuccessReponse(reuslt, MessageEnum.RecordFoundSuccessfully);

        }
    }
}
