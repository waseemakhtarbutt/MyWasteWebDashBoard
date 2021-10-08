using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrTech.Common.Enums;
using DrTech.Common.Extentions;
using DrTech.Common.ServerResponse;
using DrTech.DAL;
using DrTech.Models;
using DrTech.Models.Common;
using DrTech.Models.Dropdown;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using static DrTech.Common.Extentions.Constants;

namespace DrTech.Services.Controllers
{
    [Route("api/Ngo")]
    public class NgoController : BaseControllerBase
    {
        [HttpPost("AddNeed")]
        public async Task<ResponseObject<bool>> AddNeed([FromBody]Regift need)
        {
            try
            {
                need.NeedType = UserRoleTypeEnum.NGO.GetDescription();
                need.UserId = GetLoggedInUserId();
                //   await _IUWork.AddSubDocument<Users, Regift>(GetLoggedInUserId(), need, CollectionNames.USERS, CollectionNames.REGIFT);
                await _IUWork.InsertOne(need, CollectionNames.REGIFT);
                return ServiceResponse.SuccessReponse(true, MessageEnum.DonationAddedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
        [HttpPut("UpdateNeed")]
        public async Task<ResponseObject<bool>> UpdateNeed(string id, [FromBody]Regift need)
        {
            try
            {
                var update = Builders<Regift>.Update
                          .Set(o => o.Description, need.Description)
                          .Set(p => p.City, need.City)
                          .Set(g => g.CityDescription, need.CityDescription)
                          .Set(q => q.Type, need.Type)
                          .Set(r => r.TypeDescription, need.TypeDescription)
                          .Set(s => s.SubType, need.SubType)
                          .Set(t => t.SubTypeDescription, need.SubTypeDescription);

                bool result = _IUWork.UpdateStatus(id, update, CollectionNames.REGIFT);



                return ServiceResponse.SuccessReponse(result, MessageEnum.DonationAddedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
        [HttpGet("GetNeedList")]
        public async Task<ResponseObject<List<Regift>>> GetNeedList()
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

                List<Regift> LstRegiftItems = _IUWork.GetModelData<Regift>(filter, CollectionNames.REGIFT);

                var user = _IUWork.FindOneByID<Users>(GetLoggedInUserId(), CollectionNames.USERS).Result;

                if (LstRegiftItems.Count > 0)
                {
                    var list = LstRegiftItems.ToSortByCreationDateDescendingOrder();
                    return ServiceResponse.SuccessReponse(list, MessageEnum.DonationFoundSuccessfully);
                }
                else
                {
                    return ServiceResponse.SuccessReponse(LstRegiftItems, MessageEnum.DonationNotFound);
                }
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Regift>>(exp);
            }
        }

        //[HttpGet("GetAllNeedList")]
        //public async Task<ResponseObject<List<Regift>>> GetAllNeedList()
        //{
        //    try
        //    {
        //        List<Regift> LstRpt = _IUWork.GetModelData<Regift>(CollectionNames.REGIFT);

        //        foreach (var item in LstRpt)
        //        {
        //            var filter = new List<FilterHelper>
        //                        {
        //                              new FilterHelper
        //                                {
        //                                    Field = "Value",
        //                                    Value = item.SubType.ToString()
        //                                }
        //                        };

        //            var user = await _IUWork.FindOneByID<Users>(item.UserId, CollectionNames.USERS);

        //            if (user != null)
        //            {
        //                item.DonateToDescription = user.FullName;
        //            }

        //            var regift = _IUWork.GetModelData<DropdownDbViewModel>(filter, CollectionNames.Lookups)?.FirstOrDefault();
        //            item.SubTypeTitle = EnumExtensionMethod.GetTitle<DropdownTypeEnum>(regift?.Type);
        //        }

        //        LstRpt = LstRpt?.ToSortByCreationDateDescendingOrder();

        //        return ServiceResponse.SuccessReponse(LstRpt, MessageEnum.ComplaintGetSuccess);
        //    }
        //    catch (Exception exp)
        //    {
        //        return ServiceResponse.ErrorReponse<List<Regift>>(exp);
        //    }
        //}

        [HttpGet("GetAllNeedList")]
        public async Task<ResponseObject<List<Regift>>> GetAllNeedList()
        {
            try
            {
                List<FilterHelper> filter = new List<FilterHelper>
                {
                    new FilterHelper
                    {
                        Field = "NeedType",
                        Value = "NGO"
                    }
                };

                List<Regift> LstRegiftItems = _IUWork.GetModelData<Regift>(filter, CollectionNames.REGIFT);



                foreach (var item in LstRegiftItems)
                {
                    var filters = new List<FilterHelper>
                                        {
                                              new FilterHelper
                                                {
                                                    Field = "Value",
                                                    Value = item.SubType.ToString()
                                                }
                                        };


                    if (item.UserId != "" && item.UserId != null)
                    {
                     var  user = await _IUWork.FindOneByID<Users>(item.UserId, CollectionNames.USERS);
                    
                    if (user != null)
                    {
                        item.DonateToDescription = user.FullName;
                    }
                    }
                    var regift = _IUWork.GetModelData<DropdownDbViewModel>(filters, CollectionNames.Lookups)?.FirstOrDefault();
                    item.SubTypeTitle = EnumExtensionMethod.GetTitle<DropdownTypeEnum>(regift?.Type);
                }


                return ServiceResponse.SuccessReponse(LstRegiftItems, MessageEnum.ComplaintGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Regift>>(exp);
            }
        }

    }
}