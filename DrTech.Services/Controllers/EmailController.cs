using System;
using System.Collections.Generic;
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
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using static DrTech.Common.Extentions.Constants;

namespace DrTech.Services.Controllers
{
    [Auth(UserRoleTypeEnum.Admin)]
    [Route("api/[controller]")]
    public class EmailController : BaseControllerBase
    {

        private List<UserEmailViewModel> GetEmails(int status)
        {
            List<FilterHelper> filter = new List<FilterHelper>
                {
                    new FilterHelper
                    {
                        Field = "Status",
                        Value = "" +  status
                    }
                };


            List<EmailNotification> emailList = _IUWork.GetModelData<EmailNotification>(filter, CollectionNames.EmailNotification);
            emailList = emailList?.ToSortByCreationDateDescendingOrder();

            string adminEmail = AppSettingsHelper.GetAttributeValue(AppSettings.EMAIL_SECTION, AppSettings.EMAIL_SECTION_TO_EMAIL);

            var result = emailList.ConvertAll(p => new UserEmailViewModel
            {
                EmailBody = p.EmailBody,
                EmailCC = p.EmailCC,
                EmailSubject = p.EmailSubject,
                EmailTo = p.EmailTo,
                Id = p.Id.ToString(),
                ServerMessage = p.ServerMessage,
                ReceiverUserType = p.EmailTo == adminEmail ? "Admin" : "End User",
                Status = p.Status != 0 ? "Sent" : "Not Sent",
            });

            return result;
        }

        [HttpGet("GetSentEmailList")]
        public async Task<ResponseObject<List<UserEmailViewModel>>> GetSentEmailList()
        {
            try
            {
                var result = GetEmails(1);
                return ServiceResponse.SuccessReponse(result, MessageEnum.EmailListFound);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<UserEmailViewModel>>(exp);
            }
        }
        [HttpGet("GetNotSentEmailList")]
        public async Task<ResponseObject<List<UserEmailViewModel>>> GetNotSentEmailList()
        {
            try
            {
                var result = GetEmails(0);
                return ServiceResponse.SuccessReponse(result, MessageEnum.EmailListFound);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<UserEmailViewModel>>(exp);
            }
        }

        [HttpPost("SendEmail")]
        public async Task<ResponseObject<bool>> SendEmail([FromBody]UserEmailViewModel model)
        {
            try
            {
                if (model == null
                    || string.IsNullOrEmpty(model.EmailTo)
                    || string.IsNullOrEmpty(model.EmailSubject)
                    || string.IsNullOrEmpty(model.EmailBody))

                    return ServiceResponse.ErrorReponse<bool>(MessageEnum.EmailDataIsNotValid);

                EmailHelper.SendEmail(model.EmailSubject, model.EmailBody, model.EmailTo);

                var update = Builders<EmailNotification>.Update
                              .Set(o => o.Status, 1);
                var status = _IUWork.UpdateStatus(model.Id, update, CollectionNames.EmailNotification);

                return ServiceResponse.SuccessReponse(status, MessageEnum.EmailListFound);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
    }
}