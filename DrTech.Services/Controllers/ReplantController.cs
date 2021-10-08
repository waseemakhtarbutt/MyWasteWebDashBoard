using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrTech.DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DrTech.Models;
using System.IO;
using DrTech.Common.Extentions;
using static DrTech.Common.Extentions.Constants;
using DrTech.Common.Helpers;
using Microsoft.AspNetCore.Authorization;
using DrTech.Common.ServerResponse;
using DrTech.Common.Enums;
using DrTech.Models.ViewModels;
using DrTech.Models.Common;
using DrTech.Services.Attribute;
using MongoDB.Driver;
using DrTech.Notifications;

namespace DrTech.Services.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ReplantController : BaseControllerBase
    {
        [HttpPost("AddPlantTree"), DisableRequestSizeLimit]
        public async Task<ResponseObject<bool>> AddPlantTree(Replant tree)
        {
            try
            {
                if (tree == null) return ServiceResponse.ErrorReponse<bool>(MessageEnum.DefaultParametersCanNotBeNull);


                tree.FileName = await SaveFile(tree.File);
                
                tree.Status = (int)StatusEnum.Submit;
                tree.StatusDescription = StatusEnum.Submit.GetDescription();


                await _IUWork.AddSubDocument<Users, Replant>(GetLoggedInUserId(), tree, CollectionNames.USERS, CollectionNames.REPLANT);

                double GreenPoint = GreenPointHelper.GetGreenPointsAgainstRePlant(Convert.ToDouble(tree.Height), tree.TreeCount);

                var User = _IUWork.FindOneByID<Users>(GetLoggedInUserId(), CollectionNames.USERS).Result;

                GreenPoint = User.GreenPoints + GreenPoint;
                long Count = _IUWork.UpdateUserGreenPoints(GreenPoint, GetLoggedInUserId());

                if (Count != 0)
                {
                    NotifyEvent _event = new NotifyEvent();
                    _event.Parameters.Add("FileName", tree.FileName);
                    _event.Parameters.Add("PlantName", tree.PlantName);
                    _event.Parameters.Add("TreeCount", tree.TreeCount);
                    _event.Parameters.Add("Height", tree.Height);
                    _event.Parameters.Add("Longitude", tree.Longitude);
                    _event.Parameters.Add("Latitude", tree.Latitude);
                    _event.AddNotifyEvent((long)NotificationEventConstants.RePlant.EmailSendToAdminForReplantInfo, GetLoggedInUserId());


                    SMSNotifyEvent _events = new SMSNotifyEvent();
                    _events.Parameters.Add("PlantName", tree.PlantName);
                    _events.AddSMSNotifyEvent((long)NotificationEventConstants.RePlant.SMSSendToUser, GetLoggedInUserId());

                    //NotifyEvent _events = new NotifyEvent();
                    //_events.Parameters.Add("UserId", GetLoggedInUserId());
                    //_events.AddNotifyEvent((long)NotificationEventConstants.RePlant.EmailSentToUserForGreenPoints, GetLoggedInUserId());


                }


                return ServiceResponse.SuccessReponse(true, MessageEnum.ReplantAddedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }

        }

        //[HttpGet("GetComplaint")]
        //public async Task<ServiceResponse> GetComplaint(string UserId, string complaintId)
        //{
        //    ServiceResponse svcResponse = new ServiceResponse();

        //    try
        //    {

        //        List<FilterHelper> filter = new List<FilterHelper>
        //        {
        //             new FilterHelper
        //             {
        //                Field = "_id",
        //                Value = complaintId
        //             }
        //        };

        //        var complaint = await _IUWork.FindOneByID<Complaints>(complaintId, CollectionEnum.Complaints.GetDescription());

        //        if (complaint != null)
        //        {
        //            svcResponse.StatusCode = 0;
        //            svcResponse.StatusMessage = "Success";
        //            svcResponse.Data = complaint;
        //        }
        //        else
        //        {
        //            svcResponse.StatusCode = 1003;
        //            svcResponse.StatusMessage = "No Record Found";
        //            svcResponse.Data = null;
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        svcResponse = CreateResponseObject(1004, "Unknown Error");
        //        string excetionMsg = exp.Message;
        //    }
        //    return svcResponse;
        //}

        [HttpGet("GetPlantedTrees")]
        public async Task<ResponseObject<List<ReplantViewModel>>> GetPlantedTrees(string UserId = null)
        {
            try
            {
                var user = _IUWork.FindOneByID<Users>(GetLoggedInUserId(), CollectionNames.USERS).Result;
                var replantList = new List<ReplantViewModel>();

                if (user != null && user.Replant?.Count > 0)
                {
                    user.Replant = user.Replant?.ToSortByCreationDateDescendingOrder();

                    foreach (var item in user.Replant)
                    {
                        if (string.IsNullOrEmpty(item.Parent))
                        {
                            replantList.Add(new ReplantViewModel
                            {
                                Description = item.Description,
                                FileName = item.FileName,
                                Height = item.Height,
                                Id = item.Id.ToString(),
                                Latitude = item.Latitude,
                                Longitude = item.Longitude,
                                Reminder = item.Reminder,
                                TreeCount = item.TreeCount,
                                PlantName = item.PlantName,
                                //UserId = item.UserId,
                                PlantNameDescription = item.PlantNameDescription,
                                Child = user.Replant.Where(p => p.Parent == item.Id.ToString())?.ToList()?.ToSortByCreationDateDescendingOrder()?.ConvertAll(q => new ReplantViewModel
                                {
                                    Description = q.Description,
                                    FileName = q.FileName,
                                    Height = q.Height,
                                    Id = q.Id.ToString(),
                                    Latitude = q.Latitude,
                                    Longitude = q.Longitude,
                                    Reminder = q.Reminder,
                                    TreeCount = q.TreeCount,
                                    //UserId = q.UserId,
                                })
                            });
                        }
                    }
                    return ServiceResponse.SuccessReponse(replantList, MessageEnum.ReplantRecordFoundSuccessfully);
                }
                else
                {
                    return ServiceResponse.SuccessReponse(replantList, MessageEnum.ReplantRecordNotFound);
                }
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<ReplantViewModel>>(exp);
            }
        }

        //[HttpGet("GetReplantDropdowns")]
        //public async Task<ResponseObject<ReplantDropdownsViewModel>> GetReplantDropdowns()
        //{
        //    try
        //    {
        //        var dorpdowns = new ReplantDropdownsViewModel
        //        {
        //            PlantType = await GetDropDown(DropdownTypeEnum.PlantType.GetDescription(), false, false),
        //        };

        //        return ServiceResponse.SuccessReponse(dorpdowns, MessageEnum.DefaultSuccessMessage);
        //    }
        //    catch (Exception exp)
        //    {
        //        return ServiceResponse.ErrorReponse<ReplantDropdownsViewModel>(exp);
        //    }
        //}

        [Auth(UserRoleTypeEnum.Admin)]
        [HttpGet("GetAll")]
        public async Task<ResponseObject<List<Replant>>> GetAll(string id = null)
        {
            try
            {
                List<Users> lstUser = new List<Users>();

                if (string.IsNullOrEmpty(id))
                    lstUser = await _IUWork.GetAllSubDocuments<Users, Replant>(CollectionNames.USERS, CollectionNames.REPLANT);

                else
                {
                    lstUser = _IUWork.GetModelByUserID<Users>(id, CollectionNames.USERS);
                }

                List<Replant> reduceList = new List<Replant>();
                if (lstUser?.Count > 0)
                {
                    foreach (var item in lstUser)
                    {
                        if (item?.Replant?.Count > 0)
                        {
                            foreach (var replant in item?.Replant)
                            {
                                replant.UserId = item.Id.ToString();
                                reduceList.Add(replant);
                            }
                        }
                    }
                }
                reduceList = reduceList.ToSortByCreationDateDescendingOrder();
                return ServiceResponse.SuccessReponse(reduceList, MessageEnum.ComplaintGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Replant>>(exp);
            }
        }

        [Auth(UserRoleTypeEnum.Admin)]
        [HttpPost("UpdateStatus")]
        public async Task<ResponseObject<bool>> UpdateStatus([FromBody]ReplantViewModel mdlReplant)
        {
            if (mdlReplant == null)
                return ServiceResponse.ErrorReponse<bool>(MessageEnum.DefaultParametersCanNotBeNull);

            try
            {
                var update = Builders<Users>.Update.Set(CollectionNames.REPLANT + ".$.GreenPoints", mdlReplant.GreenPoints)
                                                   .Set(CollectionNames.REPLANT + ".$.UpdatedAt", DateTime.Now.ToString())
                                                   .Set(CollectionNames.REPLANT + ".$.Status", mdlReplant.Status)
                                                   .Set(CollectionNames.REPLANT + ".$.StatusDescription", ((StatusEnum)mdlReplant.Status).GetDescription());

                var result = await _IUWork.UpdateSubDocument<Users, Replant>(mdlReplant.Id.ToString(), update, CollectionNames.USERS, CollectionNames.REPLANT);
                
                return ServiceResponse.SuccessReponse(result, MessageEnum.ReplantStatusUpdatedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }


        [HttpPost("AddPlantTreeDropdown"), DisableRequestSizeLimit]
        public async Task<ResponseObject<bool>> AddPlantTreeDropdown(PlantTree tree)
        {
            try
            {
                if (tree == null) return ServiceResponse.ErrorReponse<bool>(MessageEnum.DefaultParametersCanNotBeNull);
                tree.FileName = await SaveFile(tree.File);
                await _IUWork.InsertOne<PlantTree>( tree, "PlantTree");
                return ServiceResponse.SuccessReponse(true, MessageEnum.ReplantAddedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }

        }


        [HttpGet("GetReplantDropdowns")]
        public async Task<ResponseObject<List<PlantTree>>> GetReplantDropdowns()
        {
            try
            {

                List<FilterHelper> filter = new List<FilterHelper>
                {
                    new FilterHelper
                    {
                        Field = "Type",
                        Value = "Tree"
                    }
                };

                List<PlantTree> list = _IUWork.GetModelData<PlantTree>(filter, "PlantTree");

                return ServiceResponse.SuccessReponse(list, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<PlantTree>>(exp);
            }
        }


    }
}