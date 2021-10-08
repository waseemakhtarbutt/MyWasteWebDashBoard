using DrTech.Amal.Common.Enums;
using DrTech.Amal.Common.Helpers;
using DrTech.Amal.Common.ServerResponse;
using DrTech.Amal.Notifications;
using DrTech.Amal.SQLDataAccess.CustomModels;
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

namespace DrTech.Amal.SQLServices.Controllers {
    [Authorize]
    public class RegiftController : BaseController
    {
        [HttpPost]
        public async Task<ResponseObject<bool>> AddDonation()
        {
            try
            {
                OrderTracking OT = new OrderTracking();
                Regift mdlDonation = new Regift();
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                string FileName = string.Empty;
                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                FileName = await FileOpsHelper.UploadFileNew(file, ContainerName.REGIFT);

                mdlDonation.FileName = FileName;
                mdlDonation.DonateToID = Convert.ToInt32(HttpContext.Current.Request.Form["donateTo"]);
                mdlDonation.Longitude = 0.0M;// Convert.ToDecimal(provider.FormData.GetValues("longitude")[0]);
                mdlDonation.Latitude = 0.0M; // Convert.ToDecimal(provider.FormData.GetValues("latitude")[0]);
                mdlDonation.Description = HttpContext.Current.Request.Form["description"];
                mdlDonation.PickupDate = Utility.GetParsedDate(HttpContext.Current.Request.Form["collectorDateTime"].ToString());
                //Convert.ToDateTime(HttpContext.Current.Request.Form["collectorDateTime"]);
                mdlDonation.CityID = 3;
                mdlDonation.StatusID = (int)StatusEnum.Submit;
                mdlDonation.CreatedBy = (int)UserID;
                mdlDonation.UserID = (int)UserID;
                mdlDonation.CreatedDate = DateTime.Now;
                mdlDonation.UpdatedDate = DateTime.Now;

                var isHasNewValue = false;
                if (mdlDonation.DonateToID < 0)
                    isHasNewValue = true;

                if (isHasNewValue)
                    mdlDonation.StatusID = (int)StatusEnum.PenddingApproval;
                else
                    mdlDonation.StatusID = (int)StatusEnum.Submit;


                int RegiftID = mdlDonation.ID;
                int TypeID = Convert.ToInt32(HttpContext.Current.Request.Form["type"]);
                int SubTypeID = Convert.ToInt32(HttpContext.Current.Request.Form["subType"]);

                RegiftSubItem SubItems = new RegiftSubItem();
                SubItems.TypeID = TypeID;
                SubItems.SubTypeID = SubTypeID;
                SubItems.Qty = 0;
                SubItems.IsParent = true;
                mdlDonation.RegiftSubItems.Add(SubItems);
                db.Repository<Regift>().Insert(mdlDonation);
                db.Save();

                OT.RsID = mdlDonation.ID;
                OT.Type = "Regift";
                OT.StatusID = (int)StatusEnum.New;
                OT.FileNameTakenByUser = FileName;
                OT.IsActive = true;
                db.Repository<OrderTracking>().Insert(OT);
                db.Save();


                //NotifyEvent _event = new NotifyEvent();
                //_event.Parameters.Add("FileName", mdlDonation.FileName);
                //_event.AddNotifyEvent((long)NotificationEventConstants.Regift.EmailSendToAdminForApproval, GetLoggedInUserId());

                LookupType mdlType = db.Repository<LookupType>().GetAll().Where(x => x.ID == TypeID).FirstOrDefault();

                LookupType mdlSubType = db.Repository<LookupType>().GetAll().Where(x => x.ID == SubTypeID).FirstOrDefault();

                Organization mdlDonationType = db.Repository<Organization>().GetAll().Where(x => x.ID == mdlDonation.DonateToID).FirstOrDefault();

                NotifyEvent _event = new NotifyEvent();
                _event.Parameters.Add("FileName", mdlDonation.FileName);
                _event.Parameters.Add("TypeDescription", mdlType.Name);
                _event.Parameters.Add("SubTypeDescription", mdlType.Name);
                _event.Parameters.Add("Longitude", mdlDonation.Longitude);
                _event.Parameters.Add("Latitude", mdlDonation.Latitude);
                _event.Parameters.Add("DonateToDescription", mdlDonationType.Name);
                _event.AddNotifyEvent((long)NotificationEventConstants.Regift.EmailSendToAdminForApproval, Convert.ToString(UserID));


                SMSNotifyEvent _events = new SMSNotifyEvent();
                _events.Parameters.Add("TypeDescription", mdlType.Name);
                _events.AddSMSNotifyEvent((long)NotificationEventConstants.Regift.SMSSendToUser, Convert.ToString(UserID));


                return ServiceResponse.SuccessReponse(true, MessageEnum.DonationAddedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
        public ResponseObject<List<Regift>> GetDonation(int userId, int DonationId)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                List<Regift> LstRegiftItems = db.Repository<Regift>().GetAll().Where(x => x.UserID == UserID && x.ID == DonationId).OrderByDescending(x => x.CreatedDate).ToList();

                if (LstRegiftItems.Count == 0)
                    return ServiceResponse.SuccessReponse(LstRegiftItems, MessageEnum.DonationFoundSuccessfully);

                return ServiceResponse.SuccessReponse(LstRegiftItems, MessageEnum.DonationNotFound);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Regift>>(exp);
            }
        }
        [HttpPost]
        public ResponseObject<object> ChangeStatusForRegift(int rID, string status)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                OrderTracking mdlOrder = db.Repository<OrderTracking>().GetAll().Where(o => o.RsID == rID && o.Type == "Regift").FirstOrDefault();
                Regift amalR = db.Repository<Regift>().FindById(rID);

                if (status == "confirm")
                {
                    amalR.StatusID = (int)StatusEnum.Complete;
                    mdlOrder.StatusID = (int)StatusEnum.Complete;
                    mdlOrder.CollectedPendingConfirmation = true;
                }
                else if (status == "decline")
                {   // You may call RejectRegift to both statuses of order tracking and regift.
                    // db.ExtRepositoryFor<RegiftRepository>().RejectRegift(UserID, rID);
                    mdlOrder.StatusID = (int)StatusEnum.Disputed;
                    mdlOrder.CollectedPendingConfirmation = null;
                    amalR.StatusID = (int)StatusEnum.Disputed;
                }
                else
                {
                    return ServiceResponse.ErrorReponse<object>("Query Parameters not correct");
                }

                db.Repository<Regift>().Update(amalR);
                db.Repository<OrderTracking>().Update(mdlOrder);
                db.Save();
                return ServiceResponse.SuccessReponse<object>(true, "Status Changed Successfully");
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }
        [AllowAnonymous]
        [HttpGet]
        public ResponseObject<List<object>> GetDonations(string UserId = null)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                List<object> LstRegiftItems = db.ExtRepositoryFor<RegiftRepository>().GetDonations(UserID);

                if (LstRegiftItems.Count > 0)
                {
                    return ServiceResponse.SuccessReponse(LstRegiftItems, MessageEnum.DonationFoundSuccessfully);
                }
                else
                {
                    return ServiceResponse.SuccessReponse(LstRegiftItems, MessageEnum.DonationNotFound);
                }
            }

            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }
        public async Task<ResponseObject<List<LookupType>>> GetDonationDropdowns()
        {
            ResponseObject<List<LookupType>> DonationList = await new CommonController().GetDropdownByType("DonationType");

            return DonationList;
        }
        public async Task<ResponseObject<List<LookupType>>> GetSubTypeDropdown(int id)
        {
            ResponseObject<List<LookupType>> SubDonationList = await new CommonController().GetDropdownByParentID(id);

            return SubDonationList;
        }
        [HttpGet] 
        public ResponseObject<List<Organization>> GetAllNGOs()
        {
            try
            {
                //List<Organization> OrgnizationList = new List<Organization>();
                //var type = db.Repository<LookupType>().GetAll().Where(x => x.Name.ToLower() == OrganizationType.Donation.ToString().ToLower()).FirstOrDefault();
                //if(type != null)
                //{
                //    OrgnizationList = db.Repository<Organization>().GetAll().Where(x => x.OrgTypeID == type.ID).ToList();
                //}

                //List<Organization> OrgnizationList = db.Repository<Organization>().GetAll().
                //    Where(x => x.LookupType.Type == "Organization" 
                //    && x.LookupType.Name.ToLower() == OrganizationType.Donation.ToString().ToLower()).ToList();
                List<Organization> OrgnizationList = db.Repository<Organization>().GetAll().
                  Where(x => x.LookupType.Type == "Organization"
                  ).ToList();
                if (OrgnizationList.Count > 0)
                {
                    return ServiceResponse.SuccessReponse(OrgnizationList, MessageEnum.DonationFoundSuccessfully);
                }
                else
                {
                    return ServiceResponse.SuccessReponse(OrgnizationList, MessageEnum.DonationNotFound);
                }
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Organization>>(exp);
            }
        }
        [HttpGet]
        public async Task<ResponseObject<List<object>>> GetRegiftsListByStatus(int StatusID = 0)
        {
            try
            {
                var regiftsList = db.ExtRepositoryFor<RegiftRepository>().GetRegiftsListByStatus(StatusID);

                if (regiftsList.Count == 0)
                    return ServiceResponse.SuccessReponse(regiftsList, MessageEnum.RegiftItemsNotFound);
                else
                    return ServiceResponse.SuccessReponse(regiftsList, MessageEnum.RegiftItemGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }
        [HttpPost]
        public async Task<ResponseObject<bool>> UpdateStatus([FromBody]Regift _mdlRegift)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                if (_mdlRegift.StatusID > 0)
                {
                    Regift mdlGift = db.Repository<Regift>().FindById(_mdlRegift.ID);

                    mdlGift.GreenPoints = _mdlRegift.GreenPoints;
                    mdlGift.StatusID = _mdlRegift.StatusID;

                    mdlGift.UpdatedBy = UserID;
                    mdlGift.UpdatedDate = DateTime.Now;

                    db.Repository<Regift>().Update(mdlGift);
                    db.Save();

                    return ServiceResponse.SuccessReponse(true, MessageEnum.RegiftUpdatedSuccessfully);
                }

                return ServiceResponse.SuccessReponse(false, MessageEnum.DefaultParametersCanNotBeNull);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
        [HttpGet]
        public async Task<ResponseObject<RegiftViewModel>> GetRegiftDetailById(int RegiftId, bool IsWebAdmin = false)
        {
            try
            {
                var regift = db.ExtRepositoryFor<RegiftRepository>().GetRegiftDetailById(RegiftId, IsWebAdmin);

                if (regift == null)
                    return ServiceResponse.SuccessReponse(regift, MessageEnum.RecordNotFound);
                else
                {
                    RefTable refTable = db.Repository<RefTable>().GetAll().FirstOrDefault();

                    if (refTable != null)
                        regift.GPV = refTable.GreenPointValue;
                    else
                        regift.GPV = 5;

                    return ServiceResponse.SuccessReponse(regift, MessageEnum.RecordFoundSuccessfully);
                }
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<RegiftViewModel>(exp);
            }
        }
        [HttpPost]
        public async Task<ResponseObject<bool>> AssignRegiftToDriver([FromBody]RegiftViewModel _mdlRegiftVM)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                db.ExtRepositoryFor<RegiftRepository>().AssignRegiftToDriver(_mdlRegiftVM, UserID);

                SMSNotifyEvent _events = new SMSNotifyEvent();
                _events.Parameters.Add("Comments", _mdlRegiftVM.Comments ?? string.Empty);
                _events.AddSMSNotifyEvent((long)NotificationEventConstants.Regift.Updated, Convert.ToString(UserID));

                PushNotificationEvent _event = new PushNotificationEvent();
                _event.Parameters.Add("GP", _mdlRegiftVM.TotalGP);
                _event.AddPushNotifyEvent((long)NotificationEventConstants.PushNotification.Regift, Convert.ToString(_mdlRegiftVM.UserID));

                return ServiceResponse.SuccessReponse(true, MessageEnum.RegiftUpdatedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
        [HttpPost]
        public ResponseObject<bool> SetConfirmation(int orderId, bool isDriver)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                OrderTracking orderTracking = db.Repository<OrderTracking>().GetAll().Where(x => x.ID == orderId).FirstOrDefault();

                if (isDriver)
                    orderTracking.CollectedPendingConfirmation = true;
                else
                    orderTracking.DeliveredPendingConfirmation = true;

                orderTracking.UpdatedBy = UserID;
                orderTracking.UpdatedDate = DateTime.Now;

                db.Repository<OrderTracking>().Update(orderTracking);

                db.Save();

                return ServiceResponse.SuccessReponse(true, MessageEnum.SchoolSuspended);
            }
            catch (Exception exp)
            {

                return ServiceResponse.ErrorReponse<bool>(exp);
            }

        }
        [HttpPost]
        public async Task<ResponseObject<bool>> RejectRegift([FromBody]RegiftViewModel _mdlRegiftVM)
        {
            try
            {
                int? userID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                db.ExtRepositoryFor<RegiftRepository>().RejectRegift(_mdlRegiftVM, userID);

                return ServiceResponse.SuccessReponse(true, MessageEnum.RegiftUpdatedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
        [HttpGet]
        public async Task<ResponseObject<ShiftViewModel>> Shift()
        {
            try
            {
                int? userID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                ShiftViewModel workingHour = db.ExtRepositoryFor<CommonRepository>().GetWorkingHours(ShiftEnum.Morning.ToString());

               // ShiftViewModel shiftViewModel = new ShiftViewModel();
               // workingHour = new DateTime (   workingHour.StartTime)

                //shiftViewModel.startDate =  workingHour.StartTime.ToString();
                //shiftViewModel.endDate =  workingHour.EndTime.ToString();
                //shiftViewModel.startDate = shiftViewModel.startDate.Replace(":", ", ");
                //shiftViewModel.endDate = shiftViewModel.endDate.Replace(":", ", ");

                return ServiceResponse.SuccessReponse(workingHour, MessageEnum.WorkingShiftSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<ShiftViewModel>(exp);
            }
        }
        [HttpPost]
        public ResponseObject<bool> SMSRegiftComments([FromBody]CommentsViewModel _mdlCommentsVM)
        {
            try
            {
                int? userID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                //send sms to user for to notification.
                SMSNotifyEvent _events = new SMSNotifyEvent();

                _events.SendSMS(_mdlCommentsVM.Phone, _mdlCommentsVM.Comments);

                // Regift Comments

                if (!string.IsNullOrEmpty(_mdlCommentsVM.Comments))
                {
                    RegiftComment regiftComments = new RegiftComment()
                    {
                        Comments = _mdlCommentsVM.Comments,
                        CreatedBy = Convert.ToInt32(userID),
                        CreatedDate = DateTime.Now,
                        UpdatedBy = Convert.ToInt32(userID),
                        UpdatedDate = DateTime.Now,
                        IsActive = true,
                        RegiftID = _mdlCommentsVM.RID
                    };

                    db.Repository<RegiftComment>().Insert(regiftComments);
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
