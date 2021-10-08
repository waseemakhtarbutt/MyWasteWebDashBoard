using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrTech.Common.Enums;
using DrTech.Common.Helpers;
using DrTech.Common.ServerResponse;
using DrTech.DAL;
using DrTech.Models;
using DrTech.Models.ViewModels;
using DrTech.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using static DrTech.Common.Extentions.Constants;

namespace DrTech.Services.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ChildController : BaseControllerBase
    {

        [HttpPost("AddKidsInformation")]
        public async Task<ResponseObject<bool>> AddKidsInformation(ChildViewModel mdlChildren)
        {
            if (mdlChildren == null)
                return ServiceResponse.ErrorReponse<bool>(MessageEnum.KidsModelNotNull);
            try
            {
                string FileName = await SaveFile(mdlChildren.File);

                Child children = new Child
                {
                    FileName = string.IsNullOrEmpty(FileName) ? "" : FileName,
                    Name = mdlChildren.Name,
                    SchoolId = mdlChildren.SchoolId,
                    SectionName = mdlChildren.SectionName,
                    ClassName = mdlChildren.ClassName,
                    RollNo = mdlChildren.RollNo,
                    UserId = GetLoggedInUserId(),
                    Gender = mdlChildren.Gender,
                    IsVerified = false,
                };

                await _IUWork.AddSubDocument<Users, Child>(GetLoggedInUserId(), children, CollectionNames.USERS, CollectionNames.Child);

                var school = _IUWork.FindOneByID<Schools>(children.SchoolId, CollectionNames.SCHOOL).Result;

                if (school != null)
                {
                NotifyEvent _event = new NotifyEvent();
                _event.Parameters.Add("ChildName", children.Name);
                _event.Parameters.Add("Gender", children.Gender);
                _event.Parameters.Add("RollNo", children.RollNo);
                _event.Parameters.Add("SchoolName", school.Name);
                    _event.Parameters.Add("Branch", school.Branch);
                _event.AddNotifyEvent((long)NotificationEventConstants.Children.SendEmailToAdmin, GetLoggedInUserId());


                SMSNotifyEvent _events = new SMSNotifyEvent();
                _events.Parameters.Add("ChildName", children.Name);
                _events.Parameters.Add("SchoolName", school.Name);
                _events.AddSMSNotifyEvent((long)NotificationEventConstants.Children.SendSMSToUser, GetLoggedInUserId());

                }
                return ServiceResponse.SuccessReponse(true, MessageEnum.KidsAddedSuccessfully);

            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }


        [HttpGet("GetPendingKidsList")]
        public async Task<ResponseObject<List<Child>>> GetPendingKidsList(string SchoolId)
        {
            try
            {
                List<FilterHelper> filter = new List<FilterHelper>
                {
                    new FilterHelper
                    {
                        Field = "SchoolId",
                        Value = SchoolId
                    },
                        new FilterHelper
                        {
                            Field = "IsVerified",
                            Value = "false"
                        }
                };

                List<Child> lstPendingRequest = _IUWork.GetModelData<Child>(filter, CollectionNames.Child);

                if (lstPendingRequest.Count == 0)
                    return ServiceResponse.SuccessReponse(lstPendingRequest, MessageEnum.ChildNotFound);

                return ServiceResponse.SuccessReponse(lstPendingRequest, MessageEnum.ChildFoundSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Child>>(exp);
            }
        }


        [HttpGet("GetChildrenByUserId")]
        public async Task<ResponseObject<List<Child>>> GetChildrenByUserId()
        {
            try
            {
                var user = _IUWork.FindOneByID<Users>(GetLoggedInUserId(), CollectionNames.USERS).Result;

                if (user.children.Count == 0)
                    return ServiceResponse.SuccessReponse(user.children.ToList(), MessageEnum.ChildNotFound);

                return ServiceResponse.SuccessReponse(user.children.ToList(), MessageEnum.ChildFoundSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Child>>(exp);
            }
        }


        //[HttpPost("ApprovalRejectionKidsBySchool")]
        //public async Task<ResponseObject<bool>> ApprovalRejectionKidsBySchool([FromBody]Child mdlKids)
        //{
        //    if (mdlKids == null)
        //        return ServiceResponse.ErrorReponse<bool>(MessageEnum.KidsModelNotNull);

        //    if (mdlKids.Id == null)
        //        return ServiceResponse.ErrorReponse<bool>(MessageEnum.KidsIDCannotbeNull);


        //    try
        //    {
        //       var update = Builders<Child>.Update
        //                                        .Set(o => o.IsVerified, mdlKids.IsVerified);

        //        bool IsUpdated = _IUWork.UpdateStatus(mdlKids.Id, update, CollectionNames.KIDS);
        //        return ServiceResponse.SuccessReponse(true, MessageEnum.KidsInfoUpdated);

        //    }
        //    catch (Exception exp)
        //    {
        //        return ServiceResponse.ErrorReponse<bool>(exp);
        //    }
        //}



    }
}