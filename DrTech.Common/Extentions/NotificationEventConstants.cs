using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Common.Helpers
{
    public class NotificationEventConstants
    {
        public enum Regift
        {
            EmailSendToAdminForApproval = 1,
            EmailSendToNGOOnceAdminApproval = 2,
            EmailSendToUser = 3,
            SMSSendToUser=23
        }

        public enum Recycle
        {
            EmailSendToAdminRecycleInfo = 4,
            EmailSendToUserCollectionTime = 5,
            SMSSentoUser=24
        }

        public enum RePlant
        {
            EmailSendToAdminForReplantInfo = 6,
            EmailSentToUserForGreenPoints = 7,
            SMSSendToUser=25

        }

        public enum RequestaBin
        {
            EmailSentoAdminForServerBin = 8,
            SendSMSToUser= 27
        }

        public enum Users
        {
            EmailSendtoUserForgotPassword = 9,
            RegistrationProcessEmailSendToAdmin = 10,
            RegistrationCompletedEmailSendToAdmin = 11,
            SendSMSToUserForEntrolment= 19,
            UpdateAfterSendToSMS = 20,
            SendVerificationCodeSMS = 21,
            SMSSendtoUserForgotPassword = 22,
        }

        public enum Common
        {
            EmailSendtoUserReportAProblem = 12,
            EmailSendtoAdminReportAProblem = 13,
            SMSSendtoUserForReportAProblem = 28
        }


        public enum InviteFriend
        {
            EmailSendtoFriend = 14
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
            SendSMSToUser = 30
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

    }
}
