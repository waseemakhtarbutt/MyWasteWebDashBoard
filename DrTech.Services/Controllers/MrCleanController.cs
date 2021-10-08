using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DrTech.DAL;
using Microsoft.AspNetCore.Mvc;
using DrTech.Models;
using DrTech.Models.ViewModels;
using DrTech.Common.Helpers;
using static DrTech.Common.Extentions.Constants;
using DrTech.Common.ServerResponse;
using DrTech.Common.Enums;
using Microsoft.AspNetCore.Authorization;
using DrTech.Common.Extentions;
using MongoDB.Driver;
using System.Linq;
using DrTech.Models.Common;
using DrTech.Notifications;

namespace DrTech.Services.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class MrCleanController : BaseControllerBase
    {
        [HttpPost("AddRecycleItem")]
        public async Task<ResponseObject<bool>> AddRecycleItem(RecycleViewModel FileInfo)
        {
            if (FileInfo == null)
                return ServiceResponse.ErrorReponse<bool>(MessageEnum.RecycleModelNotNull);

            try
            {
                string ImageName = FileInfo.FileName;
                string Description = FileInfo.Description;
                string FileName = await SaveFile(FileInfo.File);
                double Weight = FileInfo.Weight;
                //  DateTime CollectorDate = Convert.ToDateTime(FileInfo.CollectorDateTime);
                MrClean mdlMrClean = new MrClean
                {
                    FileName = string.IsNullOrEmpty(FileName) ? "" : FileName,
                    Description = string.IsNullOrEmpty(Description) ? "" : Description,
                    UserId = GetLoggedInUserId(),
                    CollectorDateTime = string.IsNullOrEmpty(FileInfo.CollectorDateTime) ? "" : FileInfo.CollectorDateTime,
                    Status = (int)StatusEnum.Submit,
                    Weight = Weight,
                    StatusDescription = StatusEnum.Submit.GetDescription()
                };

                await _IUWork.InsertOne(mdlMrClean, CollectionNames.RECYCLE);

                NotifyEvent _event = new NotifyEvent();
                _event.Parameters.Add("UserID", GetLoggedInUserId());
                _event.Parameters.Add("DateTime", mdlMrClean.CollectorDateTime);
                _event.AddNotifyEvent((long)NotificationEventConstants.Recycle.EmailSendToAdminRecycleInfo, FileName);



                SMSNotifyEvent _events = new SMSNotifyEvent();
                _events.Parameters.Add("Description", mdlMrClean.Description);
                _events.AddSMSNotifyEvent((long)NotificationEventConstants.Recycle.SMSSentoUser, GetLoggedInUserId());


                return ServiceResponse.SuccessReponse(true, MessageEnum.RecycleItemsAdded);

            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpGet("GetRecycleItem")]
        public async Task<ResponseObject<List<MrClean>>> GetRecycleItem(string UserId = null)
        {
            //if (UserId == null)
            //    return ServiceResponse.ErrorReponse<List<MrClean>>(MessageEnum.RecycleUserIdNotNull);

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

                List<MrClean> LstRecycleItems = _IUWork.GetModelData<MrClean>(filter, CollectionNames.RECYCLE);

                LstRecycleItems = LstRecycleItems?.ToSortByCreationDateDescendingOrder();

                if (LstRecycleItems?.Count == 0)
                    return ServiceResponse.SuccessReponse(LstRecycleItems, MessageEnum.RecycleItemsNotFound);

                return ServiceResponse.SuccessReponse(LstRecycleItems, MessageEnum.RecycleItemsGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<MrClean>>(exp);
            }

        }

        // insert template data
        [HttpPost("AddEmailTemplate")]
        public async Task<ResponseObject<bool>> AddEmailTemplate(NotificationEvents FileInfo)
        {
            if (FileInfo == null)
                return ServiceResponse.ErrorReponse<bool>(MessageEnum.RecycleModelNotNull);

            try
            {
                await _IUWork.InsertOne(FileInfo, "NotificationEvents");
                return ServiceResponse.SuccessReponse(true, MessageEnum.RecycleItemsAdded);

            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
        [HttpPost("UpdateStatusOfRecycleItems")]
        public async Task<ResponseObject<bool>> UpdateStatusOfRecycleItems([FromBody]MrCleanViewModel mdlMrClean)
        {
            if (mdlMrClean == null)
                return ServiceResponse.ErrorReponse<bool>(MessageEnum.RecycleModelNotNull);

            if (mdlMrClean.Id == null)
                return ServiceResponse.ErrorReponse<bool>(MessageEnum.UserIdRecycleIdNotNull);


            int ItemsStatus = 0;

            if (mdlMrClean.Status == (int)StatusEnum.InProgress)
                ItemsStatus = (int)StatusEnum.InProgress;
            else
                ItemsStatus = (int)StatusEnum.Resolved;

            try
            {
                mdlMrClean.Status = ItemsStatus;
                double GreenPoint = GreenPointHelper.GetGreenPointsAgainstRecycle(mdlMrClean.Weight);

                var update = Builders<MrClean>.Update
                                                .Set(o => o.Status, mdlMrClean.Status)
                                                .Set(p => p.StatusDescription, ((StatusEnum)mdlMrClean.Status).GetDescription())
                                                .Set(x => x.GreenPoints, GreenPoint)
                                                .Set(g => g.UpdatedAt, DateTime.Now.ToString());

                bool RecycleItemsUpdate = _IUWork.UpdateStatus(mdlMrClean.Id, update, CollectionNames.RECYCLE);

                // long RecycleItemsUpdate = _IUWork.UpdateStatusOfRecycleItems(mdlMrClean);
            

                var mdlRecycle  = _IUWork.FindOneByID<MrClean>(mdlMrClean.Id, CollectionNames.RECYCLE).Result;

                string _Id = mdlRecycle.UserId;

                var User = _IUWork.FindOneByID<Users>(_Id, CollectionNames.USERS).Result;

                 User.GreenPoints += GreenPoint;

                long Count = _IUWork.UpdateUserGreenPoints(User.GreenPoints, User.Id.ToString());  //_IUWork.UpdateUserGreenPoints(_Id, User.GreenPoints);

                if (RecycleItemsUpdate == true)
                {
                    NotifyEvent _event = new NotifyEvent();
                    _event.Parameters.Add("UserId", mdlMrClean.Id);
                    _event.AddNotifyEvent((long)NotificationEventConstants.Recycle.EmailSendToUserCollectionTime, _Id);
                }

                return ServiceResponse.SuccessReponse(true, MessageEnum.RecycleItemsAdded);

            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpGet("GetAll")]
        public async Task<ResponseObject<List<MrCleanViewModel>>> GetAll(string id = null)
        {
            try
            {

                List<MrClean> LstRecycleItems = new List<MrClean>();

                if (string.IsNullOrEmpty(id))
                    LstRecycleItems = _IUWork.GetModelData<MrClean>(CollectionNames.RECYCLE);
                else
                {
                    LstRecycleItems = _IUWork.GetModelByUserID<MrClean>(id, CollectionNames.RECYCLE);
                }

                List<Users> LstUsers = _IUWork.GetModelData<Users>(CollectionNames.USERS);
                List<MrCleanViewModel> lst = new List<MrCleanViewModel>();

                if (LstRecycleItems.Any())
                {
                    foreach (var item in LstRecycleItems)
                    {
                        var user = LstUsers.Find(p => p.Id.ToString() == item.UserId);
                        if (user != null)
                        {
                            MrCleanViewModel model = new MrCleanViewModel
                            {
                                Id = item.Id.ToString(),
                                Longitude = user.Longitude,
                                Latitude = user.Latitude,
                                UserId = item.UserId,
                                Weight = item.Weight,
                                Description = item.Description,
                                Status = item.Status,
                                StatusDescription = item.StatusDescription

                            };
                            lst.Add(model);
                        }
                    }
                }

                LstRecycleItems = LstRecycleItems?.ToSortByCreationDateDescendingOrder();

                if (LstRecycleItems?.Count == 0)
                    return ServiceResponse.SuccessReponse(lst, MessageEnum.RecycleItemsNotFound);

                return ServiceResponse.SuccessReponse(lst, MessageEnum.RecycleItemsGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<MrCleanViewModel>>(exp);
            }

        }
    }
}