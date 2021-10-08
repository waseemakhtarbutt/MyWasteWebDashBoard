using DrTech.Amal.Common.Enums;
using DrTech.Amal.Common.ServerResponse;
using DrTech.Amal.SQLDataAccess;
using DrTech.Amal.SQLDataAccess.CustomModels;
using DrTech.Amal.SQLDataAccess.Repository;
using DrTech.Amal.SQLModels;
using DrTech.Amal.SQLServices.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DrTech.Amal.SQLServices.Controllers
{
    //[Authorize]
    public class CommonController : BaseController
    {

        [HttpGet]
        public async Task<ResponseObject<List<LookupType>>> GetDropdownByType(string TypeName)
        {
            try
            {
                List<LookupType> lstLookupType = db.ExtRepositoryFor<CommonRepository>().GetLookupByTypeName(TypeName).ToList();

                return ServiceResponse.SuccessReponse(lstLookupType, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<LookupType>>(exp);
            }
        }


        

        [HttpGet]
        public async Task<ResponseObject<List<LookupType>>> GetDropdownByParentID(int ParentID)
        {
            try
            {
                List<LookupType> lstLookupType = db.ExtRepositoryFor<CommonRepository>().GetTypeByParentID(ParentID).ToList();

                return ServiceResponse.SuccessReponse(lstLookupType, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<LookupType>>(exp);
            }
        }

        [HttpGet]
        public async Task<ResponseObject<List<object>>> GetStatusList()
        {
            try
            {
                var dorpdowns = new List<object> {
                        new {
                            Description = StatusEnum.Submit.ToString(),
                            Value= (int) StatusEnum.Submit
                        },
                        new {
                            Description = StatusEnum.InProgress.ToString(),
                            Value= (int)StatusEnum.InProgress,
                        },
                        new {
                            Description =StatusEnum.Resolved.ToString(),
                            Value= (int)StatusEnum.Resolved
                        },
                        new {
                            Description =StatusEnum.Delivered.ToString(),
                            Value= (int)StatusEnum.Delivered
                        }
                };

                return ServiceResponse.SuccessReponse(dorpdowns, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }

        [HttpGet]
        public async Task<ResponseObject<List<object>>> GetAllStatuses()
        {
            try
            {
                var dorpdowns = db.ExtRepositoryFor<CommonRepository>().GetStatusessList();
                return ServiceResponse.SuccessReponse(dorpdowns, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }

        [HttpPost]
        public async Task<ResponseObject<List<object>>> GetAllStatuses([FromBody]List<string> statuses)
        {
            try
            {
                var dorpdowns = db.ExtRepositoryFor<CommonRepository>().GetStatusessList(statuses);
                return ServiceResponse.SuccessReponse(dorpdowns, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }

        [HttpGet]
        public async Task<ResponseObject<List<object>>> GetAllStatusesList()
        {
            try
            {
                var dorpdowns = db.Repository<Status>().GetAll().Where(x => x.StatusName == "Declined" || x.StatusName == "Resolved" || x.StatusName=="Submitted" || x.StatusName== "Collected" || x.StatusName == "No Show" || x.StatusName == "Pending").ToList<object>();

                return ServiceResponse.SuccessReponse(dorpdowns, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }


        [HttpGet]
        public async Task<ResponseObject<object>> GetDefaultGreenPoints()
        {
            try
            {
                var defaultGreenPoints = db.Repository<RefTable>().GetAll().Where(x=>x.ToDate ==null).FirstOrDefault<object>();

                return ServiceResponse.SuccessReponse(defaultGreenPoints, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }
        [HttpGet]
        public async Task<ResponseObject<bool>> DeAssociateUserFromGPN(int Id,string Type)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                bool IsSuccess = db.ExtRepositoryFor<CommonRepository>().DeAssociateUser(UserID, Id,Type);

                return ServiceResponse.SuccessReponse(IsSuccess, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpGet]
        public async Task<ResponseObject<ArsRequestCount>> GetArsRequestCount()
        {
            try
            {
                var ArsRequestCounts = db.ExtRepositoryFor<CommonRepository>().RequestCounts();

                return ServiceResponse.SuccessReponse(ArsRequestCounts, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<ArsRequestCount> (exp);
            }
        }
    }
}
