using DrTech.Amal.Common.Enums;
using DrTech.Amal.Common.Helpers;
using DrTech.Amal.Common.ServerResponse;
using DrTech.Amal.Notifications;
//using DrTech.Amal.SQLDataAccess.CustomModels;
using DrTech.Amal.SQLServices.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using static DrTech.Amal.Common.Extentions.Constants;
using DrTech.Amal.SQLModels;
using DrTech.Amal.SQLDataAccess.Repository;
using DrTech.Amal.SQLDataAccess.CustomModels;
using System.Data.Entity.Validation;
using DrTech.Amal.SQLServices.Models;
using OfficeOpenXml;
using System.Web.Http.Cors;
using static DrTech.Amal.SQLDataAccess.Repository.MyWasteRepository;

namespace DrTech.Amal.SQLServices.Controllers
{
    // [Authorize]
    public class MyWasteController : BaseController
    {
        [HttpPost]  
        public async Task<ResponseObject<TokenViewModel>> MyWasteUserRegistration()
        {
            try
            {
                User _mdlUser = new User();
                TokenViewModel mdlModel = new TokenViewModel();
                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    HttpPostedFile file = HttpContext.Current.Request.Files[0];
                    _mdlUser.FileName = await FileOpsHelper.UploadFileNew(file, ContainerName.USER);
                }
                else
                {
                    if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["file"]))
                        _mdlUser.FileName = HttpContext.Current.Request.Form["file"].ToString();
                }
                string BusinessKey = string.Empty;
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["fullName"]))
                    _mdlUser.FullName = HttpContext.Current.Request.Form["fullName"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["email"]))
                    _mdlUser.Email = HttpContext.Current.Request.Form["email"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["address"]))
                    _mdlUser.Address = HttpContext.Current.Request.Form["address"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["phone"]))
                    _mdlUser.Phone = HttpContext.Current.Request.Form["phone"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["password"]))
                    _mdlUser.Password = HttpContext.Current.Request.Form["password"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["longitude"]))
                    _mdlUser.Longitude = Convert.ToDecimal(HttpContext.Current.Request.Form["longitude"]);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["latitude"]))
                    _mdlUser.Latitude = Convert.ToDecimal(HttpContext.Current.Request.Form["latitude"]);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["city"]))
                    _mdlUser.City = HttpContext.Current.Request.Form["city"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["cityID"]))
                    _mdlUser.CityId = Convert.ToInt32(HttpContext.Current.Request.Form["cityID"]);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["deviceToken"]))
                    _mdlUser.DeviceToken = HttpContext.Current.Request.Form["deviceToken"];


                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["areaID"]))
                    _mdlUser.AreaID = Convert.ToInt32(HttpContext.Current.Request.Form["areaID"]);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["unionCouncil"]))
                    _mdlUser.UnionCouncil = HttpContext.Current.Request.Form["unionCouncil"];

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["businessKey"]))
                    BusinessKey = HttpContext.Current.Request.Form["businessKey"];
                BusinessKey = "ghs714310";
                _mdlUser.CompanyID = db.ExtRepositoryFor<CommonRepository>().GetCompanyIdFromBusinessKey(BusinessKey);

                _mdlUser.Type = "W";
                _mdlUser.RoleID = (int)UserRoleTypeEnum.MobileUser;

                User mdlUser = db.Repository<User>().GetAll().Where(x => x.Phone == _mdlUser.Phone).FirstOrDefault();

                if (mdlUser != null)
                    return ServiceResponse.SuccessReponse(mdlModel, MessageEnum.UserAlreadyExist);

                StringBuilder CodeModel = new StringBuilder();
                CodeModel.Append(_mdlUser.ID + ";");
                CodeModel.Append(_mdlUser.Email + ";");
                CodeModel.Append(_mdlUser.Phone + ";");
                CodeModel.Append(_mdlUser.FullName);
               // string QRCode = QRCodeTagHelper.QRCodeGenerator(CodeModel);
                int _min = 1000;
                int _max = 9999;
                Random random = new Random();
                Int32 number = random.Next(_min, _max);

                _mdlUser.IsVerified = false;
                SMSNotifyEvent _events = new SMSNotifyEvent();
                _events.Parameters.Add("SMSCode", number);
                _events.AddSMSNotifyEvent((long)NotificationEventConstants.Users.SendVerificationCodeSMS, _mdlUser.Phone);


                db.Repository<User>().Insert(_mdlUser);
                db.Save();
                Int32 ID = _mdlUser.ID;

                LookupType mdllookup = db.Repository<LookupType>().GetAll().Where(x => x.ID == _mdlUser.CityId).FirstOrDefault();

                string CityName = "";
                if (mdllookup != null)
                {
                    CityName = mdllookup.Name;
                }

                object mdlnew = new object();
                var token = JwtManager.CreateToken(_mdlUser, out mdlnew);
                return ServiceResponse.SuccessReponse(new TokenViewModel
                {
                    Token = token,
                    ID = ID,
                    FullName = _mdlUser.FullName,
                    Address = _mdlUser.Address,
                    Email = _mdlUser.Email,
                    Phone = _mdlUser.Phone,
                    FileName = _mdlUser.FileName,
                    Longitude = Convert.ToDecimal(_mdlUser.Longitude),
                    Latitude = Convert.ToDecimal(_mdlUser.Latitude),
                    QRCode = _mdlUser.QRCode,
                    GreenPoints = Convert.ToInt32(_mdlUser.GreenPoints),
                    Password = _mdlUser.Password,
                    SMSCode = number,
                    City = CityName,
                    DeviceToken = _mdlUser.DeviceToken,
                    AreaID = _mdlUser.AreaID,
                    UnionCouncil = _mdlUser.UnionCouncil,
                    CompanyID = _mdlUser.CompanyID,
                    CityId = _mdlUser.CityId,
                     BusinessKey = db.ExtRepositoryFor<CommonRepository>().GetBusinessKeyCompanyIdFrom(_mdlUser.CompanyID)

                }, MessageEnum.MyWasteRegistration);

            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<TokenViewModel>(exp);
            }
        }

        [HttpGet]
        public async Task<ResponseObject<List<Area>>> GetAllArea()
        {
           
            try
            {
                List<Area> lstArea =  db.Repository<Area>().GetAll().ToList();
           

                return ServiceResponse.SuccessReponse(lstArea, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Area>>(exp);
            }
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<ResponseObject<List<City>>> GetAllCity()
        {
            try
            {
                List<City> lstCity = db.Repository<City>().GetAll().OrderBy(x=>x.CityName).ToList();

                return ServiceResponse.SuccessReponse(lstCity, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<City>>(exp);
            }
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<ResponseObject<List<Area>>> GetAreaByID(int ID)
        {
            try
            {
                List<Area> lstArea = db.Repository<Area>().GetAll().Where(x => x.CityID == ID).OrderBy(x=>x.Name).ToList();

                return ServiceResponse.SuccessReponse(lstArea, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Area>>(exp);
            }
        }

        [HttpPost]
        public ResponseObject<bool> AddWasteSchedule(List<MSchedule> lstSchedule)
        {
            if (lstSchedule == null)
                return ServiceResponse.ErrorReponse<bool>(MessageEnum.KidsModelNotNull);
            try
            {
                //  5967;
                int? UserID =  JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                User mdlUser = db.Repository<User>().GetAll().Where(x => x.ID == UserID).FirstOrDefault();
                MSchedule schedule = lstSchedule.Single(x => x.AreaID != 0);
                List<WetWasteSchedule> ExitData = db.Repository<WetWasteSchedule>().GetAll().Where(x => x.CreatedBy == UserID && x.AreaID == schedule.AreaID && x.Month == schedule.Date.Month && x.Year == schedule.Date.Year).ToList();
                if (ExitData.Count > 0)
                {
                    return ServiceResponse.SuccessReponse(false, "Record already Exits!");
                }
                else
                {
                    lstSchedule.Remove(schedule);
                    var t = schedule;
                    List<ScheduleDetail> lstSD = new List<ScheduleDetail>();
                    foreach (var s in lstSchedule)
                    {
                        lstSD.Add(new ScheduleDetail
                        {
                            Date = s.Date.AddDays(1),
                            FromTime = Convert.ToDateTime(s.FromTime).TimeOfDay,
                            ToTime = Convert.ToDateTime(s.ToTime).TimeOfDay,
                            Day = s.Day,
                            IsActive = true,
                            CreatedBy = UserID,
                            CreatedDate = DateTime.Now

                        });

                        //var sech = new ScheduleDetail
                        //{
                        //    FromTime = s.fromTime.ToLocalTime().TimeOfDay,
                        //    ToTime = s.toTime.ToLocalTime().TimeOfDay,
                        //    Day = s.day,
                        //    IsActive = true,
                        //    CreatedBy = UserID,
                        //    CreatedDate = DateTime.Now,
                        //    ParentID = wasteSchedule.ID
                        //};
                        //db.Repository<ScheduleDetail>().Insert(sech);
                        //db.Save();
                    }

                    WetWasteSchedule wasteSchedule = new WetWasteSchedule();
                    wasteSchedule.CityID = schedule.CityID;
                    wasteSchedule.AreaID = schedule.AreaID;
                    wasteSchedule.DriverID = schedule.DriverID;
                    wasteSchedule.ScheduleDate = schedule.Date;
                    wasteSchedule.FromTime = schedule.fTime.ToLocalTime().TimeOfDay;
                    wasteSchedule.ToTime = schedule.tTime.ToLocalTime().TimeOfDay;
                    wasteSchedule.Day = schedule.Date.Day;
                    wasteSchedule.Month = schedule.Date.Month;
                    wasteSchedule.Year = schedule.Date.Year;
                    wasteSchedule.CompanyID = mdlUser.CompanyID;
                    wasteSchedule.CreatedDate = DateTime.Now;
                    wasteSchedule.IsActive = true;
                    wasteSchedule.ScheduleDetails = lstSD;
                    wasteSchedule.CreatedBy = UserID;

                    db.Repository<WetWasteSchedule>().Insert(wasteSchedule);
                    db.Save();





                    //Member MemberExist = db.Repository<Member>().GetAll().Where(x => x.NGOId == mdlMem.NGOId && x.UserId == UserID).FirstOrDefault();
                    //if (MemberExist != null)
                    //    return ServiceResponse.SuccessReponse(true, MessageEnum.NGOEmpAlreadyAdded);


                    //TimeSpan sTimeSpan = mdlModel.StartTime.ToLocalTime().TimeOfDay;
                    //TimeSpan eTimeSpan = mdlModel.EndTime.ToLocalTime().TimeOfDay;

                    //WetWasteSchedule mdlSchdule = new WetWasteSchedule
                    //{
                    //    AreaID = mdlModel.AreaID,
                    //    //FromTime = sTimeSpan,
                    //   //ToTime = eTimeSpan,
                    //    Month = mdlModel.SchduleDate.Month,
                    //    Day = mdlModel.SchduleDate.Day,
                    //    Year = mdlModel.SchduleDate.Year,
                    //    ScheduleDate = mdlModel.SchduleDate,
                    //    CreatedBy = (int)UserID,
                    //    CreatedDate = DateTime.Now,
                    //    IsActive = true
                    //};

                    //db.Repository<WetWasteSchedule>().Insert(mdlSchdule);
                    //db.Save();


                    return ServiceResponse.SuccessReponse(true, MessageEnum.EmpAddedSuccessfully);
                }
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


        public ResponseObject<List<MSchedule>> ImportExcel()
        {
            try
            {
                List<MSchedule> lstSchedule = new List<MSchedule>();
                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    HttpPostedFile file = HttpContext.Current.Request.Files[0];
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                    
                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        for (int rowIterators = 1; rowIterators <= 1; rowIterators++)
                        {
                            if (rowIterators == 1)
                            {
                                if (workSheet.Cells[rowIterators, 1].Value.ToString() == "Date" &&
                                    workSheet.Cells[rowIterators, 2].Value.ToString() == "Day" &&
                                    workSheet.Cells[rowIterators, 3].Value.ToString() == "FromTime" &&
                                    workSheet.Cells[rowIterators, 4].Value.ToString() == "ToTime" 
                                  )
                                {
                                    for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                                    {
                                        var schedule = new MSchedule();
                                        if (workSheet.Cells[rowIterator, 1].Value != null)
                                            schedule.Date = Convert.ToDateTime(workSheet.Cells[rowIterator, 1].Value);


                                        if (workSheet.Cells[rowIterator, 2].Value != null)
                                            schedule.Day = workSheet.Cells[rowIterator, 2].Value.ToString();


                                        if (workSheet.Cells[rowIterator, 3].Value != null)
                                            schedule.FromTime = workSheet.Cells[rowIterator, 3].Value.ToString();


                                        if (workSheet.Cells[rowIterator, 4].Value != null)
                                            schedule.ToTime = Convert.ToString(workSheet.Cells[rowIterator, 4].Value);

                                        schedule.Active = true;
                                        schedule.Status = "Active";




                                        lstSchedule.Add(schedule);
                                       
                                        
                                    }
                                }                                
                            }
                        }
                    }





                }
                return ServiceResponse.SuccessReponse(lstSchedule, MessageEnum.RecordFoundSuccessfully);
            }
            catch (Exception exp)
            {

                return ServiceResponse.ErrorReponse<List<MSchedule>>(exp);
            }
          
        }
        [HttpGet]
        public async Task<ResponseObject<List<object>>> GetWetWasteSchedule(int AreaID, int CompanyID=0)
        {
            try
            {
                List<object> lstSchdule = db.ExtRepositoryFor<MyWasteRepository>().GetScheduleDetails(AreaID, 0).ToList();
                if (lstSchdule.Count > 0)
                    return ServiceResponse.SuccessReponse(lstSchdule, MessageEnum.DefaultSuccessMessage);
                else
                    return ServiceResponse.SuccessReponse(lstSchdule, "Record Not Found!");
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }


        [HttpGet]
        public async Task<ResponseObject<List<DesegregatedDataViewModel>>> GetDesegregatedList()
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                List<DesegregatedDataViewModel> lstDesegregated = db.ExtRepositoryFor<MyWasteRepository>().GetDesegregatedList().ToList();
                if (lstDesegregated.Count > 0)
                    return ServiceResponse.SuccessReponse(lstDesegregated, MessageEnum.DefaultSuccessMessage);
                else
                    return ServiceResponse.SuccessReponse(lstDesegregated, "Record Not Found!");
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<DesegregatedDataViewModel>>(exp);
            }
        }
        [HttpPost]
        public async Task<ResponseObject<List<DesegregatedDataViewModel>>> GetDesegregatedListBetweenTwoDates(DateRangeViewMdoel model)
        {
            try
            {

               model.start =   model.start.AddDays(1);
               model.end =  model.end.AddDays(1);

                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                List<DesegregatedDataViewModel> lstDesegregated = db.ExtRepositoryFor<MyWasteRepository>().GetDesegregatedListBetweenTwoDates(model);
                if (lstDesegregated.Count > 0)
                    return ServiceResponse.SuccessReponse(lstDesegregated, MessageEnum.DefaultSuccessMessage);
                else
                    return ServiceResponse.SuccessReponse(lstDesegregated, "Record Not Found!");
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<DesegregatedDataViewModel>>(exp);
            }
        }

        [HttpGet]
        public async Task<ResponseObject <object>> GetDesegregatedByID(int ID)
        {
            try
            {
                object lstCycl = db.ExtRepositoryFor<MyWasteRepository>().DesegregatedByID(ID);
                return ServiceResponse.SuccessReponse(lstCycl, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }

        [HttpGet]
        public async Task<ResponseObject<bool>> IsDataSegregated(int RecycleID)
        {
            try
            {
                bool IsSegregated = db.ExtRepositoryFor<MyWasteRepository>().IsSegregated(RecycleID);
                return ServiceResponse.SuccessReponse(IsSegregated, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpGet]
        public async Task<ResponseObject<List<SegregatedDataViewModel>>> GetSegregatedDataByID(int RecycleID)
        {
            try
            {
                List< SegregatedDataViewModel> lstCycl = db.ExtRepositoryFor<MyWasteRepository>().GetSegregatedDataByID(RecycleID);
                return ServiceResponse.SuccessReponse(lstCycl, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<SegregatedDataViewModel>>(exp);
            }
        }
        [HttpPost]
        public async Task<ResponseObject<List<SegregatedDataViewModel>>> GetSegregatedDataBetweenTwoDates(DateRangeViewMdoel model)
        {
            try
            {

                model.start = model.start.AddDays(1);
                model.end = model.end.AddDays(1);
                List<SegregatedDataViewModel> lstDesegregated = db.ExtRepositoryFor<MyWasteRepository>().GetSegregatedDataByDate(model);
                if (lstDesegregated.Count > 0)
                    return ServiceResponse.SuccessReponse(lstDesegregated, MessageEnum.DefaultSuccessMessage);
                else
                    return ServiceResponse.SuccessReponse(lstDesegregated, "Record Not Found!");
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<SegregatedDataViewModel>>(exp);
            }
        }


        [HttpGet] 
        public async Task<ResponseObject<bool>> CheckStatusDesegregated(int ID)
        {
            try
            {
                Recycle lstCycl = db.Repository<Recycle>().GetAll().Where(x => x.ID == ID).FirstOrDefault();
                if (lstCycl.IsActive == true)
                {
                    return ServiceResponse.SuccessReponse(true, MessageEnum.DefaultSuccessMessage);
                }
                return ServiceResponse.SuccessReponse(false, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }


        [HttpGet]
        public async Task<ResponseObject<User>>UserPaymnet(int ID, decimal Amount, string Title)
        {
            try
            {
                User userlist = db.Repository<User>().GetAll().Where(x => x.ID == ID).FirstOrDefault();
                if (userlist.WalletBalance == null)
                {
                    userlist.WalletBalance = Amount;
                }
                else
                {
                    userlist.WalletBalance += Amount;
                }
                db.Repository<User>().Update(userlist);
                db.Save();
                UserPayment userPayment = new UserPayment();
                userPayment.AmountPaid = Amount;
                userPayment.UserID = userlist.ID;
                userPayment.Email = userlist.Email;
                userPayment.IsSuccess = "Success";
                userPayment.IsActive = true;
                userPayment.CreatedDate = DateTime.Now;
                userPayment.Mobile = userlist.Phone;
                userPayment.transactionResponse = Title;
                db.Repository<UserPayment>().Insert(userPayment);
                db.Save();


                return ServiceResponse.SuccessReponse(userlist, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<User>(exp);
            }
        }

        [HttpGet]
        public async Task<ResponseObject<object>>UserPaymentHistory(int ID)
        {
            try
            {               
                object UesrPaymentHistory = db.ExtRepositoryFor<MyWasteRepository>().GetPaymentHistory(ID);

                return ServiceResponse.SuccessReponse(UesrPaymentHistory, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }


    }
}
