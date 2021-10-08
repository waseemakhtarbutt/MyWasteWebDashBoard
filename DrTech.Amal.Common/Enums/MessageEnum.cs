using System.ComponentModel;

namespace DrTech.Amal.Common.Enums
{
    public enum MessageEnum
    {
        [Description("Request submitted successfully.")]
        DefaultSubmittedSuccessMessage = 0,
        [Description("Request processed successfully.")]
        DefaultSuccessMessage = 0,
        [Description("Failure: Unknown error occured.Please try again later.")]
        DefaultErrorMessage = 1,
        [Description("User is not authorized!!")]
        DefaultUserNotAuthorized = 2,
        [Description("Parameters cannot be null!!")]
        DefaultParametersCanNotBeNull = 3,
        [Description("Please provided correct Sataus!!")]
        DefaultProvidedCorrectStatusValue = 4,

        [Description("Report Id can not be null!!")]
        ComplaintIdCannotBeNull = 1001,
        [Description("Thank you for being responsible. We would forward your report to the concerned team.")]
        ComplaintAddedSuccess = 1002,
        [Description("Report found successfully!!")]
        ComplaintGetSuccess = 1003,
        [Description("Report not found!!")]
        ComplaintNotFound = 1004,

        [Description("Thank you for submitting the issue. Our technical team would review it and get back to you as soon as possible.")]
        ProblemReportedSuccessfully = 1005,

        //BuyBin Controller 
        [Description("Bin Record not found!!")]
        BinNotFound = 2001,
        [Description("Bin Record found successfully!!")]
        BinGetSuccess = 2002,
        [Description("BuyBin Model can not be null!!")]
        BuyBinModelCannotBeNull = 2003,
        [Description("Thank you for your order. You would be contacted for delivery date and time within two business days.")]
        BinAddedSuccess = 2004,
        [Description("Bin User Id can not be null!!")]
        BinUserIdCannotBeNull = 2005,
        [Description("BinDetails View Model Can not be Null!!")]
        BinDetailsViewModelCannotBeNull = 2006,
        [Description("Bin Details Added Successfully!!")]
        BinDetailsAddedSuccess = 2007,

        //Mr Clean Controller
        [Description("Recycle Items not found!!")]
        RecycleItemsNotFound = 3001,
        [Description("Recycle  Items found successfully!!")]
        RecycleItemsGetSuccess = 3002,
        [Description("RecycleItems Model Cannot be Null!!")]
        RecycleModelNotNull = 3003,
        [Description("RecycleItems UserId Cannot be Null!!")]
        RecycleUserIdNotNull = 3004,
        [Description("Thank you for being responsible. We would contact you before sending someone over for collection.")]
        RecycleItemsAdded = 3005,
        [Description("User ID or Recycle ID cannnot be null!!")]
        UserIdRecycleIdNotNull = 3006,
        [Description("Collection Date and Time cannnot be null!!")]
        CollectionDateNotNull = 3006,


        [Description("User Credentials are not correct!!")]
        UserCredentialsAreNotCorrect = 4001,
        [Description("Provided Email Id already exists!!")]
        UserEmailAlreadyExists = 4002,
        [Description("User authorized successfully!!")]
        UserAuthorizedSuccessFully = 4003,
        [Description("Users Rs count found successfully!!")]
        GotUserRsCountSuccessfully = 4004,


        [Description("Thank you for your kind donation. We would contact you before sending someone for collection.")]
        DonationAddedSuccessfully = 5001,
        [Description("Reciption Added Successfully!!")]
        ReciptionAddedSuccessfully = 5002,
        [Description("No Record Found!!")]
        DonationNotFound = 5003,
        [Description("Regift List Found!!")]
        DonationFoundSuccessfully = 5004,
        [Description("Parameter Cannnot be null!!")]
        DonationParameterNull = 5005,
        [Description("Please provide Donate To description!!")]
        DonationDonateToDescriptionParameterNull = 5006,
        [Description("Please provide Type description!!")]
        DonationTypeDescriptionParameterNull = 5007,
        [Description("Please provide City description!!")]
        DonationCityDescriptionParameterNull = 5008,



        [Description("No Record Found!!")]
        ReplantRecordNotFound = 6005,
        [Description("Planted Tree Found Successfully!!")]
        ReplantRecordFoundSuccessfully = 6006,
        [Description("Thank you for being responsible. You would receive Green Points once your submission is reviewed.")]
        ReplantAddedSuccessfully = 6007,
        [Description("Re-Plant Parameter can not be null!!")]
        ReplantParameterCannotBeNull = 6008,
        [Description("Plant Tree Satus updated Successfully!!")]
        ReplantStatusUpdatedSuccessfully = 6008,

        [Description("Refuse Model Cannot be Null!!")]
        RefuseModelNotNull = 7001,
        [Description("Thank you for being responsible. You would receive Green Points once your submission is reviewed.")]
        RefuseAddedSuccessfully = 7002,
        [Description("RefuseItem User Id can not be null!!")]
        RefuseUserIdCannotBeNull = 7003,
        [Description("Refuse Items Cannot be found!!")]
        RefuseItemsNotFound = 7004,
        [Description("Refuse Items found successfully!!")]
        RefuseItemGetSuccess = 7005,
        [Description("Refuse Items Updated successfully!!")]
        RefuseItemUpdatedSuccess = 7006,


        [Description("Map Points not found!")]
        MapPointsNotFound = 8001,


        [Description("Email Cannot be null!!")]
        UserForgotParameterNotnull = 9001,
        [Description("SMS Sent Successfully!!")]
        PasswordSentToUserViaEmail = 9002,
        [Description("No Email Found!!")]
        EmailNotFound = 9003,
        [Description("Email List Found!!")]
        EmailListFound = 9004,
        EmailDataIsNotValid = 9005,


        [Description("Your Profile has been Updated Successfully!!")]
        UserProfileUpdated = 10001,
        [Description("User ID cannot be null!!")]
        UserIdCannotbeNull = 10002,
        [Description("User Detail found Successfully!!")]
        UserDetailFoundSuccessfully = 10003,
        [Description("User Detail not found!!")]
        UserDetailNotFoundSuccessfully = 10004,
        [Description("Please attached picture!!")]
        PictureMessage = 10005,


        [Description("Reduce Model Cannot be Null!!")]
        ReduceModelNotNull = 20001,
        [Description("Thank you for being responsible. You would receive Green Points once your submission is reviewed.")]
        ReduceAddedSuccessfully = 20002,
        [Description("ReduceItem User Id can not be null!!")]
        ReduceUserIdCannotBeNull = 20003,
        [Description("Reduce Items Cannot be found!!")]
        ReduceItemsNotFound = 20004,
        [Description("Reduce Items found successfully!!")]
        ReduceItemGetSuccess = 20005,


        [Description("Model Cannot be Null!!")]
        KidsModelNotNull = 30001,
        [Description("Thank you for enrolling your school to the Green Points network. You would be contacted within two business days for further processing.")]
        SchoolAddedSuccessfully = 30002,
        [Description("ID Cannot be Null!!")]
        KidsIDCannotbeNull = 30003,
        [Description("Information Updated Successfully!!")]
        KidsInfoUpdated = 30004,
        [Description("Record Found Successfully!!")]
        ChildFoundSuccessfully = 30005,
        [Description("Record Not Found !!")]
        ChildNotFound = 30006,
        [Description("Thank you for enlisting with your school through the Green Points network.")]
        KidsAddedSuccessfully = 30007,
        [Description("Information Updated Successfully!!")]
        SchoolInfoUpdated = 30008,
        [Description("School Suspended Successfully!!")]
        SchoolSuspended = 30009,
        [Description("Business Suspended Successfully!!")]
        BusinessSuspended = 30010,
        [Description("Organization Suspended Successfully!!")]
        OrgSuspended = 30011,
        [Description("Organization Restored Successfully!!")]
        OrgRestored = 30012,
        [Description("Business Restored Successfully!!")]
        BusinessRestored = 30013,
        [Description("School Restored Successfully!!")]
        SchoolRestored = 30014,



        [Description("Model Cannot be Null!!")]
        OrgModelNotNull = 40001,
        [Description("Thank you for enlisting with your Organization through the Green Points network. You would be contacted within two business days for further processing.")]
        OrgAddedSuccessfully = 40002,
        [Description("ID Cannot be Null!!")]
        OrgIDCannotbeNull = 40003,
        [Description("Information Updated Successfully!!")]
        OrgInfoUpdated = 40004,
        [Description("Record Found Successfully!!")]
        OrgFoundSuccessfully = 40005,
        [Description("Record Not Found !!")]
        OrgNotFound = 40006,

        [Description("Thank you for enlisting with your Business through the Green Points network. You would be contacted within two business days for further processing.")]
        NGOAdded = 40007,



        [Description("Model Cannot be Null!!")]
        EmpModelNotNull = 50001,
        [Description("Employee Information Added Successfully!!")]
        EmpAddedSuccessfully = 50002,
        [Description("ID Cannot be Null!!")]
        EmpIDCannotbeNull = 50003,
        [Description("Information Updated Successfully!!")]
        EmpInfoUpdated = 50004,
        [Description("Record Found Successfully!!")]
        EmpFoundSuccessfully = 50005,
        [Description("Record Not Found !!")]
        EmpNotFound = 50006,
        [Description("You are already associated with this NGO !!")]
        NGOEmpAlreadyAdded = 50007,
        [Description("You are already associated with this Organization !!")]
        OrgEmpAlreadyAdded = 50008,



        [Description("Thank you for being responsible. You would receive Green Points once your submission is reviewed.")]
        ReuseItemSuccssfully = 60001,
        [Description("No Record Found!!")]
        ReuseNotFound = 60002,


        [Description("Record Updated successfully")]
        SMSCodeVerify = 70001,
        [Description("Record Not Updated")]
        SMSCodeNotVerify = 70002,

        [Description("Disclaimer Text Found.")]
        DisclaimerText = 80001,



        [Description("Current Version !!")]
        AppVersion = 80025,
        
        [Description("Green Points Average Found Successfully!")]
        GPFoundSuccessfully = 80002,

        [Description("NGO Need Added Successfully!!")]
        NGONeedAddedSuccessfully = 80003,

        [Description("NGO Need Updated Successfully!!")]
        NGONeedUpdatedSuccessfully = 80004,

        [Description("NGO Needs Cannot be found!!")]
        NGONeedsNotFound = 80005,

        [Description("NGO Needs found successfully!!")]
        NGONeedsGetSuccess = 80006,

        [Description("Regift Items Cannot be found!!")]
        RegiftItemsNotFound = 80007,

        [Description("Regift Items found successfully!!")]
        RegiftItemGetSuccess = 80008,

        [Description("Report Items Cannot be found!!")]
        ReportItemsNotFound = 80009,

        [Description("Report Items found successfully!!")]
        ReportItemGetSuccess = 80010,

        [Description("User list found successfully!!")]
        UserListGetSuccess = 80011,

        [Description("User list Cannot be found!!")]
        UserListNotFound = 80012,
        
        [Description("Contact person found successfully!!")]
        ContactPersonFoundSuccessfully = 80013,

        [Description("Contact person cannot be found!!")]
        ContactPersonNotFound = 80014,

        [Description("Please pass email or phone.")]
        EmailPhoneMissingMessage = 80015,

        [Description("Replant updated successfully!!")]
        ReplantUpdatedSuccessfully = 80016,

        [Description("Recycle updated successfully!!")]
        RecycleUpdatedSuccessfully = 80017,

        [Description("Reduce updated successfully!!")]
        ReduceUpdatedSuccessfully = 80018,

        [Description("Refuse updated successfully!!")]
        RefuseUpdatedSuccessfully = 80019,

        [Description("Regift updated successfully!!")]
        RegiftUpdatedSuccessfully = 80020,

        [Description("Report updated successfully!!")]
        ReportUpdatedSuccessfully = 80021,

        [Description("Reuse updated successfully!!")]
        ReuseUpdatedSuccessfully = 80022,

        [Description("Record(s) found successfully!!")]
        RecordFoundSuccessfully = 80023,

        [Description("Record(s) not found!!")]
        RecordNotFound = 80024,

        [Description("Bins Cannot be found!!")]
        BinsNotFound = 80025,

        [Description("Bins found successfully!!")]
        BinsGetSuccess = 80026,

        [Description("Child suspended successfully.")]
        ChildSuspendSuccessfully = 80027,

        [Description("Suspended successfully.")]
        SuspendSuccessfully = 80028,

        [Description("Driver Added Successfully.")]
        DriverAdded = 90001,

        [Description("Driver Updated Successfully.")]
        DriverUpdated = 90002,

        [Description("Driver has been logged in Successfully.")]
        DriverLoggedIn = 90003,

        [Description("Your request status has been updated successfully.")]
        StatusUpdatedSuccessfully = 90004,

        [Description("Your request status did not updated successfully.")]
        StatusNotUpdated = 90005,

        [Description("Greenshops not found.")]
        GreenshopsNotFound = 90006,

        [Description("Greenshops found successfully.")]
        GreenshopsFound = 90007,

        [Description("User Registered.")]
        MyWasteRegistration = 10000,

        [Description("User Already Exist.")]
        UserAlreadyExist = 100010,

        [Description("Gotten working shift successfully")]
        WorkingShiftSuccess = 100011,
    }
}
