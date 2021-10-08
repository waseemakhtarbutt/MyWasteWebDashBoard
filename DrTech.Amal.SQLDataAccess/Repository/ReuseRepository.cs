using DrTech.Amal.Common.Helpers;
using DrTech.Amal.SQLDatabase;
using DrTech.Amal.SQLModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess.Repository
{
    public class ReuseRepository : Repository<Reuse>
    {
        public ReuseRepository(Amal_Entities context)
         : base(context)
        {
            dbSet = context.Set<Reuse>();
        }
        public List<object> GetReusesListByStatus(int StatusID)
        {
            List<object> mdlReuses = (from ru in context.Reuses
                                      join status in context.Status on ru.StatusID equals status.ID
                                      join users in context.Users on ru.UserID equals users.ID
                                      where (StatusID > 0 && ru.StatusID == StatusID) || (StatusID == 0)
                                      select new
                                      {   ru.ID,
                                          ru.Idea,
                                          ru.Description,
                                          ru.GreenPoints,
                                          statusDescription = status.StatusName,
                                          ru.Longitude,
                                          ru.Latitude,
                                          userId = users.ID,
                                          userName = users.FullName,
                                          ru.FileName,
                                          ru.CreatedDate
                                      }).OrderByDescending(o => o.CreatedDate).ToList<object>();

            return mdlReuses;
        }

        public List<object> GetAllReusesList()
        {
            List<object> mdlReuses = (from ru in context.Reuses.ToList()
                                      join status in context.Status on ru.StatusID equals status.ID
                                      join users in context.Users on ru.UserID equals users.ID
                                      where (status.ID == 1)
                                      select new
                                      {
                                          ru.ID,
                                          ru.Idea,
                                          ru.Description,
                                          ru.GreenPoints,
                                          statusDescription = status.StatusName,
                                          ru.Longitude,
                                          ru.Latitude,
                                          userId = users.ID,
                                          userName = users.FullName,
                                          ru.FileName,
                                          ru.CreatedDate,
                                          updatedDate = Convert.ToDateTime(ru.CreatedDate).ToString("MMM dd, yyyy"),
                                      }).OrderByDescending(o => o.CreatedDate).ToList<object>();

            return mdlReuses;
        }

        public object GetReuseById(int Id)
        {
            object mdlRefuse = (from nn in context.Reuses
                                where nn.ID == Id
                                select nn);
            return mdlRefuse;
        }
        
        public List<object> GetAllReuseItemById(int? ID)
        {
            var mdlRefuse = (from rd in context.Reuses
                             where rd.UserID == ID
                             select new
                             {
                                 rd.ID,
                                 rd.Idea,
                                 rd.GreenPoints,
                                 rd.Latitude,
                                 rd.Longitude,
                                 rd.FileName,
                                 rd.CreatedDate, //= Convert.ToDateTime(rd.CreatedDate).ToString("MMM dd, yyyy"),
                                 rd.UpdatedDate,
                                 rd.UserID,
                                 rd.IsActive,
                                 rd.StatusID
                             }).ToList().OrderByDescending(x => x.CreatedDate)
                             .Select(u => new
                             {
                                 u.ID,
                                 u.Idea,
                                 u.GreenPoints,
                                 u.Latitude,
                                 u.Longitude,
                                 u.FileName,
                                 CreatedDate = Convert.ToDateTime(u.CreatedDate).ToString("MMM dd, yyyy"),
                                 UpdatedDate = Utility.CheckIfDateIsNotValid (u.UpdatedDate), // Convert.ToDateTime(u.UpdatedDate).ToString("MMM dd, yyyy"),
                                 u.UserID,
                                 u.IsActive,
                                 u.StatusID
                             }).ToList<object>();
            return mdlRefuse;
            // Convert.ToDateTime(u.CreatedDate).ToString("MMM dd, yyyy",,)
        }
    }
}
