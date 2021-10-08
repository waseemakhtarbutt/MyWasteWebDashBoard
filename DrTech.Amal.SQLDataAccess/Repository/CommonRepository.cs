using DrTech.Amal.Common.Enums;
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

    public class CommonRepository : Repository<LookupType>
    {
        public ContextDB db;
        public CommonRepository(Amal_Entities context)
       : base(context)
        {
            dbSet = context.Set<LookupType>();
            db = new ContextDB();
        }

        public List<LookupType> GetLookupByTypeName(string TypeName)
        {
            List<LookupType> lstLookup = (from des in context.LookupTypes
                                          where des.Type == TypeName && des.IsActive == true
                                          select des).ToList();
            return lstLookup;
        }

        public List<LookupType> GetTypeByParentID(int ParentID)
        {
            List<LookupType> lstLookup = (from des in context.LookupTypes
                                          where des.ParentID == ParentID && des.IsActive == true
                                          select des).ToList();
            return lstLookup;
        }
        public List<object> GetStatusessList(List<string> statuses = null)
        {
            List<object> lstStatuss = null;

            if (statuses == null || statuses.Count() == 0)
                lstStatuss = (from des in context.Status
                              select new
                              {
                                  Value = des.ID,
                                  Description = des.StatusName,
                              }

                                              ).ToList<object>();
            else
                lstStatuss = (from des in context.Status
                              where (statuses.Contains(des.StatusName))
                              select new
                              {
                                  Value = des.ID,
                                  Description = des.StatusName,
                              }

                                              ).ToList<object>();

            return lstStatuss;
        }
        #region|Update user all associated org,school, business green points for just green worth not redeamable |
        public void UpdateParentsTableGreenPoints(int UserId, int? loginAdminUserId, int RsId, string type, int UserLastGreenPoints = 0, int UserUpdatedGreenPoints = 0)
        {
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

            string flag = "";
            if (lstOrg.Count() > 0)
            {

                foreach (var item in lstOrg)
                {
                    flag = "org";
                    Organization UserOrg = db.Repository<Organization>().FindById(item.ID);
                    //int OrgOrignalGPoints = UserOrg.EmployeeGreenPoints - UserLastGreenPoints;
                    //int OrgUpdatedGPoints = OrgOrignalGPoints + UserUpdatedGreenPoints;
                    //UserOrg.EmployeeGreenPoints = OrgUpdatedGPoints;
                    //db.Repository<Organization>().Update(UserOrg);
                    //db.Save();
                    //also update log table

                    //New Fixing by Ihsan. update buesiness
                    UserOrg.EmployeeGreenPoints = UserOrg.EmployeeGreenPoints - UserLastGreenPoints;
                    UserOrg.EmployeeGreenPoints = UserOrg.EmployeeGreenPoints + UserUpdatedGreenPoints;
                    UserOrg.UpdatedDate = DateTime.Now;
                    UserOrg.UpdatedBy = loginAdminUserId;
                    db.Repository<Organization>().Update(UserOrg);
                    db.Save();

                    List<Member> lstMembers = db.Repository<Member>().GetAll().Where(x => x.OrgId == item.ID && x.UserID == UserId && x.IsActive == true).ToList();

                    foreach (var items in lstMembers)
                    {
                        AssociationVeiwModel model = new AssociationVeiwModel();
                        model.OrganizationID = UserOrg.ID;
                        model.GreenPoints = UserUpdatedGreenPoints;
                        model.UserID = UserId;
                        model.LoginAdminUserId = Convert.ToInt32(loginAdminUserId);
                        model.RsID = RsId;
                        model.Type = type;
                        model.MemeberID = items.ID;
                        SaveAssociationIndividualRecords(flag, model);
                    }
                }
            }

            if (lstSchool.Count() > 0)
            {

                foreach (var item in lstSchool)
                {
                    flag = "sch";
                    School UserSchool = db.Repository<School>().FindById(item.ID);
                    //int SchoolOrignalGPoints = UserSchool.ParentsGreenPoints - UserLastGreenPoints;
                    //int SchoolUpdatedGPoints = SchoolOrignalGPoints + UserUpdatedGreenPoints;
                    //UserSchool.ParentsGreenPoints = SchoolUpdatedGPoints;
                    //db.Repository<School>().Update(UserSchool);
                    //db.Save();
                    //also update log table

                    //New Fixing by Ihsan. update buesiness
                    UserSchool.ParentsGreenPoints = UserSchool.ParentsGreenPoints - UserLastGreenPoints;
                    UserSchool.ParentsGreenPoints = UserSchool.ParentsGreenPoints + UserUpdatedGreenPoints;
                    UserSchool.UpdatedDate = DateTime.Now;
                    UserSchool.UpdatedBy = loginAdminUserId;
                    db.Repository<School>().Update(UserSchool);
                    db.Save();


                    List<Child> lstchild = db.Repository<Child>().GetAll().Where(x => x.SchoolID == item.ID && x.UserID == UserId && x.IsActive == true).ToList();

                   // List<SchoolStaff> lststaff = db.Repository<SchoolStaff>().GetAll().Where(x => x.SchoolID == item.ID && x.UserID == UserId).ToList();

                    foreach (var items in lstchild)
                    {
                        AssociationVeiwModel model = new AssociationVeiwModel();
                        model.SchoolID = UserSchool.ID;
                        model.GreenPoints = UserUpdatedGreenPoints;
                        model.UserID = UserId;
                        model.LoginAdminUserId = Convert.ToInt32(loginAdminUserId);
                        model.RsID = RsId;
                        model.Type = type;
                        model.ChildID = items.ID;
                        SaveAssociationIndividualRecords(flag, model);
                    }


                    //foreach (var items in lststaff)
                    //{
                    //    AssociationVeiwModel model = new AssociationVeiwModel();
                    //    model.SchoolID = UserSchool.ID;
                    //    model.GreenPoints = UserUpdatedGreenPoints;
                    //    model.UserID = UserId;
                    //    model.LoginAdminUserId = Convert.ToInt32(loginAdminUserId);
                    //    model.RsID = RsId;
                    //    model.Type = type;
                    //    model.StaffID = items.ID;
                    //    SaveAssociationIndividualRecords(flag, model);
                    //}
                }
            }

            if (stafflstSchool.Count() > 0)
            {

                foreach (var item in stafflstSchool)
                {
                    flag = "sch";
                    School UserSchool = db.Repository<School>().FindById(item.ID);
                    //int SchoolOrignalGPoints = UserSchool.ParentsGreenPoints - UserLastGreenPoints;
                    //int SchoolUpdatedGPoints = SchoolOrignalGPoints + UserUpdatedGreenPoints;
                    //UserSchool.ParentsGreenPoints = SchoolUpdatedGPoints;
                    //db.Repository<School>().Update(UserSchool);
                    //db.Save();
                    //also update log table

                    //New Fixing by Ihsan. update buesiness
                    UserSchool.ParentsGreenPoints = UserSchool.ParentsGreenPoints - UserLastGreenPoints;
                    UserSchool.ParentsGreenPoints = UserSchool.ParentsGreenPoints + UserUpdatedGreenPoints;
                    UserSchool.UpdatedDate = DateTime.Now;
                    UserSchool.UpdatedBy = loginAdminUserId;
                    db.Repository<School>().Update(UserSchool);
                    db.Save();


                   // List<Child> lstchild = db.Repository<Child>().GetAll().Where(x => x.SchoolID == item.ID && x.UserID == UserId).ToList();

                    List<SchoolStaff> lststaff = db.Repository<SchoolStaff>().GetAll().Where(x => x.SchoolID == item.ID && x.UserID == UserId && x.IsActive == true).ToList();

                    //foreach (var items in lstchild)
                    //{
                    //    AssociationVeiwModel model = new AssociationVeiwModel();
                    //    model.SchoolID = UserSchool.ID;
                    //    model.GreenPoints = UserUpdatedGreenPoints;
                    //    model.UserID = UserId;
                    //    model.LoginAdminUserId = Convert.ToInt32(loginAdminUserId);
                    //    model.RsID = RsId;
                    //    model.Type = type;
                    //    model.ChildID = items.ID;
                    //    SaveAssociationIndividualRecords(flag, model);
                    //}


                    foreach (var items in lststaff)
                    {
                        AssociationVeiwModel model = new AssociationVeiwModel();
                        model.SchoolID = UserSchool.ID;
                        model.GreenPoints = UserUpdatedGreenPoints;
                        model.UserID = UserId;
                        model.LoginAdminUserId = Convert.ToInt32(loginAdminUserId);
                        model.RsID = RsId;
                        model.Type = type;
                        model.StaffID = items.ID;
                        SaveAssociationIndividualRecords(flag, model);
                    }
                }
            }

            if (lstBusiness.Count() > 0)
            {

                foreach (var item in lstBusiness)
                {
                    flag = "busi";
                    Business UserBusiness = db.Repository<Business>().FindById(item.ID);
                    //int? BusinessOrignalGPoints = UserBusiness.EmployeeGreenPoints - UserLastGreenPoints;
                    //int? BusinessUpdatedGPoints = BusinessOrignalGPoints + UserUpdatedGreenPoints;
                    //UserBusiness.EmployeeGreenPoints = BusinessUpdatedGPoints;
                    //db.Repository<Business>().Update(UserBusiness);
                    //db.Save();
                    //also update log table

                    //New Fixing by Ihsan. update buesiness
                    UserBusiness.EmployeeGreenPoints = UserBusiness.EmployeeGreenPoints - UserLastGreenPoints;
                    UserBusiness.EmployeeGreenPoints = UserBusiness.EmployeeGreenPoints + UserUpdatedGreenPoints;
                    UserBusiness.UpdatedDate = DateTime.Now;
                    UserBusiness.UpdatedBy = loginAdminUserId;
                    db.Repository<Business>().Update(UserBusiness);
                    db.Save();

                    List<Employment> lstEmployee = db.Repository<Employment>().GetAll().Where(x => x.BusId == item.ID && x.UserID == UserId && x.IsActive == true).ToList();

                    foreach (var items in lstEmployee)
                    {
                        AssociationVeiwModel model = new AssociationVeiwModel();
                        model.BusinessID = UserBusiness.ID;
                        model.GreenPoints = UserUpdatedGreenPoints;
                        model.UserID = UserId;
                        model.LoginAdminUserId = Convert.ToInt32(loginAdminUserId);
                        model.RsID = RsId;
                        model.Type = type;
                        model.EmployeeID = items.ID;
                        SaveAssociationIndividualRecords(flag, model);
                    }
                }
            }

            #region|Commented Code|
            //Member UserMem = db.Repository<Member>().GetAll().Where(x => x.UserID == UserId).FirstOrDefault();
            //Child UserChild = db.Repository<Child>().GetAll().Where(x => x.UserID == UserId).FirstOrDefault();
            //Employment UserEmp = db.Repository<Employment>().GetAll().Where(x => x.UserID == UserId).FirstOrDefault();

            //if (UserMem != null)
            //{
            //    Organization UserOrg = db.Repository<Organization>().GetAll().Where(x => x.UserID == UserMem.UserID).FirstOrDefault();
            //    if (UserOrg != null)
            //    {
            //        int OrgOrignalGPoints = UserOrg.EmployeeGreenPoints - UserLastGreenPoints;
            //        int OrgUpdatedGPoints = OrgOrignalGPoints + UserUpdatedGreenPoints;
            //        UserOrg.EmployeeGreenPoints = OrgUpdatedGPoints;
            //        db.Repository<Organization>().Update(UserOrg);
            //        db.Save();
            //    }
            //}

            //if (UserChild != null)
            //{
            //    School UserSchool = db.Repository<School>().GetAll().Where(x => x.UserID == UserChild.UserID).FirstOrDefault();

            //    if (UserSchool != null)
            //    {
            //        int SchoolOrignalGPoints = UserSchool.ParentsGreenPoints - UserLastGreenPoints;
            //        int SchoolUpdatedGPoints = SchoolOrignalGPoints + UserUpdatedGreenPoints;
            //        UserSchool.ParentsGreenPoints = SchoolUpdatedGPoints;
            //        db.Repository<School>().Update(UserSchool);
            //        db.Save();
            //    }
            //}

            //if (UserEmp != null)
            //{
            //    Business UserBusiness = db.Repository<Business>().GetAll().Where(x => x.UserID == UserEmp.UserID).FirstOrDefault();
            //    if (UserBusiness != null)
            //    {
            //        int? BusinessOrignalGPoints = UserBusiness.EmployeeGreenPoints - UserLastGreenPoints;
            //        int? BusinessUpdatedGPoints = BusinessOrignalGPoints + UserUpdatedGreenPoints;
            //        UserBusiness.EmployeeGreenPoints = BusinessUpdatedGPoints;
            //        db.Repository<Business>().Update(UserBusiness);
            //        db.Save();

            //    }
            //}

            #endregion
        }
        private void SaveAssociationIndividualRecords(string flag, AssociationVeiwModel mAssociation)
        {
            if (flag == "org")
            {
                OrgnizationGP_Log UserOrgLog = db.Repository<OrgnizationGP_Log>().GetAll().Where(x => x.RsID == mAssociation.RsID && x.Type == mAssociation.Type
                && (mAssociation.MemeberID != null && x.MemberID == mAssociation.MemeberID)).FirstOrDefault();
                if (UserOrgLog != null)
                {
                    UserOrgLog.GreenPoints = mAssociation.GreenPoints;
                    UserOrgLog.UserID = mAssociation.UserID;
                    UserOrgLog.UpdatedBy = mAssociation.LoginAdminUserId;
                    UserOrgLog.RsID = mAssociation.RsID;
                    UserOrgLog.Type = mAssociation.Type;
                    UserOrgLog.UpdatedDate = DateTime.Now;
                    UserOrgLog.MemberID = mAssociation.MemeberID;
                    db.Repository<OrgnizationGP_Log>().Update(UserOrgLog);
                    db.Save();
                }
                else
                {
                    OrgnizationGP_Log orgnizationGP_Log = new OrgnizationGP_Log();
                    orgnizationGP_Log.OrgID = mAssociation.OrganizationID;
                    orgnizationGP_Log.GreenPoints = mAssociation.GreenPoints;
                    orgnizationGP_Log.UserID = mAssociation.UserID;
                    orgnizationGP_Log.CreatedBy = mAssociation.LoginAdminUserId;
                    orgnizationGP_Log.RsID = mAssociation.RsID;
                    orgnizationGP_Log.Type = mAssociation.Type;
                    orgnizationGP_Log.CreatedDate = DateTime.Now;
                    orgnizationGP_Log.IsActive = true;
                    orgnizationGP_Log.MemberID = mAssociation.MemeberID;
                    db.Repository<OrgnizationGP_Log>().Insert(orgnizationGP_Log);
                    db.Save();
                }

            }
            if (flag == "sch")
            {
                SchoolGP_Log UserSchoolLog = db.Repository<SchoolGP_Log>().GetAll().Where(x => x.RsID == mAssociation.RsID && x.Type == mAssociation.Type
                && ((mAssociation.ChildID != null && x.ChildID == mAssociation.ChildID) || (mAssociation.StaffID != null && x.StaffID == mAssociation.StaffID)))
                    .FirstOrDefault();
                if (UserSchoolLog != null)
                {
                    UserSchoolLog.GreenPoints = mAssociation.GreenPoints;
                    UserSchoolLog.UserID = mAssociation.UserID;
                    UserSchoolLog.UpdatedBy = mAssociation.LoginAdminUserId;
                    UserSchoolLog.RsID = mAssociation.RsID;
                    UserSchoolLog.Type = mAssociation.Type;
                    UserSchoolLog.UpdatedDate = DateTime.Now;
                    UserSchoolLog.ChildID = mAssociation.ChildID;
                    UserSchoolLog.StaffID = mAssociation.StaffID;
                    db.Repository<SchoolGP_Log>().Update(UserSchoolLog);
                    db.Save();
                }
                else
                {
                    SchoolGP_Log schoolGP_Log = new SchoolGP_Log();
                    schoolGP_Log.SchoolID = mAssociation.SchoolID;
                    schoolGP_Log.GreenPoints = mAssociation.GreenPoints;
                    schoolGP_Log.UserID = mAssociation.UserID;
                    schoolGP_Log.CreatedBy = mAssociation.LoginAdminUserId;
                    schoolGP_Log.RsID = mAssociation.RsID;
                    schoolGP_Log.Type = mAssociation.Type;
                    schoolGP_Log.CreatedDate = DateTime.Now;
                    schoolGP_Log.IsActive = true;
                    schoolGP_Log.ChildID = mAssociation.ChildID;
                    schoolGP_Log.StaffID = mAssociation.StaffID;
                    db.Repository<SchoolGP_Log>().Insert(schoolGP_Log);
                    db.Save();
                }
            }
            if (flag == "busi")
            {
                BusinessGP_Log UserBusiLog = db.Repository<BusinessGP_Log>().GetAll().Where(x => x.RsID == mAssociation.RsID && x.Type == mAssociation.Type
                && (mAssociation.EmployeeID != null && x.EmployeeID == mAssociation.EmployeeID)).FirstOrDefault();
                if (UserBusiLog != null)
                {
                    UserBusiLog.UserID = mAssociation.UserID;
                    UserBusiLog.UpdatedBy = mAssociation.LoginAdminUserId;
                    UserBusiLog.RsID = mAssociation.RsID;
                    UserBusiLog.Type = mAssociation.Type;
                    UserBusiLog.UpdatedDate = DateTime.Now;
                    UserBusiLog.GreenPoints = mAssociation.GreenPoints;
                    UserBusiLog.EmployeeID = mAssociation.EmployeeID;
                    db.Repository<BusinessGP_Log>().Update(UserBusiLog);
                    db.Save();
                }
                else
                {
                    BusinessGP_Log businessGP_Log = new BusinessGP_Log();
                    businessGP_Log.BusID = mAssociation.BusinessID;
                    businessGP_Log.GreenPoints = mAssociation.GreenPoints;
                    businessGP_Log.UserID = mAssociation.UserID;
                    businessGP_Log.CreatedBy = mAssociation.LoginAdminUserId;
                    businessGP_Log.RsID = mAssociation.RsID;
                    businessGP_Log.Type = mAssociation.Type;
                    businessGP_Log.CreatedDate = DateTime.Now;
                    businessGP_Log.IsActive = true;
                    businessGP_Log.EmployeeID = mAssociation.EmployeeID;
                    db.Repository<BusinessGP_Log>().Insert(businessGP_Log);
                    db.Save();
                }

            }
        }
        #endregion



        public ShiftViewModel GetWorkingHours(string Shift)
        {
            WorkingHour workingHours = (from des in context.WorkingHours.ToList()
                                        where des.Shift == Shift && des.IsActive == true
                                        select des).FirstOrDefault();
            /////  @TimeZoneInfo.ConvertTimeFromUtc(timeUtc, TimeZoneInfo.Local) 

          //  workingHours.StartTime = @TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime( workingHours.StartTime), TimeZoneInfo.Local);
 
            ShiftViewModel shiftViewModel = new ShiftViewModel();
            shiftViewModel.startTimeHours = workingHours.StartTime.Value.ToString("hh");
            shiftViewModel.startTimeMinutes = workingHours.StartTime.Value.ToString("mm");
            shiftViewModel.endTimeHours = workingHours.EndTime.Value.ToString("hh");
            shiftViewModel.endTimeMinutes = workingHours.EndTime.Value.ToString("mm");
            return shiftViewModel;
        }


        public bool DeAssociateUser(int? UserId,int Id,string Type) // Here Id can be of school, Org,Business and Type is also respectively one of them
        {

            if (Type == GPNTypeEnum.School.ToString())
            {

               // var staffs = context.SchoolStaffs.Where(x => x.SchoolID == Id && x.UserID == UserId && x.IsActive == true).ToList();
                var childs = db.Repository<Child>().GetAll().Where(x => x.SchoolID == Id && x.UserID == UserId && x.IsActive == true).ToList();
                var staffs = db.Repository<SchoolStaff>().GetAll().Where(x => x.SchoolID == Id && x.UserID == UserId && x.IsActive == true).ToList();
                //////if (childs.Count > 0)
                //////{
                //////    childs = childs.Select(c => { c.IsActive = false; return c; }).ToList();
                //////    context.SaveChanges();
                //////    return true;

                //////}


                //////if (staffs.Count > 0)
                //////{
                //////    //staffs.ForEach(x => x.IsActive = false);
                //////    staffs = staffs.Select(c => { c.IsActive = false; return c; }).ToList();
                //////    context.SaveChanges();                   
                //////    return true;
                //////}
                //////else
                //////{
                //////    return false;
                //////}




                if (childs.Count > 0 || staffs.Count >0)

                {


                    foreach (var item in childs)
                    {
                        item.IsActive = false;
                        db.Repository<Child>().Update(item);
                        db.Save();
                        DeDuctGreenPoints(UserId, Id, GPNTypeEnum.School.ToString());
                    }
                    foreach (var item in staffs)
                    {
                        item.IsActive = false;
                        db.Repository<SchoolStaff>().Update(item);
                        db.Save();
                        DeDuctGreenPoints(UserId, Id, GPNTypeEnum.School.ToString());
                    }
                    return true;
                }
               
                else
                {
                    return false;
                }




            }
            else if (Type == GPNTypeEnum.Business.ToString())
            {
                 var employees = db.Repository<Employment>().GetAll().Where(x => x.BusId == Id && x.UserID == UserId && x.IsActive == true).ToList();

                if (employees.Count > 0 )
                {
                    foreach (var item in employees)
                    {
                        item.IsActive = false;
                        db.Repository<Employment>().Update(item);
                        db.Save();
                        DeDuctGreenPoints(UserId, Id, GPNTypeEnum.Business.ToString());
                    }
                   
                    return true;
                }
                else
                {
                    return false;
                }
               
            }
            else if (Type == GPNTypeEnum.Organization.ToString())
            {
                var members = db.Repository<Member>().GetAll().Where(x => x.OrgId == Id && x.UserID == UserId && x.IsActive == true).ToList();
                if(members.Count > 0)
                {
                    foreach (var item in members)
                    {
                        item.IsActive = false;
                        db.Repository<Member>().Update(item);
                        db.Save();
                        DeDuctGreenPoints(UserId, Id, GPNTypeEnum.Organization.ToString());
                    }
                  
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            else
            {
                return false;
            }
        }

        public void DeDuctGreenPoints(int? UserId,int Id, string type)
        {
            if(type == GPNTypeEnum.School.ToString())
            {
                var listOfUserSchoolLogGP = db.Repository<SchoolGP_Log>().GetAll().Where(x => x.SchoolID == Id && x.UserID == UserId).ToList();
                foreach (var schoolGPLog in listOfUserSchoolLogGP)
                {
                    db.Repository<SchoolGP_Log>().Delete(schoolGPLog);
                    db.Save();
                }               
            }
            else if (type == GPNTypeEnum.Business.ToString())
            {
                var listOfUserBusinessLogGP = db.Repository<BusinessGP_Log>().GetAll().Where(x => x.BusID == Id && x.UserID == UserId).ToList();
                foreach (var businessGPLog in listOfUserBusinessLogGP)
                {
                    db.Repository<BusinessGP_Log>().Delete(businessGPLog);
                    db.Save();
                }
            }
            else if (type == GPNTypeEnum.Organization.ToString())
            {
                var listOfUserOrgLogGP = db.Repository<OrgnizationGP_Log>().GetAll().Where(x => x.OrgID == Id && x.UserID == UserId).ToList();
                foreach (var orgGPLog in listOfUserOrgLogGP)
                {
                    db.Repository<OrgnizationGP_Log>().Delete(orgGPLog);
                    db.Save();
                }
            }
        }

        public  string GetLevelByGP(int GP)
        {
            string Level = string.Empty;
            var allGpLevels = db.Repository<GPLevel>().GetAll();
            foreach (var item in allGpLevels)
            {
                if (GP >= item.GPStart && GP <= item.GPEnd)
                {
                    Level = item.Level;
                }              
            }
            return Level;

        }


        

        public string GenerateRandomNo()
        {
            Random _random = new Random();
            return _random.Next(0, 9999).ToString("D4");
        }
        ////TimeZone Conversion common methods. UTC vs LOCAL
        //public DateTime GetLocalDateTimeFromUTC(DateTime dateTimeInUTC)
        //{
        //    return TimeZoneInfo.ConvertTimeFromUtc(dateTimeInUTC,TimeZoneInfo.Local);
        //}
        //public DateTime GetUTCDateTimeFromLocal(DateTime dateTime)
        //{
        //    return TimeZoneInfo.ConvertTimeToUtc(dateTime);
        //}
        public bool ExistsAssociations(int? UserId)
        {
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
            if(lstOrg.Count > 0 || lstBusiness.Count > 0 || lstSchool.Count > 0 || stafflstSchool.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ArsRequestCount RequestCounts()
        {
            ArsRequestCount RequestCount = new ArsRequestCount();
            //int submittedRecycles = (from rc in context.Recycles.ToList() 
            //                         join sub in context.RecycleSubItems on rc.ID equals sub.RecycleID
            //                         where (rc.StatusID == 1 && sub.IsParent == true)
            //                         select new
            //                         {
            //                             rc.ID
            //                         }).ToList().Count(); ///context.Recycles.Where(x => x.StatusID == 1).ToList().Count();
             List<object> mdlRecycles = (from rc in context.Recycles.ToList()
                                        join sub in context.RecycleSubItems on rc.ID equals sub.RecycleID
                                        join status in context.Status on rc.StatusID equals status.ID
                                        join users in context.Users on rc.UserID equals users.ID
                                        join city in context.Cities on users.CityId equals city.ID
                                        join area in context.Areas on users.AreaID equals area.ID

                                        where (rc.StatusID == 1 && sub.IsParent == true)
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
                                            /// rc.FileName,
                                            rc.CreatedDate,
                                            city.CityName,
                                            areaName = area.Name,
                                            users.Address,
                                            ///updatedDate = Convert.ToDateTime(rc.CreatedDate).ToString("MMM dd, yyyy "),
                                        }).OrderByDescending(o => o.CreatedDate).ToList<object>();
            int submittedRecycles = mdlRecycles.Count();
            int submittedRegifts = context.Regifts.Where(x => x.StatusID == 1).ToList().Count();
            int submittedReplants = context.Replants.Where(x => x.StatusID == 1).ToList().Count();
            int submittedReuses = context.Reuses.Where(x => x.StatusID == 1).ToList().Count();
            int submittedReduces = context.Reduces.Where(x => x.StatusID == 1).ToList().Count();
            int submittedRefuses = context.Refuses.Where(x => x.StatusID == 1).ToList().Count();
            int submittedReports = context.Reports.Where(x => x.StatusID == 1).ToList().Count();
            int submittedBuyBins = context.BuyBins.Where(x => x.StatusID == 1).ToList().Count();
            RequestCount.ToralReduse = submittedReduces;
            RequestCount.TotalBin = submittedBuyBins;
            RequestCount.TotalRefuse = submittedRefuses;
            RequestCount.TotalRegift = submittedRegifts;
            RequestCount.TotalReplant = submittedReplants;
            RequestCount.TotalReport = submittedReports;
            RequestCount.TotalRecycle = submittedRecycles;
            RequestCount.TotalReuse = submittedReuses;
            return RequestCount;
        }


        public int GetCompanyIdFromBusinessKey(string BusinessKey)
        {
            int ID = 0;
            var result = db.Repository<Company>().GetAll().Where(x => x.BusinessKey == BusinessKey).FirstOrDefault();
            if (result != null)
            {
                ID = result.ID;
            }
            return ID;

        }
        public string GetBusinessKeyCompanyIdFrom(int?  ID)
        {
            string BusinessKey = string.Empty;
            var result = db.Repository<Company>().GetAll().Where(x => x.ID == ID).FirstOrDefault();
            if (result != null)
            {
                BusinessKey = result.BusinessKey;
            }
            return BusinessKey;

        }

    }
}
