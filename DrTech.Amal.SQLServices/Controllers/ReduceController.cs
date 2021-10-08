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
    public class ReduceController : BaseController
    {
        [HttpPost]
        public async Task<ResponseObject<bool>> AddReduceItem()
        {
            try
            {
                Reduce mdlReduce = new Reduce();
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                string FileName = string.Empty;
                if (HttpContext.Current.Request.Files.Count != 0)
                {
                    HttpPostedFile file = HttpContext.Current.Request.Files[0];
                    FileName = await FileOpsHelper.UploadFileNew(file, ContainerName.REDUCE);
                }

                mdlReduce.FileName = FileName;
                mdlReduce.Idea = HttpContext.Current.Request.Form["idea"].ToString();
                //mdlReduce.Longitude = Convert.ToInt32(provider.FormData.GetValues("Longitude")[0]);
                mdlReduce.StatusID = (int)StatusEnum.Submit; //Convert.ToInt32(provider.FormData.GetValues("StatusID")[0]);
                                                             //mdlReduce.Latitude = Convert.ToInt32(provider.FormData.GetValues("Latitude")[0]); 
                mdlReduce.Longitude = Convert.ToDecimal(HttpContext.Current.Request.Form["longitude"]);
                mdlReduce.Latitude = Convert.ToDecimal(HttpContext.Current.Request.Form["latitude"]);
                mdlReduce.CreatedBy = (int)UserID;
                mdlReduce.UserID = (int)UserID;
                mdlReduce.CreatedDate = DateTime.Now;
              //  mdlReduce.UpdatedDate = DateTime.Now;
                db.Repository<Reduce>().Insert(mdlReduce);
                db.Save();

                NotifyEvent _event = new NotifyEvent();
                _event.Parameters.Add("FileName", mdlReduce.FileName);
                _event.Parameters.Add("Idea", mdlReduce.Idea);
                _event.Parameters.Add("Longitude", mdlReduce.Longitude);
                _event.Parameters.Add("Latitude", mdlReduce.Latitude);
                _event.AddNotifyEvent((long)NotificationEventConstants.Reduce.ReduceEmailSendtoAdmin, Convert.ToString(UserID));

                return ServiceResponse.SuccessReponse(true, MessageEnum.ReduceAddedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        public ResponseObject<List<Object>> GetAllReduceItem(int UserId = 0)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                var reduce = db.ExtRepositoryFor<ReduceRepository>().GetAllReduceItem(UserID);
                if (reduce.Count == 0)
                    return ServiceResponse.SuccessReponse(reduce, MessageEnum.ReduceItemsNotFound);

                return ServiceResponse.SuccessReponse(reduce, MessageEnum.ReduceItemGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }

        [HttpPost]
        public ResponseObject<object> ChangeStatusForReduce(int rID, string status)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                Reduce amalR = db.Repository<Reduce>().FindById(rID);

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

                db.Repository<Reduce>().Update(amalR);
                db.Save();
                return ServiceResponse.SuccessReponse<object>(true, "Status Changed Successfully");
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }

        [HttpGet]
        public async Task<ResponseObject<List<object>>> GetReducesListByStatus(int StatusID = 0)
        {
            try
            {
                var redusesList = db.ExtRepositoryFor<ReduceRepository>().GetReducesListByStatus(StatusID);

                if (redusesList.Count == 0)
                    return ServiceResponse.SuccessReponse(redusesList, MessageEnum.ReduceItemsNotFound);
                else
                    return ServiceResponse.SuccessReponse(redusesList, MessageEnum.ReduceItemGetSuccess);
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
                var reduceList = db.ExtRepositoryFor<ReduceRepository>().GetAllReducesList();

                if (reduceList.Count == 0)
                    return ServiceResponse.SuccessReponse(reduceList, MessageEnum.RegiftItemsNotFound);
                else
                    return ServiceResponse.SuccessReponse(reduceList, MessageEnum.RegiftItemGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }
        [HttpGet]
        public async Task<ResponseObject<object>> GetReduceById(int Id = 0)
        {
            try
            {
                var refusesList = db.ExtRepositoryFor<ReduceRepository>().GetReduceById(Id);
           
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
        public async Task<ResponseObject<bool>> UpdateStatus([FromBody]Reduce _mdlReduce)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                 if(_mdlReduce.StatusID == (int)StatusEnum.Resolved)
                {

               
                    Reduce mdlReduce = db.Repository<Reduce>().FindById(_mdlReduce.ID);
                    int lastGreenPoints = mdlReduce.GreenPoints;
                    mdlReduce.GreenPoints = _mdlReduce.GreenPoints;
                    mdlReduce.StatusID = _mdlReduce.StatusID;
                    mdlReduce.UpdatedBy = UserID;
                    mdlReduce.UpdatedDate = DateTime.Now;
                    db.Repository<Reduce>().Update(mdlReduce);
                    db.Save();
                    //Herer to update parent Table green points 
                    db.ExtRepositoryFor<CommonRepository>().UpdateParentsTableGreenPoints(mdlReduce.UserID,UserID, mdlReduce.ID,FiveREnum.Reduce.ToString(), lastGreenPoints, _mdlReduce.GreenPoints);
                    PushNotificationEvent _event = new PushNotificationEvent();
                    _event.Parameters.Add("GP", _mdlReduce.GreenPoints);
                    _event.AddPushNotifyEvent((long)NotificationEventConstants.PushNotification.Reduce, mdlReduce.UserID.ToString());
                    return ServiceResponse.SuccessReponse(true, MessageEnum.ReduceUpdatedSuccessfully);
                }
                else if (_mdlReduce.StatusID == (int)StatusEnum.Declined)
                {
                    Reduce mdlReduce = db.Repository<Reduce>().FindById(_mdlReduce.ID);
                    mdlReduce.StatusID = _mdlReduce.StatusID;
                    mdlReduce.UpdatedBy = UserID;
                    mdlReduce.UpdatedDate = DateTime.Now;
                    db.Repository<Reduce>().Update(mdlReduce);
                    db.Save();
                   
                    PushNotificationEvent _event = new PushNotificationEvent();
                    _event.AddPushNotifyEvent((long)NotificationEventConstants.PushNotification.ReduceDeclined, mdlReduce.UserID.ToString());
                    return ServiceResponse.SuccessReponse(true, MessageEnum.ReduceUpdatedSuccessfully);
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
