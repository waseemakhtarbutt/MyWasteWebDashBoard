using DrTech.Amal.SQLDatabase;
using DrTech.Amal.SQLModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess.Repository
{
    public class ChildRepository : Repository<Child>
    {
        public ChildRepository(Amal_Entities context)
       : base(context)
        {
            dbSet = context.Set<Child>();
        }

        public Child GetChildByUserID(int UserID)
        {
            Child mdlChild = (from des in context.Children
                            where des.UserID == UserID
                              select des).FirstOrDefault();
            return mdlChild;
        }

    }
}
