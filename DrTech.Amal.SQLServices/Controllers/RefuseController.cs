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
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using static DrTech.Amal.Common.Extentions.Constants;

namespace DrTech.Amal.SQLServices.Controllers
{
    [Authorize]
    public class RefuseController : BaseController
    {
        [HttpPost]
        public async Task<ResponseObject<bool>> AddRefuseItem()
        {
            try
            {
                Refuse mdlRefuse = new Refuse();
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                string FileName = string.Empty;
                if (HttpContext.Current.Request.Files.Count != 0)
                {
                    HttpPostedFile file = HttpContext.Current.Request.Files[0];
                    FileName = await FileOpsHelper.UploadFileNew(file, ContainerName.REFUSE);
                }
                mdlRefuse.FileName = FileName;
                mdlRefuse.Idea = HttpContext.Current.Request.Form["idea"].ToString();
                mdlRefuse.Longitude = Convert.ToDecimal(HttpContext.Current.Request.Form["longitude"]);
                mdlRefuse.Latitude = Convert.ToDecimal(HttpContext.Current.Request.Form["latitude"]);
                mdlRefuse.StatusID = (int)StatusEnum.Submit; // Convert.ToInt32(provider.FormData.GetValues("StatusID")[0]);
                mdlRefuse.CreatedBy = (int)UserID;
                mdlRefuse.UserID = (int)UserID;
                mdlRefuse.CreatedDate = DateTime.Now;
               

                db.Repository<Refuse>().Insert(mdlRefuse);
                db.Save();

                NotifyEvent _event = new NotifyEvent();
                _event.Parameters.Add("FileName", mdlRefuse.FileName);
                _event.Parameters.Add("Idea", mdlRefuse.Idea);
                _event.Parameters.Add("Longitude", mdlRefuse.Longitude);
                _event.Parameters.Add("Latitude", mdlRefuse.Latitude);
                _event.AddNotifyEvent((long)NotificationEventConstants.Refuse.RefuseEmailSendtoAdmin, Convert.ToString(UserID));

                return ServiceResponse.SuccessReponse(true, MessageEnum.RefuseAddedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<List<object>> GetAllRefuseItem(int UserId = 0)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                var user = db.ExtRepositoryFor<RefuseRepository>().GetAllRefuseItemById(UserID);

                if (user.Count == 0)
                    return ServiceResponse.SuccessReponse(user, MessageEnum.RefuseItemsNotFound);
                else
                    return ServiceResponse.SuccessReponse(user, MessageEnum.RefuseItemGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }

        [HttpPost]
        public ResponseObject<object> ChangeStatusForRefuse(int rID, string status)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                Refuse amalR = db.Repository<Refuse>().FindById(rID);

                if (status == "confirm")
                {
                    amalR.StatusID = (int)StatusEnum.Complete;
                }
                else if (status == "reject")
                {
                    amalR.StatusID = (int)StatusEnum.Declined;
                }
                else
                {
                    return ServiceResponse.ErrorReponse<object>("Query Parameter not correct");
                }

                db.Repository<Refuse>().Update(amalR);
                db.Save();
                return ServiceResponse.SuccessReponse<object>(true, "Status Changed Successfully");
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }

        // [Authorize(Roles = "Admin")]
        [HttpGet]
        [AllowAnonymous]
        public ResponseObject<List<object>> GetAll(string id = null)
        {
            try
            {
                var lstRefuse = new List<object>();
                if (string.IsNullOrEmpty(id))
                {
                    lstRefuse = db.ExtRepositoryFor<RefuseRepository>().GetAllRefuseItem();
                }
                else
                {
                    var refuseItem = db.ExtRepositoryFor<RefuseRepository>().GetAllRefuseItemById(1);
                    if (refuseItem != null)
                        lstRefuse.Add(refuseItem);
                }

                return ServiceResponse.SuccessReponse(lstRefuse, MessageEnum.ComplaintGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }



        //[Authorize(Roles = "Admin")]
        //[HttpPost]
        //public ResponseObject<bool> UpdateStatus([FromBody]Refuse mdlRefuse)
        //{
        //    if (mdlRefuse == null)
        //        return ServiceResponse.ErrorReponse<bool>(MessageEnum.DefaultParametersCanNotBeNull);

        //    try
        //    {
        //        db.Repository<Refuse>().Update(mdlRefuse);
        //        db.Save();
        //        return ServiceResponse.SuccessReponse(true, MessageEnum.RefuseItemUpdatedSuccess);
        //    }
        //    catch (Exception exp)
        //    {
        //        return ServiceResponse.ErrorReponse<bool>(exp);
        //    }
        //}
        [HttpPost]
        public async Task<ResponseObject<List<object>>> GetRefusesListByStatus(RecycleRequest model)
        {
            try
            {
                var refusesList = db.ExtRepositoryFor<RefuseRepository>().GetRefusesListByStatus(model);

                if (refusesList.Count == 0)
                    return ServiceResponse.SuccessReponse(refusesList, MessageEnum.RefuseItemsNotFound);
                else
                    return ServiceResponse.SuccessReponse(refusesList, MessageEnum.RefuseItemGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }
        [HttpGet]
        public async Task<ResponseObject<object>> GetRefusesById(int Id = 0)
        {
            try
            {
                var refusesList = db.ExtRepositoryFor<RefuseRepository>().GetRefuseById(Id);

                if (refusesList != null)
                    return ServiceResponse.SuccessReponse(refusesList, MessageEnum.RefuseItemsNotFound);
                else
                    return ServiceResponse.SuccessReponse(refusesList, MessageEnum.RefuseItemGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }
        [HttpPost]
        public async Task<ResponseObject<bool>> UpdateStatus([FromBody]Refuse _mdlRefuse)
        {
            try
            {

                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                if (_mdlRefuse.StatusID == (int)StatusEnum.Resolved)
                {
                    Refuse mdlRefuse = db.Repository<Refuse>().FindById(_mdlRefuse.ID);
                    int lastGreenPoints = mdlRefuse.GreenPoints;
                    mdlRefuse.GreenPoints = _mdlRefuse.GreenPoints;
                    mdlRefuse.StatusID = _mdlRefuse.StatusID;

                    mdlRefuse.UpdatedBy = UserID;
                    mdlRefuse.UpdatedDate = DateTime.Now;

                    db.Repository<Refuse>().Update(mdlRefuse);
                    db.Save();
                    //Herer to update parent Table green points 

                    db.ExtRepositoryFor<CommonRepository>().UpdateParentsTableGreenPoints(mdlRefuse.UserID, UserID, mdlRefuse.ID, FiveREnum.Refuse.ToString(), lastGreenPoints, _mdlRefuse.GreenPoints);


                    PushNotificationEvent _event = new PushNotificationEvent();
                    _event.Parameters.Add("GP", _mdlRefuse.GreenPoints);
                    _event.AddPushNotifyEvent((long)NotificationEventConstants.PushNotification.Refuse, _mdlRefuse.UserID.ToString());
                    return ServiceResponse.SuccessReponse(true, MessageEnum.RefuseUpdatedSuccessfully);
                }
                else if (_mdlRefuse.StatusID == (int)StatusEnum.Declined)
                {
                    Refuse mdlRefuse = db.Repository<Refuse>().FindById(_mdlRefuse.ID);
                    mdlRefuse.StatusID = _mdlRefuse.StatusID;
                    mdlRefuse.UpdatedBy = UserID;
                    mdlRefuse.UpdatedDate = DateTime.Now;
                    db.Repository<Refuse>().Update(mdlRefuse);
                    db.Save();
                    PushNotificationEvent _event = new PushNotificationEvent();
                    _event.AddPushNotifyEvent((long)NotificationEventConstants.PushNotification.RefuseDeclined, _mdlRefuse.UserID.ToString());
                    return ServiceResponse.SuccessReponse(true, MessageEnum.RefuseUpdatedSuccessfully);

                }
                
               return ServiceResponse.SuccessReponse(false, MessageEnum.DefaultParametersCanNotBeNull);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

    }

    
}
