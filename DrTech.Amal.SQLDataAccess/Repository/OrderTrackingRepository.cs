using DrTech.Amal.SQLDatabase;
using DrTech.Amal.SQLModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess.Repository
{
    public class OrderTrackingRepository : Repository<OrderTracking>
    {
        public OrderTrackingRepository(Amal_Entities context)
          : base(context)
        {
            dbSet = context.Set<OrderTracking>();
        }


        public List<object> GetRegiftDriverJobsByID(int DriverID)
        {
          List<object> lstregift = (from ot in context.OrderTrackings
                                      join drv in context.Drivers on ot.AssignTo equals drv.ID
                                      join reg in context.Regifts on ot.RsID equals reg.ID
                                      join user in context.Users on reg.UserID equals user.ID
                                      join vch in context.VehicleTypes on drv.VehicleID equals vch.ID
                                      where ot.AssignTo == DriverID && ot.Type == "Regift"
                                      select new
                                      {
                                          reg.Description,
                                          fullName = string.Concat(drv.FirstName, drv.LastName),
                                          vch.VehicleName,
                                          drv.RegNumber,
                                          ot.Status.StatusName,
                                          user.Longitude,
                                          user.Latitude,
                                          drv.Phone,
                                          drv.Address,
                                          drv.FileName,
                                          userName = reg.User.FullName,
                                          ot.ID
                                      }).OrderByDescending(o => o.ID).ToList<object>();


          
            return lstregift;
        }
        public List<object> GetRecycleDriverJobsByID(int DriverID)
        {
            List<object> lstrec = (from ot in context.OrderTrackings
                                   join drv in context.Drivers on ot.AssignTo equals drv.ID
                                   join rec in context.Recycles on ot.RsID equals rec.ID
                                   join user in context.Users on rec.UserID equals user.ID
                                   join subrec in context.RecycleSubItems on rec.ID equals subrec.RecycleID
                                   join vch in context.VehicleTypes on drv.VehicleID equals vch.ID
                                   where ot.AssignTo == DriverID && ot.Type == "Recycle" && subrec.IsParent == true
                                   select new
                                   {
                                       subrec.Description,
                                       fullName = string.Concat(drv.FirstName, drv.LastName),
                                       vch.VehicleName,
                                       drv.RegNumber,
                                       ot.Status.StatusName,
                                       user.Longitude,
                                       user.Latitude,
                                       drv.Phone,
                                       drv.Address,
                                       drv.FileName,
                                       userName = user.FullName,
                                       ot.ID
                                   }).OrderByDescending(o => o.ID).ToList<object>();
            return lstrec;
        }
        public List<object> GetBinDriverJobsByID(int DriverID)
        {
            List<object> lstrec = (from ot in context.OrderTrackings
                                   join drv in context.Drivers on ot.AssignTo equals drv.ID
                                   join buybin in context.BuyBins on ot.RsID equals buybin.ID
                                   join dtl in context.BinDetails on buybin.BinID equals dtl.ID
                                   join user in context.Users on buybin.UserID equals user.ID
                                   join vch in context.VehicleTypes on drv.VehicleID equals vch.ID
                                   where ot.AssignTo == DriverID && ot.Type == "Bin"
                                   select new
                                   {
                                       dtl.Description,
                                       fullName = string.Concat(drv.FirstName, drv.LastName),
                                       vch.VehicleName,
                                       drv.RegNumber,
                                       ot.Status.StatusName,
                                       user.Longitude,
                                       user.Latitude,
                                       drv.Phone,
                                       drv.Address,
                                       drv.FileName,
                                       userName = user.FullName,
                                       ot.ID
                                   }).OrderByDescending(o => o.ID).ToList<object>();



            return lstrec;
        }
        public List<object> GetDriverJobsByID(int DriverID)
        {
            List<object> lstDetails = new List<object>();

            List<object> lstregift = (from ot in context.OrderTrackings
                                      join drv in context.Drivers on ot.AssignTo equals drv.ID
                                      join reg in context.Regifts on ot.RsID equals reg.ID
                                      join user in context.Users on reg.UserID equals user.ID
                                      join vch in context.VehicleTypes on drv.VehicleID equals vch.ID
                                      where ot.AssignTo == DriverID && ot.Type == "Regift"
                                      select new
                                      {
                                          reg.Description,
                                          fullName = string.Concat(drv.FirstName, drv.LastName),
                                          vch.VehicleName,
                                          drv.RegNumber,
                                          ot.Status.StatusName,
                                          user.Longitude,
                                          user.Latitude,
                                          drv.Phone,
                                          drv.Address,
                                          drv.FileName,
                                          userName = reg.User.FullName,
                                          ot.Type,
                                          ot.ID
                                      }).OrderByDescending(o => o.ID).ToList<object>();


            List<object> lstrec = (from ot in context.OrderTrackings
                                   join drv in context.Drivers on ot.AssignTo equals drv.ID
                                   join rec in context.Recycles on ot.RsID equals rec.ID
                                   join user in context.Users on rec.UserID equals user.ID
                                   join subrec in context.RecycleSubItems on rec.ID equals subrec.RecycleID
                                   join vch in context.VehicleTypes on drv.VehicleID equals vch.ID
                                   where ot.AssignTo == DriverID && ot.Type == "Recycle" && subrec.IsParent == true
                                   select new
                                   {
                                       subrec.Description,
                                       subrec.Weight,
                                       subrec.GreenPoints,
                                       fullName = string.Concat(drv.FirstName, drv.LastName),
                                       vch.VehicleName,
                                       drv.RegNumber,
                                       ot.Status.StatusName,
                                       user.Longitude,
                                       user.Latitude,
                                       drv.Phone,
                                       drv.Address,
                                       drv.FileName,
                                       userName = user.FullName,
                                       ot.Type,
                                       ot.ID
                                   }).OrderByDescending(o => o.ID).ToList<object>();


            List<object> lstBin = (from ot in context.OrderTrackings
                                   join drv in context.Drivers on ot.AssignTo equals drv.ID
                                   join buybin in context.BuyBins on ot.RsID equals buybin.ID
                                   join dtl in context.BinDetails on buybin.BinID equals dtl.ID
                                   join user in context.Users on buybin.UserID equals user.ID
                                   join vch in context.VehicleTypes on drv.VehicleID equals vch.ID
                                   where ot.AssignTo == DriverID && ot.Type == "Bin"
                                   select new
                                   {
                                       dtl.Description,
                                       
                                       fullName = string.Concat(drv.FirstName, drv.LastName),
                                       vch.VehicleName,
                                       drv.RegNumber,
                                       ot.Status.StatusName,
                                       user.Longitude,
                                       user.Latitude,
                                       drv.Phone,
                                       drv.Address,
                                       drv.FileName,
                                       userName = user.FullName,
                                       ot.Type,
                                       ot.ID
                                   }).OrderByDescending(o => o.ID).ToList<object>();



            lstDetails.AddRange(lstregift);
            lstDetails.AddRange(lstrec);
            lstDetails.AddRange(lstBin);

            return lstDetails;
        }
        public object GetDriverTasksByID(int? DriverID, string status)
        {

            int statusID = 0;

            if (status == "open")
            {
                statusID = 9;
            }
            else if (status == "collected")
            {
                statusID = 11;
            }
            else if (status == "delivered")
            {
                statusID = 7;
            }

            List<object> lstDetails = new List<object>();

            List<object> lstregift = (from order in context.OrderTrackings
                                      join driver in context.Drivers on order.AssignTo equals driver.ID
                                      join regift in context.Regifts.Include("RegiftSubItems") on order.RsID equals regift.ID
                                      join user in context.Users on regift.UserID equals user.ID
                                      //join subRegift in context.RegiftSubItems.Include("LookupTypes") on regift.ID equals subRegift.RegiftID
                                      //join subItemName in context.LookupTypes on subRegift.TypeID equals subItemName.ID
                                      join vehicle in context.VehicleTypes on driver.VehicleID equals vehicle.ID
                                      where order.AssignTo == DriverID && order.Type == "Regift" && order.StatusID == statusID
                                      select new
                                      {
                                          regift.Description,
                                          driverName = string.Concat(driver.FirstName, driver.LastName),
                                          vehicle.VehicleName,
                                          driver.RegNumber,
                                          order.Status.StatusName,
                                          user.Longitude,
                                          user.Latitude,
                                          driver.Phone,
                                          driver.Address,
                                          driver.FileName,
                                          userName = regift.User.FullName,
                                          userAddress = regift.User.Address,
                                          userPhoneNumber = regift.User.Phone,
                                          orderNumber = order.ID,
                                          pickupTime = regift.PickupDate,
                                          order.Type,
                                          order.StatusID,
                                          regift.Quality,
                                          userID = user.ID,
                                          //subItemName = subItemName.Description,
                                          // subName = subRegift.LookupType.Description,
                                          subItems = regift.RegiftSubItems,
                                          order.ID
                                      }).OrderByDescending(o => o.ID).ToList<object>();

            List<object> lstrec = (from order in context.OrderTrackings
                                   join driver in context.Drivers on order.AssignTo equals driver.ID
                                   join recycle in context.Recycles on order.RsID equals recycle.ID
                                   join user in context.Users on recycle.UserID equals user.ID
                                   join subRecycle in context.RecycleSubItems on recycle.ID equals subRecycle.RecycleID
                                   join vehicle in context.VehicleTypes on driver.VehicleID equals vehicle.ID
                                   where order.AssignTo == DriverID && order.Type == "Recycle" && subRecycle.IsParent == true && order.StatusID == statusID
                                   select new
                                   {
                                       subRecycle.Description,
                                       driverName = string.Concat(driver.FirstName, driver.LastName),
                                       vehicle.VehicleName,
                                       driver.RegNumber,
                                       order.Status.StatusName,
                                       user.Longitude,
                                       user.Latitude,
                                       driver.Phone,
                                       driver.Address,
                                       driver.FileName,
                                       userName = user.FullName,
                                       userAddress = user.Address,
                                       userPhoneNumber = user.Phone,
                                       orderNumber = order.ID,
                                       pickupTime = recycle.CollectorDateTime,
                                       order.Type,
                                       order.StatusID,
                                       order.ID,
                                       //subItems = recycle.RecycleSubItems,
                                       userID = user.ID,

                                   }).OrderByDescending(o => o.ID).ToList<object>();

            List<object> lstBin = (from order in context.OrderTrackings
                                   join driver in context.Drivers on order.AssignTo equals driver.ID
                                   join bin in context.BuyBins on order.RsID equals bin.ID
                                   join user in context.Users on bin.UserID equals user.ID
                                   join BinDet in context.BinDetails on bin.BinID equals BinDet.ID
                                   join vehicle in context.VehicleTypes on driver.VehicleID equals vehicle.ID
                                   where order.AssignTo == DriverID && order.Type == "Bin" && order.StatusID == statusID
                                   select new
                                   {
                                       Price = BinDet.Price ,
                                       driverName = string.Concat(driver.FirstName, driver.LastName),
                                       vehicle.VehicleName,
                                       driver.RegNumber,
                                       order.Status.StatusName,
                                       driver.Phone,
                                       driver.Address,
                                       driver.FileName,
                                       order.StatusID,
                                       bin.TrackingNumber,
                                       bin.DeliveryDate, 
                                       userName = user.FullName,
                                       userAddress = user.Address,
                                       userPhoneNumber = user.Phone,
                                       latitude = user.Latitude,
                                       longitude = user.Longitude,
                                       orderNumber = order.ID,
                                       order.Type,
                                       quantity = bin.Qty,
                                       order.ID,
                                       userID = user.ID,
                                   }).OrderByDescending(o => o.ID).ToList<object>();

            Dictionary<String, List<object>> driverTasks = new Dictionary<String, List<object>>();
            driverTasks.Add("Regifts", lstregift);
            driverTasks.Add("Recycles", lstrec);
            driverTasks.Add("Bins", lstBin);

            return driverTasks;
        }
    }
}
