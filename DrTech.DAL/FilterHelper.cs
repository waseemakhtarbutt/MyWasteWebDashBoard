using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrTech.DAL
{
    public class FilterHelper
    {
        
        public FilterHelper(string Field = "", string Value = "", string Operator = "EQ")
        {
            this.Field = Field;
            this.Value = Value;
            this.Operator = Operator;
        }

        public string Field { get; set; }
        public string Value { get; set; }
        public string Operator { get; set; }
    }
}
