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
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using static DrTech.Amal.Common.Extentions.Constants;

namespace DrTech.Amal.SQLServices.Controllers
{
    [Authorize]
    public class RecycleController : BaseController
    {
        public async Task<ResponseObject<bool>> AddRecycleItem()
        {
            try
            {
                OrderTracking OT = new OrderTracking();
                Recycle mdlRecycle = new Recycle();
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                string FileName = string.Empty;
                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                FileName = await FileOpsHelper.UploadFileNew(file, ContainerName.RECYCLE);
                mdlRecycle.FileName = FileName;
                mdlRecycle.CollectorDateTime = Utility.GetParsedDate(HttpContext.Current.Request.Form["collectorDateTime"].ToString());
                //var Date = HttpContext.Current.Request.Form["collectorDateTime"].ToString();
                //mdlRecycle.CollectorDateTime = Convert.ToDateTime(Date);
              //  mdlRecycle.CollectorDateTime = Convert.ToDateTime(HttpContext.Current.Request.Form["collectorDateTime"].ToString());
                //mdlRecycle.CollectorDateTime = Utility.GetLocalDateTimeFromUTC(dateTime); 
                mdlRecycle.StatusID = (int)StatusEnum.Submit;
                mdlRecycle.GreenPoints  = 0;              
                mdlRecycle.CreatedBy = (int)UserID;
                mdlRecycle.UserID = (int)UserID;
                mdlRecycle.CreatedDate = DateTime.Now;

                int RecycleID = mdlRecycle.ID;
                string Description = HttpContext.Current.Request.Form["description"].ToString();
                int Weight = 0;
                RecycleSubItem SubItems = new RecycleSubItem();
                SubItems.Description = Description;
                SubItems.Weight = Weight;
                //SubItems.RecycleID = RecycleID;
                SubItems.IsParent = true;
                mdlRecycle.RecycleSubItems.Add(SubItems);
                db.Repository<Recycle>().Insert(mdlRecycle);
                db.Save();
                OT.RsID = mdlRecycle.ID;
                OT.Type = "Recycle";
                OT.StatusID = (int)StatusEnum.New;
                OT.FileNameTakenByUser = FileName;
                OT.IsActive = true;
                db.Repository<OrderTracking>().Insert(OT);
                db.Save();

                NotifyEvent _event = new NotifyEvent();
                _event.Parameters.Add("UserID", Convert.ToString(UserID));
               // _event.Parameters.Add("DateTime", mdlRecycle.CollectorDateTime);
                _event.AddNotifyEvent((long)NotificationEventConstants.Recycle.EmailSendToAdminRecycleInfo, FileName);

                SMSNotifyEvent _events = new SMSNotifyEvent();
                _events.Parameters.Add("Description", Description);
                _events.AddSMSNotifyEvent((long)NotificationEventConstants.Recycle.SMSSentoUser, Convert.ToString(UserID));
               // _events.AddSMSNotifyEvent((long)NotificationEventConstants.Recycle.SMSSentoUser, Convert.ToString(UserID));

                return ServiceResponse.SuccessReponse(true, MessageEnum.RecycleItemsAdded);

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
            catch (Exception exp) { return ServiceResponse.ErrorReponse<bool>(exp); }
        }

        public ResponseObject<List<object>> GetRecycleItem()
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                List<object> LstRecycleItems = db.ExtRepositoryFor<RecycleRepository>().GetRecycleItem(7);

                if (LstRecycleItems?.Count == 0)
                    return ServiceResponse.SuccessReponse(LstRecycleItems, MessageEnum.RecycleItemsNotFound);
                
                return ServiceResponse.SuccessReponse(LstRecycleItems, MessageEnum.RecycleItemsGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(MessageEnum.RecycleItemsNotFound);
            }

        }

        [HttpGet]
        public ResponseObject<List<object>> TESTGetRecycleItem()
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                List<object> LstRecycleItems = db.ExtRepositoryFor<RecycleRepository>().TESTGetRecycleItem(UserID);

                if (LstRecycleItems?.Count == 0)
                    return ServiceResponse.SuccessReponse(LstRecycleItems, MessageEnum.RecycleItemsNotFound);

                return ServiceResponse.SuccessReponse(LstRecycleItems, MessageEnum.RecycleItemsGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }

        }

        [HttpPost]
        public ResponseObject<object> ChangeStatusForRecycle(int rID, string status)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                OrderTracking mdlOrder = db.Repository<OrderTracking>().GetAll().Where(o => o.RsID == rID && o.Type == "Recycle").FirstOrDefault();
                Recycle amalR = db.Repository<Recycle>().FindById(rID);

                if (status == "confirm")
                {
                    amalR.StatusID = (int)StatusEnum.Complete;
                    mdlOrder.StatusID = (int)StatusEnum.Complete;
                    mdlOrder.CollectedPendingConfirmation = true;
                }
                else if (status == "decline")
                {
                    amalR.StatusID = (int)StatusEnum.Declined;

                    mdlOrder.StatusID = (int)StatusEnum.Declined;
                    mdlOrder.CollectedPendingConfirmation = null;

                }
                else
                {
                    return ServiceResponse.ErrorReponse<object>("Query Parameter not correct");
                }

                db.Repository<Recycle>().Update(amalR);
                db.Save();
                return ServiceResponse.SuccessReponse<object>(true, "Status Changed Successfully");
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }

        [HttpPost]
        public async Task<ResponseObject<List<RecycleDto>>> GetRecyclesListByStatus(RecycleRequest model)
        {
            try
            {
                var recyclesList = db.ExtRepositoryFor<RecycleRepository>().GetRecyclesListByStatus(model);

                if (recyclesList.Count == 0)
                    return ServiceResponse.SuccessReponse(recyclesList, MessageEnum.RecycleItemsNotFound);
                else
                    return ServiceResponse.SuccessReponse(recyclesList, MessageEnum.RecycleItemsGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<RecycleDto>>(exp);
            }
        }
        [HttpPost]
        public async Task<ResponseObject<List<object>>> GetRecyclesListByStatusExcel(RecycleRequest model)
        {
            try
            {
                var recyclesList = db.ExtRepositoryFor<RecycleRepository>().GetRecyclesListByStatusExcel(model);

                if (recyclesList.Count == 0)
                    return ServiceResponse.SuccessReponse(recyclesList, MessageEnum.RecycleItemsNotFound);
                else
                    return ServiceResponse.SuccessReponse(recyclesList, MessageEnum.RecycleItemsGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }
        [HttpPost]
        public async Task<ResponseObject<List<object>>> GetRecyclesAllListByStatusExcel(RecycleRequest model)
        {
            try
            {
                var recyclesList = db.ExtRepositoryFor<RecycleRepository>().GetRecyclesAllListByStatusExcel(model);

                if (recyclesList.Count == 0)
                    return ServiceResponse.SuccessReponse(recyclesList, MessageEnum.RecycleItemsNotFound);
                else
                    return ServiceResponse.SuccessReponse(recyclesList, MessageEnum.RecycleItemsGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }

        [HttpPost]
        public async Task<ResponseObject<List<object>>> GetRecyclesAllListByStatus(RecycleRequest model)
        {
            try
            {
                var recyclesList = db.ExtRepositoryFor<RecycleRepository>().GetRecyclesAllListByStatus(model);

                if (recyclesList.Count == 0)
                    return ServiceResponse.SuccessReponse(recyclesList, MessageEnum.RecycleItemsNotFound);
                else
                    return ServiceResponse.SuccessReponse(recyclesList, MessageEnum.RecycleItemsGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }
        [HttpPost]
        public async Task<ResponseObject<bool>> UpdateStatus([FromBody]Recycle _mdlRecycle)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                //if (_mdlRecycle.StatusID > 0)
                //{
                //    Recycle mdlRecycle = db.Repository<Recycle>().FindById(_mdlRecycle.ID);

                //    mdlRecycle.Weight = _mdlRecycle.Weight;
                //    mdlRecycle.StatusID = _mdlRecycle.StatusID;

                //    mdlRecycle.UpdatedBy = UserID;
                //    mdlRecycle.UpdatedDate = DateTime.Now;

                //    db.Repository<Recycle>().Update(mdlRecycle);
                //    db.Save();

                //    return ServiceResponse.SuccessReponse(true, MessageEnum.RecycleUpdatedSuccessfully);
                //}

                return ServiceResponse.SuccessReponse(false, MessageEnum.DefaultParametersCanNotBeNull);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
        [HttpGet]
        public async Task<ResponseObject<RecycleViewModel>> GetRecycleDetailById(int RecycleId, bool IsWebAdmin = false)
        {
            try
            {
                var recycle = db.ExtRepositoryFor<RecycleRepository>().GetRecycleDetailById(RecycleId, IsWebAdmin);

                if (recycle == null)
                    return ServiceResponse.SuccessReponse(recycle, MessageEnum.RecordNotFound);
                else
                {
                    RefTable refTable = db.Repository<RefTable>().GetAll().FirstOrDefault();

                    if (refTable != null)
                        recycle.GPV = refTable.GreenPointValue;
                    else
                        recycle.GPV = 5;

                    return ServiceResponse.SuccessReponse(recycle, MessageEnum.RecordFoundSuccessfully);
                }
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<RecycleViewModel>(exp);
            }
        }
        [HttpPost]
        public async Task<ResponseObject<bool>> AssignRecycleToDriver([FromBody]RecycleViewModel _mdlRecycleVM)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                db.ExtRepositoryFor<RecycleRepository>().AssignRecycleToDriver(_mdlRecycleVM, UserID);

                SMSNotifyEvent _events = new SMSNotifyEvent();
                _events.Parameters.Add("Comments", _mdlRecycleVM.Comments ?? string.Empty);
                _events.AddSMSNotifyEvent((long)NotificationEventConstants.Recycle.Updated, Convert.ToString(UserID));

                PushNotificationEvent _event = new PushNotificationEvent();
                _event.Parameters.Add("GP", _mdlRecycleVM.TotalGP);
                _event.AddPushNotifyEvent((long)NotificationEventConstants.PushNotification.Recycle, Convert.ToString(_mdlRecycleVM.UserID));


                return ServiceResponse.SuccessReponse(true, MessageEnum.RecycleUpdatedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
        [HttpPost]
        public async Task<ResponseObject<bool>> RejectRecycle([FromBody]RecycleViewModel _mdlRecycleVM)
        {
            try
            {
                int? userID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                db.ExtRepositoryFor<RecycleRepository>().RejectRecycle(_mdlRecycleVM, userID);

                return ServiceResponse.SuccessReponse(true, MessageEnum.RecycleUpdatedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpPost]
        public ResponseObject<bool> SMSRecycleComments([FromBody]CommentsViewModel _mdlCommentsVM)
        {
            try
            {
                int? userID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                //send sms to user for to notification.
                SMSNotifyEvent _events = new SMSNotifyEvent();

                _events.SendSMS(_mdlCommentsVM.Phone, _mdlCommentsVM.Comments);

                // Recycle Comments

                if (!string.IsNullOrEmpty(_mdlCommentsVM.Comments))
                {
                    RecycleComment recycleComments = new RecycleComment()
                    {
                        Comments = _mdlCommentsVM.Comments,
                        CreatedBy = Convert.ToInt32(userID),
                        CreatedDate = DateTime.Now,
                        UpdatedBy = Convert.ToInt32(userID),
                        UpdatedDate = DateTime.Now,
                        IsActive = true,
                        RecycleID = _mdlCommentsVM.RID
                    };

                    db.Repository<RecycleComment>().Insert(recycleComments);
                }

                db.Save();

                return ServiceResponse.SuccessReponse(true, MessageEnum.SuspendSuccessfully);
            }
            catch (Exception exp)
            {

                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
    }
}
