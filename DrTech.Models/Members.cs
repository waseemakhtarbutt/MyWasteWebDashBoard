using DrTech.Models.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models
{
    public class Members : BaseModel
    {
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        //public string EmpName { get; set; } = "";
        public string NGOId { get; set; } = "";
        public string Designation { get; set; } = "";
        public string Department { get; set; } = "";
        public string EmployeeID { get; set; } = "";
        public bool IsVerified { get; set; } = false;
        public bool IsActive { get; set; } = true;

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool IsCurrentlyWorking { get; set; }
    }
}
