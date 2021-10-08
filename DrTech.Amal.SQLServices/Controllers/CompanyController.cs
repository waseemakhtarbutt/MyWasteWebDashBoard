using ClosedXML.Excel;
using DrTech.Amal.Common.Enums;
using DrTech.Amal.Common.Helpers;
using DrTech.Amal.Common.ServerResponse;
using DrTech.Amal.Notifications;
using DrTech.Amal.SQLDataAccess.CustomModels;
using DrTech.Amal.SQLDataAccess.Repository;
using DrTech.Amal.SQLModels;
using DrTech.Amal.SQLServices.Auth;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using static DrTech.Amal.Common.Extentions.Constants;

namespace DrTech.Amal.SQLServices.Controllers
{
    public class CompanyController : BaseController
    {
        [HttpPost]
        public async Task<ResponseObject<bool>> AddCompanyInformation()
        {
            try
            {
                Company mdlCompany = new Company();
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
              

                mdlCompany.FileName = FileName;
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["id"]))
                    mdlCompany.ID = Convert.ToInt32( HttpContext.Current.Request.Form["id"].ToString());

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["companyName"]))
                    mdlCompany.CompanyName = HttpContext.Current.Request.Form["companyName"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["address"]))
                    mdlCompany.Address = HttpContext.Current.Request.Form["address"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["phone"]))
                    mdlCompany.Phone = HttpContext.Current.Request.Form["phone"].ToString();

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["email"]))
                    mdlCompany.Email = HttpContext.Current.Request.Form["email"].ToString();


                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["cityid"]))
                    mdlCompany.CityID = Convert.ToInt32(HttpContext.Current.Request.Form["cityid"]);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["contactPerson"]))
                    mdlCompany.ContactPerson = HttpContext.Current.Request.Form["contactPerson"].ToString();
                if(mdlCompany.ID == 0)
                {


                                      
                    mdlCompany.IsActive = true;
                    mdlCompany.CreatedBy = (int)UserID;
                    mdlCompany.CreatedDate = DateTime.Now;
                    mdlCompany.UserID = UserID;
                    db.Repository<Company>().Insert(mdlCompany);
                    db.Save();

                    User user = new User
                    {
                        FullName = mdlCompany.ContactPerson,
                        Phone = mdlCompany.Phone,
                        Email = "admin" + mdlCompany.Email,
                        Password = "admin@1234",
                        UserTypeID = (int)UserTypeEnum.Web,
                        RoleID = (int)UserRoleTypeEnum.CompanyAdmin,
                        CreatedBy = db.Repository<User>().GetAll().Where(x => x.RoleID == 1).Select(x => x.ID).FirstOrDefault(),
                        CompanyID = mdlCompany.ID,
                        Type = "W"
                    };

                db.Repository<User>().Insert(user);
                db.Save();
                    //Generate BusinessKeY
                    string first3Charcteres = new string(mdlCompany.CompanyName.ToLower().Take(3).ToArray());
                    string BusinessKey = first3Charcteres + db.ExtRepositoryFor<CommonRepository>().GenerateRandomNo() + mdlCompany.ID;
                    mdlCompany.BusinessKey = BusinessKey;
                    db.Repository<Company>().Update(mdlCompany);
                    db.Save();
                    //Create BusinessKey and QR Code

                    var list = new List<KeyValuePair<string, string>>();
                    list.Add(new KeyValuePair<string, string>("ID", Convert.ToString(mdlCompany.ID)));
                    list.Add(new KeyValuePair<string, string>("Email", mdlCompany.Email));
                    list.Add(new KeyValuePair<string, string>("Phone", mdlCompany.Phone));
                    list.Add(new KeyValuePair<string, string>("CompanyName", mdlCompany.CompanyName));

                    StringBuilder CodeModel = new StringBuilder();
                    CodeModel.Append(mdlCompany.ID + ";");
                    CodeModel.Append(mdlCompany.Email + ";");
                    CodeModel.Append(mdlCompany.Phone + ";");
                    CodeModel.Append(mdlCompany.BusinessKey + ";");
                    CodeModel.Append(mdlCompany.CompanyName);
                   


                    mdlCompany.QRCode = QRCodeTagHelper.QRCodeGenerator(CodeModel);
                    db.Repository<Company>().Update(mdlCompany);
                    db.Save();




                }
                else
                {
                    //get Company ByID
                    var dbCompanyModel = db.Repository<Company>().FindById(mdlCompany.ID);
                    dbCompanyModel.IsActive = true;
                    dbCompanyModel.UpdatedBy = (int)UserID;
                    dbCompanyModel.UpdatedDate = DateTime.Now;
                    dbCompanyModel.UserID = UserID;
                    dbCompanyModel.Phone = mdlCompany.Phone;
                    dbCompanyModel.Email = mdlCompany.Email;
                    dbCompanyModel.CompanyName = mdlCompany.CompanyName;
                    dbCompanyModel.ContactPerson = mdlCompany.ContactPerson;
                    dbCompanyModel.FileName = mdlCompany.FileName;
                    dbCompanyModel.Address = mdlCompany.Address;
                    dbCompanyModel.CityID = mdlCompany.CityID;
                    // mdlCompany.IsActive = true;
                    // mdlCompany.UpdatedBy = (int)UserID;
                    // mdlCompany.UpdatedDate = DateTime.Now;
                    // mdlCompany.UserID = UserID;
                    // //Map the old data as well
                    // mdlCompany.BusinessKey = dbModel.BusinessKey;
                    // mdlCompany.CreatedDate = dbModel.CreatedDate;
                    // mdlCompany.CreatedBy = dbModel.CreatedBy;
                    //db.Repository<Company>().Update(mdlCompany);
                    // db.Save();

                    //Create BusinessKey and QR Code

                    StringBuilder CodeModel = new StringBuilder();
                    CodeModel.Append(dbCompanyModel.ID + ";");
                    CodeModel.Append(dbCompanyModel.Email + ";");
                    CodeModel.Append(dbCompanyModel.Phone + ";");
                    CodeModel.Append(dbCompanyModel.BusinessKey + ";");
                    CodeModel.Append(dbCompanyModel.CompanyName);

                    //QRCodeViewModel mdlQR = new QRCodeViewModel();
                    //mdlQR.ID = mdlCompany.ID;
                    //mdlQR.Email = mdlCompany.Email;
                    //mdlQR.Phone = mdlCompany.Phone;

                    dbCompanyModel.QRCode = QRCodeTagHelper.QRCodeGenerator(CodeModel);
                    db.Repository<Company>().Update(dbCompanyModel);
                    db.Save();


                    //string first3Charcteres = new string(mdlCompany.CompanyName.ToLower().Take(3).ToArray());
                    //string BusinessKey = first3Charcteres + db.ExtRepositoryFor<CommonRepository>().GenerateRandomNo() + mdlCompany.ID;
                    //mdlCompany.BusinessKey = BusinessKey;
                    //db.Repository<Company>().Update(mdlCompany);
                    //db.Save();

                    //Company company = db.Repository<Company>().FindById(mdlCompany.ID);
                    //company.CompanyName = mdlCompany.CompanyName;
                    //company.Address = mdlCompany.Address;
                    //company.ContactPerson = mdlCompany.ContactPerson;
                    //company.CityID = mdlCompany.CityID;
                    //company.Email = mdlCompany.Email;
                    //company.FileName = mdlCompany.FileName;
                    //company.IsActive = true;
                    //company.UpdatedBy = (int)UserID;
                    //company.UpdatedDate = DateTime.Now;
                    //company.UserID = UserID;
                    //db.Repository<Company>().Update(company);
                    //db.Save();

                    ////Create BusinessKey and QR Code

                    //StringBuilder CodeModel = new StringBuilder();
                    //CodeModel.Append(company.ID + ";");
                    //CodeModel.Append(company.Email + ";");
                    //CodeModel.Append(company.Phone + ";");
                    //CodeModel.Append(company.CompanyName);


                    //mdlCompany.QRCode = QRCodeTagHelper.QRCodeGenerator(CodeModel);



                    //string first3Charcteres = new string(company.CompanyName.ToLower().Take(3).ToArray());
                    //string BusinessKey = first3Charcteres + db.ExtRepositoryFor<CommonRepository>().GenerateRandomNo() + company.ID;
                    //mdlCompany.BusinessKey = BusinessKey;
                    //db.Repository<Company>().Update(mdlCompany);
                    //db.Save();

                }



                return ServiceResponse.SuccessReponse(true, MessageEnum.NGOAdded);
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

        [HttpPost]
        public async Task<ResponseObject<bool>> AddGuiForRecycle1(GUIForRecycleViewModel model)
        {
            //In this method the requirments changed as , MCB is now considering as business as initially it was considered as a separate comapany so I am changing this accordingly.

            try
            {
                int? RoleID = 0;
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                var UserRole = db.Repository<User>().FindById(UserID);
                if(UserRole != null)
                {
                    RoleID = UserRole.RoleID;
                }
                if(RoleID == (int)UserRoleTypeEnum.Admin)
                {
                    //Find userID by company ID comming in the viewModel from SuperAdmin and then save Recycle against that UserID in the recycle tables.
                    int CompanyUserID = 0;
                    var CompanyUser = db.Repository<User>().GetAll().Where(x => x.CompanyID == model.companyID).FirstOrDefault();
                    if(CompanyUser != null)
                    {
                        CompanyUserID = CompanyUser.ID;
                    }

                    Recycle mdlRecycle = new Recycle();

                    mdlRecycle.CollectorDateTime = model.collectDate;
                    mdlRecycle.FileName = "";
                    mdlRecycle.IsActive = true;
                    mdlRecycle.CreatedBy = (int)UserID;
                    mdlRecycle.CreatedDate = DateTime.Now;
                    mdlRecycle.UserID = (int)CompanyUserID;
                    db.Repository<Recycle>().Insert(mdlRecycle);
                    db.Save();
                    RecycleSubItem mdlRecycleSubItem = new RecycleSubItem();
                    mdlRecycleSubItem.RecycleID = mdlRecycle.ID;
                    mdlRecycleSubItem.Weight = model.Weight;
                    mdlRecycleSubItem.IsActive = true;
                    mdlRecycleSubItem.CreatedDate = DateTime.Now;
                    mdlRecycleSubItem.GreenPoints = 0;
                    mdlRecycleSubItem.CreatedBy = (int)UserID;
                    db.Repository<RecycleSubItem>().Insert(mdlRecycleSubItem);
                    db.Save();

                    return ServiceResponse.SuccessReponse(true, MessageEnum.RecycleItemsAdded);

                }
                else
                {
                    Recycle mdlRecycle = new Recycle();

                    mdlRecycle.CollectorDateTime = model.collectDate;
                    mdlRecycle.FileName = "";
                    mdlRecycle.IsActive = true;
                    mdlRecycle.CreatedBy = (int)UserID;
                    mdlRecycle.CreatedDate = DateTime.Now;
                    mdlRecycle.UserID = (int)UserID;
                    db.Repository<Recycle>().Insert(mdlRecycle);
                    db.Save();
                    RecycleSubItem mdlRecycleSubItem = new RecycleSubItem();
                    mdlRecycleSubItem.RecycleID = mdlRecycle.ID;
                    mdlRecycleSubItem.Weight = model.Weight;
                    mdlRecycleSubItem.IsActive = true;
                    mdlRecycleSubItem.CreatedDate = DateTime.Now;
                    mdlRecycleSubItem.GreenPoints = 0;
                    mdlRecycleSubItem.CreatedBy = (int)UserID;
                    db.Repository<RecycleSubItem>().Insert(mdlRecycleSubItem);
                    db.Save();


                    return ServiceResponse.SuccessReponse(true, MessageEnum.RecycleItemsAdded);

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


        [HttpPost]
        public async Task<ResponseObject<bool>> AddGuiForRecycle(GUIForRecycleViewModel model)
        {
            //Here I am adding 1 in the day part of the date because during the request the completion the day goes back 1 day. 
            model.collectDate =  model.collectDate.AddDays(1);
            //In this method the requirments changed as , MCB is now considering as business as initially it was considered as a separate comapany so I am changing this accordingly.
           // DateTime d = model.collectDate.AddHours(5);
            try
            {
                int? RoleID = 0;
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                var UserRole = db.Repository<User>().FindById(UserID);
                if (UserRole != null)
                {
                    RoleID = UserRole.RoleID;
                }
                if (RoleID == (int)UserRoleTypeEnum.BusinessAdmin)
                {
                    //Find userID by company ID comming in the viewModel from SuperAdmin and then save Recycle against that UserID in the recycle tables.
                    int CompanyUserID = 0;
                   var UserBusiness = db.Repository<Business>().GetAll().Where(x => x.ID == model.companyID).FirstOrDefault();
                    int? GOIOrBusinessUserID = UserBusiness.UserID;
                    var CompanyUser = db.Repository<User>().GetAll().Where(x => x.ID == GOIOrBusinessUserID).FirstOrDefault();
                    if (CompanyUser != null)
                    {
                        CompanyUserID = CompanyUser.ID;
                    }

                    Recycle mdlRecycle = new Recycle();

                    mdlRecycle.CollectorDateTime = model.collectDate;
                    mdlRecycle.FileName = "";
                    mdlRecycle.IsActive = true;
                    mdlRecycle.CreatedBy = (int)UserID;
                    mdlRecycle.CreatedDate = DateTime.Now.ToUniversalTime();
                    mdlRecycle.UserID = (int)CompanyUserID;
                    db.Repository<Recycle>().Insert(mdlRecycle);
                    db.Save();
                    RecycleSubItem mdlRecycleSubItem = new RecycleSubItem();
                    mdlRecycleSubItem.RecycleID = mdlRecycle.ID;
                    mdlRecycleSubItem.Weight = model.Weight;
                    mdlRecycleSubItem.WasteTypeID = model.typeID;
                    mdlRecycleSubItem.IsActive = true;
                    mdlRecycleSubItem.CreatedDate = DateTime.Now;
                    mdlRecycleSubItem.GreenPoints = 0;
                    mdlRecycleSubItem.CreatedBy = (int)UserID;
                    db.Repository<RecycleSubItem>().Insert(mdlRecycleSubItem);
                    db.Save();

                    return ServiceResponse.SuccessReponse(true, MessageEnum.RecycleItemsAdded);

                }
                else if (RoleID == (int)UserRoleTypeEnum.Admin)
                {
                    int CompanyUserID = 0;
                    var UserBusiness = db.Repository<Business>().GetAll().Where(x => x.ID == model.companyID).FirstOrDefault();
                    int? GOIOrBusinessUserID = UserBusiness.UserID;
                    var CompanyUser = db.Repository<User>().GetAll().Where(x => x.ID == GOIOrBusinessUserID).FirstOrDefault();
                    if (CompanyUser != null)
                    {
                        CompanyUserID = CompanyUser.ID;
                    }

                    Recycle mdlRecycle = new Recycle();

                    mdlRecycle.CollectorDateTime = model.collectDate;
                    mdlRecycle.FileName = "";
                    mdlRecycle.IsActive = true;
                    mdlRecycle.CreatedBy = (int)UserID;
                    mdlRecycle.CreatedDate = DateTime.Now.ToUniversalTime();
                    mdlRecycle.UserID = (int)CompanyUserID;
                    db.Repository<Recycle>().Insert(mdlRecycle);
                    db.Save();
                    RecycleSubItem mdlRecycleSubItem = new RecycleSubItem();
                    mdlRecycleSubItem.RecycleID = mdlRecycle.ID;
                    mdlRecycleSubItem.Weight = model.Weight;
                    mdlRecycleSubItem.WasteTypeID = model.typeID;
                    mdlRecycleSubItem.IsActive = true;
                    mdlRecycleSubItem.CreatedDate = DateTime.Now;
                    mdlRecycleSubItem.GreenPoints = 0;
                    mdlRecycleSubItem.CreatedBy = (int)UserID;
                    db.Repository<RecycleSubItem>().Insert(mdlRecycleSubItem);
                    db.Save();

                    return ServiceResponse.SuccessReponse(true, MessageEnum.RecycleItemsAdded);

                }
                else
                {
                    Recycle mdlRecycle = new Recycle();

                    mdlRecycle.CollectorDateTime = model.collectDate;
                    mdlRecycle.FileName = "";
                    mdlRecycle.IsActive = true;
                    mdlRecycle.CreatedBy = (int)UserID;
                    mdlRecycle.CreatedDate = DateTime.Now.ToUniversalTime();
                    mdlRecycle.UserID = (int)UserID;
                    db.Repository<Recycle>().Insert(mdlRecycle);
                    db.Save();
                    RecycleSubItem mdlRecycleSubItem = new RecycleSubItem();
                    mdlRecycleSubItem.RecycleID = mdlRecycle.ID;
                    mdlRecycleSubItem.Weight = model.Weight;
                    mdlRecycleSubItem.WasteTypeID = model.typeID;
                    mdlRecycleSubItem.IsActive = true;
                    mdlRecycleSubItem.CreatedDate = DateTime.Now;
                    mdlRecycleSubItem.GreenPoints = 0;
                    mdlRecycleSubItem.CreatedBy = (int)UserID;
                    db.Repository<RecycleSubItem>().Insert(mdlRecycleSubItem);
                    db.Save();


                    return ServiceResponse.SuccessReponse(true, MessageEnum.RecycleItemsAdded);

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
        [AllowAnonymous]
        [HttpPost]
        public async Task<ResponseObject<bool>> DumpRecycle(DumpRecycleItemViewModel model)
        {
            model.collectDate = model.collectDate.AddDays(1);
            try
            {
                int? RoleID = 0;
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                var UserRole = db.Repository<User>().FindById(UserID);
                if (UserRole != null)
                {
                    RoleID = UserRole.RoleID;
                }
                if (RoleID == (int)UserRoleTypeEnum.Admin)
                {
                    int CompanyUserID = 0;
                    var UserBusiness = db.Repository<Business>().GetAll().Where(x => x.ID == model.companyID).FirstOrDefault();
                    int? GOIOrBusinessUserID = UserBusiness.UserID;
                    var CompanyUser = db.Repository<User>().GetAll().Where(x => x.ID == GOIOrBusinessUserID).FirstOrDefault();
                    if (CompanyUser != null)
                    {
                        CompanyUserID = CompanyUser.ID;
                    }
                    // Insert into parent table
                    Recycle mdlRecycle = new Recycle();
                    mdlRecycle.CollectorDateTime = model.collectDate;
                    mdlRecycle.FileName = "";
                    mdlRecycle.IsActive = true;
                    mdlRecycle.CreatedBy = (int)UserID;
                    mdlRecycle.CreatedDate = DateTime.Now.ToUniversalTime();
                    mdlRecycle.UserID = (int)CompanyUserID;
                    db.Repository<Recycle>().Insert(mdlRecycle);
                    db.Save();
                    // Insert into child table
                    RecycleSubItem mdlRecycleSubItem = new RecycleSubItem();
                    mdlRecycleSubItem.RecycleID = mdlRecycle.ID;
                    //  mdlRecycleSubItem.Weight = model.Weight;
                    //  mdlRecycleSubItem.WasteTypeID = model.typeID;
                    decimal? total = model.lists.Sum(item => item.Weight);
                    mdlRecycleSubItem.Weight = total;
                    mdlRecycleSubItem.IsActive = true;
                    mdlRecycleSubItem.CreatedDate = DateTime.Now;
                    mdlRecycleSubItem.GreenPoints = 0;
                    mdlRecycleSubItem.CreatedBy = (int)UserID;
                    db.Repository<RecycleSubItem>().Insert(mdlRecycleSubItem);
                    db.Save();
                    // Insert into sub child table
                    foreach (var item in model.lists)
                    {
                        RecycleSubItemsType recycleSubItemsType = new RecycleSubItemsType();
                        recycleSubItemsType.RecycleSubItemID = mdlRecycleSubItem.ID;
                        recycleSubItemsType.WasteTypeID = item.typeID;
                        recycleSubItemsType.Weight = item.Weight;
                        recycleSubItemsType.Rate = item.rate;
                        recycleSubItemsType.Total = item.Weight* item.rate;
                        db.Repository<RecycleSubItemsType>().Insert(recycleSubItemsType);
                        db.Save();
                    }
                   
                    return ServiceResponse.SuccessReponse(true, MessageEnum.RecycleItemsAdded);

                }

            }
            catch(Exception ex)
            {

            }
           

            return ServiceResponse.SuccessReponse(false, MessageEnum.DefaultSuccessMessage);
        }

        [HttpPost]
        public async Task<ResponseObject<bool>> DumpSegregatedRecycle(DumpRecycleItemViewModel model)
        {
          // model.collectDate = model.collectDate.AddDays(1);
            try
            {
                var TWeights = model.lists.Sum(x => x.Weight);
                TWeights = Convert.ToDecimal(TWeights);
                if (TWeights != Convert.ToDecimal(model.TWeight))
                {
                    return ServiceResponse.SuccessReponse(false, "Weight must be  Equal to Total weight");

                }


                int? RoleID = 0;
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                var UserRole = db.Repository<User>().FindById(UserID);
                if (UserRole != null)
                {
                    RoleID = UserRole.RoleID;
                }
                if (RoleID == (int)UserRoleTypeEnum.Admin)
                {
                    int CompanyUserID = 0;
                    var UserBusiness = db.Repository<Business>().GetAll().Where(x => x.ID == model.companyID).FirstOrDefault();
                    int? GOIOrBusinessUserID = UserBusiness.UserID;
                    var CompanyUser = db.Repository<User>().GetAll().Where(x => x.ID == GOIOrBusinessUserID).FirstOrDefault();
                    if (CompanyUser != null)
                    {
                        CompanyUserID = CompanyUser.ID;
                    }

                    // Insert into parent table
                    var mRecycle = db.Repository<Recycle>().FindById(model.RecycleID);
                    if(mRecycle != null)
                    {
                        mRecycle.IsActive = true;
                        mRecycle.CollectorDateTime = model.collectDate;
                        db.Repository<Recycle>().Update(mRecycle);
                        db.Save();
                    }
                    else
                    {
                        return ServiceResponse.SuccessReponse(false, MessageEnum.DefaultErrorMessage);
                    }

                    // Insert into child table
                    var mRecycleSubItem = db.Repository<RecycleSubItem>().GetAll().Where(x => x.RecycleID == mRecycle.ID).FirstOrDefault();

                    // Insert into sub child table
                    foreach (var item in model.lists)
                    {
                        RecycleSubItemsType recycleSubItemsType = new RecycleSubItemsType();
                        recycleSubItemsType.RecycleSubItemID = mRecycleSubItem.ID;
                        recycleSubItemsType.WasteTypeID = item.typeID;
                        recycleSubItemsType.Weight = item.Weight;
                        recycleSubItemsType.Rate = item.rate;
                        recycleSubItemsType.Total = item.Weight * item.rate;
                        db.Repository<RecycleSubItemsType>().Insert(recycleSubItemsType);
                        db.Save();
                    }

                    return ServiceResponse.SuccessReponse(true, MessageEnum.RecycleItemsAdded);

                }

            }
            catch (Exception ex)
            {

            }


            return ServiceResponse.SuccessReponse(false, MessageEnum.DefaultSuccessMessage);
        }


        /// <summary>
        /// SaveRecycle used to save data from mobile app. driver will use it.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ResponseObject<bool>> SaveRecycle(RecycleSaveDataView model)
        { 

          //  model.CollectionDate = model.CollectionDate.AddDays(1);
            try
            {
                //int? RoleID = 0;
              //  int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                //var UserRole = db.Repository<User>().FindById(UserID);
                //if (UserRole != null)
                //{
                //    RoleID = UserRole.RoleID;
                //}
                //if (RoleID == (int)UserRoleTypeEnum.Admin)
                //{

                int CompanyUserID = 0;
                var UserCompany = db.Repository<RegBusiness>().GetAll().Where(x => x.ID == model.CompanyID).FirstOrDefault();
                var CompanyallBraches = db.Repository<Business>().GetAll().Where(x => x.ParentId == UserCompany.ID).ToList();


                var UserBusiness = CompanyallBraches.Where(x=>x.ID == model.BranchID).FirstOrDefault();

                    int? GOIOrBusinessUserID = UserBusiness.UserID;
                    var CompanyUser = db.Repository<User>().GetAll().Where(x => x.ID == GOIOrBusinessUserID).FirstOrDefault();
                    if (CompanyUser != null)
                    {
                        CompanyUserID = CompanyUser.ID;
                    }
                else
                {
                    return ServiceResponse.SuccessReponse(false, MessageEnum.DefaultErrorMessage);
                }
                    // Insert into parent table
                    Recycle mdlRecycle = new Recycle();
                    mdlRecycle.CollectorDateTime = model.CollectionDate;
                    mdlRecycle.FileName = "";
                    mdlRecycle.IsActive = false;
                    mdlRecycle.CreatedBy = 1; // (int)UserID;
                    mdlRecycle.CreatedDate = DateTime.Now.ToUniversalTime();
                    mdlRecycle.UserID = (int)CompanyUserID;
                    db.Repository<Recycle>().Insert(mdlRecycle);
                    db.Save();
                    // Insert into child table
                    RecycleSubItem mdlRecycleSubItem = new RecycleSubItem();
                    mdlRecycleSubItem.RecycleID = mdlRecycle.ID;
                    decimal? total = model.Weight;
                    mdlRecycleSubItem.Weight = total;
                    mdlRecycleSubItem.IsActive = false;
                    mdlRecycleSubItem.CreatedDate = DateTime.Now;
                    mdlRecycleSubItem.GreenPoints = 0;
                    mdlRecycleSubItem.CreatedBy = 1; // (int)UserID;
                    db.Repository<RecycleSubItem>().Insert(mdlRecycleSubItem);
                    db.Save();
                // Insert into sub child table
                //Here to send message to location admin for notification.
                SMSNotifyEvent _events = new SMSNotifyEvent();
                _events.Parameters.Add("Company", UserCompany.Name);
                _events.Parameters.Add("Location", UserBusiness.OfficeName);
                DateTime dat = db.ExtRepositoryFor<CompanyRepository>().GetLocalDateTimeFromUTC(DateTime.Now.ToUniversalTime());

                _events.Parameters.Add("Date", dat.ToString("MM/dd/yy hh:mm tt"));
                _events.Parameters.Add("Weight", model.Weight);
                _events.AddSMSNotifyEvent((long)NotificationEventConstants.GOI.SMSSendToLocationAdmin, CompanyUserID.ToString());

                return ServiceResponse.SuccessReponse(true, MessageEnum.DefaultSubmittedSuccessMessage);

                //}

            }
            catch (Exception ex)
            {

            }


            return ServiceResponse.SuccessReponse(false, MessageEnum.DefaultSuccessMessage);
        }

        [HttpGet]
        public async Task<ResponseObject<List<object>>> GetAll()
        {
            try
            {
                List<object> lstCompanies = db.ExtRepositoryFor<CompanyRepository>().GetCompanyList();

                return ServiceResponse.SuccessReponse(lstCompanies, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }
        
         [HttpGet]
        public async Task<ResponseObject<List<object>>> GetWasteTypes()
        {
            try
            {
                List<object> lstWastTypes = db.Repository<WasteType>().GetAll().ToList<object>();

                return ServiceResponse.SuccessReponse(lstWastTypes, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }

        [HttpGet]
        public async Task<ResponseObject<List<object>>> GetAllByAdminRole()
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                List<object> lstCompanies = db.ExtRepositoryFor<CompanyRepository>().GetBusinessListByAdminRole(UserID);

                return ServiceResponse.SuccessReponse(lstCompanies, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }

        [HttpGet]
        public async Task<ResponseObject<List<object>>> GetGUIList()
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                List<object> lstGUIs = db.ExtRepositoryFor<CompanyRepository>().GUIList(UserID);

                return ServiceResponse.SuccessReponse(lstGUIs, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }

        [HttpGet]
        public async Task<ResponseObject<List<object>>> GetGOIListForSuperAdminByBranchID(int BranchID)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                List<object> lstGUIs = db.ExtRepositoryFor<CompanyRepository>().GOIListForSuperAdmin(BranchID);

                return ServiceResponse.SuccessReponse(lstGUIs, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }
        [HttpPost]
        public async Task<ResponseObject<List<object>>> GetGOIListForSuperAdminByBranchDate(RecycleRequest model)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                List<object> lstGUIs = db.ExtRepositoryFor<CompanyRepository>().GOIListForSuperAdmin(model);

                return ServiceResponse.SuccessReponse(lstGUIs, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<object>>(exp);
            }
        }
        [HttpGet]
        public  HttpResponseMessage DownloadExcelDocument(int BranchID)
        {
            try
            {
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                var lstGUIs = db.ExtRepositoryFor<CompanyRepository>().GOIListForSuperAdmin1s(BranchID);

                var sb = new StringBuilder();
                MemoryStream stream = new MemoryStream();
                StreamWriter writer = new StreamWriter(stream);
                foreach (var tempResult in lstGUIs)
                {
                    sb.Append(tempResult.ID + ",");
                    sb.Append(tempResult.Date + ",");
                    sb.Append(tempResult.GreenPoints + ",");
                    sb.Append(tempResult.LocationName + ",");
                    sb.Append(tempResult.Weight + ",");
                }
                writer.Write(sb.ToString());
                writer.Flush();
                stream.Position = 0;
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StreamContent(stream);
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/csv");
                result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = "Export.csv" };
                return result;
                //using (var workbook = new XLWorkbook())
                //{
                //    var worksheet = workbook.Worksheets.Add("RecyclesDataExcel");
                //    var currentRow = 1;
                //    worksheet.Cell(currentRow, 1).Value = "Id";
                //    worksheet.Cell(currentRow, 2).Value = "LocationName";
                //    foreach (var recyclesDataExcel in lstGUIs)
                //    {
                //        currentRow++;

                //        worksheet.Cell(currentRow, 1).Value = recyclesDataExcel.ID;
                //        worksheet.Cell(currentRow, 2).Value = recyclesDataExcel.LocationName;
                //    }

                //    using (var stream = new MemoryStream())
                //    {
                //        workbook.SaveAs(stream);
                //        var content = stream.ToArray();

                //        return File( content,"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet","users.xlsx");
                //    }

                // return ServiceResponse.SuccessReponse(lstGUIs, MessageEnum.DefaultSuccessMessage);
        }
            catch (Exception exp)
            {
                return null;
            }
        }


        
        [HttpGet]
        public async Task<ResponseObject<object>> GetCompanyByID(int ID)
        {
            try
            {
                object company = db.Repository<Company>().FindById(ID);

                if (company != null)
                    return ServiceResponse.SuccessReponse(company, MessageEnum.RecordNotFound);
                else
                    return ServiceResponse.SuccessReponse(company, MessageEnum.RecordFoundSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<object>(exp);
            }
        }
        [HttpGet]
        public async Task<ResponseObject<bool>> SuspendCompany(int ID)
        {
            try
            {
                Company company = db.Repository<Company>().FindById(ID);
               
                if (company != null)
                {
                    company.IsActive = false;
                    db.Repository<Company>().Update(company);
                    db.Save();
                    return ServiceResponse.SuccessReponse(true, MessageEnum.RecordNotFound);
                }
                else
                {
                    return ServiceResponse.SuccessReponse(false, MessageEnum.RecordFoundSuccessfully);
                }
                   
               
                    
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        public class RecyclesDataExcel
        {
            public int ID { get; set; }
            public string Time { get; set; }
            public decimal? NotWaste { get; set; }
            public DateTime? Date { get; set; }
            public dynamic LocationName { get; set; }
            public decimal? GreenPoints { get; set; }
            public decimal? Weight { get; set; }

        }


    }
}
