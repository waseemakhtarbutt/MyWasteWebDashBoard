using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Common.Attributes
{
    public class SubTypeTitleAttribute : Attribute
    {
        private readonly string _title;
        public SubTypeTitleAttribute(string msg)
        {
            _title = msg;
        }
        public virtual string Title
        {
            get { return _title; }
        }
    }
}
