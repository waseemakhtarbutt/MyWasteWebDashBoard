using DrTech.Amal.Common.Enums;
using DrTech.Amal.Common.Helpers;
using DrTech.Amal.Common.ServerResponse;
using DrTech.Amal.SQLModels;
using DrTech.Amal.SQLServices.Auth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using static DrTech.Amal.Common.Extentions.Constants;

namespace DrTech.Amal.SQLServices.Controllers
{
    [Authorize]
    public class NGONetworkController : BaseController 
    {
        [HttpPost]
        public async Task<ResponseObject<bool>> AddNGO()
        {
            try
            {
                NGO mdlNGO = new NGO();
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                string FileName = string.Empty;
                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                FileName = await FileOpsHelper.UploadFileNew(file, ContainerName.NGO);

                mdlNGO.FileName = FileName;

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["name"]))
                    mdlNGO.Name = HttpContext.Current.Request.Form["name"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["address"]))
                    mdlNGO.Address = HttpContext.Current.Request.Form["address"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["phone"]))
                    mdlNGO.Phone = HttpContext.Current.Request.Form["phone"].ToString();
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["nGOParentId"]))
                    mdlNGO.ParentId = Convert.ToInt32( HttpContext.Current.Request.Form["nGOParentId"]);
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["nGOLevel"]))
                    mdlNGO.Level = HttpContext.Current.Request.Form["nGOLevel"].ToString();

                //foreach (var key in provider.FormData.AllKeys)
                //{
                //    foreach (var val in provider.FormData.GetValues(key))
                //    {

                //        if (key == "Name")
                //        {
                //            mdlNGO.Name = val;
                //        }

                //        else if (key == "Address")
                //        {
                //            mdlNGO.Address = val;
                //        }

                //        else if (key == "Phone")
                //        {
                //            mdlNGO.Phone = val;
                //        }
                //        else if (key == "NGOParentId")
                //        {
                //            mdlNGO.ParentId = Convert.ToInt32(val);
                //        }
                //        else if (key == "NGOLevel")
                //        {
                //            mdlNGO.Level = val;
                //        }
                //        mdlNGO.IsActive = true;
                //    }
                //}
                mdlNGO.UserID = UserID;
                mdlNGO.CreatedBy = (int)UserID;
                mdlNGO.CreatedDate = DateTime.Now;
                
                db.Repository<NGO>().Insert(mdlNGO);
                db.Save();
        
                return ServiceResponse.SuccessReponse(true, MessageEnum.NGOAdded);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<List<NGO>> GetNGODropdowns()
        {
            try
            {
                var dorpdowns = db.Repository<NGO>().GetAll().ToList();
                return ServiceResponse.SuccessReponse(dorpdowns, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<NGO>>(exp);
            }            
        }

        [HttpGet]
        public ResponseObject<List<NGO>> GetNGOSubOfficesDropdown(int ID)
        {
            try
            {
                var dorpdowns = db.Repository<NGO>().GetAll().Where( x => x.ParentId == ID || x.ParentId == null || x.ParentId == 0).OrderBy(x => x.ParentId).ToList();
                return ServiceResponse.SuccessReponse(dorpdowns, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<NGO>>(exp);
            }          
        }
    }
}
