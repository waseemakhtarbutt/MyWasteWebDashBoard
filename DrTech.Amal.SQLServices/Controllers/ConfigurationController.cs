using DrTech.Amal.Common.Enums;
using DrTech.Amal.Common.ServerResponse;
using DrTech.Amal.SQLDataAccess.CustomModels;
using DrTech.Amal.SQLModels;
using DrTech.Amal.SQLServices.Auth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;

namespace DrTech.Amal.SQLServices.Controllers
{
    public class ConfigurationController : BaseController
    {
        // GET: Configuration

        [System.Web.Http.HttpPost]
        public async Task<ResponseObject<bool>> UpdateWorkingHours(HoursViewModel workingHour)
        {
          
            try
            {
                string sTimeSpan = GetLocalDateTimeFromUTC(workingHour.StartTime).ToString("hh:mm tt");
                //TimeSpan sTimeSpan = workingHour.StartTime.ToUniversalTime().TimeOfDay;
                // TimeSpan eTimeSpan = workingHour.EndTime.TimeOfDay;     
                string eTimeSpan = GetLocalDateTimeFromUTC(workingHour.EndTime).ToString("hh:mm tt");

                WorkingHour workingTime = db.Repository<WorkingHour>().GetAll().Where(x => x.Shift == "Morning").FirstOrDefault();
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                if (workingTime != null)
                {                    
                    workingTime.StartTime = Convert.ToDateTime(sTimeSpan).TimeOfDay;
                    workingTime.EndTime = Convert.ToDateTime(eTimeSpan).TimeOfDay;//eTimeSpan;
                    workingTime.UpdatedBy = UserID;
                    workingTime.IsActive = true;
                    workingTime.UpdatedDate = DateTime.Now;
                    db.Repository<WorkingHour>().Update(workingTime);
                    db.Save();
                    return ServiceResponse.SuccessReponse(true, MessageEnum.DefaultSuccessMessage);
                }
                else
                {
                    WorkingHour wh =new WorkingHour() ;
                    workingTime.StartTime = Convert.ToDateTime(sTimeSpan).TimeOfDay;
                    workingTime.EndTime = Convert.ToDateTime(sTimeSpan).TimeOfDay;//eTimeSpan;
                    wh.UpdatedBy = UserID;
                    wh.IsActive = true;
                    wh.Shift = "Morning";
                    wh.CreatedDate = DateTime.Now;
                    db.Repository<WorkingHour>().Insert(wh);
                    db.Save();
                    return ServiceResponse.SuccessReponse(true, MessageEnum.DefaultErrorMessage);
                }
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
        public DateTime GetLocalDateTimeFromUTC(DateTime dateTimeInUTC)
        {
            // TimeZone curTimeZone = TimeZone.CurrentTimeZone;
            TimeZoneInfo pakZone = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
            DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(dateTimeInUTC, pakZone);
            return easternTime;
            //  return TimeZoneInfo.ConvertTimeFromUtc(dateTimeInUTC,TimeZoneInfo.ConvertTimeBySystemTimeZoneId();
        }
        [HttpGet]
        public ResponseObject<List<GPLevel>> GetGPNLevelsList()
        {
            try
            {
                List<GPLevel> gpnLevels = db.Repository<GPLevel>().GetAll().ToList();
                return ServiceResponse.SuccessReponse(gpnLevels, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<GPLevel>>(exp);
            }
        }
        
        [HttpGet]
        public ResponseObject<GPLevel> GetGPNLevelById(int Id=0)
        {
            try
            {GPLevel gpnLevel = db.Repository<GPLevel>().FindById(Id);
                return ServiceResponse.SuccessReponse(gpnLevel, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<GPLevel>(exp);
            }
        }

        [System.Web.Http.HttpPost]
        public ResponseObject<bool> SaveDefaultGreenPoints(RefTable refTable)
        {
            try
            {
                RefTable gpnLevel = db.Repository<RefTable>().GetAll().Where(x => x.ToDate == null).FirstOrDefault();
                gpnLevel.GreenPointValue = refTable.GreenPointValue;
                db.Repository<RefTable>().Update(gpnLevel);
                db.Save();
                return ServiceResponse.SuccessReponse(true, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [System.Web.Http.HttpPost]
        public async Task<ResponseObject<bool>> AddEditGPNLevel(GPLevel model)
        {
            try
            {

                GPLevel mdlGPLevel = new GPLevel();
                int? UserID = JwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
                if (model.ID == 0)
                {
                    mdlGPLevel.GPStart = model.GPStart;
                    mdlGPLevel.GPEnd = model.GPEnd;
                    mdlGPLevel.Level = model.Level;
                    mdlGPLevel.CreatedDate = DateTime.Now;
                    mdlGPLevel.IsActive = true;
                    mdlGPLevel.CreatedBy = (int)UserID;
                    db.Repository<GPLevel>().Insert(mdlGPLevel);
                    db.Save();
                    return ServiceResponse.SuccessReponse(true, MessageEnum.DefaultSuccessMessage);
                }
                else
                {
                    GPLevel mdlGPLevelUpadate = db.Repository<GPLevel>().FindById(model.ID);
                    mdlGPLevelUpadate.GPStart = model.GPStart;
                    mdlGPLevelUpadate.GPEnd = model.GPEnd;
                    mdlGPLevelUpadate.Level = model.Level;
                    mdlGPLevelUpadate.UpdatedBy = UserID;
                    mdlGPLevelUpadate.UpdatedDate = DateTime.Now;
                    db.Repository<GPLevel>().Update(mdlGPLevelUpadate);
                    db.Save();
                    return ServiceResponse.SuccessReponse(true, MessageEnum.NGONeedUpdatedSuccessfully);
                }
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<List<WorkingHour>> GetWorkingHoursList()
        {
            try
            {
                List<WorkingHour> workingHourslist = db.Repository<WorkingHour>().GetAll().ToList();
                return ServiceResponse.SuccessReponse(workingHourslist, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<WorkingHour>>(exp);
            }
        }

        [HttpGet]
        public ResponseObject<List<RefTable>> GetDefaultGreenPointsList()
        {
            try
            {
                List<RefTable> configuredGPlist = db.Repository<RefTable>().GetAll().ToList();
                return ServiceResponse.SuccessReponse(configuredGPlist, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<RefTable>>(exp);
            }
        }
        [HttpGet]
        public ResponseObject<RefTable> GetMyWasteGreenPoint()
        {
            try
            {
                RefTable configuredGPlist = db.Repository<RefTable>().GetAll().Where(x=>x.Type== "MyWaste").FirstOrDefault();
                return ServiceResponse.SuccessReponse(configuredGPlist, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<RefTable>(exp);
            }
        }
    }
}