using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using static DrTech.Common.Extentions.Constants;

namespace DrTech.Models.Dropdown
{
    [BsonDiscriminator(CollectionNames.SCHOOL)]
    public class DropdownSchoolDbViewModel
    {
        public int Value { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int ParentId { get; set; } = 0;


    }
}
