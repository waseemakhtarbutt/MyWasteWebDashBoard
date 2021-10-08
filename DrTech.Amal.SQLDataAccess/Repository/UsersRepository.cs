using DrTech.Amal.Common.Enums;
using DrTech.Amal.Common.Helpers;
using DrTech.Amal.SQLDataAccess.CustomModels;
using DrTech.Amal.SQLDatabase;
using DrTech.Amal.SQLModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DrTech.Amal.SQLDataAccess.Repository
{
    public class UsersRepository : Repository<User>
    {
        //ContextDB db = new ContextDB();
        public UsersRepository(Amal_Entities context)
            : base(context)
        {
            dbSet = context.Set<User>();
        }


        public User GetBasicUserDetails(string Phone)
        {
            User mdlUser = (from des in context.Users
                            where des.Phone == Phone && des.Phone != null
                            select des).FirstOrDefault();
            return mdlUser;
        }

        public User GetUserDetailsByID(int UserID)
        {
            User mdlUser = (from des in context.Users
                            where des.ID == UserID
                            select des).FirstOrDefault();


            return mdlUser;
        }

        public User GetUserByPhoneandPassword(string Phone, string Password)
        {
            User mdlUser = (from des in context.Users
                            where des.Phone == Phone && des.Password == Password
                            select des).FirstOrDefault();
            return mdlUser;
        }

        public object GetCountsByUserID(int UserID)
        {
            var mdlUser = (from des in context.Users
                           where des.ID == UserID
                           select new
                           {
                               ChildrenCount = 0,//des.Children.Where(x => x.IsActive == true).Count(),
                               EmployeesCount =0,// des.Employments.Where(x => x.IsActive == true).Count(),
                               MembersCount = 0,//des.Members.Where(x => x.IsActive == true).Count(),
                           }).FirstOrDefault();
            return mdlUser;
        }

        public User GetUserDetailsByEmail(string Email)
        {
            User mdlUser = (from des in context.Users
                            where des.Email == Email
                            select des).FirstOrDefault();
            return mdlUser;
        }

        public User GetUserDetailsBySocialMediaKey(string SocialMediaKey)
        {
            User mdlUser = (from des in context.Users
                            where des.SocialMediaKey == SocialMediaKey
                            select des).FirstOrDefault();
            return mdlUser;
        }

        public List<User> GetUserBySchoolID(int SchoolID)
        {
            List<User> mdlUser = (from des in context.Users
                                  join child in context.Children on des.ID equals child.UserID
                                  join school in context.Schools on child.SchoolID equals school.ID
                                  where school.ID == SchoolID
                                  select des).Distinct().ToList();
            return mdlUser;
        }

        public List<object> GetUserByBusinessID(int OrgID)
        {
            List<object> mdlUser = (from des in context.Users
                                    join emp in context.Employments on des.ID equals emp.UserID
                                    join Org in context.Businesses on emp.BusId equals Org.ID
                                    where Org.ID == OrgID
                                    select des).Distinct().ToList<object>();
            return mdlUser;
        }

        public List<User> GetUserByNGOID(int NGOID)
        {
            List<User> mdlUser = (from des in context.Users
                                  join emp in context.Members on des.ID equals emp.UserID
                                  join Org in context.Organizations on emp.OrgId equals Org.ID
                                  where Org.ID == NGOID
                                  select des).Distinct().ToList();
            return mdlUser;
        }

        public GPNAverageViewModel GetGPNAverageByUser(int UserID)
        {
            IEnumerable<GPNAverageViewModel> result = (from user in context.Users
                                                       join re in context.Refuses on user.ID equals re.UserID into refuses
                                                       join rd in context.Reduces on user.ID equals rd.UserID into reduses
                                                       join ru in context.Reuses on user.ID equals ru.UserID into reuses
                                                       join rp in context.Replants on user.ID equals rp.UserID into replants
                                                       join rc in context.Recycles on user.ID equals rc.UserID into recycles
                                                       join rg in context.Regifts on user.ID equals rg.UserID into regifts
                                                       join rp in context.Reports on user.ID equals rp.UserID into reports
                                                       join bin in context.BuyBins on user.ID equals bin.UserID into bins
                                                       where user.ID == UserID
                                                       select new GPNAverageViewModel
                                                       {
                                                           Refuses = refuses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                                           Reduces = reduses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                                           Reuses = reuses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum() ?? 0,
                                                           Replants = replants.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                                           Recycles = recycles.Select(g => g.RecycleSubItems).Select(x => x.Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum() ?? 0,
                                                           Regifts = regifts.Select(g => g.RegiftSubItems).Select(x => x.Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum() ?? 0,
                                                           Reports = reports.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                                           Bins = bins.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum() ?? 0,
                                                           //Redeemed = user.Redeemed ? 0  : (decimal)user.Redeemed
                                                           //Redeemed = user.Redeemed ? 0 : (decimal)user.Redeemed
                                                       });
            GPNAverageViewModel gpAverage;
            if (result.ToList().Count > 0)
            {
                gpAverage = result.ToList()[0];
                gpAverage.TotalGW = gpAverage.Refuses + gpAverage.Reduces + gpAverage.Reuses + gpAverage.Replants + gpAverage.Recycles + gpAverage.Regifts + gpAverage.Reports + gpAverage.Bins;
                gpAverage.RedeemedPoints = context.GCRedeems.Where(x => x.UserID == UserID).ToList().Sum(y => y.GCRedeemed);
                gpAverage.RedeemablePoints = gpAverage.TotalGW - gpAverage.RedeemedPoints;

            }
            else
            {
                gpAverage = new GPNAverageViewModel();
                gpAverage.TotalGW = 0;
            }
            //if (gpAverage.TotalGW > 0)
            //{
            //    gpAverage.Refuses = (gpAverage.Refuses / gpAverage.TotalGW) * 100;
            //    gpAverage.Reduces = (gpAverage.Reduces / gpAverage.TotalGW) * 100;
            //    gpAverage.Reuses = (gpAverage.Reuses / gpAverage.TotalGW) * 100;
            //    gpAverage.Replants = (gpAverage.Replants / gpAverage.TotalGW) * 100;
            //    gpAverage.Recycles = (gpAverage.Recycles / gpAverage.TotalGW) * 100;
            //    gpAverage.Regifts = (gpAverage.Regifts / gpAverage.TotalGW) * 100;
            //    gpAverage.Reports = (gpAverage.Reports / gpAverage.TotalGW) * 100;
            //    gpAverage.Bins = (gpAverage.Bins / gpAverage.TotalGW) * 100;
            //}

            return gpAverage;
        }

        public dynamic GetUserRsCount(int? UserID)
        {
            var v = from user in context.Users
                    where user.ID == UserID
                    //  select u;
                    select new
                    {
                        Refuse = user.Refuses.Count,
                        Reduce = user.Reduces.Count,
                        Reuse = user.Reuses.Count,
                        Recycle = user.Recycles.Count,
                        Regift = user.Regifts.Count,
                        Replant = user.Replants.Count,
                        Report = user.Reports.Count,
                    };

            return v;
        }

        public dynamic GetAllMapPins()
        {
            var refusePins = (from refuse in context.Refuses select new { refuse.Latitude, refuse.Longitude, refuse.User.FullName, refuse.GreenPoints }).ToList();
            var reducePins = (from refuse in context.Reduces select new { refuse.User.Latitude, refuse.User.Longitude, refuse.User.FullName, refuse.GreenPoints }).ToList();
            var reusePins = (from refuse in context.Reuses select new { refuse.User.Latitude, refuse.User.Longitude, refuse.User.FullName, refuse.GreenPoints }).ToList();
            var replantPins = (from refuse in context.Replants select new { refuse.Latitude, refuse.Longitude, refuse.User.FullName, refuse.GreenPoints }).ToList();
            var regiftPins = (from refuse in context.Regifts select new { refuse.User.Latitude, refuse.User.Longitude, refuse.User.FullName, refuse.GreenPoints }).ToList();
            var recyclePins = (from refuse in context.Recycles select new { refuse.User.Latitude, refuse.User.Longitude, refuse.User.FullName, refuse.GreenPoints }).ToList();
            var reportPins = (from refuse in context.Reports select new { refuse.Latitude, refuse.Longitude, refuse.User.FullName, refuse.GreenPoints }).ToList();

            Dictionary<string, object> allRs = new Dictionary<string, object>
            {
                { "Refuse", refusePins },
                { "Reduce", reducePins },
                { "Reuse", reusePins },
                { "Replant", replantPins },
                { "Regift", regiftPins },
                { "Recycle", recyclePins},
                { "Report", reportPins },
            };

            return allRs;
        }

        public GPNAverageViewModel GetUserTotalGP(int UserID)
        {
            GPNAverageViewModel TotalGP = (from user in context.Users
                                           join re in context.Refuses on user.ID equals re.UserID into refuses
                                           join rd in context.Reduces on user.ID equals rd.UserID into reduses
                                           join ru in context.Reuses on user.ID equals ru.UserID into reuses
                                           join rp in context.Replants on user.ID equals rp.UserID into replants
                                           join rc in context.Recycles on user.ID equals rc.UserID into recycles
                                           join rg in context.Regifts on user.ID equals rg.UserID into regifts
                                           join rp in context.Reports on user.ID equals rp.UserID into reports
                                           join bin in context.BuyBins on user.ID equals bin.UserID into bin
                                           where user.ID == UserID
                                           select new 
                           {
                                               Refuses = refuses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                               Reduces = reduses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                               Reuses = reuses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum() ?? 0,
                                               Replants = replants.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                               Recycles = recycles.Select(g => g.RecycleSubItems).Select(x => x.Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum() ?? 0,
                                               Regifts = regifts.Select(g => g.RegiftSubItems).Select(x => x.Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum() ?? 0,
                                               Reports = reports.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                               Bins = bin.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum() ?? 0,
                                               //Redeemed = user.Redeemed ? 0  : (decimal)user.Redeemed
                                               //Redeemed = user.Redeemed ? 0 : (decimal)user.Redeemed
                                           }).Select(u => new GPNAverageViewModel
                           {
                                               TotalGP = u.Refuses + u.Reduces + u.Reuses + u.Replants + u.Recycles + u.Regifts + u.Reports + u.Bins,
                                               Refuses = u.Refuses,
                                               Reduces = u.Reduces,
                                               Reuses = u.Reuses,
                                               Replants = u.Replants,
                                               Recycles = u.Recycles,
                                               Regifts = u.Regifts,
                                               Reports = u.Reports,
                                               Bins = u.Bins

                                           }).FirstOrDefault();


            //var vv = from user in context.Users
            //         where user.ID == UserID
            //         select new
            //         {
            //             Refuse = user.Refuses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum()
            //         };


            return TotalGP;
        }
        public int GetUserGreenPointsTotal(int UserId)
        {
            object TotalGP = GetUserTotalGP(UserId);
            int GP = 0;
            if (TotalGP != null)
            {

                GP = Convert.ToInt32(TotalGP.GetType().GetProperty("TotalGP").GetValue(TotalGP));
            }
            return GP;
        }

        public List<object> TopGPUsers(int UserId)
        {
            //var currentUser = context.Users.Where(x => x.ID == UserId).FirstOrDefault();


            var query = (from u in context.Users
                         join c in context.Children on u.ID equals c.UserID
                         join s in context.Schools on c.SchoolID equals s.ID
                         where s.ID == UserId
                         select new
                         {
                             UserID = u.ID,
                             FullName = u.FullName,
                             SchoolName = s.BranchName

                         })
                    .ToList()
                    .Distinct()
                    //.OrderByDescending(d => d.GPs)
                    //.Take(20)
                    .Select(o => new
                    {
                        UserID = o.UserID,
                        FullName = o.FullName,
                        GPs = GetUserGreenPointsTotal(o.UserID)
                    })
                    .OrderByDescending(d => d.GPs)
                    .Take(20)
                    .ToList();




            return query.ToList<object>();


           // user.RoleID != (int)UserRoleTypeEnum.Admin
            //if (currentUser != null)
            //{
            //    if(currentUser.RoleID == (int)UserRoleTypeEnum.Admin)
            //    {

            //    }
            //    else if (currentUser.RoleID == (int)UserRoleTypeEnum.SchoolAdmin)
            //    {

            //    }
            //    else if (currentUser.RoleID == (int)UserRoleTypeEnum.BusinessAdmin)
            //    {

            //    }
            //    else if (currentUser.RoleID == (int)UserRoleTypeEnum.OrganizationAdmin)
            //    {

            //    }
            //}
            //return new List<TopGPViewModel>();
        }

        public int GetSchoolIDFromUserID(int? UserID)
        {
            int SchoolID = 0;

            var UserSchool =
                       (from u in context.Users
                       // join c in context.Children on u.ID equals c.UserID
                        join s in context.Schools on u.ID equals s.UserID
                       
                        select new
                        {
                            SchoolID = s.ID
                        })
                        .FirstOrDefault();
            if(UserSchool != null)
            {
                SchoolID = UserSchool.SchoolID;
            }
            return SchoolID;

        }

        public object RsCountForGPN(int SchoolID)
        {
            var SchoolUsers =
                        (from u in context.Users
                         join c in context.Children on u.ID equals c.UserID
                         join s in context.Schools on c.SchoolID equals s.ID
                         where s.ID == SchoolID
                         select new
                         {
                             UserID = u.ID
                         })
                         .ToList();

            List<int> lstUsers = SchoolUsers.Select(d => d.UserID).ToList<int>();


            var RsCount = (from user in context.Users
                           join re in context.Refuses on user.ID equals re.UserID into refuses
                           join rd in context.Reduces on user.ID equals rd.UserID into reduses
                           join ru in context.Reuses on user.ID equals ru.UserID into reuses
                           join rp in context.Replants on user.ID equals rp.UserID into replants
                           join rc in context.Recycles on user.ID equals rc.UserID into recycles
                           join rg in context.Regifts on user.ID equals rg.UserID into regifts
                           join rp in context.Reports on user.ID equals rp.UserID into reports
                           //join bin in context.BuyBins on user.ID equals bin.UserID into bin
                           where lstUsers.Contains(user.ID)
                           select new
                           {
                               //user.ID,
                               //user.FullName,
                               Refuses = refuses.Where(x => x.StatusID == (int)StatusEnum.Resolved),
                               Reduces = reduses.Where(x => x.StatusID == (int)StatusEnum.Resolved),
                               Reuses = reuses.Where(x => x.StatusID == (int)StatusEnum.Resolved),
                               Replants = replants.Where(x => x.StatusID == (int)StatusEnum.Resolved),
                               Recycles = recycles.Where(x => x.StatusID == (int)StatusEnum.Complete),
                               Regifts = regifts.Where(x => x.StatusID == (int)StatusEnum.Delivered),
                               Reports = reports.Where(x => x.StatusID == (int)StatusEnum.Resolved),
                               //BuyBins = bin.Where(x => x.StatusID == (int)StatusEnum.Delivered)
                           })
                           .Select(u => new
                           {
                               //UserID = u.ID,
                               //FullName = u.FullName,
                               Refuses = u.Refuses.DefaultIfEmpty().Count(),
                               Reduces = u.Reduces.DefaultIfEmpty().Count(),
                               Reuses = u.Reuses.DefaultIfEmpty().Count(),
                               Replants = u.Replants.DefaultIfEmpty().Count(),
                               Recycles = u.Recycles.DefaultIfEmpty().Count(),
                               Regifts = u.Regifts.DefaultIfEmpty().Count(),
                               Reports = u.Reports.DefaultIfEmpty().Count()
                               //Bins = u.BuyBins.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum() ?? 0,
                           });


            int RefuseCount = 0;
            int ReduceCount = 0;
            int ReuseCount = 0;
            int ReplantCount = 0;
            int RecycleCount = 0;
            int RegiftCount = 0;
            int ReportCount = 0;

            foreach (var v in RsCount)
            {
                RefuseCount += v.Refuses;
                ReduceCount += v.Reduces;
                ReuseCount += v.Reuses;
                ReplantCount += v.Replants;
                RecycleCount += v.Recycles;
                RegiftCount += v.Regifts;
                ReportCount += v.Reports;
            }

            object o = new { RefuseCount, ReduceCount, ReuseCount, ReplantCount, RecycleCount, RegiftCount, ReportCount };

            return o;


        }
        public List<spGetDailyGreenPoints_Result> GetDailyGreenPointsGraph(int UserID)
        {          
            var v = (from goi in context.spGetDailyGreenPoints(UserID)
                     select goi);
            return v.ToList<spGetDailyGreenPoints_Result>();
        }
        public List<spGetWasteWeightDaily_Result> GetDailyWasteWeightGraph(int UserID)
        {
            var v = (from goi in context.spGetWasteWeightDaily(UserID)
                     select goi);
            return v.ToList<spGetWasteWeightDaily_Result>();
        }

        public List<GetGOIChart_Result> GetGOIGraph(int UserID)
        {
            //var v = (from rs in context.RecycleSubItems
            //         join r in context.Recycles on rs.RecycleID equals r.ID into mrs
            //         where rs.Recycle.UserID == UserID
            //         select new
            //         {
            //             RS = mrs.Where(d => d.CollectorDateTime != null),
            //         })
            //         .ToList()
            //         .Select(d => new
            //         {
            //                ID = d.RS.Sum(),

            //         });
            //        //.ToList()
            //        //.Select(d => new
            //        //{


            //        //});


            var v = (from goi in context.GetGOIChart(UserID)
                     select goi);

            return v.ToList<GetGOIChart_Result>();
        }

        public List<GetGreenPointsYearWise_Result> GetGOIGraphYearWise(int UserID)
        {

            var v = (from goi in context.GetGreenPointsYearWise(UserID)
                     select goi);

            return v.ToList<GetGreenPointsYearWise_Result>();
        }

        public List<GetGreenPointsMonthWise_Result> GetGOIGraphMonthWise(int UserID)
        {

            var v = (from goi in context.GetGreenPointsMonthWise(UserID)
                     select goi);

            return v.ToList<GetGreenPointsMonthWise_Result>();
        }
        public List<GetGOIGreenPoints_Result> GetGreenCreditsForGOI (int GOI1, int GOI2, int GOI3)
        {

            //var query = (from b in context.Businesses
            //            join e in context.Employments on b.ID equals e.BusId
            //            join u in context.Users on e.UserID equals u.ID
            //            where b.ID == GOI1 || b.ID == GOI2 || b.ID == GOI3
            //            select new
            //            {
            //                u.GreenPoints
            //            })

            var v = (from goi in context.GetGOIGreenPoints(GOI1, GOI2, GOI3)
                     select goi);

            return v.ToList<GetGOIGreenPoints_Result>();

        }



        public object GetUserCurrentMonthGP(int UserID)
        {
            DateTime Date = DateTime.Now;

            var FromDate = new DateTime(Date.Year, Date.Month, 1);
            var ToDate = FromDate.AddMonths(1).AddDays(-1);


            var TotalGP = (from user in context.Users
                           join re in context.Refuses on user.ID equals re.UserID into refuses
                           join rd in context.Reduces on user.ID equals rd.UserID into reduses
                           join ru in context.Reuses on user.ID equals ru.UserID into reuses
                           join rp in context.Replants on user.ID equals rp.UserID into replants
                           join rc in context.Recycles on user.ID equals rc.UserID into recycles
                           join rg in context.Regifts on user.ID equals rg.UserID into regifts
                           join rp in context.Reports on user.ID equals rp.UserID into reports
                           join bin in context.BuyBins on user.ID equals bin.UserID into bin
                           where user.ID == UserID
                           select new
                           {
                               Refuses = refuses.Where(x => x.UpdatedDate >= FromDate && x.UpdatedDate <= ToDate && x.StatusID == (int)StatusEnum.Resolved),
                               Reduces = reduses.Where(x => x.UpdatedDate >= FromDate && x.UpdatedDate <= ToDate && x.StatusID == (int)StatusEnum.Resolved),
                               Reuses = reuses.Where(x => x.UpdatedDate >= FromDate && x.UpdatedDate <= ToDate && x.StatusID == (int)StatusEnum.Resolved),
                               Replants = replants.Where(x => x.UpdatedDate >= FromDate && x.UpdatedDate <= ToDate && x.StatusID == (int)StatusEnum.Resolved),
                               Recycles = recycles.Where(x => x.UpdatedDate >= FromDate && x.UpdatedDate <= ToDate && x.StatusID == (int)StatusEnum.Complete),
                               Regifts = regifts.Where(x => x.UpdatedDate >= FromDate && x.UpdatedDate <= ToDate && x.StatusID == (int)StatusEnum.Delivered),
                               Reports = reports.Where(x => x.UpdatedDate >= FromDate && x.UpdatedDate <= ToDate && x.StatusID == (int)StatusEnum.Resolved),
                               BuyBins = bin.Where(x => x.UpdatedDate >= FromDate && x.UpdatedDate <= ToDate && x.StatusID == (int)StatusEnum.Delivered)
                           }).Select(u => new
                           {
                               Refuses = u.Refuses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                               Reduces = u.Reduces.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                               Reuses = u.Reuses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum() ?? 0,
                               Replants = u.Replants.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                               Recycles = u.Recycles.Select(g => g.RecycleSubItems).Select(x => x.Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum() ?? 0,
                               Regifts = u.Regifts.Select(g => g.RegiftSubItems).Select(x => x.Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum() ?? 0,
                               Reports = u.Reports.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                               Bins = u.BuyBins.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum() ?? 0,
                           }).Select(u => new
                           {
                               MonthlyGP = u.Refuses + u.Reduces + u.Reuses + u.Replants + u.Recycles + u.Regifts + u.Reports + u.Bins
                           }).FirstOrDefault();

            return TotalGP;
        }
        //public object GetUserCurrentMonthGP(int UserID)
        //{
        //    DateTime Date = DateTime.Now;

        //    var FromDate = new DateTime(Date.Year, Date.Month, 1);
        //    var ToDate = FromDate.AddMonths(1).AddDays(-1);


        //    var TotalGP = (from user in context.Users
        //                   join re in context.Refuses on user.ID equals re.UserID into refuses
        //                   join rd in context.Reduces on user.ID equals rd.UserID into reduses
        //                   join ru in context.Reuses on user.ID equals ru.UserID into reuses
        //                   join rp in context.Replants on user.ID equals rp.UserID into replants
        //                   join rc in context.Recycles on user.ID equals rc.UserID into recycles
        //                   join rg in context.Regifts on user.ID equals rg.UserID into regifts
        //                   join rp in context.Reports on user.ID equals rp.UserID into reports
        //                   join bin in context.BuyBins on user.ID equals bin.UserID into bin
        //                   where user.ID == UserID
        //                   select new
        //                   {
        //                       Refuses = refuses.Where(x => x.UpdatedDate >= FromDate && x.UpdatedDate <= ToDate && x.StatusID == (int)StatusEnum.Resolved),
        //                       Reduces = reduses.Where(x => x.UpdatedDate >= FromDate && x.UpdatedDate <= ToDate && x.StatusID == (int)StatusEnum.Resolved),
        //                       Reuses = reuses.Where(x => x.UpdatedDate >= FromDate && x.UpdatedDate <= ToDate && x.StatusID == (int)StatusEnum.Resolved),
        //                       Replants = replants.Where(x => x.UpdatedDate >= FromDate && x.UpdatedDate <= ToDate && x.StatusID == (int)StatusEnum.Resolved),
        //                       Recycles = recycles.Where(x => x.UpdatedDate >= FromDate && x.UpdatedDate <= ToDate && x.StatusID == (int)StatusEnum.Complete),
        //                       Regifts = regifts.Where(x => x.UpdatedDate >= FromDate && x.UpdatedDate <= ToDate && x.StatusID == (int)StatusEnum.Delivered),
        //                       Reports = reports.Where(x => x.UpdatedDate >= FromDate && x.UpdatedDate <= ToDate && x.StatusID == (int)StatusEnum.Resolved),
        //                       BuyBins = bin.Where(x => x.UpdatedDate >= FromDate && x.UpdatedDate <= ToDate && x.StatusID == (int)StatusEnum.Delivered)
        //                   }).Select(u => new
        //                   {
        //                       Refuses = u.Refuses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
        //                       Reduces = u.Reduces.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
        //                       Reuses = u.Reuses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum() ?? 0,
        //                       Replants = u.Replants.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
        //                       Recycles = u.Recycles.Select(g => g.RecycleSubItems).Select(x => x.Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum() ?? 0,
        //                       Regifts = u.Regifts.Select(g => g.RegiftSubItems).Select(x => x.Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum() ?? 0,
        //                       Reports = u.Reports.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
        //                       Bins = u.BuyBins.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum() ?? 0,
        //                       //Redeemed = user.Redeemed ? 0  : (decimal)user.Redeemed
        //                       //Redeemed = user.Redeemed ? 0 : (decimal)user.Redeemed
        //                   }).Select(u => new
        //                   {
        //                       MonthlyGP = u.Refuses + u.Reduces + u.Reuses + u.Replants + u.Recycles + u.Regifts + u.Reports + u.Bins
        //                   }).FirstOrDefault();

        //    return TotalGP;
        //}


        public int GetUserGreenPoints(int UserID)
        {
            object TotalGP = (from user in context.Users
                              join re in context.Refuses on user.ID equals re.UserID into refuses
                              join rd in context.Reduces on user.ID equals rd.UserID into reduses
                              join ru in context.Reuses on user.ID equals ru.UserID into reuses
                              join rp in context.Replants on user.ID equals rp.UserID into replants
                              join rc in context.Recycles on user.ID equals rc.UserID into recycles
                              join rg in context.Regifts on user.ID equals rg.UserID into regifts
                              join rp in context.Reports on user.ID equals rp.UserID into reports
                              join bin in context.BuyBins on user.ID equals bin.UserID into bin
                              where user.ID == UserID
                              select new
                              {
                                  Refuses = refuses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                  Reduces = reduses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                  Reuses = reuses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum() ?? 0,
                                  Replants = replants.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                  Recycles = recycles.Select(g => g.RecycleSubItems).Select(x => x.Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum() ?? 0,
                                  Regifts = regifts.Select(g => g.RegiftSubItems).Select(x => x.Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum() ?? 0,
                                  Reports = reports.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                  Bins = bin.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum() ?? 0,
                                  //Redeemed = user.Redeemed ? 0  : (decimal)user.Redeemed
                                  //Redeemed = user.Redeemed ? 0 : (decimal)user.Redeemed
                              }).Select(u => new
                              {
                                  TotalGP = u.Refuses + u.Reduces + u.Reuses + u.Replants + u.Recycles + u.Regifts + u.Reports + u.Bins
                              }).FirstOrDefault();
            int GP = 0;
            if (TotalGP != null)
            {
                GP = Convert.ToInt32(TotalGP.GetType().GetProperty("TotalGP").GetValue(TotalGP));

            }


            return GP;
        }

        public IEnumerable<object> GetUserList(UserRequestDto model)
        {
            bool IsVerified = true;

            if (model.Type != "registered")
                IsVerified = false;

            var users = (from user in context.Users
                         join city in context.Cities on user.CityId equals city.ID into city
                         join bb in context.BuyBins on user.ID equals bb.UserID into buybin
                         join re in context.Refuses on user.ID equals re.UserID into refuses
                         join rd in context.Reduces on user.ID equals rd.UserID into reduses
                         join rp in context.Replants on user.ID equals rp.UserID into replants
                         join ru in context.Reuses on user.ID equals ru.UserID into reuses
                         join rc in context.Recycles on user.ID equals rc.UserID into recycles
                         join rg in context.Regifts on user.ID equals rg.UserID into regifts
                         join rp in context.Reports on user.ID equals rp.UserID into reports

                         where ((user.RoleID != (int)UserRoleTypeEnum.Admin && user.RoleID != (int)UserRoleTypeEnum.NGO)
                                && user.IsVerified == IsVerified)
                         select new
                         {
                             FullName = user.FullName,
                             Email = user.Email,
                             Phone = user.Phone,
                             CityName=city.FirstOrDefault().CityName,
                             Latitude = user.Latitude,
                             Longitude = user.Longitude,
                             UserId = user.ID.ToString(),
                             Address = user.Address,
                             FileName = user.FileName,
                             IsVarified = user.IsVerified,
                             GreenPoints = user.WalletBalance,//recycles.FirstOrDefault().RecycleSubItems.FirstOrDefault().GreenPoints,
                             BinCount = buybin.Count(),
                             RecycleCount = recycles.Count(),
                             ReduceCount = reduses.Count(),
                             RefuseCount = refuses.Count(),
                             ReplantCount = replants.Count(),
                             ReportCount = reports.Count(),
                             ReuseCount = reuses.Count(),
                             RegiftCount = regifts.Count(),
                             CreatedDate = user.CreatedDate,
                             WalletBalance=user.WalletBalance,
                             PaymentStatus="Paid"

                             //   }).ToList().OrderByDescending(x => x.CreatedDate)
                         }).ToList()
                         .Select(u => new
                         { 
                             MemberSince = Convert.ToDateTime(u.CreatedDate).ToString("MMM dd, yyyy"),
                             FullName = u.FullName,
                             Email = u.Email,
                             Phone = u.Phone,
                             Latitude = u.Latitude,
                             Longitude = u.Longitude,
                             UserId = u.UserId,
                             Address = u.Address,
                             FileName = u.FileName,
                             UserType = u.IsVarified == true ? "Registered" : "Basic",
                             IsVarified = u.IsVarified == true,
                             GreenPoints = u.GreenPoints,
                             BinCount = u.BinCount,
                             RecycleCount = u.RecycleCount,
                             ReduceCount = u.ReduceCount,
                             RefuseCount = u.RefuseCount,
                             ReplantCount = u.ReplantCount,
                             ReportCount = u.ReportCount,
                             ReuseCount = u.ReuseCount,
                             RegiftCount = u.RegiftCount,
                             CityName=u.CityName,
                             CreatedDate = u.CreatedDate,
                             WalletBalance = u.WalletBalance,
                             PaymentStatus = "Paid",
u                         }).ToList();
            if (model.StartDate != null && model.EndDate != null)
            {
                return users.Where(x => x.CreatedDate >= Utility.GetDateFromString(model.StartDate) && x.CreatedDate <= Utility.GetDateFromString(model.EndDate)).OrderByDescending(x => x.CreatedDate).ToList();
                //response = mdlRefuses.Where(x => x.CreatedDate >= model.StartDate && x.CreatedDate <= model.EndDate).ToList<object>();
                //return response;
                // return mdlRecycles.Where(x => x.CreatedDate >= model.StartDate && x.CreatedDate <= model.EndDate).ToList();
            }
            else
            {
                return users.OrderByDescending(x => x.CreatedDate).ToList();
            }

           
        }
        public string getUserSchools(int id)
        {
            string strSchools = string.Empty;
            var result = context.Schools.Where(x => x.UserID == id).ToList();
            foreach (var item in result)
            {
                strSchools += item.Name + ",";
            }
            return strSchools;
        }
        public string getUserOrganizations(int id)
        {
            string strOrganiztions = string.Empty;
            var result = context.Organizations.Where(x => x.UserID == id).ToList();
            foreach (var item in result)
            {
                strOrganiztions += item.Name + ",";
            }
            return strOrganiztions;
        }
        public string getUserBusiness(int id)
        {
            string strBusiness = string.Empty;
            var result = context.Businesses.Where(x => x.UserID == id).ToList();
            foreach (var item in result)
            {
                strBusiness += item.Name + ",";
            }
            return strBusiness;
        }
        public  List<UserAssociation> UserAssocations(int UserId)
        {

            List<UserAssociation> lists = new List<UserAssociation>();

            List<Organization> lstOrg = (from mem in context.Members
                                         join org in context.Organizations on mem.OrgId equals org.ID
                                         where mem.UserID == UserId && mem.IsActive == true
                                         select org).Distinct().ToList();
            List<School> lstSchool = (from child in context.Children
                                      join sch in context.Schools on child.SchoolID equals sch.ID
                                      where child.UserID == UserId && child.IsActive == true
                                      select sch).Distinct().ToList();
            List<Business> lstBusiness = (from emp in context.Employments
                                          join businss in context.Businesses on emp.BusId equals businss.ID
                                          where emp.UserID == UserId && emp.IsActive == true
                                          select businss).Distinct().ToList();
            List<School> stafflstSchool = (from staff in context.SchoolStaffs
                                           join sch in context.Schools on staff.SchoolID equals sch.ID
                                           where staff.UserID == UserId && staff.IsActive == true
                                           select sch).Distinct().ToList();
            //lstSchool.AddRange(stafflstSchool);
            //lists.AddRange(lstOrg);
            //lists.AddRange(lstBusiness);
            //lists.AddRange(lstSchool);
            UserAssociation mdl = new UserAssociation();

            foreach (var item in lstOrg)
            {
                mdl = new UserAssociation();
                mdl.Name = item.Name;
                mdl.Type = "Organization";
                lists.Add(mdl);
            }

            foreach (var item1 in lstSchool)
            {
                mdl = new UserAssociation();
                mdl.Name = item1.Name;
                mdl.Type = "School";
                lists.Add(mdl);
            }

            foreach (var item2 in lstBusiness)
            {
                mdl = new UserAssociation();
                mdl.Name = item2.Name;
                mdl.Type = "Business";
                lists.Add(mdl);

            }

            foreach (var item3 in stafflstSchool)
            {
                mdl = new UserAssociation();
                mdl.Name = item3.Name;
                mdl.Type = "School";
                lists.Add(mdl);
            }
            lists.OrderBy(x => x.Type).ToList();

            //var SchoolsList = context.Schools.Where(x => x.UserID == UserId).ToList();
            //lists.AddRange(SchoolsList);
            //var OrgList = context.Organizations.Where(x => x.UserID == UserId).ToList();
            //lists.AddRange(OrgList);
            //var BusinssList = context.Businesses.Where(x => x.UserID == UserId).ToList();
            //lists.AddRange(BusinssList);
            return lists;

        }

        public object GetUserDetail(int id)
        {
            var user = (from usr in context.Users.ToList()
                        join bb in context.BuyBins on usr.ID equals bb.UserID into buybin
                        join re in context.Refuses on usr.ID equals re.UserID into refuses
                        join rd in context.Reduces on usr.ID equals rd.UserID into reduses
                        join rp in context.Replants on usr.ID equals rp.UserID into replants
                        join ru in context.Reuses on usr.ID equals ru.UserID into reuses
                        join rc in context.Recycles on usr.ID equals rc.UserID into recycles
                        join rg in context.Regifts on usr.ID equals rg.UserID into regifts
                        join rp in context.Reports on usr.ID equals rp.UserID into reports
                        //join business in context.Businesses on usr.ID equals business.UserID into businesss
                        //join school in context.Schools on usr.ID equals school.UserID into schools
                        //join organizaton in context.Organizations on usr.ID equals organizaton.UserID into organizatons
                        where usr.ID == id
                        select new
                        {
                            //new List<object> { schools.Select(y=> new {y.ID,y.Name }) }
                           // UserSchools = getUserSchools(id),
                           // UserOrganizations = getUserOrganizations(id),
                          //  UserBusiness = getUserBusiness(id),
                            TotalGP = GetUserGreenPointsTotal(id),
                            FullName = usr.FullName,
                            Email = usr.Email,
                            Phone = usr.Phone,
                            Latitude = usr.Latitude,
                            Longitude = usr.Longitude,
                            UserId = usr.ID.ToString(),
                            Address = usr.Address,
                            FileName = usr.FileName,
                            IsVarified = usr.IsVerified,
                            GreenPoints = usr.GreenPoints,
                            BinCount = buybin.Count(),
                            RecycleCount = recycles.Count(),
                            ReduceCount = reduses.Count(),
                            RefuseCount = refuses.Count(),
                            ReplantCount = replants.Count(),
                            ReportCount = reports.Count(),
                            ReuseCount = reuses.Count(),
                            RegiftCount = regifts.Count(),
                            CreatedDate = usr.CreatedDate,
                            

                        }).ToList()
                         .Select(u => new
                         {
                             MemberSince = Convert.ToDateTime(u.CreatedDate).ToString("MMM dd, yyyy"),
                             FullName = u.FullName,
                             Email = u.Email,
                             Phone = u.Phone,
                             Latitude = u.Latitude,
                             Longitude = u.Longitude,
                             UserId = u.UserId,
                             Address = u.Address,
                             FileName = u.FileName,
                             UserType = u.IsVarified == true ? "Registered" : "Basic",
                             GreenPoints = u.GreenPoints,
                             BinCount = u.BinCount,
                             RecycleCount = u.RecycleCount,
                             ReduceCount = u.ReduceCount,
                             RefuseCount = u.RefuseCount,
                             ReplantCount = u.ReplantCount,
                             ReportCount = u.ReportCount,
                             ReuseCount = u.ReuseCount,
                             RegiftCount = u.RegiftCount,
                             CreatedDate = u.CreatedDate,
                           //  UserSchools = u.UserSchools,
                         //    UserOrganizations = u.UserOrganizations,
                           //  UserBusiness = u.UserBusiness,
                             TotalGP = u.TotalGP
                         }).ToList()[0];

            return user;
        }

        public object GetUserDetaisForWeb(string Email, String Password)
        {
            object mdlUser = (from user in context.Users
                              join role in context.Roles on user.RoleID equals role.ID
                              where user.Email == Email && user.Password == Password
                              select new
                              {
                                  ID = user.ID,
                                  FullName = user.FullName,
                                  Address = user.Address,
                                  Email = user.Email,
                                  Phone = user.Phone,
                                  FileName = user.FileName,
                                  Longitude = Convert.ToDecimal(user.Longitude),
                                  Latitude = Convert.ToDecimal(user.Latitude),
                                  QRCode = user.QRCode,
                                  GreenPoints = Convert.ToInt32(user.GreenPoints),
                                  City = user.City,
                                  IsVerified = Convert.ToBoolean(user.IsVerified),
                                  DeviceToken = user.DeviceToken,
                                  Role = role.RoleName,
                                  RoleID = user.RoleID
                              });
            return mdlUser;
        }

        //public IEnumerable<object> GetContactPersonDetail(int id)
        //{
        //    var user = (from usr in context.Users
        //                where usr.ID == id
        //                select new
        //                {
        //                    FullName = usr.FullName,
        //                }).ToList();

        //    return user;
        //}

        public List<object> GetChildrenBySchoolID1(int SchoolID)
        {
            List<object> mdlChild = (from child in context.Children.ToList()
                                     where child.SchoolID == SchoolID
                                     select new
                                     {
                                         child.CreatedDate,
                                         child.FileName,
                                         child.ID,
                                         child.IsActive,
                                         child.IsVerified,
                                         child.Name,
                                         //child.School,
                                         child.SchoolID,
                                         child.SectionName,
                                         child.ClassName,
                                         child.RegistrationNo,
                                         //child.User.GreenPoints,
                                         GreenPoints = GetUserGreenPoints(child.UserID),
                                         child.UserID,
                                         child.Gender,
                                         Level = Utility.GetLevelByGP(GetUserGreenPoints(child.UserID)),
                                     })
                                     .ToList<object>();
            //foreach (var item in  mdlChild)
            //{

            //}

            return mdlChild;
        }

        public List<object> GetChildrenBySchoolID(int SchoolID)
        {

            List<object> mdlChild = (from child in context.Children.ToList()
                                     where child.SchoolID == SchoolID && child.IsActive ==true
                                     select new
                                     {
                                         child.FileName,
                                         child.Name,
                                         child.SchoolID,
                                         GreenPoints = GetUserGreenPoints(child.UserID),
                                         Level = Utility.GetLevelByGP(GetUserGreenPoints(child.UserID)),
                                         child.UserID,
                                         Type = "Student"
                                     }).Union((from staff in context.SchoolStaffs.ToList()
                                               where staff.SchoolID == SchoolID && staff.IsActive == true
                                               select new
                                               {
                                                   staff.FileName,
                                                   staff.Name,
                                                   staff.SchoolID,
                                                   GreenPoints = GetUserGreenPoints(staff.UserID),
                                                   Level = Utility.GetLevelByGP(GetUserGreenPoints(staff.UserID)),
                                                   staff.UserID,
                                                   Type = "Staff"
                                               }))
                                     .ToList<object>();        


            return mdlChild;
        }

        public List<object> GetMembersByOrgID(int OrgID)
        {
            List<object> mdlUser = (from member in context.Members.ToList()
                                    where member.OrgId == OrgID && member.IsActive ==true
                                    select new
                                    {
                                        member.CreatedBy,
                                        member.CreatedDate,
                                        member.Department,
                                        member.Designation,
                                        member.EmployeeID,
                                        member.ID,
                                        member.IsActive,
                                        member.IsCurrentlyWorking,
                                        member.IsVerified,
                                        member.Location,
                                        //member.Organization,
                                        member.OrgId,
                                        member.Name,
                                        member.Gender,
                                        member.FileName,
                                        //member.User.GreenPoints,
                                        // GreenPoints = GetUserGreenPoints(member.UserID),
                                        member.UserID
                                    }).ToList().Select(member => new
                                    {
                                        member.CreatedBy,
                                        member.CreatedDate,
                                        member.Department,
                                        member.Designation,
                                        member.EmployeeID,
                                        member.ID,
                                        member.IsActive,
                                        member.IsCurrentlyWorking,
                                        member.IsVerified,
                                        member.Location,
                                        //member.Organization,
                                        member.OrgId,
                                        member.Name,
                                        member.Gender,
                                        member.FileName,
                                        //member.User.GreenPoints,
                                        GreenPoints = GetUserGreenPoints(member.UserID),
                                        Level = Utility.GetLevelByGP(GetUserGreenPoints(member.UserID)),
                                        member.UserID
                                    })

                                    .Distinct().ToList<object>();
            return mdlUser;
        }

        public List<object> GetEmployeesByBusinessID(int OrgID)
        {
            List<object> mdlUser = (from des in context.Users.ToList()
                                    join emp in context.Employments on des.ID equals emp.UserID
                                    join Org in context.Businesses on emp.BusId equals Org.ID
                                    where Org.ID == OrgID && emp.IsActive ==true
                                    select new
                                    {
                                        des.Address,
                                        des.DeviceID,
                                        des.City,
                                        des.CityId,
                                        des.CreatedBy,
                                        des.CreatedDate,
                                        des.DeviceToken,
                                        des.Email,
                                        //des.Employments,
                                        des.FileName,
                                        Name = des.FullName,
                                        //des.GreenPoints,
                                        des.ID,
                                        des.IsActive,
                                        des.IsVerified,
                                        des.Latitude,
                                        des.Longitude,
                                        des.Password,
                                        des.Phone,
                                        UserID = des.ID
                                    }).ToList()
                                    .Select(des => new
                                    {
                                        des.Address,
                                        des.DeviceID,
                                        des.City,
                                        des.CityId,
                                        des.CreatedBy,
                                        des.CreatedDate,
                                        des.DeviceToken,
                                        des.Email,
                                        //des.Employments,
                                        des.FileName,
                                        des.Name,
                                        //des.GreenPoints,
                                        GreenPoints = GetUserGreenPoints(des.UserID),
                                        des.ID,
                                        des.IsActive,
                                        des.IsVerified,
                                        des.Latitude,
                                        des.Longitude,
                                        des.Password,
                                        des.Phone,
                                        des.UserID,
                                        Level = Utility.GetLevelByGP(GetUserGreenPoints(des.UserID)),
                                    }).ToList<object>();
            return mdlUser;
        }
    }
}
