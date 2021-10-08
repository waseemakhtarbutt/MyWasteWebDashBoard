using System.ComponentModel;

namespace DrTech.Common.Enums
{
    public enum MessageEnum
    {
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


        [Description("User Credentials are not correct!!")]
        UserCredentialsAreNotCorrect = 4001,
        [Description("Provided Email Id already exists!!")]
        UserEmailAlreadyExists = 4002,
        [Description("User authorized successfully!!")]
        UserAuthorizedSuccessFully = 4003,

       

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
        


        [Description("Model Cannot be Null!!")]
        OrgModelNotNull = 40001,
        [Description("Thank you for enlisting with your office through the Green Points network.")]
        OrgAddedSuccessfully = 40002,
        [Description("ID Cannot be Null!!")]
        OrgIDCannotbeNull = 40003,
        [Description("Information Updated Successfully!!")]
        OrgInfoUpdated = 40004,
        [Description("Record Found Successfully!!")]
        OrgFoundSuccessfully = 40005,
        [Description("Record Not Found !!")]
        OrgNotFound = 40006,

        [Description("Thank you for enlisting with your NGO through the Green Points network.")]
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
        AppVersion = 80001,

    }
}
