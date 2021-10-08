using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models
{
    public partial class SMSNotificationEvents
    {
            [BsonId]
            public ObjectId ID { get; set; }
            public string SMSTemplateBody { get; set; } = string.Empty;
            public string EventId { get; set; } = string.Empty;
       
    }
}
