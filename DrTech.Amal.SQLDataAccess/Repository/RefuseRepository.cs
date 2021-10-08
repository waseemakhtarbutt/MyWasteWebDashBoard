using DrTech.Amal.Common.Helpers;
using DrTech.Amal.SQLDataAccess.CustomModels;
using DrTech.Amal.SQLDatabase;
using DrTech.Amal.SQLModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess.Repository
{
   public class RefuseRepository : Repository<Refuse>
    {
        public RefuseRepository(Amal_Entities context)
           : base(context)
        {
            dbSet = context.Set<Refuse>();
        }

        public bool AddRefuseItem(Refuse mdlRefuse)
        {
            return true;
        }

        //public List<object> GetAllRefuseItemById(int? ID)
        //{
        //    List<object> mdlRefuse = (from rf in context.Refuses
        //                     where rf.UserID == ID
        //                     select rf).OrderByDescending(o => o.CreatedDate).ToList<object>();
        //    return mdlRefuse;
        //}
        public string CheckIfDateIsNotValid(DateTime dateTime)
        {
            string resultedDate = "";
            if (dateTime == DateTime.MinValue)
            {
                resultedDate = "";
            }
            else
            {
                resultedDate = Convert.ToDateTime(dateTime).ToString("MMM dd, yyyy");
            }
            return resultedDate;
        }

        public List<object> GetAllRefuseItemById(int? ID)
        {
            var mdlRefuse = (from rd in context.Refuses
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
                                 rd.UserID,
                                 rd.IsActive,
                                 rd.StatusID,
                                 rd.UpdatedDate
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
                                         UpdatedDate = CheckIfDateIsNotValid(u.UpdatedDate), // Convert.ToDateTime(u.UpdatedDate).ToString("MMM dd, yyyy"),
                                         u.UserID,
                                         u.IsActive,
                                         u.StatusID

                                     }).ToList<object>();



            return mdlRefuse;
            // Convert.ToDateTime(u.CreatedDate).ToString("MMM dd, yyyy",,)
        }

        public List<object> GetAllRefuseItem()
        {
            //List<object> mdlRefuse = (from rf in context.Refuses                                      
            //                          select rf).OrderByDescending(o => o.CreatedDate).ToList<object>();
            //return mdlRefuse;
            List<object> mdlRefuses = (from rf in context.Refuses.ToList()
                                       join status in context.Status on rf.StatusID equals status.ID
                                       join users in context.Users on rf.UserID equals users.ID
                                       // where (StatusID > 0 && rf.StatusID == StatusID) || (StatusID == 0)
                                       where (status.ID==1)
                                       select new
                                       {
                                           rf.ID,
                                           rf.Idea,
                                           rf.GreenPoints,
                                           statusDescription = status.StatusName,
                                           rf.Longitude,
                                           rf.Latitude,
                                           userId = users.ID,
                                           userName = users.FullName,
                                           rf.FileName,
                                           rf.CreatedDate,
                                           updatedDate = Convert.ToDateTime(rf.CreatedDate).ToString("MMM dd, yyyy"),
                                       }).OrderByDescending(o => o.CreatedDate).ToList<object>();

            return mdlRefuses;
        }
        public List<object> GetRefusesListByStatus(RecycleRequest model)
        {
            List<object> response = new List<object>();
            var mdlRefuses = (from rf in context.Refuses
                                       join status in context.Status on rf.StatusID equals status.ID
                                       join users in context.Users on rf.UserID equals users.ID
                                       where (model.StatusID > 0 && rf.StatusID == model.StatusID) || (model.StatusID == 0)
                                       select new
                                       {
                                           rf.ID,
                                           rf.Idea,
                                           rf.GreenPoints,
                                           statusDescription = status.StatusName,
                                           rf.Longitude,
                                           rf.Latitude,
                                           userId = users.ID,
                                           userName = users.FullName,
                                           rf.FileName,
                                           rf.CreatedDate,                                       
                                       }).OrderByDescending(o => o.CreatedDate).ToList();
            if (model.StartDate != null && model.EndDate != null)
            {
              response = mdlRefuses.Where(x => x.CreatedDate >= Utility.GetDateFromString(model.StartDate) && x.CreatedDate <= Utility.GetDateFromString(model.EndDate)).ToList<object>();
                return response;
               // return mdlRecycles.Where(x => x.CreatedDate >= model.StartDate && x.CreatedDate <= model.EndDate).ToList();
            }
            else
            {
                response = mdlRefuses.ToList<object>();
                return response;
            }
        }

        public object GetRefuseById(int Id)
        {      
            object mdlRefuse = (from nn in context.Refuses
                                      where nn.ID == Id
                                      select nn);
            return mdlRefuse;           
        }
       


    }
}
