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
using Microsoft.AspNetCore.Http;
using DrTech.Models.ViewModels;
using DrTech.Common.Extentions;
using MongoDB.Driver;

namespace DrTech.Services.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ComplaintController : BaseControllerBase
    {

        [HttpPost("AddComplaint"), DisableRequestSizeLimit]
        public async Task<ResponseObject<bool>> AddComplaint(Complaints complaint)
        {
            if (complaint == null)
                return ServiceResponse.ErrorReponse<bool>(MessageEnum.ComplaintIdCannotBeNull);

            try
            {
                complaint.FileName = await SaveFile(complaint.File);

                complaint.File = null;

                complaint.Status = (int)StatusEnum.Submit;
                complaint.StatusDescription = StatusEnum.Submit.GetDescription();


                await _IUWork.InsertOne(complaint, CollectionNames.COMPLAINTS);

                return ServiceResponse.SuccessReponse(true, MessageEnum.ComplaintAddedSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
        [HttpGet("GetComplaint")]
        public async Task<ResponseObject<Complaints>> GetComplaint(string complaintId)
        {
            if (complaintId == null)
                return ServiceResponse.ErrorReponse<Complaints>(MessageEnum.ComplaintIdCannotBeNull);
            try
            {
                List<FilterHelper> filter = new List<FilterHelper>
                {
                    new FilterHelper
                    {
                        Field = "_id",
                        Value = complaintId
                    }
                };

                var complaint = await _IUWork.FindOneByID<Complaints>(complaintId, CollectionNames.COMPLAINTS);

                if (complaint == null)
                    return ServiceResponse.SuccessReponse(complaint, MessageEnum.ComplaintNotFound);

                return ServiceResponse.SuccessReponse(complaint, MessageEnum.ComplaintGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<Complaints>(exp);
            }
        }
        [HttpGet("GetComplaints")]
        public async Task<ResponseObject<List<Complaints>>> GetComplaints(string UserId = null)
        {
            //if (UserId == null)
            //    return ServiceResponse.ErrorReponse<List<Complaints>>(MessageEnum.DefaultUserNotAuthorized);
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

                List<Complaints> LstRecycleItems = _IUWork.GetModelData<Complaints>(filter, CollectionNames.COMPLAINTS);

                if (LstRecycleItems == null && LstRecycleItems.Count == 0)
                    return ServiceResponse.SuccessReponse(LstRecycleItems, MessageEnum.ComplaintNotFound);
                return ServiceResponse.SuccessReponse(LstRecycleItems, MessageEnum.ComplaintGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Complaints>>(exp);
            }
        }

        [HttpPost("UpdateStatus")]
        public async Task<ResponseObject<bool>> UpdateStatus([FromBody]UpdateStatusViewModel complaint)
        {
            if (complaint == null)
                return ServiceResponse.ErrorReponse<bool>(MessageEnum.ComplaintIdCannotBeNull);

            try
            {
                var update = Builders<Complaints>.Update
                                                    .Set(o => o.Status, complaint.Status)
                                                    .Set(p => p.StatusDescription, ((StatusEnum)complaint.Status).GetDescription());

                var result = _IUWork.UpdateStatus(complaint.Id.ToString(), update, CollectionNames.COMPLAINTS);

                return ServiceResponse.SuccessReponse(result, MessageEnum.ComplaintGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
        [HttpGet("GetAll")]
        public async Task<ResponseObject<List<Complaints>>> GetAll()
        {
            try
            {
                var LstRecycleItems = _IUWork.GetModelData<Complaints>(CollectionNames.COMPLAINTS);
                if (LstRecycleItems == null && LstRecycleItems.Count == 0)
                    return ServiceResponse.SuccessReponse(LstRecycleItems, MessageEnum.ComplaintNotFound);
                return ServiceResponse.SuccessReponse(LstRecycleItems, MessageEnum.ComplaintGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Complaints>>(exp);
            }
        }
    }
}