using DrTech.Amal.Common.Enums;
using DrTech.Amal.Common.Helpers;
using DrTech.Amal.Common.ServerResponse;
using DrTech.Amal.Notifications;
using DrTech.Amal.SQLDataAccess.Repository;
using DrTech.Amal.SQLModels;
using DrTech.Amal.SQLServices.Auth;
using System;
using System.Collections.Generic;
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
    public class ReuseController : BaseController
    {
        public async System.Threading.Tasks.Task<ResponseObject<bool>> AddReuseItems()
        {         
            try
            {
                Reuse mdlReuse = new Reuse();
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                string FileName = string.Empty;
                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                FileName = await FileOpsHelper.UploadFileNew(file, ContainerName.REUSE);

                mdlReuse.FileName = FileName;
                mdlReuse.Idea = HttpContext.Current.Request.Form["idea"];

                mdlReuse.Longitude = Convert.ToDecimal(HttpContext.Current.Request.Form["longitude"]);
                mdlReuse.Latitude = Convert.ToDecimal(HttpContext.Current.Request.Form["latitude"]);

                mdlReuse.CreatedBy = (int)UserID;
                mdlReuse.UserID = (int)UserID;
                mdlReuse.CreatedDate = DateTime.Now;
               // mdlReuse.UpdatedDate = DateTime.Now;
                mdlReuse.StatusID = (int)StatusEnum.Submit;
                mdlReuse.GreenPoints = 0;

                db.Repository<Reuse>().Insert(mdlReuse);
                db.Save();
                NotifyEvent _event = new NotifyEvent();
                _event.Parameters.Add("FileName", mdlReuse.FileName);
                _event.Parameters.Add("Idea", mdlReuse.Idea);
                _event.Parameters.Add("Longitude", mdlReuse.Longitude);
                _event.Parameters.Add("Latitude", mdlReuse.Latitude);
                _event.AddNotifyEvent((long)NotificationEventConstants.Reuse.ReuseEmailSendtoAdmin, Convert.ToString(UserID));

                return ServiceResponse.SuccessReponse(true, MessageEnum.ReuseItemSuccssfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        public ResponseObject<List<object>> GetReuseItemsByUserID(int UserId = 0)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                // var lstReuse = db.Repository<Reuse>().GetAll().Where(d => d.UserID == UserID).OrderByDescending(x => x.CreatedDate).ToList();
                var reusesList = db.ExtRepositoryFor<ReuseRepository>().GetAllReuseItemById(UserID);
                if (reusesList.Count == 0)
                    return ServiceResponse.SuccessReponse(reusesList, MessageEnum.DefaultSuccessMessage);
                
                return ServiceResponse.SuccessReponse(reusesList, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }

        [HttpGet]
        public async Task<ResponseObject<List<object>>> GetReusesListByStatus(int StatusID = 0)
        {
            try
            {
                var reusesList = db.ExtRepositoryFor<ReuseRepository>().GetReusesListByStatus(StatusID);

                if (reusesList.Count == 0)
                    return ServiceResponse.SuccessReponse(reusesList, MessageEnum.RegiftItemsNotFound);
                else
                    return ServiceResponse.SuccessReponse(reusesList, MessageEnum.RegiftItemGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }

        [HttpGet]
        public async Task<ResponseObject<List<object>>> GetAll()
        {
            try
            {
                var reusesList = db.ExtRepositoryFor<ReuseRepository>().GetAllReusesList();

                if (reusesList.Count == 0)
                    return ServiceResponse.SuccessReponse(reusesList, MessageEnum.RegiftItemsNotFound);
                else
                    return ServiceResponse.SuccessReponse(reusesList, MessageEnum.RegiftItemGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }
        [HttpGet]
        public async Task<ResponseObject<object>> GetReusesById(int Id = 0)
        {
            try
            {
                var refusesList = db.ExtRepositoryFor<ReuseRepository>().GetReuseById(Id);

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
        public async Task<ResponseObject<bool>> UpdateStatus([FromBody]Reuse _mdlReuse)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                if (_mdlReuse.StatusID == (int)StatusEnum.Resolved)
                {
                    Reuse mdlReuse = db.Repository<Reuse>().FindById(_mdlReuse.ID);
                    int lastGreenPoints = Convert.ToInt32( mdlReuse.GreenPoints);
                    mdlReuse.GreenPoints = _mdlReuse.GreenPoints;
                    mdlReuse.StatusID = _mdlReuse.StatusID;

                    mdlReuse.UpdatedBy = UserID;
                    mdlReuse.UpdatedDate = DateTime.Now;

                    db.Repository<Reuse>().Update(mdlReuse);
                    db.Save();
                    //Herer to update parent Table green points 
                    db.ExtRepositoryFor<CommonRepository>().UpdateParentsTableGreenPoints(mdlReuse.UserID,UserID, mdlReuse.ID,FiveREnum.Reuse.ToString(), lastGreenPoints,Convert.ToInt32( _mdlReuse.GreenPoints));
                    PushNotificationEvent _event = new PushNotificationEvent();
                    _event.Parameters.Add("GP", _mdlReuse.GreenPoints);
                    _event.AddPushNotifyEvent((long)NotificationEventConstants.PushNotification.Reuse, mdlReuse.UserID.ToString());

                    return ServiceResponse.SuccessReponse(true, MessageEnum.ReuseUpdatedSuccessfully);
                }
                else if (_mdlReuse.StatusID == (int)StatusEnum.Declined)
                {
                    Reuse mdlReuse = db.Repository<Reuse>().FindById(_mdlReuse.ID);
                    mdlReuse.StatusID = _mdlReuse.StatusID;

                    mdlReuse.UpdatedBy = UserID;
                    mdlReuse.UpdatedDate = DateTime.Now;

                    db.Repository<Reuse>().Update(mdlReuse);
                    db.Save();
                    PushNotificationEvent _event = new PushNotificationEvent();
                    _event.AddPushNotifyEvent((long)NotificationEventConstants.PushNotification.ReuseDeclined, mdlReuse.UserID.ToString());

                    return ServiceResponse.SuccessReponse(true, MessageEnum.ReuseUpdatedSuccessfully);
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
