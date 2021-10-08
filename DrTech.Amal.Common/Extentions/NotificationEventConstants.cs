using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Amal.Common.Helpers
{
    public class NotificationEventConstants
    {
        public enum GOI
        {
      
            SMSSendToLocationAdmin = 50,
            SMSSendToDrtechAdmin = 51,


        }

        public enum Regift
        {
            EmailSendToAdminForApproval = 1,
            EmailSendToNGOOnceAdminApproval = 2,
            EmailSendToUser = 3,
            SMSSendToUser = 23,
            Updated = 45
        }

        public enum Recycle
        {
            EmailSendToAdminRecycleInfo = 4,
            EmailSendToUserCollectionTime = 5,
            SMSSentoUser = 24,
            Updated = 46
        }

        public enum RePlant
        {
            EmailSendToAdminForReplantInfo = 6,
            EmailSentToUserForGreenPoints = 7,
            SMSSendToUser = 25

        }

        public enum RequestaBin
        {
            EmailSentoAdminForServerBin = 8,
            SendSMSToUser = 27,
            Updated = 47
        }

        public enum Users
        {
            EmailSendtoUserForgotPassword = 9,
            RegistrationProcessEmailSendToAdmin = 10,
            RegistrationCompletedEmailSendToAdmin = 11,
            SendSMSToUserForEntrolment = 19,
            UpdateAfterSendToSMS = 20,
            SendVerificationCodeSMS = 21,
            SMSSendtoUserForgotPassword = 22,
            SendEmailToSchoolUserForFurtherProcess =45,
            SendEmailToOrgUserForFurtherProcess = 46,
            SendEmailToBusinessUserForFurtherProcess = 47,
          
        }

        public enum Common
        {
            EmailSendtoUserReportAProblem = 12,
            EmailSendtoAdminReportAProblem = 13,
            SMSSendtoUserForReportAProblem = 28
        }


        public enum InviteFriend
        {
            EmailSendtoFriend = 14,
            SMSSendtoFriend = 37
        }

        // For Refuse, Reduce, Report, Resue

        public enum Refuse
        {
            RefuseEmailSendtoAdmin = 15
        }

        public enum Reduce
        {
            ReduceEmailSendtoAdmin = 16
        }

        public enum Report
        {
            ReportEmailSendtoAdmin = 17,
            SendSMSToUser = 26
        }

        public enum Reuse
        {
            ReuseEmailSendtoAdmin = 18
        }

        public enum Children
        {
            SendEmailToAdmin = 29,
            SendSMSToUser = 30,
            SendProcessEmailToMember = 37,
            SendProcessEmailToEmployee = 38,
            SendProcessEmailToUser = 39,
        }

        public enum Organization
        {
            SendEmailToAdmin = 31,
            SendSMSToUser = 32
        }

        public enum NGO
        {
            SendEmailToAdmin = 33,
            SendSMSToUser = 34
        }

        public enum School
        {
            SendEmailToAdmin = 35,
            SendSMSToUser = 36
        }


        public enum PushNotification
        {
            Reduce = 1,
            Refuse = 2,
            Reuse = 3,
            Replant = 4,
            Recycle = 5,
            Regift = 6,
            Report = 7,
            General = 9,
            RegiftCollected = 10,
            RegiftDelivered = 11,
            RecycleCollected = 12,
            ReduceDeclined=13,
            RefuseDeclined=14,
            ReuseDeclined=15,
            ReplantDeclined=16,
            ReportDeclined=17,
            Bin=18,
            RegiftCollectedRedirect = 19,
            RcycleCollectedRedirect = 20,
            NotifyMemberOnSuspention =21,
            NotifyEmployeeOnSuspention = 22,
            NotifyStaffOnSuspention = 23,
            NotifyStudentOnSuspention = 24,
            RecycleDelivered = 25,
        }
       

        public enum Driver
        {
            SendtoAdminPin = 38
        }

        public enum SchoolStaff
        {
            SendEmailToAdmin = 39,
            SendSMSToUser = 40
        }
        public enum Email
        {
            SendEmailToUser =48,
             SendEmailOfPhoneConfirmation = 49
        }
        public enum SuspendUsers
        {
           
            SMSSendtoEmployee = 41,
            SMSSendtoMember = 42,
            SMSSendtoStudent = 43,
            SMSSendtoStaff = 44
        }       
    }
}
