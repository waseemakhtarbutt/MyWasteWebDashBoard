using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static DrTech.Common.Extentions.Constants;
using DrTech.Common.Helpers;
using DrTech.Common.Extentions;
using Microsoft.AspNetCore.Http;
using System.IO;
using DrTech.Models.Dropdown;
using DrTech.DAL;
using System.Collections.Generic;
using System.Linq;
using DrTech.Common.Enums;
using DrTech.Models.ViewModels;
using DrTech.Models;

namespace DrTech.Services.Controllers
{
    public class BaseControllerBase : ControllerBase
    {
        protected IMongoDAL _Source;
        protected IMongoDAL _IUWork;

        public BaseControllerBase()
        {
            _Source = new MongoDAL();
        }

        public BaseControllerBase(string test="")
        {
            _IUWork = new MongoDAL("mongodb://10.200.10.33:27017/");
        }



        public string MakeRequestObject<TObject>(TObject requestObj)
        {
            var accesToken = Request.Headers["Authorization"];
            return accesToken;
        }

        public string GetLoggedInUserId()
        {

  

            var claims = User.Claims;
            foreach (var item in claims)
            {
                if (item.Type.ToLower() == "id")
                {
                    return item.Value;
                }
            }
            return "";
        }

        public int GetDropdownLastIndex(string colName)
        {
            var dd1 = _IUWork.GetModelData<DropdownDbViewModel>(Constants.CollectionNames.Lookups);


            return 1;
        }

        protected async Task<string> SaveFile(IFormFile file)
        {
            string Enviornment = AppSettingsHelper.GetEnvironmentValue(AppSettings.FILE_NAME, Constants.AppSettings.ENV);
            string host = "http://" + Convert.ToString(HttpContext.Request.Host) + "/Images"; //@Context.Request.Path
            string FileName = string.Empty;

            string fileName = "";

            if (Enviornment == Enviornemnt.CLOUD)
            {
                fileName = await FileOpsHelper.UploadFile(file);
            }
            else
            {
                string serverUploadFolder = System.AppDomain.CurrentDomain.BaseDirectory + "Images";
                string filePath = serverUploadFolder;
                var httpRequest = HttpContext.Request;
                string guid = Guid.NewGuid().ToString();
                var postedFile = httpRequest.Form.Files[0];
                FileName = guid + ".png";// +postedFile.FileName;
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
                filePath = filePath + "/" + FileName;

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                FileName = host + "/" + guid + ".png";

                fileName = FileName;

            }
            return fileName;
        }

        protected async Task<int> InsertDropdownValue(string decscription, string type, int value = 0)
        {
            var dd1 = _IUWork.GetModelData<DropdownDbViewModel>(CollectionNames.Lookups);
            var lastValue = (int)(dd1?.Count + 1);
            var dd = new DropdownDbViewModel { Value = lastValue, Description = decscription, Type = type };
            await _IUWork.InsertOne(dd, CollectionNames.Lookups);

            return lastValue;
        }
        protected async Task<DropdownViewModelWithTitle> GetDropDown(string type, bool isHasAnyValue = true, bool hasNone = true)
        {
            List<DropdownDbViewModel> list = null;

            if (type == CollectionNames.SCHOOL )
            {
                List<FilterHelper> filter = new List<FilterHelper>
                {
                    new FilterHelper
                    {
                        Field = "Type",
                        Value = "School"
                    }
                };

                list = _IUWork.GetModelData<DropdownDbViewModel>(filter, CollectionNames.SCHOOL);
            }
            else
            {

            
            List<FilterHelper> filter = new List<FilterHelper>
                {
                    new FilterHelper
                    {
                        Field = "Type",
                        Value = type
                    }
                };

                list = _IUWork.GetModelData<DropdownDbViewModel>(filter, CollectionNames.Lookups);
            }
            return MakeDropdown(list, EnumExtensionMethod.GetTitle<DropdownTypeEnum>(type), isHasAnyValue, hasNone);
            
        }
        protected DropdownViewModelWithTitle MakeDropdown(List<DropdownDbViewModel> list, string title, bool isHasAnyValue = true, bool hasNone = true)
        {
            var tempResult = InsertAnyAtStart(list?.ConvertAll(c => new DropdownViewModel
            {
                Description = c.Description,
                Value = c.Value
            }), isHasAnyValue, hasNone);

            return new DropdownViewModelWithTitle { Title = title, List = tempResult };
        }

        protected async Task<DropdownViewModelWithTitle> GetSubTypeDropDown(string id, bool isHasAnyValue = true, bool hasNone = true)
        {
            List<FilterHelper> filter = new List<FilterHelper>
                {
                    new FilterHelper
                    {
                        Field = "Value",
                        Value = id
                    }
                };
            DropdownDbViewModel dropdown = _IUWork.GetModelData<DropdownDbViewModel>(filter, CollectionNames.Lookups)?.FirstOrDefault();

            if (dropdown != null)
            {
                List<FilterHelper> SubTypefilter = new List<FilterHelper>
                {
                    new FilterHelper
                    {
                        Field = "ParentId",
                        Value = id
                    }
                };
                var subTypeDropdown = _IUWork.GetModelData<DropdownDbViewModel>(SubTypefilter, CollectionNames.Lookups);
                return MakeDropdown(subTypeDropdown, EnumExtensionMethod.GetTitle<DropdownTypeEnum>(subTypeDropdown?.FirstOrDefault()?.Type), isHasAnyValue, hasNone);
            }
            return MakeDropdown(new List<DropdownDbViewModel>(), "", isHasAnyValue, hasNone);
        }

        public List<DropdownViewModel> InsertAnyAtStart(List<DropdownViewModel> list, bool isHasAnyValue = true, bool hasNone = true)
        {
            if (hasNone)
            {
                var val = "Any";

                if (!isHasAnyValue)
                    val = "Select";

                list.Insert(0, new DropdownViewModel
                {
                    Description = val,
                    Value = 0
                });
            }
            return list;
        }

        // For School
        protected async Task<List<Schools>> GetSchoolDropDown()
        {

               List<FilterHelper> filter = new List<FilterHelper>
                {
                    new FilterHelper
                    {
                        Field = "ParentId",
                        Value = "0"
                    }
                };

               List<Schools> list = _IUWork.GetModelData<Schools>(filter, CollectionNames.SCHOOL);

            return list;
              //  return MakeDropdown(list, EnumExtensionMethod.GetTitle<DropdownTypeEnum>(type), isHasAnyValue, hasNone);

        }

        protected async Task<List<Schools>> GetSubTypeDropDown( string id)
        {

            List<FilterHelper> filter = new List<FilterHelper>
                {
                    new FilterHelper
                    {
                        Field = "ParentId",
                        Value = id
                    }
                };

            List<Schools> list = _IUWork.GetModelData<Schools>(filter, CollectionNames.SCHOOL);

            Schools MainBranch = await _IUWork.FindOneByID<Schools>(id, CollectionNames.SCHOOL);

            list.Add(MainBranch);


            list = list.OrderBy(d => d.ParentId).ToList();

            return list;
            //  return MakeDropdown(list, EnumExtensionMethod.GetTitle<DropdownTypeEnum>(type), isHasAnyValue, hasNone);

        }
    }
}