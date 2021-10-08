//using Microsoft.AspNetCore.Http;
//using MongoDB.Driver.Core.Configuration;
using DrTech.Common.Extentions;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DrTech.Common.Helpers
{
    public static class FileOpsHelper
    {
        /// <summary>
        /// Uploads File and returns new Unique Uploaded File Name
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>

        static CloudBlobContainer _blobContainerPublic;
        public static string _containerSig = "";

        static FileOpsHelper()
        {
            string connectionString = AppSettingsHelper.GetAttributeValue(Constants.AppSettings.AZURE_SECTION, Constants.AppSettings.AZURE_CONSTR);
            var containerName = AppSettingsHelper.GetAttributeValue(Constants.AppSettings.AZURE_SECTION, Constants.AppSettings.AZURE_CONTAINER_PUBLIC);

            CloudStorageAccount _cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient _cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient();

            _blobContainerPublic = _cloudBlobClient.GetContainerReference(containerName);

        }

        public static async Task<string> UploadFile(IFormFile InputFile)
        {
            FileInfo fi = new FileInfo(InputFile.FileName);
            string UploadFileName = fi.Name.Substring(0, fi.Name.IndexOf(".")) + "_" + Guid.NewGuid().ToString() + fi.Extension;

            try
            {
                CloudBlockBlob BlockBlobPublic = _blobContainerPublic.GetBlockBlobReference(UploadFileName);
                BlockBlobPublic.Properties.ContentType = InputFile.ContentType;
                Stream OriginalFileStream = InputFile.OpenReadStream();
                await BlockBlobPublic.UploadFromStreamAsync(OriginalFileStream);
                return BlockBlobPublic.Uri.ToString();
            }
            catch (Exception e)
            {
                throw e;
            }

        }




    }
}
