using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models.ViewModels
{
    public class OrganizationViewModel
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";

        public string Address { get; set; } = "";
        public string Phone { get; set; } = "";
        public int GreenPoints { get; set; } = 0;

        public string Level { get; set; } = "";
        public int EmployeeGreenPoints { get; set; } = 0;
        public string ParentId { get; set; } = "";

        public bool IsActive { get; set; } = false;

        public int Value { get; set; } = 0;

        public string FileName { get; set; } = "";

    }
}
