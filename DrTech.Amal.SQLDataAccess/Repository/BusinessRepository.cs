using DrTech.Amal.Common.Enums;
using DrTech.Amal.SQLDatabase;
using DrTech.Amal.SQLModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrTech.Amal.Common.Helpers;
using System.Data.Entity.Core.Objects;
using Newtonsoft.Json;
using DrTech.Amal.SQLDataAccess.CustomModels;

namespace DrTech.Amal.SQLDataAccess.Repository
{
    public class BusinessRepository : Repository<Business>
    {
        public BusinessRepository(Amal_Entities context)
        : base(context)
        {
            dbSet = context.Set<Business>();
        }

        public List<object> GetEmployListByRole(int? UserID, bool IsSuspended, int? RoleID)
        {
            List<object> mdlEmploy = new List<object>();

            if (RoleID == (int)UserRoleTypeEnum.SubBusinessAdmin || RoleID == (int)UserRoleTypeEnum.Admin)
            {
                mdlEmploy = (from emp in context.Employments
                             join bs in context.Businesses on emp.BusId equals bs.ID
                             join user in context.Users on emp.UserID equals user.ID
                             join role in context.Roles on user.RoleID equals role.ID
                             join re in context.Refuses on user.ID equals re.UserID into refuses
                             join rd in context.Reduces on user.ID equals rd.UserID into reduses
                             join ru in context.Reuses on user.ID equals ru.UserID into reuses
                             join rp in context.Replants on user.ID equals rp.UserID into replants
                             join rc in context.Recycles on user.ID equals rc.UserID into recycles
                             join rg in context.Regifts on user.ID equals rg.UserID into regifts
                             join rp in context.Reports on user.ID equals rp.UserID into reports
                             join bin in context.BuyBins on user.ID equals bin.UserID into buybin
                             where ((bs.UserID == UserID && RoleID == (int)UserRoleTypeEnum.SubBusinessAdmin) || (RoleID == (int)UserRoleTypeEnum.Admin))
                                    && ((emp.IsActive != false && IsSuspended == false) || (emp.IsActive == false && IsSuspended == true))
                             select new
                             {
                                 id = emp.ID,
                                 name = user.FullName,
                                 businessname = bs.Name,
                                 filename = user.FileName,
                                 department = emp.Department,
                                 designation = emp.Designation,
                                 role = role.RoleName,
                                 suboffice = emp.SubOffice,
                                 contactno = user.Phone,
                                 Refuses = refuses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                 Reduces = reduses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                 Reuses = reuses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum() ?? 0,
                                 Replants = replants.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                 Recycles = recycles.Select(g => g.RecycleSubItems).Select(x => x.Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum() ?? 0,
                                 Regifts = regifts.Select(g => g.RegiftSubItems).Select(x => x.Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum() ?? 0,
                                 Reports = reports.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                 Bins = reports.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                 totalGP = 0
                             }).Select(u => new
                             {
                                 id = u.id,
                                 name = u.name,
                                 businessname = u.businessname,
                                 filename = u.filename,
                                 department = u.department,
                                 designation = u.designation,
                                 role = u.role,
                                 suboffice = u.suboffice,
                                 contactno = u.contactno,
                                 totalGP = u.Refuses + u.Reduces + u.Reuses + u.Replants + u.Recycles + u.Regifts + u.Reports + u.Bins
                             }).OrderByDescending(x => x.totalGP).ToList<object>();
            }
            else if (RoleID == (int)UserRoleTypeEnum.BusinessAdmin)
            {
                mdlEmploy = (from rbus in context.RegBusinesses
                             join bus in context.Businesses on rbus.ID equals bus.ParentId
                             join emp in context.Employments on bus.ID equals emp.BusId
                             join user in context.Users on emp.UserID equals user.ID
                             join role in context.Roles on user.RoleID equals role.ID
                             join re in context.Refuses on user.ID equals re.UserID into refuses
                             join rd in context.Reduces on user.ID equals rd.UserID into reduses
                             join ru in context.Reuses on user.ID equals ru.UserID into reuses
                             join rp in context.Replants on user.ID equals rp.UserID into replants
                             join rc in context.Recycles on user.ID equals rc.UserID into recycles
                             join rg in context.Regifts on user.ID equals rg.UserID into regifts
                             join rp in context.Reports on user.ID equals rp.UserID into reports
                             join bin in context.BuyBins on user.ID equals bin.UserID into buybin
                             where rbus.UserID == UserID
                                && ((emp.IsActive != false && IsSuspended == false) || (emp.IsActive == false && IsSuspended == true))
                             select new
                             {
                                 id = emp.ID,
                                 name = user.FullName,
                                 businessname = bus.Name,
                                 filename = user.FileName,
                                 department = emp.Department,
                                 designation = emp.Designation,
                                 role = role.RoleName,
                                 suboffice = emp.SubOffice,
                                 contactno = user.Phone,
                                 Refuses = refuses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                 Reduces = reduses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                 Reuses = reuses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum() ?? 0,
                                 Replants = replants.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                 Recycles = recycles.Select(g => g.RecycleSubItems).Select(x => x.Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum() ?? 0,
                                 Regifts = regifts.Select(g => g.RegiftSubItems).Select(x => x.Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum() ?? 0,
                                 Reports = reports.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                 Bins = reports.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                 totalGP = 0
                             }).Select(u => new
                             {
                                 id = u.id,
                                 name = u.name,
                                 businessname = u.businessname,
                                 filename = u.filename,
                                 department = u.department,
                                 designation = u.designation,
                                 role = u.role,
                                 suboffice = u.suboffice,
                                 contactno = u.contactno,
                                 totalGP = u.Refuses + u.Reduces + u.Reuses + u.Replants + u.Recycles + u.Regifts + u.Reports + u.Bins
                             }).OrderByDescending(x => x.totalGP).ToList<object>();
            }

            return mdlEmploy;
        }

        public List<object> GetEmployListByRoleWithEmployeeProgress(int? UserID, bool IsSuspended, int? RoleID)
        {
            List<object> mdlEmploy = new List<object>();

            if (RoleID == (int)UserRoleTypeEnum.SubBusinessAdmin || RoleID == (int)UserRoleTypeEnum.Admin)
            {
                mdlEmploy = (from emp in context.Employments.ToList()
                             join bs in context.Businesses on emp.BusId equals bs.ID
                             join user in context.Users on emp.UserID equals user.ID
                             join role in context.Roles on user.RoleID equals role.ID
                             join re in context.Refuses on user.ID equals re.UserID into refuses
                             join rd in context.Reduces on user.ID equals rd.UserID into reduses
                             join ru in context.Reuses on user.ID equals ru.UserID into reuses
                             join rp in context.Replants on user.ID equals rp.UserID into replants
                             join rc in context.Recycles on user.ID equals rc.UserID into recycles
                             join rg in context.Regifts on user.ID equals rg.UserID into regifts
                             join rp in context.Reports on user.ID equals rp.UserID into reports
                             join bin in context.BuyBins on user.ID equals bin.UserID into buybin
                             where ((bs.UserID == UserID && RoleID == (int)UserRoleTypeEnum.SubBusinessAdmin) || (RoleID == (int)UserRoleTypeEnum.Admin))
                                    && ((emp.IsActive != false && IsSuspended == false) || (emp.IsActive == false && IsSuspended == true))
                             select new
                             {
                                 id = emp.ID,
                                 name = user.FullName,
                                 businessname = bs.Name,
                                 filename = user.FileName,
                                 department = emp.Department,
                                 designation = emp.Designation,
                                 role = role.RoleName,
                                 suboffice = emp.SubOffice,
                                 contactno = user.Phone,
                                 Refuses = refuses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                 Reduces = reduses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                 Reuses = reuses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum() ?? 0,
                                 Replants = replants.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                 Recycles = recycles.Select(g => g.RecycleSubItems).Select(x => x.Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum() ?? 0,
                                 Regifts = regifts.Select(g => g.RegiftSubItems).Select(x => x.Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum() ?? 0,
                                 Reports = reports.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                 Bins = reports.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                 totalGP = 0
                             }).Select(u => new
                             {
                                 id = u.id,
                                 name = u.name,
                                 filename = u.filename,
                                 totalGP = u.Refuses + u.Reduces + u.Reuses + u.Replants + u.Recycles + u.Regifts + u.Reports + u.Bins,
                                 level = Utility.GetLevelByGP(u.Refuses + u.Reduces + u.Reuses + u.Replants + u.Recycles + u.Regifts + u.Reports + u.Bins)
                             }).OrderByDescending(x => x.totalGP).ToList<object>();
            }
            else if (RoleID == (int)UserRoleTypeEnum.BusinessAdmin)
            {
                mdlEmploy = (from rbus in context.RegBusinesses.ToList()
                             join bus in context.Businesses on rbus.ID equals bus.ParentId
                             join emp in context.Employments on bus.ID equals emp.BusId
                             join user in context.Users on emp.UserID equals user.ID
                             join role in context.Roles on user.RoleID equals role.ID
                             join re in context.Refuses on user.ID equals re.UserID into refuses
                             join rd in context.Reduces on user.ID equals rd.UserID into reduses
                             join ru in context.Reuses on user.ID equals ru.UserID into reuses
                             join rp in context.Replants on user.ID equals rp.UserID into replants
                             join rc in context.Recycles on user.ID equals rc.UserID into recycles
                             join rg in context.Regifts on user.ID equals rg.UserID into regifts
                             join rp in context.Reports on user.ID equals rp.UserID into reports
                             join bin in context.BuyBins on user.ID equals bin.UserID into buybin
                             where rbus.UserID == UserID
                                && ((emp.IsActive != false && IsSuspended == false) || (emp.IsActive == false && IsSuspended == true))
                             select new
                             {
                                 id = emp.ID,
                                 name = user.FullName,
                                 filename = user.FileName,
                                 Refuses = refuses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                 Reduces = reduses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                 Reuses = reuses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum() ?? 0,
                                 Replants = replants.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                 Recycles = recycles.Select(g => g.RecycleSubItems).Select(x => x.Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum() ?? 0,
                                 Regifts = regifts.Select(g => g.RegiftSubItems).Select(x => x.Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum() ?? 0,
                                 Reports = reports.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                 Bins = reports.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                 totalGP = 0
                             }).Select(u => new
                             {
                                 id = u.id,
                                 name = u.name,
                                 filename = u.filename,
                                 totalGP = u.Refuses + u.Reduces + u.Reuses + u.Replants + u.Recycles + u.Regifts + u.Reports + u.Bins,
                                 level = Utility.GetLevelByGP(u.Refuses + u.Reduces + u.Reuses + u.Replants + u.Recycles + u.Regifts + u.Reports + u.Bins)
                             }).OrderByDescending(x => x.totalGP).ToList<object>();
            }

            return mdlEmploy;
        }

        public List<Object> GetAllBusinessWithRegistrationStatus(int? UserID)
        {
            List<Object> mdlBusiness = (from reg in context.RegBusinesses
                                        join bus in context.Businesses on reg.ID equals bus.ParentId
                                        join emp in context.Employments on bus.ID equals emp.BusId into employees
                                        where (bus.IsVerified == true && bus.IsActive == true)
                                        select new
                                        {
                                            reg.ID,
                                            reg.Name,
                                            bus.CreatedDate,
                                            bus.FileName,
                                            empGW = bus.BusinessGP_Log.Where(x => x.IsActive != false && x.EmployeeID != null && x.Employment.IsActive != false && x.CreatedDate >= EntityFunctions.TruncateTime(x.Business.CreatedDate)).Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum(),
                                            empCount = employees.Where(x => x.UserID == UserID && x.IsActive == true).Count(),
                                        }).OrderBy(x => x.CreatedDate)
                                     .GroupBy(x => new { x.ID, x.Name }).ToList()
                                     .Select(s => new
                                     {
                                         ID = s.Key.ID,
                                         Name = s.Key.Name,
                                         FileName = s.Select(y => y.FileName).First(),
                                         greenWorth = s.Select(y => y.empGW).Sum(),
                                         empCount = s.Select(y => y.empCount).Sum(),
                                     })
                                     .ToList()
                                     .Select(z => new
                                     {
                                         z.ID,
                                         z.Name,
                                         z.FileName,
                                         z.greenWorth,
                                         Level = Utility.GetLevelByGP(z.greenWorth),
                                         IsAdded = (z.empCount > 0) ? true : false,
                                     }).ToList<object>();

            return mdlBusiness;
        }

        public List<Object> GetAllSubBusinessWithRegistrationStatus(int? UserID, int? BusinessID)
        {
            List<Object> mdlSchool = (from business in context.Businesses
                                          //join child in context.Children on school.ID equals child.SchoolID into st
                                          //from stype in st.DefaultIfEmpty()
                                      where business.ParentId == BusinessID || business.ParentId == null || business.ParentId == 0
                                      select business)
                                      .ToList()
                                      .Select(b => new
                                      {
                                          b.ID,
                                          b.Name,
                                          b.GreenPoints,
                                          b.Phone,
                                          b.Address,
                                          BusinessType = b.LookupType.Name,
                                          b.ContactPerson,
                                          b.FileName,
                                          b.CreatedBy,
                                          b.EmployeeGreenPoints,
                                          b.Level,
                                          b.OfficeName,
                                          b.UserID,
                                          b.IsMainBranch,
                                          b.Email,
                                          b.ParentId,
                                          IsAdded = CheckUserRegistrationInBusiness(UserID, b.ID)
                                      }).OrderBy(o => o.ParentId).ToList<object>();
            return mdlSchool;
        }

        public bool CheckUserRegistrationInBusiness(int? UserID, int? businessID)
        {
            int v = (from c in context.Employments
                     where c.UserID == UserID && c.BusId == businessID
                     select c).Count();
            if (v > 0)
                return true;
            return false;
        }

        public List<object> GetBusinessBranchesByID(int? UserID, int? ParentID)
        {
            List<Object> mdlBusiness = (from business in context.Businesses
                                        join emp in context.Employments on business.ID equals emp.BusId into employees
                                        where business.ParentId == ParentID && business.IsActive == true
                                        select new
                                        {
                                            business.ID,
                                            business.Name,
                                            business.Phone,
                                            business.Address,
                                            business.ParentId,
                                            business.FileName,
                                            business.ContactPerson,
                                            business.CreatedBy,
                                            business.UserID,
                                            business.OfficeName,
                                            business.Email,
                                            greenWorth = business.BusinessGP_Log.Where(x => x.IsActive != false && x.EmployeeID != null && x.Employment.IsActive != false && x.CreatedDate >= EntityFunctions.TruncateTime(x.Business.CreatedDate)).Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum(),
                                            empCount = employees.Where(x => x.UserID == UserID && x.IsActive == true).Count(),
                                        }).OrderBy(o => o.ParentId).ToList()
                                      .Select(z => new
                                      {
                                          z.ID,
                                          z.Name,
                                          z.Phone,
                                          z.Address,
                                          z.ParentId,
                                          z.FileName,
                                          z.ContactPerson,
                                          z.CreatedBy,
                                          z.UserID,
                                          BranchName = z.OfficeName,
                                          z.Email,
                                          z.greenWorth,
                                          Level = Utility.GetLevelByGP(z.greenWorth),
                                          IsAdded = z.empCount > 0 ? true : false,
                                      }).ToList<object>();
            return mdlBusiness;
        }

        public List<object> GetDepartmentsByRole(int? UserId, int? RoleId)
        {
            List<object> mdlDepartments = new List<object>();

            mdlDepartments = (from emp in context.Employments
                              join bus in context.Businesses on emp.BusId equals bus.ID
                              where (bus.UserID == UserId && RoleId == (int)UserRoleTypeEnum.SubBusinessAdmin && bus.IsActive != false && emp.IsActive != false)
                              select new
                              {
                                  department = emp.Department,
                              }).Distinct().ToList<object>();

            return mdlDepartments;
        }
        public List<object> GetBusinessComparison(int? BusinessId, string Departments, string Businesses, DateTime FromDate, DateTime ToDate, int? RoleId)
        {
            List<object> mdlList = new List<object>();

            string[] department = new string[0];
            int[] business = new int[0];

            if (!string.IsNullOrEmpty(Departments))
                department = Departments.Split(',');

            if (!string.IsNullOrEmpty(Businesses))
                business = Array.ConvertAll<string, int>(Businesses.Split(','), int.Parse);

            mdlList = (from emp in context.Employments
                       join bus in context.Businesses on emp.BusId equals bus.ID
                       join user in context.Users on emp.UserID equals user.ID
                       join re in context.Refuses on user.ID equals re.UserID into refuses
                       join rd in context.Reduces on user.ID equals rd.UserID into reduces
                       join ru in context.Reuses on user.ID equals ru.UserID into reuses
                       join rp in context.Replants on user.ID equals rp.UserID into replants
                       join rc in context.Recycles on user.ID equals rc.UserID into recycles
                       join rg in context.Regifts on user.ID equals rg.UserID into regifts
                       join rp in context.Reports on user.ID equals rp.UserID into reports
                       join bin in context.BuyBins on user.ID equals bin.UserID into buybin
                       where (((bus.ID == BusinessId && department.Contains(emp.Department) && RoleId == (int)UserRoleTypeEnum.SubBusinessAdmin)
                            || (business.Contains(bus.ID) && RoleId == (int)UserRoleTypeEnum.BusinessAdmin))
                            && emp.IsActive != false && bus.IsActive != false)
                       select new
                       {
                           UserId = emp.UserID,
                           Department = emp.Department,
                           BusinessId = bus.ID,
                           BusinessName = bus.OfficeName,
                           Refuses = refuses.Where(x => x.UpdatedDate >= FromDate && x.UpdatedDate <= ToDate && x.StatusID == (int)StatusEnum.Resolved)
                            .GroupBy(x => new { x.UpdatedDate.Year, x.UpdatedDate.Month })
                            .Select(y => new { y.Key, gp = y.Sum(g => g.GreenPoints) }),
                           Reduces = reduces.Where(x => x.UpdatedDate >= FromDate && x.UpdatedDate <= ToDate && x.StatusID == (int)StatusEnum.Resolved)
                            .GroupBy(x => new { x.UpdatedDate.Year, x.UpdatedDate.Month })
                            .Select(y => new { y.Key, gp = y.Sum(g => g.GreenPoints) }),
                           Reuses = reuses.Where(x => x.UpdatedDate >= FromDate && x.UpdatedDate <= ToDate && x.StatusID == (int)StatusEnum.Resolved)
                            .GroupBy(x => new { x.UpdatedDate.Year, x.UpdatedDate.Month })
                            .Select(y => new { y.Key, gp = y.Sum(g => g.GreenPoints) ?? 0 }),
                           Replants = replants.Where(x => x.UpdatedDate >= FromDate && x.UpdatedDate <= ToDate && x.StatusID == (int)StatusEnum.Resolved)
                            .GroupBy(x => new { x.UpdatedDate.Year, x.UpdatedDate.Month })
                            .Select(y => new { y.Key, gp = y.Sum(g => g.GreenPoints) }),

                           Recycles = recycles.Where(x => x.UpdatedDate >= FromDate && x.UpdatedDate <= ToDate && x.StatusID == (int)StatusEnum.Delivered)
                            .GroupBy(x => new { x.UpdatedDate.Year, x.UpdatedDate.Month })
                            .Select(y => new { y.Key, gp = y.Select(g => g.RecycleSubItems).Select(z => z.Select(x => x.GreenPoints).DefaultIfEmpty(0).Sum()).Sum() ?? 0 }),

                           Regifts = regifts.Where(x => x.UpdatedDate >= FromDate && x.UpdatedDate <= ToDate && x.StatusID == (int)StatusEnum.Delivered)
                            .GroupBy(x => new { x.UpdatedDate.Year, x.UpdatedDate.Month })
                            .Select(y => new { y.Key, gp = y.Select(g => g.RegiftSubItems).Select(z => z.Select(x => x.GreenPoints).DefaultIfEmpty(0).Sum()).Sum() ?? 0 }),

                           Reports = reports.Where(x => x.UpdatedDate >= FromDate && x.UpdatedDate <= ToDate && x.StatusID == (int)StatusEnum.Resolved)
                            .GroupBy(x => new { x.UpdatedDate.Year, x.UpdatedDate.Month })
                            .Select(y => new { y.Key, gp = y.Sum(g => g.GreenPoints) }),
                           BuyBins = buybin.Where(x => x.UpdatedDate >= FromDate && x.UpdatedDate <= ToDate && x.StatusID == (int)StatusEnum.Delivered)
                            .GroupBy(x => new { x.UpdatedDate.Year, x.UpdatedDate.Month })
                            .Select(y => new { y.Key, gp = y.Sum(g => g.GreenPoints) ?? 0 })
                       })
                       .ToList<object>();

            return mdlList;
        }

        public List<object> GetBranchesByBusinessAdmin(int? UserId)
        {
            List<object> mdlBranches = new List<object>();

            mdlBranches = (from rb in context.RegBusinesses
                           join bus in context.Businesses on rb.ID equals bus.ParentId
                           where (rb.UserID == UserId && bus.IsActive != false && rb.IsActive != false)
                           select new
                           {
                               businessId = bus.ID,
                               businessName = bus.OfficeName,
                           }).Distinct().ToList<object>();

            return mdlBranches;
        }

        public List<object> GetBusinessListForGOI(int? ParentID)
        {
            List<object> lstBranches = new List<object>();

            lstBranches = (from bus in context.Businesses
                           where (bus.ParentId == ParentID && bus.IsActive != false)
                           select new
                           {
                               bus.ID,
                               Name = bus.OfficeName
                           }).ToList<object>();

            return lstBranches;
        }
        public List<object> GetHeadOfficesOFBusinessForGOI()
        {
            List<object> lsBusinesses = new List<object>();

            lsBusinesses = (from Regbus in context.RegBusinesses
                            join bus in context.Businesses on Regbus.ID equals bus.ParentId
                            // join lookups in context.LookupTypes on bus.BusTypeID equals lookups.ID
                            join type in context.LookupTypes on bus.BusTypeID equals type.ID
                            where type.Name.ToLower() == Common.Enums.BusinessType.WWF.ToString().ToLower()
                            select new
                            {
                                Regbus.ID,
                                Name = Regbus.Name
                            }).OrderBy(x => x.Name).Distinct().ToList<object>();

            return lsBusinesses;
        }
       

        public List<RecycleDetailChartVM> GetDataForRecycleDetailChartByAdmin(int UserID)
        {

            var v = (from goi in context.GetDataForRecycleDetailChartByAdmin(UserID)
                     select goi);
            List<GetDataForRecycleDetailChartByAdmin_Result> newList = new List<GetDataForRecycleDetailChartByAdmin_Result>(v);
            List<RecycleDetailChartVM> myREsults = new List<RecycleDetailChartVM>();
            try
            {

                var xResult = newList.Select(p => new
                {
                    Month = p.MON

                }).Distinct().ToList();


                foreach (var item in xResult)
                {
                    RecycleDetailChartVM myREsult = new RecycleDetailChartVM();
                    myREsult.name = item.Month;
                    List<Records> recordsList = new List<Records>();
                    List<GetDataForRecycleDetailChartByAdmin_Result> InnerREsult = newList.Where(x => x.MON == item.Month).ToList();
                    foreach (var inneritem in InnerREsult)
                    {
                        Records records = new Records();
                        records.name = inneritem.name;
                        records.value = inneritem.wei;
                        myREsult.series.Add(records);
                    }
                    myREsults.Add(myREsult);

                }
            }
            catch (Exception ex)
            {
                //Log
            }
           // GetCircularChartData(UserID);
            return myREsults.ToList();
        }
        public List<object> GetCircularChartData(int UserID)
        {
            var v = (from goi in context.GetDataForRecycleDetailChartByAdmin(UserID)
                     select goi);

           // v.GroupBy(x=>x.name)
                List<object> result = v
    .GroupBy(l => l.name)
    .Select(cl => new 
    {
        name = cl.First().name,
       // Quantity = cl.Count().ToString(),
        value = cl.Sum(c => c.wei),
    }).ToList<object>();


            return result;
        }








        //public List<object> GetDataForRecycleDetailChartByAdmin()
        //{
        //    List<object> lstRecyle = new List<object>();

        //    lstRecyle = (from recycl in context.Recycles
        //                 join subRec in context.RecycleSubItems on recycl.ID equals subRec.RecycleID
        //                 join type in context.WasteTypes on subRec.WasteTypeID equals type.ID

        //               ///  group new { recycl.CollectorDateTime.Value.Month, subRec.Weight, type.Name } by type.Name into g
        //                 select new
        //                 {




        //                     //ID = g.Key,
        //                     //CollectionDate = g.Select(y => y.Month),
        //                     //TypeName = g.Select(y => y.Name)
        //                 }).ToList<object>();
        //                    //.Select(x=> new {



        //                    //}).ToList<object>();

        //    return lstRecyle;
        //}


    }
}
