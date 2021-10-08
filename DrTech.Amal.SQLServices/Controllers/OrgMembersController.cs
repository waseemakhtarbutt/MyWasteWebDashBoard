using DrTech.Amal.Common.Enums;
using DrTech.Amal.Common.Helpers;
using DrTech.Amal.Common.ServerResponse;
using DrTech.Amal.Notifications;
using DrTech.Amal.SQLDataAccess.Repository;
using DrTech.Amal.SQLModels;
using DrTech.Amal.SQLServices.Auth;
using System;
using System.Data.Entity.Validation;
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
    public class OrgMembersController : BaseController
    {
       

        [HttpPost]
        public async Task<ResponseObject<bool>> AddOrgMemberInformation()
        {
            try
            {
                Member mdlMember = new Member();
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
                   // _event.Parameters.Add("GPNName", org.Name);
                    _event.AddNotifyEvent((long)NotificationEventConstants.Email.SendEmailToUser, Convert.ToString(UserID));
                }
                string FileName = string.Empty;
                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                FileName = await FileOpsHelper.UploadFileNew(file, ContainerName.ORGANIZATION);

                mdlMember.FileName = FileName;

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["name"]))
                    mdlMember.Name = HttpContext.Current.Request.Form["name"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["orgId"]))
                    mdlMember.OrgId = Convert.ToInt32( HttpContext.Current.Request.Form["orgId"]);
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["employeeID"]))
                    mdlMember.EmployeeID = Convert.ToString( HttpContext.Current.Request.Form["employeeID"]);
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["designation"]))
                    mdlMember.Designation = HttpContext.Current.Request.Form["designation"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["fromDate"]))
                {
                    if (HttpContext.Current.Request.Form["fromDate"].ToString() != "null")
                    {
                        mdlMember.FromDate = Utility.GetParsedDate(HttpContext.Current.Request.Form["fromDate"].ToString());
                    }
                    else
                    {
                        mdlMember.FromDate = null;
                    }
                }

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["toDate"]))

                {
                    if (HttpContext.Current.Request.Form["toDate"].ToString() != "null")
                    {
                        mdlMember.FromDate = Utility.GetParsedDate(HttpContext.Current.Request.Form["toDate"].ToString());
                    }
                    else
                    {
                        mdlMember.FromDate = null;
                    }
                }
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["gender"]))
                    mdlMember.Gender = HttpContext.Current.Request.Form["gender"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["isCurrentlyWorking"]))
                    mdlMember.IsCurrentlyWorking =  Convert.ToBoolean(HttpContext.Current.Request.Form["isCurrentlyWorking"]);
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["department"]))
                    mdlMember.Department = HttpContext.Current.Request.Form["department"];
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["location"]))
                    mdlMember.Location = HttpContext.Current.Request.Form["location"];


                mdlMember.IsVerified = false;
               // mdlMember.CreatedBy = (int)UserID;
                mdlMember.CreatedDate = DateTime.Now;
                mdlMember.UserID = (int)UserID;
                mdlMember.IsActive = true;

                db.Repository<Member>().Insert(mdlMember);
                db.Save();
                var org = db.Repository<Organization>().GetAll().Where(x => x.ID == mdlMember.OrgId).FirstOrDefault();

                if (org != null)
                {
                    NotifyEvent _event = new NotifyEvent();
                    _event.Parameters.Add("NGOName", org.Name);
                    _event.Parameters.Add("EmployeeID", mdlMember.EmployeeID);
                    _event.AddNotifyEvent((long)NotificationEventConstants.NGO.SendEmailToAdmin, Convert.ToString(UserID));

                    SMSNotifyEvent _events = new SMSNotifyEvent();
                    _events.Parameters.Add("NGOName", org.Name);
                    _events.AddSMSNotifyEvent((long)NotificationEventConstants.NGO.SendSMSToUser, Convert.ToString(UserID));

                }
              

                return ServiceResponse.SuccessReponse(true, MessageEnum.OrgAddedSuccessfully);
            }

            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
    }
}
