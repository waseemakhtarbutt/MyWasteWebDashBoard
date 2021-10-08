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
    public class OrganizationController : BaseControllerBase
    {
        [HttpPost("AddOrganizationInformation")]
        public async Task<ResponseObject<bool>> AddOrganizationInformation(Organization mdlOrganization)
        {
            if (mdlOrganization == null)
                return ServiceResponse.ErrorReponse<bool>(MessageEnum.OrgModelNotNull);
            try
            {
                if (mdlOrganization.OrgParentId == "")
                    mdlOrganization.OrgParentId = "0";

                string FileName = await SaveFile(mdlOrganization.File);

                Organization orgnization = new Organization
                {
                    FileName = string.IsNullOrEmpty(FileName) ? "" : FileName,
                    OrgnizationName = mdlOrganization.OrgnizationName,
                    OrgAddress = mdlOrganization.OrgAddress,
                    OrgPhone = mdlOrganization.OrgPhone,
                    OrgParentId = mdlOrganization.OrgParentId,
                    Value = mdlOrganization.Value,
                    Level = mdlOrganization.Level

                };

                await _IUWork.InsertOne(orgnization, CollectionNames.Organization);
                return ServiceResponse.SuccessReponse(true, MessageEnum.OrgAddedSuccessfully);

            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpGet("GetOrganizationDropdowns")]
        public async Task<ResponseObject<List<Organization>>> GetOrganizationDropdowns()
        {
            try
            {
                List<FilterHelper> filter = new List<FilterHelper>
                {
                    new FilterHelper
                    {
                        Field = "OrgParentId",
                        Value = "0"
                    }
                };

                List<Organization> dorpdowns = _IUWork.GetModelData<Organization>(filter, CollectionNames.Organization);

                //List<OrganizationViewModel> OrgViewModel = new List<OrganizationViewModel>();

                //OrganizationViewModel mdlOrg = new OrganizationViewModel();

                //foreach (var item in dorpdowns)
                //{
                //    mdlOrg.Name = item.OrgnizationName;
                //    mdlOrg.Address = item.OrgAddress;
                //    mdlOrg.Phone = item.OrgPhone;
                //    mdlOrg.Level = item.Level;
                //    mdlOrg.ParentId = item.OrgParentId;
                //    mdlOrg.GreenPoints = item.OrgGreenPoints;
                //    mdlOrg.FileName = item.FileName;
                //    mdlOrg.Id = item.Id.ToString();

                //    OrgViewModel.Add(mdlOrg);
                //}

                return ServiceResponse.SuccessReponse(dorpdowns, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Organization>>(exp);
            }
        }



        [HttpGet("GetSubOfficesDropdown")]
        public async Task<ResponseObject<List<Organization>>> GetSubOfficesDropdown(string id)
        {
            try
            {
                List<FilterHelper> filter = new List<FilterHelper>
                {
                    new FilterHelper
                    {
                        Field = "OrgParentId",
                        Value = id
                    }
                };

                List<Organization> dorpdowns = _IUWork.GetModelData<Organization>(filter, CollectionNames.Organization);

                Organization MainBranch = await _IUWork.FindOneByID<Organization>(id, CollectionNames.Organization);

                dorpdowns.Add(MainBranch);

                dorpdowns = dorpdowns.OrderBy(d => d.OrgParentId).ToList();


                return ServiceResponse.SuccessReponse(dorpdowns, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Organization>>(exp);
            }
        }


        [HttpGet("FullTextSearch")]
        public ResponseObject<List<Organization>> FullTextSearch(string Query)
        {
            try
            {
                var dorpdowns = _IUWork.FullTextSearch<Organization>(Query, CollectionNames.Organization);

                return ServiceResponse.SuccessReponse(dorpdowns, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Organization>>(exp);
            }
        }

    }
}