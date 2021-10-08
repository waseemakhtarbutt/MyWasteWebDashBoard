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
   public class ReplantRepository : Repository<Replant>
    {
        public ReplantRepository(Amal_Entities context)
         : base(context)
        {
            dbSet = context.Set<Replant>();            
        }
      
        public List<object> GetAllReplantById(int? ID)
        {

            List<object> mdlReplant = (from rf in context.Replants
                                       join lookup in context.LookupTypes on rf.PlantID equals lookup.ID
                                       where rf.UserID == ID
                                       select new
                                       {
                                           Description = rf.Description,
                                           FileName = rf.FileName,
                                           Height = rf.Height,
                                           Id = rf.ID,
                                           Latitude = rf.Latitude,
                                           Longitude = rf.Longitude,
                                           Reminder = rf.Reminder,
                                           TreeCount = rf.TreeCount,
                                           PlantName = lookup.Name == "Others" ? rf.Description : lookup.Name,
                                           UserId = rf.UserID,
                                           CreatedDate = rf.CreatedDate,
                                           UpdatedDate = rf.UpdatedDate,
                                           StatusID = rf.StatusID,
                                           rf.GreenPoints

                                       })
                                       .ToList()
                                       .Select( x => new
                                       {
                                           x.Description,
                                           x.FileName,
                                           Height = x.Height,
                                           Id = x.Id,
                                           Latitude = x.Latitude,
                                           Longitude = x.Longitude,
                                           Reminder = x.Reminder,
                                           TreeCount = x.TreeCount,
                                           PlantName = x.PlantName,
                                           UserId = x.UserId,
                                           StatusID = x.StatusID,
                                           x.GreenPoints,
                                           CreatedDate = Convert.ToDateTime(x.CreatedDate).ToString("MMM dd, yyyy"),
                                           UpdatedDate = Utility.CheckIfDateIsNotValid (x.UpdatedDate), // Convert.ToDateTime(x.UpdatedDate).ToString("MMM dd, yyyy"),
                                           child = Array.Empty<string>()
                                       }).OrderByDescending(o => o.Id).ToList<object>();
            return mdlReplant;
        }
        public List<object> GetReplantsListByStatus(int StatusID)
        {
            List<object> mdlReplants = (from rp in context.Replants
                                        join status in context.Status on rp.StatusID equals status.ID
                                        join users in context.Users on rp.UserID equals users.ID
                                      where (StatusID > 0 && rp.StatusID == StatusID) || (StatusID == 0)
                                      select new {
                                          rp.ID,
                                          rp.Description,
                                          rp.TreeCount,
                                          rp.GreenPoints,
                                          statusDescription = status.StatusName,
                                          rp.Longitude,
                                          rp.Latitude,
                                          userId = users.ID,
                                          userName = users.FullName,
                                          rp.FileName,
                                          rp.CreatedDate
                                      })
                                      .OrderByDescending(o => o.CreatedDate).ToList<object>();

            return mdlReplants;
        }

        public List<object> GetAllReplantsList()
        {
            List<object> mdlReplants = (from rp in context.Replants.ToList()
                                        join status in context.Status on rp.StatusID equals status.ID
                                        join users in context.Users on rp.UserID equals users.ID
                                        where (status.ID == 1)
                                        select new
                                        {
                                            rp.ID,
                                            Description = rp.Description == "null" ? "" : rp.Description,
                                             //null "" rp.Description  ,
                                            rp.TreeCount,
                                            rp.GreenPoints,
                                            statusDescription = status.StatusName,
                                            rp.Longitude,
                                            rp.Latitude,
                                            userId = users.ID,
                                            userName = users.FullName,
                                            rp.FileName,
                                            rp.CreatedDate,
                                            updatedDate = Convert.ToDateTime(rp.CreatedDate).ToString("MMM dd, yyyy"),
                                        })
                                      .OrderByDescending(o => o.CreatedDate).ToList<object>();

            return mdlReplants;
        }


        public object GetReplantById(int Id)
        {
            object mdlRefuse = (from nn in context.Replants
                                where nn.ID == Id
                                select nn);
            return mdlRefuse;
        }

        public LookupType GetTypeNameWithPlantID (int PlantID)
        {
            LookupType mdlType = (from type in context.LookupTypes
                                  where type.ID == PlantID
                                  select type).FirstOrDefault();
            return mdlType;
        }
    }
}
