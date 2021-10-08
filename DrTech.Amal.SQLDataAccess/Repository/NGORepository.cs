using DrTech.Amal.SQLDatabase;
using DrTech.Amal.SQLModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess.Repository
{
    public class NGORepository : Repository<Business>
    {
        public NGORepository(Amal_Entities context)
        : base(context)
        {
            dbSet = context.Set<Business>();
        }

        public List<Business> GetEmployeeNGOByUserID(int? UserID)
        {
            List<Business> mdlBusiness = (from des in context.Employments
                                                  join sch in context.Businesses on des.BusId equals sch.ID
                                                  where des.UserID == UserID
                                                  select sch).Distinct().ToList();
            return mdlBusiness;
        }
    }
}
