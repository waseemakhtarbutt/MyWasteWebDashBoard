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
using DrTech.Amal.SQLDataAccess.CustomModels;
using DrTech.Amal.Common.Helpers;

namespace DrTech.Amal.SQLDataAccess.Repository
{
    public class OrganizationRepository : Repository<Organization>
    {
        public OrganizationRepository(Amal_Entities context)
        : base(context)
        {
            dbSet = context.Set<Organization>();
        }

        public List<Organization> GetEmployeeOrganizationByUserID(int? UserID)
        {
            List<Organization> mdlOrganization = (from des in context.Members
                                                  join sch in context.Organizations on des.OrgId equals sch.ID
                                                  where des.UserID == UserID
                                                  select sch).Distinct().ToList();
            return mdlOrganization;
        }

        public List<object> GetMemberListByRole(int? UserID, bool IsSuspended, int? RoleID)
        {
            List<object> mdlMembers = new List<object>();

            if (RoleID == (int)UserRoleTypeEnum.SubOrganizationAdmin || RoleID == (int)UserRoleTypeEnum.Admin)
            {
                mdlMembers = (from mb in context.Members
                              join org in context.Organizations on mb.OrgId equals org.ID
                              join user in context.Users on mb.UserID equals user.ID
                              join role in context.Roles on user.RoleID equals role.ID
                              join re in context.Refuses on user.ID equals re.UserID into refuses
                              join rd in context.Reduces on user.ID equals rd.UserID into reduses
                              join ru in context.Reuses on user.ID equals ru.UserID into reuses
                              join rp in context.Replants on user.ID equals rp.UserID into replants
                              join rc in context.Recycles on user.ID equals rc.UserID into recycles
                              join rg in context.Regifts on user.ID equals rg.UserID into regifts
                              join rp in context.Reports on user.ID equals rp.UserID into reports
                              join bin in context.BuyBins on user.ID equals bin.UserID into buybin
                              where ((org.UserID == UserID && RoleID == (int)UserRoleTypeEnum.SubOrganizationAdmin)
                              || (RoleID == (int)UserRoleTypeEnum.Admin))
                               && ((mb.IsActive != false && IsSuspended == false) || (mb.IsActive == false && IsSuspended == true))
                             // && mb.IsActive != false
                              select new
                              {
                                  id = mb.ID,
                                  name = user.FullName,
                                  orgname = org.Name,
                                  filename = user.FileName,
                                  department = mb.Department,
                                  role = role.RoleName,
                                  location = mb.Location,
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
                                  orgname = u.orgname,
                                  filename = u.filename,
                                  department = u.department,
                                  role = u.role,
                                  location = u.location,
                                  contactno = u.contactno,
                                  totalGP = u.Refuses + u.Reduces + u.Reuses + u.Replants + u.Recycles + u.Regifts + u.Reports + u.Bins
                              }).OrderByDescending(x=>x.totalGP).ToList<object>();
            }
            else if (RoleID == (int)UserRoleTypeEnum.OrganizationAdmin)
            {
                mdlMembers = (from rorg in context.RegOrganizations
                              join org in context.Organizations on rorg.ID equals org.ParentID
                              join mb in context.Members on org.ID equals mb.OrgId
                              join user in context.Users on mb.UserID equals user.ID
                              join role in context.Roles on user.RoleID equals role.ID
                              join re in context.Refuses on user.ID equals re.UserID into refuses
                              join rd in context.Reduces on user.ID equals rd.UserID into reduses
                              join ru in context.Reuses on user.ID equals ru.UserID into reuses
                              join rp in context.Replants on user.ID equals rp.UserID into replants
                              join rc in context.Recycles on user.ID equals rc.UserID into recycles
                              join rg in context.Regifts on user.ID equals rg.UserID into regifts
                              join rp in context.Reports on user.ID equals rp.UserID into reports
                              join bin in context.BuyBins on user.ID equals bin.UserID into buybin
                              where rorg.UserID == UserID

                               && ((mb.IsActive != false && IsSuspended == false) || (mb.IsActive == false && IsSuspended == true))
                              //&& mb.IsActive != false
                              select new
                              {
                                  id = mb.ID,
                                  name = user.FullName,
                                  orgname = org.Name,
                                  filename = user.FileName,
                                  department = mb.Department,
                                  role = role.RoleName,
                                  location = mb.Location,
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
                                  orgname = u.orgname,
                                  filename = u.filename,
                                  department = u.department,
                                  role = u.role,
                                  location = u.location,
                                  contactno = u.contactno,
                                  totalGP = u.Refuses + u.Reduces + u.Reuses + u.Replants + u.Recycles + u.Regifts + u.Reports + u.Bins
                              }).OrderByDescending(x => x.totalGP).ToList<object>();
            }

            return mdlMembers;
        }

        public List<object> GetMemberListByRoleWithMemberProgress(int? UserID, bool IsSuspended, int? RoleID)
        {
            List<object> mdlMembers = new List<object>();

            if (RoleID == (int)UserRoleTypeEnum.SubOrganizationAdmin || RoleID == (int)UserRoleTypeEnum.Admin)
            {
                mdlMembers = (from mb in context.Members.ToList()
                              join org in context.Organizations on mb.OrgId equals org.ID
                              join user in context.Users on mb.UserID equals user.ID
                              join role in context.Roles on user.RoleID equals role.ID
                              join re in context.Refuses on user.ID equals re.UserID into refuses
                              join rd in context.Reduces on user.ID equals rd.UserID into reduses
                              join ru in context.Reuses on user.ID equals ru.UserID into reuses
                              join rp in context.Replants on user.ID equals rp.UserID into replants
                              join rc in context.Recycles on user.ID equals rc.UserID into recycles
                              join rg in context.Regifts on user.ID equals rg.UserID into regifts
                              join rp in context.Reports on user.ID equals rp.UserID into reports
                              join bin in context.BuyBins on user.ID equals bin.UserID into buybin
                              where ((org.UserID == UserID && RoleID == (int)UserRoleTypeEnum.SubOrganizationAdmin)
                              || (RoleID == (int)UserRoleTypeEnum.Admin))
                               && ((mb.IsActive != false && IsSuspended == false) || (mb.IsActive == false && IsSuspended == true))
                              // && mb.IsActive != false
                              select new
                              {
                                  id = mb.ID,
                                  name = user.FullName,
                                  orgname = org.Name,
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
                                  level    =Utility.GetLevelByGP(u.Refuses + u.Reduces + u.Reuses + u.Replants + u.Recycles + u.Regifts + u.Reports + u.Bins)
                              }).OrderByDescending(x => x.totalGP).ToList<object>();
            }
            else if (RoleID == (int)UserRoleTypeEnum.OrganizationAdmin)
            {
                mdlMembers = (from rorg in context.RegOrganizations.ToList()
                              join org in context.Organizations on rorg.ID equals org.ParentID
                              join mb in context.Members on org.ID equals mb.OrgId
                              join user in context.Users on mb.UserID equals user.ID
                              join role in context.Roles on user.RoleID equals role.ID
                              join re in context.Refuses on user.ID equals re.UserID into refuses
                              join rd in context.Reduces on user.ID equals rd.UserID into reduses
                              join ru in context.Reuses on user.ID equals ru.UserID into reuses
                              join rp in context.Replants on user.ID equals rp.UserID into replants
                              join rc in context.Recycles on user.ID equals rc.UserID into recycles
                              join rg in context.Regifts on user.ID equals rg.UserID into regifts
                              join rp in context.Reports on user.ID equals rp.UserID into reports
                              join bin in context.BuyBins on user.ID equals bin.UserID into buybin
                              where rorg.UserID == UserID

                               && ((mb.IsActive != false && IsSuspended == false) || (mb.IsActive == false && IsSuspended == true))
                              //&& mb.IsActive != false
                              select new
                              {
                                  id = mb.ID,
                                  name = user.FullName,
                                  orgname = org.Name,
                                  filename = user.FileName,
                                  department = mb.Department,
                                  role = role.RoleName,
                                  location = mb.Location,
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

            return mdlMembers;
        }
        public List<object> GetApprovedOrganizationList(OrganizationRequestDto model,int? UserId)
        {
            List<object> response = new List<object>();
           var mdlApprovedOrgs = (from rg in context.Organizations                                     
                                      // join city in context.LookupTypes on rg.CityID equals city.ID
                                       where ( rg.IsVerified == true  && rg.IsActive == true)
                                       select new
                                       {
                                           rg.ID,
                                           rg.Name,
                                           rg.SiteOffice,
                                           rg.ContactPerson,
                                           rg.Phone,                                        
                                           userId = UserId,
                                          // cityDescription = city.Name,
                                           rg.FileName,
                                           rg.Email,
                                           rg.CreatedDate
                                       }).OrderByDescending(o => o.Name).ToList();
            if (model.StartDate != null && model.EndDate != null)
            {
                response = mdlApprovedOrgs.Where(x => x.CreatedDate >= Utility.GetDateFromString(model.StartDate) && x.CreatedDate <= Utility.GetDateFromString(model.EndDate)).ToList<object>();
                return response;
                // return mdlRecycles.Where(x => x.CreatedDate >= model.StartDate && x.CreatedDate <= model.EndDate).ToList();
            }
            else
            {
                response = mdlApprovedOrgs.ToList<object>();
                return response;
            }

        }
        public List<object> GetSuspendedOrganizationList(int? UserId)
        {
            List<object> mdlApprovedOrgs = (from rg in context.Organizations
                                           // join  city in context.LookupTypes on rg.CityID equals city.ID 
                                            where (rg.IsActive == false)
                                            select new
                                            {
                                                rg.ID,
                                                rg.Name,
                                                rg.SiteOffice,
                                                rg.ContactPerson,
                                                rg.Phone,
                                                userId = UserId,
                                                //cityDescription = city.Name,
                                                rg.FileName,
                                                rg.Email
                                            }).OrderByDescending(o => o.Name).ToList<object>();

            return mdlApprovedOrgs;
        }
        public int GetOrganizationGreenPoints(int SchoolID)
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
        public int GetOrgAllBranchesGreenPointWorth(int? ParentID)
        {
            int OrgGreenWorthTotal = 0;
            var allBranches = context.Organizations.Where(x => x.ParentID == ParentID).ToList();
            if (allBranches.Count > 0)
            {
                foreach (var item in allBranches)
                {
                    OrgGreenWorthTotal += GetOrganizationGreenPoints(item.ID);
                }            
            }
            return OrgGreenWorthTotal;
        }

        public List<Object> GetAllOrganizationWithRegistrationStatus(int? UserID)
        {
            List<Object> mdlOrganization = (from reg in context.RegOrganizations
                                      join org in context.Organizations on reg.ID equals org.ParentID
                                      join mem in context.Members on org.ID equals mem.OrgId into member
                                      where (org.IsVerified == true && org.IsActive == true)
                                      select new
                                      {
                                          reg.ID,
                                          reg.Name,
                                          org.CreatedDate,
                                          org.FileName,
                                          memberGW = org.OrgnizationGP_Log.Where(x => x.IsActive != false && x.MemberID != null && x.Member.IsActive != false && x.CreatedDate >= EntityFunctions.TruncateTime(x.Organization.CreatedDate)).Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum(),
                                          memCount = member.Where(x => x.UserID == UserID && x.IsActive != false).Count(),
                                      }).OrderBy(x => x.CreatedDate)
                                    .GroupBy(x => new { x.ID, x.Name }).ToList()
                                    .Select(s => new
                                    {
                                        ID = s.Key.ID,
                                        Name = s.Key.Name,
                                        FileName = s.Select(y => y.FileName).First(),
                                        greenWorth = s.Select(y => y.memberGW).Sum(),
                                        memCount = s.Select(y => y.memCount).Sum(),
                                    })
                                    .ToList()
                                    .Select(z => new
                                    {
                                        z.ID,
                                        z.Name,
                                        z.FileName,
                                        z.greenWorth,
                                        Level = Utility.GetLevelByGP(z.greenWorth),
                                        IsAdded = (z.memCount > 0) ? true : false,
                                    }).ToList<object>();

            return mdlOrganization;
        }

        public List<Object> GetAllSubOfficesWithRegistrationStatus(int? UserID, int? ID)
        {
            List<Object> mdlSchool = (from org in context.Organizations
                                          //join child in context.Children on school.ID equals child.SchoolID into st
                                          //from stype in st.DefaultIfEmpty()
                                      where org.ParentID == ID || org.ParentID == null || org.ParentID == 0
                                      select org)
                                      .ToList()
                                      .Select(o => new
                                      {
                                          o.ID,
                                          o.Name,
                                          o.GreenPoints,
                                          o.Phone,
                                          o.Address,
                                          OrgType = o.LookupType.Name,
                                          o.OrgTypeID,
                                          o.ContactPerson,
                                          o.FileName,
                                          o.CreatedBy,
                                          o.EmployeeGreenPoints,
                                          o.Level,
                                          o.UserID,
                                          o.IsMainBranch,
                                          o.Email,
                                          IsAdded = CheckUserRegistrationInOrganization(UserID, o.ID)
                                      }).ToList<object>();
            return mdlSchool;
        }

        public bool CheckUserRegistrationInOrganization(int? UserID, int? OrgID)
        {
            int v = (from c in context.Members
                     where c.UserID == UserID && c.OrgId == OrgID
                     select c).Count();
            if (v > 0)
                return true;
            return false;
        }

        public List<object> GetOrgBranchesByID(int? UserID, int? ParentID)
        {
            List<Object> mdlOrganization = (from org in context.Organizations
                                      join mem in context.Members on org.ID equals mem.OrgId into members
                                      where org.ParentID == ParentID && org.IsActive == true
                                      select new
                                      {
                                          org.ID,
                                          org.Name,
                                          org.Phone,
                                          org.Address,
                                          org.ParentID,
                                          org.FileName,
                                          org.ContactPerson,
                                          org.CreatedBy,
                                          org.UserID,
                                          org.SiteOffice,
                                          org.Email,
                                          memberGW = org.OrgnizationGP_Log.Where(x => x.IsActive != false && x.MemberID != null && x.Member.IsActive != false && x.CreatedDate >= EntityFunctions.TruncateTime(x.Organization.CreatedDate)).Select(y => y.GreenPoints).DefaultIfEmpty(0).Sum(),
                                          memCount = members.Where(x => x.UserID == UserID && x.IsActive != false).Count(),
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
                                          z.CreatedBy,
                                          z.UserID,
                                          BranchName = z.SiteOffice,
                                          z.Email,
                                          greenWorth = z.memberGW,
                                          Level = Utility.GetLevelByGP(z.memberGW),
                                          IsAdded = z.memCount > 0 ? true : false,
                                      }).ToList<object>();
            return mdlOrganization;
        }
        public List<object> GetDepartmentsByRole(int? UserId, int? RoleId)
        {
            List<object> mdlDepartments = new List<object>();

            mdlDepartments = (from mem in context.Members
                              join org in context.Organizations on mem.OrgId equals org.ID
                              where (org.UserID == UserId && RoleId == (int)UserRoleTypeEnum.SubOrganizationAdmin && org.IsActive != false && mem.IsActive != false)
                              select new
                              {
                                  department = mem.Department,
                              }).Distinct().ToList<object>();

            return mdlDepartments;
        }
        public List<object> GetOrganizationComparison(int? OrganizationId, string Departments, string Organizations, DateTime FromDate, DateTime ToDate, int? RoleId)
        {
            List<object> mdlList = new List<object>();

            string[] department = new string[0];
            int[] organization = new int[0];

            if (!string.IsNullOrEmpty(Departments))
                department = Departments.Split(',');

            if (!string.IsNullOrEmpty(Organizations))
                organization = Array.ConvertAll<string, int>(Organizations.Split(','), int.Parse);

            mdlList = (from mem in context.Members
                       join org in context.Organizations on mem.OrgId equals org.ID
                       join user in context.Users on mem.UserID equals user.ID
                       join re in context.Refuses on user.ID equals re.UserID into refuses
                       join rd in context.Reduces on user.ID equals rd.UserID into reduces
                       join ru in context.Reuses on user.ID equals ru.UserID into reuses
                       join rp in context.Replants on user.ID equals rp.UserID into replants
                       join rc in context.Recycles on user.ID equals rc.UserID into recycles
                       join rg in context.Regifts on user.ID equals rg.UserID into regifts
                       join rp in context.Reports on user.ID equals rp.UserID into reports
                       join bin in context.BuyBins on user.ID equals bin.UserID into buybin
                       where (((org.ID == OrganizationId && department.Contains(mem.Department) && RoleId == (int)UserRoleTypeEnum.SubOrganizationAdmin)
                            || (organization.Contains(org.ID) && RoleId == (int)UserRoleTypeEnum.OrganizationAdmin))
                            && mem.IsActive != false && org.IsActive != false)
                       select new
                       {
                           UserId = mem.UserID,
                           Department = mem.Department,
                           OrganizationId = org.ID,
                           OrganizationName = org.SiteOffice,
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
        public List<object> GetBranchesByOrganizationAdmin(int? UserId)
        {
            List<object> mdlBranches = new List<object>();

            mdlBranches = (from ro in context.RegOrganizations
                           join org in context.Organizations on ro.ID equals org.ParentID
                           where (ro.UserID == UserId && org.IsActive != false && ro.IsActive != false)
                           select new
                           {
                               organizationId = org.ID,
                               organizationName = org.SiteOffice,
                           }).Distinct().ToList<object>();

            return mdlBranches;
        }
    }
}
