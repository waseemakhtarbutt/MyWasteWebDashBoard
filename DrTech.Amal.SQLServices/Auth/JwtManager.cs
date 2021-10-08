using DrTech.Amal.SQLModels;
using JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace DrTech.Amal.SQLServices.Auth
{
    public class JwtManager
    {
        /// <summary>
        /// Create a Jwt with user information
        /// </summary>
        /// <param name="user"></param>
        /// <param name="dbUser"></param>
        /// <returns></returns>
        public static string CreateToken(User user, out object dbUser)
        {

            var unixEpoch = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var expiry = Math.Round((DateTime.UtcNow.AddMinutes(45) - unixEpoch).TotalSeconds);
            var issuedAt = Math.Round((DateTime.UtcNow - unixEpoch).TotalSeconds);
            var notBefore = Math.Round((DateTime.UtcNow.AddMonths(6) - unixEpoch).TotalSeconds);


            var payload = new Dictionary<string, object>
            {
                {"email", user.Email},
                {"userId", user.ID},
                {"role", user.RoleID},
                 {"type", user.Type},
                //{"sub", user.Id},
                //{"nbf", notBefore},
                //{"iat", issuedAt},
                //{"exp", expiry}
            };

            var secret = WebConfigurationManager.AppSettings.Get("jwtKey"); //secret key
            dbUser = new { user.Email, user.ID, user.RoleID };
            var token = JsonWebToken.Encode(payload, secret, JwtHashAlgorithm.HS256);
            return token;
        }

        public static string CreateTokenForDriver(Driver user, out object dbUser)
        {

            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var expiry = Math.Round((DateTime.UtcNow.AddMinutes(45) - unixEpoch).TotalSeconds);
            var issuedAt = Math.Round((DateTime.UtcNow - unixEpoch).TotalSeconds);
            var notBefore = Math.Round((DateTime.UtcNow.AddMonths(6) - unixEpoch).TotalSeconds);

            var payload = new Dictionary<string, object>
            {
                {"phone", user.Phone},
                {"userId", user.ID},
                {"Password", user.PIN},
            };

            var secret = WebConfigurationManager.AppSettings.Get("jwtKey"); //secret key
            dbUser = new { user.Phone, user.ID, user.PIN };
            var token = JsonWebToken.Encode(payload, secret, JwtHashAlgorithm.HS256);
            return token;
        }
    }
}