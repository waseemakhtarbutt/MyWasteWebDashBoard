using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models
{
    public partial class NotificationEvents
    {
        [BsonId]
        public ObjectId ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string EmailTemplateSubject { get; set; } = string.Empty;
        public string EmailTemplateBody { get; set; } = string.Empty;
        public Nullable<bool> IsActive { get; set; } = true;
        public string CollectionName { get; set; } = string.Empty;

        public string EventId { get; set; } = string.Empty;

    }
}
