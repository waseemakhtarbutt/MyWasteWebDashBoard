using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Models.ViewModels
{
    public class UpdateStatusViewModel
    {
        public string Id { get; set; }
        public int Status { get; set; }
        public string Parent { get; set; }
        public int GreenPoints { get; set; }
    }
}
