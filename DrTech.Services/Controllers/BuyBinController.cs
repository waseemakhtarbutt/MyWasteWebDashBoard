using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DrTech.DAL;
using Microsoft.AspNetCore.Mvc;
using DrTech.Models;
using DrTech.Models.ViewModels;
using static DrTech.Common.Extentions.Constants;
using DrTech.Common.Helpers;
using DrTech.Common.ServerResponse;
using DrTech.Common.Enums;
using Microsoft.AspNetCore.Authorization;
using DrTech.Common.Extentions;
using System.IO;
using DrTech.Models.Common;
using System.Linq;
using DrTech.Notifications;

namespace DrTech.Services.Controllers
{
    [Route("api/[controller]")]
    public class BuyBinController : BaseControllerBase
    {
        [AllowAnonymous]
        [HttpGet("GetAvailableBinList")]
        public async Task<ResponseObject<List<BinDetails>>> GetAvailableBinList()
        {
            try
            {
                List<BinDetails> lstBinDetails = _IUWork.GetModelData<BinDetails>(CollectionNames.BINDETAIL);

                if (lstBinDetails.Count == 0)
                    return ServiceResponse.SuccessReponse(lstBinDetails, MessageEnum.BinNotFound);
                return ServiceResponse.SuccessReponse(lstBinDetails, MessageEnum.BinGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<BinDetails>>(exp);
            }
        }
        [Authorize]
        [HttpPost("RequestForNewBin")]
        public async Task<ResponseObject<bool>> RequestForNewBin([FromBody]BuyBin buyBin)
        {
            if (buyBin.BinId == null)
                return ServiceResponse.ErrorReponse<bool>(MessageEnum.BuyBinModelCannotBeNull);
            try
            {
                Random random = new Random();
                int num = random.Next(1000);
                BuyBin dd = new BuyBin
                {
                    BinId = buyBin.BinId,
                    UserId = GetLoggedInUserId(),
                    Status = (int)StatusEnum.Submit,
                    StatusDescription = StatusEnum.Submit.GetDescription(),
                    Qty = buyBin.Qty,
                    TrackingNumber = GetLoggedInUserId() + num,
                    Price = buyBin.Price,
                    FileName = buyBin.FileName,
                    BinName = buyBin.BinName

                };

                await _IUWork.AddSubDocument<Users, BuyBin>(GetLoggedInUserId(), dd, CollectionNames.USERS, CollectionNames.BUYBIN);

                NotifyEvent _event = new NotifyEvent();
                _event.Parameters.Add("TrackingNumber", dd.TrackingNumber);
                _event.Parameters.Add("BinName", dd.BinName);
                _event.AddNotifyEvent((long)NotificationEventConstants.RequestaBin.EmailSentoAdminForServerBin, GetLoggedInUserId());


                SMSNotifyEvent _events = new SMSNotifyEvent();
                _events.Parameters.Add("TrackingNumber", dd.TrackingNumber);
                _events.AddSMSNotifyEvent((long)NotificationEventConstants.RequestaBin.SendSMSToUser, GetLoggedInUserId());



                return ServiceResponse.SuccessReponse(true, "Thank you for your order. Please note your Tracking #" + dd.TrackingNumber + "  You would be contacted for delivery date and time within two business days.");
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
        [Authorize]
        [HttpGet("GetStatusOfBin")]
        public async Task<ResponseObject<List<BuyBin>>> GetStatusOfBin(string UserId = null)
        {
            //if (UserId == null)
            //    return ServiceResponse.ErrorReponse<List<BuyBin>>(MessageEnum.BinUserIdCannotBeNull);

            try
            {
                var user = _IUWork.FindOneByID<Users>(GetLoggedInUserId(), CollectionNames.USERS).Result;

                if (user.BuyBinDetails.Count == 0)
                    return ServiceResponse.SuccessReponse(user.BuyBinDetails.ToList(), MessageEnum.ReduceItemsNotFound);


                return ServiceResponse.SuccessReponse(user.BuyBinDetails.ToList(), MessageEnum.ReduceItemGetSuccess);

              //  List<FilterHelper> filter = new List<FilterHelper>
                //{
                //    new FilterHelper
                //    {
                //        Field = "UserId",
                //        Value = GetLoggedInUserId()
                //    }
                //};

                //List<BuyBin> LstBuyBin = _IUWork.GetModelData<BuyBin>(filter, CollectionNames.BUYBIN);

                //if (LstBuyBin.Count == 0)
                //    return ServiceResponse.SuccessReponse(LstBuyBin, MessageEnum.BinNotFound);

                //return ServiceResponse.SuccessReponse(LstBuyBin, MessageEnum.BinGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<BuyBin>>(exp);
            }
        }

        [HttpPost("AddNewBinInformation")]
        public async Task<ResponseObject<bool>> AddNewBinInformation(BinDetailsViewModel mdlBinInformation)
        {
            if (mdlBinInformation == null)
                return ServiceResponse.ErrorReponse<bool>(MessageEnum.BinDetailsViewModelCannotBeNull);
            try
            {
                string FileName = await SaveFile(mdlBinInformation.File); // await FileOpsHelper.UploadFile(mdlRefuse.File);

                BinDetails mdlBinDetail = new BinDetails
                {
                    Capacity = mdlBinInformation.Capacity,
                    Price = mdlBinInformation.Price,
                    FileName = FileName,
                    QRCode = mdlBinInformation.QRCode,
                    BinName = mdlBinInformation.BinName,
                    Description = mdlBinInformation.Description
                };
                mdlBinInformation.File = null;
                await _IUWork.InsertOne(mdlBinDetail, CollectionNames.BINDETAIL);
                return ServiceResponse.SuccessReponse(true, MessageEnum.BinDetailsAddedSuccess);

            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }

        }

         
    }
}