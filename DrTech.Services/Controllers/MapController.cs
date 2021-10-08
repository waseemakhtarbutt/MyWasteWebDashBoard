using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrTech.Common.Enums;
using DrTech.Common.Extentions;
using DrTech.Common.ServerResponse;
using DrTech.DAL;
using DrTech.Models;
using DrTech.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static DrTech.Common.Extentions.Constants;

namespace DrTech.Services.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class MapController : BaseControllerBase
    {
        [HttpGet("GetUsersGreenPointStatus")]
        public ResponseObject<List<Users>> GetUsersGreenPointStatus()
        {
            try
            {
                List<Users> lstUsersDetails = _IUWork.GetModelData<Users>(CollectionNames.USERS);

                if (lstUsersDetails.Count > 0)
                {
                    return ServiceResponse.SuccessReponse(lstUsersDetails, MessageEnum.DefaultSuccessMessage);
                }
                else
                {
                    return ServiceResponse.ErrorReponse<List<Users>>(MessageEnum.MapPointsNotFound);
                }
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<Users>>(exp);
            }
        }

        [HttpGet("GetUserRefusePoints")]
        public async Task<ResponseObject<List<MapMarker>>> GetUserRefusePoints()
        {
            try
            {
                var lstUsersDetails = _IUWork.GetModelData<Users>(CollectionNames.USERS);
                List<MapMarker> markers = new List<MapMarker>();

                if (lstUsersDetails != null && lstUsersDetails.Any(p => p.UserRole != UserRoleTypeEnum.Admin.GetDescription() && p.UserRole != UserRoleTypeEnum.NGO.GetDescription()))
                {
                    foreach (var user in lstUsersDetails.FindAll(p => p.UserRole != UserRoleTypeEnum.Admin.GetDescription() && p.UserRole != UserRoleTypeEnum.NGO.GetDescription()))
                    {
                        if (user.Refuse.Any(p => p.Status == (int)StatusEnum.Submit))
                        {
                            foreach (var refuse in user.Refuse.FindAll(p => p.Status == (int)StatusEnum.Resolved))
                            {
                                markers.Add(new MapMarker
                                {
                                    PinImage = "refuse.png",
                                    Latitude = refuse.Latitude,
                                    Longitude = refuse.Longitude,
                                    Type = "Refuse",
                                    Label = "refuse",
                                    Cash = 0,
                                    GreenPoints = refuse.GreenPoints,
                                    FileName = refuse.FileName,
                                    Description = refuse.Idea
                                });
                            }
                        }
                    }
                }
                return markers.Count > 0 ? ServiceResponse.SuccessReponse(markers, MessageEnum.DefaultSuccessMessage) : ServiceResponse.SuccessReponse(new List<MapMarker>(), MessageEnum.MapPointsNotFound);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<MapMarker>>(exp);
            }
        }

        [HttpGet("GetUserReducePoints")]
        public async Task<ResponseObject<List<MapMarker>>> GetUserReducePoints()
        {
            try
            {
                var lstUsersDetails = _IUWork.GetModelData<Users>(CollectionNames.USERS);
                List<MapMarker> markers = new List<MapMarker>();

                if (lstUsersDetails != null && lstUsersDetails.Any(p => p.UserRole != UserRoleTypeEnum.Admin.GetDescription() && p.UserRole != UserRoleTypeEnum.NGO.GetDescription()))
                {
                    foreach (var user in lstUsersDetails.FindAll(p => p.UserRole != UserRoleTypeEnum.Admin.GetDescription() && p.UserRole != UserRoleTypeEnum.NGO.GetDescription()))
                    {
                        if (user.Reduce.Any(p => p.Status == (int)StatusEnum.Submit))
                        {
                            foreach (var reduce in user.Reduce.FindAll(p => p.Status == (int)StatusEnum.Resolved))
                            {
                                markers.Add(new MapMarker
                                {
                                    PinImage = "reduce.png",
                                    Latitude = user.Latitude,
                                    Longitude = user.Longitude,
                                    Type = "Reduce",
                                    Label = "reduce",
                                    Cash = 0,
                                    GreenPoints = reduce.GreenPoints,
                                    FileName = reduce.FileName,
                                    Description = reduce.Idea
                                });
                            }
                        }
                    }
                }
                return markers.Count > 0 ? ServiceResponse.SuccessReponse(markers, MessageEnum.DefaultSuccessMessage) : ServiceResponse.SuccessReponse(new List<MapMarker>(), MessageEnum.MapPointsNotFound);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<MapMarker>>(exp);
            }
        }

        [HttpGet("GetUserReusePoints")]
        public async Task<ResponseObject<List<MapMarker>>> GetUserReusePoints()
        {
            try
            {
                var lstUsersDetails = _IUWork.GetModelData<Users>(CollectionNames.USERS);
                List<MapMarker> markers = new List<MapMarker>();

                if (lstUsersDetails != null && lstUsersDetails.Any(p => p.UserRole != UserRoleTypeEnum.Admin.GetDescription() && p.UserRole != UserRoleTypeEnum.NGO.GetDescription()))
                {
                    foreach (var user in lstUsersDetails.FindAll(p => p.UserRole != UserRoleTypeEnum.Admin.GetDescription() && p.UserRole != UserRoleTypeEnum.NGO.GetDescription()))
                    {
                        if (user.Reuse.Any(p => p.Status == (int)StatusEnum.Submit))
                        {
                            foreach (var reuse in user.Reuse.FindAll(p => p.Status == (int)StatusEnum.Resolved))
                            {
                                markers.Add(new MapMarker
                                {
                                    PinImage = "reuse.png",
                                    Latitude = user.Latitude,
                                    Longitude = user.Longitude,
                                    Type = "reuse",
                                    Label = "reuse",
                                    Cash = 0,
                                    GreenPoints = reuse.GreenPoints,
                                    FileName = reuse.FileName,
                                    Description = reuse.Idea
                                });
                            }
                        }
                    }
                }
                return markers.Count > 0 ? ServiceResponse.SuccessReponse(markers, MessageEnum.DefaultSuccessMessage) : ServiceResponse.SuccessReponse(new List<MapMarker>(), MessageEnum.MapPointsNotFound);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<MapMarker>>(exp);
            }
        }

        [HttpGet("GetUserRegiftPoints")]
        public async Task<ResponseObject<List<MapMarker>>> GetUserRegiftPoints()
        {
            try
            {

                List<FilterHelper> filter = new List<FilterHelper>
                                    {
                                        new FilterHelper
                                        {
                                            Field = "Status",
                                            Value = ((int)StatusEnum.Delivered).ToString()

                                        }
                                    };


                List<Regift> lstUsersDetails = _IUWork.GetModelData<Regift>(filter, CollectionNames.REGIFT);
                var mapMarker = new List<MapMarker>();

                var list = lstUsersDetails.ConvertAll(regift => new MapMarker
                {
                    PinImage = "regift.png",
                    Latitude = regift.Latitude,
                    Longitude = regift.Longitude,
                    Type = "Regift",
                    Label = "regift",
                    Cash = 0,
                    GreenPoints = 0,
                    FileName = regift.FileName,
                    Description=  regift.Description
                });
                mapMarker = list ?? new List<MapMarker>();

                return mapMarker.Count > 0 ? ServiceResponse.SuccessReponse(mapMarker, MessageEnum.DefaultSuccessMessage) : ServiceResponse.SuccessReponse(new List<MapMarker>(), MessageEnum.MapPointsNotFound);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<MapMarker>>(exp);
            }
        }

        [HttpGet("GetUserReportPoints")]
        public async Task<ResponseObject<List<MapMarker>>> GetUserReportsPoint()
        {
            try
            {
                List<FilterHelper> filter = new List<FilterHelper>
                                    {
                                        new FilterHelper
                                        {
                                            Field = "Status",
                                            Value = ((int)StatusEnum.Submit).ToString()

                                        }
                                    };
                List<Report> lstReportDetails = _IUWork.GetModelData<Report>(filter, CollectionNames.Report);
                var mapMarker = new List<MapMarker>();

                var list = lstReportDetails.ConvertAll(reuse => new MapMarker
                {
                    PinImage = "report.png",
                    Latitude = reuse.Latitude,
                    Longitude = reuse.Longitude,
                    Type = "Report",
                    Label = "report",
                    Cash = 0,
                    GreenPoints = reuse.GreenPoints,
                    FileName = reuse.FileName,
                    Description = reuse.Description
                });

                mapMarker = list ?? new List<MapMarker>();

                return mapMarker.Count > 0 ? ServiceResponse.SuccessReponse(mapMarker, MessageEnum.DefaultSuccessMessage) : ServiceResponse.SuccessReponse(new List<MapMarker>(), MessageEnum.MapPointsNotFound);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<MapMarker>>(exp);
            }
        }
        [HttpGet("GetUserRecyclePoints")]
        public async Task<ResponseObject<List<MapMarker>>> GetUserRecyclePoints()
        {
            try
            {
                List<FilterHelper> filter = new List<FilterHelper>
                                    {
                                        new FilterHelper
                                        {
                                            Field = "Status",
                                            Value = ((int)StatusEnum.Submit).ToString()

                                        }
                                    };
                List<MrClean> lstUsersDetails = _IUWork.GetModelData<MrClean>(filter, CollectionNames.RECYCLE);
                var markers = new List<MapMarker>();

                if (lstUsersDetails.Count > 0)
                {
                    foreach (var item in lstUsersDetails)
                    {
                        var user = await _IUWork.FindOneByID<Users>(item.UserId, CollectionNames.USERS);
                        if (user != null)
                        {
                            markers.Add(new MapMarker
                            {
                                PinImage = "recycle.png",
                                Latitude = user.Latitude,
                                Longitude = user.Longitude,
                                Type = "Recyle",
                                Label = "recycle",
                                Cash = user.Cash,
                                GreenPoints = item.GreenPoints,
                                FileName = item.FileName,
                                Description= item.Description
                            });
                        }
                    }

                }
                return markers.Count > 0 ? ServiceResponse.SuccessReponse(markers, MessageEnum.DefaultSuccessMessage) : ServiceResponse.SuccessReponse(new List<MapMarker>(), MessageEnum.MapPointsNotFound);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<MapMarker>>(exp);
            }
        }

        [HttpGet("GetUserReplantPoints")]
        public async Task<ResponseObject<List<MapMarker>>> GetUserReplantPoints()
        {
            try
            {
                var lstUsersDetails = _IUWork.GetModelData<Users>(CollectionNames.USERS);
                List<MapMarker> markers = new List<MapMarker>();

                if (lstUsersDetails != null && lstUsersDetails.Any(p => p.UserRole != UserRoleTypeEnum.Admin.GetDescription() && p.UserRole != UserRoleTypeEnum.NGO.GetDescription()))
                {
                    foreach (var user in lstUsersDetails.FindAll(p => p.UserRole != UserRoleTypeEnum.Admin.GetDescription() && p.UserRole != UserRoleTypeEnum.NGO.GetDescription()))
                    {
                        if (user.Replant.Any(p => string.IsNullOrEmpty(p.Parent) && p.Status == (int)StatusEnum.Submit))
                        {
                            foreach (var replant in user.Replant.FindAll(p => string.IsNullOrEmpty(p.Parent) && p.Status == (int)StatusEnum.Submit))
                            {
                                markers.Add(new MapMarker
                                {
                                    PinImage = "replant.png",
                                    Latitude = replant.Latitude,
                                    Longitude = replant.Longitude,
                                    Type = "Replant",
                                    Label = "replant",
                                    Cash = 0,
                                    GreenPoints = replant.GreenPoints,
                                    FileName = replant.FileName,
                                    PlantCount = replant.TreeCount,
                                    Description = replant.PlantNameDescription
                                });
                            }
                        }
                    }
                }
                return markers.Count > 0 ? ServiceResponse.SuccessReponse(markers, MessageEnum.DefaultSuccessMessage) : ServiceResponse.SuccessReponse(new List<MapMarker>(), MessageEnum.MapPointsNotFound);

            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<MapMarker>>(exp);
            }
        }

        [HttpGet("GetUserBinPoints")]
        public async Task<ResponseObject<List<MapMarker>>> GetUserBinPoints()
        {
            try
            {
                var lstUsersDetails = _IUWork.GetModelData<Users>(CollectionNames.USERS);
                List<MapMarker> markers = new List<MapMarker>();
                if (lstUsersDetails != null && lstUsersDetails?.Count > 0)
                {
                    foreach (var item in lstUsersDetails)
                    {
                        if (item.BuyBinDetails?.Count > 0)
                        {
                            foreach (var bin in item.BuyBinDetails)
                            {
                                List<FilterHelper> filter = new List<FilterHelper>
                                    {
                                        new FilterHelper
                                        {
                                            Field = "BinId",
                                            Value = bin.BinId

                                        }
                                    };

                                var binDetail = _IUWork.GetModelData<BinDetails>(filter, CollectionNames.BINDETAIL).FirstOrDefault();

                                markers.Add(new MapMarker
                                {
                                    PinImage = "bin.png",
                                    Latitude = item.Latitude,
                                    Longitude = item.Longitude,
                                    Type = "amalbin",
                                    Label = "amalbin",
                                    Cash = 0,
                                    FileName = binDetail?.FileName
                                });
                            }
                        }
                    }
                }
                return markers.Count > 0 ? ServiceResponse.SuccessReponse(markers, MessageEnum.DefaultSuccessMessage) : ServiceResponse.SuccessReponse(new List<MapMarker>(), MessageEnum.MapPointsNotFound);
            }
            catch (Exception exp)
            {
                return ServiceResponse.ErrorReponse<List<MapMarker>>(exp);
            }
        }
    }
}
