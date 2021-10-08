using DrTech.Models.Common;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models
{
    public class RefuseViewModel : BaseModel
    {
        public string Id { get; set; }
        public int GreenPoints { get; set; } = 0;
        public int Status { get; set; } = 0;
    }
}
