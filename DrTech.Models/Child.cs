using DrTech.Common.Enums;
using DrTech.Models.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models
{
    public class Child : BaseModel
    {
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public string Name { get; set; } = "";
        public string SchoolId { get; set; } = "";
        public string  SectionName { get; set; } = "";
        public string ClassName { get; set; } = "";
        public string RollNo { get; set; } = "";
        public bool IsVerified { get; set; } = false;
        public bool  IsActive { get; set; } = true;
        public int GreenPoints { get; set; } = 0;
        public string UserId { get; set; } = "";
        public string FileName { get; set; } = "";
      //  public ChildTypeEnum ChildType { get; set; } = ChildTypeEnum.Son;

       public string Gender { get; set; } = "";
    }
}
