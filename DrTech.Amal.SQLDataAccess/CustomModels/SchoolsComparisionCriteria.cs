using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess.CustomModels
{
  public  class SchoolsComparisionCriteria
    {
        public int City { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public List<int> ShoolId { get; set; }

        public  string Type { get; set; }

    }
    public class BranchRequest
    {
        public int Id { get; set; }
        public bool All { get; set; }

    }
}
