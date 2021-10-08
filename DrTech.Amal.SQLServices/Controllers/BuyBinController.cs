using DrTech.Amal.Common.Enums;
using DrTech.Amal.Common.ServerResponse;
using DrTech.Amal.SQLModels;
using DrTech.Amal.SQLServices.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DrTech.Amal.Notifications;
using DrTech.Amal.Common.Helpers;
using System.Data.Entity.Validation;
using DrTech.Amal.SQLDataAccess.Repository;
using DrTech.Amal.SQLDataAccess.CustomModels;
using DrTech.Amal.SQLServices.Models;
using System.Web;
using static DrTech.Amal.Common.Extentions.Constants;

namespace DrTech.Amal.SQLServices.Controllers
{
    [Authorize]
    public class BuyBinController : BaseController
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<ResponseObject<List<object>>> GetAvailableBinList()
        {
            try
            {
               // List<BinDetail> lstBinDetails = db.Repository<BinDetail>().GetAll().ToList();
                List<object> lstBinDetails = db.ExtRepositoryFor<BuyBinRepository>().GetBuyBinList();

                if (lstBinDetails.Count == 0)
                    return ServiceResponse.SuccessReponse(lstBinDetails, MessageEnum.BinNotFound);
                return ServiceResponse.SuccessReponse(lstBinDetails, MessageEnum.BinGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }
        [HttpPost]
        public async Task<ResponseObject<bool>> RequestForNewBin([FromBody]BuyBinOrderWithPayment buyBin)
        {
            if (buyBin.BinID == 0)
                return ServiceResponse.ErrorReponse<bool>(MessageEnum.BuyBinModelCannotBeNull);
            try
            {
                int? UserID;
                BinDetail binDetail = new BinDetail();
                binDetail = db.Repository<BinDetail>().FindById(buyBin.BinID);
                UserPayment mdlUserPayment = new UserPayment();
                if (buyBin.paymentMethodID == 4)
                {
                    #region|Save user payment history|

                    UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                    if (buyBin.Price == buyBin.PaidAmount + buyBin.deductedFromWallet)
                    {

                        mdlUserPayment.OrderPrice = buyBin.Price;
                        mdlUserPayment.DeductedFromWallet = buyBin.deductedFromWallet;
                        // mdlUserPayment.AmountPaid = buyBin.Price;
                        mdlUserPayment.AmountPaid = buyBin.PaidAmount;
                        mdlUserPayment.RemainingAmount = buyBin.RemainingAmount;
                        mdlUserPayment.PaymentMethodID = buyBin.paymentMethodID;
                        mdlUserPayment.UserID = UserID;
                        mdlUserPayment.CreatedBy = UserID;
                        mdlUserPayment.CreatedDate = DateTime.Now;
                        mdlUserPayment.IsActive = true;
                        db.Repository<UserPayment>().Insert(mdlUserPayment);
                        db.Save();
                        //Deduct Wallet Amount from User wallet;
                        var mdlUser = db.Repository<User>().FindById(UserID);
                        mdlUser.WalletBalance = mdlUser.WalletBalance - mdlUserPayment.DeductedFromWallet;
                        db.Repository<User>().Update(mdlUser);
                        db.Save();
                    }
                    else
                    {
                        return ServiceResponse.SuccessReponse(false, MessageEnum.DefaultErrorMessage);

                    }

                    #endregion

                }
                if (buyBin.paymentMethodID == 5)
                {
                    #region|Save user payment history|

                        UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                        mdlUserPayment.OrderPrice = buyBin.Price;
                        mdlUserPayment.DeductedFromWallet = buyBin.deductedFromWallet;
                        // mdlUserPayment.AmountPaid = buyBin.Price;
                        mdlUserPayment.AmountPaid = buyBin.PaidAmount;
                        mdlUserPayment.RemainingAmount = buyBin.RemainingAmount;
                        mdlUserPayment.PaymentMethodID = buyBin.paymentMethodID;
                        mdlUserPayment.UserID = UserID;
                        mdlUserPayment.CreatedBy = UserID;
                        mdlUserPayment.CreatedDate = DateTime.Now;
                        mdlUserPayment.IsActive = true;
                        db.Repository<UserPayment>().Insert(mdlUserPayment);
                        db.Save();
                        //Deduct Wallet Amount from User wallet;
                        var mdlUser = db.Repository<User>().FindById(UserID);
                        mdlUser.WalletBalance = mdlUser.WalletBalance - mdlUserPayment.DeductedFromWallet;
                        db.Repository<User>().Update(mdlUser);
                        db.Save();
                   

                    #endregion

                }
                else {
                    #region|Save user payment Easypaisa history|
                    UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                    if (buyBin.Price == buyBin.PaidAmount + buyBin.deductedFromWallet)
                    {
                        //UserID = buyBin.UserID;
                        mdlUserPayment.OrderPrice = buyBin.Price;
                        mdlUserPayment.DeductedFromWallet = buyBin.deductedFromWallet;
                        mdlUserPayment.AmountPaid = buyBin.PaidAmount;
                        mdlUserPayment.RemainingAmount = buyBin.RemainingAmount;
                        mdlUserPayment.PaymentMethodID = buyBin.paymentMethodID;
                        if (buyBin.UserPayment != null)
                        {
                            mdlUserPayment.orderRefNumber = buyBin.UserPayment.orderRefNumber;
                            mdlUserPayment.batchNumber = buyBin.UserPayment.batchNumber;
                            mdlUserPayment.transactionNumber = buyBin.UserPayment.transactionNumber;
                            mdlUserPayment.transactionResponse = buyBin.UserPayment.transactionResponse;
                        }
                        mdlUserPayment.UserID = UserID;
                        mdlUserPayment.CreatedBy = UserID;
                        mdlUserPayment.CreatedDate = DateTime.Now;
                        mdlUserPayment.IsActive = true;
                        db.Repository<UserPayment>().Insert(mdlUserPayment);
                        db.Save();
                        //Deduct Wallet Amount from User wallet;
                        var mdlUser = db.Repository<User>().FindById(UserID);
                        mdlUser.WalletBalance = mdlUser.WalletBalance - mdlUserPayment.DeductedFromWallet;
                        db.Repository<User>().Update(mdlUser);
                        db.Save();
                    }
                    else
                    {
                        return ServiceResponse.SuccessReponse(false, MessageEnum.DefaultErrorMessage);

                    }

                    #endregion
                }
                OrderTracking OT = new OrderTracking();
                Random random = new Random();
                int num = random.Next(1000);
                BuyBin dd = new BuyBin
                {
                    BinID = buyBin.BinID,
                    UserPaymentID = mdlUserPayment.ID,
                    UserID = (int)UserID,
                    StatusID = (int)StatusEnum.Submit,
                    TrackingNumber = "bin",
                    Qty = buyBin.Qty,
                    Price = buyBin.Price,
                    FileName = binDetail.FileName,
                    CreatedBy = (int)UserID,
                    CreatedDate = DateTime.Now
                };
                db.Repository<BuyBin>().Insert(dd);
                db.Save();
                OT.RsID = dd.ID;
                OT.Type = "Bin";
                OT.StatusID = (int)StatusEnum.New;
                OT.IsActive = true;
                OT.FileNameTakenByUser = buyBin.FileName;
                db.Repository<OrderTracking>().Insert(OT);
                db.Save();
                int TrackingID = OT.ID;
                BuyBin update = db.Repository<BuyBin>().GetAll().Where(x => x.ID == dd.ID).FirstOrDefault();
                update.TrackingNumber = TrackingID.ToString();
                db.Repository<BuyBin>().Update(update);
                db.Save();
                NotifyEvent _event = new NotifyEvent();
                _event.Parameters.Add("TrackingNumber", TrackingID);
                _event.Parameters.Add("BinName", binDetail.BinName);
                _event.AddNotifyEvent((long)NotificationEventConstants.RequestaBin.EmailSentoAdminForServerBin, UserID.ToString());
                SMSNotifyEvent _events = new SMSNotifyEvent();
                _events.Parameters.Add("TrackingNumber", TrackingID);
                _events.AddSMSNotifyEvent((long)NotificationEventConstants.RequestaBin.SendSMSToUser, UserID.ToString());
                return ServiceResponse.SuccessReponse(true, "Thank you for your order. Please note your Tracking #" + TrackingID + "  You would be contacted for delivery date and time within two business days.");
            }
            catch (DbEntityValidationException e)
            {
                String errorMessage = string.Empty;
                foreach (var eve in e.EntityValidationErrors)
                {
                    errorMessage = string.Format("Entity of type {0} in state {1} has the following validation errors: ",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State) + Environment.NewLine;
                    foreach (var ve in eve.ValidationErrors)
                    {
                        errorMessage = errorMessage + string.Format("- Property: {0}, Error: {1}",
                            ve.PropertyName, ve.ErrorMessage) + Environment.NewLine;
                    }
                }
                return ServiceResponse.ErrorReponse<bool>(errorMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
        [HttpGet]
        public ResponseObject<List<BinDetail>> GetBinDetail()
        {
            try
            {
                List<BinDetail> getBinDetail = db.Repository<BinDetail>().GetAll().Where(x=>x.IsActive==true).ToList();
                return ServiceResponse.SuccessReponse(getBinDetail, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<BinDetail>>(exp);
            }
        }
        [HttpGet]
        public ResponseObject<List<object>> GetBinDetailsList()
        {
            try
            {
                List<object> getBinDetails = db.ExtRepositoryFor<UserPaymentRepository>().GetBinDetailsList();
                return ServiceResponse.SuccessReponse(getBinDetails, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }
        [HttpGet]
        public ResponseObject<bool> DeleteBinDetails(int id)
        {
            try
            {
                BinDetail org = db.Repository<BinDetail>().GetAll().Where(x => x.ID == id).FirstOrDefault();
                org.IsActive = false;
                db.Repository<BinDetail>().Update(org);
                db.Save();
                return ServiceResponse.SuccessReponse(true, MessageEnum.OrgSuspended);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
        [HttpGet]
        public ResponseObject<object> GetBinDetailsByID(int id)
        {
            try
            {
                var getBinDetails = db.ExtRepositoryFor<UserPaymentRepository>().GetBinDetailsByID(id);
                return ServiceResponse.SuccessReponse(getBinDetails, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }
        [HttpPost]
        public async Task<ResponseObject<bool>> AddNewBinInformation()
        {
            try
            {
                BinDetail mdlBinDetail = new BinDetail();
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
                mdlBinDetail.FileName = FileName;
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["price"]))
                    mdlBinDetail.Price = Convert.ToDouble( HttpContext.Current.Request.Form["price"]);
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["BinName"]))
                    mdlBinDetail.BinName = HttpContext.Current.Request.Form["binName"];
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["desction"]))
                    mdlBinDetail.Description = HttpContext.Current.Request.Form["desction"];
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["capacity"]))
                    mdlBinDetail.Capacity = HttpContext.Current.Request.Form["capacity"];
                mdlBinDetail.IsActive = true;
                mdlBinDetail.CreatedBy = (int)UserID;
                mdlBinDetail.CreatedDate = DateTime.Now;
                db.Repository<BinDetail>().Insert(mdlBinDetail);
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
        public async Task<ResponseObject<bool>> UpdateBinInformation()
        {
            try
            {
                BinDetail mdlBinDetail = new BinDetail();
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["fileName"]))
                    mdlBinDetail.FileName = HttpContext.Current.Request.Form["fileName"];
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["id"]))
                    mdlBinDetail.ID = Convert.ToInt32( HttpContext.Current.Request.Form["id"]);
                if (mdlBinDetail.FileName == "" || mdlBinDetail.FileName== "undefined,undefined")
                {
                    string FileName = string.Empty;

                    var files = HttpContext.Current.Request.Files;
                    if (files.Count > 0)
                    {
                        HttpPostedFile file = HttpContext.Current.Request.Files[0];
                        FileName = await FileOpsHelper.UploadFileNew(file, ContainerName.Business);
                    }
                    mdlBinDetail.FileName = FileName;
                }
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["price"]))
                    mdlBinDetail.Price = Convert.ToDouble(HttpContext.Current.Request.Form["price"]);
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["BinName"]))
                    mdlBinDetail.BinName = HttpContext.Current.Request.Form["binName"];
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["desction"]))
                    mdlBinDetail.Description = HttpContext.Current.Request.Form["desction"];
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["capacity"]))
                    mdlBinDetail.Capacity = HttpContext.Current.Request.Form["capacity"];
                mdlBinDetail.IsActive = true;
                mdlBinDetail.UpdatedBy = (int)UserID;
                mdlBinDetail.CreatedDate = DateTime.Now;
                db.Repository<BinDetail>().Update(mdlBinDetail);
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
        [HttpGet]
        public ResponseObject<List<object>> GetStatusOfBin(int UserId = 0)
        {
            //if (UserId == 0)
            //    return ServiceResponse.ErrorReponse<BuyBin>(MessageEnum.BinUserIdCannotBeNull);

            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                List<object> lstBuyBin = db.ExtRepositoryFor<BuyBinRepository>().GetBuyBinDetail(UserID);

                if (lstBuyBin == null)
                    return ServiceResponse.SuccessReponse(lstBuyBin, MessageEnum.ReduceItemsNotFound);


                return ServiceResponse.SuccessReponse(lstBuyBin, MessageEnum.ReduceItemGetSuccess);

                
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }
        [HttpGet]
        public async Task<ResponseObject<List<object>>> GetBinsListByStatus(int StatusID = 0)
        {
            try
            {
                var binsList = db.ExtRepositoryFor<BuyBinRepository>().GetBinsListByStatus(StatusID);

                if (binsList.Count == 0)
                    return ServiceResponse.SuccessReponse(binsList, MessageEnum.BinNotFound);
                else
                    return ServiceResponse.SuccessReponse(binsList, MessageEnum.BinsGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }
        [HttpGet]
        public async Task<ResponseObject<BuyBinViewModel>> GetBinDetailById(int BinId)
        {
            try
            {
                var binDetail = db.ExtRepositoryFor<BuyBinRepository>().GetBinDetailById(BinId);

                if (binDetail == null)
                    return ServiceResponse.SuccessReponse(binDetail, MessageEnum.RecordNotFound);
                else
                {
                    RefTable refTable = db.Repository<RefTable>().GetAll().FirstOrDefault();

                    if (refTable != null)
                        binDetail.GPV = refTable.GreenPointValue;
                    else
                        binDetail.GPV = 5;

                    return ServiceResponse.SuccessReponse(binDetail, MessageEnum.RecordFoundSuccessfully);
                }
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<BuyBinViewModel>(exp);
            }
        }
        [HttpPost]
        public async Task<ResponseObject<bool>> AssignBinToDriver([FromBody]BuyBinViewModel _mdlBuyBinVM)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                db.ExtRepositoryFor<BuyBinRepository>().AssignBinToDriver(_mdlBuyBinVM, UserID);

                SMSNotifyEvent _events = new SMSNotifyEvent();
                _events.Parameters.Add("Comments", _mdlBuyBinVM.Comments ?? string.Empty);
                _events.AddSMSNotifyEvent((long)NotificationEventConstants.RequestaBin.Updated, Convert.ToString(UserID));

                PushNotificationEvent _event = new PushNotificationEvent();
                _event.Parameters.Add("GP", _mdlBuyBinVM.TotalGP);
                _event.AddPushNotifyEvent((long)NotificationEventConstants.PushNotification.Bin, Convert.ToString(_mdlBuyBinVM.UserID));

                return ServiceResponse.SuccessReponse(true, MessageEnum.RegiftUpdatedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
        [HttpPost]
        public ResponseObject<bool> SMSBinComments([FromBody]CommentsViewModel _mdlCommentsVM)
        {
            try
            {
                int? userID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                //send sms to user for to notification.
                SMSNotifyEvent _events = new SMSNotifyEvent();

                _events.SendSMS(_mdlCommentsVM.Phone, _mdlCommentsVM.Comments);

                // Recycle Comments

                if (!string.IsNullOrEmpty(_mdlCommentsVM.Comments))
                {
                    BuyBinComment buybinComments = new BuyBinComment()
                    {
                        Comments = _mdlCommentsVM.Comments,
                        CreatedBy = Convert.ToInt32(userID),
                        CreatedDate = DateTime.Now,
                        UpdatedBy = Convert.ToInt32(userID),
                        UpdatedDate = DateTime.Now,
                        IsActive = true,
                        BinID = _mdlCommentsVM.RID
                    };

                    db.Repository<BuyBinComment>().Insert(buybinComments);
                }

                db.Save();

                return ServiceResponse.SuccessReponse(true, MessageEnum.SuspendSuccessfully);
            }
            catch (Exception exp)
            {

                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
    }
}
