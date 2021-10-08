using DrTech.Amal.Common.Enums;
using DrTech.Amal.Common.Helpers;
using DrTech.Amal.Common.ServerResponse;
using DrTech.Amal.SQLModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;


namespace DrTech.Amal.SQLServices.Controllers
{
    //[Authorize]
    //public class EasyPaisaController : BaseController
    //{
    //    [HttpGet]
    //    public string Get()
    //    {
    //        HttpContext context = HttpContext.Current;

    //        var url = "https://easypay.easypaisa.com.pk/easypay/Index.jsf";
    //        context.Response.Clear();
    //        var sb = new System.Text.StringBuilder();
    //        sb.Append("<html>");
    //        sb.AppendFormat("<body onload='document.forms[0].submit()'>");
    //        sb.AppendFormat("<form action='{0}' method='post'>", url);
    //        sb.AppendFormat("<input type='hidden' name='storeId' value='{0}'>", "8858");
    //        sb.AppendFormat("<input type='hidden' name='amount' value='{0}'>", "10");
    //        sb.AppendFormat("<input type='hidden' name='postBackURL' value='{0}'>", "https://localhost:44344/Home/Test1");
    //        sb.AppendFormat("<input type='hidden' name='orderRefNum' value='{0}'>", "2100");
    //        sb.AppendFormat("<input type='hidden' name='expiryDate' value='{0}'>", "20190606 201521");
    //        sb.AppendFormat("<input type='hidden' name='merchantHashedReq' value='{0}'>", "V578SV96OTM9E9SD");
    //        sb.AppendFormat("<input type='hidden' name='autoRedirect' value='{0}'>", "1");
    //        sb.AppendFormat("<input type='hidden' name='paymentMethod' value='{0}'>", "CC_PAYMENT_METHOD");
    //        sb.AppendFormat("<input type='hidden' name='emailAddr' value='{0}'>", "Nabeelislam500@gmail.com");
    //        sb.AppendFormat("<input type='hidden' name='mobileNum' value='{0}'>", "03124678345");
    //        sb.Append("</form>");
    //        sb.Append("</body>");
    //        sb.Append("</html>");
    //        context.Response.Write(sb.ToString());
    //        return "Good";
    //    }

    //    [HttpGet]
    //    public string GetResponse()
    //    {
    //        HttpContext context = HttpContext.Current;

    //        if (context.Request.QueryString.Count > 0)
    //        {
    //            var urls = "https://easypay.easypaisa.com.pk/easypay/Confirm.jsf";

    //            context.Response.Clear();
    //            var s = new System.Text.StringBuilder();
    //            s.Append("<html>");
    //            s.AppendFormat("<body onload='document.forms[0].submit()'>");
    //            s.AppendFormat("<form action='{0}' method='post'>", urls);
    //            s.AppendFormat("<input type='hidden' name='auth_token' value='{0}'>", context.Request.QueryString["auth_token"].ToString());
    //            s.AppendFormat("<input type='hidden' name='postBackURL' value='{0}'>", "https://amalforlife.com/");
    //            s.Append("</form>");
    //            s.Append("</body>");
    //            s.Append("</html>");
    //            context.Response.Write(s.ToString());
    //            return "ok";
    //        }
    //        return "NO";
    //    }

    //    public async System.Threading.Tasks.Task UpdateAPi()
    //    {
    //        var url = "https://easypay.easypaisa.com.pk/easypay/Index.jsf";
    //        Response.Clear();
    //        var sb = new System.Text.StringBuilder();
    //        sb.Append("<html>");
    //        sb.AppendFormat("<body onload='document.forms[0].submit()'>");
    //        sb.AppendFormat("<form action='{0}' method='post'>", url);
    //        sb.AppendFormat("<input type='hidden' name='storeId' value='{0}'>", "8858");
    //        sb.AppendFormat("<input type='hidden' name='amount' value='{0}'>", "10");
    //        sb.AppendFormat("<input type='hidden' name='postBackURL' value='{0}'>", "https://localhost:44344/Home/Test1");
    //        sb.AppendFormat("<input type='hidden' name='orderRefNum' value='{0}'>", "2100");
    //        sb.AppendFormat("<input type='hidden' name='expiryDate' value='{0}'>", "20190606 201521");
    //        sb.AppendFormat("<input type='hidden' name='merchantHashedReq' value='{0}'>", "V578SV96OTM9E9SD");
    //        sb.AppendFormat("<input type='hidden' name='autoRedirect' value='{0}'>", "1");
    //        sb.AppendFormat("<input type='hidden' name='paymentMethod' value='{0}'>", "_PAYMENT_METHOD");
    //        sb.AppendFormat("<input type='hidden' name='emailAddr' value='{0}'>", "Nabeelislam500@gmail.com");
    //        sb.AppendFormat("<input type='hidden' name='mobileNum' value='{0}'>", "03124678345");
    //        sb.Append("</form>");
    //        sb.Append("</body>");
    //        sb.Append("</html>");
    //        Response.Write(sb.ToString());
    //    }
    //    public async System.Threading.Tasks.Task TestAsync()
    //    {
    //        if (Request.QueryString.Count > 0)
    //        {
    //            var urls = "https://easypay.easypaisa.com.pk/easypay/Confirm.jsf";

    //            Response.Clear();
    //            var s = new System.Text.StringBuilder();
    //            s.Append("<html>");
    //            s.AppendFormat("<body onload='document.forms[0].submit()'>");
    //            s.AppendFormat("<form action='{0}' method='post'>", urls);
    //            s.AppendFormat("<input type='hidden' name='auth_token' value='{0}'>", Request.QueryString["auth_token"].ToString());
    //            s.AppendFormat("<input type='hidden' name='postBackURL' value='{0}'>", "https://localhost:44344/");
    //            s.Append("</form>");
    //            s.Append("</body>");
    //            s.Append("</html>");
    //            Response.Write(s.ToString());
    //        }

    //    }
    //}
}
