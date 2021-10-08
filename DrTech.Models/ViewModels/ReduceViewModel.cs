using DrTech.Models.Common;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models
{
    public class ReduceViewModel : BaseModel
    {
        public string Id { get; set; }
        
        public int GreenPoints { get; set; }

        public int Status { get; set; }
    }
}
