using DrTech.Common.Extentions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models.Dropdown
{
    public class SubTypeDropdownDbViewModel
    {
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public int ParentId { get; set; } = 0;
        //public List<DropdownDbViewModel> SubType { get; set; }
        public int Value { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
    }

    public class SubTypeDropdownViewModelWithTitle
    {
        public string Title { get; set; }
        public List<SubTypeDropdownDbViewModel> List { get; set; } = new List<SubTypeDropdownDbViewModel>();
    }
}
