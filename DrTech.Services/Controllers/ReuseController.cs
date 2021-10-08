using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrTech.Common.Enums;
using DrTech.Common.Extentions;
using DrTech.Common.Helpers;
using DrTech.Common.ServerResponse;
using DrTech.Models;
using DrTech.Models.Common;
using DrTech.Services.Attribute;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using static DrTech.Common.Extentions.Constants;

namespace DrTech.Services.Controllers
{
    [Route("api/Reuse")]
    public class ReuseController : BaseControllerBase
    {
        [HttpPost("AddReuseItems"), DisableRequestSizeLimit]
        public async Task<ResponseObject<bool>> AddReuseItems(Reuse mdlReuse)
        {
            if (mdlReuse == null)
                return ServiceResponse.ErrorReponse<bool>(MessageEnum.DefaultParametersCanNotBeNull);

            try
            {
                mdlReuse.FileName = await SaveFile(mdlReuse.File);

                mdlReuse.Status = (int)StatusEnum.Submit;
                mdlReuse.StatusDescription = StatusEnum.Submit.GetDescription();

                await _IUWork.AddSubDocument<Users, Reuse>(GetLoggedInUserId(), mdlReuse, CollectionNames.USERS, CollectionNames.REUSE);


                NotifyEvent _event = new NotifyEvent();
                _event.Parameters.Add("FileName", mdlReuse.FileName);
                _event.Parameters.Add("Idea", mdlReuse.Idea);
                _event.Parameters.Add("Longitude", mdlReuse.Longitude);
                _event.Parameters.Add("Latitude", mdlReuse.Latitude);
                _event.AddNotifyEvent((long)NotificationEventConstants.Reuse.ReuseEmailSendtoAdmin, GetLoggedInUserId());

                return ServiceResponse.SuccessReponse(true, MessageEnum.ReuseItemSuccssfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
       

        [HttpGet("GetReuseItemsByUserID")]
        public async Task<ResponseObject<List<Reuse>>> GetReuseItemsByUserID(string UserId = null)
        {
            try
            {
                var user = _IUWork.FindOneByID<Users>(GetLoggedInUserId(), CollectionNames.USERS).Result;
                if (user.Reuse.Count == 0)
                    return ServiceResponse.SuccessReponse(user.Reuse.ToList(), MessageEnum.DefaultSuccessMessage);

                user.Reuse = user.Reuse?.ToSortByCreationDateDescendingOrder();
                return ServiceResponse.SuccessReponse(user.Reuse.ToList(), MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Reuse>>(exp);
            }
        }

       
        [HttpGet("GetAllReuseItems")]
        public async Task<ResponseObject<List<Reuse>>> GetAllReuseItems()
        {
            try
            {
                List<Users> lstUsersDetails = _IUWork.GetModelData<Users>(CollectionNames.USERS);

                List<Reuse> LstRpt = new List<Reuse>();

                foreach (var item in lstUsersDetails)
                {
                    if (item.Reuse?.Count > 0)
                    {
                        foreach (var report in item.Reuse)
                        {
                            report.UserId = item.Id.ToString();
                            LstRpt.Add(report);
                        }
                    }
                }

                LstRpt = LstRpt?.ToSortByCreationDateDescendingOrder();

                return ServiceResponse.SuccessReponse(LstRpt, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Reuse>>(exp);
            }
        }



        [Auth(UserRoleTypeEnum.Admin)]
        [HttpGet("GetAll")]
        public async Task<ResponseObject<List<Reuse>>> GetAll(string id = null)
        {
            try
            {
                List<Users> lstUser = new List<Users>();

                if (string.IsNullOrEmpty(id))
                    lstUser = await _IUWork.GetAllSubDocuments<Users, Reuse>(CollectionNames.USERS, CollectionNames.REUSE);
                else
                {
                    var dd = await _IUWork.FindOneByID<Users>(id, CollectionNames.USERS);
                    if (dd != null)
                        lstUser.Add(dd);
                }
                
                List<Reuse> reduceList = new List<Reuse>();
                if (lstUser?.Count > 0)
                {
                    foreach (var item in lstUser)
                    {
                        if (item?.Reuse?.Count > 0)
                        {
                            foreach (var reuse in item?.Reuse)
                            {
                                reuse.UserId = item.Id.ToString();
                                //if (Refuse.Status != (int)StatusEnum.PenddingApproval)
                                reduceList.Add(reuse);
                            }
                        }
                    }
                }
                reduceList = reduceList.ToSortByCreationDateDescendingOrder();
                return ServiceResponse.SuccessReponse(reduceList, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Reuse>>(exp);
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
                var update = Builders<Users>.Update.Set(CollectionNames.REUSE + ".$.GreenPoints", mdlReuse.GreenPoints)
                                                    .Set(CollectionNames.REUSE + ".$.UpdatedAt", DateTime.Now.ToString())
                                                    .Set(CollectionNames.REUSE + ".$.Status", mdlReuse.Status)
                                                    .Set(CollectionNames.REUSE + ".$.StatusDescription", ((StatusEnum)mdlReuse.Status).GetDescription());

                var result = await _IUWork.UpdateSubDocument<Users, Reuse>(mdlReuse.Id.ToString(), update, CollectionNames.USERS, CollectionNames.REUSE);

                return ServiceResponse.SuccessReponse(result, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
    }
}