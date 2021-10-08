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
    public class ReportController : BaseController
    {
        [HttpPost]
        public async Task<ResponseObject<bool>> AddReport()
        {
            try
            {
                Report mdlReport = new Report();
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                string FileName = string.Empty;
                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                FileName = await FileOpsHelper.UploadFileNew(file, ContainerName.REPORT);

                mdlReport.FileName = FileName;
                mdlReport.Priority = "High"; // HttpContext.Current.Request.Form["Priority")[0];
                mdlReport.Longitude = Convert.ToDecimal(HttpContext.Current.Request.Form["longitude"]);
                mdlReport.GreenPoints = 0; // Convert.ToInt32(HttpContext.Current.Request.Form["GreenPoints")[0]);
                mdlReport.Latitude = Convert.ToDecimal(HttpContext.Current.Request.Form["latitude"]);
                mdlReport.Description = HttpContext.Current.Request.Form["description"];
                mdlReport.StatusID = (int)StatusEnum.Submit; //Convert.ToInt32(HttpContext.Current.Request.Form["StatusID")[0]);              
                mdlReport.CreatedBy = (int)UserID;
                mdlReport.UserID = (int)UserID;
                mdlReport.CreatedDate = DateTime.Now;
              //  mdlReport.UpdatedDate = DateTime.Now;

                db.Repository<Report>().Insert(mdlReport);
                db.Save();

                NotifyEvent _event = new NotifyEvent();
                _event.Parameters.Add("Description", mdlReport.Description);
                _event.Parameters.Add("Longitude", mdlReport.Longitude);
                _event.Parameters.Add("Latitude", mdlReport.Latitude);
                _event.AddNotifyEvent((long)NotificationEventConstants.Report.ReportEmailSendtoAdmin, Convert.ToString(UserID));


                SMSNotifyEvent _events = new SMSNotifyEvent();
                _events.Parameters.Add("Description", mdlReport.Description);
                _events.AddSMSNotifyEvent((long)NotificationEventConstants.Report.SendSMSToUser, Convert.ToString(UserID));

                return ServiceResponse.SuccessReponse(true, MessageEnum.ComplaintAddedSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpPost]
        public ResponseObject<object> ChangeStatusForReport(int rID, string status)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                Report amalR = db.Repository<Report>().FindById(rID);

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

                db.Repository<Report>().Update(amalR);
                db.Save();
                return ServiceResponse.SuccessReponse<object>(true, "Status Changed Successfully");
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }


        [HttpGet]
        public ResponseObject<Report> GetReportByID(string ReportId)
        {
            if (ReportId == null)
                return ServiceResponse.ErrorReponse<Report>(MessageEnum.ComplaintIdCannotBeNull);
            try
            {
                var OneReport = db.Repository<Report>().GetAll().FirstOrDefault(d => d.ID.ToString() == ReportId);

                if (OneReport == null)
                    return ServiceResponse.SuccessReponse(OneReport, MessageEnum.ComplaintNotFound);

                return ServiceResponse.SuccessReponse(OneReport, MessageEnum.ComplaintGetSuccess);

            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<Report>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<List<object>> GetReportsByUserID(int UserId = 0)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                //  List<Report> lstReport = db.Repository<Report>().GetAll().Where(d => d.UserID == UserID).ToList();   
                List<object> lstReport = db.ExtRepositoryFor<ReportRepository>().GetAllReportsItemById(UserID);
                if (lstReport.Count == 0)
                    return ServiceResponse.SuccessReponse(lstReport, MessageEnum.ComplaintNotFound);
                return ServiceResponse.SuccessReponse(lstReport, MessageEnum.ComplaintGetSuccess);

            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ResponseObject<List<object>>> GetReportsListByStatus(int StatusID = 0)
        {
            try
            {
                var reportsList = db.ExtRepositoryFor<ReportRepository>().GetReportsListByStatus(StatusID);

                if (reportsList.Count == 0)
                    return ServiceResponse.SuccessReponse(reportsList, MessageEnum.ReportItemsNotFound);
                else
                    return ServiceResponse.SuccessReponse(reportsList, MessageEnum.ReportItemGetSuccess);
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
                var reportList = db.ExtRepositoryFor<ReportRepository>().GetAllReportsList();

                if (reportList.Count == 0)
                    return ServiceResponse.SuccessReponse(reportList, MessageEnum.RegiftItemsNotFound);
                else
                    return ServiceResponse.SuccessReponse(reportList, MessageEnum.RegiftItemGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }
        [HttpGet]
        public async Task<ResponseObject<object>> GetReportById(int Id = 0)
        {
            try
            {
                var refusesList = db.ExtRepositoryFor<ReportRepository>().GetReportById(Id);

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
        public async Task<ResponseObject<bool>> UpdateStatus([FromBody]Report _mdlReport)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                if (_mdlReport.StatusID == (int)StatusEnum.Resolved)
                {
                    Report mdlReport = db.Repository<Report>().FindById(_mdlReport.ID);
                    int lastGreenPoints = mdlReport.GreenPoints;
                    mdlReport.GreenPoints = _mdlReport.GreenPoints;
                    mdlReport.StatusID = _mdlReport.StatusID;
                    mdlReport.UpdatedBy = UserID;
                    mdlReport.UpdatedDate = DateTime.Now;
                    db.Repository<Report>().Update(mdlReport);
                    db.Save();
                    //Herer to update parent Table green points 
                    db.ExtRepositoryFor<CommonRepository>().UpdateParentsTableGreenPoints(mdlReport.UserID,UserID, mdlReport.ID,FiveREnum.Report.ToString(), lastGreenPoints, _mdlReport.GreenPoints);
                    PushNotificationEvent _event = new PushNotificationEvent();
                    _event.Parameters.Add("GP", _mdlReport.GreenPoints);
                    _event.AddPushNotifyEvent((long)NotificationEventConstants.PushNotification.Report, mdlReport.UserID.ToString());
                   
                    return ServiceResponse.SuccessReponse(true, MessageEnum.ReportUpdatedSuccessfully);
                }
                else if (_mdlReport.StatusID == (int)StatusEnum.Declined)
                {
                    Report mdlReport = db.Repository<Report>().FindById(_mdlReport.ID);                  
                    mdlReport.StatusID = _mdlReport.StatusID;
                    mdlReport.UpdatedBy = UserID;
                    mdlReport.UpdatedDate = DateTime.Now;
                    db.Repository<Report>().Update(mdlReport);
                    db.Save();

                    PushNotificationEvent _event = new PushNotificationEvent();
                    _event.AddPushNotifyEvent((long)NotificationEventConstants.PushNotification.ReportDeclined, mdlReport.UserID.ToString());
                    return ServiceResponse.SuccessReponse(true, MessageEnum.ReportUpdatedSuccessfully);
                }

                return ServiceResponse.SuccessReponse(false, MessageEnum.DefaultParametersCanNotBeNull);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        //[Auth(UserRoleTypeEnum.Admin)]
        //[HttpPost("UpdateStatus")]
        //public async Task<ResponseObject<bool>> UpdateStatus([FromBody]UpdateStatusViewModel complaint)
        //{
        //    if (complaint == null)
        //        return ServiceResponse.ErrorReponse<bool>(MessageEnum.DefaultParametersCanNotBeNull);

        //    try
        //    {

        //        var update = Builders<Report>.Update
        //                                 .Set(o => o.Status, complaint.Status)
        //                                 .Set(p => p.StatusDescription, ((StatusEnum)complaint.Status).GetDescription())
        //                                 .Set(g => g.GreenPoints, complaint.GreenPoints)
        //                                 .Set(g => g.UpdatedAt, DateTime.Now.ToString());


        //        bool result = _IUWork.UpdateStatus(complaint.Id.ToString(), update, CollectionNames.Report);



        //        //var update = Builders<Users>.Update.Set(CollectionNames.Report + ".$.Status", complaint.Status)
        //        //                                       .Set(CollectionNames.Report + ".$.StatusDescription", ((StatusEnum)complaint.Status).GetDescription())
        //        //                                       .Set(CollectionNames.Report + ".$.GreenPoints", complaint.GreenPoints)
        //        //                                       .Set(CollectionNames.Report + ".$.UpdatedAt", DateTime.Now.ToString());

        //        //var result = await _IUWork.UpdateSubDocument<Users, Report>(complaint.Id.ToString(), update, CollectionNames.USERS, CollectionNames.Report);
        //        return ServiceResponse.SuccessReponse(result, MessageEnum.ComplaintGetSuccess);
        //    }
        //    catch (Exception exp)
        //    {
        //        return ServiceResponse.ErrorReponse<bool>(exp);
        //    }
        //}
        //[Auth(UserRoleTypeEnum.Admin)]
        //[HttpGet("GetAll")]
        //public async Task<ResponseObject<List<Report>>> GetAll(string id = null)
        //{
        //    try
        //    {
        //        List<Report> lstAllReports = new List<Report>();

        //        if (string.IsNullOrEmpty(id))
        //            lstAllReports = _IUWork.GetModelData<Report>(CollectionNames.Report);
        //        else
        //        {
        //            lstAllReports = _IUWork.GetModelByUserID<Report>(id, CollectionNames.Report);
        //        }


        //        lstAllReports = lstAllReports?.ToSortByCreationDateDescendingOrder();
        //        return ServiceResponse.SuccessReponse(lstAllReports, MessageEnum.ComplaintGetSuccess);
        //    }
        //    catch (Exception exp)
        //    {
        //        return ServiceResponse.ErrorReponse<List<Report>>(exp);
        //    }
        //}
    }
}
