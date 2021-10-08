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
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using static DrTech.Amal.Common.Extentions.Constants;

namespace DrTech.Amal.SQLServices.Controllers
{
    [Authorize]
    public class ReplantController : BaseController
    {

        public async System.Threading.Tasks.Task<ResponseObject<bool>> AddPlantTree()
        {
            try
            {
                Replant mdlReplant = new Replant();
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                string FileName = string.Empty;
                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                mdlReplant.FileName = await FileOpsHelper.UploadFileNew(file, ContainerName.REPLANT);

                mdlReplant.TreeCount = Convert.ToInt32(HttpContext.Current.Request.Form["treeCount"]);
                mdlReplant.Longitude = Convert.ToDecimal(HttpContext.Current.Request.Form["longitude"]);
                mdlReplant.Height = Convert.ToInt32(HttpContext.Current.Request.Form["height"]);
                mdlReplant.Latitude = Convert.ToDecimal(HttpContext.Current.Request.Form["latitude"]);
                mdlReplant.Description = HttpContext.Current.Request.Form["description"];
                mdlReplant.PlantID = Convert.ToInt32(HttpContext.Current.Request.Form["plantID"]);


                mdlReplant.StatusID = (int)StatusEnum.Submit;
                mdlReplant.CreatedBy = (int)UserID;
                mdlReplant.UserID = (int)UserID;
                mdlReplant.CreatedDate = DateTime.Now;
               // mdlReplant.UpdatedDate = DateTime.Now;
                mdlReplant.GreenPoints = 0;


                db.Repository<Replant>().Insert(mdlReplant);
                db.Save();


                //double GreenPoint = GreenPointHelper.GetGreenPointsAgainstRePlant(Convert.ToDouble(mdlReplant.Height), mdlReplant.TreeCount);
                //mdlReplant.GreenPoints = Convert.ToInt32(GreenPoint);


                //User User = new User();
                //User = db.ExtRepositoryFor<UsersRepository>().GetUserDetailsByID(mdlReplant.UserID);

                //GreenPoint = (double)User.GreenPoints + GreenPoint;
                //db.Repository<User>().Update(User);
                //db.Save();

                LookupType mdlLookup = db.ExtRepositoryFor<ReplantRepository>().GetTypeNameWithPlantID(mdlReplant.PlantID);


                NotifyEvent _event = new NotifyEvent();
                _event.Parameters.Add("FileName", mdlReplant.FileName);
                _event.Parameters.Add("PlantName", mdlLookup.Name);
                _event.Parameters.Add("TreeCount", mdlReplant.TreeCount);
                _event.Parameters.Add("Height", mdlReplant.Height);
                _event.Parameters.Add("Longitude", mdlReplant.Longitude);
                _event.Parameters.Add("Latitude", mdlReplant.Latitude);
                _event.AddNotifyEvent((long)NotificationEventConstants.RePlant.EmailSendToAdminForReplantInfo, mdlReplant.UserID.ToString());

                SMSNotifyEvent _events = new SMSNotifyEvent();
                _events.Parameters.Add("PlantName", mdlLookup.Name);
                _events.AddSMSNotifyEvent((long)NotificationEventConstants.RePlant.SMSSendToUser, Convert.ToString(UserID));

                //NotifyEvent _events = new NotifyEvent();
                //_events.Parameters.Add("UserId", GetLoggedInUserId());
                //_events.AddNotifyEvent((long)NotificationEventConstants.RePlant.EmailSentToUserForGreenPoints, GetLoggedInUserId());

                return ServiceResponse.SuccessReponse(true, MessageEnum.ReplantAddedSuccessfully);
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

        public ResponseObject<List<object>> GetPlantedTrees()
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                var replantList = db.ExtRepositoryFor<ReplantRepository>().GetAllReplantById(UserID);

                if (replantList.Count > 0 && replantList != null)
                {
                    return ServiceResponse.SuccessReponse(replantList, MessageEnum.ReplantRecordFoundSuccessfully);
                }
                else
                {
                    return ServiceResponse.SuccessReponse(replantList, MessageEnum.ReplantRecordNotFound);
                }
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }

        public async Task<ResponseObject<List<LookupType>>> GetReplantDropdowns()
        {
            ResponseObject<List<LookupType>> PlantsList = await new CommonController().GetDropdownByType("Replant");

            

            return PlantsList;

        }

        [HttpGet]
        public async Task<ResponseObject<List<object>>> GetReplantsListByStatus(int StatusID = 0)
        {
            try
            {
                var replantsList = db.ExtRepositoryFor<ReplantRepository>().GetReplantsListByStatus(StatusID);

                if (replantsList.Count == 0)
                    return ServiceResponse.SuccessReponse(replantsList, MessageEnum.ReplantRecordNotFound);
                else
                    return ServiceResponse.SuccessReponse(replantsList, MessageEnum.ReplantRecordFoundSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }
        [HttpPost]
        public async Task<ResponseObject<bool>> UpdateStatus([FromBody]Replant _mdlReplant)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                if (_mdlReplant.StatusID == (int)StatusEnum.Resolved)
                {
                    Replant mdlReplant = db.Repository<Replant>().FindById(_mdlReplant.ID);
                    int lastGreenPoints = mdlReplant.GreenPoints;
                    mdlReplant.GreenPoints = _mdlReplant.GreenPoints;
                    mdlReplant.StatusID = _mdlReplant.StatusID;

                    mdlReplant.UpdatedBy = UserID;
                    mdlReplant.UpdatedDate = DateTime.Now;

                    db.Repository<Replant>().Update(mdlReplant);
                    db.Save();
                    //Herer to update parent Table green points 
                    db.ExtRepositoryFor<CommonRepository>().UpdateParentsTableGreenPoints(mdlReplant.UserID,UserID, mdlReplant.ID,FiveREnum.Replant.ToString(), lastGreenPoints, _mdlReplant.GreenPoints);

                    PushNotificationEvent _event = new PushNotificationEvent();
                    _event.Parameters.Add("GP", _mdlReplant.GreenPoints);
                    _event.AddPushNotifyEvent((long)NotificationEventConstants.PushNotification.Replant, mdlReplant.UserID.ToString());

                    return ServiceResponse.SuccessReponse(true, MessageEnum.ReplantUpdatedSuccessfully);
                }
                else if (_mdlReplant.StatusID == (int)StatusEnum.Declined)
                {
                    Replant mdlReplant = db.Repository<Replant>().FindById(_mdlReplant.ID);
                    mdlReplant.StatusID = _mdlReplant.StatusID;

                    mdlReplant.UpdatedBy = UserID;
                    mdlReplant.UpdatedDate = DateTime.Now;

                    db.Repository<Replant>().Update(mdlReplant);
                    db.Save();

                    PushNotificationEvent _event = new PushNotificationEvent();
                    _event.AddPushNotifyEvent((long)NotificationEventConstants.PushNotification.ReplantDeclined, mdlReplant.UserID.ToString());

                    return ServiceResponse.SuccessReponse(true, MessageEnum.ReplantUpdatedSuccessfully);
                }

                return ServiceResponse.SuccessReponse(false, MessageEnum.DefaultParametersCanNotBeNull);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpPost]
        public ResponseObject<object> ChangeStatusForReplant(int rID, string status)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                Replant amalR = db.Repository<Replant>().FindById(rID);

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

                db.Repository<Replant>().Update(amalR);
                db.Save();
                return ServiceResponse.SuccessReponse<object>(true, "Status Changed Successfully");
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }

        [HttpGet]
        public async Task<ResponseObject<List<object>>> GetAll()
        {
            try
            {
                var replantList = db.ExtRepositoryFor<ReplantRepository>().GetAllReplantsList();

                if (replantList.Count == 0)
                    return ServiceResponse.SuccessReponse(replantList, MessageEnum.RegiftItemsNotFound);
                else
                    return ServiceResponse.SuccessReponse(replantList, MessageEnum.RegiftItemGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }
        [HttpGet]
        public async Task<ResponseObject<object>> GetReplantById(int Id = 0)
        {
            try
            {
                var refusesList = db.ExtRepositoryFor<ReplantRepository>().GetReplantById(Id);

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
    }
}
