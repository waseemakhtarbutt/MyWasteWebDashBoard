using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrTech.Common.Enums;
using DrTech.Common.ServerResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DrTech.Models;
using static DrTech.Common.Extentions.Constants;
using DrTech.DAL;
using DrTech.Common.Helpers;
using DrTech.Notifications;

namespace DrTech.Services.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class EmployeeController : BaseControllerBase
    {
        [HttpPost("AddEmployeeInformation")]
        public async Task<ResponseObject<bool>> AddEmployeeInformation(Employment mdlEmp)
        {
            if (mdlEmp == null)
                return ServiceResponse.ErrorReponse<bool>(MessageEnum.KidsModelNotNull);
            try
            {

                Employment emp = new Employment
                {
                    OrgnizationId = mdlEmp.OrgnizationId,
                    Designation = mdlEmp.Designation,
                    Department = mdlEmp.Department,
                    EmployeeID = mdlEmp.EmployeeID,
                    FromDate  = Convert.ToDateTime(mdlEmp.FromDate),
                    ToDate = Convert.ToDateTime(mdlEmp.ToDate),
                    IsCurrentlyWorking = mdlEmp.IsCurrentlyWorking,
                    IsVerified = false,
                };

                await _IUWork.AddSubDocument<Users, Employment>(GetLoggedInUserId(), emp, CollectionNames.USERS, CollectionNames.Employee);


               var org = _IUWork.FindOneByID<Organization>(mdlEmp.OrgnizationId, CollectionNames.Organization).Result;

                if (org != null)
                {
                
                NotifyEvent _event = new NotifyEvent();
                _event.Parameters.Add("OrgName", org.OrgnizationName);
                _event.Parameters.Add("EmployeeID", mdlEmp.EmployeeID);
                _event.AddNotifyEvent((long)NotificationEventConstants.Organization.SendEmailToAdmin, GetLoggedInUserId());


                SMSNotifyEvent _events = new SMSNotifyEvent();
                _events.Parameters.Add("OrgName", org.OrgnizationName);
                _events.AddSMSNotifyEvent((long)NotificationEventConstants.Organization.SendSMSToUser, GetLoggedInUserId());

                }
                return ServiceResponse.SuccessReponse(true, MessageEnum.EmpAddedSuccessfully);

            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }


        [HttpGet("GetPendingEmployeeList")]
        public ResponseObject<List<Employment>> GetPendingEmployeeList(string OrgId)
        {
            try
            {
                List<FilterHelper> filter = new List<FilterHelper>
                {
                    new FilterHelper
                    {
                        Field = "OrgnizationId",
                        Value = OrgId
                    },
                        new FilterHelper
                        {
                            Field = "IsVerified",
                            Value = "false"
                        }

                };

                List<Employment> lstPendingRequest = _IUWork.GetModelData<Employment>(filter, CollectionNames.Employee);

                if (lstPendingRequest.Count == 0)
                    return ServiceResponse.SuccessReponse(lstPendingRequest, MessageEnum.EmpNotFound);

                return ServiceResponse.SuccessReponse(lstPendingRequest, MessageEnum.EmpFoundSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Employment>>(exp);
            }
        }


        [HttpGet("GetEmployeeInformationByUserId")]
        public  ResponseObject<List<Employment>> GetEmployeeInformationByUserId()
        {
            try
            {
                var user =  _IUWork.FindOneByID<Users>(GetLoggedInUserId(), CollectionNames.USERS).Result;

                if (user.Employments.Count == 0)
                    return ServiceResponse.SuccessReponse(user.Employments.ToList(), MessageEnum.EmpNotFound);

                return ServiceResponse.SuccessReponse(user.Employments.ToList(), MessageEnum.EmpFoundSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Employment>>(exp);
            }
        }
    }
}