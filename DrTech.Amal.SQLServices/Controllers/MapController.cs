using DrTech.Amal.Common.Enums;
using DrTech.Amal.Common.ServerResponse;
using DrTech.Amal.SQLDataAccess;
using DrTech.Amal.SQLDataAccess.Repository;
using DrTech.Amal.SQLModels;
using DrTech.Amal.Common.Enums;
using DrTech.Amal.Common.Helpers;
using DrTech.Amal.Common.ServerResponse;
using DrTech.Amal.Notifications;
using DrTech.Amal.SQLModels;
using DrTech.Amal.SQLServices.Auth;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Http;
using System.Collections.Generic;
using System.Web.Http.Cors;

namespace DrTech.Amal.SQLServices.Controllers
{
    public class MapController : BaseController
    {
        private object jwtdecoder;    
        
        [HttpGet]
        public ResponseObject<List<object>> getuserMappoints(string type)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                int? RoleID = JwtDecoder.GetUserRoleFromToken(Request.Headers.Authorization.Parameter);
                //List<int> lstChildUserID;
                //List<int> lstOrgUserID;
                //List<int> lstBusUserID;
                List<int> allUserID = new List<int>();
                int roleID = Convert.ToInt32(UserRoleTypeEnum.Admin);



                #region Admin Type Check
                //if (RoleID == Convert.ToInt32(UserRoleTypeEnum.SubSchoolAdmin))
                //{
                //    lstChildUserID = new List<int>();
                //    lstChildUserID = db.Repository<Child>().GetAll().Where(x => (x.School.UserID == UserID || RoleID == roleID)).Select(x => x.UserID).ToList();
                //    allUserID.AddRange(lstChildUserID);
                //}
                //else if (RoleID == Convert.ToInt32(UserRoleTypeEnum.SubOrganizationAdmin))
                //{
                //    lstOrgUserID = new List<int>();
                //    lstOrgUserID = db.Repository<Member>().GetAll().Where(x => (x.Organization.UserID == UserID || RoleID == roleID)).Select(x => x.UserID).ToList();
                //    allUserID.AddRange(lstOrgUserID);
                //}
                //else if (RoleID == Convert.ToInt32(UserRoleTypeEnum.SubBusinessAdmin))
                //{
                //    lstBusUserID = new List<int>();
                //    lstBusUserID = db.Repository<Employment>().GetAll().Where(x => (x.Business.UserID == UserID || RoleID == roleID)).Select(x => x.UserID).ToList();
                //    allUserID.AddRange(lstBusUserID);
                //}
                //else
                //{
                //    lstChildUserID = new List<int>();
                //    lstOrgUserID = new List<int>();
                //    lstBusUserID = new List<int>();
                //    lstChildUserID = db.Repository<Child>().GetAll().Where(x => (x.School.UserID == UserID || RoleID == roleID)).Select(x => x.UserID).ToList();
                //    lstOrgUserID = db.Repository<Member>().GetAll().Where(x => (x.Organization.UserID == UserID || RoleID == roleID)).Select(x => x.UserID).ToList();
                //    lstBusUserID = db.Repository<Employment>().GetAll().Where(x => (x.Business.UserID == UserID || RoleID == roleID)).Select(x => x.UserID).ToList();
                //    allUserID = new List<int>();
                //    allUserID.AddRange(lstChildUserID);
                //    allUserID.AddRange(lstOrgUserID);
                //    allUserID.AddRange(lstBusUserID);
                //}
                #endregion

                #region Request Type

                switch (type)
                {
                    case "refuse":
                        #region refuse
                        List<object> lstRefuse = db.Repository<Refuse>()
                     .GetAll()
                     //.Where(x => allUserID.Contains(x.UserID) )
                     .Select(refuse => new
                     {
                         pinImage = "refuse.png",
                         latitude = refuse.Latitude,
                         longitude = refuse.Longitude,
                         type = "refuse",
                         label = "refuse",
                         cash = 0,
                         greenPoints = refuse.GreenPoints,
                         fileName = refuse.FileName,
                         description = refuse.Idea,
                         name = refuse.User.FullName
                     }).ToList<object>();
                        return lstRefuse.Count > 0 ? ServiceResponse.SuccessReponse(lstRefuse, MessageEnum.DefaultSuccessMessage) : ServiceResponse.SuccessReponse(new List<object>(), MessageEnum.MapPointsNotFound);
                        
                    #endregion
                    case "reduce":
                        #region reduce
                        List<object> lstReduce = db.Repository<Reduce>()
                    .GetAll()
                    //.Where(x => allUserID.Contains(x.UserID) )
                    .Select(reduce => new
                    {
                        pinImage = "reduce.png",
                        latitude = reduce.User.Latitude,
                        longitude = reduce.User.Longitude,
                        type = "reduce",
                        label = "reduce",
                        cash = 0,
                        greenPoints = reduce.GreenPoints,
                        fileName = reduce.FileName,
                        description = reduce.Idea,
                        name = reduce.User.FullName
                    }).ToList<object>();
                        return lstReduce.Count > 0 ? ServiceResponse.SuccessReponse(lstReduce, MessageEnum.DefaultSuccessMessage) : ServiceResponse.SuccessReponse(new List<object>(), MessageEnum.MapPointsNotFound);
                       
                    #endregion
                    case "recycle":
                        #region recycle
                        List<object> lstRecycle = db.Repository<Recycle>()
                    .GetAll()
                    //.Where(x => allUserID.Contains(x.UserID))
                    .Select(recycle => new
                    {
                        pinImage = "recycle.png",
                        latitude = recycle.User.Latitude,
                        longitude = recycle.User.Longitude,
                        type = "recycle",
                        label = "recycle",
                        cash = 0,
                        greenPoints = recycle.GreenPoints,
                        fileName = recycle.FileName,
                        description = "",
                        name = recycle.User.FullName
                    }).ToList<object>();
                        return lstRecycle.Count > 0 ? ServiceResponse.SuccessReponse(lstRecycle, MessageEnum.DefaultSuccessMessage) : ServiceResponse.SuccessReponse(new List<object>(), MessageEnum.MapPointsNotFound);
                      
                    #endregion
                    case "regift":
                        #region regift
                        List<object> lstRegift = db.Repository<Regift>()
                    .GetAll()
                   // .Where(x => allUserID.Contains(x.UserID) )
                    .Select(regift => new
                    {
                        pinImage = "regift.png",
                        latitude = regift.Latitude,
                        longitude = regift.Longitude,
                        type = "regift",
                        label = "regift",
                        cash = 0,
                        greenPoints = regift.GreenPoints,
                        fileName = regift.FileName,
                        description = regift.Description,
                        name = regift.User.FullName
                    }).ToList<object>();
                        return lstRegift.Count > 0 ? ServiceResponse.SuccessReponse(lstRegift, MessageEnum.DefaultSuccessMessage) : ServiceResponse.SuccessReponse(new List<object>(), MessageEnum.MapPointsNotFound);
                      
                    #endregion
                    case "replant":
                        #region replant
                        List<object> lstReplant = db.Repository<Replant>()
                    .GetAll()
                   // .Where(x => allUserID.Contains(x.UserID) )
                    .Select(replant => new
                    {
                        pinImage = "replant.png",
                        Latitude = replant.Latitude,
                        Longitude = replant.Longitude,
                        Type = "Replant",
                        Label = "replant",
                        Cash = 0,
                        greenPoints = replant.GreenPoints,
                        fileName = replant.FileName,
                        PlantCount = replant.TreeCount,
                        Description = replant.Description,
                        name = replant.User.FullName
                    }).ToList<object>();
                        return lstReplant.Count > 0 ? ServiceResponse.SuccessReponse(lstReplant, MessageEnum.DefaultSuccessMessage) : ServiceResponse.SuccessReponse(new List<object>(), MessageEnum.MapPointsNotFound);
                      
                    #endregion
                    case "report":
                        #region report
                        List<object> lstReport = db.Repository<Report>()
                    .GetAll()
                   // .Where(x => allUserID.Contains(x.UserID) )
                    .Select(report => new
                    {
                        pinImage = "report.png",
                        latitude = report.Latitude,
                        longitude = report.Longitude,
                        type = "report",
                        label = "report",
                        cash = 0,
                        greenPoints = report.GreenPoints,
                        fileName = report.FileName,
                        description = report.Description,
                        name = report.User.FullName
                    }).ToList<object>();
                        return lstReport.Count > 0 ? ServiceResponse.SuccessReponse(lstReport, MessageEnum.DefaultSuccessMessage) : ServiceResponse.SuccessReponse(new List<object>(), MessageEnum.MapPointsNotFound);
                      
                    #endregion
                    case "bin":
                        #region bin
                        List<object> lstBuyBin = db.Repository<BuyBin>()
                    .GetAll()
                   // .Where(x => allUserID.Contains(x.UserID) )
                    .Select(bin => new
                    {
                        pinImage = "bin.png",
                        Latitude = bin.User.Latitude,
                        Longitude = bin.User.Longitude,
                        Type = "amalbin",
                        Label = "amalbin",
                        Cash = 0,
                        fileName = bin.FileName,
                        greenPoints = bin.GreenPoints,
                        name = bin.User.FullName
                    }).ToList<object>();
                        return lstBuyBin.Count > 0 ? ServiceResponse.SuccessReponse(lstBuyBin, MessageEnum.DefaultSuccessMessage) : ServiceResponse.SuccessReponse(new List<object>(), MessageEnum.MapPointsNotFound);

                    #endregion
                    case "reuse":
                        #region reuse
                        List<object> lstreuse = db.Repository<Reuse>()
                    .GetAll()
                    //.Where(x => allUserID.Contains(x.UserID))
                    .Select(report => new
                    {
                        pinImage = "reuse.png",
                        latitude = report.Latitude,
                        longitude = report.Longitude,
                        type = "reuse",
                        label = "reuse",
                        cash = 0,
                        greenPoints = report.GreenPoints,
                        fileName = report.FileName,
                        description = report.Description,
                        name = report.User.FullName
                    }).ToList<object>();
                        return lstreuse.Count > 0 ? ServiceResponse.SuccessReponse(lstreuse, MessageEnum.DefaultSuccessMessage) : ServiceResponse.SuccessReponse(new List<object>(), MessageEnum.MapPointsNotFound);

                    #endregion

                    default:
                        return ServiceResponse.SuccessReponse(new List<object>(), MessageEnum.MapPointsNotFound);                        
                }
                #endregion

            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }          
        }
        [HttpGet]
        public ResponseObject<List<object>> GetStudentStaffRsByRole(string type)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                int? RoleID = JwtDecoder.GetUserRoleFromToken(Request.Headers.Authorization.Parameter);

                List<object> studentList = db.ExtRepositoryFor<SchoolRepository>().GetStudentStaffRsByRole(UserID, RoleID, type);
                return ServiceResponse.SuccessReponse(studentList, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }
    }
}
