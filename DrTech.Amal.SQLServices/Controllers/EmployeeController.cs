using DrTech.Amal.Common.Enums;
using DrTech.Amal.Common.Helpers;
using DrTech.Amal.Common.ServerResponse;
using DrTech.Amal.Notifications;
using DrTech.Amal.SQLDataAccess.Repository;
using DrTech.Amal.SQLModels;
using DrTech.Amal.SQLServices.Auth;
using System;
using System.Collections.Generic;
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
    public class EmployeeController : BaseController
    {
      
       // [AllowAnonymous]
        [HttpPost]
        public async Task<ResponseObject<bool>> AddEmployeeInformation()
        {
            try
            {
                Employment mdlEmployment = new Employment();
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                //6261
                int? UserID =JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                bool IsExists = db.ExtRepositoryFor<CommonRepository>().ExistsAssociations(UserID);
                if (IsExists == false)
                {
                    NotifyEvent _event = new NotifyEvent();
                   // _event.Parameters.Add("GPNName", business.Name);
                    _event.AddNotifyEvent((long)NotificationEventConstants.Email.SendEmailToUser, Convert.ToString(UserID));
                }
                string FileName = string.Empty;
                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                FileName = await FileOpsHelper.UploadFileNew(file, ContainerName.Business);

                mdlEmployment.FileName = FileName;

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["busId"]))
                    mdlEmployment.BusId = Convert.ToInt32(HttpContext.Current.Request.Form["busId"]);
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["name"]))
                    mdlEmployment.Name = HttpContext.Current.Request.Form["name"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["designation"]))
                    mdlEmployment.Designation = HttpContext.Current.Request.Form["designation"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["department"]))
                    mdlEmployment.Department = HttpContext.Current.Request.Form["department"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["employeeID"]))
                    mdlEmployment.EmployeeID =  HttpContext.Current.Request.Form["employeeID"];
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["fromDate"]))
                {
                    if (HttpContext.Current.Request.Form["fromDate"].ToString() != "null")
                    {
                        mdlEmployment.FromDate = Utility.GetParsedDate(HttpContext.Current.Request.Form["fromDate"].ToString());
                    }
                    else
                    {
                        mdlEmployment.FromDate = null;
                    }
                }

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["toDate"]))

                {
                    if (HttpContext.Current.Request.Form["toDate"].ToString() != "null")
                    {
                        mdlEmployment.FromDate = Utility.GetParsedDate(HttpContext.Current.Request.Form["toDate"].ToString());
                    }
                    else
                    {
                        mdlEmployment.FromDate = null;
                    }
                }
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["gender"]))
                    mdlEmployment.Gender = HttpContext.Current.Request.Form["gender"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["isCurrentlyWorking"]))
                    mdlEmployment.IsCurrentlyWorking = Convert.ToBoolean(HttpContext.Current.Request.Form["isCurrentlyWorking"]);


                mdlEmployment.IsVerified = false;
                mdlEmployment.IsActive = true;
                mdlEmployment.CreatedBy = (int)UserID;
                mdlEmployment.CreatedDate = DateTime.Now;
                mdlEmployment.UserID = (int) UserID;

                db.Repository<Employment>().Insert(mdlEmployment);
                db.Save();
                var business = db.Repository<Business>().GetAll().Where(x => x.ID == mdlEmployment.BusId).FirstOrDefault();

                if (business != null)
                {
                    NotifyEvent _event = new NotifyEvent();
                    _event.Parameters.Add("OrgName", business.Name);
                    _event.Parameters.Add("EmployeeID", mdlEmployment.EmployeeID);
                    _event.AddNotifyEvent((long)NotificationEventConstants.Organization.SendEmailToAdmin, Convert.ToString(UserID));

                    SMSNotifyEvent _events = new SMSNotifyEvent();
                    _events.Parameters.Add("OrgName", business.Name);
                    _events.AddSMSNotifyEvent((long)NotificationEventConstants.Organization.SendSMSToUser, Convert.ToString(UserID));

                }
                

                return ServiceResponse.SuccessReponse(true, MessageEnum.EmpAddedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
    }
}
