using DrTech.Amal.Common.EasyPaaisa;
using DrTech.Amal.SQLDataAccess;
using DrTech.Amal.SQLDataAccess.CustomModels;
using DrTech.Amal.SQLServices.Models;
using DrTech.Amal.SQLModels;
using DrTech.Amal.SQLServices.Auth;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using DrTech.Amal.Common.Helpers;

namespace DrTech.Amal.SQLServices.Controllers
{
    public class HomeController : Controller
    {
                 
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        [System.Web.Http.HttpPost]
        public ActionResult EasyPaaisashop([FromBody]PaymentViewModel Payment) //string Mobile, string Email, decimal amount, string PaymentMethod) bbh nnnnnnnnnnnn 
        {
            var urlBuilder =
             new System.UriBuilder(Request.Url.AbsoluteUri)
             {
                 Path = Url.Content("~/Home/PaymentStatus"),
                 Query = null,
             };
            
          //  urlBuilder
            //UpdateAPi("03338628764", "Nabeelislam500@gmail.com", "2", "CC_PAYMENT_METHOD");
            Session["Payment"] = Payment;
            var Keys = "NQMXXTL0FVZ23ADG";
            //var url = "https://easypay.easypaisa.com.pk/tpg";
            string hashRequest = " ";
            string hashKey = "NQMXXTL0FVZ23ADG";
            string storeId = "44353";
            string amount = Payment.amount;
           // string postBackURL = urls;
            string orderRefNum = "abc123";
            string expiryDate = "20201231%20111111";
            //string paymentMethod = PaymentMethod;
            //string emailAddr = Email;
            // string mobileNum = Mobile;
            string postbackurl = "http://localhost:64331/home/PaymentStatus";
            DateTime dateTime = DateTime.UtcNow;
            var Requestparameters = "amount="+amount+"&orderRefNum=abc123&paymentMethod=InitialRequest&postBackURL=" + urlBuilder + "&storeId=" + storeId + "&timeStamp="+string.Format("{0:s}", Utility.GetLocalDateTimeFromUTC(dateTime))+"";
            Cryptography Crypt = new Cryptography();
            string RequestparametersEncripted = "";
            RequestparametersEncripted = (string)Crypt.Crypt(CryptType.ENCRYPT, CryptTechnique.RIJ, Requestparameters, hashKey);
            hashRequest = HttpUtility.UrlEncode(RequestparametersEncripted);// Encript Query String Parameters
            string Urls = "https://easypay.easypaisa.com.pk/tpg/?storeId=" + storeId + "&orderId=" + orderRefNum + "&transactionAmount="+amount+"&mobileAccountNo=&emailAddress=&transactionType=InitialRequest&tokenExpiry=&bankIdentificationNumber=&merchantPaymentMethod=&postBackURL="+ urlBuilder + "&signature=&encryptedHashRequest=" + hashRequest + "";
           // string url = HttpUtility.UrlEncode(Urls);
          
            return Redirect(Urls);
        }

        public void UpdateAPi(string Mobile, string Email, string amounts, string PaymentMethod)
        {
            var urlBuilder = "http://localhost:4200/";
          

            //Uri uri = urlBuilder.Uri;
            string urls = urlBuilder.ToString();
            // localhost
            var Keys = "NQMXXTL0FVZ23ADG";
            var url = "https://easypay.easypaisa.com.pk/tpg";
            string hashRequest = " ";
            string hashKey = "NQMXXTL0FVZ23ADG";
            string storeId = "44353";
            string amount = amounts;
            string postBackURL = urls;
            string orderRefNum = "6";
            string expiryDate = "20201231%20111111";
            string paymentMethod = PaymentMethod;
            string emailAddr = Email;
            string mobileNum = Mobile;
            NameValueCollection paramMap = new NameValueCollection();
            paramMap["storeId"] = storeId;//
            paramMap["orderId"] = orderRefNum;//
            paramMap["transactionAmount"] = amount;//
            paramMap["mobileAccountNo"] = mobileNum;//
            paramMap["emailAddress"] = emailAddr;//
            paramMap["transactionType"] = "InitialRequest";
            paramMap["tokenExpiry"] = hashKey;
            paramMap["merchantPaymentMethod"] = paymentMethod;
            paramMap["postBackURL"] = postBackURL;//
            //string Requestparameters = "amount=" + amount + "&emailAddress=" + emailAddr + "&timeStamp=" + "2020-07-15T18:03:00" + 
            //                            "&paymentMethod=" + "InitialRequest" + "&expiryDate=" + expiryDate + "&mobileNum=" + mobileNum +
            //                            "&orderId=" + orderRefNum + "&merchantPaymentMethod=" + paymentMethod + "&postBackURL=" + postBackURL
            //                            + "&storeId=" + storeId;// Encript Query String Parameters
            var Requestparameters = "amount=1.0&orderRefNum=abc123&paymentMethod=InitialRequest&storeId="+storeId+"&timeStamp=2021-04-30T12:06:00";
            Cryptography Crypt = new Cryptography();
            string RequestparametersEncripted = "";
            RequestparametersEncripted = (string)Crypt.Crypt(CryptType.ENCRYPT, CryptTechnique.RIJ, Requestparameters, hashKey);
            hashRequest = RequestparametersEncripted;
            string Urls="https://easypay.easypaisa.com.pk/tpg/?storeId="+storeId+"&orderId="+orderRefNum+"&transactionAmount=1.0&mobileAccountNo=&emailAddress=&transactionType=InitialRequest&tokenExpiry=&bankIdentificationNumber=&merchantPaymentMethod=&postBackURL=&signature=&encryptedHashRequest=+"+ RequestparametersEncripted + "";

            //var sb = new System.Text.StringBuilder();
            //sb.Append("<html>");
            //sb.AppendFormat("<body onload='document.forms[0].submit()'>");
            //sb.AppendFormat("<form action='{0}' method='get'>", url);
            //sb.AppendFormat("<input type='hidden' name='storeId' value='{0}'>", storeId);
            //sb.AppendFormat("<input type='hidden' name='orderId' value='{0}'>", orderRefNum);
            //sb.AppendFormat("<input type='hidden' name='transactionAmount' value='{0}'>", amount);
            //sb.AppendFormat("<input type='hidden' name='mobileAccountNo' value='{0}'>", mobileNum);
            //sb.AppendFormat("<input type='hidden' name='emailAddress' value='{0}'>", emailAddr);
            //sb.AppendFormat("<input type='hidden' name='transactionType' value='{0}'>", "InitialRequest");
            //sb.AppendFormat("<input type='hidden' name='tokenExpiry' value='{0}'>", "20201231%20111111");
            //sb.AppendFormat("<input type='hidden' name='bankIdentificationNumber' value='{0}'>", "20201231%20111111");
            //sb.AppendFormat("<input type='hidden' name='encryptedHashRequest' value='{0}'>", hashRequest);
            //sb.AppendFormat("<input type='hidden' name='merchantPaymentMethod' value='{0}'>", paymentMethod);
            //sb.AppendFormat("<input type='hidden' name='postBackURL' value='{0}'>", postBackURL);
            //sb.AppendFormat("<input type='hidden' name='signature' value='{0}'>", "Nabeel");
            //sb.Append("</form>");
            //sb.Append("</body>");
            //sb.Append("</html>");
            //Response.Write(sb.ToString());
            var sb = new System.Text.StringBuilder();
            sb.Append("<html>");
            sb.AppendFormat("<body onload='document.forms[0].submit()'>");
            sb.AppendFormat("<form action='{0}' method='get'>", Urls);
            sb.Append("</form>");
            sb.Append("</body>");
            sb.Append("</html>");
            Response.Write(sb.ToString());
            var abc = HttpContext.Request.Files.Count;

        }
        public void UpdateAPiS(string Mobile, string Email, string amounts, string PaymentMethod)
        {
            var urlBuilder =
            new System.UriBuilder(Request.Url.AbsoluteUri)
                {
                    Path = Url.Content("~/Home/PostBack"),
                    Query = null,
                };

            Uri uri = urlBuilder.Uri;
            string urls = urlBuilder.ToString();
            // localhost
            var Keys = "NQMXXTL0FVZ23ADG";
            var url = "https://easypay.easypaisa.com.pk/easypay/Index.jsf";
            string hashRequest = " ";
            string hashKey = "NQMXXTL0FVZ23ADG";
            string storeId = "44353";
            string amount = amounts;
            // string postBackURL = "https://easypaystg.easypaisa.com.pk/easypay/Confirm.jsf";: https://easypaystg.easypaisa.com.pk/easypay/Index.jsf
            string postBackURL = urls;
            string orderRefNum = "1008";
            string expiryDate = "20201230 112300";
            int autoRedirect = 0;
            string paymentMethod = PaymentMethod;
            string emailAddr = Email;
            string mobileNum = Mobile;
            NameValueCollection paramMap = new NameValueCollection();
            paramMap["amount"] = amount;
            paramMap["autoRedirect"] = autoRedirect.ToString();
            paramMap["emailAddr"] = emailAddr;
            paramMap["expiryDate"] = expiryDate;
            paramMap["mobileNum"] = mobileNum;
            paramMap["orderRefNum"] = orderRefNum;
            paramMap["paymentMethod"] = paymentMethod;
            paramMap["postBackURL"] = postBackURL;
            paramMap["storeId"] = storeId;
            string Requestparameters = "amount=" + amount + "&autoRedirect=" + 0 + "&emailAddr=" + emailAddr + "&expiryDate=" + expiryDate + "&mobileNum=" + mobileNum + "&orderRefNum=" + orderRefNum + "&paymentMethod=" + paymentMethod + "&postBackURL=" + postBackURL + "&storeId=" + storeId;// Encript Query String Parameters
            Cryptography Crypt = new Cryptography();
            string RequestparametersEncripted = "";
            RequestparametersEncripted = (string)Crypt.Crypt(CryptType.ENCRYPT, CryptTechnique.RIJ, Requestparameters, hashKey);
            hashRequest = RequestparametersEncripted;
            var sb = new System.Text.StringBuilder();
            sb.Append("<html>");
            sb.AppendFormat("<body onload='document.forms[0].submit()'>");
            sb.AppendFormat("<form action='{0}' method='post'>", url);
            sb.AppendFormat("<input type='hidden' name='storeId' value='{0}'>", storeId);
            sb.AppendFormat("<input type='hidden' name='amount' value='{0}'>", amount);
            sb.AppendFormat("<input type='hidden' name='postBackURL' value='{0}'>", postBackURL);
            sb.AppendFormat("<input type='hidden' name='orderRefNum' value='{0}'>", orderRefNum);
            sb.AppendFormat("<input type='hidden' name='expiryDate' value='{0}'>", expiryDate);
            sb.AppendFormat("<input type='hidden' name='merchantHashedReq' value='{0}'>", hashRequest);
            sb.AppendFormat("<input type='hidden' name='autoRedirect' value='{0}'>", autoRedirect);
            sb.AppendFormat("<input type='hidden' name='paymentMethod' value='{0}'>", paymentMethod);
            sb.AppendFormat("<input type='hidden' name='emailAddr' value='{0}'>", emailAddr);
            sb.AppendFormat("<input type='hidden' name='mobileNum' value='{0}'>", mobileNum);
            sb.Append("</form>");
            sb.Append("</body>");
            sb.Append("</html>");
            Response.Write(sb.ToString());
            var abc = HttpContext.Request.Files.Count;

        }
        [System.Web.Http.HttpGet]
        public ActionResult PostBack()
        {
            TestAsync();
            return View();
        }

        public void TestAsync()
        {
            if (Request.QueryString.Count > 0)
            {

                var urlBuilder =
               new System.UriBuilder(Request.Url.AbsoluteUri)
               {
                   Path = Url.Content("~/Home/PaymentStatus"),
                   Query = null,
               };

                Uri StstusUrl = urlBuilder.Uri;
                var urls = "https://easypay.easypaisa.com.pk/easypay/Confirm.jsf";

                Response.Clear();
                var s = new System.Text.StringBuilder();
                s.Append("<html>");
                s.AppendFormat("<body onload='document.forms[0].submit()'>");
                s.AppendFormat("<form action='{0}' method='post'>", urls);
                s.AppendFormat("<input type='hidden' name='auth_token' value='{0}'>", Request.QueryString["auth_token"].ToString());
                s.AppendFormat("<input type='hidden' name='postBackURL' value='{0}'>", StstusUrl);
                s.Append("</form>");
                s.Append("</body>");
                s.Append("</html>");
                Response.Write(s.ToString());
            }
            else
            { 
            
            
            }

        }
        [System.Web.Http.AllowAnonymous]
        [System.             Web.Http.HttpGet]
        public ActionResult PaymentStatus(string orderRefNumber,string paymentMethod ,string transactionRefNumber, string status ,string description)
        {
            if (status == "0000") { 
            var Payment = Session["Payment"] as PaymentViewModel;
            BuyBinOrderWithPayment BuyBinOrderWithPayment = new BuyBinOrderWithPayment();
            BuyBinOrderWithPayment.UserPayment = new UserPayment();
            BuyBinOrderWithPayment.Price = Payment.price;
            BuyBinOrderWithPayment.deductedFromWallet = Payment.deductedFromWallet;
            BuyBinOrderWithPayment.PaidAmount = Payment.paidAmount;
            BuyBinOrderWithPayment.paymentMethodID = Payment.paymentMethodID;
            BuyBinOrderWithPayment.UserPayment.transactionNumber = transactionRefNumber  ?? "";
            //BuyBinOrderWithPayment.UserPayment.batchNumber = batchNumber ?? "";
            //BuyBinOrderWithPayment.UserPayment.authorizeId = authorizeId ?? "";
            BuyBinOrderWithPayment.UserPayment.orderRefNumber = orderRefNumber ?? "";
            BuyBinOrderWithPayment.UserPayment.IsSuccess = "Success";
            BuyBinOrderWithPayment.BinID = Payment.binId;
            BuyBinOrderWithPayment.Qty = Payment.qty;
            BuyBinController buyBin = new BuyBinController();
            var Result = buyBin.RequestForNewBin(BuyBinOrderWithPayment);
            var Test = Result.Result.StatusCode;
            var Message = Result.Result.StatusMessage + "," + status + "," + transactionRefNumber + "," + Payment + "," + orderRefNumber;
            ViewBag.Title = "Thanks for payments ";
            ViewBag.Message = Message;
            return RedirectToAction("Success");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpGet]
        public ActionResult Error()
        {
            ViewBag.Title = "Error";
            return View();

        }

        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpGet]
        public ActionResult Success()
        {
            ViewBag.Title = "Success";
            return View();

        }
    }
}
