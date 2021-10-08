using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrTech.Common.Enums;
using DrTech.Common.ServerResponse;
using DrTech.DAL;
using DrTech.Models;
using DrTech.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static DrTech.Common.Extentions.Constants;

namespace DrTech.Services.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class NGONetworkController : BaseControllerBase
    {
        [HttpPost("AddNGO")]
        public async Task<ResponseObject<bool>> AddNGO(NGOViewModel mdlNGO)
        {
            if (mdlNGO == null)
                return ServiceResponse.ErrorReponse<bool>(MessageEnum.OrgModelNotNull);
            try
            {
                if (mdlNGO.NGOParentId == "")
                    mdlNGO.NGOParentId = "0";

                string FileName = await SaveFile(mdlNGO.File);

                NGO ngo = new NGO
                {
                    FileName = FileName,
                    Name = mdlNGO.Name,
                    Address = mdlNGO.Address,
                    Phone = mdlNGO.Phone,
                    Level = mdlNGO.Level,
                    IsActive = true,
                    NGOParentId = mdlNGO.NGOParentId
                };

                await _IUWork.InsertOne(ngo, CollectionNames.NGO);
                return ServiceResponse.SuccessReponse(true, MessageEnum.NGOAdded);

            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }


        [HttpGet("GetNGODropdowns")]
        public async Task<ResponseObject<List<NGO>>> GetNGODropdowns()
        {
            try
            {
                List<FilterHelper> filter = new List<FilterHelper>
                {
                    new FilterHelper
                    {
                        Field = "NGOParentId",
                        Value = "0"
                    }
                };

                List<NGO> dorpdowns = _IUWork.GetModelData<NGO>(filter, CollectionNames.NGO);

                return ServiceResponse.SuccessReponse(dorpdowns, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<NGO>>(exp);
            }
        }



        [HttpGet("GetNGOSubOfficesDropdown")]
        public async Task<ResponseObject<List<NGO>>> GetNGOSubOfficesDropdown(string id)
        {
            try
            {
                List<FilterHelper> filter = new List<FilterHelper>
                {
                    new FilterHelper
                    {
                        Field = "NGOParentId",
                        Value = id
                    }
                };

                List<NGO> dorpdowns = _IUWork.GetModelData<NGO>(filter, CollectionNames.NGO);


                NGO MainBranch = await _IUWork.FindOneByID<NGO>(id, CollectionNames.NGO);

                dorpdowns.Add(MainBranch);

                dorpdowns = dorpdowns.OrderBy(d => d.NGOParentId).ToList();

                return ServiceResponse.SuccessReponse(dorpdowns, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<NGO>>(exp);
            }
        }


        [HttpGet("FullTextSearch")]
        public ResponseObject<List<NGO>> FullTextSearch(string Query)
        {
            try
            {
                var dorpdowns = _IUWork.FullTextSearch<NGO>(Query, CollectionNames.NGO);

                return ServiceResponse.SuccessReponse(dorpdowns, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<NGO>>(exp);
            }
        }
    }
}