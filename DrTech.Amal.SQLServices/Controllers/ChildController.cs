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

namespace DrTech.Amal.SQLServices.Controllers
{
    [Authorize]
    public class ChildController : BaseController
    {

        [HttpPost]
        public async Task<ResponseObject<bool>> AddKidsInformation()
        {
            try
            {

             

                Child _mdlChild = new Child();
                string FileName = string.Empty;
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                //Check
                bool IsExists = db.ExtRepositoryFor<CommonRepository>().ExistsAssociations(UserID);
                if (IsExists == false)
                {
                    NotifyEvent _event = new NotifyEvent();
                   // _event.Parameters.Add("GPNName", school.Name);
                    _event.AddNotifyEvent((long)NotificationEventConstants.Email.SendEmailToUser, Convert.ToString(UserID));
                }
                // first time registration
                var lstUsers = db.Repository<Child>().GetAll().Where(x => x.UserID == UserID).ToList();
                if (lstUsers.Count == 0)
                {
                    NotifyEvent _eventProcess = new NotifyEvent();
                    _eventProcess.AddNotifyEvent((long)NotificationEventConstants.Users.SendEmailToSchoolUserForFurtherProcess, Convert.ToString(UserID));
                }


                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                FileName = await FileOpsHelper.UploadFileNew(file, ContainerName.CHILD);
                _mdlChild.FileName = FileName;

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["name"]))
                    _mdlChild.Name = HttpContext.Current.Request.Form["name"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["schoolId"]))
                    _mdlChild.SchoolID = Convert.ToInt32(HttpContext.Current.Request.Form["schoolId"]);
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["sectionName"]))
                    _mdlChild.SectionName = HttpContext.Current.Request.Form["sectionName"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["className"]))
                    _mdlChild.ClassName = HttpContext.Current.Request.Form["className"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["rollNo"]))
                    _mdlChild.RegistrationNo = Convert.ToString(HttpContext.Current.Request.Form["rollNo"]);
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["gender"]))
                    _mdlChild.Gender = HttpContext.Current.Request.Form["gender"].ToString();
                _mdlChild.UserID = (int)UserID;
                _mdlChild.CreatedBy = (int)UserID;
                _mdlChild.CreatedDate = DateTime.Now;
                _mdlChild.IsVerified = false;
                _mdlChild.IsActive = true;
                db.Repository<Child>().Insert(_mdlChild);
                db.Save();
                var school = db.Repository<School>().FindById(_mdlChild.SchoolID);
                if (school != null)
                {
                    NotifyEvent _event = new NotifyEvent();
                    _event.Parameters.Add("ChildName", _mdlChild.Name);
                    _event.Parameters.Add("Gender", _mdlChild.Gender);
                    _event.Parameters.Add("RollNo", _mdlChild.RegistrationNo);
                    _event.Parameters.Add("SchoolName", school.Name);
                    _event.Parameters.Add("Branch", school.BranchName);
                    _event.AddNotifyEvent((long)NotificationEventConstants.Children.SendEmailToAdmin, Convert.ToString(UserID));

                    SMSNotifyEvent _events = new SMSNotifyEvent();
                    _events.Parameters.Add("ChildName", _mdlChild.Name);
                    _events.Parameters.Add("SchoolName", school.Name);
                    _events.AddSMSNotifyEvent((long)NotificationEventConstants.Children.SendSMSToUser, Convert.ToString(UserID));

                }
              
                return ServiceResponse.SuccessReponse(true, MessageEnum.KidsAddedSuccessfully);
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
        public async Task<ResponseObject<bool>> AddStaffInformation()
        {
            try
            {
                SchoolStaff _mdlChild = new SchoolStaff();
                string FileName = string.Empty;
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                //Check
                bool IsExists = db.ExtRepositoryFor<CommonRepository>().ExistsAssociations(UserID);
                if (IsExists == false)
                {
                    NotifyEvent _event = new NotifyEvent();
                   // _event.Parameters.Add("GPNName", school.Name);
                    _event.AddNotifyEvent((long)NotificationEventConstants.Email.SendEmailToUser, Convert.ToString(UserID));
                }
                //
                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                FileName = await FileOpsHelper.UploadFileNew(file, ContainerName.CHILD);
                _mdlChild.FileName = FileName;

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["name"]))
                    _mdlChild.Name = HttpContext.Current.Request.Form["name"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["schoolId"]))
                    _mdlChild.SchoolID = Convert.ToInt32(HttpContext.Current.Request.Form["schoolId"]);
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["department"]))
                    _mdlChild.Department = HttpContext.Current.Request.Form["department"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["designation"]))
                    _mdlChild.Designation = HttpContext.Current.Request.Form["designation"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["employeeID"]))
                    _mdlChild.EmployeeID = Convert.ToString(HttpContext.Current.Request.Form["employeeID"].ToString());
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["gender"]))
                    _mdlChild.Gender = HttpContext.Current.Request.Form["gender"].ToString();

                _mdlChild.UserID = (int)UserID;
                _mdlChild.CreatedBy = (int)UserID;
                _mdlChild.CreatedDate = DateTime.Now;
                _mdlChild.IsVerified = false;
                _mdlChild.IsActive = true;
                db.Repository<SchoolStaff>().Insert(_mdlChild);
                db.Save();
                
                var school = db.Repository<School>().FindById(_mdlChild.SchoolID);
                if (school != null)
                {
                    NotifyEvent _event = new NotifyEvent();
                    _event.Parameters.Add("StaffName", _mdlChild.Name);
                    _event.Parameters.Add("Gender", _mdlChild.Gender);
                    _event.Parameters.Add("Department", _mdlChild.Department);
                    _event.Parameters.Add("SchoolName", school.Name);
                    _event.Parameters.Add("Branch", school.BranchName);
                    _event.AddNotifyEvent((long)NotificationEventConstants.SchoolStaff.SendEmailToAdmin, Convert.ToString(UserID));

                    SMSNotifyEvent _events = new SMSNotifyEvent();
                    _events.Parameters.Add("StaffName", _mdlChild.Name);
                    _events.Parameters.Add("SchoolName", school.Name);
                    _events.AddSMSNotifyEvent((long)NotificationEventConstants.SchoolStaff.SendSMSToUser, Convert.ToString(UserID));

                }
               
                return ServiceResponse.SuccessReponse(true, MessageEnum.KidsAddedSuccessfully);
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
        public async Task<ResponseObject<List<Child>>> GetPendingKidsList(int SchoolId)
        {
            try
            {


                List<Child> lstPendingRequest = db.Repository<Child>().GetAll().Where(n => n.SchoolID == SchoolId && n.IsVerified == false).ToList<Child>();

                if (lstPendingRequest.Count == 0)
                    return ServiceResponse.SuccessReponse(lstPendingRequest, MessageEnum.ChildNotFound);

                return ServiceResponse.SuccessReponse(lstPendingRequest, MessageEnum.ChildFoundSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Child>>(exp);
            }
        }


        [HttpGet]
        public async Task<ResponseObject<List<Child>>> GetChildrenByUserId()
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                List<Child> lstChildren = db.Repository<Child>().GetAll().Where(x => x.UserID == UserID && x.IsActive == true).ToList<Child>();
                if (lstChildren.Count == 0)
                    return ServiceResponse.SuccessReponse(lstChildren, MessageEnum.ChildNotFound);

                return ServiceResponse.SuccessReponse(lstChildren, MessageEnum.ChildFoundSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Child>>(exp);
            }
        }



    }
}
