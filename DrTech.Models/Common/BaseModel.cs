using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models.Common
{
    public class BaseModel
    {
       // [JsonIgnore]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

     //   [JsonIgnore]
        public string UpdatedAt { get; set; } = DateTime.Now.ToString();
    }
}
