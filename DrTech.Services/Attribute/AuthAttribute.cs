using DrTech.Common.Enums;
using DrTech.Common.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrTech.Services.Attribute
{
    public class AuthAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly UserRoleTypeEnum[] _roles;
        public AuthAttribute(params UserRoleTypeEnum[] roles)
        {
            _roles = roles;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                // it isn't needed to set unauthorized result 
                // as the base class already requires the user to be authenticated
                // this also makes redirect to a login page work properly
                // context.Result = new UnauthorizedResult();
                return;
            }

            var claims = context.HttpContext.User.Claims;
            var roleType = "";
            foreach (var item in claims)
            {
                if (item.Type.ToLower() == "role")
                {
                    roleType = item.Value;
                }
            }
            var isExisted = (bool)_roles?.Any(p => p.GetDescription() == roleType);

            if (!isExisted)
            {
               // var dd = DrTech.Common.ServerResponse.ServiceResponse.ErrorReponse<bool>("User is not authorized!!");
                context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Unauthorized);
                return;
            }
        }
    }
}
