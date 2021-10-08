using DrTech.Amal.Common.Enums;
using DrTech.Amal.Common.Helpers;
using DrTech.Amal.Common.ServerResponse;
using DrTech.Amal.SQLModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DrTech.Amal.SQLServices.Controllers
{
    [Authorize]
    public class EmailController : BaseController
    {
        //private List<UserEmailViewModel> GetEmails(int status)
        //{
        //    List<FilterHelper> filter = new List<FilterHelper>
        //        {
        //            new FilterHelper
        //            {
        //                Field = "Status",
        //                Value = "" +  status
        //            }
        //        };


        //    List<EmailNotification> emailList = _IUWork.GetModelData<EmailNotification>(filter, CollectionNames.EmailNotification);
        //    emailList = emailList?.ToSortByCreationDateDescendingOrder();

        //    string adminEmail = AppSettingsHelper.GetAttributeValue(AppSettings.EMAIL_SECTION, AppSettings.EMAIL_SECTION_TO_EMAIL);

        //    var result = emailList.ConvertAll(p => new UserEmailViewModel
        //    {
        //        EmailBody = p.EmailBody,
        //        EmailCC = p.EmailCC,
        //        EmailSubject = p.EmailSubject,
        //        EmailTo = p.EmailTo,
        //        Id = p.Id.ToString(),
        //        ServerMessage = p.ServerMessage,
        //        ReceiverUserType = p.EmailTo == adminEmail ? "Admin" : "End User",
        //        Status = p.Status != 0 ? "Sent" : "Not Sent",
        //    });

        //    return result;
        //}

        

        [HttpGet]
        public async Task<ResponseObject<List<EmailNotification>>> GetSentEmailList()
        {
            try
            {
                var result = db.Repository<EmailNotification>().GetAll().Where(x => x.Status == 1).OrderByDescending(x=>x.ID).ToList();
                return ServiceResponse.SuccessReponse(result, MessageEnum.EmailListFound);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<EmailNotification>>(exp);
            }
        }
        [HttpGet]
        public async Task<ResponseObject<EmailNotification>> GetEmailById(int Id)
        {
            try
            {
                var result = db.Repository<EmailNotification>().GetAll().Where(x => x.ID == Id).FirstOrDefault();
                return ServiceResponse.SuccessReponse(result, MessageEnum.EmailListFound);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<EmailNotification>(exp);
            }
        }
        [HttpGet]
        public async Task<ResponseObject<List<EmailNotification>>> GetNotSentEmailList()
        {
            try
            {
                var result = db.Repository<EmailNotification>().GetAll().Where(x => x.Status == 0).ToList();
                return ServiceResponse.SuccessReponse(result, MessageEnum.EmailListFound);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<EmailNotification>>(exp);
            }
        }

        [HttpPost]
        public async Task<ResponseObject<bool>> SendEmail([FromBody]EmailNotification model)
        {
            try
            {
                if (model == null
                    || string.IsNullOrEmpty(model.EmailTo)
                    || string.IsNullOrEmpty(model.EmailSubject)
                    || string.IsNullOrEmpty(model.EmailBody))

                    return ServiceResponse.ErrorReponse<bool>(MessageEnum.EmailDataIsNotValid);

                EmailHelper.SendEmail(model.EmailSubject, model.EmailBody, model.EmailTo);
                model.Status = 1;
                db.Repository<EmailNotification>().Update(model);
                db.Save();
                return ServiceResponse.SuccessReponse(true, MessageEnum.EmailListFound);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }


        [HttpGet]
        public async Task<ResponseObject<bool>> SendEmails()
        {
            try
            {


                List<EmailNotification> lstEmailNotifications = db.Repository<EmailNotification>().GetAll().Where(x => x.Status == 0).ToList();
                foreach (EmailNotification mdlEmail in lstEmailNotifications)
                {
                    if (mdlEmail == null)
                        continue;

                    if (mdlEmail.EmailTo == string.Empty)
                        //{
                        //return ServiceResponse.SuccessReponse(false, "");
                        continue;
                    //}
                    if (!mdlEmail.EmailTo.Contains("@"))
                        continue;

                    //else

                    EmailHelper.SendEmail(mdlEmail.EmailSubject, mdlEmail.EmailBody, mdlEmail.EmailTo);
                    mdlEmail.Status = 1;
                    db.Repository<EmailNotification>().Update(mdlEmail);
                    db.Save();

                }

                return ServiceResponse.SuccessReponse(true, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ResponseObject<bool>> SendTemplateEmails(string Name ,string htmlBody,string Email)
        {
            try
            {
                //string FilePath = "C:\\Users\\DrTech\\Downloads\\MY HTML\\My-Waste.html";
                //StreamReader str = new StreamReader(FilePath);
                //string MailText = str.ReadToEnd();
                //str.Close();
                //string htmlBody;


                //EmailHelper.SendEmailByTemplate("Nabeel Islam ", htmlBody, "nabeel.islam@drtechpk.com","");
                return ServiceResponse.SuccessReponse(true, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

    }
}
