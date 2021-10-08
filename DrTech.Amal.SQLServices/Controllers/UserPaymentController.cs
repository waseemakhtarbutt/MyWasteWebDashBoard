using DrTech.Amal.Common.Enums;
using DrTech.Amal.Common.Helpers;
using DrTech.Amal.Common.ServerResponse;
using DrTech.Amal.SQLDataAccess.Repository;
using DrTech.Amal.SQLModels;
using DrTech.Amal.SQLServices.Auth;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using static DrTech.Amal.Common.Extentions.Constants;

namespace DrTech.Amal.SQLServices.Controllers
{
    public class UserPaymentController : BaseController
    {
        #region|Amal Add's Functinalities|
        [HttpGet]
        public ResponseObject<List<AdType>> GetAdType()
        {
            try
            {
                List<AdType> getAdType = db.Repository<AdType>().GetAll().ToList();
                return ServiceResponse.SuccessReponse(getAdType, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<AdType>>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<List<AddWeight>> GetWeight()
        {
            try
            {
                List<AddWeight> getWeight = db.Repository<AddWeight>().GetAll().ToList();
                return ServiceResponse.SuccessReponse(getWeight, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<AddWeight>>(exp);
            }
        }
        [HttpGet]
        public ResponseObject<List<object>> GetAdListByType(string type)
        {
            try
            {

                List<object> adList = new List<object>();

                if (type.ToLower() == AdTypeEnum.Internal.ToString().ToLower())
                {
                    adList = db.ExtRepositoryFor<UserPaymentRepository>().GetAdListByType(AdTypeEnum.Internal.ToString());
                }
                else if (type.ToLower() == AdTypeEnum.External.ToString().ToLower())
                {
                    adList = db.ExtRepositoryFor<UserPaymentRepository>().GetAdListByType(AdTypeEnum.External.ToString());
                }


                return ServiceResponse.SuccessReponse(adList, MessageEnum.ChildFoundSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }
        [HttpGet]
        public ResponseObject<List<Ad>> GetAdListByArea(int AreaID)
        {
            try
            {
                List<Ad> adList = new List<Ad>();

                adList = db.Repository<Ad>().GetAll().Where(x => x.AreaID == AreaID && x.AdType.Name.ToLower() == AdTypeEnum.External.ToString().ToLower()).ToList();
                return ServiceResponse.SuccessReponse(adList, MessageEnum.ChildFoundSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Ad>>(exp);
            }
        }

        //[HttpGet]
        //public ResponseObject<object> CheckApiResponse()
        //{
        //    try
        //    {
        //        object storeIds = new object();
        //        if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["desction"]))
        //            mdlAd.Description = HttpContext.Current.Request.Form["desction"];
        //        return ServiceResponse.SuccessReponse(storeIds, MessageEnum.ChildFoundSuccessfully);
        //    }
        //    catch (Exception exp)
        //    {
        //        return ServiceResponse.ErrorReponse<object>(exp);
        //    }
        //}
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage CheckApiResponse()
        {
            try
            {
                object storeIds = new object();
                EasyPessaModel mdl = new EasyPessaModel();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["vpc_VerStatus"]))
                    mdl.vpc_VerStatus = HttpContext.Current.Request.Form["vpc_VerStatus"];
                if (mdl.vpc_VerStatus == "Y")
                {
                    var newUrl = this.Url.Link("Default", new
                    {
                        Controller = "Home",
                        Action = "Success"
                    });
                    return Request.CreateResponse(HttpStatusCode.OK, new { Success = true, RedirectUrl = newUrl });

                }
                else
                {
                    var newUrl = this.Url.Link("Default", new
                    {
                        Controller = "Home",
                        Action = "Error"
                    });
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new { Success = false, RedirectUrl = newUrl });
                }
                //return ServiceResponse.SuccessReponse(mdl, MessageEnum.ChildFoundSuccessfully);
            }
            catch (Exception exp)
            {
                var newUrl = this.Url.Link("Default", new
                {
                    Controller = "Home",
                    Action = "Error"
                });
                return Request.CreateResponse(HttpStatusCode.NotFound, new { Success = false, RedirectUrl = newUrl });
            }
        }
        public class EasyPessaModel
        {
            public string vpc_VerStatus { get;set;}
            public string paymentMethod { get; set; }

        }

        [HttpGet]
        public ResponseObject<List<PaymentMethod>> GetPaymentMethodDetail()
        {
            try
            {
                List<PaymentMethod> GetPaymentMethodDetail = db.Repository<PaymentMethod>().GetAll().ToList();
                return ServiceResponse.SuccessReponse(GetPaymentMethodDetail, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<PaymentMethod>>(exp);
            }
        }
        [HttpGet]
        public ResponseObject<List<PaymentMethod>> GetPaymentMethoths()
        {
            try
            {
                List<PaymentMethod> GetPaymentMethodDetail = db.Repository<PaymentMethod>().GetAll().ToList();
                return ServiceResponse.SuccessReponse(GetPaymentMethodDetail, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            { 
                return ServiceResponse.ErrorReponse<List<PaymentMethod>>(exp);
            }
        }
        [HttpGet]
        public ResponseObject<List<object>> GetAdList()
        {
            try
            {
                List<object> getAd = db.ExtRepositoryFor<UserPaymentRepository>().GetAdList();
                return ServiceResponse.SuccessReponse(getAd, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }
        [HttpGet]
        public ResponseObject<List<object>> GetWeightList()
        {
            try
            {
                List<object> getAd = db.ExtRepositoryFor<UserPaymentRepository>().GetAdList();
                return ServiceResponse.SuccessReponse(getAd, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }

        [HttpPost]
        public async Task<ResponseObject<bool>> AddAdInformation()
        {
            try
            {
                Ad mdlAd = new Ad();
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                string FileName = string.Empty;

                var files = HttpContext.Current.Request.Files;
                if (files.Count > 0)
                {
                    HttpPostedFile file = HttpContext.Current.Request.Files[0];
                    FileName = await FileOpsHelper.UploadFileNew(file, ContainerName.Business);

                }


                mdlAd.FileName = FileName;
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["desction"]))
                    mdlAd.Description = HttpContext.Current.Request.Form["desction"];
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["AreaID"]))
                    mdlAd.AreaID = Convert.ToInt32(HttpContext.Current.Request.Form["AreaID"].ToString());
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["adTypeID"]))
                    mdlAd.AdTypeID = Convert.ToInt32(HttpContext.Current.Request.Form["adTypeID"].ToString());
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["CityID"]))
                    mdlAd.CityID = Convert.ToInt32(HttpContext.Current.Request.Form["CityID"].ToString());
                mdlAd.IsActive = true;
                mdlAd.CreatedBy = (int)UserID;
                mdlAd.CreatedDate = DateTime.Now;
                db.Repository<Ad>().Insert(mdlAd);
                db.Save();

                return ServiceResponse.SuccessReponse(true, MessageEnum.NGOAdded);
            }
            catch (DbEntityValidationException e)
            {

                return ServiceResponse.ErrorReponse<bool>("Text");
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpPost]
        public async Task<ResponseObject<bool>> AddWeightInformation()
        {
            try
            {
                AddWeight mdweight = new AddWeight();
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["weight"]))
                    mdweight.Weight = HttpContext.Current.Request.Form["weight"];
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["AreaID"]))
                    mdweight.AreaID = Convert.ToInt32(HttpContext.Current.Request.Form["AreaID"].ToString());
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["CityID"]))
                    mdweight.CityID = Convert.ToInt32(HttpContext.Current.Request.Form["CityID"].ToString());
                mdweight.IsActive = true;
                mdweight.CreatedBy = (int)UserID;
                mdweight.CreatedDate = DateTime.Now;
                db.Repository<AddWeight>().Insert(mdweight);
                db.Save();

                return ServiceResponse.SuccessReponse(true, MessageEnum.NGOAdded);
            }
            catch (DbEntityValidationException e)
            {

                return ServiceResponse.ErrorReponse<bool>("Text");
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
        #endregion
        #region|User GC Redeem Functionalites|
        [HttpGet]
        public ResponseObject<bool> RedeemUserGC(decimal? GC)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                GCRedeem mdlgCRedeem = new GCRedeem();
                mdlgCRedeem.GCRedeemed = GC;
                mdlgCRedeem.UserID = UserID;
                mdlgCRedeem.AmountGivenToUser = db.ExtRepositoryFor<UserPaymentRepository>().CalculateAmount(GC);
                mdlgCRedeem.CreatedBy = UserID;
                mdlgCRedeem.IsActive = true;
                db.Repository<GCRedeem>().Insert(mdlgCRedeem);
                db.Save();
                #region|Updadet User Wallet Amount|
                var mdlUser = db.Repository<User>().FindById(UserID);
                if (mdlUser != null)
                {
                    decimal PreAmount = mdlUser.WalletBalance ?? 0;
                    mdlUser.WalletBalance = PreAmount + mdlgCRedeem.AmountGivenToUser;
                    db.Repository<User>().Update(mdlUser);
                    db.Save();
                }
                #endregion

                return ServiceResponse.SuccessReponse(true, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
        #endregion
        #region|User Payments Functionalities|
        [HttpGet]
        public ResponseObject<object> GetUserWalletAmount()
        {
            try
            {
                object userObject = new object();
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                var mdlUser = db.Repository<User>().FindById(UserID);
                if (mdlUser != null)
                {
                    userObject = mdlUser.WalletBalance ?? 0;
                    return ServiceResponse.SuccessReponse(userObject, MessageEnum.DefaultSuccessMessage);
                }
                else
                {
                    return ServiceResponse.SuccessReponse(userObject, MessageEnum.DefaultErrorMessage);
                }
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }

        }


        //public ResponseObject<bool> SaveUserPaymentDetails()
        //{
        //    try
        //    {
        //        int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
        //        UserPayment mdlUserPyament = new UserPayment();
        //        mdlUserPyament.GCRedeemed = GC;
        //        mdlUserPyament.UserID = UserID;
        //        mdlUserPyament.AmountGivenToUser = db.ExtRepositoryFor<UserPaymentRepository>().CalculateAmount(GC);
        //        mdlUserPyament.CreatedBy = UserID;
        //        mdlUserPyament.IsActive = true;
        //        db.Repository<GCRedeem>().Insert(mdlgCRedeem);
        //        db.Save();
        //        #region|Updadet User Wallet Amount|
        //        var mdlUser = db.Repository<User>().FindById(UserID);
        //        if (mdlUser != null)
        //        {
        //            mdlUser.WalletBalance = mdlUser.WalletBalance + mdlgCRedeem.AmountGivenToUser;
        //            db.Repository<User>().Update(mdlUser);
        //            db.Save();
        //        }
        //        #endregion

        //        return ServiceResponse.SuccessReponse(true, MessageEnum.ChildFoundSuccessfully);
        //    }
        //    catch (Exception exp)
        //    {
        //        return ServiceResponse.ErrorReponse<bool>(exp);
        //    }
        //}
        #endregion
    }
}
