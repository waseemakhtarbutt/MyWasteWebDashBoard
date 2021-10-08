using DrTech.Amal.Common.Enums;
using DrTech.Amal.SQLDatabase;
using DrTech.Amal.SQLModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DrTech.Amal.Common.Helpers;
using System.Data.Entity.Core.Objects;
using DrTech.Amal.SQLDataAccess.CustomModels;
using System.Dynamic;

namespace DrTech.Amal.SQLDataAccess.Repository
{
    public class SchoolRepository : Repository<School>
    {

        public ContextDB db;
        public SchoolRepository(Amal_Entities context)
        : base(context)
        {
            dbSet = context.Set<School>();
            db = new ContextDB();
        }

        public List<School> GetChildSchoolByUserID(int? UserID)
        {
            List<School> mdlSchool = (from des in context.Children
                                      join sch in context.Schools on des.SchoolID equals sch.ID
                                      where des.UserID == UserID
                                      select sch).Distinct().ToList();
            return mdlSchool;
        }

        public List<Object> GetAllSchoolsWithRegisteredChildren1(int? UserID)
        {
            List<Object> mdlSchool = (from reg in context.RegSchools.ToList()
                                      join sch in context.Schools on reg.ID equals sch.ParentID
                                      //join ch in context.Children on sch.ID equals ch.SchoolID into children
                                      //join ss in context.SchoolStaffs on sch.ID equals ss.SchoolID into staff
                                      //    where (sch.IsVerified == true && sch.IsActive == true)
                                      select new
                                      {
                                          reg.ID,
                                          reg.Name,
                                          sch.CreatedDate,
                                          sch.FileName,
                                          //childrenGW = children.GroupBy(x => x.UserID).Select(y => y.Select(z => z.User).Distinct().Select(z => z.SchoolGP_Log).Select(a => a.Select(b => b.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum(),
                                          //staffGW = staff.GroupBy(x => x.UserID).Select(y => y.Select(z => z.User).Distinct().Select(z => z.SchoolGP_Log).Select(a => a.Select(b => b.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum(),

                                          childrenGW = sch.SchoolGP_Log.Where(x => x.IsActive != false && x.ChildID != null && x.Child.IsActive != false && x.CreatedDate >= EntityFunctions.TruncateTime(x.Child.CreatedDate)).Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum(),
                                          staffGW = sch.SchoolGP_Log.Where(x => x.IsActive == false && x.StaffID != null && x.SchoolStaff.IsActive != false && x.CreatedDate >= EntityFunctions.TruncateTime(x.SchoolStaff.CreatedDate)).Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum(),
                                          // greenWorth = GetAllBranchesGreenPointWorth(reg.ID),
                                          // childrenCount = children.Where(x => x.UserID == UserID && x.IsActive == true).Count(),
                                          // staffCount = staff.Where(x => x.UserID == UserID && x.IsActive == true).Count()
                                      }).OrderBy(x => x.CreatedDate)
                                     .GroupBy(x => new { x.ID, x.Name }).ToList()
                                     .Select(s => new
                                     {
                                         ID = s.Key.ID,
                                         Name = s.Key.Name,
                                         FileName = s.Select(y => y.FileName).First(),
                                         greenWorth = s.Select(y => y.childrenGW).Sum() + s.Select(y => y.staffGW).Sum(),
                                         //   greenWorth = s.Select(y=>y.greenWorth).Sum(),
                                         //  childrenCount = s.Select(y => y.childrenCount).Sum(),
                                         //  staffCount = s.Select(y => y.staffCount).Sum()
                                     })
                                     .ToList()
                                     .Select(z => new
                                     {
                                         z.ID,
                                         z.Name,
                                         z.FileName,
                                         z.greenWorth,
                                         Level = Utility.GetLevelByGP(z.greenWorth),
                                         // IsAdded = (z.childrenCount + z.staffCount > 0) ? true : false,
                                     }).ToList<object>();

            return mdlSchool;
        }
        public List<Object> GetAllSchoolsWithRegisteredChildren2(int? UserID)
        {
            List<Object> mdlSchool = (from reg in context.RegSchools.ToList()
                                      join sch in context.Schools on reg.ID equals sch.ParentID
                                      join ch in context.Children on sch.ID equals ch.SchoolID into children
                                      join ss in context.SchoolStaffs on sch.ID equals ss.SchoolID into staff
                                      where (sch.IsVerified == true && sch.IsActive == true)
                                      select new
                                      {
                                          reg.ID,
                                          reg.Name,
                                          sch.CreatedDate,
                                          sch.FileName,
                                          //childrenGW = children.GroupBy(x => x.UserID).Select(y => y.Select(z => z.User).Distinct().Select(z => z.SchoolGP_Log).Select(a => a.Select(b => b.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum(),
                                          //staffGW = staff.GroupBy(x => x.UserID).Select(y => y.Select(z => z.User).Distinct().Select(z => z.SchoolGP_Log).Select(a => a.Select(b => b.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum(),

                                          // childrenGW = sch.SchoolGP_Log.Where(x => x.IsActive != false && x.ChildID != null && x.Child.IsActive != false && x.CreatedDate >= EntityFunctions.TruncateTime(x.Child.CreatedDate)).Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum(),
                                          //  staffGW = sch.SchoolGP_Log.Where(x => x.IsActive == false && x.StaffID != null && x.SchoolStaff.IsActive != false && x.CreatedDate >= EntityFunctions.TruncateTime(x.SchoolStaff.CreatedDate)).Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum(),
                                          greenWorth = GetAllBranchesGreenPointWorth(reg.ID),
                                          childrenCount = children.Where(x => x.UserID == UserID && x.IsActive == true).Count(),
                                          staffCount = staff.Where(x => x.UserID == UserID && x.IsActive == true).Count()
                                      }).OrderBy(x => x.CreatedDate)
                                     .GroupBy(x => new { x.ID, x.Name }).ToList()
                                     .Select(s => new
                                     {
                                         ID = s.Key.ID,
                                         Name = s.Key.Name,
                                         FileName = s.Select(y => y.FileName).First(),
                                         //  greenWorth = s.Select(y => y.childrenGW).Sum() + s.Select(y => y.staffGW).Sum(),
                                         greenWorth = s.Select(y => y.greenWorth).Sum(),
                                         childrenCount = s.Select(y => y.childrenCount).Sum(),
                                         staffCount = s.Select(y => y.staffCount).Sum()
                                     })
                                     .ToList()
                                     .Select(z => new
                                     {
                                         z.ID,
                                         z.Name,
                                         z.FileName,
                                         z.greenWorth,
                                         Level = Utility.GetLevelByGP(z.greenWorth),
                                         IsAdded = (z.childrenCount + z.staffCount > 0) ? true : false,
                                     }).ToList<object>();

            return mdlSchool;
        }
        public List<object> GetAllSchoolsWithRegisteredChildren(int? UserID)
        {
            List<Object> mdlSchool = (from reg in context.RegSchools.ToList()
                                      join sch in context.Schools.ToList() on reg.ID equals sch.ParentID
                                      join ch in context.Children on sch.ID equals ch.SchoolID into children
                                      join ss in context.SchoolStaffs on sch.ID equals ss.SchoolID into staff
                                      where (sch.IsVerified == true && sch.IsActive == true)
                                      select new
                                      {
                                          reg.ID,
                                          reg.Name,
                                          sch.CreatedDate,
                                          sch.FileName,
                                          greenWorth = 0,// GetAllBranchesGreenPointWorth(reg.ID),
                                          childrenCount = children.Where(x => x.UserID == UserID && x.IsActive == true).Count(),
                                          staffCount = staff.Where(x => x.UserID == UserID && x.IsActive == true).Count()

                                      }).GroupBy(x => x.Name).ToList()
                                     .Select(z => new
                                     {
                                         z.FirstOrDefault().ID,
                                         z.FirstOrDefault().Name,
                                         z.FirstOrDefault().FileName,
                                         z.FirstOrDefault().greenWorth,
                                         Level = Utility.GetLevelByGP(z.FirstOrDefault().greenWorth),
                                         IsAdded = (z.FirstOrDefault().childrenCount + z.FirstOrDefault().staffCount > 0) ? true : false,
                                     }).ToList<object>();
            return mdlSchool;
        }
        public int GetSchoolGreenPoints(int SchoolID)
        {
            int total = 0;
            var allLoggedSchools = context.SchoolGP_Log.Where(x => x.SchoolID == SchoolID).ToList();
            if (allLoggedSchools.Count > 0)
            {
                total = allLoggedSchools.Sum(item => item.GreenPoints);
            }
            else
            {
                return 0;
            }
            return total;


        }
        public int GetAllBranchesGreenPointWorth(int? ParentID)
        {
            int SchoolGreenWorthTotal = 0;
            var allBranches = context.Schools.Where(x => x.ParentID == ParentID).ToList();
            if (allBranches.Count > 0)
            {
                foreach (var item in allBranches)
                {
                    SchoolGreenWorthTotal += GetSchoolGreenPoints(item.ID);
                }

                //foreach (var item in allBranches)
                //{

                //    var allLoggedSchools = context.SchoolGP_Log.Where(x => x.SchoolID == item.ID).ToList();
                //    if(allLoggedSchools.Count > 0)
                //    {
                //        int total = 0;
                //        foreach (var log in allLoggedSchools)
                //        {
                //            total += log.GreenPoints;
                //        }

                //        //total += allLoggedSchools.Sum(pm => pm.GreenPoints);
                //        SchoolGreenWorthTotal += total;
                //    }

                //}
            }
            return SchoolGreenWorthTotal;
        }

        public List<Object> GetAllSchoolBranchesWithRegistrationStatus(int? UserID, int? ParentID)
        {
            List<Object> mdlSchool = (from school in context.Schools.ToList()
                                      join ch in context.Children on school.ID equals ch.SchoolID into children
                                      join ss in context.SchoolStaffs on school.ID equals ss.SchoolID into staff
                                      where school.ParentID == ParentID && school.IsActive == true
                                      select new
                                      {
                                          school.ID,
                                          school.Name,
                                          school.Phone,
                                          school.Address,
                                          school.ParentID,
                                          school.FileName,
                                          school.ContactPerson,
                                          school.ContactPersonPhone,
                                          school.CreatedBy,
                                          school.UserID,
                                          school.BranchName,
                                          school.Email,
                                          school.RegFormat,
                                          //childrenGW = children.GroupBy(x => x.UserID).Select(y => y.Select(z => z.User).Distinct().Select(z => z.SchoolGP_Log).Select(a => a.Select(b => b.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum(),
                                          //staffGW = staff.GroupBy(x => x.UserID).Select(y => y.Select(z => z.User).Distinct().Select(z => z.SchoolGP_Log).Select(a => a.Select(b => b.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum(),

                                          //////childrenGW = school.SchoolGP_Log.Where(x=>x.IsActive != false && x.ChildID != null && x.Child.IsActive != false && x.CreatedDate >= EntityFunctions.TruncateTime(x.Child.CreatedDate)).Select(y=>y.GreenPoints).DefaultIfEmpty(0).Sum(),
                                          //////staffGW = school.SchoolGP_Log.Where(x => x.IsActive != false && x.StaffID != null && x.SchoolStaff.IsActive != false && x.CreatedDate >= EntityFunctions.TruncateTime(x.SchoolStaff.CreatedDate)).Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum(),
                                          SchoolGW = GetSchoolGreenPoints(school.ID),
                                          childrenCount = children.Where(x => x.UserID == UserID && x.IsActive == true).Count(),
                                          staffCount = staff.Where(x => x.UserID == UserID && x.IsActive == true).Count()
                                      }).OrderBy(o => o.ParentID).ToList()
                                      .Select(z => new
                                      {
                                          z.ID,
                                          z.Name,
                                          z.Phone,
                                          z.Address,
                                          z.ParentID,
                                          z.FileName,
                                          z.ContactPerson,
                                          z.ContactPersonPhone,
                                          z.CreatedBy,
                                          z.UserID,
                                          z.BranchName,
                                          z.Email,
                                          z.RegFormat,
                                          greenWorth = z.SchoolGW,
                                          Level = Utility.GetLevelByGP(z.SchoolGW),
                                          IsAdded = z.childrenCount + z.staffCount > 0 ? true : false,
                                      }).ToList<object>();
            return mdlSchool;
        }

        public bool CheckChildInSchool(int? UserID, int? SchoolID)
        {
            int v = (from c in context.Children
                     where c.UserID == UserID && c.SchoolID == SchoolID && c.IsActive != false
                     select c).Count();
            if (v > 0)
                return true;
            return false;
        }

        public int CheckChildrenCountByUser(int? UserID)
        {
            int v = (from user in context.Users
                     where user.ID == UserID
                     select new
                     {
                         childrenCount = user.Children.Count
                     }).Count();

            return v;
        }

        public List<object> GetSchoolBranchesByID(int ParentID)
        {
            List<object> lstSchool = (from regsch in context.RegSchools
                                      join school in context.Schools on regsch.ID equals school.ParentID
                                      join schoolGW in context.SchoolGP_Log on school.ID equals schoolGW.SchoolID into schoolGreenWorth
                                      where regsch.ID == ParentID && school.IsActive == true
                                      select new
                                      {

                                          school.Address,
                                          school.BranchName,
                                          school.ID,
                                          greenWorth = schoolGreenWorth.Select(gw => gw.GreenPoints).DefaultIfEmpty(0).Sum(),
                                          school.Level,
                                      }).ToList().Select(u => new
                                      {
                                          Level = Utility.GetLevelByGP(u.greenWorth),
                                          u.Address,
                                          u.BranchName,
                                          u.ID,
                                          u.greenWorth
                                      }).ToList<object>();
            //List<School> lstSchool = (from des in context.Schools
            //                          where des.ParentID == ParentID || des.ID == ParentID && des.IsActive == true
            //                          select des).ToList();


            return lstSchool;
        }

        public List<object> GetStudentListByRole(int? UserID, bool IsSuspended, int? RoleID)
        {
            List<object> mdlChildren = new List<object>();

            if (RoleID == (int)UserRoleTypeEnum.SubSchoolAdmin || RoleID == (int)UserRoleTypeEnum.Admin)
            {
                mdlChildren = (from ch in context.Children
                               join sch in context.Schools on ch.SchoolID equals sch.ID
                               join user in context.Users on ch.UserID equals user.ID
                               join re in context.Refuses on user.ID equals re.UserID into refuses
                               join rd in context.Reduces on user.ID equals rd.UserID into reduses
                               join ru in context.Reuses on user.ID equals ru.UserID into reuses
                               join rp in context.Replants on user.ID equals rp.UserID into replants
                               join rc in context.Recycles on user.ID equals rc.UserID into recycles
                               join rg in context.Regifts on user.ID equals rg.UserID into regifts
                               join rp in context.Reports on user.ID equals rp.UserID into reports
                               join bin in context.BuyBins on user.ID equals bin.UserID into buybin
                               where ((sch.UserID == UserID && RoleID == (int)UserRoleTypeEnum.SubSchoolAdmin) || (RoleID == (int)UserRoleTypeEnum.Admin))
                                && ((ch.IsActive != false && IsSuspended == false) || (ch.IsActive == false && IsSuspended == true))
                               //&& ch.IsActive != false
                               select new
                               {
                                   id = ch.ID,
                                   name = ch.Name,
                                   schoolname = sch.Name,
                                   filename = ch.FileName,
                                   clas = ch.ClassName,
                                   section = ch.SectionName,
                                   rollno = ch.RegistrationNo,
                                   contactno = user.Phone,
                                   Refuses = refuses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                   Reduces = reduses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                   Reuses = reuses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum() ?? 0,
                                   Replants = replants.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                   Recycles = recycles.Select(g => g.RecycleSubItems).Select(x => x.Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum() ?? 0,
                                   Regifts = regifts.Select(g => g.RegiftSubItems).Select(x => x.Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum() ?? 0,
                                   Reports = reports.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                   Bins = buybin.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum() ?? 0,
                               })
                               .Select(g => new
                               {
                                   id = g.id,
                                   name = g.name,
                                   schoolname = g.schoolname,
                                   filename = g.filename,
                                   clas = g.clas,
                                   section = g.section,
                                   rollno = g.rollno,
                                   contactno = g.contactno,
                                   totalGP = g.Refuses + g.Reduces + g.Reuses + g.Replants + g.Recycles + g.Regifts + g.Reports + g.Bins
                               }).OrderByDescending(x => x.totalGP).ToList<object>();
            }
            else if (RoleID == (int)UserRoleTypeEnum.SchoolAdmin)
            {
                mdlChildren = (from rs in context.RegSchools
                               join sch in context.Schools on rs.ID equals sch.ParentID
                               join ch in context.Children on sch.ID equals ch.SchoolID
                               join user in context.Users on ch.UserID equals user.ID
                               join re in context.Refuses on user.ID equals re.UserID into refuses
                               join rd in context.Reduces on user.ID equals rd.UserID into reduses
                               join ru in context.Reuses on user.ID equals ru.UserID into reuses
                               join rp in context.Replants on user.ID equals rp.UserID into replants
                               join rc in context.Recycles on user.ID equals rc.UserID into recycles
                               join rg in context.Regifts on user.ID equals rg.UserID into regifts
                               join rp in context.Reports on user.ID equals rp.UserID into reports
                               join bin in context.BuyBins on user.ID equals bin.UserID into buybin
                               where rs.UserID == UserID
                               && ((ch.IsActive != false && IsSuspended == false) || (ch.IsActive == false && IsSuspended == true))
                               //&& ch.IsActive != false
                               select new
                               {
                                   id = ch.ID,
                                   name = ch.Name,
                                   schoolname = sch.Name,
                                   filename = ch.FileName,
                                   clas = ch.ClassName,
                                   section = ch.SectionName,
                                   rollno = ch.RegistrationNo,
                                   contactno = user.Phone,
                                   Refuses = refuses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                   Reduces = reduses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                   Reuses = reuses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum() ?? 0,
                                   Replants = replants.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                   Recycles = recycles.Select(g => g.RecycleSubItems).Select(x => x.Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum() ?? 0,
                                   Regifts = regifts.Select(g => g.RegiftSubItems).Select(x => x.Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum() ?? 0,
                                   Reports = reports.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                   Bins = buybin.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum() ?? 0,
                                   totalGP = 0
                               })
                               .Select(g => new
                               {
                                   id = g.id,
                                   name = g.name,
                                   schoolname = g.schoolname,
                                   filename = g.filename,
                                   clas = g.clas,
                                   section = g.section,
                                   rollno = g.rollno,
                                   contactno = g.contactno,
                                   totalGP = g.Refuses + g.Reduces + g.Reuses + g.Replants + g.Recycles + g.Regifts + g.Reports + g.Bins
                               }).OrderByDescending(x => x.totalGP).ToList<object>();
            }

            return mdlChildren;
        }

        public List<object> GetStudentListByRoleWithPointsProgress(int? UserID, bool IsSuspended, int? RoleID)
        {
            List<object> mdlChildren = new List<object>();

            if (RoleID == (int)UserRoleTypeEnum.SubSchoolAdmin || RoleID == (int)UserRoleTypeEnum.Admin)
            {
                mdlChildren = (from ch in context.Children.ToList()
                               join sch in context.Schools on ch.SchoolID equals sch.ID
                               join user in context.Users on ch.UserID equals user.ID
                               join re in context.Refuses on user.ID equals re.UserID into refuses
                               join rd in context.Reduces on user.ID equals rd.UserID into reduses
                               join ru in context.Reuses on user.ID equals ru.UserID into reuses
                               join rp in context.Replants on user.ID equals rp.UserID into replants
                               join rc in context.Recycles on user.ID equals rc.UserID into recycles
                               join rg in context.Regifts on user.ID equals rg.UserID into regifts
                               join rp in context.Reports on user.ID equals rp.UserID into reports
                               join bin in context.BuyBins on user.ID equals bin.UserID into buybin
                               where ((sch.UserID == UserID && RoleID == (int)UserRoleTypeEnum.SubSchoolAdmin) || (RoleID == (int)UserRoleTypeEnum.Admin))
                                && ((ch.IsActive != false && IsSuspended == false) || (ch.IsActive == false && IsSuspended == true))
                               //&& ch.IsActive != false
                               select new
                               {
                                   id = ch.ID,
                                   name = ch.Name,
                                   filename = ch.FileName,
                                   // schoolname = sch.Name,                                 
                                   //clas = ch.ClassName,
                                   //section = ch.SectionName,
                                   //rollno = ch.RegistrationNo,
                                   //contactno = user.Phone,
                                   Refuses = refuses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                   Reduces = reduses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                   Reuses = reuses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum() ?? 0,
                                   Replants = replants.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                   RecyclesWeight = recycles.Select(g => g.RecycleSubItems).Select(x => x.Select(y => y.Weight).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum() ?? 0,
                                   Recycles = recycles.Select(g => g.RecycleSubItems).Select(x => x.Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum() ?? 0,
                                   Regifts = regifts.Select(g => g.RegiftSubItems).Select(x => x.Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum() ?? 0,
                                   Reports = reports.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                   Bins = buybin.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum() ?? 0,
                               })
                               .Select(g => new
                               {
                                   id = g.id,
                                   name = g.name,
                                   filename = g.filename,
                                   // schoolname = g.schoolname,                                 
                                   // clas = g.clas,
                                   // section = g.section,
                                   //  rollno = g.rollno,
                                   // contactno = g.contactno,
                                   Weight = g.RecyclesWeight,
                                   totalGP = g.Refuses + g.Reduces + g.Reuses + g.Replants + g.Recycles + g.Regifts + g.Reports + g.Bins,
                                   level = Utility.GetLevelByGP(g.Refuses + g.Reduces + g.Reuses + g.Replants + g.Recycles + g.Regifts + g.Reports + g.Bins)
                               }).OrderByDescending(x => x.totalGP).ToList<object>();
            }
            else if (RoleID == (int)UserRoleTypeEnum.SchoolAdmin)
            {
                mdlChildren = (from rs in context.RegSchools.ToList()
                               join sch in context.Schools on rs.ID equals sch.ParentID
                               join ch in context.Children on sch.ID equals ch.SchoolID
                               join user in context.Users on ch.UserID equals user.ID
                               join re in context.Refuses on user.ID equals re.UserID into refuses
                               join rd in context.Reduces on user.ID equals rd.UserID into reduses
                               join ru in context.Reuses on user.ID equals ru.UserID into reuses
                               join rp in context.Replants on user.ID equals rp.UserID into replants
                               join rc in context.Recycles on user.ID equals rc.UserID into recycles
                               join rg in context.Regifts on user.ID equals rg.UserID into regifts
                               join rp in context.Reports on user.ID equals rp.UserID into reports
                               join bin in context.BuyBins on user.ID equals bin.UserID into buybin
                               where rs.UserID == UserID
                               && ((ch.IsActive != false && IsSuspended == false) || (ch.IsActive == false && IsSuspended == true))
                               //&& ch.IsActive != false
                               select new
                               {
                                   id = ch.ID,
                                   name = ch.Name,
                                   filename = ch.FileName,
                                   //schoolname = sch.Name,
                                   //clas = ch.ClassName,
                                   //section = ch.SectionName,
                                   //rollno = ch.RegistrationNo,
                                   //contactno = user.Phone,
                                   Refuses = refuses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                   Reduces = reduses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                   Reuses = reuses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum() ?? 0,
                                   Replants = replants.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                   Recycles = recycles.Select(g => g.RecycleSubItems).Select(x => x.Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum() ?? 0,
                                   Regifts = regifts.Select(g => g.RegiftSubItems).Select(x => x.Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum() ?? 0,
                                   Reports = reports.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                   Bins = buybin.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum() ?? 0,
                                   totalGP = 0
                               })
                               .Select(g => new
                               {
                                   id = g.id,
                                   name = g.name,
                                   //schoolname = g.schoolname,
                                   filename = g.filename,
                                   //clas = g.clas,
                                   //section = g.section,
                                   //rollno = g.rollno,
                                   //contactno = g.contactno,
                                   totalGP = g.Refuses + g.Reduces + g.Reuses + g.Replants + g.Recycles + g.Regifts + g.Reports + g.Bins,
                                   level = Utility.GetLevelByGP(g.Refuses + g.Reduces + g.Reuses + g.Replants + g.Recycles + g.Regifts + g.Reports + g.Bins)
                               }).OrderByDescending(x => x.totalGP).ToList<object>();
            }

            return mdlChildren;
        }

        public List<object> GetStaffListByRole(int? UserID, bool IsSuspended, int? RoleID)
        {
            List<object> mdlStaff = new List<object>();


            if (RoleID == (int)UserRoleTypeEnum.SubSchoolAdmin || RoleID == (int)UserRoleTypeEnum.Admin)
            {
                mdlStaff = (from ss in context.SchoolStaffs
                            join sch in context.Schools on ss.SchoolID equals sch.ID
                            join user in context.Users on ss.UserID equals user.ID
                            join re in context.Refuses on user.ID equals re.UserID into refuses
                            join rd in context.Reduces on user.ID equals rd.UserID into reduses
                            join ru in context.Reuses on user.ID equals ru.UserID into reuses
                            join rp in context.Replants on user.ID equals rp.UserID into replants
                            join rc in context.Recycles on user.ID equals rc.UserID into recycles
                            join rg in context.Regifts on user.ID equals rg.UserID into regifts
                            join rp in context.Reports on user.ID equals rp.UserID into reports
                            join bin in context.BuyBins on user.ID equals bin.UserID into buybin
                            where ((sch.UserID == UserID && RoleID == (int)UserRoleTypeEnum.SubSchoolAdmin) || (RoleID == (int)UserRoleTypeEnum.Admin))
                             && ((ss.IsActive != false && IsSuspended == false) || (ss.IsActive == false && IsSuspended == true))
                            //&& ss.IsActive != false
                            select new
                            {
                                id = ss.ID,
                                name = ss.Name,
                                schoolname = sch.Name,
                                filename = ss.FileName,
                                designation = ss.Designation,
                                department = ss.Department,
                                employid = ss.EmployeeID,
                                contactno = user.Phone,
                                Refuses = refuses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                Reduces = reduses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                Reuses = reuses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum() ?? 0,
                                Replants = replants.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                Recycles = recycles.Select(g => g.RecycleSubItems).Select(x => x.Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum() ?? 0,
                                Regifts = regifts.Select(g => g.RegiftSubItems).Select(x => x.Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum() ?? 0,
                                Reports = reports.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                Bins = buybin.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum() ?? 0,
                                totalGP = 0
                            })
                            .Select(g => new
                            {
                                id = g.id,
                                name = g.name,
                                schoolname = g.schoolname,
                                filename = g.filename,
                                designation = g.designation,
                                department = g.department,
                                employid = g.employid,
                                contactno = g.contactno,
                                totalGP = g.Refuses + g.Reduces + g.Reuses + g.Replants + g.Recycles + g.Regifts + g.Reports + g.Bins
                            }).OrderByDescending(x => x.totalGP).ToList<object>();
            }
            else if (RoleID == (int)UserRoleTypeEnum.SchoolAdmin)
            {
                mdlStaff = (from rs in context.RegSchools
                            join sch in context.Schools on rs.ID equals sch.ParentID
                            join ss in context.SchoolStaffs on sch.ID equals ss.SchoolID
                            join user in context.Users on ss.UserID equals user.ID
                            join re in context.Refuses on user.ID equals re.UserID into refuses
                            join rd in context.Reduces on user.ID equals rd.UserID into reduses
                            join ru in context.Reuses on user.ID equals ru.UserID into reuses
                            join rp in context.Replants on user.ID equals rp.UserID into replants
                            join rc in context.Recycles on user.ID equals rc.UserID into recycles
                            join rg in context.Regifts on user.ID equals rg.UserID into regifts
                            join rp in context.Reports on user.ID equals rp.UserID into reports
                            join bin in context.BuyBins on user.ID equals bin.UserID into buybin
                            where rs.UserID == UserID
                             && ((ss.IsActive != false && IsSuspended == false) || (ss.IsActive == false && IsSuspended == true))
                            //&& ss.IsActive != false
                            select new
                            {
                                id = ss.ID,
                                name = ss.Name,
                                schoolname = sch.Name,
                                filename = ss.FileName,
                                designation = ss.Designation,
                                department = ss.Department,
                                employid = ss.EmployeeID,
                                contactno = user.Phone,
                                Refuses = refuses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                Reduces = reduses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                Reuses = reuses.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum() ?? 0,
                                Replants = replants.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                Recycles = recycles.Select(g => g.RecycleSubItems).Select(x => x.Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum() ?? 0,
                                Regifts = regifts.Select(g => g.RegiftSubItems).Select(x => x.Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum()).DefaultIfEmpty(0).Sum() ?? 0,
                                Reports = reports.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),
                                Bins = buybin.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum() ?? 0,
                                totalGP = 0
                            })
                            .Select(g => new
                            {
                                id = g.id,
                                name = g.name,
                                schoolname = g.schoolname,
                                filename = g.filename,
                                designation = g.designation,
                                department = g.department,
                                employid = g.employid,
                                contactno = g.contactno,
                                totalGP = g.Refuses + g.Reduces + g.Reuses + g.Replants + g.Recycles + g.Regifts + g.Reports + g.Bins
                            }).OrderByDescending(x => x.totalGP).ToList<object>();
            }

            return mdlStaff;
        }
        public List<object> GetStudentStaffRsByRole(int? UserID, int? RoleID, string type)
        {
            switch (type)
            {
                case "refuse":
                    if (RoleID == (int)UserRoleTypeEnum.SubSchoolAdmin)
                    {
                        List<object> lstRefuse = (from ch in context.Children
                                                  join sch in context.Schools on ch.SchoolID equals sch.ID
                                                  join user in context.Users on ch.UserID equals user.ID
                                                  join re in context.Refuses on user.ID equals re.UserID
                                                  where sch.UserID == UserID && ch.IsActive != false
                                                  select new
                                                  {
                                                      id = ch.ID,
                                                      name = ch.Name,

                                                      pinimage = "refuse.png",
                                                      latitude = re.Latitude,
                                                      longitude = re.Longitude,
                                                      type = "refuse",
                                                      label = "refuse",
                                                      cash = 0,
                                                      greenpoints = re.GreenPoints,
                                                      filename = re.FileName,
                                                      description = re.Idea
                                                  }).ToList<object>();

                        return lstRefuse;
                    }
                    else //if (RoleID == (int)UserRoleTypeEnum.SchoolAdmin)
                    {
                        List<object> lstRefuse = (from rs in context.RegSchools
                                                  join sch in context.Schools on rs.ID equals sch.ParentID
                                                  join ch in context.Children on sch.ID equals ch.SchoolID
                                                  join user in context.Users on ch.UserID equals user.ID
                                                  join re in context.Refuses on user.ID equals re.UserID
                                                  where rs.UserID == UserID && ch.IsActive != false
                                                  select new
                                                  {
                                                      id = ch.ID,
                                                      name = ch.Name,

                                                      pinimage = "refuse.png",
                                                      latitude = re.Latitude,
                                                      longitude = re.Longitude,
                                                      type = "refuse",
                                                      label = "refuse",
                                                      cash = 0,
                                                      greenpoints = re.GreenPoints,
                                                      filename = re.FileName,
                                                      description = re.Idea
                                                  }).ToList<object>();

                        return lstRefuse;
                    }
                case "reduce":
                    if (RoleID == (int)UserRoleTypeEnum.SubSchoolAdmin)
                    {
                        List<object> lstReduce = (from ch in context.Children
                                                  join sch in context.Schools on ch.SchoolID equals sch.ID
                                                  join user in context.Users on ch.UserID equals user.ID
                                                  join rd in context.Reduces on user.ID equals rd.UserID
                                                  where sch.UserID == UserID && ch.IsActive != false
                                                  select new
                                                  {
                                                      id = ch.ID,
                                                      name = ch.Name,

                                                      pinimage = "reduce.png",
                                                      latitude = rd.Latitude,
                                                      longitude = rd.Longitude,
                                                      type = "reduce",
                                                      label = "reduce",
                                                      cash = 0,
                                                      greenpoints = rd.GreenPoints,
                                                      filename = rd.FileName,
                                                      description = rd.Idea
                                                  }).ToList<object>();

                        return lstReduce;
                    }
                    else //if (RoleID == (int)UserRoleTypeEnum.SchoolAdmin)
                    {
                        List<object> lstReduce = (from rs in context.RegSchools
                                                  join sch in context.Schools on rs.ID equals sch.ParentID
                                                  join ch in context.Children on sch.ID equals ch.SchoolID
                                                  join user in context.Users on ch.UserID equals user.ID
                                                  join rd in context.Reduces on user.ID equals rd.UserID
                                                  where rs.UserID == UserID && ch.IsActive != false
                                                  select new
                                                  {
                                                      id = ch.ID,
                                                      name = ch.Name,

                                                      pinimage = "reduce.png",
                                                      latitude = rd.Latitude,
                                                      longitude = rd.Longitude,
                                                      type = "reduce",
                                                      label = "reduce",
                                                      cash = 0,
                                                      greenpoints = rd.GreenPoints,
                                                      filename = rd.FileName,
                                                      description = rd.Idea
                                                  }).ToList<object>();

                        return lstReduce;
                    }

                case "reuse":
                    if (RoleID == (int)UserRoleTypeEnum.SubSchoolAdmin)
                    {
                        List<object> lstReuse = (from ch in context.Children
                                                 join sch in context.Schools on ch.SchoolID equals sch.ID
                                                 join user in context.Users on ch.UserID equals user.ID
                                                 join ru in context.Reuses on user.ID equals ru.UserID
                                                 where sch.UserID == UserID && ch.IsActive != false
                                                 select new
                                                 {
                                                     id = ch.ID,
                                                     name = ch.Name,

                                                     pinimage = "reuse.png",
                                                     latitude = ru.Latitude,
                                                     longitude = ru.Longitude,
                                                     type = "reuse",
                                                     label = "reuse",
                                                     cash = 0,
                                                     greenpoints = ru.GreenPoints,
                                                     filename = ru.FileName,
                                                     description = ru.Idea
                                                 }).ToList<object>();

                        return lstReuse;
                    }
                    else //if (RoleID == (int)UserRoleTypeEnum.SchoolAdmin)
                    {
                        List<object> lstReuse = (from rs in context.RegSchools
                                                 join sch in context.Schools on rs.ID equals sch.ParentID
                                                 join ch in context.Children on sch.ID equals ch.SchoolID
                                                 join user in context.Users on ch.UserID equals user.ID
                                                 join ru in context.Reuses on user.ID equals ru.UserID
                                                 where rs.UserID == UserID && ch.IsActive != false
                                                 select new
                                                 {
                                                     id = ch.ID,
                                                     name = ch.Name,

                                                     pinimage = "reuse.png",
                                                     latitude = ru.Latitude,
                                                     longitude = ru.Longitude,
                                                     type = "reuse",
                                                     label = "reuse",
                                                     cash = 0,
                                                     greenpoints = ru.GreenPoints,
                                                     filename = ru.FileName,
                                                     description = ru.Idea
                                                 }).ToList<object>();

                        return lstReuse;
                    }
                case "replant":
                    if (RoleID == (int)UserRoleTypeEnum.SubSchoolAdmin)
                    {
                        List<object> lstReplant = (from ch in context.Children
                                                   join sch in context.Schools on ch.SchoolID equals sch.ID
                                                   join user in context.Users on ch.UserID equals user.ID
                                                   join rp in context.Replants on user.ID equals rp.UserID
                                                   where sch.UserID == UserID && ch.IsActive != false
                                                   select new
                                                   {
                                                       id = ch.ID,
                                                       name = ch.Name,

                                                       pinimage = "replant.png",
                                                       latitude = rp.Latitude,
                                                       longitude = rp.Longitude,
                                                       type = "replant",
                                                       label = "replant",
                                                       cash = 0,
                                                       greenpoints = rp.GreenPoints,
                                                       filename = rp.FileName,
                                                       description = rp.Description
                                                   }).ToList<object>();

                        return lstReplant;
                    }
                    else //if (RoleID == (int)UserRoleTypeEnum.SchoolAdmin)
                    {
                        List<object> lstReplant = (from rs in context.RegSchools
                                                   join sch in context.Schools on rs.ID equals sch.ParentID
                                                   join ch in context.Children on sch.ID equals ch.SchoolID
                                                   join user in context.Users on ch.UserID equals user.ID
                                                   join rp in context.Replants on user.ID equals rp.UserID
                                                   where rs.UserID == UserID && ch.IsActive != false
                                                   select new
                                                   {
                                                       id = ch.ID,
                                                       name = ch.Name,

                                                       pinimage = "replant.png",
                                                       latitude = rp.Latitude,
                                                       longitude = rp.Longitude,
                                                       type = "replant",
                                                       label = "replant",
                                                       cash = 0,
                                                       greenpoints = rp.GreenPoints,
                                                       filename = rp.FileName,
                                                       description = rp.Description
                                                   }).ToList<object>();

                        return lstReplant;
                    }
                case "recycle":
                    if (RoleID == (int)UserRoleTypeEnum.SubSchoolAdmin)
                    {
                        List<object> lstRecycle = (from ch in context.Children
                                                   join sch in context.Schools on ch.SchoolID equals sch.ID
                                                   join user in context.Users on ch.UserID equals user.ID
                                                   join rc in context.Recycles on user.ID equals rc.UserID
                                                   where sch.UserID == UserID && ch.IsActive != false
                                                   select new
                                                   {
                                                       id = ch.ID,
                                                       name = ch.Name,

                                                       pinimage = "recycle.png",
                                                       latitude = user.Latitude,
                                                       longitude = user.Longitude,
                                                       type = "recycle",
                                                       label = "recycle",
                                                       cash = 0,
                                                       greenpoints = rc.GreenPoints,
                                                       filename = rc.FileName,
                                                       //description = rc.des
                                                   }).ToList<object>();

                        return lstRecycle;
                    }
                    else //if (RoleID == (int)UserRoleTypeEnum.SchoolAdmin)
                    {
                        List<object> lstRecycle = (from rs in context.RegSchools
                                                   join sch in context.Schools on rs.ID equals sch.ParentID
                                                   join ch in context.Children on sch.ID equals ch.SchoolID
                                                   join user in context.Users on ch.UserID equals user.ID
                                                   join rc in context.Recycles on user.ID equals rc.UserID
                                                   where rs.UserID == UserID && ch.IsActive != false
                                                   select new
                                                   {
                                                       id = ch.ID,
                                                       name = ch.Name,

                                                       pinimage = "recycle.png",
                                                       latitude = user.Latitude,
                                                       longitude = user.Longitude,
                                                       type = "recycle",
                                                       label = "recycle",
                                                       cash = 0,
                                                       greenpoints = rc.GreenPoints,
                                                       filename = rc.FileName,
                                                       //description = rc.Idea
                                                   }).ToList<object>();

                        return lstRecycle;
                    }
                case "regift":
                    if (RoleID == (int)UserRoleTypeEnum.SubSchoolAdmin)
                    {
                        List<object> lstRegift = (from ch in context.Children
                                                  join sch in context.Schools on ch.SchoolID equals sch.ID
                                                  join user in context.Users on ch.UserID equals user.ID
                                                  join rg in context.Regifts on user.ID equals rg.UserID
                                                  where sch.UserID == UserID && ch.IsActive != false
                                                  select new
                                                  {
                                                      id = ch.ID,
                                                      name = ch.Name,

                                                      pinimage = "regift.png",
                                                      latitude = rg.Latitude,
                                                      longitude = rg.Longitude,
                                                      type = "regift",
                                                      label = "regift",
                                                      cash = 0,
                                                      greenpoints = rg.GreenPoints,
                                                      filename = rg.FileName,
                                                      description = rg.Description
                                                  }).ToList<object>();

                        return lstRegift;
                    }
                    else //if (RoleID == (int)UserRoleTypeEnum.SchoolAdmin)
                    {
                        List<object> lstRegift = (from rs in context.RegSchools
                                                  join sch in context.Schools on rs.ID equals sch.ParentID
                                                  join ch in context.Children on sch.ID equals ch.SchoolID
                                                  join user in context.Users on ch.UserID equals user.ID
                                                  join rg in context.Regifts on user.ID equals rg.UserID
                                                  where rs.UserID == UserID && ch.IsActive != false
                                                  select new
                                                  {
                                                      id = ch.ID,
                                                      name = ch.Name,

                                                      pinimage = "regift.png",
                                                      latitude = rg.Latitude,
                                                      longitude = rg.Longitude,
                                                      type = "regift",
                                                      label = "regift",
                                                      cash = 0,
                                                      greenpoints = rg.GreenPoints,
                                                      filename = rg.FileName,
                                                      description = rg.Description
                                                  }).ToList<object>();

                        return lstRegift;
                    }
                case "report":
                    if (RoleID == (int)UserRoleTypeEnum.SubSchoolAdmin)
                    {
                        List<object> lstReport = (from ch in context.Children
                                                  join sch in context.Schools on ch.SchoolID equals sch.ID
                                                  join user in context.Users on ch.UserID equals user.ID
                                                  join rpt in context.Reports on user.ID equals rpt.UserID
                                                  where sch.UserID == UserID && ch.IsActive != false
                                                  select new
                                                  {
                                                      id = ch.ID,
                                                      name = ch.Name,

                                                      pinimage = "report.png",
                                                      latitude = rpt.Latitude,
                                                      longitude = rpt.Longitude,
                                                      type = "report",
                                                      label = "report",
                                                      cash = 0,
                                                      greenpoints = rpt.GreenPoints,
                                                      filename = rpt.FileName,
                                                      description = rpt.Description
                                                  }).ToList<object>();

                        return lstReport;
                    }
                    else //if (RoleID == (int)UserRoleTypeEnum.SchoolAdmin)
                    {
                        List<object> lstReport = (from rs in context.RegSchools
                                                  join sch in context.Schools on rs.ID equals sch.ParentID
                                                  join ch in context.Children on sch.ID equals ch.SchoolID
                                                  join user in context.Users on ch.UserID equals user.ID
                                                  join rpt in context.Reports on user.ID equals rpt.UserID
                                                  where rs.UserID == UserID && ch.IsActive != false
                                                  select new
                                                  {
                                                      id = ch.ID,
                                                      name = ch.Name,

                                                      pinimage = "report.png",
                                                      latitude = rpt.Latitude,
                                                      longitude = rpt.Longitude,
                                                      type = "report",
                                                      label = "report",
                                                      cash = 0,
                                                      greenpoints = rpt.GreenPoints,
                                                      filename = rpt.FileName,
                                                      description = rpt.Description
                                                  }).ToList<object>();

                        return lstReport;
                    }
            }

            return new List<object>();
        }

        public List<object> GetClassesBySchool(int? UserId, int? RoleId)
        {
            List<object> mdlClasses = new List<object>();

            mdlClasses = (from ch in context.Children
                          join sch in context.Schools on ch.SchoolID equals sch.ID
                          where (sch.UserID == UserId && RoleId == (int)UserRoleTypeEnum.SubSchoolAdmin && ch.IsActive != false)
                          select new
                          {
                              clas = ch.ClassName,
                          }).Distinct().ToList<object>();

            return mdlClasses;
        }

        public List<object> GetSectionByClass(int? UserId, int? RoleId, string Class)
        {
            List<object> mdlClasses = new List<object>();

            mdlClasses = (from ch in context.Children
                          join sch in context.Schools on ch.SchoolID equals sch.ID
                          where (sch.UserID == UserId && ch.ClassName == Class && RoleId == (int)UserRoleTypeEnum.SubSchoolAdmin && ch.IsActive != false)
                          select new
                          {
                              section = ch.SectionName,
                          }).Distinct().ToList<object>();

            return mdlClasses;
        }

        public List<object> GetBranchesBySchoolAdmin(int? UserId)
        {
            List<object> mdlBranches = new List<object>();

            mdlBranches = (from rs in context.RegSchools
                           join sch in context.Schools on rs.ID equals sch.ParentID
                           where (rs.UserID == UserId)
                           select new
                           {
                               schoolId = sch.ID,
                               schoolName = sch.BranchName,
                           }).ToList<object>();

            return mdlBranches;
        }

        public List<object> GetSchoolComparison(int? BranchId, string Clas, string Sections, string Schools, DateTime FromDate, DateTime ToDate, int? RoleId)
        {
            List<object> mdlList = new List<object>();

            string[] section = new string[0];
            int[] school = new int[0];

            if (!string.IsNullOrEmpty(Sections))
                section = Sections.Split(',');

            if (!string.IsNullOrEmpty(Schools))
                school = Array.ConvertAll<string, int>(Schools.Split(','), int.Parse);

            mdlList = (from ch in context.Children
                       join sch in context.Schools on ch.SchoolID equals sch.ID
                       join user in context.Users on ch.UserID equals user.ID
                       join re in context.Refuses on user.ID equals re.UserID into refuses
                       join rd in context.Reduces on user.ID equals rd.UserID into reduces
                       join ru in context.Reuses on user.ID equals ru.UserID into reuses
                       join rp in context.Replants on user.ID equals rp.UserID into replants
                       join rc in context.Recycles on user.ID equals rc.UserID into recycles
                       join rg in context.Regifts on user.ID equals rg.UserID into regifts
                       join rp in context.Reports on user.ID equals rp.UserID into reports
                       join bin in context.BuyBins on user.ID equals bin.UserID into buybin
                       where (((sch.ID == BranchId && ch.ClassName == Clas && section.Contains(ch.SectionName) && RoleId == (int)UserRoleTypeEnum.SubSchoolAdmin)
                            || (school.Contains(sch.ID) && RoleId == (int)UserRoleTypeEnum.SchoolAdmin))
                            && ch.IsActive != false && sch.IsActive != false)
                       select new
                       {
                           UserId = ch.UserID,
                           Section = ch.SectionName,
                           SchoolId = ch.SchoolID,
                           SchoolName = sch.Name,
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

        public List<SchoolsComparisionResult> GetSchoolsBranchesComparisionChartBySchoolAdmin(SchoolsComparisionCriteria filter, int UserID)
        {

            List<SchoolsComparisionResult> compList = new List<SchoolsComparisionResult>();
            List<School> schoolsList = new List<School>();
            var result = db.Repository<School>().GetAll().ToList();

            if (filter.ShoolId.Count > 0)
            {
                foreach (var id in filter.ShoolId)
                {
                    var school = result.Where(x => x.ID == id).FirstOrDefault();
                    schoolsList.Add(school);
                }
            }

            List<SchoolGP_Log> schoolsgpLog = new List<SchoolGP_Log>();

            foreach (var item in schoolsList)
            {
                var list = db.Repository<SchoolGP_Log>().GetAll().Where(x => x.SchoolID == item.ID).ToList();
                schoolsgpLog.AddRange(list);
            }

            schoolsgpLog = schoolsgpLog.Where(x => x.CreatedDate >= filter.From && x.CreatedDate <= filter.To).OrderBy(x => x.CreatedDate.Year).ThenBy(x => x.CreatedDate.Month).ToList();
            if (filter.Type.Trim().ToLower() == "y")
            {
                compList = Yearly(schoolsgpLog);
            }
            else if (filter.Type.Trim().ToLower() == "m")
            {
                compList = Monthly(schoolsgpLog);
            }
            else
            {
                compList = Daily(schoolsgpLog);
            }


            //////////var resultedData = new object();
            //////////dynamic MyDynamicData = new ExpandoObject();
            //////////if (filter.Type.Trim().ToLower() == "m")
            //////////{
            //////////    resultedData = schoolsgpLog.Select(k => new { k.School.Name, k.CreatedDate.Year, k.CreatedDate.Month, k.GreenPoints }).GroupBy(x => new { x.Name, x.Year, x.Month }, (key, group) => new // SchoolsComparisionResult
            //////////    {
            //////////        Name = key.Name,
            //////////        // yr = key.Year,
            //////////        name = key.Month,
            //////////        value = group.Sum(k => k.GreenPoints)
            //////////    }).ToList();
            //////////}
            //////////else if (filter.Type.Trim().ToLower() == "y")
            //////////{
            //////////    MyDynamicData = schoolsgpLog.Select(k => new { k.School.Name, k.CreatedDate.Year, k.CreatedDate.Month, k.GreenPoints }).GroupBy(x => new { x.Name, x.Year, x.Month }, (key, group) => new // SchoolsComparisionResult
            //////////    {
            //////////        Name = key.Name,
            //////////        // yr = key.Year,
            //////////        name = key.Year,
            //////////        value = group.Sum(k => k.GreenPoints)
            //////////    }).ToList();
            //////////}




            return compList;
        }

        private List<SchoolsComparisionResult> Yearly(List<SchoolGP_Log> list)
        {
            List<SchoolsComparisionResult> compList = new List<SchoolsComparisionResult>();
            var data = list.Select(k => new { k.School.BranchName, k.CreatedDate.Year, k.CreatedDate.Month, k.GreenPoints }).GroupBy(x => new { x.BranchName, x.Year, x.Month }, (key, group) => new // SchoolsComparisionResult
            {
                Name = key.BranchName,
                // yr = key.Year,
                name = key.Year,
                value = group.Sum(k => k.GreenPoints)
            }).ToList();

            var results = from p in data
                          group p by p.Name into g
                          select new { Name = g.Key, series = g.ToList().Select(i => new { i.name, i.value }).ToList() };
            foreach (var r in results)
            {
                SchoolsComparisionResult schoolsComparisionResult = new SchoolsComparisionResult();
                schoolsComparisionResult.Name = r.Name;
                foreach (var item in r.series)
                {
                    Records record = new Records();
                    record.name = item.name.ToString();      //getAbbreviatedName(item.name);
                    record.value = item.value;
                    schoolsComparisionResult.Series.Add(record);

                }
                compList.Add(schoolsComparisionResult);


            }
            return compList;
        }
        private List<SchoolsComparisionResult> Monthly(List<SchoolGP_Log> list)
        {
            List<SchoolsComparisionResult> compList = new List<SchoolsComparisionResult>();
            var data = list.Select(k => new { k.School.BranchName, k.CreatedDate.Year, k.CreatedDate.Month, k.GreenPoints }).GroupBy(x => new { x.BranchName, x.Year, x.Month }, (key, group) => new // SchoolsComparisionResult
            {
                Name = key.BranchName,
                // yr = key.Year,
                name = getAbbreviatedName(key.Month) + "_" + key.Year,
                value = group.Sum(k => k.GreenPoints)
            }).ToList();

            var results = from p in data
                          group p by p.Name into g
                          select new { Name = g.Key, series = g.ToList().Select(i => new { i.name, i.value }).ToList() };
            foreach (var r in results)
            {
                SchoolsComparisionResult schoolsComparisionResult = new SchoolsComparisionResult();
                schoolsComparisionResult.Name = r.Name;
                foreach (var item in r.series)
                {
                    Records record = new Records();
                    record.name = item.name;
                    record.value = item.value;
                    schoolsComparisionResult.Series.Add(record);

                }
                compList.Add(schoolsComparisionResult);


            }
            return compList;
        }
        private List<SchoolsComparisionResult> Daily(List<SchoolGP_Log> list)
        {
            List<SchoolsComparisionResult> compList = new List<SchoolsComparisionResult>();
            var data = list.Select(k => new { k.School.BranchName, k.CreatedDate.Month, k.CreatedDate.Day, k.GreenPoints }).GroupBy(x => new { x.BranchName, x.Month, x.Day }, (key, group) => new // SchoolsComparisionResult
            {
                Name = key.BranchName,
                // yr = key.Year,
                name = key.Day + "_" + getAbbreviatedName(key.Month),
                value = group.Sum(k => k.GreenPoints)
            }).ToList();

            var results = from p in data
                          group p by p.Name into g
                          select new { Name = g.Key, series = g.ToList().Select(i => new { i.name, i.value }).ToList() };
            foreach (var r in results)
            {
                SchoolsComparisionResult schoolsComparisionResult = new SchoolsComparisionResult();
                schoolsComparisionResult.Name = r.Name;
                foreach (var item in r.series)
                {
                    Records record = new Records();
                    record.name = item.name.ToString();   //getAbbreviatedName(item.name);
                    record.value = item.value;
                    schoolsComparisionResult.Series.Add(record);

                }
                compList.Add(schoolsComparisionResult);


            }
            //compList.OrderBy(x => x.Series.OrderBy(y => y.name)).ToList();
            return compList;
        }

        static string getAbbreviatedName(int month)
        {
            DateTime date = new DateTime(2021, month, 1);

            return date.ToString("MMM");
        }

        public List<Records> GetSchoolsBranchesComparisionPieChartBySchoolAdmin(int UserID)
        {

            List<Records> compList = new List<Records>();
            List<School> schoolsList = new List<School>();
            var baseSchool = db.Repository<RegSchool>().GetAll().Where(x => x.UserID == UserID).FirstOrDefault();
            var result = db.Repository<School>().GetAll().Where(x => x.ParentID == baseSchool.ID);
            //schoolsList = result.Where(x => x.UserID == UserID).ToList();
            List<SchoolGP_Log> schoolsgpLog = new List<SchoolGP_Log>();
            foreach (var item in result)
            {
                var list = db.Repository<SchoolGP_Log>().GetAll().Where(x => x.SchoolID == item.ID).ToList();
                schoolsgpLog.AddRange(list);
            }

            var data = schoolsgpLog.Select(k => new { k.School.BranchName, k.GreenPoints }).GroupBy(x => new { x.BranchName }, (key, group) => new
            {
                name = key.BranchName,
                value = group.Sum(k => k.GreenPoints)
            }).ToList();

            foreach (var item in data)
            {
                Records record = new Records();
                record.name = item.name;
                record.value = item.value;
                compList.Add(record);
            }

            return compList;
        }

        public List<Records> GetSchoolsBranchesStudentsPieChartBySchoolAdmin(int UserID)
        {
            List<Records> compList = new List<Records>();
            List<School> schoolsList = new List<School>();
            List<Child> students = new List<Child>();
            var baseSchool = db.Repository<RegSchool>().GetAll().Where(x => x.UserID == UserID).FirstOrDefault();
            var result = db.Repository<School>().GetAll().Where(x => x.ParentID == baseSchool.ID);
            foreach (var item in result)
            {
                var student = db.Repository<Child>().GetAll().Where(x => x.SchoolID == item.ID).ToList();
                students.AddRange(student);
            }
            //foreach (var item in result)
            //{
            //    var list = db.Repository<SchoolGP_Log>().GetAll().Where(x => x.SchoolID == item.ID).ToList();
            //    schoolsgpLog.AddRange(list);
            //}

            var data = students.Select(k => new { k.School.BranchName }).GroupBy(x => new { x.BranchName }, (key, group) => new
            {
                name = key.BranchName,
                value = group.Count()
            }).ToList();

            foreach (var item in data)
            {
                Records record = new Records();
                record.name = item.name;
                record.value = item.value;
                compList.Add(record);
            }

            return compList;
        }

        public List<object> GetSchoolBranchesByUserId(int UserId)
        {
            List<object> lstSchool = new List<object>();
            //List<object> lstSchool = (from regsch in context.RegSchools
            //                          join school in context.Schools on regsch.ID equals school.ParentID
            //                          where regsch.UserID == UserId && school.IsActive == true
            //                          select new
            //                          {
            //                              school.ID,
            //                              school.BranchName,
            //                          }).ToList().Select(u => new
            //                          {
            //                              u.ID,
            //                              u.BranchName,
            //                          }).ToList<object>();

            var result = db.Repository<RegSchool>().GetAll().Where(x => x.UserID == UserId).FirstOrDefault();
            var userSchools = new List<School>();
            if (result != null)
            {
                userSchools = db.Repository<School>().GetAll().Where(x => x.ParentID == result.ID && x.IsActive == true).ToList();
            }
            if (userSchools.Count > 0)
            {
                lstSchool = userSchools.Select(x => new
                {
                    x.ID,
                    x.BranchName
                }).ToList<object>();
            }


            return lstSchool;
        }

        public List<object> GetSchoolStudentsBySchoolId(BranchRequest model, int? UserId)
        {
            if (model.All)
            {
                List<object> lstSchool = (from regsch in context.RegSchools
                                          join school in context.Schools on regsch.ID equals school.ParentID
                                          join student in context.Children on school.ID equals student.SchoolID
                                          where school.IsActive == true && regsch.UserID == UserId
                                          select new
                                          {

                                              school.ID,
                                              school.BranchName,
                                              student.Name,
                                              student.ClassName,
                                              school.Address,
                                              greenWorth = school.GreenPoints,
                                              school.Level,
                                          }).ToList().Select(u => new
                                          {
                                              u.ID,
                                              u.BranchName,
                                              u.Name,
                                              u.ClassName,
                                              u.Address,
                                              u.greenWorth,
                                              Level = Utility.GetLevelByGP(u.greenWorth),
                                          }).ToList<object>();

                return lstSchool;
            }
            else
            {
                List<object> lstSchool = (from school in context.Schools
                                              // join school in context.Schools on regsch.ID equals school.ParentID
                                          join student in context.Children on school.ID equals student.SchoolID
                                          where school.ID == model.Id && school.IsActive == true
                                          select new
                                          {
                                              school.ID,
                                              school.BranchName,
                                              student.Name,
                                              student.ClassName,
                                              school.Address,
                                              greenWorth = school.GreenPoints,
                                              school.Level,
                                          }).ToList().Select(u => new
                                          {
                                              u.ID,
                                              u.BranchName,
                                              u.Name,
                                              u.ClassName,
                                              u.Address,
                                              u.greenWorth,
                                              Level = Utility.GetLevelByGP(u.greenWorth),
                                          }).ToList<object>();

                return lstSchool;
            }

        }
    }
}