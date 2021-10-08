using DrTech.Models.Dropdown;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models.ViewModels
{
    public class DonationDropdownsViewModel
    {
        public DropdownViewModelWithTitle NGOs { get; set; }
        public DropdownViewModelWithTitle DonationType { get; set; }
        public DropdownViewModelWithTitle CityList { get; set; }
        public DropdownViewModelWithTitle AgeGroup { get; set; }

        public DropdownViewModelWithTitle SubType { get; set; }

        
    }
}
