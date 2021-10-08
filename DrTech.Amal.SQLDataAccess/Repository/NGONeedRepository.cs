using DrTech.Amal.SQLDatabase;
using DrTech.Amal.SQLModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess.Repository
{
   public class NGONeedRepository : Repository<NGONeedRepository>
    {
        public NGONeedRepository(Amal_Entities context)
           : base(context)
        {
            dbSet = context.Set<NGONeedRepository>();
        }

        public List<object> GetNGONeedListOld(int? UserID)
        {
            List<object> mdlRefuse = (from nn in context.NGONeeds
                                          // where nn.UserID == UserID
                                      select nn).OrderByDescending(o => o.CreatedDate).ToList<object>();

            return mdlRefuse;
        }

        public List<object> GetNGONeedListbyUserID(int? UserID)
        {
            List<object> NGODonations = (from ru in context.NGONeeds
                                         where ru.IsActive == true && ru.UserID == UserID
                                         //join type in context.LookupTypes on ru.TypeID equals type.ID
                                         //join subtype in context.LookupTtypes on ru.SubTypeID equals subtype.ID
                                         select new
                                         {
                                             ru.ID,
                                             ru.Qty,
                                             ru.TypeID,
                                             ru.SubTypeID,
                                             // ru.Description,                                        
                                             typeDescription = ru.LookupType1.Name, // type.Name,
                                             type = ru.TypeID,
                                             subTypeDescription = ru.LookupType.Name,
                                             subType = ru.SubTypeID,
                                             donateTo = ru.OrgID,
                                             donateToDescription = ru.Organization.Name,
                                             cityDescription = "Lahore",
                                             city = ru.CityID,
                                             typeName = ru.LookupType1.Name,
                                             subtypeName = ru.LookupType.Name


                                         }).ToList<object>();

            return NGODonations;
        }

        public List<object> GetNGONeedList(int? UserID)
        {
            List<object> NGODonations = (from ru in context.NGONeeds
                                         where ru.IsActive==true 
                                             //join type in context.LookupTypes on ru.TypeID equals type.ID
                                             //join subtype in context.LookupTtypes on ru.SubTypeID equals subtype.ID
                                         select new
                                         {
                                             ru.ID,
                                             ru.Qty,
                                             ru.TypeID,
                                             ru.SubTypeID,
                                             // ru.Description,                                        
                                             typeDescription = ru.LookupType1.Name, // type.Name,
                                             type = ru.TypeID,
                                             subTypeDescription = ru.LookupType.Name,
                                             subType = ru.SubTypeID,
                                             donateTo = ru.OrgID,
                                             donateToDescription = ru.Organization.Name,
                                             cityDescription = "Lahore",
                                             city = ru.CityID,
                                             typeName = ru.LookupType1.Name,
                                             subtypeName = ru.LookupType.Type


                                         }).ToList<object>();

            return NGODonations;
        }

        public List<object> GetNGONeedListInActive(int? UserID)
        {
            List<object> NGODonationsInActive = (from ru in context.NGONeeds
                                                 where ru.IsActive==false
                                             //join type in context.LookupTypes on ru.TypeID equals type.ID
                                             //join subtype in context.LookupTypes on ru.SubTypeID equals subtype.ID
                                         select new
                                         {
                                             ru.ID,
                                             ru.Qty,
                                             // ru.Description,                                        
                                             TypeName = ru.LookupType1.Name, // type.Name,
                                             SubtypeName = ru.LookupType.Name,
                                         }).ToList<object>();

            return NGODonationsInActive;
        }


        public object GetNGONeedById(int? Id)
        {
            object mdlRefuse = (from ru in context.NGONeeds
                                where ru.ID == Id
                                join type in context.LookupTypes on ru.TypeID equals type.ID
                                join subtype in context.LookupTypes on ru.SubTypeID equals subtype.ID

                                select new
                                {
                                    ru.ID,
                                    ru.Qty,
                                    ru.TypeID,
                                    ru.SubTypeID,
                                    // ru.Description,                                        
                                    TypeDescription = type.Name, // type.Name,
                                    SubTypeDescription = subtype.Name,
                                }).FirstOrDefault<object>();

            return mdlRefuse;
        }

    }
}
