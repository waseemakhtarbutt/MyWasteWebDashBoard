using DrTech.Amal.Common.Enums;
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
   

    public class UserPaymentRepository : Repository<Ad>
    {
        public UserPaymentRepository(Amal_Entities context)
        : base(context)
        {
            dbSet = context.Set<Ad>();
        }
        public List<object> GetWeightList()
        {
            List<object> mdlAD = (from com in context.AddWeights.ToList()

                                  join city in context.Cities on com.CityID equals city.ID
                                  join area in context.Areas on com.AreaID equals area.ID
                                  where com.IsActive == true
                                  select new
                                  {
                                      com.ID,
                                      com.Weight,
                                      com.CityID,
                                      City = city.CityName,
                                      Area = area.Name,
                                  }).OrderByDescending(o => o.ID).ToList<object>();
            return mdlAD;
        }
        #region|Amal Ad's Functionalities|
        public List<object> GetBinDetailsList()
        {
            List<object> mdlAD = (from com in context.BinDetails.ToList()
                                  where com.IsActive == true
                                  select new
                                  {
                                      com.ID,
                                      com.FileName,
                                      com.BinName,
                                      com.Price,
                                      com.Capacity,
                                      com.Description
                                  }).OrderByDescending(o => o.ID).ToList<object>();
            return mdlAD;
        }

        public object GetBinDetailsByID(int id)
        {
            object mdlAD = (from com in context.BinDetails
            where com.IsActive == true
                                  select new
                                  {
                                      com.ID,
                                      com.FileName,
                                      com.BinName,
                                      com.Price,
                                      com.Capacity,
                                      com.Description
                                  }).Where(x =>x.ID==id).OrderByDescending(o => o.ID).FirstOrDefault();
            return mdlAD;
        }
        public List<object> GetAdList()
        {
            List<object> mdlAD = (from com in context.Ads.ToList()
                                        
                                        join city in context.Cities on com.CityID equals city.ID
                                        join area in context.Areas on com.AreaID equals area.ID
                                         where com.IsActive == true
                                        select new
                                        {
                                            com.ID,
                                            com.FileName,
                                            com.CityID,
                                            City = city.CityName,
                                            Area = area.Name,
                                            com.Description
                                        }).OrderByDescending(o => o.ID).ToList<object>();
            return mdlAD;
        }
        public List<object> GetAdListByType(string type)
        {
            List<object> mdlAdList = new List<object>();                        
                mdlAdList = (from com in context.Ads.ToList()

                                          join typ in context.AdTypes on com.AdTypeID equals typ.ID
                                          join areaaa in context.Areas on com.AreaID equals areaaa.ID
                                             join cityy in context.Cities on com.CityID equals cityy.ID
                                             where typ.Name.ToLower() == type.ToLower() && com.IsActive == true
                                          select new
                                          {
                                              com.ID,
                                              areaaa.Name,
                                             com.FileName,
                                             com.Description,
                                             cityy.CityName

                                          }).ToList<object>();
          
            
            return mdlAdList;
        }
        #endregion
        #region|User GC Redeem Functionalities|
        //public bool RedeemUserGC (GCRedeemViewModel model )
        //{
        //    GCRedeem mdlGcRedeem = new GCRedeem();
        //    mdlGcRedeem.GCRedeemed = model.GCRedeem;
        //    mdlGcRedeem.UserID = model.UserID;
        //    mdlGcRedeem.AmountGivenToUser = CalculateAmount(model.GCRedeem);
        //    mdlGcRedeem.CreatedBy = model.UserID;
        //    mdlGcRedeem.IsActive = true;
        //  context.Ads.Add()
        //    return false;
        //}
        #endregion
        #region|User Payment Functionalities|
        #endregion
        #region|Calculations|
        public decimal? CalculateAmount(decimal? GC)
        {
            return GC / 3;
        }
        #endregion

    }


}
