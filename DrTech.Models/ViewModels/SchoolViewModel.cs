using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models.ViewModels
{
    public class SchoolViewModel
    {
        public IFormFile File { get; set; }

        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public string Phone { get; set; } = "";
        public int GreenPoints { get; set; } = 0;

        public int ParentsGreenPoints { get; set; } = 0;
        public string ParentId { get; set; } = "";

        public bool IsActive { get; set; } = false;

        public int Value { get; set; } = 0;

        public string Level { get; set; } = "";

        public string Email { get; set; } = "";

        public string Branch { get; set; } = "";

        public string ContactPerson { get; set; } = "";
    }
}
