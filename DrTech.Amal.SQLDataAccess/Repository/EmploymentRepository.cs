using DrTech.Amal.SQLDatabase;
using DrTech.Amal.SQLModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess.Repository
{
    public class EmploymentRepository : Repository<Employment>
    {
        public EmploymentRepository(Amal_Entities context)
        : base(context)
        {
            dbSet = context.Set<Employment>();
        }

       
    }
}
