using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DrTech.DAL;
using Microsoft.AspNetCore.Mvc;
using DrTech.Models;
using DrTech.Models.ViewModels;
using static DrTech.Common.Extentions.Constants;
using DrTech.Common.Helpers;
using DrTech.Common.ServerResponse;
using DrTech.Common.Enums;
using Microsoft.AspNetCore.Authorization;
using DrTech.Common.Extentions;
using System.IO;
using DrTech.Models.Dropdown;
using MongoDB.Driver;
using MongoDB.Bson;
using DrTech.Services.Attribute;

namespace DrTech.Services.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class CommonController : BaseControllerBase
    {
        [Authorize]
        [HttpGet("GetStatusList")]
        public ResponseObject<List<DropdownViewModel>> GetStatusList()
        {
            try
            {
                var dorpdowns = new List<DropdownViewModel> {
                        new DropdownViewModel{
                            Description = StatusEnum.Submit.GetDescription(),
                            Value= (int) StatusEnum.Submit
                        },
                        new DropdownViewModel{
                            Description = StatusEnum.InProgress.GetDescription(),
                            Value= (int)StatusEnum.InProgress,
                        },
                        new DropdownViewModel{
                            Description =StatusEnum.Resolved.GetDescription(),
                            Value= (int)StatusEnum.Resolved
                        },
                        new DropdownViewModel{
                            Description =StatusEnum.Delivered.GetDescription(),
                            Value= (int)StatusEnum.Delivered
                        }
                };

                return ServiceResponse.SuccessReponse(dorpdowns, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<DropdownViewModel>>(exp);
            }
        }

        [Authorize]
        [HttpPost("AddDropdown")]
        public async Task<ResponseObject<bool>> AddDropdown([FromBody]List<DropdownDbViewModel> list)
        {
            try
            {
                foreach (var item in list)
                {
                    await _IUWork.InsertOne(item, CollectionNames.Lookups);
                }

                return ServiceResponse.SuccessReponse(true, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [Authorize]
        [HttpPost("AddSubTypeDropdown")]
        public async Task<ResponseObject<bool>> AddSubTypeDropdown([FromBody]List<SubTypeDropdownDbViewModel> list)
        {
            try
            {
                foreach (var item in list)
                {
                    var parentDocumentFilter = Builders<DropdownDbViewModel>.Filter.And(new BsonDocument { { "Value", item.ParentId } });
                    //await _IUWork.AddSubDocument<DropdownDbViewModel, DropdownDbViewModel>(list.Id, item, CollectionNames.Lookups, CollectionNames.Lookups);
                    await _IUWork.AddSubDocument(parentDocumentFilter, item, CollectionNames.Lookups, CollectionNames.SubTypeLookups);
                }

                return ServiceResponse.SuccessReponse(true, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
        [Authorize]
        [HttpGet("GetSubTypeDropdown")]
        public async Task<ResponseObject<DonationDropdownsViewModel>> GetSubTypeDropdown(string id)
        {
            try
            {
                var dorpdowns1 = await GetSubTypeDropDown(id, false, false);

                var dorpdowns = new DonationDropdownsViewModel
                {
                    DonationType = await GetSubTypeDropDown(id, false, false)

                };

                return ServiceResponse.SuccessReponse(dorpdowns, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<DonationDropdownsViewModel>(exp);
            }
        }


        [HttpGet("GetCityDropdowns")]
        public async Task<ResponseObject<DonationDropdownsViewModel>> GetCityDropdowns()
        {
            try
            {
                var dorpdowns = new DonationDropdownsViewModel
                {
                    CityList = await GetDropDown(DropdownTypeEnum.City.GetDescription(), false, false),
                };


                return ServiceResponse.SuccessReponse(dorpdowns, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<DonationDropdownsViewModel>(exp);
            }
        }


        [HttpGet("GetAmalDropdown")]
        public async Task<ResponseObject<AmalRsDropdownViewModel>> GetAmalDropdown(string Type)
        {
            try
            {
                var dorpdowns = new AmalRsDropdownViewModel
                {
                    AmalRs = await GetDropDown(Type, false, false),
                };

                return ServiceResponse.SuccessReponse(dorpdowns, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<AmalRsDropdownViewModel>(exp);
            }
        }


    }
}