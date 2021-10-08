using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models.ViewModels
{
    public class NGOViewModel
    {
        public IFormFile File { get; set; }
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public string Phone { get; set; } = "";
        public int GreenPoints { get; set; } = 0;
        public string Level { get; set; } = "";
        public int EmployeeGreenPoints { get; set; } = 0;
        public string NGOParentId { get; set; } = "";
        public bool IsActive { get; set; } = false;
        public int Value { get; set; } = 0;
        public string FileName { get; set; } = "";
    }
}
