using DrTech.Amal.Common.Helpers;
using DrTech.Amal.SQLDataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls;
using static DrTech.Amal.Common.Extentions.Constants;

namespace DrTech.Amal.SQLServices.Controllers
{
    public class BaseController : ApiController
    {
        public ContextDB db;
        public BaseController()
        {
             db = new ContextDB();
        }
        protected string SaveFile(HttpPostedFileBase file)
        {
            //string Enviornment = AppSettingsHelper.GetEnvironmentValue(AppSettings.FILE_NAME, Constants.AppSettings.ENV);
            //string host = "http://" + Convert.ToString(HttpContext.Current.Request.UserHostAddress) + "/Images"; //@Context.Request.Path
            //string FileName = string.Empty;
            //FileUpload oFileUpload = null;

            //string fileName = "";

            //if (Enviornment == Enviornemnt.CLOUD)
            //{
            //    fileName = await FileOpsHelper.UploadFile(file);
            //}
            //else
            //{
            //    string serverUploadFolder = System.AppDomain.CurrentDomain.BaseDirectory + "Images";
            //    string filePath = serverUploadFolder;
            //    var httpRequest = HttpContext.Current.Request;
            //    string guid = Guid.NewGuid().ToString();
            //    var postedFile = httpRequest.Form.Files[0];
            //    FileName = guid + ".png";// +postedFile.FileName;
            //    if (!Directory.Exists(filePath))
            //        Directory.CreateDirectory(filePath);
            //    filePath = filePath + "/" + FileName;

            //    using (var stream = new FileStream(filePath, FileMode.Create))
            //    {
            //        postedFile.CopyTo(stream);
            //    }

            //    FileName = host + "/" + guid + ".png";

            //    fileName = FileName;
        //}
            return "";

        }

        //public string GetLoggedInUserId()
        //{
        //    var identity = (ClaimsIdentity)User.Identity;
        //    IEnumerable<Claim> claims = identity.Claims;
        //    foreach (var item in claims)
        //    {
        //        if (item.Type.ToLower() == "id")
        //        {
        //            return item.Value;
        //        }
        //    }
        //    return "";
        //}



    }
}
