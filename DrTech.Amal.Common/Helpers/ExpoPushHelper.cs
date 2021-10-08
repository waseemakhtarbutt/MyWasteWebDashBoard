using DrTech.Amal.SQLModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.Common.Helpers
{
    public class ExpoPushHelper
    {
        public static dynamic SendPushNotification(string ExpoToken, ExpoPushNotificationEvent mdlTemplate)
        {
            dynamic body = new
            {
                to = ExpoToken,
                title = mdlTemplate.Title,
                body = mdlTemplate.Body,
                sound = "default",
                data = new
                {

                    navigation = new { mdlTemplate.NavigateTo, mdlTemplate.AmalID },
                    title = mdlTemplate.Title,
                    body = mdlTemplate.Body,
                   // amalID = mdlTemplate.AmalID
                }
            };
            string response = null;

//          dynamic body1 = new 
//                {
//              startDateTime = "2019-05-13T00:00:00Z",

//              endDateTime = "2019-05-13T23:59:59Z",

//            timeOffset =  480,

//            userId = "1",

//            historyDataFilterSec = "1",

//             vehicles = new { 
//                          vehicleName= "abcd", 
//                          deviceId= "8832569498"
//                }
            
   

//};
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("accept", "application/json");
                client.Headers.Add("accept-encoding", "gzip, deflate");
                client.Headers.Add("Content-Type", "application/json");
                response = client.UploadString("https://exp.host/--/api/v2/push/send", JsonExtensions.SerializeToJson(body));
            }
            var json = JsonExtensions.DeserializeFromJson<dynamic>(response);
            return json;
        }


        public static int SendBatchNotification(List<User> lstUser, ExpoPushNotificationEvent mdlTemplate)
        {
            List<dynamic> lst = new List<dynamic>();

            int count = 0;
            foreach (var item in lstUser)
            {
                if (!string.IsNullOrEmpty(item.DeviceToken))
                {
                    dynamic body = new
                    {
                        to = item.DeviceToken,
                        title = mdlTemplate.Title,
                        body = mdlTemplate.Body,
                        sound = "default",
                        data = new
                        {
                            navigateTo = mdlTemplate.NavigateTo,
                            title = mdlTemplate.Title,
                            body = mdlTemplate.Body,
                        }
                    };
                    lst.Add(body);
                    count++;
                }
                if (count == 100)
                {
                    
                    count = 0;
                    string JSONString = string.Empty;
                    JSONString = JsonConvert.SerializeObject(lst);

                    string response = null;
                    using (WebClient client = new WebClient())
                    {
                        client.Headers.Add("accept", "application/json");
                        client.Headers.Add("accept-encoding", "gzip, deflate");
                        client.Headers.Add("Content-Type", "application/json");
                        response = client.UploadString("https://exp.host/--/api/v2/push/send", JsonExtensions.SerializeToJson(lst));
                    }
                    lst = null;
                    var json = JsonExtensions.DeserializeFromJson<dynamic>(response);
                }
                
            }

            string JSONStrings = string.Empty;
            JSONStrings = JsonConvert.SerializeObject(lst);

            string responses = null;
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("accept", "application/json");
                client.Headers.Add("accept-encoding", "gzip, deflate");
                client.Headers.Add("Content-Type", "application/json");
                responses = client.UploadString("https://exp.host/--/api/v2/push/send", JsonExtensions.SerializeToJson(lst));
                var jsons = JsonExtensions.DeserializeFromJson<dynamic>(responses);
            }
            

            return 1;
            
        }


    }
}
