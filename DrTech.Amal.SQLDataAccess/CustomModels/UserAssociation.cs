using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess.CustomModels
{
    public class UserAssociation
    {
        public string Type { get; set; }
        public string Name { get; set; }
    }
    public class TopGPViewModel
    {
        public string Name { get; set; }
        public int GP { get; set; }
    }
    public class UserRequestDto
    {
        public string Type { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
    public class SchoolRequestDto
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
    public class OrganizationRequestDto
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
    public class BusinesssRequestDto
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
    public class DriverRequestDto
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
