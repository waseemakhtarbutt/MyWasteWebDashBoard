using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace DrTech.Amal.Common.Extentions
{
    public static class Constants
    {
        public static class SessionKeys
        {
            public static string SIGNUP_TEMP_DATA => "SIGN_UP";
            public static string SIGNUP_TEMP_LOGO => "SIGN_UP_LOGO";
            public static string SIGNUP_TEMP_USERTYPE => "SIGN_UP_USER_TYPE";
            public static string SIGNUP_TEMP_RETURN_URL => "RETURN_URL";
            public static string SIGNUP_TEMP_FORM_MODE => "FORM_MODE";

            //public static string GALLERY_ID => "GID";
            //public static string GALLERY_NAME => "GN";
            public static string ARTIST_ID => "AID";



            public static string USER_ID => "UID";
            public static string USER_NAME => "USER_NAME";
            public static string USER_TYPE => "USER_TYPE";
        }


        public static class GPNTypes
        {
            public static string SCHOOL => "School";
            public static string ORGANIZATION => "Organization";
            public static string Business => "Business";
        }
        public static class CollectionNames
        {
            public const string APP_USER = "applicationUsers";
            public const string Report = "Report";
            public const string RECYCLE = "Recycle";
            public const string REPLANT = "Replant";
            public const string USERS = "Users";
            public const string BUYBIN = "BuyBin";
            public const string BINDETAIL = "BinDetail";
            public const string REUSE = "Reuse";
            public const string RECIPTION = "Reciption";
            public const string Refuse = "Refuse";
            public const string NotificationEvents = "NotificationEvents";
            public const string Lookups = "Lookups";
            public const string EmailNotification = "EmailNotification";
            public const string SubTypeLookups = "SubType";
            public const string Reduce = "Reduce";
            public const string REGIFT = "Regift";
            public const string SocialMedia = "SocialMedia";
            public const string Child = "Child";
            public const string SCHOOL = "Schools";
            public const string Organization = "Organization";
            public const string Employee = "Employment";
            public const string Kids = "Kids";
            public const string NGO = "NGO";
            public const string MEMBERS = "Members";
            public const string Disclaimer = "Disclaimer";
            public const string SMSNotificationEvents = "SMSNotificationEvents";
        }



        public static class AppSettings
        {
            public static string FILE_NAME => "appsettings.json";
            public static string ENV => "ENV";

            public static string MONGO_SECTION => "MongoDbSettings";
            public static string MONGO_CONSTR => "ConnectionString";
            public static string MONGO_DB => "DatabaseName";

            public static string AZURE_SECTION => "AzureStorage";
            public static string AZURE_CONSTR => "ConnectionString";
            public static string AZURE_CONTAINER_PUBLIC => "ContainerName";

            public static string EMAIL_SECTION => "EmailSettings";
            public static string EMAIL_SECTION_DOMAIN => "Domain";
            public static string EMAIL_SECTION_PORT => "Port";
            //public static string EMAIL_SECTION_USERNAME => "UsernameEmail";
            public static string EMAIL_SECTION_FROM_NAME => "FromName";
            public static string EMAIL_SECTION_PASSWORD => "UsernamePassword";
            public static string EMAIL_SECTION_FROM_EMAIL => "FromEmail";
            public static string EMAIL_SECTION_CC_EMAIL => "CcEmail";
            public static string EMAIL_SECTION_TO_EMAIL => "ToEmail";
            public static string EMAIL_SECTION_SENT => "SendEmail";

            public static string SMS_SECTION_SENT => "SendSMS";
            public static string SECURITY_SECTION => "JWT";
            public static string SECURITY_ISSUER => "issuer";
            public static string SECURITY_KEY => "key";
            public static string SECURITY_AUDIENCE => "audience";

            public static string APP_SECTION => "App";
            public static string APP_SECTION_VERSION => "Version";

            public static string Blobe_String => "Blobe_String";

        }


        public static class ArtWorkFormats
        {
            public static List<string> IMAGE => new List<string>
            {
                ".png",
                ".jpg",
                ".jpeg",
                ".gif",
                ".tiff",
                ".exiff",
                ".bmp"
            };
        }

        public enum MessageType
        {
            Info,
            Warning,
            Error
        }
        public static class Enviornemnt
        {
            public static string DEV => "DEV";
            public static string CLOUD => "CLOUD";
        }



        public static class ContainerName
        {
        
            public const string REPORT = "report";
            public const string RECYCLE = "recycle";
            public const string REPLANT = "replant";
            public const string REUSE = "reuse";
            public const string REFUSE = "refuse";
            public const string REDUCE = "reduce";
            public const string REGIFT = "regift";
            public const string CHILD = "child";
            public const string SCHOOL = "schools";
            public const string ORGANIZATION = "organization";
            public const string Business = "business";
            public const string USER = "users";

        }

        
    }
}
