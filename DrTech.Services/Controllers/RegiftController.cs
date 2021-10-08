using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrTech.DAL;
using Microsoft.AspNetCore.Mvc;
using DrTech.Models;
using static DrTech.Common.Extentions.Constants;
using DrTech.Common.Helpers;
using DrTech.Models.Dropdown;
using Microsoft.AspNetCore.Authorization;
using DrTech.Common.ServerResponse;
using DrTech.Common.Enums;
using DrTech.Models.ViewModels;
using DrTech.Common.Extentions;
using MongoDB.Driver;
using MongoDB.Bson;
using DrTech.Models.Common;
using DrTech.Services.Attribute;
using DrTech.Notifications;

namespace DrTech.Services.Controllers
{
    [Authorize]
    [Route("api/Regift")]
    public class RegiftController : BaseControllerBase
    {

        [HttpPost("AddDonation"), DisableRequestSizeLimit]
        public async Task<ResponseObject<bool>> AddDonation(Regift donation)
        {
            try
            {
                var isHasNewValue = false;

                if (donation.DonateTo < 0)
                {
                    if (string.IsNullOrEmpty(donation.DonateToDescription))
                        return ServiceResponse.ErrorReponse<bool>(MessageEnum.DonationDonateToDescriptionParameterNull);
                    isHasNewValue = true;
                }
                if (donation.Type < 0)
                {
                    if (string.IsNullOrEmpty(donation.TypeDescription))
                        return ServiceResponse.ErrorReponse<bool>(MessageEnum.DonationTypeDescriptionParameterNull);
                    isHasNewValue = true;
                }
                if (donation.City < 0)
                {
                    if (string.IsNullOrEmpty(donation.CityDescription))
                        return ServiceResponse.ErrorReponse<bool>(MessageEnum.DonationCityDescriptionParameterNull);
                    isHasNewValue = true;
                }

                if (donation.File != null)
                    donation.FileName = await SaveFile(donation.File);

                if (isHasNewValue)
                {
                    donation.Status = (int)StatusEnum.PenddingApproval;
                    donation.StatusDescription = StatusEnum.PenddingApproval.GetDescription();
                }
                else
                {
                    donation.Status = (int)StatusEnum.Submit;
                    donation.StatusDescription = StatusEnum.Submit.GetDescription();
                }

                donation.UserId = GetLoggedInUserId();
                //   await _IUWork.AddSubDocument<Users, Regift>(GetLoggedInUserId(), donation, CollectionNames.USERS, CollectionNames.REGIFT);
                await _IUWork.InsertOne(donation, CollectionNames.REGIFT);

                //NotifyEvent _event = new NotifyEvent();
                //_event.Parameters.Add("FileName", donation.FileName);
                //_event.AddNotifyEvent((long)NotificationEventConstants.Regift.EmailSendToAdminForApproval, GetLoggedInUserId());


                NotifyEvent _event = new NotifyEvent();
                _event.Parameters.Add("FileName", donation.FileName);
                _event.Parameters.Add("TypeDescription", donation.TypeDescription);
                _event.Parameters.Add("SubTypeDescription", donation.SubTypeDescription);
                _event.Parameters.Add("Longitude", donation.Longitude);
                _event.Parameters.Add("Latitude", donation.Latitude);
                _event.Parameters.Add("DonateToDescription", donation.DonateToDescription);
                _event.AddNotifyEvent((long)NotificationEventConstants.Regift.EmailSendToAdminForApproval, GetLoggedInUserId());


                SMSNotifyEvent _events = new SMSNotifyEvent();
                _events.Parameters.Add("TypeDescription", donation.TypeDescription);
                _events.AddSMSNotifyEvent((long)NotificationEventConstants.Regift.SMSSendToUser, GetLoggedInUserId());


                return ServiceResponse.SuccessReponse(true, MessageEnum.DonationAddedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpGet("GetDonation")]
        public async Task<ResponseObject<List<Regift>>> GetDonation(string userId, string DonationId)
        {
            try
            {

                List<FilterHelper> filter = new List<FilterHelper>
                {
                    new FilterHelper
                    {
                        Field = "_Id",
                        Value = DonationId
                    },
                    new FilterHelper
                    {
                        Field = "UserId",
                        Value = userId
                    }
                };

                List<Regift> LstRegiftItems = _IUWork.GetModelData<Regift>(filter, CollectionNames.REGIFT);

                LstRegiftItems = LstRegiftItems?.ToSortByCreationDateDescendingOrder();

                if (LstRegiftItems?.Count == 0)
                    return ServiceResponse.SuccessReponse(LstRegiftItems, MessageEnum.DonationFoundSuccessfully);

                return ServiceResponse.SuccessReponse(LstRegiftItems, MessageEnum.DonationNotFound);

            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Regift>>(exp);
            }
        }
        [HttpGet("GetDonations")]
        public async Task<ResponseObject<List<Regift>>> GetDonations(string UserId = null)
        {
            try
            {

                List<FilterHelper> filter = new List<FilterHelper>
                {
                    new FilterHelper
                    {
                        Field = "UserId",
                        Value = GetLoggedInUserId()
                    }
                };

                List<Regift> LstRegiftItems = _IUWork.GetModelData<Regift>(filter, CollectionNames.REGIFT);


                // var user = await _IUWork.FindOneByID<Users>(GetLoggedInUserId(), CollectionNames.USERS);

                if (LstRegiftItems.Any(p => string.IsNullOrEmpty(p.NeedType)))
                {
                    var orderedList = LstRegiftItems?.OrderByDescending(p => p.CreatedAt).ToList();

                    foreach (var item in orderedList.FindAll(p => string.IsNullOrEmpty(p.NeedType)))
                    {
                        var filters = new List<FilterHelper>
                                {
                                      new FilterHelper
                                        {
                                            Field = "Value",
                                            Value = item.SubType.ToString()
                                        }
                                };

                        var regift = _IUWork.GetModelData<DropdownDbViewModel>(filters, CollectionNames.Lookups)?.FirstOrDefault();
                        item.SubTypeTitle = EnumExtensionMethod.GetTitle<DropdownTypeEnum>(regift?.Type);
                    }
                    return ServiceResponse.SuccessReponse(orderedList, MessageEnum.DonationFoundSuccessfully);
                }
                else
                {
                    return ServiceResponse.SuccessReponse(LstRegiftItems, MessageEnum.DonationNotFound);
                }
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Regift>>(exp);
            }
        }

        //[HttpPost("AddReception")]
        //public async Task<ResponseObject<bool>> AddReception([FromBody]Reciption model)
        //{
        //    try
        //    {
        //        await _IUWork.AddSubDocument<Users, Reciption>(GetLoggedInUserId(), model, CollectionNames.USERS, CollectionNames.RECIPTION);
        //        return ServiceResponse.SuccessReponse(true, MessageEnum.ReciptionAddedSuccessfully);
        //    }
        //    catch (Exception exp)
        //    {
        //        return ServiceResponse.ErrorReponse<bool>(exp);
        //    }
        //}


        [HttpGet("GetDonationDropdowns")]
        public async Task<ResponseObject<DonationDropdownsViewModel>> GetDonationDropdowns()
        {
            try
            {
                var dorpdowns = new DonationDropdownsViewModel
                {
                    DonationType = await GetDropDown(DropdownTypeEnum.DonationType.GetDescription(), false, false),
                    NGOs = await GetDropDown(DropdownTypeEnum.DonateToType.GetDescription()),
                    CityList = await GetDropDown(DropdownTypeEnum.City.GetDescription(),false,false),
                    AgeGroup = await GetDropDown(DropdownTypeEnum.AgeGroup.GetDescription())
                };

                return ServiceResponse.SuccessReponse(dorpdowns, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<DonationDropdownsViewModel>(exp);
            }
        }
        [HttpGet("GetRecipientDropdowns")]
        public async Task<ResponseObject<DonationDropdownsViewModel>> GetRecipientDropdowns()
        {
            try
            {
                var dorpdowns = new DonationDropdownsViewModel
                {
                    DonationType = await GetDropDown(DropdownTypeEnum.DonationType.GetDescription()),
                    CityList = await GetDropDown(DropdownTypeEnum.City.GetDescription()),
                };

                return ServiceResponse.SuccessReponse(dorpdowns, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<DonationDropdownsViewModel>(exp);
            }
        }

      //  [Auth(UserRoleTypeEnum.Admin)]
        [HttpPost("UpdateStatus"), DisableRequestSizeLimit]
        public async Task<ResponseObject<bool>> UpdateStatus(ReuseViewModel mdlReuse)
        {
            if (mdlReuse == null)
                return ServiceResponse.ErrorReponse<bool>(MessageEnum.DonationParameterNull);

            try
            {
                var fileName = "";
                if (mdlReuse.File != null && mdlReuse.Status == (int)StatusEnum.Delivered)
                    fileName = await SaveFile(mdlReuse.File);

                int ItemsStatus = 0;

                if (mdlReuse.Status == (int)StatusEnum.InProgress)
                    ItemsStatus = (int)StatusEnum.InProgress;
                else
                    ItemsStatus = (int)StatusEnum.Delivered;

                if (ItemsStatus == (int)StatusEnum.Delivered)
                {
                    NotifyEvent _event = new NotifyEvent();
                    _event.Parameters.Add("ReuserId", mdlReuse.Id);
                    _event.AddNotifyEvent((long)NotificationEventConstants.Regift.EmailSendToUser, mdlReuse.UserId);
                }

                var update = Builders<Regift>.Update
                                              .Set(o => o.Status, ItemsStatus)
                                              .Set(p => p.StatusDescription, ((StatusEnum)ItemsStatus).GetDescription())
                                              .Set(g => g.Type, mdlReuse.Type)
                                              .Set(g => g.City, mdlReuse.City)
                                              .Set(g => g.DonateTo, mdlReuse.DonateTo)
                                              .Set(g => g.RecieptFileName, fileName)
                                              .Set(g => g.GreenPoints, mdlReuse.GreenPoints)
                                              .Set(g => g.UpdatedAt, DateTime.Now.ToString());

                bool result = _IUWork.UpdateStatus(mdlReuse.Id.ToString(), update, CollectionNames.REGIFT);

                if (result == true)
                {
                    var Regift = _IUWork.FindOneByID<Regift>(mdlReuse.Id.ToString(), CollectionNames.REGIFT).Result;
                    var User = _IUWork.FindOneByID<Users>(Regift.UserId.ToString(), CollectionNames.USERS).Result;
                    User.GreenPoints += mdlReuse.GreenPoints;
                    long ID = _IUWork.UpdateUserGreenPoints(User.GreenPoints, User.Id.ToString());
                }

                return ServiceResponse.SuccessReponse(result, MessageEnum.DonationAddedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }

        [HttpPost("UpdateApprovalStatus")]
        public async Task<ResponseObject<bool>> UpdateApprovalStatus([FromBody]ReuseViewModel mdlReuse)
        {

            if (mdlReuse == null)
                return ServiceResponse.ErrorReponse<bool>(MessageEnum.DonationParameterNull);

            try
            {
                if (mdlReuse.Status != (int)StatusEnum.Approved && mdlReuse.Status != (int)StatusEnum.Rejected)
                    return ServiceResponse.ErrorReponse<bool>(MessageEnum.DefaultProvidedCorrectStatusValue);

                if (mdlReuse.Status != (int)StatusEnum.Rejected)
                {
                    if (mdlReuse.DonateTo < 0)
                    {
                        if (mdlReuse.DonateTo < 0 && string.IsNullOrEmpty(mdlReuse.DonateToDescription))
                        {
                            return ServiceResponse.ErrorReponse<bool>(MessageEnum.DonationDonateToDescriptionParameterNull);
                        }

                        mdlReuse.DonateTo = await InsertDropdownValue(mdlReuse.DonateToDescription, DropdownTypeEnum.DonateToType.GetDescription());
                    }
                    if (mdlReuse.Type < 0)
                    {
                        if (mdlReuse.Type < 0 && string.IsNullOrEmpty(mdlReuse.TypeDescription))
                        {
                            return ServiceResponse.ErrorReponse<bool>(MessageEnum.DonationTypeDescriptionParameterNull);
                        }
                        mdlReuse.Type = await InsertDropdownValue(mdlReuse.TypeDescription, DropdownTypeEnum.DonationType.GetDescription());
                    }
                    if (mdlReuse.City < 0)
                    {
                        if (mdlReuse.City < 0 && string.IsNullOrEmpty(mdlReuse.CityDescription))
                        {
                            return ServiceResponse.ErrorReponse<bool>(MessageEnum.DonationCityDescriptionParameterNull);
                        }
                        mdlReuse.City = await InsertDropdownValue(mdlReuse.CityDescription, DropdownTypeEnum.City.GetDescription());
                    }
                }
                var status = StatusEnum.Submit;
                if (mdlReuse.Status == (int)StatusEnum.Rejected)
                {
                    status = StatusEnum.Rejected;
                }

                //var update = Builders<Users>.Update.Set(CollectionNames.REGIFT + ".$.Status", (int)status)
                //                                    .Set(CollectionNames.REGIFT + ".$.StatusDescription", status.GetDescription())
                //                                    .Set(CollectionNames.REGIFT + ".$.Type", mdlReuse.Type)
                //                                    .Set(CollectionNames.REGIFT + ".$.City", mdlReuse.City)
                //                                    .Set(CollectionNames.REGIFT + ".$.DonateTo", mdlReuse.DonateTo);

                //var result = await _IUWork.UpdateSubDocument<Users, Regift>(mdlReuse.Id.ToString(), update, CollectionNames.USERS, CollectionNames.REGIFT);


                var update = Builders<Regift>.Update
                                          .Set(o => o.Status, (int)status)
                                          .Set(p => p.StatusDescription, status.GetDescription())
                                          .Set(g => g.Type, mdlReuse.Type)
                                          .Set(g => g.City, mdlReuse.City)
                                          .Set(g => g.DonateTo, mdlReuse.DonateTo);

                bool result = _IUWork.UpdateStatus(mdlReuse.Id.ToString(), update, CollectionNames.REGIFT);


                return ServiceResponse.SuccessReponse(result, MessageEnum.DonationAddedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }
        }
        [HttpGet("GetAll")]
        public async Task<ResponseObject<List<Regift>>> GetAll(string id = null)
        {
            try
            {
                List<Regift> lstRegift = new List<Regift>();

                if (string.IsNullOrEmpty(id))
                    lstRegift = _IUWork.GetModelData<Regift>(CollectionNames.REGIFT);
                else
                {
                    lstRegift = _IUWork.GetModelByUserID<Regift>(id, CollectionNames.REGIFT);
                }


                foreach (var item in lstRegift)
                {

                    List<FilterHelper> filter = new List<FilterHelper>
                    {
                        new FilterHelper
                        {
                            Field = "Value",
                            Value = item.SubType.ToString()
                        }
                    };

                    var list = _IUWork.GetModelData<DropdownDbViewModel>(filter, CollectionNames.Lookups).FirstOrDefault();

                    item.SubTypeTitle = EnumExtensionMethod.GetTitle<DropdownTypeEnum>(list?.Type) + " : " + item.SubTypeDescription;
                }

                lstRegift = lstRegift.FindAll(p => p.Status > 0 && p.Status != (int)StatusEnum.PenddingApproval);
                lstRegift = lstRegift.ToSortByCreationDateDescendingOrder();

                return ServiceResponse.SuccessReponse(lstRegift, MessageEnum.ComplaintGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Regift>>(exp);
            }
        }
        [HttpGet("GetAllPendingForApproval")]
        public async Task<ResponseObject<List<Regift>>> GetAllPendingForApproval()
        {
            try
            {

                var filter = Builders<Regift>.Filter.And(new BsonDocument { { "Status", (int)StatusEnum.PenddingApproval } });
                //       var list = await _IUWork.GetAllSubDocuments<Users, Regift>(CollectionNames.USERS, CollectionNames.REGIFT, filter);
                List<Regift> list = _IUWork.GetModelData<Regift>(CollectionNames.REGIFT);
                List<Regift> donationList = new List<Regift>();
                if (list?.Count > 0)
                {
                    foreach (var donation in list.FindAll(p => p.Status == (int)StatusEnum.PenddingApproval))
                    {
                        if (donation.Status == (int)StatusEnum.PenddingApproval)
                            donationList.Add(donation);
                    }
                }
                donationList = donationList.ToSortByCreationDateDescendingOrder();
                return ServiceResponse.SuccessReponse(donationList, MessageEnum.ComplaintGetSuccess);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Regift>>(exp);
            }
        }



        [HttpPost("AddDonationDropdown"), DisableRequestSizeLimit]
        public async Task<ResponseObject<bool>> AddDonationDropdown(DonationType DonationType)
        {
            try
            {
                if (DonationType == null) return ServiceResponse.ErrorReponse<bool>(MessageEnum.DefaultParametersCanNotBeNull);
                DonationType.FileName = await SaveFile(DonationType.File);
                await _IUWork.InsertOne<DonationType>(DonationType, "DonationType");
                return ServiceResponse.SuccessReponse(true, MessageEnum.ReplantAddedSuccessfully);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<bool>(exp);
            }

        }


        [HttpGet("GetDonationDropdown")]
        public async Task<ResponseObject<List<DonationType>>> GetDonationDropdown()
        {
            try
            {

                List<FilterHelper> filter = new List<FilterHelper>
                {
                    new FilterHelper
                    {
                        Field = "Type",
                        Value = "DonationType"
                    }
                };

                List<DonationType> list = _IUWork.GetModelData<DonationType>(filter, "DonationType");

                return ServiceResponse.SuccessReponse(list, MessageEnum.DefaultSuccessMessage);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<DonationType>>(exp);
            }
        }

    }
}