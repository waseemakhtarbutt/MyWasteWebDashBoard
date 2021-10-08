using DrTech.Common.Enums;
using DrTech.Common.Extentions;
using DrTech.ExceptionLogger;
using Microsoft.AspNetCore.Http;
using System;


namespace DrTech.Common.ServerResponse
{
    public class ServiceResponse
    {
        private static ResponseObject<TObject> Response<TObject>(TObject data, string message, int code)
        {
            return new ResponseObject<TObject> { Data = data, StatusMessage = message, StatusCode = code };
        }
        
        #region Error 
        
        public static ResponseObject<TObject> ErrorReponse<TObject>(MessageEnum message = MessageEnum.DefaultErrorMessage)
        {
            return Response(default(TObject), message.GetDescription(), (int)message);
        }

        public static ResponseObject<TObject> ErrorReponse<TObject>(MessageEnum message, HttpContext httpContext, int code = StatusCodes.Status400BadRequest)
        {
            httpContext.Response.StatusCode = code;
            return Response(default(TObject), message.GetDescription(), (int)message);
        }
        public static ResponseObject<TObject> ErrorReponse<TObject>(string message)
        {
            return Response(default(TObject), message, (int)MessageEnum.DefaultErrorMessage);
        }
        public static ResponseObject<TObject> ErrorReponse<TObject>(Exception expMessage)
        {
            return Response(default(TObject), GetAllExceptionText(expMessage), (int)MessageEnum.DefaultErrorMessage);
        }

        public static ResponseObject<TObject> ErrorReponse<TObject>(MessageEnum message = MessageEnum.DefaultErrorMessage, Exception expMessage = null)
        {
            if (expMessage != null)
                return ErrorReponse<TObject>(expMessage);
            return Response(default(TObject), message.GetDescription(), (int)message);
        }

        public static ResponseObject<TObject> ErrorReponse<TObject>(MessageEnum message = MessageEnum.DefaultErrorMessage, string expMessage = null)
        {
            if (expMessage != null)
                return Response(default(TObject), expMessage, (int)MessageEnum.DefaultErrorMessage);
            return Response(default(TObject), message.GetDescription(), (int)message);
        }
        public static void LogError(Exception exp)
        {
            LoggerExtention.LogException(exp);
        }
        private static string GetAllExceptionText(Exception exp)
        {
            LogError(exp);

            string message = exp.Message;
            if (exp.InnerException == null)
                return message;
            return message + " ==>> Inner Exception: " + GetAllExceptionText(exp.InnerException);
        }
        #endregion

        #region Success

        public static ResponseObject<TObject> SuccessReponse<TObject>(TObject data, MessageEnum message = MessageEnum.DefaultSuccessMessage)
        {
            return Response(data, message.GetDescription(), (int)MessageEnum.DefaultSuccessMessage);
        }
        public static ResponseObject<TObject> SuccessReponse<TObject>(TObject data, string message = "")
        {
            return Response(data, message, (int)MessageEnum.DefaultSuccessMessage);
        }



        #endregion


    }
}
