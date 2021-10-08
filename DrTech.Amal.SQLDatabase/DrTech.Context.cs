﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DrTech.Amal.SQLDatabase
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using DrTech.Amal.SQLModels;

    public partial class Amal_Entities : DbContext
    {
        public Amal_Entities()
            : base("name=Amal_Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<BinDetail> BinDetails { get; set; }
        public virtual DbSet<Business> Businesses { get; set; }
        public virtual DbSet<BusinessGP_Log> BusinessGP_Log { get; set; }
        public virtual DbSet<BusinessType> BusinessTypes { get; set; }
        public virtual DbSet<BuyBin> BuyBins { get; set; }
        public virtual DbSet<BuyBinComment> BuyBinComments { get; set; }
        public virtual DbSet<Child> Children { get; set; }
        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<EmailNotification> EmailNotifications { get; set; }
        public virtual DbSet<EmailNotificationEvent> EmailNotificationEvents { get; set; }
        public virtual DbSet<Employment> Employments { get; set; }
        public virtual DbSet<ExpoPushNotificationEvent> ExpoPushNotificationEvents { get; set; }
        public virtual DbSet<GPLevel> GPLevels { get; set; }
        public virtual DbSet<GreenShop> GreenShops { get; set; }
        public virtual DbSet<LookupType> LookupTypes { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<NGONeed> NGONeeds { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<OrgnizationGP_Log> OrgnizationGP_Log { get; set; }
        public virtual DbSet<OrgType> OrgTypes { get; set; }
        public virtual DbSet<Recycle> Recycles { get; set; }
        public virtual DbSet<RecycleComment> RecycleComments { get; set; }
        public virtual DbSet<Reduce> Reduces { get; set; }
        public virtual DbSet<RefTable> RefTables { get; set; }
        public virtual DbSet<Refuse> Refuses { get; set; }
        public virtual DbSet<RegBusiness> RegBusinesses { get; set; }
        public virtual DbSet<RegiftComment> RegiftComments { get; set; }
        public virtual DbSet<RegiftSubItem> RegiftSubItems { get; set; }
        public virtual DbSet<RegOrganization> RegOrganizations { get; set; }
        public virtual DbSet<RegSchool> RegSchools { get; set; }
        public virtual DbSet<Replant> Replants { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<Reuse> Reuses { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<School> Schools { get; set; }
        public virtual DbSet<SchoolGP_Log> SchoolGP_Log { get; set; }
        public virtual DbSet<SchoolStaff> SchoolStaffs { get; set; }
        public virtual DbSet<SMSNotificationEvent> SMSNotificationEvents { get; set; }
        public virtual DbSet<SMSNotification> SMSNotifications { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<STG_Business> STG_Business { get; set; }
        public virtual DbSet<STG_Organization> STG_Organization { get; set; }
        public virtual DbSet<STG_School> STG_School { get; set; }
        public virtual DbSet<UserType> UserTypes { get; set; }
        public virtual DbSet<VehicleType> VehicleTypes { get; set; }
        public virtual DbSet<WorkingHour> WorkingHours { get; set; }
        public virtual DbSet<Regift> Regifts { get; set; }
        public virtual DbSet<OrderTracking> OrderTrackings { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<WetWasteSchedule> WetWasteSchedules { get; set; }
        public virtual DbSet<ScheduleDetail> ScheduleDetails { get; set; }
        public virtual DbSet<ContactU> ContactUs { get; set; }
        public virtual DbSet<WasteType> WasteTypes { get; set; }
        public virtual DbSet<RecycleSubItem> RecycleSubItems { get; set; }
        public virtual DbSet<RecycleSubItemsType> RecycleSubItemsTypes { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Ad> Ads { get; set; }
        public virtual DbSet<AdType> AdTypes { get; set; }
        public virtual DbSet<GCRedeem> GCRedeems { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }
        public virtual DbSet<UserPayment> UserPayments { get; set; }
        public virtual DbSet<AddWeight> AddWeights { get; set; }
        public virtual DbSet<User> Users { get; set; }
    
        public virtual ObjectResult<GetGOIChart_Result> GetGOIChart(Nullable<int> userID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetGOIChart_Result>("GetGOIChart", userIDParameter);
        }
    
        public virtual ObjectResult<GetGOIGreenPoints_Result> GetGOIGreenPoints(Nullable<int> gOI1, Nullable<int> gOI2, Nullable<int> gOI3)
        {
            var gOI1Parameter = gOI1.HasValue ?
                new ObjectParameter("GOI1", gOI1) :
                new ObjectParameter("GOI1", typeof(int));
    
            var gOI2Parameter = gOI2.HasValue ?
                new ObjectParameter("GOI2", gOI2) :
                new ObjectParameter("GOI2", typeof(int));
    
            var gOI3Parameter = gOI3.HasValue ?
                new ObjectParameter("GOI3", gOI3) :
                new ObjectParameter("GOI3", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetGOIGreenPoints_Result>("GetGOIGreenPoints", gOI1Parameter, gOI2Parameter, gOI3Parameter);
        }
    
        public virtual ObjectResult<GetGreenPointsMonthWise_Result> GetGreenPointsMonthWise(Nullable<int> userID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetGreenPointsMonthWise_Result>("GetGreenPointsMonthWise", userIDParameter);
        }
    
        public virtual ObjectResult<GetGreenPointsYearWise_Result> GetGreenPointsYearWise(Nullable<int> userID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetGreenPointsYearWise_Result>("GetGreenPointsYearWise", userIDParameter);
        }
    
        public virtual ObjectResult<GetDataForRecycleDetailChartByAdmin_Result> GetDataForRecycleDetailChartByAdmin(Nullable<int> userID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetDataForRecycleDetailChartByAdmin_Result>("GetDataForRecycleDetailChartByAdmin", userIDParameter);
        }
    
        public virtual ObjectResult<spGetDailyGreenPoints_Result> spGetDailyGreenPoints(Nullable<int> userID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetDailyGreenPoints_Result>("spGetDailyGreenPoints", userIDParameter);
        }
    
        public virtual ObjectResult<spGetWasteWeightDaily_Result> spGetWasteWeightDaily(Nullable<int> userID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetWasteWeightDaily_Result>("spGetWasteWeightDaily", userIDParameter);
        }
    }
}