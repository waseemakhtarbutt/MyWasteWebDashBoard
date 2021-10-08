using DrTech.Amal.Common.Enums;
using DrTech.Amal.Common.Helpers;
using DrTech.Amal.SQLDataAccess.CustomModels;
using DrTech.Amal.SQLDatabase;
using DrTech.Amal.SQLModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace DrTech.Amal.SQLDataAccess.Repository
{
    public class RegiftRepository : Repository<Regift>
    {
        public RegiftRepository(Amal_Entities context)
          : base(context)
        {
            dbSet = context.Set<Regift>();
        }
        public List<object> GetDonations(int? userID)
        {
            var reGift = (from rg in context.Regifts
                          join sub in context.RegiftSubItems on rg.ID equals sub.RegiftID into st
                          from stype in st.DefaultIfEmpty()
                          join ot in context.OrderTrackings on rg.ID equals ot.RsID
                          //join lokup in context.LookupTypes on stype.TypeID equals lokup.ID 
                          //join lokupsub in context.LookupTypes on stype.SubTypeID equals lokupsub.ID
                          //join lk in context.LookupTypes on rg.TypeID equals lk.ID
                          //join sl in context.LookupTypes on lk.ID equals sl.ParentID
                          //join dn in context.LookupTypes on rg.DonateToID equals dn.ID
                          //join st in context.Status on rg.StatusID equals st.ID
                          where (rg.UserID == userID && ot.Type == "Regift" && stype.IsParent == true)
                          select new
                          {
                              ID = rg.ID,
                              Description = rg.Description,
                              Longitude = rg.Longitude,
                              Latitude = rg.Latitude,
                              FileName = rg.FileName,
                              StatusName = rg.Status.StatusName,
                              GreenPoints = rg.GreenPoints, //,

                              TypeDescription = stype.LookupType1.Name,
                              SubTypeDescription = stype.LookupType.Name,
                              SubTypeName = stype.LookupType.Type,
                              DonateToDescription = rg.Organization.Name,
                              StatusID = rg.StatusID,
                              CreatedDate =rg.CreatedDate,
                              PickupDate = rg.PickupDate,
                              UpdatedDate = rg.UpdatedDate,
                              ot.CollectedPendingConfirmation,
                              ot.DeliveredPendingConfirmation,
                              RecieptFileName = ot.FileNameTakenByDriver,
                              ot.CollectedDate,
                              ot.DeliveredDate,
                              //TypeName = lk.Name,
                              //SubTypeName = sl.Name,
                              //DonationName = dn.Name
                          }).ToList().OrderByDescending(x => x.CreatedDate)
                             .Select(u => new {
                                 ID = u.ID,
                                 Description = u.Description,
                                 Longitude = u.Longitude,
                                 Latitude = u.Latitude,
                                 FileName = u.FileName,
                                 StatusName = u.StatusName,
                                 GreenPoints = subitemscalculate(u.ID), //,
                                 TypeDescription = u.TypeDescription,
                                 SubTypeDescription = u.SubTypeDescription,
                                 SubTypeName = u.SubTypeName,
                                 DonateToDescription = u.DonateToDescription,
                                 StatusID = u.StatusID,
                                 CreatedDate = Convert.ToDateTime(u.CreatedDate).ToString("MMM dd, yyyy"),
                                 UpdatedDate = Utility.CheckIfDateIsNotValid(u.UpdatedDate), // Convert.ToDateTime(u.UpdatedDate).ToString("MMM dd, yyyy"),
                                 PickupDate = Convert.ToDateTime(u.PickupDate).ToString("MMM dd, yyyy"),
                                 CollectedPendingConfirmation = u.CollectedPendingConfirmation,
                                 DeliveredPendingConfirmation =  u.DeliveredPendingConfirmation,
                                 RecieptFileName = u.RecieptFileName,
                                 CollectedDate = u.CollectedDate == null ? null : Convert.ToDateTime(u.CollectedDate).ToString("MMM dd, yyyy"),
                                 DeliveredDate = u.DeliveredDate == null ? null : Convert.ToDateTime(u.DeliveredDate).ToString("MMM dd, yyyy"),
                                 //CollectedDate = Convert.ToDateTime(u.CollectedDate).ToString("MMM dd, yyyy"),
                                 //DeliveredDate = Convert.ToDateTime(u.DeliveredDate).ToString("MMM dd, yyyy"),

                             })
                         .ToList<object>();

            return reGift;

        } 
        public List<object> GetRegiftsListByStatus(int StatusID)
        {
            List<object> mdlRegifts = (from rg in context.Regifts
                                       join sub in context.RegiftSubItems on rg.ID equals sub.RegiftID
                                       join status in context.Status on rg.StatusID equals status.ID
                                       join users in context.Users on rg.UserID equals users.ID
                                     //  join type in context.LookupTypes on sub.TypeID equals type.ID
                                       join subtype in context.LookupTypes on sub.SubTypeID equals subtype.ID into st
                                      // from stype in st.DefaultIfEmpty()
                                       join city in context.LookupTypes on rg.CityID equals city.ID
                                       where (StatusID > 0 && rg.StatusID == StatusID) || (StatusID == 0 )
                                       select new
                                       {
                                           rg.ID,
                                           rg.Description,
                                           rg.GreenPoints,
                                          // typeDescription = type.Name,
                                          // subTypeTitle = stype.Name,
                                           statusDescription = status.StatusName,
                                           users.Longitude,
                                           users.Latitude,
                                           userId = users.ID,
                                           userName = users.FullName,
                                           cityDescription = city.Name,
                                           rg.FileName,
                                           rg.CreatedDate,
                                           rg.UpdatedDate
                                       }).OrderByDescending(o => o.CreatedDate).ToList().Select(u => new
                                       {
                                           ID = u.ID,
                                           Description = u.Description,
                                           GreenPoints = subitemscalculate(u.ID),
                                         //  typeDescription = u.typeDescription,
                                          // subTypeTitle = u.subTypeTitle,
                                           statusDescription = u.statusDescription,
                                           Longitude = u.Longitude,
                                           Latitude = u.Latitude,
                                           userId = u.userId,
                                           userName = u.userName,
                                           cityDescription = u.cityDescription,
                                           FileName = u.FileName,
                                           CreatedDate = u.CreatedDate,
                                           updatedDate = Convert.ToDateTime(u.CreatedDate).ToString("MMM dd, yyyy"),
                                       })
                                       .ToList<object>();

            return mdlRegifts;
        }
        public int subitemscalculate (int id)
        {
            int count = 0;
            var TotalGP = (from user in context.Regifts
                           join re in context.RegiftSubItems on user.ID equals re.RegiftID into regift
                           where user.ID == id
                           select new
                           {
                               RegiftCount = regift.Select(g => g.GreenPoints).DefaultIfEmpty(0).Sum(),

                           }).Select(u => new
                           {
                               TotalGP = u.RegiftCount
                           }).FirstOrDefault();

            if (TotalGP != null)
            {
                count = Convert.ToInt32(TotalGP.GetType().GetProperty("TotalGP").GetValue(TotalGP));
            }


            return count;
        }
        public RegiftViewModel GetRegiftDetailById(int RegiftID, bool IsWebAdmin)
        {
            RegiftViewModel mdlRegift = (from rg in context.Regifts
                                         join users in context.Users on rg.UserID equals users.ID
                                         join orderTracking in context.OrderTrackings on rg.ID equals orderTracking.RsID
                                         join status in context.Status on orderTracking.StatusID equals status.ID
                                         where (rg.ID == RegiftID && orderTracking.Type == "Regift")
                                         select new RegiftViewModel()
                                         {
                                             RegiftSubItems = rg.RegiftSubItems.Where(x => (x.IsParent == true && !IsWebAdmin) || IsWebAdmin).Select(x => new RegiftSubItemViewModel
                                             {
                                                 ID = x.ID,
                                                 TypeID = x.TypeID,
                                                 Qty = x.Qty
                                             }).ToList(),
                                             RegiftComments = rg.RegiftComments.OrderByDescending(x=>x.CreatedDate).Select(x => new CommentsViewModel()
                                             {
                                                 ID = x.ID,
                                                 Comments = x.Comments,
                                                 Date = SqlFunctions.DateName("m", x.CreatedDate) + " " + SqlFunctions.DateName("dd", x.CreatedDate) + ", " + SqlFunctions.DateName("yyyy", x.CreatedDate) + " " +
                                                 SqlFunctions.DateName("hh", x.CreatedDate) + ":" + SqlFunctions.DateName("n", x.CreatedDate),
                                                 User = x.User.FullName
                                             }).ToList(),
                                             ID = rg.ID,
                                             Description = rg.Description,
                                             Latitude = rg.Latitude,
                                             Longitude = rg.Longitude,
                                             FileNameTakenByUser = orderTracking.FileNameTakenByUser,
                                             FileNameTakenByDriver = orderTracking.FileNameTakenByDriver,
                                             FileNameTakenByOrg = orderTracking.FileNameTakenByOrg,
                                             CollectedPendingConfirmation = orderTracking.CollectedPendingConfirmation == null ? false : orderTracking.CollectedPendingConfirmation,
                                             DeliveredPendingConfirmation = orderTracking.DeliveredPendingConfirmation == null ? false : orderTracking.DeliveredPendingConfirmation,
                                             OrderID = orderTracking.ID,
                                             UserID = users.ID,
                                             UserName = users.FullName,
                                             UserPhone = users.Phone,
                                             UserAddress = users.Address,
                                             StatusName = status.StatusName,
                                             PickDate = rg.PickupDate,
                                             OrderStatusID = (orderTracking.StatusID == (int)StatusEnum.Pending || orderTracking.StatusID == (int)StatusEnum.Declined) ? orderTracking.StatusID: -1,
                                             AssignTo = orderTracking.AssignTo ?? -1
                                         }).ToList()[0];

            return mdlRegift;
        }
        public bool AssignRegiftToDriver(RegiftViewModel _mdlRegiftVM, int? userId)
        {
            try
            {
                // Regift

                Regift mdlRegift = context.Regifts.Include(x => x.RegiftSubItems).First(x => x.ID == _mdlRegiftVM.ID);

                mdlRegift.PickupDate = Utility.GetParsedDate(_mdlRegiftVM.PickupDate);

                if (_mdlRegiftVM.AssignTo != null)
                {
                    if (_mdlRegiftVM.AssignTo != -1)
                    {

                        mdlRegift.StatusID = (int)StatusEnum.InProgress;
                        mdlRegift.GreenPoints = 0; //_mdlRegiftVM.TotalGP;

                    }
                    else if (_mdlRegiftVM.AssignTo == -1)
                    {
                        if (_mdlRegiftVM.OrderStatusID == (int)StatusEnum.Pending || _mdlRegiftVM.OrderStatusID == (int)StatusEnum.Declined)
                            mdlRegift.StatusID = _mdlRegiftVM.OrderStatusID;
                        else
                            mdlRegift.StatusID = (int)StatusEnum.Submit;
                    }
                }

                mdlRegift.UpdatedBy = userId;
                mdlRegift.UpdatedDate = DateTime.Now;

                // Regift Subitems

                List<RegiftSubItem> lstSubItems = new List<RegiftSubItem>();

                bool flag = true;

                foreach (RegiftSubItemViewModel subitem in _mdlRegiftVM.RegiftSubItems)
                {
                    lstSubItems.Add(new RegiftSubItem()
                    {
                        TypeID = subitem.TypeID,
                        Qty = subitem.Qty,
                        RegiftID = _mdlRegiftVM.ID,
                        IsParent = (flag ? true : false),
                        CreatedBy = userId,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        GreenPoints = 0 //subitem.Qty * _mdlRegiftVM.GPV
                    });

                    flag = false;
                }

                context.RegiftSubItems.RemoveRange(mdlRegift.RegiftSubItems);
                mdlRegift.RegiftSubItems = lstSubItems;

                // Order Tracking

                OrderTracking mdlOrderTracking = context.OrderTrackings.Find(_mdlRegiftVM.OrderID);

                if (_mdlRegiftVM.AssignTo != null)
                {
                    if (_mdlRegiftVM.AssignTo != -1)
                    {
                        if (mdlOrderTracking.AssignTo != _mdlRegiftVM.AssignTo)
                        {
                            mdlOrderTracking.CollectedPendingConfirmation = null;
                            mdlOrderTracking.DeliveredPendingConfirmation = null;
                        }

                        mdlOrderTracking.AssignTo = _mdlRegiftVM.AssignTo;
                        mdlOrderTracking.StatusID = (int)StatusEnum.Assigned;                    
                    }
                    else if (_mdlRegiftVM.AssignTo == -1)
                    {
                        mdlOrderTracking.AssignTo = null;

                        if (_mdlRegiftVM.OrderStatusID == (int)StatusEnum.Pending || _mdlRegiftVM.OrderStatusID == (int)StatusEnum.Declined)
                            mdlOrderTracking.StatusID = _mdlRegiftVM.OrderStatusID;
                        else
                            mdlOrderTracking.StatusID = (int)StatusEnum.New;
                    }

                    mdlOrderTracking.UpdatedBy = userId;
                    mdlOrderTracking.UpdatedDate = DateTime.Now;
                    context.SaveChanges();
                }            

                // Regift Comments

                //if (!string.IsNullOrEmpty(_mdlRegiftVM.Comments))
                {
                    RegiftComment regiftComments = new RegiftComment()
                    {
                        Comments = _mdlRegiftVM.Comments ?? string.Empty,
                        CreatedBy = Convert.ToInt32(userId),
                        CreatedDate = DateTime.Now,
                        UpdatedBy = Convert.ToInt32(userId),
                        UpdatedDate = DateTime.Now,
                        IsActive = true
                    };

                    mdlRegift.RegiftComments.Add(regiftComments);
                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }
        public bool CollectedRegiftByDriver(int? regiftID, List<RegiftSubItem> lstSubItems)
        {
            try
            {
                // Regift
                Regift mdlRegift = context.Regifts.Include(x => x.RegiftSubItems).First(x => x.ID == regiftID);

                context.RegiftSubItems.RemoveRange(mdlRegift.RegiftSubItems);
                mdlRegift.RegiftSubItems = lstSubItems;

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
        public bool RejectRegift(RegiftViewModel _mdlRegiftVM, int? userId)
        {
            try
            {
                // Regift

                Regift mdlRegift = context.Regifts.First(x => x.ID == _mdlRegiftVM.ID);

                mdlRegift.StatusID = (int)StatusEnum.Declined;

                mdlRegift.UpdatedBy = userId;
                mdlRegift.UpdatedDate = DateTime.Now;

                // Order Tracking

                OrderTracking mdlOrderTracking = context.OrderTrackings.Where(x => x.Type == "Regift" && x.RsID == _mdlRegiftVM.ID).FirstOrDefault();

                mdlOrderTracking.StatusID = (int)StatusEnum.Declined;

                mdlOrderTracking.UpdatedBy = userId;
                mdlOrderTracking.UpdatedDate = DateTime.Now;
                context.SaveChanges();
                // Regift Comments

                if (!string.IsNullOrEmpty(_mdlRegiftVM.Comments))
                {
                    RegiftComment regiftComments = new RegiftComment()
                    {
                        Comments = _mdlRegiftVM.Comments,
                        CreatedBy = Convert.ToInt32(userId),
                        CreatedDate = DateTime.Now,
                        UpdatedBy = Convert.ToInt32(userId),
                        UpdatedDate = DateTime.Now,
                        IsActive = true
                    };

                    mdlRegift.RegiftComments.Add(regiftComments);
                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

    }
}
