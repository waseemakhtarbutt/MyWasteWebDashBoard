using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Amal.Common.Attributes
{
    public class TypeTitleAttribute : Attribute
    {
        private readonly string _title;
        public TypeTitleAttribute(string msg)
        {
            _title = msg;
        }
        public virtual string Title
        {
            get { return _title; }
        }
    }
}
