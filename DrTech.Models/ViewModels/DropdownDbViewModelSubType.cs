using DrTech.Models.Dropdown;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using static DrTech.Common.Extentions.Constants;

namespace DrTech.Models.ViewModels
{
    public class DropdownDbViewModelSubType
    {
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public int Value { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }

        public int ParentId { get; set; } = 0;

        [BsonElement(CollectionNames.SubTypeLookups)]
        public List<DropdownDbViewModelSubType> SubTypes { get; set; } = new List<DropdownDbViewModelSubType>();
    }
}
