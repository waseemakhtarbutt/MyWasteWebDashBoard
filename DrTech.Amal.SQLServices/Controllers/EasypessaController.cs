using DrTech.Amal.Common.EasyPaaisa;
using DrTech.Amal.Common.Enums;
using DrTech.Amal.Common.ServerResponse;
using DrTech.Amal.SQLServices.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DrTech.Amal.SQLServices.Controllers
{
    public class EasypessaController : BaseController
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<ResponseObject<EasypessaModel>> EasyPessaPayment([FromBody]EasypessaModel model)
        {
            try
            {
                if (model == null)
                return ServiceResponse.ErrorReponse<EasypessaModel>(MessageEnum.DefaultErrorMessage);
                if (model.paymentMethod == "CC_PAYMENT_METHOD")
                {
                    string hashRequest = " ";
                    string hashKey = "NQMXXTL0FVZ23ADG";
                    string storeId = "44353";
                    string postBackURL = "https://easypaystg.easypaisa.com.pk/easypay/Confirm.jsf";
                    string orderRefNum = "1008";
                    string expiryDate = "20190721 112300";
                    int autoRedirect = 0;
                    string paymentMethod = "CC_PAYMENT_METHOD";
                    model.storeId = storeId;
                    model.postBackURL = postBackURL;
                    model.orderRefNum = orderRefNum;
                    model.expiryDate = expiryDate;
                    model.autoRedirect = autoRedirect;
                    model.paymentMethod = paymentMethod;
                    NameValueCollection paramMap = new NameValueCollection();
                    paramMap["amount"] = model.amount;
                    paramMap["autoRedirect"] = model.autoRedirect.ToString();
                    paramMap["emailAddr"] = model.emailAddr;
                    paramMap["expiryDate"] = model.expiryDate;
                    paramMap["mobileNum"] = model.mobileNum;
                    paramMap["orderRefNum"] = model.orderRefNum;
                    paramMap["paymentMethod"] = model.paymentMethod;
                    paramMap["postBackURL"] = model.postBackURL;
                    paramMap["storeId"] = model.storeId;
                    string Requestparameters = "amount=" + model.amount + "&autoRedirect=" + 0 + "&emailAddr=" + model.emailAddr + "&expiryDate=" + model.expiryDate + "&mobileNum=" + model.mobileNum + "&orderRefNum=" + model.orderRefNum + "&paymentMethod=" + model.paymentMethod + "&postBackURL=" + model.postBackURL + "&storeId=" + model.storeId;// Encript Query String Parameters
                    Cryptography Crypt = new Cryptography();
                    string RequestparametersEncripted = "";
                    RequestparametersEncripted = (string)Crypt.Crypt(CryptType.ENCRYPT, CryptTechnique.RIJ, Requestparameters, hashKey);
                    hashRequest = RequestparametersEncripted;
                    model.merchantHashedReq = hashRequest;
                   // return ServiceResponse.SuccessReponse(model, MessageEnum.DefaultSuccessMessage);
                }
                if (model.paymentMethod == "MA_PAYMENT_METHOD")
                {
                    string hashRequest = " ";
                    string hashKey = "NQMXXTL0FVZ23ADG";
                    string storeId = "44353";
                    string postBackURL = "https://easypaystg.easypaisa.com.pk/easypay/Confirm.jsf";
                    string orderRefNum = "1008";
                    string expiryDate = "20190721 112300";
                    int autoRedirect = 0;
                    string paymentMethod = "CC_PAYMENT_METHOD";
                    model.storeId = storeId;
                    model.postBackURL = postBackURL;
                    model.orderRefNum = orderRefNum;
                    model.expiryDate = expiryDate;
                    model.autoRedirect = autoRedirect;
                    model.paymentMethod = paymentMethod;
                    NameValueCollection paramMap = new NameValueCollection();
                    paramMap["amount"] = model.amount;
                    paramMap["autoRedirect"] = model.autoRedirect.ToString();
                    paramMap["emailAddr"] = model.emailAddr;
                    paramMap["expiryDate"] = model.expiryDate;
                    paramMap["mobileNum"] = model.mobileNum;
                    paramMap["orderRefNum"] = model.orderRefNum;
                    paramMap["paymentMethod"] = model.paymentMethod;
                    paramMap["postBackURL"] = model.postBackURL;
                    paramMap["storeId"] = model.storeId;
                    string Requestparameters = "amount=" + model.amount + "&autoRedirect=" + 0 + "&emailAddr=" + model.emailAddr + "&expiryDate=" + model.expiryDate + "&mobileNum=" + model.mobileNum + "&orderRefNum=" + model.orderRefNum + "&paymentMethod=" + model.paymentMethod + "&postBackURL=" + model.postBackURL + "&storeId=" + model.storeId;// Encript Query String Parameters
                    Cryptography Crypt = new Cryptography();
                    string RequestparametersEncripted = "";
                    RequestparametersEncripted = (string)Crypt.Crypt(CryptType.ENCRYPT, CryptTechnique.RIJ, Requestparameters, hashKey);
                    hashRequest = RequestparametersEncripted;
                    model.merchantHashedReq = hashRequest;
                }
                return ServiceResponse.SuccessReponse(model, MessageEnum.DefaultSuccessMessage);

            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<EasypessaModel>(exp);
            }
        }
    }
}
