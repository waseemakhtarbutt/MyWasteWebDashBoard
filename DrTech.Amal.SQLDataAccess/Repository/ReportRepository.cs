using DrTech.Amal.SQLDatabase;
using DrTech.Amal.SQLModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess.Repository
{
    public class ReportRepository : Repository<Report>
    {
        public ReportRepository(Amal_Entities context)
         : base(context)
        {
            dbSet = context.Set<Report>();
        }

        public List<Report> GetReportsByUserID(int id)
        {
            List<Report> mdlReport = (from rf in context.Reports
                                      where rf.UserID == id
                                      select rf).OrderByDescending(o => o.CreatedDate).ToList<Report>();
            return mdlReport;
        }
        public List<object> GetReportsListByStatus(int StatusID)
        {
            List<object> mdlReports = (from rp in context.Reports
                                       join status in context.Status on rp.StatusID equals status.ID
                                       join users in context.Users on rp.UserID equals users.ID
                                       where (StatusID > 0 && rp.StatusID == StatusID) || (StatusID == 0)
                                       select new
                                       {
                                           rp.ID,
                                           rp.Description,
                                           rp.GreenPoints,
                                           statusDescription = status.StatusName,
                                           rp.Longitude,
                                           rp.Latitude,
                                           userId = users.ID,
                                           userName = users.FullName,
                                           rp.FileName,
                                           rp.CreatedDate
                                       }).OrderByDescending(o => o.CreatedDate).ToList<object>();

            return mdlReports;
        }

        public List<object> GetAllReportsList()
        {
            List<object> mdlReuses = (from ru in context.Reports.ToList()
                                      join status in context.Status on ru.StatusID equals status.ID
                                      join users in context.Users on ru.UserID equals users.ID
                                      where(status.ID==1)
                                      select new
                                      {
                                          ru.ID,
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

        public List<object> GetAllReportsItemById(int? ID)
        {
            var mdlReports = (from rd in context.Reports
                             where rd.UserID == ID
                             select new
                             {
                                 rd.ID,
                                 rd.Description,
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
                                 u.Description,
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
            return mdlReports;
            // Convert.ToDateTime(u.CreatedDate).ToString("MMM dd, yyyy",,)
        }

        public object GetReportById(int Id)
        {
            object mdlRefuse = (from nn in context.Reports
                                where nn.ID == Id
                                select nn);
            return mdlRefuse;
        }
    }
}
