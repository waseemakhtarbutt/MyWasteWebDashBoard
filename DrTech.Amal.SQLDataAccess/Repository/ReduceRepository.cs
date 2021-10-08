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
   public class ReduceRepository : Repository<Reduce>
    {
        public ReduceRepository(Amal_Entities context)
          : base(context)
        {
            dbSet = context.Set<Reduce>();
        }
      

        public List<object> GetAllReduceItem(int? ID)
        {
            var mdlReduce = (from rd in context.Reduces
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
                                         UpdatedDate = Utility.CheckIfDateIsNotValid(u.UpdatedDate), //    Convert.ToDateTime(u.UpdatedDate).ToString("MMM dd, yyyy"),
                                         u.UserID,
                                         u.IsActive,
                                         u.StatusID

                                     }).ToList<object>();

                                      

            return mdlReduce;
           // Convert.ToDateTime(u.CreatedDate).ToString("MMM dd, yyyy",,)
        }
        public List<object> GetReducesListByStatus(int StatusID)
        {
            List<object> mdlReduces = (from rd in context.Reduces
                                       join status in context.Status on rd.StatusID equals status.ID
                                       join users in context.Users on rd.UserID equals users.ID
                                       where (StatusID > 0 && rd.StatusID == StatusID) || (StatusID == 0)
                                       select new
                                       {
                                           rd.ID,
                                           rd.Idea,
                                           rd.GreenPoints,
                                           statusDescription = status.StatusName,
                                           rd.Longitude,
                                           rd.Latitude,
                                           userId = users.ID,
                                           userName = users.FullName,
                                           rd.FileName,
                                           rd.CreatedDate
                                       }).OrderByDescending(o => o.CreatedDate).ToList<object>();

            return mdlReduces;
        }
        public List<object> GetAllReducesList()
        {
            List<object> mdlReduces = (from ru in context.Reduces.ToList()
                                      join status in context.Status on ru.StatusID equals status.ID
                                      join users in context.Users on ru.UserID equals users.ID
                                      where (status.ID == 1)
                                       select new
                                      {
                                          ru.ID,
                                          ru.Idea,
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

            return mdlReduces;
        }
        public object GetReduceById(int Id)
        {
            object mdlRefuse = (from nn in context.Reduces
                                where nn.ID == Id
                                select nn);
            return mdlRefuse;
        }
    }
}
