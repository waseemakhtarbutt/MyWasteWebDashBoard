using DrTech.Amal.Common.Enums;
using DrTech.Amal.Common.Helpers;
using DrTech.Amal.Common.ServerResponse;
using DrTech.Amal.SQLDataAccess.Repository;
using DrTech.Amal.SQLModels;
using DrTech.Amal.SQLServices.Auth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using static DrTech.Amal.Common.Extentions.Constants;

namespace DrTech.Amal.SQLServices.Controllers
{
    [Authorize]
    public class NGONeedController : BaseController
    {
        [HttpPost]
        public async Task<ResponseObject<bool>> AddNGONeed()
        {
            try
            {
                NGONeed mdlNGONeed = new NGONeed();

                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["type"]))
                    mdlNGONeed.TypeID = Convert.ToInt32(HttpContext.Current.Request.Form["type"]);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["subtype"]))
                    mdlNGONeed.SubTypeID = Convert.ToInt32(HttpContext.Current.Request.Form["subtype"]);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["city"]))
                    mdlNGONeed.CityID = Convert.ToInt32(HttpContext.Current.Request.Form["city"]);

                mdlNGONeed.IsActive = true;

                mdlNGONeed.UserID = (int)UserID;
                mdlNGONeed.CreatedBy = (int)UserID;
                mdlNGONeed.CreatedDate = DateTime.Now;

                db.Repository<NGONeed>().Insert(mdlNGONeed);
                db.Save();

                return ServiceResponse.SuccessReponse(true, MessageEnum.NGONeedAddedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpPost]
        public async Task<ResponseObject<bool>> ApproveCallForNGONeed(NGONeed model)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                NGONeed mdlNGONeedApprovel = db.Repository<NGONeed>().FindById(model.ID);
                mdlNGONeedApprovel.Qty = model.Qty;
                mdlNGONeedApprovel.IsActive = true;
                mdlNGONeedApprovel.UserID = (int)UserID;
                mdlNGONeedApprovel.UpdatedBy = (int)UserID;
                mdlNGONeedApprovel.UpdatedDate = DateTime.Now;
                db.Repository<NGONeed>().Update(mdlNGONeedApprovel);
                db.Save();

                LookupType mdllookup = db.Repository<LookupType>().FindById(mdlNGONeedApprovel.TypeID);
                mdllookup.Name = model.TypeDescription;
                mdllookup.Description = model.TypeDescription;
                mdllookup.Type = "DonationType";
                mdllookup.IsActive = true;
                db.Repository<LookupType>().Update(mdllookup);
                db.Save();
                LookupType sublookupType = db.Repository<LookupType>().GetAll().Where(x => x.ParentID == mdllookup.ID).FirstOrDefault();
                sublookupType.Name = model.SubTypeDescription;
                sublookupType.Description = model.SubTypeDescription;
                sublookupType.Type = "DonationType";
                sublookupType.IsActive = true;
                db.Repository<LookupType>().Update(sublookupType);
                db.Save();


                return ServiceResponse.SuccessReponse(true, MessageEnum.NGONeedUpdatedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpPost]
        public async Task<ResponseObject<bool>> AddNGODonation(NGONeed model)
        {
            try
            {
                            
                NGONeed mdlNGONeed = new NGONeed();
                LookupType mdllookup = new LookupType();
                LookupType sublookupType = new LookupType();
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                int? OrgID = db.Repository<Organization>().GetAll().Where(x => x.UserID == UserID).FirstOrDefault().ID;
                if (model.ID == 0)
                {
                    if (model.TypeID == 0)
                    {
                        mdllookup.Name = model.TypeDescription;
                        mdllookup.Description = model.TypeDescription;
                        mdllookup.Type = "DonationType";
                        mdllookup.IsActive = false;
                        db.Repository<LookupType>().Insert(mdllookup);
                        db.Save();
                        int typeid = mdllookup.ID;
                        sublookupType.ParentID = typeid;
                        sublookupType.Name = model.SubTypeDescription;
                        sublookupType.Description = model.SubTypeDescription;
                        sublookupType.Type = "DonationType";
                        sublookupType.IsActive = false;
                        db.Repository<LookupType>().Insert(sublookupType);
                        db.Save();
                        int subtypeid = sublookupType.ID;

                        mdlNGONeed.Qty = model.Qty;
                        mdlNGONeed.TypeID = typeid;
                        mdlNGONeed.SubTypeID = subtypeid;
                        mdlNGONeed.IsActive = false;
                        mdlNGONeed.UserID = (int)UserID;
                        mdlNGONeed.CreatedBy = (int)UserID;
                        mdlNGONeed.OrgID = OrgID;
                        mdlNGONeed.CreatedDate = DateTime.Now;

                        db.Repository<NGONeed>().Insert(mdlNGONeed);
                        db.Save();

                        return ServiceResponse.SuccessReponse(true, MessageEnum.NGONeedAddedSuccessfully);

                    }
                    else
                    {
                        mdlNGONeed.Qty = model.Qty;
                        mdlNGONeed.TypeID = model.TypeID;
                        mdlNGONeed.SubTypeID = model.SubTypeID;
                        mdlNGONeed.IsActive = true;
                        mdlNGONeed.UserID = (int)UserID;
                        mdlNGONeed.CreatedBy = (int)UserID;
                        mdlNGONeed.CreatedDate = DateTime.Now;
                        mdlNGONeed.OrgID = OrgID;
                        db.Repository<NGONeed>().Insert(mdlNGONeed);
                        db.Save();

                        return ServiceResponse.SuccessReponse(true, MessageEnum.NGONeedAddedSuccessfully);
                    }
                }
                else
                {
                    NGONeed mdlNGONeedUpadate = db.Repository<NGONeed>().FindById(model.ID);
                    mdlNGONeedUpadate.Qty = model.Qty;
                    mdlNGONeedUpadate.TypeID = model.TypeID;
                    mdlNGONeedUpadate.SubTypeID = model.SubTypeID;
                    mdlNGONeedUpadate.UpdatedBy = UserID;
                    mdlNGONeedUpadate.UpdatedDate = DateTime.Now;
                    db.Repository<NGONeed>().Update(mdlNGONeedUpadate);
                    db.Save();
                    return ServiceResponse.SuccessReponse(true, MessageEnum.NGONeedUpdatedSuccessfully);
                }
               

                // mdlNGONeed.TypeDescription = model.TypeDescription;
                // mdlNGONeed.SubTypeDescription = model.SubTypeDescription;
              
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpPost]
        public async Task<ResponseObject<bool>> UpdateNGONeed()
        {
            try
            {
                NGONeed _mdlNGONeed = new NGONeed();

                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["ID"]))
                    _mdlNGONeed.ID = Convert.ToInt32(HttpContext.Current.Request.Form["ID"]);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["type"]))
                    _mdlNGONeed.TypeID = Convert.ToInt32(HttpContext.Current.Request.Form["type"]);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["subtype"]))
                    _mdlNGONeed.SubTypeID = Convert.ToInt32(HttpContext.Current.Request.Form["subtype"]);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["city"]))
                    _mdlNGONeed.CityID = Convert.ToInt32(HttpContext.Current.Request.Form["city"]);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["isactive"]))
                    _mdlNGONeed.IsActive = Convert.ToBoolean(HttpContext.Current.Request.Form["isactive"]);

                NGONeed mdlNGONeed = db.Repository<NGONeed>().FindById(_mdlNGONeed.ID);

                if (_mdlNGONeed.TypeID != 0)
                    mdlNGONeed.TypeID = _mdlNGONeed.TypeID;

                if (_mdlNGONeed.SubTypeID != 0)
                    mdlNGONeed.SubTypeID = _mdlNGONeed.SubTypeID;

                if (_mdlNGONeed.CityID != 0)
                    mdlNGONeed.CityID = _mdlNGONeed.CityID;

                if (_mdlNGONeed.IsActive != null)
                    mdlNGONeed.IsActive = _mdlNGONeed.IsActive;

                mdlNGONeed.UpdatedBy = UserID;
                mdlNGONeed.UpdatedDate = DateTime.Now;

                db.Repository<NGONeed>().Update(mdlNGONeed);
                db.Save();

                return ServiceResponse.SuccessReponse(true, MessageEnum.NGONeedUpdatedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }


        

             [HttpGet]
        public ResponseObject<List<object>> GetNGONeedListByUserID()
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                var ngoNeeds = db.ExtRepositoryFor<NGONeedRepository>().GetNGONeedListbyUserID(UserID);

                if (ngoNeeds.Count == 0)
                    return ServiceResponse.SuccessReponse(ngoNeeds, MessageEnum.NGONeedsNotFound);
                else
                    return ServiceResponse.SuccessReponse(ngoNeeds, MessageEnum.NGONeedsGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<List<object>> GetNGONeedList()
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                var ngoNeeds = db.ExtRepositoryFor<NGONeedRepository>().GetNGONeedList(UserID);

                if (ngoNeeds.Count == 0)
                    return ServiceResponse.SuccessReponse(ngoNeeds, MessageEnum.NGONeedsNotFound);
                else
                    return ServiceResponse.SuccessReponse(ngoNeeds, MessageEnum.NGONeedsGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }
        [HttpGet]
        public ResponseObject<List<object>> GetNGONeedListInActive()
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                var ngoNeedsInActive = db.ExtRepositoryFor<NGONeedRepository>().GetNGONeedListInActive(UserID);

                if (ngoNeedsInActive.Count == 0)
                    return ServiceResponse.SuccessReponse(ngoNeedsInActive, MessageEnum.NGONeedsNotFound);
                else
                    return ServiceResponse.SuccessReponse(ngoNeedsInActive, MessageEnum.NGONeedsGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<object> GetNGONeedById(int Id)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                var ngoNeeds = db.ExtRepositoryFor<NGONeedRepository>().GetNGONeedById(Id);

                if (ngoNeeds !=null)
                    return ServiceResponse.SuccessReponse(ngoNeeds, MessageEnum.NGONeedsNotFound);
                else
                    return ServiceResponse.SuccessReponse(ngoNeeds, MessageEnum.NGONeedsGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }


        [HttpGet]
        public async Task<ResponseObject<List<LookupType>>> GetDropdownByType(string TypeName)
        {
            try
            {
                List<LookupType> lstLookupType = db.ExtRepositoryFor<CommonRepository>().GetLookupByTypeName(TypeName).ToList();
                lstLookupType.Add(new LookupType { ID = 0, Name = "Other" });

                return ServiceResponse.SuccessReponse(lstLookupType, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<LookupType>>(exp);
            }
        }
    }
}
