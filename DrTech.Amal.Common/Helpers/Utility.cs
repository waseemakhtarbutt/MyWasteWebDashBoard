using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utf8Json.Formatters;

namespace DrTech.Amal.Common.Helpers
{
    public static class Utility
    {
        public static string GetFormattedDateTime(DateTime _DateTime)
        {
            return GetFormattedDate(_DateTime) + " " + GetFormattedTime(_DateTime);
        }
        public static DateTime GetParsedDates(string date)
        {
           // DateTime parsedDate = DateTime.Now.ToUniversalTime();
            //DateTime parsedDate = Convert.ToDateTime(date, System.Globalization.CultureInfo.GetCultureInfo("ur-PK").DateTimeFormat);
            DateTime parsedDate = DateTime.ParseExact(date, "dd/MM/yyyy hh:mm", CultureInfo.InvariantCulture);
            return parsedDate;
        }
        public static DateTime GetParsedDate(string date)
        {
            DateTime parsedDate=  DateTime.Now.ToUniversalTime();
            //DateTime parsedDate = Convert.ToDateTime(date, System.Globalization.CultureInfo.GetCultureInfo("ur-PK").DateTimeFormat);
            //DateTime parsedDate = DateTime.ParseExact(date, "dd/MM/yyyy hh:mm", CultureInfo.InvariantCulture);
            return parsedDate;
        }
        public static string GetFormattedDate(DateTime date)
        {
            //string dateFormat = ReadConfiguration("DateFormat");
            //if (string.IsNullOrEmpty(dateFormat))
            string dateFormat = "dd-MMM-yyyy";

            string _date = date.ToString(dateFormat);

            return _date;
        }
        public static DateTime GetLocalDateTimeFromUTC(DateTime dateTimeInUTC)
        {
            DateTime dateTimeInUTC1 = Convert.ToDateTime(dateTimeInUTC);
            TimeZoneInfo pakZone = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
            DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(dateTimeInUTC1, pakZone);
            return easternTime;
            //  return TimeZoneInfo.ConvertTimeFromUtc(dateTimeInUTC,TimeZoneInfo.ConvertTimeBySystemTimeZoneId();
        }
        public static string GetFormattedTime(DateTime date)
        {
            string timeFormat = ReadConfiguration("TimeFormat");
            if (string.IsNullOrEmpty(timeFormat))
                timeFormat = "hh:mm tt";

            string _time = date.ToString(timeFormat);

            return _time;
        }

        public static string ReadConfiguration(string Key)
        {
            string value = string.Empty;
            if (ConfigurationManager.AppSettings.Count > 0 && ConfigurationManager.AppSettings[Key] != null)
            {
                value = ConfigurationManager.AppSettings[Key];
            }
            return value;
        }

        public static string GetFormattedDateTime(string date)
        {
            date = Convert.ToDateTime(date).ToString("dd-MM-yyyy") + " " + Convert.ToDateTime(date).ToString("hh:mm tt");
            return date;
        }

        public static string GetLevelByGP1(decimal GP)
        {
            string Level = string.Empty;

            if (GP > 0 && GP <= 500)
                Level = "Captain";
            else if (GP > 500 && GP <= 1000)
                Level = "Major";
            else if (GP > 1000 && GP <= 5000)
                Level = "General";
            else if (GP > 5000)
                Level = "Commander";
            else
                Level = "Captain";

            return Level;
        }
        public static string GetLevelByGP (int GP)
        {
            string Level = string.Empty;

            if (GP > 0 && GP <= 500)
                Level = "Captain";
            else if (GP > 500 && GP <= 1000)
                Level = "Major";
            else if (GP > 1000 && GP <= 5000)
                Level = "General";
            else if (GP > 5000)
                Level = "Commander";
            else
                Level = "Captain";

                return Level;
        }

        public static string CheckIfDateIsNotValid(DateTime dateTime)
        {
            string resultedDate = "";
            if (dateTime == DateTime.MinValue)
            {
                resultedDate = "";
            }
            else
            {
                resultedDate = Convert.ToDateTime(dateTime).ToString("MMM dd, yyyy");
            }
            return resultedDate;
        }

        public static DateTime GetDateFromString(string date)
        {
            DateTime dt = Convert.ToDateTime(date);
            return dt;
        }
        public static DateTime GetDateFromStrings(string date)
        {
            DateTime dt = Convert.ToDateTime(date);
            return dt;
        }
    }

}