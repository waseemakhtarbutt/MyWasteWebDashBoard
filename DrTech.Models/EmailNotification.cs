using DrTech.Models.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models
{
    public class EmailNotification: BaseModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string EmailTo { get; set; } = string.Empty;
        public string EmailCC { get; set; } = string.Empty;
        public string EmailSubject { get; set; } = string.Empty;
        public string EmailBody { get; set; } = string.Empty;
        public int Status { get; set; } = 0;
        public string ServerMessage { get; set; }
        public short? TryCount { get; set; }

    }
}
