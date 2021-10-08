using DrTech.Amal.Common.Enums;
using DrTech.Amal.Common.Helpers;
using DrTech.Amal.Common.ServerResponse;
using DrTech.Amal.Notifications;
using DrTech.Amal.SQLDataAccess.Repository;
using DrTech.Amal.SQLDataAccess.CustomModels;
using DrTech.Amal.SQLModels;
using DrTech.Amal.SQLServices.Auth;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using static DrTech.Amal.Common.Extentions.Constants;
using Newtonsoft.Json;
using DrTech.Amal.SQLServices.Models;
using System.Net.Http.Headers;

namespace DrTech.Amal.SQLServices.Controllers
{
    public class DriverController : BaseController
    {
        [HttpPost]
        public async Task<ResponseObject<bool>> AddDriverDetails()
        {
            try
            {
                Driver mdlDriver = new Driver();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["id"]))
                    mdlDriver = db.Repository<Driver>().FindById(Convert.ToInt32(HttpContext.Current.Request.Form["id"].ToString()));

                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                string FileName = string.Empty;
                string LicenceFileName = string.Empty;
                HttpPostedFile LicenceFile;

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["fileImage"]))
                {
                    HttpPostedFile file = HttpContext.Current.Request.Files[0];
                    FileName = await FileOpsHelper.UploadFileNew(file, ContainerName.USER);
                }
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["Filelicence"]))
                {
                    if (HttpContext.Current.Request.Files.Count == 1)
                    {
                        LicenceFile = HttpContext.Current.Request.Files[0];
                        LicenceFileName = await FileOpsHelper.UploadFileNew(LicenceFile, ContainerName.USER);
                    }
                    else
                    {
                        LicenceFile = HttpContext.Current.Request.Files[1];
                        LicenceFileName = await FileOpsHelper.UploadFileNew(LicenceFile, ContainerName.USER);
                    }

                }
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["firstName"]))
                    mdlDriver.FirstName = HttpContext.Current.Request.Form["firstName"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["lastName"]))
                    mdlDriver.LastName = HttpContext.Current.Request.Form["lastName"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["greenshopID"]))
                    mdlDriver.GreenShopID = Convert.ToInt32(HttpContext.Current.Request.Form["greenshopID"]);

                if (!string.IsNullOrEmpty(FileName))
                    mdlDriver.FileName = FileName;

                if (!string.IsNullOrEmpty(LicenceFileName))
                    mdlDriver.LicienceFileName = LicenceFileName;

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["address"]))
                    mdlDriver.Address = HttpContext.Current.Request.Form["address"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["phone"]))
                    mdlDriver.Phone = HttpContext.Current.Request.Form["phone"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["regNumber"]))
                    mdlDriver.RegNumber = HttpContext.Current.Request.Form["regNumber"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["pin"]))
                    mdlDriver.PIN = HttpContext.Current.Request.Form["pin"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["vehicleID"]))
                    mdlDriver.VehicleID = Convert.ToInt32(HttpContext.Current.Request.Form["vehicleID"].ToString());

                mdlDriver.IsActive = true;

                if (mdlDriver.ID != 0)
                {

                    Driver obj = db.Repository<Driver>().GetAll().Where(x => x.ID == mdlDriver.ID).FirstOrDefault();

                    if (string.IsNullOrEmpty(mdlDriver.FileName))
                        mdlDriver.FileName = obj.FileName;

                    if (string.IsNullOrEmpty(mdlDriver.LicienceFileName))
                        mdlDriver.LicienceFileName = obj.LicienceFileName;

                    mdlDriver.UpdatedBy = (int)UserID;
                    mdlDriver.UpdatedDate = DateTime.Now;
                    db.Repository<Driver>().Update(mdlDriver);
                    db.Save();
                }
                else
                {
                    //int _min = 1000;
                    //int _max = 9999;
                    //Random random = new Random();
                    //Int32 number = random.Next(_min, _max);

                    //mdlDriver.PIN = number.ToString();

                    mdlDriver.CreatedBy = (int)UserID;
                    mdlDriver.CreatedDate = DateTime.Now;
                    db.Repository<Driver>().Insert(mdlDriver);
                    db.Save();
                    

                    NotifyEvent _event = new NotifyEvent();
                    _event.Parameters.Add("PIN", mdlDriver.PIN);
                    _event.Parameters.Add("Phone", mdlDriver.Phone);
                    _event.AddNotifyEvent((long)NotificationEventConstants.Driver.SendtoAdminPin, UserID.ToString());

                }
                return ServiceResponse.SuccessReponse(true, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpPost]
        public async Task<ResponseObject<List<object>>> GetAllDrivers(DriverRequestDto model)
        {
            try
            {
                var drivers = db.ExtRepositoryFor<DriverRepository>().GetAllDrivers(model);

                if (drivers.Count() == 0)
                    return ServiceResponse.SuccessReponse(drivers, MessageEnum.RecordNotFound);
                else
                    return ServiceResponse.SuccessReponse(drivers, MessageEnum.RecordFoundSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }
        [HttpGet]
        public async Task<ResponseObject<List<object>>> GetAllDriver()
        {
            try
            {
                var drivers = db.ExtRepositoryFor<DriverRepository>().GetAllDriver();

                if (drivers.Count() == 0)
                    return ServiceResponse.SuccessReponse(drivers, MessageEnum.RecordNotFound);
                else
                    return ServiceResponse.SuccessReponse(drivers, MessageEnum.RecordFoundSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }

        [HttpGet]
        public async Task<ResponseObject<bool>> CheckPhoneNumber(string phoneNumber)
        {
            try
            {
                bool exists = db.ExtRepositoryFor<DriverRepository>().CheckPhoneNumber(phoneNumber);

                //if (exists == true)
                //    return ServiceResponse.SuccessReponse(exists, MessageEnum.RecordNotFound);
                //else
                    return ServiceResponse.SuccessReponse(exists, MessageEnum.RecordFoundSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpGet]
        public async Task<ResponseObject<bool>> CheckDriverAssignments(int Id)
        {
            try
            {
                // bool exists = db.ExtRepositoryFor<DriverRepository>().CheckDriverAssignment(Id);
                int Count = db.Repository<OrderTracking>().GetAll().Where(
                x => x.AssignTo == Id && 
                (x.Type == "Regift" && (x.StatusID != (int)StatusEnum.Declined && x.StatusID != (int)StatusEnum.Delivered)
                || x.Type == "Recycle" && (x.StatusID != (int)StatusEnum.Declined && x.StatusID != (int)StatusEnum.Delivered)
                || x.Type == "Bin" && (x.StatusID != (int)StatusEnum.Declined && x.StatusID != (int)StatusEnum.Delivered)))
                .ToList().Count;


               // int Count = db.Repository<OrderTracking>().GetAll().Where(x => x.AssignTo == Id).ToList().Count;
                if (Count > 0)
                {
                    return ServiceResponse.SuccessReponse(false, MessageEnum.RecordFoundSuccessfully);
                }
                else
                {
                    return ServiceResponse.SuccessReponse(true, MessageEnum.RecordFoundSuccessfully);
                }

            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        //Download doc
        [HttpGet]//http get as it return file 
        public HttpResponseMessage GetLicenceFile(int id)
        {
            //below code locate physcial file on server 
            Driver obj = db.Repository<Driver>().GetAll().Where(x => x.ID == id).FirstOrDefault();
            //var localFilePath = HttpContext.Current.Server.MapPath(obj.LicienceFileName);
            var localFilePath = obj.LicienceFileName;

            HttpResponseMessage response = null;
            if (!File.Exists(localFilePath))
            {
                //if file not found than return response as resource not present 
                response = Request.CreateResponse(HttpStatusCode.Gone);
            }
            else
            {
                //if file present than read file 
                var fStream = new FileStream(localFilePath, FileMode.Open, FileAccess.Read);
                //compose response and include file as content in it
                response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StreamContent(fStream)
                };
                //set content header of reponse as file attached in reponse
                response.Content.Headers.ContentDisposition =
                new ContentDispositionHeaderValue("attachment")
                {
                    FileName = Path.GetFileName(fStream.Name)
                };
                //set the content header content type as application/octet-stream as it returning file as reponse 
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            }
            return response;
        }


        [HttpGet]
        public async Task<ResponseObject<object>> GetDriverByID(int ID)
        {
            try
            {
                var drivers = db.ExtRepositoryFor<DriverRepository>().GetDriverByID(ID);

                if (drivers != null)
                    return ServiceResponse.SuccessReponse(drivers, MessageEnum.RecordNotFound);
                else
                    return ServiceResponse.SuccessReponse(drivers, MessageEnum.RecordFoundSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }

        [HttpGet]
        public async Task<ResponseObject<List<object>>> GetDriverJobsByID(int ID)
        {
            try
            {
                var lstDetails = db.ExtRepositoryFor<OrderTrackingRepository>().GetDriverJobsByID(ID);

                if (lstDetails.Count() == 0)
                    return ServiceResponse.SuccessReponse(lstDetails, MessageEnum.RecordNotFound);
                else
                    return ServiceResponse.SuccessReponse(lstDetails, MessageEnum.RecordFoundSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }

        [HttpGet]
        public async Task<ResponseObject<List<object>>> GetRegiftDriverJobsByID(int ID)
        {
            try
            {
                var lstDetails = db.ExtRepositoryFor<OrderTrackingRepository>().GetRegiftDriverJobsByID(ID);

                if (lstDetails.Count() == 0)
                    return ServiceResponse.SuccessReponse(lstDetails, MessageEnum.RecordNotFound);
                else
                    return ServiceResponse.SuccessReponse(lstDetails, MessageEnum.RecordFoundSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }

        [HttpGet]
        public async Task<ResponseObject<List<object>>> GetRecycleDriverJobsByID(int ID)
        {
            try
            {
                var lstDetails = db.ExtRepositoryFor<OrderTrackingRepository>().GetRecycleDriverJobsByID(ID);

                if (lstDetails.Count() == 0)
                    return ServiceResponse.SuccessReponse(lstDetails, MessageEnum.RecordNotFound);
                else
                    return ServiceResponse.SuccessReponse(lstDetails, MessageEnum.RecordFoundSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }



        [HttpGet]
        public async Task<ResponseObject<List<object>>> GetBinDriverJobsByID(int ID)
        {
            try
            {
                var lstDetails = db.ExtRepositoryFor<OrderTrackingRepository>().GetBinDriverJobsByID(ID);

                if (lstDetails.Count() == 0)
                    return ServiceResponse.SuccessReponse(lstDetails, MessageEnum.RecordNotFound);
                else
                    return ServiceResponse.SuccessReponse(lstDetails, MessageEnum.RecordFoundSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }
        [HttpGet]
        public async Task<ResponseObject<List<VehicleType>>> GetVechileList()
        {
            try
            {
                List<VehicleType> lstDetails = db.Repository<VehicleType>().GetAll().ToList();
                if (lstDetails.Count() == 0)
                    return ServiceResponse.SuccessReponse(lstDetails, MessageEnum.RecordNotFound);
                else
                    return ServiceResponse.SuccessReponse(lstDetails, MessageEnum.RecordFoundSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<VehicleType>>(exp);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public ResponseObject<DriverTokenViewModel> LoginDriver([FromBody]Driver mdlUser)
        {
            try
            {
                if (mdlUser.Phone == "" || mdlUser.PIN == "")
                {
                    return ServiceResponse.ErrorReponse<DriverTokenViewModel>(MessageEnum.DefaultParametersCanNotBeNull);
                }
                else
                {
                    var user = db.ExtRepositoryFor<DriverRepository>().GetDriverByPhoneAndPIN(mdlUser.Phone, mdlUser.PIN);

                    if (user != null)
                    {
                        object mdlnew = new object();
                        var token = JwtManager.CreateTokenForDriver(user, out mdlnew);
                        return ServiceResponse.SuccessReponse(new DriverTokenViewModel
                        {
                            Token = token,
                            ID = user.ID,
                            FullName = user.FirstName + " " + user.LastName,
                            FileName = user.FileName,
                            Phone = user.Phone,
                            Email = user.Email,
                            CityID = user.CityID,
                            PIN = user.PIN,
                            VehicleId = user.VehicleID,
                            RegNumber = user.RegNumber,
                            LicenseFileName = user.LicienceFileName

                        }, MessageEnum.UserAuthorizedSuccessFully);
                    }
                    else
                    {
                        return ServiceResponse.ErrorReponse<DriverTokenViewModel>(MessageEnum.UserCredentialsAreNotCorrect);
                    }
                }
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<DriverTokenViewModel>(exp);
            }

        }

        [HttpGet]
        public ResponseObject<object> GetDriverAllTasks(string status)
        {
            try
            {
                int? DriverID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                var lstDetails = db.ExtRepositoryFor<OrderTrackingRepository>().GetDriverTasksByID(DriverID, status);
                if (lstDetails == null)
                    return ServiceResponse.SuccessReponse(lstDetails, MessageEnum.RecordNotFound);
                else
                    return ServiceResponse.SuccessReponse(lstDetails, MessageEnum.RecordFoundSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }

        [HttpPost]
        public async Task<ResponseObject<object>> CollectedRecycleItem()
        {
            try
            {

                int orderID = Int32.Parse(HttpContext.Current.Request.Form["orderID"]);
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                if (orderID == 0)
                {
                    return ServiceResponse.ErrorReponse<object>("Invalid order ID");
                }

                if (HttpContext.Current.Request.Files.Count == 0)
                {
                    return ServiceResponse.ErrorReponse<object>("Uploading image is must");
                }

                if (string.IsNullOrEmpty(HttpContext.Current.Request.Form["weight"]))
                {
                    return ServiceResponse.ErrorReponse<object>("Weight for recycle request is must");
                }

                if (string.IsNullOrEmpty(HttpContext.Current.Request.Form["cash"]))
                {
                    return ServiceResponse.ErrorReponse<object>("Cash for recycle request is must");
                }

                if (string.IsNullOrEmpty(HttpContext.Current.Request.Form["greenPoints"]))
                {
                    return ServiceResponse.ErrorReponse<object>("GreenPoints for recycle request is must");
                }

                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                string FileName = await FileOpsHelper.UploadFileNew(file, ContainerName.RECYCLE);

                OrderTracking mdlOrder = db.Repository<OrderTracking>().FindById(orderID);
                Recycle mdlRecycle = db.Repository<Recycle>().FindById(mdlOrder.RsID);
                RecycleSubItem mdlRecycleSubItems = db.Repository<RecycleSubItem>().GetAll().Where(s => s.RecycleID == mdlRecycle.ID).FirstOrDefault();
               // int? lastGreenPoints = mdlRecycleSubItems.GreenPoints;
                mdlOrder.UpdatedDate = DateTime.Now;
                mdlOrder.UpdatedBy = UserID;
                mdlOrder.FileNameTakenByDriver = FileName;
                mdlOrder.StatusID = (int)StatusEnum.Collected;
                mdlRecycle.StatusID = (int)StatusEnum.Complete;
                mdlRecycle.UpdatedDate = DateTime.Now;
                mdlRecycle.UpdatedBy = UserID;
                mdlOrder.CollectedDate = DateTime.Now;
                mdlRecycle.FileName = FileName;
                mdlRecycle.Cash = decimal.Parse(HttpContext.Current.Request.Form["cash"]);
                mdlRecycle.GreenPoints = int.Parse(HttpContext.Current.Request.Form["greenPoints"]);
                mdlRecycleSubItems.UpdatedBy = UserID;
                mdlRecycleSubItems.UpdatedDate = DateTime.Now;
                mdlRecycleSubItems.Weight = int.Parse(HttpContext.Current.Request.Form["weight"]);
                mdlRecycleSubItems.GreenPoints = int.Parse(HttpContext.Current.Request.Form["greenPoints"]);

                //int NewGP = Convert.ToInt32(mdlRecycleSubItems.GreenPoints);
                //db.ExtRepositoryFor<CommonRepository>().UpdateParentsTableGreenPoints(mdlRecycle.UserID, UserID, mdlRecycle.ID, FiveREnum.Recycle.ToString(), Convert.ToInt32( lastGreenPoints), NewGP);


                db.Repository<OrderTracking>().Update(mdlOrder);
                db.Repository<Recycle>().Update(mdlRecycle);
                db.Repository<RecycleSubItem>().Update(mdlRecycleSubItems);
                db.Save();

                PushNotificationEvent _event = new PushNotificationEvent();
                _event.Parameters.Add("GP", int.Parse(HttpContext.Current.Request.Form["greenPoints"]));
                _event.AddPushNotifyEvent((long)NotificationEventConstants.PushNotification.Recycle, Convert.ToString(mdlRecycle.UserID));

                return ServiceResponse.SuccessReponse<object>(mdlOrder, MessageEnum.StatusUpdatedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }

        [HttpPost]
        public ResponseObject<object> CollectedRecycleItemWithList([FromBody] RecycleCollection mdlRecycleCollection)
        {
            try
            {

                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                if (mdlRecycleCollection.OrderID == 0 || mdlRecycleCollection.Cash == 0 || mdlRecycleCollection.GreenPoints == 0)
                {
                    return ServiceResponse.ErrorReponse<object>("Required Parameters are missing");
                }

                OrderTracking mdlOrder = db.Repository<OrderTracking>().FindById(mdlRecycleCollection.OrderID);
                Recycle mdlRecycle = db.Repository<Recycle>().FindById(mdlOrder.RsID);

                mdlOrder.UpdatedDate = DateTime.Now;
                mdlOrder.UpdatedBy = UserID;
                mdlOrder.StatusID = (int)StatusEnum.Collected;
                mdlOrder.CollectedDate = DateTime.Now;
                mdlRecycle.StatusID = (int)StatusEnum.Collected;
                mdlRecycle.UpdatedDate = DateTime.Now;
                mdlRecycle.UpdatedBy = UserID;
                mdlRecycle.Cash = mdlRecycleCollection.Cash;
                mdlRecycle.GreenPoints = mdlRecycleCollection.GreenPoints;

                bool flag = true;
                int? lastGreenPoints = mdlRecycle.GreenPoints;
                List<RecycleSubItem> lstSubItems = new List<RecycleSubItem>();

                foreach (RecycleSubItem subitem in mdlRecycleCollection.RecycleSubItems)
                {
                    lstSubItems.Add(new RecycleSubItem()
                    {
                        Description = subitem.Description,
                        Weight = subitem.Weight,
                        RecycleID = mdlRecycle.ID,
                        IsParent = (flag ? true : false),
                        CreatedBy = UserID,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        GreenPoints = subitem.GreenPoints
                    });

                    flag = false;
                }

                var recycle = db.ExtRepositoryFor<RecycleRepository>().CollectedRecycleByDriver(mdlOrder.RsID, lstSubItems);

                if (recycle == false)
                {
                    return ServiceResponse.SuccessReponse<object>(recycle, MessageEnum.RecordNotFound);
                }

                db.Repository<OrderTracking>().Update(mdlOrder);
                db.Repository<Recycle>().Update(mdlRecycle);
                db.Save();
                int NewGP = Convert.ToInt32(lstSubItems.Sum(x=>x.GreenPoints));
                db.ExtRepositoryFor<CommonRepository>().UpdateParentsTableGreenPoints(mdlRecycle.UserID, UserID, mdlRecycle.ID, FiveREnum.Recycle.ToString(), Convert.ToInt32(lastGreenPoints), NewGP);

                return ServiceResponse.SuccessReponse<object>(mdlOrder, MessageEnum.StatusUpdatedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }

        [HttpPost]
        public async Task<ResponseObject<object>> CollectedRecycleItemWithFile()
        {
            try
            {
                int orderID = Int32.Parse(HttpContext.Current.Request.Form["orderID"]);
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                if (orderID == 0)
                {
                    return ServiceResponse.ErrorReponse<object>("Invalid order ID");
                }

                if (HttpContext.Current.Request.Files.Count == 0)
                {
                    return ServiceResponse.ErrorReponse<object>("Uploading image is must");
                }

                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                string FileName = await FileOpsHelper.UploadFileNew(file, ContainerName.RECYCLE);

                OrderTracking mdlOrder = db.Repository<OrderTracking>().FindById(orderID);
                Recycle mdlRecycle = db.Repository<Recycle>().FindById(mdlOrder.RsID);

                mdlOrder.UpdatedDate = DateTime.Now;
                mdlOrder.UpdatedBy = UserID;
                mdlOrder.StatusID = (int)StatusEnum.Collected;
                mdlOrder.FileNameTakenByDriver = FileName;
                mdlOrder.CollectedDate = DateTime.Now;
                mdlRecycle.UpdatedDate = DateTime.Now;
                mdlRecycle.UpdatedBy = UserID;
                mdlRecycle.StatusID = (int)StatusEnum.Collected;

                db.Repository<OrderTracking>().Update(mdlOrder);
                db.Repository<Recycle>().Update(mdlRecycle);
                db.Save();

                PushNotificationEvent _event = new PushNotificationEvent();
                _event.Parameters.Add("AmalID", mdlRecycle.ID);
                _event.AddPushNotifyEvent((long)NotificationEventConstants.PushNotification.RecycleCollected, UserID.ToString());

                PushNotificationEvent _eventNew = new PushNotificationEvent();
                _eventNew.Parameters.Add("AmalID", mdlRecycle.ID);
                _eventNew.AddPushNotifyEvent((long)NotificationEventConstants.PushNotification.RcycleCollectedRedirect, UserID.ToString());

                return ServiceResponse.SuccessReponse<object>(mdlOrder, MessageEnum.StatusUpdatedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }

        [HttpPost]
        public ResponseObject<object> CollectedRegiftItemWithList([FromBody] RegiftCollection mdlRegiftCollection)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                if (mdlRegiftCollection.OrderID == 0 || mdlRegiftCollection.Quality == null || mdlRegiftCollection.GreenPoints == 0)
                {
                    return ServiceResponse.ErrorReponse<object>("Required Parameters are missing");
                }

                OrderTracking mdlOrder = db.Repository<OrderTracking>().FindById(mdlRegiftCollection.OrderID);
                Regift mdlRegift = db.Repository<Regift>().FindById(mdlOrder.RsID);

                mdlOrder.UpdatedDate = DateTime.Now;
                mdlOrder.UpdatedBy = UserID;
                mdlOrder.StatusID = (int)StatusEnum.Collected;
                mdlOrder.CollectedDate = DateTime.Now;
                mdlRegift.StatusID = (int)StatusEnum.Collected;
                mdlRegift.UpdatedDate = DateTime.Now;
                mdlRegift.UpdatedBy = UserID;
                mdlRegift.Quality = mdlRegiftCollection.Quality;

                bool flag = true;
                int? lastGreenPoints = mdlRegift.GreenPoints;
                List<RegiftSubItem> lstSubItems = new List<RegiftSubItem>();

                foreach (RegiftSubItem subitem in mdlRegiftCollection.RegiftSubItems)
                {
                    lstSubItems.Add(new RegiftSubItem()
                    {
                        TypeID = subitem.TypeID,
                        Qty = subitem.Qty,
                        RegiftID = mdlRegift.ID,
                        IsParent = (flag ? true : false),
                        CreatedBy = UserID,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        GreenPoints = subitem.GreenPoints
                        
                    });
                    mdlRegift.GreenPoints += Convert.ToInt32(subitem.GreenPoints);

                    flag = false;
                }

                var regift1 = db.ExtRepositoryFor<RegiftRepository>().CollectedRegiftByDriver(mdlOrder.RsID, lstSubItems);

                if (regift1 == null)
                {
                    return ServiceResponse.SuccessReponse<object>(regift1, MessageEnum.RecordNotFound);
                }

                db.Repository<OrderTracking>().Update(mdlOrder);
                db.Repository<Regift>().Update(mdlRegift);
                db.Save();
                int NewGP = Convert.ToInt32(lstSubItems.Sum(x=>x.GreenPoints));
                db.ExtRepositoryFor<CommonRepository>().UpdateParentsTableGreenPoints(mdlRegift.UserID, UserID, mdlRegift.ID, FiveREnum.Regift.ToString(), Convert.ToInt32(lastGreenPoints), NewGP);

                return ServiceResponse.SuccessReponse<object>(mdlOrder, MessageEnum.StatusUpdatedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }

        [HttpPost]
        public async Task<ResponseObject<object>> CollectedRegiftItemWithFile()
        {
            try
            {
                int orderID = Int32.Parse(HttpContext.Current.Request.Form["orderID"]);
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                if (orderID == 0)
                {
                    return ServiceResponse.ErrorReponse<object>("Invalid order ID");
                }

                if (HttpContext.Current.Request.Files.Count == 0)
                {
                    return ServiceResponse.ErrorReponse<object>("Uploading image is must");
                }

                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                string FileName = await FileOpsHelper.UploadFileNew(file, ContainerName.RECYCLE);

                OrderTracking mdlOrder = db.Repository<OrderTracking>().FindById(orderID);
                Regift mdlRegift = db.Repository<Regift>().FindById(mdlOrder.RsID);

                mdlOrder.UpdatedDate = DateTime.Now;
                mdlOrder.UpdatedBy = UserID;
                mdlOrder.StatusID = (int)StatusEnum.Collected;
                mdlOrder.FileNameTakenByDriver = FileName;
                mdlOrder.CollectedPendingConfirmation = false;
                mdlOrder.CollectedDate = DateTime.Now;
                mdlRegift.UpdatedDate = DateTime.Now;
                mdlRegift.UpdatedBy = UserID;
                mdlRegift.StatusID = (int)StatusEnum.Collected;

                db.Repository<OrderTracking>().Update(mdlOrder);
                db.Repository<Regift>().Update(mdlRegift);
                db.Save();

                PushNotificationEvent _event = new PushNotificationEvent();
                _event.Parameters.Add("AmalID", mdlRegift.ID);
                _event.AddPushNotifyEvent((long)NotificationEventConstants.PushNotification.RegiftCollected, UserID.ToString());

                PushNotificationEvent _eventNew = new PushNotificationEvent();
                _eventNew.Parameters.Add("AmalID", mdlRegift.ID);
                _eventNew.AddPushNotifyEvent((long)NotificationEventConstants.PushNotification.RegiftCollectedRedirect, UserID.ToString());

                return ServiceResponse.SuccessReponse<object>(mdlOrder, MessageEnum.StatusUpdatedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }

        [HttpPost]
        public async Task<ResponseObject<object>> DeliveredBin()
        {
            try
            {
                int orderID = Int32.Parse(HttpContext.Current.Request.Form["orderID"]);
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                if (orderID == 0)
                {
                    return ServiceResponse.ErrorReponse<object>("Invalid order ID");
                }

                if (HttpContext.Current.Request.Files.Count == 0)
                {
                    return ServiceResponse.ErrorReponse<object>("Uploading image is must");
                }

                if (string.IsNullOrEmpty(HttpContext.Current.Request.Form["count"]))
                {
                    return ServiceResponse.ErrorReponse<object>("Count for bin request is must");
                }

                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                string FileName = await FileOpsHelper.UploadFileNew(file, ContainerName.RECYCLE);
                string Count = Convert.ToString(HttpContext.Current.Request.Form["count"]);
                OrderTracking mdlOrder = db.Repository<OrderTracking>().FindById(orderID);
                BuyBin mdlBin = db.Repository<BuyBin>().FindById(mdlOrder.RsID);

                RefTable mdlTable = db.Repository<RefTable>().GetAll().Where(x => x.Type == "GP").FirstOrDefault();
               
                int? GPValue = mdlTable.GreenPointValue;
                int count = Convert.ToInt32(Count);

                int? lastGreenPoints = (GPValue * count);
                mdlOrder.UpdatedDate = DateTime.Now;
                mdlOrder.UpdatedBy = UserID;
                mdlOrder.StatusID = (int)StatusEnum.Delivered;
                mdlOrder.FileNameTakenByDriver = FileName;
                mdlOrder.DeliveredDate= DateTime.Now;
                mdlBin.UpdatedDate = DateTime.Now;
                mdlBin.UpdatedBy = UserID;
                // mdlBin.FileName = FileName;
                mdlBin.Qty = count;
                mdlBin.GreenPoints = (GPValue * count);
                mdlBin.StatusID = (int)StatusEnum.Delivered;

                db.Repository<OrderTracking>().Update(mdlOrder);
                db.Repository<BuyBin>().Update(mdlBin);
                db.Save();
              
                int NewGP = Convert.ToInt32(mdlBin.GreenPoints);
                db.ExtRepositoryFor<CommonRepository>().UpdateParentsTableGreenPoints(mdlBin.UserID, UserID, mdlBin.ID, FiveREnum.Regift.ToString(), Convert.ToInt32(lastGreenPoints), NewGP);

                PushNotificationEvent _event = new PushNotificationEvent();
                _event.AddPushNotifyEvent((long)NotificationEventConstants.PushNotification.Bin, Convert.ToString(mdlBin.UserID));



                return ServiceResponse.SuccessReponse<object>(mdlOrder, MessageEnum.StatusUpdatedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }

        [HttpPost]
        public async Task<ResponseObject<object>> DeliveredRegiftItem()
        {
            try
            {
                int orderID = Int32.Parse(HttpContext.Current.Request.Form["orderID"]);
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                if (orderID == 0)
                {
                    return ServiceResponse.ErrorReponse<object>("Invalid order ID");
                }

                if (HttpContext.Current.Request.Files.Count == 0)
                {
                    return ServiceResponse.ErrorReponse<object>("Uploading image is must");
                }

                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                string FileName = await FileOpsHelper.UploadFileNew(file, ContainerName.RECYCLE);

                OrderTracking mdlOrder = db.Repository<OrderTracking>().FindById(orderID);
                Regift mdlRegift = db.Repository<Regift>().FindById(mdlOrder.RsID);

                mdlOrder.UpdatedDate = DateTime.Now;
                mdlOrder.UpdatedBy = UserID;
                mdlOrder.StatusID = (int)StatusEnum.Delivered;
                mdlOrder.FileNameTakenByOrg = FileName;
                mdlOrder.DeliveredPendingConfirmation = false;
                mdlOrder.DeliveredDate= DateTime.Now;
                mdlRegift.UpdatedDate = DateTime.Now;
                mdlRegift.UpdatedBy = UserID;
                mdlRegift.StatusID = (int)StatusEnum.Delivered;

                db.Repository<OrderTracking>().Update(mdlOrder);
                db.Repository<Regift>().Update(mdlRegift);
                db.Save();

                PushNotificationEvent _event = new PushNotificationEvent();
                _event.Parameters.Add("AmalID", mdlRegift.ID);
                _event.AddPushNotifyEvent((long)NotificationEventConstants.PushNotification.RegiftDelivered, UserID.ToString());

                return ServiceResponse.SuccessReponse<object>(mdlOrder, MessageEnum.StatusUpdatedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }

        [HttpPost]
        public async Task<ResponseObject<object>> DeliveredRecycleItem()
        {
            try
            {
                int orderID = Int32.Parse(HttpContext.Current.Request.Form["orderID"]);
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                if (orderID == 0)
                {
                    return ServiceResponse.ErrorReponse<object>("Invalid order ID");
                }

                if (HttpContext.Current.Request.Files.Count == 0)
                {
                    return ServiceResponse.ErrorReponse<object>("Uploading image is must");
                }

                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                string FileName = await FileOpsHelper.UploadFileNew(file, ContainerName.RECYCLE);

                OrderTracking mdlOrder = db.Repository<OrderTracking>().FindById(orderID);
                Recycle mdlRecycle = db.Repository<Recycle>().FindById(mdlOrder.RsID);

                mdlOrder.UpdatedDate = DateTime.Now;
                mdlOrder.UpdatedBy = UserID;
                mdlOrder.StatusID = (int)StatusEnum.Delivered;
                mdlOrder.FileNameTakenByOrg = FileName;
                mdlOrder.DeliveredPendingConfirmation = false;
                mdlOrder.DeliveredDate = DateTime.Now;
                mdlRecycle.UpdatedDate = DateTime.Now;
                mdlRecycle.UpdatedBy = UserID;
                mdlRecycle.StatusID = (int)StatusEnum.Delivered;

                db.Repository<OrderTracking>().Update(mdlOrder);
                db.Repository<Recycle>().Update(mdlRecycle);
                db.Save();

                PushNotificationEvent _event = new PushNotificationEvent();
                _event.Parameters.Add("AmalID", mdlRecycle.ID);
                _event.AddPushNotifyEvent((long)NotificationEventConstants.PushNotification.RegiftDelivered, UserID.ToString());

                return ServiceResponse.SuccessReponse<object>(mdlOrder, MessageEnum.StatusUpdatedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }

        [HttpPost]
        public ResponseObject<object> RejectedRegiftItem(int orderID = 0)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                if (orderID == 0)
                {
                    return ServiceResponse.ErrorReponse<object>("Invalid order ID");
                }

                OrderTracking mdlOrder = db.Repository<OrderTracking>().FindById(orderID);
                Regift mdlRegift = db.Repository<Regift>().FindById(mdlOrder.RsID);

                mdlOrder.UpdatedDate = DateTime.Now;
                mdlOrder.UpdatedBy = UserID;
                mdlOrder.StatusID = (int)StatusEnum.Declined;
                mdlRegift.UpdatedDate = DateTime.Now;
                mdlRegift.UpdatedBy = UserID;
                mdlRegift.StatusID = (int)StatusEnum.Declined;

                db.Repository<OrderTracking>().Update(mdlOrder);
                db.Repository<Regift>().Update(mdlRegift);
                db.Save();

                //List<RegiftSubItem> subitems = db.Repository<RegiftSubItem>().GetAll().Where(x => x.RegiftID == mdlRegift.ID).ToList();

                //foreach (var item in subitems)
                //{
                //    item.GreenPoints = 0;
                //    db.Repository<RegiftSubItem>().Update(item);
                //    db.Save();
                //}

                return ServiceResponse.SuccessReponse<object>(mdlOrder, MessageEnum.StatusUpdatedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }

        [HttpPost]
        public ResponseObject<object> RejectedRecycleItem(int orderID = 0)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                if (orderID == 0)
                {
                    return ServiceResponse.ErrorReponse<object>("Invalid order ID");
                }

                OrderTracking mdlOrder = db.Repository<OrderTracking>().FindById(orderID);
                Recycle mdlRecycle = db.Repository<Recycle>().FindById(mdlOrder.RsID);

                mdlOrder.UpdatedDate = DateTime.Now;
                mdlOrder.UpdatedBy = UserID;
                mdlOrder.StatusID = (int)StatusEnum.Declined;
                mdlRecycle.UpdatedDate = DateTime.Now;
                mdlRecycle.UpdatedBy = UserID;
                mdlRecycle.StatusID = (int)StatusEnum.Declined;

                db.Repository<OrderTracking>().Update(mdlOrder);
                db.Repository<Recycle>().Update(mdlRecycle);
                db.Save();

                //List<RegiftSubItem> subitems = db.Repository<RegiftSubItem>().GetAll().Where(x => x.RegiftID == mdlRegift.ID).ToList();

                //foreach (var item in subitems)
                //{
                //    item.GreenPoints = 0;
                //    db.Repository<RegiftSubItem>().Update(item);
                //    db.Save();
                //}

                return ServiceResponse.SuccessReponse<object>(mdlOrder, MessageEnum.StatusUpdatedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }

        public ResponseObject<object> NoShowRegiftItem(int orderID = 0)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                if (orderID == 0)
                {
                    return ServiceResponse.ErrorReponse<object>("Invalid order ID");
                }

                OrderTracking mdlOrder = db.Repository<OrderTracking>().FindById(orderID);
                Regift mdlRegift = db.Repository<Regift>().FindById(mdlOrder.RsID);

                mdlOrder.UpdatedDate = DateTime.Now;
                mdlOrder.UpdatedBy = UserID;
                mdlOrder.StatusID = (int)StatusEnum.NoShow;
                mdlRegift.UpdatedDate = DateTime.Now;
                mdlRegift.UpdatedBy = UserID;
                mdlRegift.StatusID = (int)StatusEnum.NoShow;

                db.Repository<OrderTracking>().Update(mdlOrder);
                db.Repository<Regift>().Update(mdlRegift);
                db.Save();

                return ServiceResponse.SuccessReponse<object>(mdlOrder, MessageEnum.StatusUpdatedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }

        public ResponseObject<object> NoShowRecycleItem(int orderID = 0)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                if (orderID == 0)
                {
                    return ServiceResponse.ErrorReponse<object>("Invalid order ID");
                }

                OrderTracking mdlOrder = db.Repository<OrderTracking>().FindById(orderID);
                Recycle mdlRecycle = db.Repository<Recycle>().FindById(mdlOrder.RsID);

                mdlOrder.UpdatedDate = DateTime.Now;
                mdlOrder.UpdatedBy = UserID;
                mdlOrder.StatusID = (int)StatusEnum.NoShow;
                mdlRecycle.UpdatedDate = DateTime.Now;
                mdlRecycle.UpdatedBy = UserID;
                mdlRecycle.StatusID = (int)StatusEnum.NoShow;

                db.Repository<OrderTracking>().Update(mdlOrder);
                db.Repository<Recycle>().Update(mdlRecycle);
                db.Save();

                return ServiceResponse.SuccessReponse<object>(mdlOrder, MessageEnum.StatusUpdatedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }

        public ResponseObject<object> NoShowDeliveryBin(int orderID = 0)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);

                if (orderID == 0)
                {
                    return ServiceResponse.ErrorReponse<object>("Invalid order ID");
                }

                OrderTracking mdlOrder = db.Repository<OrderTracking>().FindById(orderID);
                BuyBin mdlBin = db.Repository<BuyBin>().FindById(mdlOrder.RsID);

                mdlOrder.UpdatedDate = DateTime.Now;
                mdlOrder.UpdatedBy = UserID;
                mdlOrder.StatusID = (int)StatusEnum.NoShow;
                mdlBin.UpdatedDate = DateTime.Now;
                mdlBin.UpdatedBy = UserID;
                mdlBin.StatusID = (int)StatusEnum.NoShow;

                db.Repository<OrderTracking>().Update(mdlOrder);
                db.Repository<BuyBin>().Update(mdlBin);
                db.Save();

                return ServiceResponse.SuccessReponse<object>(mdlOrder, MessageEnum.StatusUpdatedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }

        [HttpPost]
        public ResponseObject<bool> SuspendDriver(int id)
        {
            try
            {
                // select* from orderTracking where assignto = 41 and statusid = 9
                List<OrderTracking> lstOrderTracking = db.Repository<OrderTracking>().GetAll().Where(x => x.AssignTo == id && x.StatusID == (int)StatusEnum.Assigned).ToList();
                if (lstOrderTracking.Count > 0)
                    return ServiceResponse.SuccessReponse(false, MessageEnum.SuspendSuccessfully);

                Driver driver = db.Repository<Driver>().GetAll().Where(x => x.ID == id).FirstOrDefault();

                driver.IsActive = false;
               
                db.Repository<Driver>().Update(driver);
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
