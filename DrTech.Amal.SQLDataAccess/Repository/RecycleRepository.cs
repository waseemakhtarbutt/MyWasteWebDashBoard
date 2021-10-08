using DrTech.Amal.Common.Enums;
using DrTech.Amal.Common.Helpers;
using DrTech.Amal.SQLDataAccess.CustomModels;
using DrTech.Amal.SQLDatabase;
using DrTech.Amal.SQLModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DrTech.Amal.SQLDataAccess.Repository
{
   public class RecycleRepository : Repository<Recycle>
    {
        public RecycleRepository(Amal_Entities context)
         : base(context)
        {
            dbSet = context.Set<Recycle>();
        }

        public int GetValueofWeight()
        {
            RefTable mdlRef = (from des in context.RefTables
                            where des.Type == "Recycle"
                            select des).FirstOrDefault();
            int value = 0;
            if (mdlRef != null)
            {
               value  = Convert.ToInt32(mdlRef.GreenPointValue);
            }
            
            return value;
        }
       

        public List<object> GetRecycleItem(int? id)
        {
            try { 
            List<object> mdlRecycle = (from rf in context.Recycles
                                       join sub in context.RecycleSubItems on rf.ID equals sub.RecycleID
                                       join ot in context.OrderTrackings on rf.ID equals ot.RsID
                                       where (rf.UserID == id) && (sub.IsParent == true) && (ot.Type == "Recycle")
                                       select new
                                       {
                                           Status = rf.Status.StatusName,
                                           ID = rf.ID,
                                           FileName = rf.FileName,
                                           Description = sub.Description,
                                           UserID = rf.UserID,
                                           CollectorDateTime = rf.CollectorDateTime.ToString(),
                                           StatusID = rf.StatusID,
                                           Weight = sub.Weight,
                                           GreenPoints = rf.GreenPoints,
                                           CreatedBy = rf.CreatedBy,
                                           CreatedDate = rf.CreatedDate.ToString(),
                                           UpdatedBy = rf.UpdatedBy,
                                           UpdatedDate = rf.UpdatedDate,
                                           IsActive = rf.IsActive,
                                           CollectedPendingConfirmation = ot.CollectedPendingConfirmation,
                                           DeliveredPendingConfirmation = ot.DeliveredPendingConfirmation,
                                           ot.DeliveredDate,
                                           ot.CollectedDate,
                                       }).ToList().Select(x => new
                                       {
                                           Status = x.Status,
                                           ID = x.ID,
                                           FileName = x.FileName,
                                           Description = x.Description,
                                           UserID = x.UserID,
                                           CollectorDateTime = Utility.GetFormattedDateTime(x.CollectorDateTime),
                                           StatusID = x.StatusID,
                                           Weight = GetWeight(x.ID),
                                           GreenPoints = subitemscalculate(x.ID),
                                           CreatedBy = x.CreatedBy,
                                           CreatedDate = string.IsNullOrEmpty(x.CreatedDate) ? string.Empty : Utility.GetFormattedDateTime(x.CreatedDate),
                                           UpdatedBy = x.UpdatedBy,
                                           UpdatedDate = Utility.CheckIfDateIsNotValid(x.UpdatedDate),     // Convert.ToDateTime(x.UpdatedDate).ToString("MMM dd, yyyy"),
                                           // UpdatedDate = string.IsNullOrEmpty(x.UpdatedDate) ? string.Empty : Utility.GetFormattedDateTime(x.UpdatedDate),
                                           IsActive = x.IsActive,
                                           //Carbons = GetValue(x.ID),
                                           CollectedPendingConfirmation = x.CollectedPendingConfirmation,
                                           DeliveredPendingConfirmation = x.DeliveredPendingConfirmation,
                                           CollectedDate = x.CollectedDate == null ? null : Convert.ToDateTime(x.CollectedDate).ToString("MMM dd, yyyy"),
                                           DeliveredDate = x.DeliveredDate == null ? null : Convert.ToDateTime(x.DeliveredDate).ToString("MMM dd, yyyy"),
                                           //CollectedDate = Convert.ToDateTime(x.CollectedDate).ToString("MMM dd, yyyy"),
                                           //DeliveredDate = Convert.ToDateTime(x.DeliveredDate).ToString("MMM dd, yyyy"),
                                       }).OrderByDescending(o => o.ID).ToList<object>();
                                      
            return mdlRecycle;
            }
            catch (Exception ex)
            {
                var message= ex.Message;
                return null;
            }
        }

        public List<object> TESTGetRecycleItem(int? id)
        {
           
           

                //var allRecycles = context.Recycles.Where(x => x.UserID == id).ToList();
                //var allRecycleSubItems = context.RecycleSubItems.Where(y => allRecycles.Select(k => k.ID).Contains(y.RecycleID)).ToList();
                //var allOrdersOfRecycles = context.OrderTrackings.Where(t => allRecycles.Select(y=>y.ID).Contains(t.RsID)).ToList();

                //List<object> list = new List<object>();
                //foreach (var item in collection)
                //{


                //}




                List<object>  mdlRecycle = (from rf in context.Recycles.ToList()
                                           join sub in context.RecycleSubItems.ToList() on rf.ID equals sub.RecycleID
                                           join ot in context.OrderTrackings.ToList() on rf.ID equals ot.RsID
                                           join st in context.Status.ToList() on rf.StatusID equals st.ID
                                           where (rf.UserID == id) && (sub.IsParent == true) && (ot.Type == "Recycle")
                                           select new
                                           {


                                                Status = st.StatusName,
                                               ID = rf.ID,
                                               FileName = rf.FileName ?? "",
                                               Description = sub.Description ?? "",
                                               UserID = rf.UserID,
                                               CollectorDateTime = rf.CollectorDateTime == null ? null : Utility.GetFormattedDateTime(rf.CollectorDateTime.ToString()),
                                               StatusID = rf.StatusID,
                                               Weight = GetWeight(rf.ID),
                                               GreenPoints = subitemscalculate(rf.ID),
                                               CreatedBy = rf.CreatedBy == null ? 0 : rf.CreatedBy,
                                               CreatedDate = rf.CreatedDate == null ? null : Convert.ToDateTime(rf.CreatedDate).ToString("MMM dd, yyyy"),
                                               UpdatedDate = rf.UpdatedDate == null ? null : Convert.ToDateTime(rf.UpdatedDate).ToString("MMM dd, yyyy"),
                                               UpdatedBy = rf.UpdatedBy ?? 0,
                                               IsActive = rf.IsActive ?? false,
                                               CollectedPendingConfirmation = ot.CollectedPendingConfirmation == null ? false : ot.CollectedPendingConfirmation,
                                               DeliveredPendingConfirmation = ot.DeliveredPendingConfirmation == null ? false : ot.DeliveredPendingConfirmation,
                                               CollectedDate = ot.CollectedDate == null ? null : Convert.ToDateTime(ot.CollectedDate).ToString("MMM dd, yyyy"),
                                               DeliveredDate = ot.DeliveredDate == null ? null : Convert.ToDateTime(ot.DeliveredDate).ToString("MMM dd, yyyy"),
                                               Carbons = GetValue(rf.ID),

                                           })
                                           .ToList<object>();
            
            
                                       
                                       //.Select(x => new
                                       //{
                                       //    Status = x.Status,
                                       //    ID = x.ID,
                                       //    FileName = x.FileName,
                                       //    Description = x.Description,
                                       //    UserID = x.UserID,
                                       //    CollectorDateTime = Utility.GetFormattedDateTime(x.CollectorDateTime),
                                       //    StatusID = x.StatusID,
                                       //    Weight = GetWeight(x.ID),
                                       //    GreenPoints = subitemscalculate(x.ID),
                                       //    CreatedBy = x.CreatedBy,
                                       //    CreatedDate = string.IsNullOrEmpty(x.CreatedDate) ? string.Empty : Utility.GetFormattedDateTime(x.CreatedDate),
                                       //    UpdatedBy = x.UpdatedBy,
                                       //    UpdatedDate = Convert.ToDateTime(x.UpdatedDate).ToString("MMM dd, yyyy"),
                                       //    // UpdatedDate = string.IsNullOrEmpty(x.UpdatedDate) ? string.Empty : Utility.GetFormattedDateTime(x.UpdatedDate),
                                       //    IsActive = x.IsActive,
                                       //    Carbons = GetValue(x.ID),
                                       //    CollectedPendingConfirmation = x.CollectedPendingConfirmation,
                                       //    DeliveredPendingConfirmation = x.DeliveredPendingConfirmation,
                                       //    CollectedDate = x.CollectedDate == null ? null : Convert.ToDateTime(x.CollectedDate).ToString("MMM dd, yyyy"),
                                       //    DeliveredDate = x.DeliveredDate == null ? null : Convert.ToDateTime(x.DeliveredDate).ToString("MMM dd, yyyy"),
                                       //    //CollectedDate = Convert.ToDateTime(x.CollectedDate).ToString("MMM dd, yyyy"),
                                       //    //DeliveredDate = Convert.ToDateTime(x.DeliveredDate).ToString("MMM dd, yyyy"),
                                       //}).OrderByDescending(o => o.ID).ToList<object>();

            return mdlRecycle;
        }

        public int subitemscalculate(int id)
        {
            int count = 0;
            var TotalGP = (from user in context.Recycles
                           join re in context.RecycleSubItems on user.ID equals re.RecycleID into regift
                           where user.ID == id
                           select new
                           {
                               RegiftCount = regift.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),

                           }).Select(u => new
                           {
                               TotalGP = u.RegiftCount
                           }).FirstOrDefault();

            if (TotalGP != null)
            {
                count = Convert.ToInt32(TotalGP.GetType().GetProperty("TotalGP").GetValue(TotalGP));
            }


            return count;
        }

        public decimal GetValue(int value)
        {
            if (value != 0)
            {
                decimal? Sum = 0;
                //List<RecycleSubItem> lstRec = (from sub in context.RecycleSubItems
                //                               where sub.RecycleID == value
                //                               select sub).ToList();
                //foreach (var item in lstRec)
                //{
                //    Sum += item.Weight;
                //}

                Sum = context.RecycleSubItems.Where(x => x.RecycleID == value).ToList().Sum(y => y.Weight);
                return Math.Ceiling(Convert.ToDecimal(Sum) / GetValueofWeight());
            }

            return 0;



            //if (value != null)
            //{ 
            //   int GP = Convert.ToInt32(value.GetType().GetProperty("Sum").GetValue(value));
            //return Math.Ceiling(Convert.ToDecimal(value) / GetValueofWeight());
            //}

            //return 0;   

        }

        public decimal? GetWeight(int value)
        {
            if (value != 0)
            {
                decimal? Sum = 0;
                //List<RecycleSubItem> lstRec = (from sub in context.RecycleSubItems
                //                               where sub.RecycleID == value
                //                               select sub).ToList();

                //foreach (var item in lstRec)
                //{
                //    Sum += item.Weight;
                //}
                Sum = context.RecycleSubItems.Where(x => x.RecycleID == value).ToList().Sum(y => y.Weight);


                return Sum;
            }

            return 0;



            //if (value != null)
            //{ 
            //   int GP = Convert.ToInt32(value.GetType().GetProperty("Sum").GetValue(value));
            //return Math.Ceiling(Convert.ToDecimal(value) / GetValueofWeight());
            //}

            //return 0;   

        }

        public List<object> GetRecyclesListByStatusExcel(RecycleRequest model)
        {
           
            List<object> response = new List<object>();
            var mdlRecycles = (from rc in context.Recycles.ToList()
                                            join sub in context.RecycleSubItems on rc.ID equals sub.RecycleID
                                            join status in context.Status on rc.StatusID equals status.ID
                                            join users in context.Users on rc.UserID equals users.ID
                                            join city in context.Cities on users.CityId equals city.ID
                                            join area in context.Areas on users.AreaID equals area.ID

                                            where (model.StatusID > 0 && rc.StatusID == model.StatusID && sub.IsParent == true) || (model.StatusID == 0 && sub.IsParent == true)
                                            select new
                               {
                                   statusDescription = status.StatusName,
                                   userName = users.FullName,
                                   city.CityName,
                                   areaName = area.Name,
                                   users.Address,
                                   users.Phone,
                                   CreatedDate = GetLocalDateTimeFromUTC(rc.CollectorDateTime).ToString("MMM dd, yyyy h:mm tt"),
                                   FDate = rc.CollectorDateTime
                               }).OrderByDescending(o => o.FDate).ToList();
            if (model.StartDate != null && model.EndDate != null)
            {

                return response = mdlRecycles.Where(x => x.FDate >= Utility.GetDateFromString(model.StartDate) && x.FDate <= Utility.GetDateFromString(model.EndDate)).OrderByDescending(o => o.CreatedDate).ToList<object>();

            }
            else
            {
                response = mdlRecycles.ToList<object>();
                return response;
            }


        }
        public List<object> GetRecyclesAllListByStatusExcel(RecycleRequest model)
        {
            List<object> response = new List<object>();
            var mdlRecycles = (from rc in context.Recycles.ToList()
                               join sub in context.RecycleSubItems on rc.ID equals sub.RecycleID
                               join status in context.Status on rc.StatusID equals status.ID
                               join users in context.Users on rc.UserID equals users.ID
                               join city in context.Cities on users.CityId equals city.ID
                               join area in context.Areas on users.AreaID equals area.ID

                               where (model.StatusID > 0 && sub.IsParent == true) || (model.StatusID == 0 && sub.IsParent == true)
                               select new
                               {
                                   statusDescription = status.StatusName,
                                   userName = users.FullName,
                                   city.CityName,
                                   areaName = area.Name,
                                   users.Address,
                                   users.Phone,
                                   CreatedDate = GetLocalDateTimeFromUTC(rc.CollectorDateTime).ToString("MMM dd, yyyy h:mm tt"),
                                   FDate =rc.CollectorDateTime
                               }).OrderByDescending(o => o.FDate).ToList();

            if (model.StartDate != null && model.EndDate != null)
            {

                response = mdlRecycles.Where(x => x.FDate >= Utility.GetDateFromString(model.StartDate) && x.FDate <= Utility.GetDateFromString(model.EndDate)).OrderByDescending(o => o.CreatedDate).ToList<object>();
                return response;            }
            else
            {
                response = mdlRecycles.ToList<object>();
                return response;
            }
        }
        public List<RecycleDto> GetRecyclesListByStatus(RecycleRequest model)
        {
            List<RecycleDto> mdlRecycles = (from rc in context.Recycles.ToList()
                                       // join sub in context.RecycleSubItems on rc.ID equals sub.RecycleID
                                        join status in context.Status on rc.StatusID equals status.ID
                                        join users in context.Users on rc.UserID equals users.ID
                                        join city in context.Cities on users.CityId equals city.ID
                                        join area in context.Areas on users.AreaID equals area.ID

                                        where (model.StatusID > 0 && rc.StatusID == model.StatusID ) || (model.StatusID == 0 )
                                        select new RecycleDto
                                        {
                                          ID =  rc.ID,
                                            //sub.Description,
                                           // rc.GreenPoints,
                                          statusDescription = status.StatusName,
                                            //users.Longitude,
                                            //users.Latitude,
                                            userId = users.ID,
                                            userName = users.FullName,
                                           /// rc.FileName,
                                           CreatedDate = rc.CreatedDate,
                                           CityName = city.CityName,
                                            areaName=  area.Name,
                                           Address = users.Address,
                                           collectionDate = rc.CollectorDateTime,
                                            collectorDateTime = GetLocalDateTimeFromUTC(rc.CollectorDateTime).ToString("MMM dd, yyyy h:mm tt"),
                                            ///updatedDate = Convert.ToDateTime(rc.CreatedDate).ToString("MMM dd, yyyy "),
                                        }).OrderByDescending(o => o.CreatedDate).ToList();
            if(model.StartDate != null && model.EndDate != null)
            {
              
              return  mdlRecycles.Where(x => x.collectionDate >=  Utility.GetDateFromString(model.StartDate) && x.collectionDate <= Utility.GetDateFromString(model.EndDate)).ToList();
            }
            else
            {
                return mdlRecycles;
            }

            
        }
        public List<object> GetRecyclesAllListByStatus(RecycleRequest model)
        {
            List<object> response = new List<object>();
          var mdlRecycles = (from rc in context.Recycles.ToList()
                                        ///join sub in context.RecycleSubItems on rc.ID equals sub.RecycleID
                                        join status in context.Status on rc.StatusID equals status.ID
                                        join users in context.Users on rc.UserID equals users.ID
                                        join city in context.Cities on users.CityId equals city.ID
                                        join area in context.Areas on users.AreaID equals area.ID

                                        where (model.StatusID > 0) || (model.StatusID == 0)
                                        select new
                                        {
                                            rc.ID,
                                            //sub.Description,
                                            // rc.GreenPoints,  
                                            statusDescription = status.StatusName,
                                            //users.Longitude,
                                            //users.Latitude,
                                            userId = users.ID,
                                            userName = users.FullName,
                                            /// rc.FileName,=
                                          //  sub.Weight,
                                            CreatedDate = GetLocalDateTimeFromUTC(rc.CollectorDateTime).ToString("MMM dd, yyyy h:mm tt"),
                                            city.CityName,
                                            areaName = area.Name,
                                            users.Address,
                                            users.Phone,
                                            collectionDate = rc.CollectorDateTime,
                                            collectorDateTime = GetLocalDateTimeFromUTC(rc.CollectorDateTime).ToString("MMM dd, yyyy h:mm tt"),
                                            ///updatedDate = Convert.ToDateTime(rc.CreatedDate).ToString("MMM dd, yyyy "),
                                        }).OrderByDescending(o => o.CreatedDate).ToList();

            if (model.StartDate != null && model.EndDate != null)
            {
               
                response = mdlRecycles.Where(x => x.collectionDate >= Utility.GetDateFromString(model.StartDate) && x.collectionDate <= Utility.GetDateFromString(model.EndDate)).OrderByDescending(o => o.CreatedDate).ToList<object>();
                return response;
                // return mdlRecycles.Where(x => x.CreatedDate >= model.StartDate && x.CreatedDate <= model.EndDate).ToList();
            }
            else
            {
                response = mdlRecycles.ToList<object>();
                return response;
            }
        }
        public DateTime GetLocalDateTimeFromUTC(DateTime? dateTimeInUTC)
        {
            DateTime dateTimeInUTC1 = Convert.ToDateTime(dateTimeInUTC);
            TimeZoneInfo pakZone = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
            DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(dateTimeInUTC1, pakZone);
            return easternTime;
            //  return TimeZoneInfo.ConvertTimeFromUtc(dateTimeInUTC,TimeZoneInfo.ConvertTimeBySystemTimeZoneId();
        }
        public RecycleViewModel GetRecycleDetailById(int RecycleID, bool IsWebAdmin)
        {
            RecycleViewModel mdlRecycle = (from rc in context.Recycles
                                         join users in context.Users on rc.UserID equals users.ID
                                         join orderTracking in context.OrderTrackings on rc.ID equals orderTracking.RsID
                                         join status in context.Status on orderTracking.StatusID equals status.ID
                                         join city in context.Cities on users.CityId equals city.ID
                                         where (rc.ID == RecycleID && orderTracking.Type == "Recycle")
                                         select new RecycleViewModel()
                                         {
                                             RecycleSubItems = rc.RecycleSubItems.Where(x => (x.IsParent == true && !IsWebAdmin) || IsWebAdmin).Select(x => new RecycleSubItemViewModel
                                             {
                                                 ID = x.ID,
                                                 Description = x.Description,
                                                 Weight = x.Weight
                                             }).ToList(),
                                             RecycleComments = rc.RecycleComments.OrderByDescending(x => x.CreatedDate).Select(x => new CommentsViewModel()
                                             {
                                                 ID = x.ID,
                                                 Comments = x.Comments,
                                                 Date = SqlFunctions.DateName("m", x.CreatedDate) + " " + SqlFunctions.DateName("dd", x.CreatedDate) + ", " + SqlFunctions.DateName("yyyy", x.CreatedDate) + " " +
                                                   SqlFunctions.DateName("hh", x.CreatedDate) + ":" + SqlFunctions.DateName("n", x.CreatedDate),
                                                 User = x.User.FullName
                                             }).ToList(),
                                             ID = rc.ID,
                                             Description = (rc.RecycleSubItems.Where(x=>x.IsParent == true).FirstOrDefault() as RecycleSubItem).Description,
                                             Latitude = users.Latitude,
                                             Longitude = users.Longitude,
                                             FileNameTakenByUser = orderTracking.FileNameTakenByUser,
                                             FileNameTakenByDriver = orderTracking.FileNameTakenByDriver,
                                             FileNameTakenByOrg = orderTracking.FileNameTakenByOrg,
                                             CollectedPendingConfirmation = orderTracking.CollectedPendingConfirmation == null ? false : orderTracking.CollectedPendingConfirmation,
                                             DeliveredPendingConfirmation = orderTracking.DeliveredPendingConfirmation == null ? false : orderTracking.DeliveredPendingConfirmation,
                                             OrderID = orderTracking.ID,
                                             UserID = users.ID,
                                             UserName = users.FullName,
                                             CityName=city.CityName,
                                             UserPhone = users.Phone,
                                             UserAddress = users.Address,
                                             StatusName = status.StatusName,
                                             CollectDate = rc.CollectorDateTime,
                                             OrderStatusID = (orderTracking.StatusID == (int)StatusEnum.Pending || orderTracking.StatusID == (int)StatusEnum.Declined) ? orderTracking.StatusID : -1,
                                             AssignTo = orderTracking.AssignTo ?? -1,
                                             Cash = rc.Cash==null ? 0 : (decimal)rc.Cash 
                                         }).ToList()[0];

            return mdlRecycle;
        }
        public bool AssignRecycleToDriver(RecycleViewModel _mdlRecycleVM, int? userId)
        {
            try
            {
                // Recycle

                Recycle mdlRecycle = context.Recycles.Include(x=>x.RecycleSubItems).First(x => x.ID == _mdlRecycleVM.ID);
                DateTime dateTime = Convert.ToDateTime(_mdlRecycleVM.CollectDate);

                mdlRecycle.CollectorDateTime = Utility.GetLocalDateTimeFromUTC(dateTime);

                if (_mdlRecycleVM.AssignTo != null)
                {
                    if (_mdlRecycleVM.AssignTo != -1)
                    {
                        mdlRecycle.StatusID = (int)StatusEnum.InProgress;
                        mdlRecycle.GreenPoints = 0; //_mdlRegiftVM.TotalGP;
                    }
                    else if (_mdlRecycleVM.AssignTo == -1)
                    {
                        if (_mdlRecycleVM.OrderStatusID == (int)StatusEnum.Pending || _mdlRecycleVM.OrderStatusID == (int)StatusEnum.Declined)
                            mdlRecycle.StatusID = _mdlRecycleVM.OrderStatusID;
                        else
                            mdlRecycle.StatusID = (int)StatusEnum.Submit;
                    }
                }

                mdlRecycle.UpdatedBy = userId;
                mdlRecycle.UpdatedDate = DateTime.Now;

                // Regift Subitems

                List<RecycleSubItem> lstSubItems = new List<RecycleSubItem>();

                bool flag = true;

                foreach (RecycleSubItemViewModel subitem in _mdlRecycleVM.RecycleSubItems)
                {
                    lstSubItems.Add(new RecycleSubItem()
                    {
                        Description = subitem.Description,
                        Weight = subitem.Weight,
                        RecycleID = _mdlRecycleVM.ID,
                        IsParent = (flag ? true : false),
                        CreatedBy = userId,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        GreenPoints = 0  //Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(subitem.Weight))) * _mdlRecycleVM.GPV
                    });

                    flag = false;
                }

                context.RecycleSubItems.RemoveRange(mdlRecycle.RecycleSubItems);
                mdlRecycle.RecycleSubItems = lstSubItems;

                // Order Tracking

                if (_mdlRecycleVM.AssignTo != null)
                {
                    OrderTracking mdlOrderTracking = context.OrderTrackings.Find(_mdlRecycleVM.OrderID);

                    if (_mdlRecycleVM.AssignTo != -1)
                    {
                        if (mdlOrderTracking.AssignTo != _mdlRecycleVM.AssignTo)
                        {
                            mdlOrderTracking.CollectedPendingConfirmation = null;
                            mdlOrderTracking.DeliveredPendingConfirmation = null;
                        }

                        mdlOrderTracking.AssignTo = _mdlRecycleVM.AssignTo;
                        mdlOrderTracking.StatusID = (int)StatusEnum.Assigned;                       
                    }
                    else if (_mdlRecycleVM.AssignTo == -1)
                    {
                        mdlOrderTracking.AssignTo = null;

                        if (_mdlRecycleVM.OrderStatusID == (int)StatusEnum.Pending || _mdlRecycleVM.OrderStatusID == (int)StatusEnum.Declined)
                            mdlOrderTracking.StatusID = _mdlRecycleVM.OrderStatusID;
                        else
                            mdlOrderTracking.StatusID = (int)StatusEnum.New;
                    }

                    mdlOrderTracking.UpdatedBy = userId;
                    mdlOrderTracking.UpdatedDate = DateTime.Now;
                }

                // Recycle Comments

                //if (!string.IsNullOrEmpty(_mdlRecycleVM.Comments))
                {
                    RecycleComment recycleComments = new RecycleComment()
                    {
                        Comments = _mdlRecycleVM.Comments ?? string.Empty,
                        CreatedBy = Convert.ToInt32(userId),
                        CreatedDate = DateTime.Now,
                        UpdatedBy = Convert.ToInt32(userId),
                        UpdatedDate = DateTime.Now,
                        IsActive = true
                    };

                    mdlRecycle.RecycleComments.Add(recycleComments);
                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
        public bool CollectedRecycleByDriver(int? recycleID, List<RecycleSubItem> lstSubItems)
        {
            try
            {
                // Recycle
                Recycle mdlRecycle = context.Recycles.Include(x => x.RecycleSubItems).First(x => x.ID == recycleID);

                context.RecycleSubItems.RemoveRange(mdlRecycle.RecycleSubItems);
                mdlRecycle.RecycleSubItems = lstSubItems;

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
        public bool RejectRecycle(RecycleViewModel _mdlRecycleVM, int? userId)
        {
            try
            {
                // Recycle

                Recycle mdlRecycle = context.Recycles.First(x => x.ID == _mdlRecycleVM.ID);

                mdlRecycle.StatusID = (int)StatusEnum.Declined;

                mdlRecycle.UpdatedBy = userId;
                mdlRecycle.UpdatedDate = DateTime.Now;

                // Order Tracking

                OrderTracking mdlOrderTracking = context.OrderTrackings.Where(x => x.Type == "Recycle" && x.RsID == _mdlRecycleVM.ID).FirstOrDefault();

                mdlOrderTracking.StatusID = (int)StatusEnum.Declined;

                mdlOrderTracking.UpdatedBy = userId;
                mdlOrderTracking.UpdatedDate = DateTime.Now;

                // Recycle Comments

                if (!string.IsNullOrEmpty(_mdlRecycleVM.Comments))
                {
                    RecycleComment recycleComments = new RecycleComment()
                    {
                        Comments = _mdlRecycleVM.Comments,
                        CreatedBy = Convert.ToInt32(userId),
                        CreatedDate = DateTime.Now,
                        UpdatedBy = Convert.ToInt32(userId),
                        UpdatedDate = DateTime.Now,
                        IsActive = true
                    };

                    mdlRecycle.RecycleComments.Add(recycleComments);
                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

    }
}

