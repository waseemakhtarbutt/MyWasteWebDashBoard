using DrTech.Amal.Common.Enums;
using DrTech.Amal.Common.Helpers;
using DrTech.Amal.Common.ServerResponse;
using DrTech.Amal.Notifications;
using DrTech.Amal.SQLDataAccess.CustomModels;
using DrTech.Amal.SQLDataAccess.Repository;
using DrTech.Amal.SQLModels;
using DrTech.Amal.SQLServices.Auth;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
    public class GreenshopController : BaseController
    {
        [HttpGet]
        public async Task<ResponseObject<List<GreenShop>>> GetGreenshops()
        {
            try
            {
                List<GreenShop> greenShops = db.Repository<GreenShop>().GetAll().Where(x=>x.IsActive != false).ToList();

                if (greenShops.Count() == 0)
                    return ServiceResponse.SuccessReponse(greenShops, MessageEnum.GreenshopsNotFound);
                else
                    return ServiceResponse.SuccessReponse(greenShops, MessageEnum.GreenshopsFound);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<GreenShop>>(exp);
            }
        }
    }
}
