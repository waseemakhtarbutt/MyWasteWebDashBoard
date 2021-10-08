using DrTech.Amal.SQLDataAccess.CustomModels;
using DrTech.Amal.SQLDatabase;
using DrTech.Amal.SQLModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using DrTech.Amal.Common.Helpers;
using DrTech.Amal.Common.Enums;
using System.Data.Entity.SqlServer;

namespace DrTech.Amal.SQLDataAccess.Repository
{
    public class BuyBinRepository : Repository<BuyBin>
    {
        public BuyBinRepository(Amal_Entities context)
       : base(context)
        {
            dbSet = context.Set<BuyBin>();
        }

        public List<object> GetBuyBinDetail(int? userID)
        {
            var BuyBinDetails = (from bin in context.BuyBins
                                 join status in context.Status on bin.StatusID equals status.ID
                                 join details in context.BinDetails on bin.BinID equals details.ID
                                 where bin.UserID == userID
                                 select new
                                 {
                                     ID = bin.ID,
                                     FileName = bin.FileName,
                                     BinName = details.BinName,
                                     TrackingNumber = bin.TrackingNumber,
                                     Price = details.Price,
                                     Qty = bin.Qty,
                                     StatusName = status.StatusName,
                                     StatusID = bin.StatusID,
                                     CreatedAt = bin.CreatedDate
                                 }).OrderByDescending(o => o.ID).ToList<object>();

            return BuyBinDetails;
        }

        public List<object> GetBuyBinList()
        {
            var binDetailList = (from bin in context.BinDetails.ToList()
                                 where bin.IsActive==true
                                 select new
                                 {
                                     bin.BinName,
                                     bin.Price,
                                     bin.QRCode,
                                     bin.Capacity,
                                     bin.Description,
                                     bin.FileName,
                                     bin.ID,
                                     PriceInGC = bin.Price * 3
             }).ToList<object>();
            return binDetailList;
        }

        public List<object> GetBinsListByStatus(int StatusID)
        {
            List<object> mdlBins = (from bin in context.BuyBins.ToList()
                                       join bindet in context.BinDetails on bin.BinID equals bindet.ID
                                       join status in context.Status on bin.StatusID equals status.ID
                                       join users in context.Users on bin.UserID equals users.ID
                                       join userPayments in context.UserPayments on bin.UserPaymentID equals userPayments.ID
                                       join paymentMethods in context.PaymentMethods on userPayments.PaymentMethodID equals paymentMethods.ID

                                    where ((StatusID > 0 && bin.StatusID == StatusID) || (StatusID == 0))
                                       select new
                                       {
                                          bin.ID,
                                          bindet.BinName,
                                          greenPoints=bin.GreenPoints,
                                          statusDescription = status.StatusName,
                                          users.Longitude,
                                          users.Latitude,
                                          userId = users.ID,
                                          userName = users.FullName,
                                          bin.FileName,
                                          bin.CreatedDate,
                                          price= bin.Price,
                                          paymentMethodName= paymentMethods.Name,
                                          updatedDate = Convert.ToDateTime(bin.CreatedDate).ToString("MMM dd, yyyy"),
                                       }).OrderByDescending(o => o.CreatedDate).ToList<object>();

            return mdlBins;
        }

        public BuyBinViewModel GetBinDetailById(int BinID)
        {
            BuyBinViewModel mdlBinDetail = (from buybin in context.BuyBins
                                         join bd in context.BinDetails on buybin.BinID equals bd.ID
                                         join users in context.Users on buybin.UserID equals users.ID
                                         join orderTracking in context.OrderTrackings on buybin.ID equals orderTracking.RsID
                                         join status in context.Status on orderTracking.StatusID equals status.ID
                                         where (buybin.ID == BinID && orderTracking.Type == "Bin")
                                         select new BuyBinViewModel()
                                         {
                                             BinSubItems = new List<BinDetailViewModel>()
                                             {
                                                   new BinDetailViewModel()
                                                   {
                                                        ID = buybin.ID,
                                                        Description = bd.Description,
                                                        Qty = buybin.Qty
                                                   }
                                             },
                                             BuyBinComments = buybin.BuyBinComments.OrderByDescending(x => x.CreatedDate).Select(x => new CommentsViewModel()
                                             {
                                                 ID = x.ID,
                                                 Comments = x.Comments,
                                                 Date = SqlFunctions.DateName("m", x.CreatedDate) + " " + SqlFunctions.DateName("dd", x.CreatedDate) + ", " + SqlFunctions.DateName("yyyy", x.CreatedDate) + " " +
                                                   SqlFunctions.DateName("hh", x.CreatedDate) + ":" + SqlFunctions.DateName("n", x.CreatedDate),
                                                 User = x.User.FullName
                                             }).ToList(),
                                             ID = buybin.ID,
                                             Name = bd.BinName,
                                             Description = bd.Description,
                                             Latitude = users.Latitude,
                                             Longitude = users.Longitude,
                                             FileNameTakenByUser = orderTracking.FileNameTakenByUser,
                                             OrderID = orderTracking.ID,
                                             UserName = users.FullName,
                                             UserPhone = users.Phone,
                                             UserAddress = users.Address,
                                             StatusName = status.StatusName,
                                             DeliverDate = (buybin.DeliveryDate == null) ? DateTime.Now : buybin.DeliveryDate,
                                             OrderStatusID = (orderTracking.StatusID == (int)StatusEnum.Pending || orderTracking.StatusID == (int)StatusEnum.Declined) ? orderTracking.StatusID : -1,
                                             AssignTo = orderTracking.AssignTo ?? -1,
                                             Price = bd.Price,
                                             PaidAmount = buybin.UserPayment.AmountPaid,
                                             PaymentMethod = buybin.UserPayment.PaymentMethod.Name
                                         }).ToList()[0];

            return mdlBinDetail;
        }
        public bool AssignBinToDriver(BuyBinViewModel _mdlBinVM, int? userId)
        {
            try
            {
                BuyBin buyBin = context.BuyBins.First(x => x.ID == _mdlBinVM.ID);

                buyBin.DeliveryDate = Utility.GetParsedDate(_mdlBinVM.DeliveryDate);

                if (_mdlBinVM.AssignTo != null)
                {
                    if (_mdlBinVM.AssignTo != -1)
                    {
                        buyBin.Qty = _mdlBinVM.BinSubItems[0].Qty;
                        buyBin.StatusID = (int)StatusEnum.InProgress;
                        buyBin.GreenPoints = 0;// _mdlBinVM.TotalGP;
                    }
                    else if (_mdlBinVM.AssignTo == -1)
                    {
                        if (_mdlBinVM.OrderStatusID == (int)StatusEnum.Pending || _mdlBinVM.OrderStatusID == (int)StatusEnum.Declined)
                            buyBin.StatusID = _mdlBinVM.OrderStatusID;
                        else
                            buyBin.StatusID = (int)StatusEnum.Submit;
                    }
                }

                buyBin.UpdatedBy = userId;
                buyBin.UpdatedDate = DateTime.Now;

                BinDetail mdlBinDetail = context.BinDetails.First(x => x.ID == buyBin.BinID);

                mdlBinDetail.Description = _mdlBinVM.BinSubItems[0].Description;

                mdlBinDetail.UpdatedBy = userId;
                mdlBinDetail.UpdatedDate = DateTime.Now;

                // Order Tracking

                if (_mdlBinVM.AssignTo != null)
                {
                    OrderTracking mdlOrderTracking = context.OrderTrackings.Find(_mdlBinVM.OrderID);

                    if (_mdlBinVM.AssignTo != -1)
                    {
                        mdlOrderTracking.AssignTo = _mdlBinVM.AssignTo;
                        mdlOrderTracking.StatusID = (int)StatusEnum.Assigned;
                    }
                    else if (_mdlBinVM.AssignTo == -1)
                    {
                        mdlOrderTracking.AssignTo = null;

                        if (_mdlBinVM.OrderStatusID == (int)StatusEnum.Pending || _mdlBinVM.OrderStatusID == (int)StatusEnum.Declined)
                            mdlOrderTracking.StatusID = _mdlBinVM.OrderStatusID;
                        else
                            mdlOrderTracking.StatusID = (int)StatusEnum.New;
                    }

                    mdlOrderTracking.UpdatedBy = userId;
                    mdlOrderTracking.UpdatedDate = DateTime.Now;
                }

                // BuyBin Comments

                //if (!string.IsNullOrEmpty(_mdlBinVM.Comments))
                {
                    BuyBinComment buybinComments = new BuyBinComment()
                    {
                        Comments = _mdlBinVM.Comments ?? string.Empty,
                        CreatedBy = Convert.ToInt32(userId),
                        CreatedDate = DateTime.Now,
                        UpdatedBy = Convert.ToInt32(userId),
                        UpdatedDate = DateTime.Now,
                        IsActive = true
                    };

                    buyBin.BuyBinComments.Add(buybinComments);
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
