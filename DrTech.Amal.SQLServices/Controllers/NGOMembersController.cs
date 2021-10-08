using DrTech.Amal.Common.Enums;
using DrTech.Amal.Common.Helpers;
using DrTech.Amal.Common.ServerResponse;
using DrTech.Amal.Notifications;
using DrTech.Amal.SQLModels;
using DrTech.Amal.SQLServices.Auth;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Http;

namespace DrTech.Amal.SQLServices.Controllers
{
    [Authorize]
    public class NGOMembersController : BaseController
    {
        public ResponseObject<bool> AddNOGEmployeeInformation(Member mdlMem)
        {
            if (mdlMem == null)
                return ServiceResponse.ErrorReponse<bool>(MessageEnum.KidsModelNotNull);
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);           

                //Member MemberExist = db.Repository<Member>().GetAll().Where(x => x.NGOId == mdlMem.NGOId && x.UserId == UserID).FirstOrDefault();
                //if (MemberExist != null)
                //    return ServiceResponse.SuccessReponse(true, MessageEnum.NGOEmpAlreadyAdded);

                Member member = new Member
                {
                    OrgId = mdlMem.OrgId,
                    Designation = mdlMem.Designation,
                    Department = mdlMem.Department,
                    EmployeeID = mdlMem.EmployeeID,
                    FromDate = Convert.ToDateTime(mdlMem.FromDate),
                    ToDate = Convert.ToDateTime(mdlMem.ToDate),
                    IsCurrentlyWorking = mdlMem.IsCurrentlyWorking,
                    UserID = (int)UserID,
                    IsVerified = false,
                    CreatedBy = (int)UserID,
                    CreatedDate = DateTime.Now

                };

                db.Repository<Member>().Insert(member);
                db.Save();
                var org = db.Repository<Organization>().GetAll().Where(x => x.ID == mdlMem.OrgId).FirstOrDefault();

                if (org != null)
                {
                    NotifyEvent _event = new NotifyEvent();
                    _event.Parameters.Add("NGOName", org.Name);
                    _event.Parameters.Add("EmployeeID", member.EmployeeID);
                    _event.AddNotifyEvent((long)NotificationEventConstants.NGO.SendEmailToAdmin, Convert.ToString(UserID));

                    SMSNotifyEvent _events = new SMSNotifyEvent();
                    _events.Parameters.Add("NGOName", org.Name);
                    _events.AddSMSNotifyEvent((long)NotificationEventConstants.NGO.SendSMSToUser, Convert.ToString(UserID));

                }

                return ServiceResponse.SuccessReponse(true, MessageEnum.EmpAddedSuccessfully);

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
    }
}
