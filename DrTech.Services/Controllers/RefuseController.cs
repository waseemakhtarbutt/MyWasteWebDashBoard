using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DrTech.Common.Enums;
using DrTech.Common.Extentions;
using DrTech.Common.Helpers;
using DrTech.Common.ServerResponse;
using DrTech.DAL;
using DrTech.Models;
using DrTech.Models.Common;
using DrTech.Models.ViewModels;
using DrTech.Services.Attribute;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using static DrTech.Common.Extentions.Constants;

namespace DrTech.Services.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class RefuseController : BaseControllerBase
    {
        [HttpPost("AddRefuseItem")]
        public async Task<ResponseObject<bool>> AddRefuseItem(Refuse mdlRefuse)
        {
            if (mdlRefuse == null)
                return ServiceResponse.ErrorReponse<bool>(MessageEnum.RefuseModelNotNull);
            try
            {
                string fileName = string.Empty; // await FileOpsHelper.UploadFile(mdlRefuse.File);
                if (mdlRefuse.File != null)
                    fileName = await SaveFile(mdlRefuse.File);

                Refuse refuse = new Refuse
                {
                    FileName = fileName,
                    Idea = mdlRefuse.Idea,
                    Longitude = mdlRefuse.Longitude,
                    Latitude = mdlRefuse.Latitude,
                    Status = (int)StatusEnum.Submit,
                    StatusDescription = StatusEnum.Submit.GetDescription(),


                };

                await _IUWork.AddSubDocument<Users, Refuse>(GetLoggedInUserId(), refuse, CollectionNames.USERS, CollectionNames.Refuse);
                //await _IUWork.InsertOne(refuse, CollectionNames.Refuse);

                NotifyEvent _event = new NotifyEvent();
                _event.Parameters.Add("FileName", refuse.FileName);
                _event.Parameters.Add("Idea", refuse.Idea);
                _event.Parameters.Add("Longitude", refuse.Longitude);
                _event.Parameters.Add("Latitude", refuse.Latitude);
                _event.AddNotifyEvent((long)NotificationEventConstants.Refuse.RefuseEmailSendtoAdmin, GetLoggedInUserId());


                return ServiceResponse.SuccessReponse(true, MessageEnum.RefuseAddedSuccessfully);

            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpGet("GetAllRefuseItem")]
        public async Task<ResponseObject<List<Refuse>>> GetAllRefuseItem()
        {

            try
            {
                var user = _IUWork.FindOneByID<Users>(GetLoggedInUserId(), CollectionNames.USERS).Result;

                if (user.Refuse.Count == 0)
                    return ServiceResponse.SuccessReponse(user.Refuse.ToList(), MessageEnum.RefuseItemsNotFound);
                var dd = user.Refuse?.ToSortByCreationDateDescendingOrder();
                return ServiceResponse.SuccessReponse(dd, MessageEnum.RefuseItemGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Refuse>>(exp);
            }
        }

        [Auth(UserRoleTypeEnum.Admin)]
        [HttpGet("GetAll")]
        public async Task<ResponseObject<List<Refuse>>> GetAll(string id = null)
        {
            try
            {
                List<Users> lstUser = new List<Users>();

                if (string.IsNullOrEmpty(id))
                    lstUser = await _IUWork.GetAllSubDocuments<Users, Refuse>(CollectionNames.USERS, CollectionNames.Refuse);
                else
                {
                    var dd = await _IUWork.FindOneByID<Users>(id, CollectionNames.USERS);
                    if(dd != null)
                        lstUser.Add(dd);
                }
                List<Refuse> reduceList = new List<Refuse>();

                if (lstUser?.Count > 0)
                {
                    foreach (var item in lstUser)
                    {
                        if (item?.Refuse?.Count > 0)
                        {
                            foreach (var refuse in item?.Refuse)
                            {
                                refuse.UserId = item.Id.ToString();
                                //if (Refuse.Status != (int)StatusEnum.PenddingApproval)
                                reduceList.Add(refuse);
                            }
                        }
                    }
                }
                reduceList = reduceList.ToSortByCreationDateDescendingOrder();
                return ServiceResponse.SuccessReponse(reduceList, MessageEnum.ComplaintGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Refuse>>(exp);
            }
        }

        [Auth(UserRoleTypeEnum.Admin)]
        [HttpPost("UpdateStatus")]
        public async Task<ResponseObject<bool>> UpdateStatus([FromBody]RefuseViewModel mdlReuse)
        {
            if (mdlReuse == null)
                return ServiceResponse.ErrorReponse<bool>(MessageEnum.DefaultParametersCanNotBeNull);

            try
            {
                var update = Builders<Users>.Update.Set(CollectionNames.Refuse + ".$.GreenPoints", mdlReuse.GreenPoints)
                                                    .Set(CollectionNames.Refuse + ".$.UpdatedAt", DateTime.Now.ToString())
                                                    .Set(CollectionNames.Refuse + ".$.Status", mdlReuse.Status)
                                                    .Set(CollectionNames.Refuse + ".$.StatusDescription", ((StatusEnum)mdlReuse.Status).GetDescription());

                var result = await _IUWork.UpdateSubDocument<Users, Refuse>(mdlReuse.Id.ToString(), update, CollectionNames.USERS, CollectionNames.Refuse);

                return ServiceResponse.SuccessReponse(result, MessageEnum.RefuseItemUpdatedSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }


   


    }
}
