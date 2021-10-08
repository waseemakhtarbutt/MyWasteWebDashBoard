//using Microsoft.AspNetCore.Http;
//using MongoDB.Driver.Core.Configuration;
//using DrTech.Common.Extentions;
//using Microsoft.AspNetCore.Http;
//using Microsoft.WindowsAzure.Storage;
//using Microsoft.WindowsAzure.Storage.Blob;
//using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Web;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using static DrTech.Amal.Common.Extentions.Constants;

namespace DrTech.Amal.Common.Helpers
{
    public static class FileOpsHelper 
    {
        /// <summary>
        /// Uploads File and returns new Unique Uploaded File Name
        /// </summary>
        /// <param name = "file" ></ param >
        /// < returns ></ returns >


        static CloudBlobContainer _blobContainerPublic;
        public static string _containerSig = "";

        static FileOpsHelper()
        {
            //string connectionString = "DefaultEndpointsProtocol=https;AccountName=amalforlife;AccountKey=hu+4cpPNEQPgSvwRn9xAjLIE1i+PWry3gsYAl+/YXx8PAIz/Y5vZpI1jo72KgsvHonrqAM43+O3zwQOSsQpLkw==;EndpointSuffix=core.windows.net"; // System.Configuration.ConfigurationManager.AppSettings[AppSettings.Blobe_String].ToString();
            //var containerName = "amal-recycle"; // System.Configuration.ConfigurationManager.AppSettings[AppSettings.AZURE_CONTAINER_PUBLIC].ToString();

          //  string connectionString = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://10.200.10.33:10000/devstoreaccount1;QueueEndpoint=http://10.200.10.33:10001/devstoreaccount1;TableEndpoint=http://10.200.10.33:10002/devstoreaccount1;"; // System.Configuration.ConfigurationManager.AppSettings[AppSettings.Blobe_String].ToString();

            //string connectionString = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";

            //string connectionString = System.Configuration.ConfigurationManager.AppSettings[AppSettings.Blobe_String].ToString();

            //var containerName = "recycle"; // System.Configuration.ConfigurationManager.AppSettings[AppSettings.AZURE_CONTAINER_PUBLIC].ToString();

            //CloudStorageAccount _cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            //CloudBlobClient _cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient();

          

        }

        //public static async Task<string> UploadFile(IFormFile InputFile)
        //{
        //    FileInfo fi = new FileInfo(InputFile.FileName);
        //    string UploadFileName = fi.Name.Substring(0, fi.Name.IndexOf(".")) + "_" + Guid.NewGuid().ToString() + fi.Extension;
        //    try
        //    {
        //        CloudBlockBlob BlockBlobPublic = _blobContainerPublic.GetBlockBlobReference(UploadFileName);
        //        BlockBlobPublic.Properties.ContentType = InputFile.ContentType;
        //        Stream OriginalFileStream = InputFile.OpenReadStream();
        //        await BlockBlobPublic.UploadFromStreamAsync(OriginalFileStream);

        //        return BlockBlobPublic.Uri.ToString();
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }

        //}


        public static async Task<string> UploadFileNew(HttpPostedFile InputFile, string containerName)
        {

            string connectionString = System.Configuration.ConfigurationManager.AppSettings[AppSettings.Blobe_String].ToString();

           // var containerName = "recycle"; // System.Configuration.ConfigurationManager.AppSettings[AppSettings.AZURE_CONTAINER_PUBLIC].ToString();

            CloudStorageAccount _cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient _cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient();

            _blobContainerPublic = _cloudBlobClient.GetContainerReference(containerName);


            _cloudBlobClient.AuthenticationScheme = AuthenticationScheme.SharedKey;

            //if (_blobContainerPublic.CreateIfNotExistsAsync().Result)
            //{
          await  _blobContainerPublic.SetPermissionsAsync(
                new BlobContainerPermissions
                {
                    PublicAccess =
                        BlobContainerPublicAccessType.Blob
                });
            //}


            FileInfo fi = new FileInfo(InputFile.FileName);
            string UploadFileName = fi.Name.Substring(0, fi.Name.IndexOf(".")) + "_" + Guid.NewGuid().ToString() + fi.Extension;
            try
            {
                CloudBlockBlob BlockBlobPublic = _blobContainerPublic.GetBlockBlobReference(UploadFileName);
                BlockBlobPublic.Properties.ContentType = InputFile.ContentType;
                Stream OriginalFileStream = InputFile.InputStream;
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
