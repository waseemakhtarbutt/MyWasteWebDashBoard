using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrTech.Common.Enums;
using DrTech.Common.Extentions;
using DrTech.Common.Helpers;
using DrTech.Common.ServerResponse;
using DrTech.DAL;
using DrTech.Models;
using DrTech.Models.Common;
using DrTech.Services.Attribute;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using static DrTech.Common.Extentions.Constants;

namespace DrTech.Services.Controllers
{
    [Authorize]
    [Route("api/Reduce")]
    public class ReduceController : BaseControllerBase
    {
        [HttpPost("AddReduceItem")]
        public async Task<ResponseObject<bool>> AddReduceItem(Reduce mdlReduce)
        {
            if (mdlReduce == null)
                return ServiceResponse.ErrorReponse<bool>(MessageEnum.ReduceModelNotNull);
            try
            {
                string fileName = string.Empty; // await FileOpsHelper.UploadFile(mdlRefuse.File);
                if (mdlReduce.File != null)
                    fileName = await SaveFile(mdlReduce.File);

                Reduce reduce = new Reduce
                {
                    FileName = fileName,
                    Idea = mdlReduce.Idea,
                    Status = (int)StatusEnum.Submit,
                    StatusDescription = StatusEnum.Submit.GetDescription(),
                    Latitude = mdlReduce.Latitude,
                    Longitude =mdlReduce.Longitude
                };

                //    await _IUWork.InsertOne(reduce, CollectionNames.Reduce);
                await _IUWork.AddSubDocument<Users, Reduce>(GetLoggedInUserId(), reduce, CollectionNames.USERS, CollectionNames.Reduce);

                NotifyEvent _event = new NotifyEvent();
                _event.Parameters.Add("FileName", reduce.FileName);
                _event.Parameters.Add("Idea", reduce.Idea);
                _event.Parameters.Add("Longitude", reduce.Longitude);
                _event.Parameters.Add("Latitude", reduce.Latitude);
                _event.AddNotifyEvent((long)NotificationEventConstants.Reduce.ReduceEmailSendtoAdmin, GetLoggedInUserId());


                return ServiceResponse.SuccessReponse(true, MessageEnum.ReduceAddedSuccessfully);

            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpGet("GetAllReduceItem")]
        public async Task<ResponseObject<List<Reduce>>> GetAllReduceItem()
        {
            try
            {
                var user = _IUWork.FindOneByID<Users>(GetLoggedInUserId(), CollectionNames.USERS).Result;

                if (user.Reduce.Count == 0)
                    return ServiceResponse.SuccessReponse(user.Reduce.ToList(), MessageEnum.ReduceItemsNotFound);

                var dd = user.Reduce?.ToSortByCreationDateDescendingOrder();
                return ServiceResponse.SuccessReponse(dd, MessageEnum.ReduceItemGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Reduce>>(exp);
            }
        }

        [Auth(UserRoleTypeEnum.Admin)]
        [HttpGet("GetAll")]
        public async Task<ResponseObject<List<Reduce>>> GetAll(string id = null)
        {
            try
            {
                List<Users> lstUser = new List<Users>();

                if (string.IsNullOrEmpty(id))
                    lstUser = await _IUWork.GetAllSubDocuments<Users, Reduce>(CollectionNames.USERS, CollectionNames.Reduce); 
                else
                {
                    var dd = await _IUWork.FindOneByID<Users>(id, CollectionNames.USERS);
                    if (dd != null)
                        lstUser.Add(dd);
                }
                
                List<Reduce> reduceList = new List<Reduce>();

                if (lstUser?.Count > 0)
                {
                    foreach (var item in lstUser)
                    {
                        if (item?.Reduce?.Count > 0)
                        {
                            foreach (var reduce in item?.Reduce)
                            {
                                reduce.Latitude = item.Latitude;
                                reduce.Longitude = item.Longitude;
                                reduce.UserId = item.Id.ToString();
                                //if (reduce.Status != (int)StatusEnum.PenddingApproval)
                                reduceList.Add(reduce);
                            }
                        }
                    }
                }
                reduceList = reduceList.ToSortByCreationDateDescendingOrder();
                return ServiceResponse.SuccessReponse(reduceList, MessageEnum.ComplaintGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Reduce>>(exp);
            }
        }

        [Auth(UserRoleTypeEnum.Admin)]
        [HttpPost("UpdateStatus")]
        public async Task<ResponseObject<bool>> UpdateStatus([FromBody]ReduceViewModel mdlReuse)
        {
            if (mdlReuse == null)
                return ServiceResponse.ErrorReponse<bool>(MessageEnum.DefaultParametersCanNotBeNull);

            try
            {
                var update = Builders<Users>.Update.Set(CollectionNames.Reduce + ".$.GreenPoints", mdlReuse.GreenPoints)
                                                    .Set(CollectionNames.Reduce + ".$.UpdatedAt", DateTime.Now.ToString())
                                                     .Set(CollectionNames.Reduce + ".$.Status", mdlReuse.Status)
                                                    .Set(CollectionNames.Reduce + ".$.StatusDescription", ((StatusEnum)mdlReuse.Status).GetDescription());
                 
                var result = await _IUWork.UpdateSubDocument<Users, Reduce>(mdlReuse.Id.ToString(), update, CollectionNames.USERS, CollectionNames.Reduce);

                return ServiceResponse.SuccessReponse(result, MessageEnum.DonationAddedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
    }
}