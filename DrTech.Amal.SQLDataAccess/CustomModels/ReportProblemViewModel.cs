using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess.CustomModels
{
    public class ReportProblemViewModel
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Problem { get; set; }

        public string Subject { get; set; }
    }
}
