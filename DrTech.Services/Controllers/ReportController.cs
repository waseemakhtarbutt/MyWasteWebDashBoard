using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DrTech.DAL;
using Microsoft.AspNetCore.Mvc;
using DrTech.Models;
using static DrTech.Common.Extentions.Constants;
using DrTech.Common.Helpers;
using DrTech.Common.ServerResponse;
using DrTech.Common.Enums;
using Microsoft.AspNetCore.Authorization; 
using DrTech.Models.ViewModels;
using DrTech.Common.Extentions;
using MongoDB.Driver;
using System.Linq;
using DrTech.Models.Common;
using DrTech.Services.Attribute;
using DrTech.Notifications;

namespace DrTech.Services.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ReportController : BaseControllerBase
    {

        [HttpPost("AddReport"), DisableRequestSizeLimit]
        public async Task<ResponseObject<bool>> AddReport(Report mdlReport)
        {
            if (mdlReport == null)
                return ServiceResponse.ErrorReponse<bool>(MessageEnum.ComplaintIdCannotBeNull);

            try
            {
                mdlReport.FileName = await SaveFile(mdlReport.File);
                mdlReport.File = null;
                mdlReport.Status = (int)StatusEnum.Submit;
                mdlReport.StatusDescription = StatusEnum.Submit.GetDescription();
                mdlReport.UserId = GetLoggedInUserId();
                await _IUWork.InsertOne(mdlReport, CollectionNames.Report);
                // await _IUWork.AddSubDocument<Users, Report>(GetLoggedInUserId(), mdlReport, CollectionNames.USERS, CollectionNames.Report);

                NotifyEvent _event = new NotifyEvent();
                _event.Parameters.Add("Description", mdlReport.Description);
                _event.Parameters.Add("Longitude", mdlReport.Longitude);
                _event.Parameters.Add("Latitude", mdlReport.Latitude);
                _event.AddNotifyEvent((long)NotificationEventConstants.Report.ReportEmailSendtoAdmin, GetLoggedInUserId());


                SMSNotifyEvent _events = new SMSNotifyEvent();
                _events.Parameters.Add("Description", mdlReport.Description);
                _events.AddSMSNotifyEvent((long)NotificationEventConstants.Report.SendSMSToUser, GetLoggedInUserId());


                return ServiceResponse.SuccessReponse(true, MessageEnum.ComplaintAddedSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
        [HttpGet("GetReportByID")]
        public async Task<ResponseObject<Report>> GetReportByID(string ReportId)
        {
            if (ReportId == null)
                return ServiceResponse.ErrorReponse<Report>(MessageEnum.ComplaintIdCannotBeNull);
            try
            {
                List<FilterHelper> filter = new List<FilterHelper>
                {
                    new FilterHelper
                    {
                        Field = "_id",
                        Value = ReportId
                    }
                };

                var complaint = await _IUWork.FindOneByID<Report>(ReportId, CollectionNames.Report);

                if (complaint == null)
                    return ServiceResponse.SuccessReponse(complaint, MessageEnum.ComplaintNotFound);

                return ServiceResponse.SuccessReponse(complaint, MessageEnum.ComplaintGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<Report>(exp);
            }
        }

        [HttpGet("GetReportsByUserID")]
        public async Task<ResponseObject<List<Report>>> GetReportsByUserID(string UserId = null)
        {
            try
            {

                List<FilterHelper> filter = new List<FilterHelper>
                {
                    new FilterHelper
                    {
                        Field = "UserId",
                        Value = GetLoggedInUserId()
                    }
                };

                // var complaint = await _IUWork.FindOneByID<Report>(filter, CollectionNames.Report);
                List<Report> lstReport = _IUWork.GetModelData<Report>(filter, CollectionNames.Report);
                // var user = _IUWork.FindOneByID<Users>(GetLoggedInUserId(), CollectionNames.USERS).Result;
                if (lstReport.Count == 0)
                    return ServiceResponse.SuccessReponse(lstReport, MessageEnum.ComplaintNotFound);
                lstReport = lstReport?.ToSortByCreationDateDescendingOrder();
                return ServiceResponse.SuccessReponse(lstReport, MessageEnum.ComplaintGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Report>>(exp);
            }
        }

        [Auth(UserRoleTypeEnum.Admin)]
        [HttpPost("UpdateStatus")]
        public async Task<ResponseObject<bool>> UpdateStatus([FromBody]UpdateStatusViewModel complaint)
        {
            if (complaint == null)
                return ServiceResponse.ErrorReponse<bool>(MessageEnum.DefaultParametersCanNotBeNull);

            try
            {

                var update = Builders<Report>.Update
                                         .Set(o => o.Status, complaint.Status)
                                         .Set(p => p.StatusDescription, ((StatusEnum)complaint.Status).GetDescription())
                                         .Set(g => g.GreenPoints, complaint.GreenPoints)
                                         .Set(g => g.UpdatedAt, DateTime.Now.ToString());


                bool result = _IUWork.UpdateStatus(complaint.Id.ToString(), update, CollectionNames.Report);


                if (result == true)
                {
                    var Regift = _IUWork.FindOneByID<Report>(complaint.Id.ToString(), CollectionNames.Report).Result;
                    var User = _IUWork.FindOneByID<Users>(Regift.UserId.ToString(), CollectionNames.USERS).Result;
                    User.GreenPoints += complaint.GreenPoints;
                    long ID = _IUWork.UpdateUserGreenPoints(User.GreenPoints, User.Id.ToString());
                }
                //var update = Builders<Users>.Update.Set(CollectionNames.Report + ".$.Status", complaint.Status)
                //                                       .Set(CollectionNames.Report + ".$.StatusDescription", ((StatusEnum)complaint.Status).GetDescription())
                //                                       .Set(CollectionNames.Report + ".$.GreenPoints", complaint.GreenPoints)
                //                                       .Set(CollectionNames.Report + ".$.UpdatedAt", DateTime.Now.ToString());

                //var result = await _IUWork.UpdateSubDocument<Users, Report>(complaint.Id.ToString(), update, CollectionNames.USERS, CollectionNames.Report);
                return ServiceResponse.SuccessReponse(result, MessageEnum.ComplaintGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
        [Auth(UserRoleTypeEnum.Admin)]
        [HttpGet("GetAll")]
        public async Task<ResponseObject<List<Report>>> GetAll(string id = null)
        {
            try
            {
                List<Report> lstAllReports = new List<Report>();

                if (string.IsNullOrEmpty(id))
                    lstAllReports = _IUWork.GetModelData<Report>(CollectionNames.Report);
                else
                {
                    lstAllReports = _IUWork.GetModelByUserID<Report>(id, CollectionNames.Report);
                }


                lstAllReports = lstAllReports?.ToSortByCreationDateDescendingOrder();
                return ServiceResponse.SuccessReponse(lstAllReports, MessageEnum.ComplaintGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Report>>(exp);
            }
        }
    }
}