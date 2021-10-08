using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrTech.Common.Enums;
using DrTech.Common.ServerResponse;
using DrTech.Models.Dropdown;
using DrTech.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DrTech.Common.Extentions;
using DrTech.Models;
using static DrTech.Common.Extentions.Constants;
using DrTech.Common.Helpers;
using DrTech.Notifications;

namespace DrTech.Services.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class SchoolsController : BaseControllerBase
    {

        [HttpPost("AddSchoolInformation")]
        public async Task<ResponseObject<bool>> AddSchoolInformation(SchoolViewModel mdlSchool)
        {
            if (mdlSchool == null)
                return ServiceResponse.ErrorReponse<bool>(MessageEnum.KidsModelNotNull);
            try
            {
                if (mdlSchool.ParentId == "")
                    mdlSchool.ParentId = "0";

                string FileName = await SaveFile(mdlSchool.File);

                Schools school = new Schools
                {
                    FileName = string.IsNullOrEmpty(FileName) ? "" : FileName,
                    Name = mdlSchool.Name,
                    Address = mdlSchool.Address,
                    Phone = mdlSchool.Phone,
                    ParentId = mdlSchool.ParentId,
                    Value = mdlSchool.Value,
                    Level = mdlSchool.Level,
                    Email = mdlSchool.Email,
                    Branch = mdlSchool.Branch,
                    ContactPerson  =mdlSchool.ContactPerson
                };
                await _IUWork.InsertOne(school, CollectionNames.SCHOOL);

                NotifyEvent _event = new NotifyEvent();
                _event.Parameters.Add("ContactPerson", mdlSchool.ContactPerson);
                _event.Parameters.Add("Name", mdlSchool.Name);
                _event.Parameters.Add("Branch", mdlSchool.Branch);
                _event.Parameters.Add("Email", mdlSchool.Email);
                _event.Parameters.Add("Phone", mdlSchool.Phone);
                _event.AddNotifyEvent((long)NotificationEventConstants.School.SendEmailToAdmin, GetLoggedInUserId());


                SMSNotifyEvent _events = new SMSNotifyEvent();
                _events.Parameters.Add("ContactPerson", mdlSchool.ContactPerson);
                _event.Parameters.Add("Phone", mdlSchool.Phone);
                _event.Parameters.Add("Name", mdlSchool.Name);
                _events.AddSMSNotifyEvent((long)NotificationEventConstants.School.SendSMSToUser, mdlSchool.Phone);


                return ServiceResponse.SuccessReponse(true, MessageEnum.SchoolAddedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpGet("GetSchoolsDropdowns")]
        public async Task<ResponseObject<List<Schools>>> GetSchoolsDropdowns()
        {
            try
            {
                var dorpdowns = await GetSchoolDropDown();                

                return ServiceResponse.SuccessReponse(dorpdowns, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Schools>>(exp);
            }
        }

      

        [HttpGet("GetSchoolBranchDropdown")]
        public async Task<ResponseObject<List<Schools>>> GetSchoolBranchDropdown(string id)
        {
            try
            {

                var dorpdowns = await GetSubTypeDropDown(id);
                return ServiceResponse.SuccessReponse(dorpdowns, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Schools>>(exp);
            }
        }


        [HttpGet("FullTextSearch")]
        public  ResponseObject<List<Schools>> FullTextSearch(string Query)
        {
            try
            {
                var dorpdowns =  _IUWork.FullTextSearch<Schools>(Query, CollectionNames.SCHOOL);

                return ServiceResponse.SuccessReponse(dorpdowns, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Schools>>(exp);
            }
        }

  

    }
}