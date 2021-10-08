using System;
using System.Data;

namespace DrTech.Amal.ExceptionLogger
{
    public class ExceptionLogging
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string ErrorMessage { get; set; }
        public string InnerErrorMessage { get; set; }
        public string StackTrace { get; set; }
    }
}