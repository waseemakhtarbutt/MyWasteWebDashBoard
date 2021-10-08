using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models.Dropdown
{
    public class DropdownViewModel
    {
        public int Value { get; set; }
        public string Description { get; set; }
    }
    public class DropdownViewModelWithTitle
    {
        public string Title { get; set; }
        public List<DropdownViewModel> List { get; set; } = new List<DropdownViewModel>();
    }

}

