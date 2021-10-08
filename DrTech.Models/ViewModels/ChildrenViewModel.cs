using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models.ViewModels
{
    public class ChildrenViewModel
    {
        public IFormFile File { get; set; }
        public string Name { get; set; } = "";
        public string SchoolId { get; set; } = "";
        public string SectionName { get; set; } = "";
        public string ClassName { get; set; } = "";
        public string RollNo { get; set; } = "";
        public bool IsVerified { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public int GreenPoints { get; set; } = 0;
        public string UserId { get; set; } = "";

        public string FileName { get; set; } = "";
    }
}
