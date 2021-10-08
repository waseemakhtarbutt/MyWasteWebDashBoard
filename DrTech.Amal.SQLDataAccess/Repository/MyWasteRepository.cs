using DrTech.Amal.SQLDataAccess.CustomModels;
using DrTech.Amal.SQLDatabase;
using DrTech.Amal.SQLModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DrTech.Amal.SQLDataAccess.Repository.CompanyRepository;


namespace DrTech.Amal.SQLDataAccess.Repository
{
    public class MyWasteRepository : Repository<WetWasteSchedule>
    {

        public MyWasteRepository(Amal_Entities context)
        : base(context)
        {
            dbSet = context.Set<WetWasteSchedule>();
        }

        public List<Object> GetScheduleDetails(int AreaID, int CompanyID=0)
        {
            Object mdlsch = (from wws in context.WetWasteSchedules
                             where wws.AreaID == AreaID 
                             select new
                             {
                                 wws.ID,
                                 wws.Month,
                                 wws.Year,
                             }).Take(1).OrderByDescending(o => o.ID).FirstOrDefault();
            int ID = 0;
            if (mdlsch != null)
            {
                ID = Convert.ToInt32(mdlsch.GetType().GetProperty("ID").GetValue(mdlsch));
            }

            List<Object> mdlSchool = new List<object>();
            if (ID != 0)
            {
                mdlSchool = (from wws in context.WetWasteSchedules.ToList()
                             join details in context.ScheduleDetails.ToList() on wws.ID equals details.ParentID
                             where details.ParentID == ID
                             select new
                             {
                                 wws.ID,
                                 Month = details.Date.Value.Month,
                                 Year = details.Date.Value.Year,
                                 //details.Day,

                                 FromTime = GetValidTime(details.FromTime ?? TimeSpan.Zero),
                                 ToTime = GetValidTime(details.ToTime ?? TimeSpan.Zero),
                                 details.Date,
                                 Day = details.Date.Value.Day
                             }).OrderByDescending(o => o.ID).ToList<object>();

            }


            return mdlSchool;
        }




        public decimal? getWieght(int RecycleID)
        {
            var result = context.RecycleSubItems.Where(x => x.RecycleID == RecycleID).FirstOrDefault();
            decimal? weight = 0;
            if (result != null)
            {
                weight = result.Weight;
            }
            else
            {
                return 0;
            }
            return weight;
        }
        public bool IsSegregated(int RecycleID)
        {
            var result = (from rec in context.Recycles
                          join recSub in context.RecycleSubItems on rec.ID equals recSub.RecycleID
                          join recSubType in context.RecycleSubItemsTypes on recSub.ID equals recSubType.RecycleSubItemID
                          where rec.ID == RecycleID
                          select rec).FirstOrDefault();

            if (result != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<DesegregatedDataViewModel> GetDesegregatedList()
        {

            List<DesegregatedDataViewModel> DesegregatedList = new List<DesegregatedDataViewModel>();
            List<DesegregatedDataViewModel> DesegregatedListCurrentDate = new List<DesegregatedDataViewModel>();

            List<RegBusiness> Companies = new List<RegBusiness>();

            Companies = (from regBus in context.RegBusinesses.ToList()
                         join usr in context.Users on regBus.UserID equals usr.ID
                         where usr.Type == "G"



                         select regBus).ToList();

            foreach (var company in Companies)
            {
                var listAllBrachs = context.Businesses.Where(xy => xy.ParentId == company.ID).ToList();
                foreach (var branch in listAllBrachs)
                {
                    var allRecycleByBranch = context.Recycles.ToList().Where(x => x.UserID == branch.UserID && x.CollectorDateTime.Value.Date == DateTime.Now.Date).ToList();
                    foreach (var recycle in allRecycleByBranch)
                    {
                        DesegregatedDataViewModel newRecord = new DesegregatedDataViewModel();
                        newRecord.UserID = recycle.UserID;
                        newRecord.Weight = Convert.ToDecimal(getWieght(recycle.ID));
                        newRecord.ID = recycle.ID;
                        newRecord.IsActive = recycle.IsActive;
                        newRecord.date = recycle.CreatedDate;
                        newRecord.CompanyName = company.Name;
                        newRecord.BranchName = branch.OfficeName;
                        DesegregatedList.Add(newRecord);
                    }
                }
            }

            DesegregatedList = DesegregatedList.OrderByDescending(o => o.date).ToList();
            // DesegregatedList.ForEach(x => Convert.ToDateTime(x.date).ToString("MMM dd, yyyy"));

            return DesegregatedList;
        }

        public List<DesegregatedDataViewModel> GetDesegregatedListBetweenTwoDates(DateRangeViewMdoel model)
        {

            List<DesegregatedDataViewModel> DesegregatedList = new List<DesegregatedDataViewModel>();

            List<RegBusiness> Companies = new List<RegBusiness>();

            Companies = (from regBus in context.RegBusinesses.ToList()
                         join usr in context.Users on regBus.UserID equals usr.ID
                         where usr.Type == "G"



                         select regBus).ToList();

            foreach (var company in Companies)
            {
                var listAllBrachs = context.Businesses.Where(xy => xy.ParentId == company.ID).ToList();
                foreach (var branch in listAllBrachs)
                {
                    var allRecycleByBranch = context.Recycles.ToList()
                        .Where(x => x.UserID == branch.UserID
                          && (x.CollectorDateTime.Value.Date >= model.start.Date && x.CollectorDateTime.Value.Date <= model.end.Date))
                        .ToList();
                    foreach (var recycle in allRecycleByBranch)
                    {
                        DesegregatedDataViewModel newRecord = new DesegregatedDataViewModel();
                        newRecord.UserID = recycle.UserID;
                        newRecord.Weight = Convert.ToDecimal(getWieght(recycle.ID));
                        newRecord.ID = recycle.ID;
                        newRecord.IsActive = recycle.IsActive;
                        newRecord.date = recycle.CollectorDateTime;
                        newRecord.CompanyName = company.Name;
                        newRecord.BranchName = branch.OfficeName;
                        DesegregatedList.Add(newRecord);
                    }
                }
            }
            DesegregatedList = DesegregatedList.OrderByDescending(o => o.date).ToList();
            // DesegregatedList.ForEach(x => Convert.ToDateTime(x.date).ToString("MMM dd, yyyy"));

            return DesegregatedList;
        }

        public int? getCompanyID(int branchID)
        {
            int? companyID = 0;
            var branch = context.Businesses.Where(x => x.ID == branchID).FirstOrDefault();
            companyID = branch.ParentId;
            return companyID;

        }

        public Object DesegregatedByID(int RecycleID)
        {
            Object DesegregatedList = new object();
            DesegregatedList = (from rec in context.Recycles.ToList()
                                join details in context.RecycleSubItems on rec.ID equals details.RecycleID
                                where rec.ID == RecycleID
                                select new
                                {
                                    RecycleD = rec.ID,
                                    //CompanyID = context.RegBusinesses.Where(x => x.UserID == rec.UserID).FirstOrDefault().ID,
                                    CompanyID = getCompanyID(context.Businesses.Where(x => x.UserID == rec.UserID).FirstOrDefault().ID),
                                    BranchID = context.Businesses.Where(x => x.UserID == rec.UserID).FirstOrDefault().ID,
                                    collectDate = rec.CollectorDateTime,
                                    Tweight = details.Weight,
                                    rec.UserID

                                }
                                ).OrderByDescending(o => o.RecycleD).FirstOrDefault();


            return DesegregatedList;
        }
        public string GetValidTime(TimeSpan timespan)
        {
            //  TimeSpan timespan = new TimeSpan(03, 00, 00);
            DateTime time = DateTime.Today.Add(timespan);
            string displayTime = time.ToString("hh:mm tt");
            return displayTime;
        }


        public List<SegregatedDataViewModel> GetSegregatedDataByID(int RecycleID)
        {
            int Srno = 0;
            List<SegregatedDataViewModel> mdlRecycle = (from rec in context.Recycles
                                                        join recSubItem in context.RecycleSubItems on rec.ID equals recSubItem.RecycleID
                                                        join recSutItemTypes in context.RecycleSubItemsTypes on recSubItem.ID equals recSutItemTypes.RecycleSubItemID
                                                        where rec.ID == RecycleID  //&& rec.IsActive == true 
                                                        select new SegregatedDataViewModel
                                                        {
                                                            RecycleID = rec.ID,
                                                            Type = recSutItemTypes.WasteType.Name,
                                                            Weight = recSutItemTypes.Weight,
                                                            rate = recSutItemTypes.Rate,
                                                            total = recSutItemTypes.Total

                                                        }).ToList().Select((x, index) => new SegregatedDataViewModel
                                                        {
                                                            RowNumber = index + 1,
                                                            RecycleID = x.RecycleID,
                                                            Type = x.Type,
                                                            Weight = x.Weight,
                                                            rate = x.rate,
                                                            total = x.total
                                                        }).ToList(); ;

            //  mdlRecycle = mdlRecycle


            return mdlRecycle;
        }

        public List<SegregatedDataViewModel> GetSegregatedDataByDate(DateRangeViewMdoel model)
        {
            int Srno = 0;
            try
            {
                int id = model.branchID;
                List<SegregatedDataViewModel> results = new List<SegregatedDataViewModel>();
                Business business = new Business();
                business =context.Businesses.Where(xy => xy.ID == id).FirstOrDefault();
                if (model.companyID > 0)
                {
                    List<SegregatedDataViewModel> mdlRecycle = (from rec in context.Recycles.ToList().Where(x => x.UserID == business.UserID && x.CollectorDateTime.Value.Date >= model.start.Date && x.CollectorDateTime.Value.Date <= model.end.Date)
                                                                join recSubItem in context.RecycleSubItems on rec.ID equals recSubItem.RecycleID
                                                                join recSutItemTypess in context.RecycleSubItemsTypes on recSubItem.ID equals recSutItemTypess.RecycleSubItemID
                                                                join wastetype in context.WasteTypes on recSutItemTypess.WasteTypeID equals wastetype.ID
                                                                select new SegregatedDataViewModel
                                                                {
                                                                    Type = wastetype.Name,
                                                                    Weight = recSutItemTypess.Weight,//.Select(g => g.Weight).DefaultIfEmpty(0).Sum() ?? 0,
                                                                    rate = recSutItemTypess.Rate,//Select(g => g.Rate).DefaultIfEmpty(0).Sum() ?? 0,
                                                                    total = recSutItemTypess.Total,//.Select(g => g.Total).DefaultIfEmpty(0).Sum() ?? 0
                                                                    CollectedDate= rec.CollectorDateTime
                                                                }).ToList().Select((x, index) => new SegregatedDataViewModel
                                                                {
                                                                    RowNumber = index + 1,
                                                                    Type = x.Type,
                                                                    Weight = x.Weight,
                                                                    rate = x.rate,
                                                                    total = x.total,
                                                                    CollectedDate=x.CollectedDate
                                                                }).ToList<SegregatedDataViewModel>();


                     results = (from p in mdlRecycle
                                                             group p by p.Type into g
                                                             select new SegregatedDataViewModel
                                                             {
                                                                 RowNumber = g.Select(c => c.RowNumber).FirstOrDefault()/**/,
                                                                 Type = g.Select(c => c.Type).FirstOrDefault()/**/,
                                                                 Weight = g.Select(c => c.Weight).DefaultIfEmpty(0).Sum() ?? 0,
                                                                 rate = g.Select(c => c.rate).FirstOrDefault(),
                                                                 total = g.Select(c => c.total).DefaultIfEmpty(0).Sum() ?? 0,
                                                                 Days= g.Select(c => c.CollectedDate).Count(),
                                                                 CompanyName = business.Name,
                                                                 BranchName = business.OfficeName,
                                                             }).ToList();
                }
                else {
                    List<SegregatedDataViewModel> mdlRecycle = (from rec in context.Recycles.ToList().Where(x => x.CollectorDateTime.Value.Date >= model.start.Date && x.CollectorDateTime.Value.Date <= model.end.Date)
                                                                join recSubItem in context.RecycleSubItems on rec.ID equals recSubItem.RecycleID
                                                                join recSutItemTypess in context.RecycleSubItemsTypes on recSubItem.ID equals recSutItemTypess.RecycleSubItemID
                                                                join wastetype in context.WasteTypes on recSutItemTypess.WasteTypeID equals wastetype.ID
                                                                select new SegregatedDataViewModel
                                                                {
                                                                    Type = wastetype.Name,
                                                                    Weight = recSutItemTypess.Weight,//.Select(g => g.Weight).DefaultIfEmpty(0).Sum() ?? 0,
                                                                    rate = recSutItemTypess.Rate,//Select(g => g.Rate).DefaultIfEmpty(0).Sum() ?? 0,
                                                                    total = recSutItemTypess.Total,//.Select(g => g.Total).DefaultIfEmpty(0).Sum() ?? 0
                                                                }).ToList().Select((x, index) => new SegregatedDataViewModel
                                                                {
                                                                    RowNumber = index + 1,
                                                                    Type = x.Type,
                                                                    Weight = x.Weight,
                                                                    rate = x.rate,
                                                                    total = x.total,
                                                                }).ToList<SegregatedDataViewModel>();


                   results = (from p in mdlRecycle
                                                             group p by p.Type into g
                                                             select new SegregatedDataViewModel
                                                             {
                                                                 Type = g.Select(c => c.Type).FirstOrDefault()/**/,
                                                                 RowNumber = g.Select(c => c.RowNumber).FirstOrDefault()/**/,
                                                                 Weight = g.Select(c => c.Weight).DefaultIfEmpty(0).Sum() ?? 0,
                                                                 rate = g.Select(c => c.rate).FirstOrDefault(),
                                                                 total = g.Select(c => c.total).DefaultIfEmpty(0).Sum() ?? 0,
                                                             }).ToList();
                }

            return results;
            }
            catch (Exception exp)
            {
                return null;//ServiceResponse.ErrorReponse<List<SegregatedDataViewModel>>(exp);
            }
        }
        public Object GetPaymentHistory(int ID)
        {
           

            Object mdlhistory = new object();
            if (ID != 0)
            {
                mdlhistory = (from user in context.Users.ToList()
                             join userpaymnet in context.UserPayments.ToList() on user.ID equals userpaymnet.UserID
                             where user.ID == ID
                             select new
                             {
                                 user.ID,
                                 UserName = user.FullName,
                                 CreatedDate=userpaymnet.CreatedDate,
                                 Amount=userpaymnet.AmountPaid,
                                 Purpose=userpaymnet.transactionResponse

                             }).OrderByDescending(o => o.ID).ToList<object>();

            }


            return mdlhistory;
        }



    }
}
