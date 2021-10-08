using DrTech.Models.Common;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models
{
    public class Organization : BaseModel
    {
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public string OrgnizationName { get; set; } = "";

        public string OrgAddress { get; set; } = "";
        public string OrgPhone { get; set; } = "";
        public int OrgGreenPoints { get; set; } = 0;

        public string Level { get; set; } = "";
        public int EmployeeGreenPoints { get; set; } = 0;
        public string OrgParentId { get; set; } = "";

        public bool IsActive { get; set; } = false;

        public int Value { get; set; } = 0;

        public string FileName { get; set; } = "";


        [BsonIgnore]
        public IFormFile File { get; set; }
    }
}
