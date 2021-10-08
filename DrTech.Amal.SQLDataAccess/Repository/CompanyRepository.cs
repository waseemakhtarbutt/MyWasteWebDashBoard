using DrTech.Amal.Common.Enums;
using DrTech.Amal.Common.Helpers;
using DrTech.Amal.SQLDataAccess.CustomModels;
using DrTech.Amal.SQLDatabase;
using DrTech.Amal.SQLModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess.Repository
{
   

    public class CompanyRepository : Repository<Company>
    {
        public CompanyRepository(Amal_Entities context)
        : base(context)
        {
            dbSet = context.Set<Company>();
        }

        public List<object> GetCompanyList()
        {
            List<object> mdlCompanies = (from com in context.Companies.ToList()
                                        
                                        join city in context.Cities on com.CityID equals city.ID
                                       // join type in context.LookupTypes on com.CityID equals type.ID
                                        where com.IsActive == true
                                        select new
                                        {
                                            com.ID,
                                            com.FileName,
                                            com.CompanyName,
                                            City = city.CityName,
                                            com.Phone,
                                            com.Email,
                                            com.Address,
                                            com.ContactPerson,
                                            CreatedDate = Convert.ToDateTime(com.CreatedDate).ToString("MMM dd, yyyy"),
                                        }).OrderByDescending(o => o.ID).ToList<object>();

            return mdlCompanies;
        }

        public List<object> GetCompanyListByAdminRole(int? UserID)
        {
            var UserRole = context.Users.Where(x => x.ID == UserID).FirstOrDefault();

            int? RoleID = UserRole.RoleID;
            if(RoleID == (int)UserRoleTypeEnum.Admin)
            {
                List<object> mdlCompanies = (from com in context.Companies.ToList()


                                            // join type in context.LookupTypes on com.CityID equals type.ID
                                             where com.IsActive == true
                                             select new
                                             {
                                                 com.ID,                                               
                                                 com.CompanyName,                                                                                                                                             
                                             }).OrderByDescending(o => o.CompanyName).ToList<object>();

                return mdlCompanies;
            }
            else
            {
                List<object> mdlCompanies = (from com in context.Companies.ToList()


                                                 // join type in context.LookupTypes on com.CityID equals type.ID
                                             where com.IsActive == true && com.UserID == UserID
                                             select new
                                             {
                                                 com.ID,
                                                 com.CompanyName,
                                             }).OrderByDescending(o => o.CompanyName).ToList<object>();

                return mdlCompanies;
            }

           
        }

        public List<object> GetBusinessListByAdminRole(int? UserID)
        {
            var UserRole = context.Users.Where(x => x.ID == UserID).FirstOrDefault();

            int? RoleID = UserRole.RoleID;
            if (RoleID == (int)UserRoleTypeEnum.Admin)
            {
                //When Amal Super  admin is login -- for example DrTech
                List<object> mdlBusinesses = (from com in context.RegBusinesses.ToList()


                                                 // join type in context.LookupTypes on com.CityID equals type.ID
                                             where com.IsActive == true
                                             select new
                                             {
                                                 com.ID,
                                                 Name = com.Name,
                                             }).OrderByDescending(o => o.Name).ToList<object>();

                return mdlBusinesses;
            }
            else if (RoleID == (int)UserRoleTypeEnum.BusinessAdmin)
            {

                RegBusiness mdl = context.RegBusinesses.Where(x => x.UserID == UserID).FirstOrDefault();

                //When Company admin is login -- for example MCB
                List<object> mdlBusinesses = (from com in context.RegBusinesses


                                                   join branch in context.Businesses on com.ID  equals branch.ParentId
                                              where com.IsActive == true && branch.ParentId == mdl.ID
                                              select new
                                              {
                                                  branch.ID,
                                                  Name = branch.OfficeName,
                                              }).OrderByDescending(o => o.Name).ToList<object>();

                return mdlBusinesses;
            }

            else
            {
                //When GOI admin is login -- for example MCB GOI1
                List<object> mdlBusinesses = (from com in context.Businesses.ToList()


                                              //join branch in context.Businesses on com.ID equals branch.ParentId
                                              where com.IsActive == true && com.UserID == UserID
                                              select new
                                              {
                                                  com.ID,
                                                  Name = com.OfficeName,
                                              }).OrderByDescending(o => o.Name).ToList<object>();

                return mdlBusinesses;
            }


        }

        //public List<object> GUIList(int? UserID)
        public List<object> GUIList1(int? UserID)
        {
            int CompanyId = 0;
            List<object> mdlGUIList;

            var UserFromDB = context.Users.Where(x => x.ID == UserID).FirstOrDefault();
            var UserRoleFromDB = context.Roles.Where(x => x.ID == UserFromDB.RoleID).FirstOrDefault();
            var  UserCompany = context.Companies.Where(x => x.ID == UserFromDB.CompanyID).FirstOrDefault();
            if(UserCompany == null && UserRoleFromDB.ID == (int)UserRoleTypeEnum.Admin)
            {
                mdlGUIList = (from rec in context.Recycles.ToList()

                              join subItem in context.RecycleSubItems on rec.ID equals subItem.RecycleID
                              where rec.IsActive == true && rec.CollectorDateTime.Value.Date.Year == DateTime.Now.Year
                              select new
                              {
                                  rec.ID,
                                  subItem.GreenPoints,
                                  Time = Convert.ToDateTime(rec.CreatedDate).ToString("hh:mm tt"),
                                  subItem.Weight,
                                  Date = Convert.ToDateTime(rec.CollectorDateTime).ToString("MMM dd, yyyy"),
                              }).OrderByDescending(o => o.Date).ToList<object>();

                return mdlGUIList;
                
            }
            else if(UserCompany != null)
            {
                CompanyId = UserCompany.ID;

                mdlGUIList = (from rec in context.Recycles.ToList()

                              join subItem in context.RecycleSubItems on rec.ID equals subItem.RecycleID
                              where rec.IsActive == true && rec.UserID == UserID && rec.CollectorDateTime.Value.Date.Year == DateTime.Now.Year
                              select new
                              {
                                  rec.ID,
                                  subItem.GreenPoints,
                                  Time = Convert.ToDateTime(rec.CreatedDate).ToString("hh:mm tt"),
                                  subItem.Weight,
                                  Date = Convert.ToDateTime(rec.CollectorDateTime).ToString("MMM dd, yyyy"),
                              }).OrderByDescending(o => o.Date).ToList<object>();


                return mdlGUIList;
            }
            else
            {
                return new List<object>();
            }
        }
        /// <summary>
        /// return only waste weight not the (Not in waste type values)
        /// </summary>
        /// <param name="rSubItemID"></param>
        /// <returns></returns>
        public decimal? getTotalWasteFromWasteTypes(int rSubItemID)
        {
     
            int NotWasteTypeID = (int)ConstantValues.NotWasteTypeID;
            int Greenwaste = (int)ConstantValues.Greenwaste;
            int TeaBags = (int)ConstantValues.TeaBags;
            int Tissue = (int)ConstantValues.Tissue;

            var allRecords = context.RecycleSubItemsTypes.Where(x => x.RecycleSubItemID == rSubItemID && x.WasteTypeID != NotWasteTypeID && x.WasteTypeID != Greenwaste && x.WasteTypeID != TeaBags && x.WasteTypeID != Tissue).ToList();
                if(allRecords != null && allRecords.Count > 0)
            {
                return allRecords.Sum(item => item.Weight);
            }
            else
            {
                //Return 1 because 1 multi anything become that number it okay.
                return 1;
            }
        }

        public decimal? getNotWasteFromWasteTypes(int rSubItemID)
        {

            int NotWasteTypeID = (int)ConstantValues.NotWasteTypeID;
            int TeaBags = (int)ConstantValues.TeaBags;
            int Greenwaste = (int)ConstantValues.Greenwaste;
            int Tissue = (int)ConstantValues.Tissue;

            List<int?> myObjects = new List<int?>();
            myObjects.AddRange((new[] { NotWasteTypeID, TeaBags, Greenwaste, Tissue }).Cast<int?>());

          
            var allRecords = context.RecycleSubItemsTypes.Where(x => x.RecycleSubItemID == rSubItemID && myObjects.Contains(x.WasteTypeID)).ToList();
            if (allRecords != null && allRecords.Count > 0)
            {
                return allRecords.Sum(item => item.Weight);
            }
            else
            {
                //Return 1 because 1 multi anything become that number it okay.
                return 0;
            }
        }
        public decimal? getWasteWeightFromRSTypes(int rSubItemID)
        {

            int NotWasteTypeID = (int)ConstantValues.NotWasteTypeID;
            int TeaBags = (int)ConstantValues.TeaBags;
            int Greenwaste = (int)ConstantValues.Greenwaste;
            int Tissue = (int)ConstantValues.Tissue;

            var allRecords = context.RecycleSubItemsTypes.Where(x => x.RecycleSubItemID == rSubItemID && x.WasteTypeID != NotWasteTypeID && x.WasteTypeID != TeaBags && x.WasteTypeID != Greenwaste && x.WasteTypeID != Tissue).ToList();
            if (allRecords != null && allRecords.Count > 0)
            {
                return allRecords.Sum(item => item.Weight);
            }
            else
            {
                //Return 1 because 1 multi anything become that number it okay.
                return 0;
            }
        }

        public List<object> GUIList(int? UserID)
        {
           
            int CompanyId = 0;
            List<object> mdlGUIList;

            var UserFromDB = context.Users.Where(x => x.ID == UserID).FirstOrDefault();
            var UserRoleFromDB = context.Roles.Where(x => x.ID == UserFromDB.RoleID).FirstOrDefault();

            RegBusiness regbus = context.RegBusinesses.Where(x => x.UserID == UserID).FirstOrDefault();


            if (UserRoleFromDB.ID == (int)UserRoleTypeEnum.BusinessAdmin)
            {
                List<int?> mdlBus = context.Businesses.Where(x => x.ParentId == regbus.ID).Select(x => x.UserID).ToList();

                mdlGUIList = (from rec in context.Recycles.ToList()
                              join subItem in context.RecycleSubItems on rec.ID equals subItem.RecycleID
                              where rec.IsActive == true  //&&  rec.CollectorDateTime.Value.Date.Year == DateTime.Now.Year
                               && mdlBus.Contains(rec.UserID)
                              select new
                              {
                                  rec.ID,
                                  GreenPoints = (int)ConstantValues.WasteDefaultGCValuePerKG * getTotalWasteFromWasteTypes(subItem.ID),
                                  Time = GetLocalDateTimeFromUTC(Convert.ToDateTime(rec.CreatedDate)).ToString("hh:mm tt"),
                                  Weight = getWasteWeightFromRSTypes(subItem.ID),
                                  NotWaste = getNotWasteFromWasteTypes(subItem.ID),
                                  Date = rec.CollectorDateTime,                       //Convert.ToDateTime(rec.CollectorDateTime).ToString("MMM dd, yyyy"),
                                  LocationName = GetLocationNameByUserID(rec.UserID)
                              }).OrderByDescending(o => o.Date).ToList<object>();

                return mdlGUIList;

            }
            else
            {


                mdlGUIList = (from rec in context.Recycles.ToList()
                              join subItem in context.RecycleSubItems on rec.ID equals subItem.RecycleID
                              where rec.IsActive == true && rec.UserID == UserID   //&& rec.CollectorDateTime.Value.Date.Year == DateTime.Now.Year
                              select new
                              {
                                  rec.ID,
                                  GreenPoints = (int)ConstantValues.WasteDefaultGCValuePerKG * getTotalWasteFromWasteTypes(subItem.ID),
                                  Time = GetLocalDateTimeFromUTC(Convert.ToDateTime(rec.CreatedDate)).ToString("hh:mm tt"),
                                  Weight = getWasteWeightFromRSTypes(subItem.ID),
                                  NotWaste = getNotWasteFromWasteTypes(subItem.ID),
                                  Date = rec.CollectorDateTime                        // Convert.ToDateTime(rec.CollectorDateTime).ToString("MMM dd, yyyy"),
                              }).OrderByDescending(o => o.Date).ToList<object>();


                return mdlGUIList;
            }

        }

        public List<dynamic> GOIListForSuperAdmin(int BranchID)
        {
            int CompanyId = 0;
            List<dynamic> mdlGUIList;
            var branch = context.Businesses.Where(x => x.ID == BranchID).FirstOrDefault();
            var UserFromDB = context.Users.Where(x => x.ID == branch.UserID).FirstOrDefault();

            mdlGUIList = (from rec in context.Recycles.ToList()
                          join subItem in context.RecycleSubItems on rec.ID equals subItem.RecycleID
                          where rec.IsActive == true && rec.UserID == UserFromDB.ID    //&&  rec.CollectorDateTime.Value.Date.Year == DateTime.Now.Year
                          select new
                          {
                              rec.ID,
                              GreenPoints = (int)ConstantValues.WasteDefaultGCValuePerKG * getTotalWasteFromWasteTypes(subItem.ID),
                              Time = GetLocalDateTimeFromUTC(Convert.ToDateTime(rec.CreatedDate)).ToString("hh:mm tt"),
                              Weight = getWasteWeightFromRSTypes(subItem.ID),
                              NotWaste = getNotWasteFromWasteTypes(subItem.ID),
                              Date = rec.CollectorDateTime,                          //Convert.ToDateTime(rec.CollectorDateTime).ToString("MMM dd, yyyy"),
                              LocationName = GetLocationNameByUserID(rec.UserID)
                          }).OrderByDescending(o => o.Date).ToList<object>();


            return mdlGUIList;


        }
        public List<object> GOIListForSuperAdmin(RecycleRequest model)
        {
            List<object> response = new List<object>();

            try
            {
                int CompanyId = 0;
                if (model.StartDate != null && model.EndDate != null)
                {
                    if (model.BranchID > 0)
                    {
                        var branch = context.Businesses.Where(x => x.ID == model.BranchID).FirstOrDefault();
                        var UserFromDB = context.Users.Where(x => x.ID == branch.UserID).FirstOrDefault();

                        var mdlGUIList = (from rec in context.Recycles.ToList()
                                          join subItem in context.RecycleSubItems on rec.ID equals subItem.RecycleID
                                          where rec.IsActive == true && rec.UserID == UserFromDB.ID
                                          select new
                                          {
                                              rec.ID,
                                              GreenPoints = (int)ConstantValues.WasteDefaultGCValuePerKG * getTotalWasteFromWasteTypes(subItem.ID),
                                              Time = GetLocalDateTimeFromUTC(Convert.ToDateTime(rec.CreatedDate)).ToString("hh:mm tt"),
                                              Weight = getWasteWeightFromRSTypes(subItem.ID),
                                              NotWaste = getNotWasteFromWasteTypes(subItem.ID),
                                              Date = rec.CollectorDateTime,                          //Convert.ToDateTime(rec.CollectorDateTime).ToString("MMM dd, yyyy"),
                                              LocationName = GetLocationNameByUserID(rec.UserID)
                                          }).ToList();

                        if (model.StartDate != null && model.EndDate != null)
                        {

                            response = mdlGUIList.Where(x => x.Date >= Utility.GetDateFromString(model.StartDate) && x.Date <= Utility.GetDateFromString(model.EndDate)).OrderByDescending(o => o.Date).ToList<object>();
                            return response;
                        }
                        else
                        {
                            return response = mdlGUIList.ToList<object>();
                        }
                    }
                    else
                    {

                        var mdlGUIList = (from rec in context.Recycles.ToList()
                                          join subItem in context.RecycleSubItems on rec.ID equals subItem.RecycleID
                                          where rec.IsActive == true //&& rec.UserID == UserFromDB.ID
                                          select new
                                          {
                                              rec.ID,
                                              GreenPoints = (int)ConstantValues.WasteDefaultGCValuePerKG * getTotalWasteFromWasteTypes(subItem.ID),
                                              Time = GetLocalDateTimeFromUTC(Convert.ToDateTime(rec.CreatedDate)).ToString("hh:mm tt"),
                                              Weight = getWasteWeightFromRSTypes(subItem.ID),
                                              NotWaste = getNotWasteFromWasteTypes(subItem.ID),
                                              Date = rec.CollectorDateTime,                          //Convert.ToDateTime(rec.CollectorDateTime).ToString("MMM dd, yyyy"),
                                              LocationName = GetLocationNameByUserID(rec.UserID)
                                          }).ToList();

                        if (model.StartDate != null && model.EndDate != null)
                        {

                            response = mdlGUIList.Where(x => x.Date >= Utility.GetDateFromString(model.StartDate) && x.Date <= Utility.GetDateFromString(model.EndDate)).OrderByDescending(o => o.Date).ToList<object>();
                            return response;
                        }
                        else
                        {
                            return response = mdlGUIList.ToList<object>();
                        }

                    }
                    //    return response;

                }
                else { return response; }
            }
            catch (Exception ex)
            {
                return response;
            }
        }
        public List<RecyclesDataExcel> GetSegregatedDataByID(int BranchID)
        {
            int Srno = 0; 
            var branch = context.Businesses.Where(x => x.ID == BranchID).FirstOrDefault();
            var UserFromDB = context.Users.Where(x => x.ID == branch.UserID).FirstOrDefault();
            List<RecyclesDataExcel> mdlRecycle = (from rec in context.Recycles
                                                  join recSubItem in context.RecycleSubItems on rec.ID equals recSubItem.RecycleID
                                                  join recSutItemTypes in context.RecycleSubItemsTypes on recSubItem.ID equals recSutItemTypes.RecycleSubItemID
                                                  where rec.IsActive == true && rec.UserID == UserFromDB.ID
                                                  select new RecyclesDataExcel
                                                  {
                                                      GreenPoints = (int)ConstantValues.WasteDefaultGCValuePerKG * getTotalWasteFromWasteTypes(recSubItem.ID),
                                                      Time = GetLocalDateTimeFromUTC(Convert.ToDateTime(rec.CreatedDate)).ToString("hh:mm tt"),
                                                      Weight = getWasteWeightFromRSTypes(recSubItem.ID),
                                                      NotWaste = getNotWasteFromWasteTypes(recSubItem.ID),
                                                      Date = rec.CollectorDateTime,                          //Convert.ToDateTime(rec.CollectorDateTime).ToString("MMM dd, yyyy"),
                                                      LocationName = GetLocationNameByUserID(rec.UserID),
                                                      Type = recSutItemTypes.WasteType.Name,
                                                      TWeight = recSutItemTypes.Weight,
                                                      RecycleID = rec.ID,
                                                  }).ToList().Select((x, index) => new RecyclesDataExcel
                                                  {
                                                      RowNumber = index + 1,
                                                      RecycleID = x.RecycleID,
                                                      Type = x.Type,
                                                      Weight = x.Weight,
                                                      GreenPoints = x.GreenPoints,
                                                      Time = x.Time,
                                                      NotWaste = x.NotWaste,
                                                      Date = x.Date,
                                                      LocationName = x.LocationName,
                                                      TWeight = x.TWeight,
                                                  }).ToList(); ;

            //  mdlRecycle = mdlRecycle


            return mdlRecycle;
        }
        public IList<RecyclesDataExcel> GOIListForSuperAdmin1s(int BranchID)
        {
            int CompanyId = 0;
            List<RecyclesDataExcel> mdlGUIList;
            var branch = context.Businesses.Where(x => x.ID == BranchID).FirstOrDefault();
            var UserFromDB = context.Users.Where(x => x.ID == branch.UserID).FirstOrDefault();

            mdlGUIList = (from rec in context.Recycles.ToList()
                          join subItem in context.RecycleSubItems on rec.ID equals subItem.RecycleID
                          where rec.IsActive == true && rec.UserID == UserFromDB.ID    //&&  rec.CollectorDateTime.Value.Date.Year == DateTime.Now.Year
                          select new RecyclesDataExcel
                          {
                              ID=  rec.ID,
                              GreenPoints = (int)ConstantValues.WasteDefaultGCValuePerKG * getTotalWasteFromWasteTypes(subItem.ID),
                              Time = GetLocalDateTimeFromUTC(Convert.ToDateTime(rec.CreatedDate)).ToString("hh:mm tt"),
                              Weight = getWasteWeightFromRSTypes(subItem.ID),
                              NotWaste = getNotWasteFromWasteTypes(subItem.ID),
                              Date = rec.CollectorDateTime,                          //Convert.ToDateTime(rec.CollectorDateTime).ToString("MMM dd, yyyy"),
                              LocationName = GetLocationNameByUserID(rec.UserID)
                          }).ToList();


            return mdlGUIList;


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
            public string Type { get; set; }
            public int RecycleID { get; set; }
            public decimal? TWeight { get; set; }
            public int RowNumber { get; set; }

        }
        public string GetLocationNameByUserID(int UserID)
        {
            string LocationName = string.Empty;
            var userLocation = context.Businesses.Where(x => x.UserID == UserID).FirstOrDefault();
            if(userLocation != null)
              {
                LocationName = userLocation.OfficeName ?? "";
              }

            return LocationName;
        }

        public List<object> GUIListData(int? UserID)
        {
            int CompanyId = 0;
            List<object> mdlGUIList;
            var UserCompany = context.Companies.Where(x => x.UserID == UserID).FirstOrDefault();
            if (UserCompany != null)
            {
                CompanyId = UserCompany.ID;
            }
            else
            {
                return new List<object>();
            }
            if (CompanyId == 0)
            {
                mdlGUIList = (from rec in context.Recycles.ToList()

                              join subItem in context.RecycleSubItems on rec.ID equals subItem.RecycleID
                              where rec.IsActive == true
                              select new
                              {
                                  rec.ID,
                                  subItem.GreenPoints,
                                  Time = Convert.ToDateTime(rec.CreatedDate).ToString("hh:mm tt"),
                                  subItem.Weight,
                                  Date = Convert.ToDateTime(rec.CollectorDateTime).ToString("MMM dd, yyyy"),
                              }).OrderByDescending(o => o.Date).ToList<object>();

                return mdlGUIList;
            }
            else
            {
                mdlGUIList = (from rec in context.Recycles.ToList()

                              join subItem in context.RecycleSubItems on rec.ID equals subItem.RecycleID
                              where rec.IsActive == true && rec.UserID == UserID
                              select new
                              {
                                  rec.ID,
                                  subItem.GreenPoints,
                                  Time = GetLocalDateTimeFromUTC( Convert.ToDateTime(rec.CollectorDateTime)).ToString("hh:mm tt"),
                                  subItem.Weight,
                                  Date = Convert.ToDateTime(rec.CollectorDateTime).ToString("MMM dd, yyyy"),
                              }).OrderByDescending(o => o.Date).ToList<object>();

                return mdlGUIList;
            }




        }
        //public List<object> GetGUIGroup(int UserId)
        //{

        //    List<object> lst = GUIList(UserId);
        //    var data = lst.Select(k => new { k..Year, k.Deliverydate.Month, k.TotalCharge }).GroupBy(x => new { x.Year, x.Month }, (key, group) => new
        //    {
        //        yr = key.Year,
        //        mnth = key.Month,
        //        tCharge = group.Sum(k => k.TotalCharge)
        //    }).ToList();

        //    return new List<object>();
        //}

         //TimeZone Conversion common methods. UTC vs LOCAL
        public DateTime GetLocalDateTimeFromUTC(DateTime dateTimeInUTC)
        {
            // TimeZone curTimeZone = TimeZone.CurrentTimeZone;
            TimeZoneInfo pakZone = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
            DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(dateTimeInUTC, pakZone);
            return easternTime;
          //  return TimeZoneInfo.ConvertTimeFromUtc(dateTimeInUTC,TimeZoneInfo.ConvertTimeBySystemTimeZoneId();
        }
        public DateTime GetUTCDateTimeFromLocal(DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTimeToUtc(dateTime);
        }
        public string GetLoggedInAdminBusinessName(int UserID)
        {
            var user = context.Users.Where(x => x.ID == UserID).FirstOrDefault();
            int? UserRoleID = 0;
            string BusinessName = string.Empty;
            if(user != null)
            {
                UserRoleID = user.RoleID;
            }
           
            if(UserRoleID == (int)UserRoleTypeEnum.Admin)
            {
                BusinessName = "DrTech Admin";
            }
            else if (UserRoleID == (int)UserRoleTypeEnum.BusinessAdmin)
            {
                var regBusiness = context.RegBusinesses.Where(y => y.UserID == UserID).FirstOrDefault();
                BusinessName = regBusiness.Name ?? "";
            }
            else if (UserRoleID == (int)UserRoleTypeEnum.SubBusinessAdmin)
            {
                var business = context.Businesses.Where(y => y.UserID == UserID).FirstOrDefault();
                BusinessName = business.OfficeName ?? "";
            }
            return BusinessName;

        }

    }
}
