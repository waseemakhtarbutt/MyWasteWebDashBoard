using DrTech.Models.Dropdown;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models.ViewModels
{
   public  class SchoolDropdownsViewModel
    {
        public DropdownViewModelWithTitle Schools { get; set; }
        public DropdownViewModelWithTitle Branches { get; set; }
    }
}
