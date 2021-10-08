using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models
{
    public class SMSNotifications
    {
        [BsonId]
        public ObjectId ID { get; set; }
        public string NotificationEventID { get; set; } = "";
        public string SMSTemplateText { get; set; } = "";
        public string MobileNumber { get; set; } = "";
        public string SMSText { get; set; } = "";
        public short Status { get; set; } = 0;
     
    }

   
        
    
}
