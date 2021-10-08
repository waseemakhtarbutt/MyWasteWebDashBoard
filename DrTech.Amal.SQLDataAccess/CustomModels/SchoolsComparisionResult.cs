using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess.CustomModels
{
    public class SchoolsComparisionResult
    {
        public SchoolsComparisionResult()
        {
            Series = new List<Records>();
        }
        public string Name { get; set; }    
        public List<Records> Series { get; set; }
    }
 
}
