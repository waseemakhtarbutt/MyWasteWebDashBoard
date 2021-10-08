using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrTech.Common.Enums;
using DrTech.Common.Helpers;
using DrTech.Common.ServerResponse;
using DrTech.DAL;
using DrTech.Models;
using DrTech.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static DrTech.Common.Extentions.Constants;

namespace DrTech.Services.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class NGOMembersController : BaseControllerBase
    {
        [HttpPost("AddNOGEmployeeInformation")]
        public async Task<ResponseObject<bool>> AddNOGEmployeeInformation(Members mdlEmp)
        {
            if (mdlEmp == null)
                return ServiceResponse.ErrorReponse<bool>(MessageEnum.KidsModelNotNull);
            try
            {

                Members member = new Members
                {
                    NGOId = mdlEmp.NGOId,
                    Designation = mdlEmp.Designation,
                    Department = mdlEmp.Department,
                    EmployeeID = mdlEmp.EmployeeID,
                    FromDate = Convert.ToDateTime(mdlEmp.FromDate),
                    ToDate = Convert.ToDateTime(mdlEmp.ToDate),
                    IsCurrentlyWorking = mdlEmp.IsCurrentlyWorking,
                    IsVerified = false,
                };

                await _IUWork.AddSubDocument<Users, Members>(GetLoggedInUserId(), member, CollectionNames.USERS, CollectionNames.MEMBERS);

                var org = _IUWork.FindOneByID<NGO>(mdlEmp.NGOId, CollectionNames.NGO).Result;

                if (org != null)
                {

                    NotifyEvent _event = new NotifyEvent();
                    _event.Parameters.Add("NGOName", org.Name);
                    _event.Parameters.Add("EmployeeID", mdlEmp.EmployeeID);
                    _event.AddNotifyEvent((long)NotificationEventConstants.NGO.SendEmailToAdmin, GetLoggedInUserId());


                    SMSNotifyEvent _events = new SMSNotifyEvent();
                    _events.Parameters.Add("NGOName", org.Name);
                    _events.AddSMSNotifyEvent((long)NotificationEventConstants.NGO.SendSMSToUser, GetLoggedInUserId());

                }

                return ServiceResponse.SuccessReponse(true, MessageEnum.EmpAddedSuccessfully);

            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }


        [HttpGet("GetNGOPendingEmployeeList")]
        public ResponseObject<List<Members>> GetNGOPendingEmployeeList(string NGOId)
        {
            try
            {
                List<FilterHelper> filter = new List<FilterHelper>
                {
                    new FilterHelper
                    {
                        Field = "OrgnizationId",
                        Value = NGOId
                    },
                        new FilterHelper
                        {
                            Field = "IsVerified",
                            Value = "false"
                        }

                };

                List<Members> lstPendingRequest = _IUWork.GetModelData<Members>(filter, CollectionNames.MEMBERS);

                if (lstPendingRequest.Count == 0)
                    return ServiceResponse.SuccessReponse(lstPendingRequest, MessageEnum.EmpNotFound);

                return ServiceResponse.SuccessReponse(lstPendingRequest, MessageEnum.EmpFoundSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Members>>(exp);
            }
        }


        [HttpGet("GetNGOEmployeeInformationByUserId")]
        public ResponseObject<List<Members>> GetNGOEmployeeInformationByUserId()
        {
            try
            {
                var user = _IUWork.FindOneByID<Users>(GetLoggedInUserId(), CollectionNames.USERS).Result;

                if (user.Memberships.Count == 0)
                    return ServiceResponse.SuccessReponse(user.Memberships.ToList(), MessageEnum.EmpNotFound);

                return ServiceResponse.SuccessReponse(user.Memberships.ToList(), MessageEnum.EmpFoundSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Members>>(exp);
            }
        }
    }
}