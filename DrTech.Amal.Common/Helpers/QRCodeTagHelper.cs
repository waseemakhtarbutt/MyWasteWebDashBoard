using System;
using System.Collections.Generic;
using System.Text;
//using Microsoft.AspNetCore;
using System.IO;
using ZXing;
using ZXing.Common;
//using System.DrawingCore.Imaging;
//using Microsoft.AspNetCore.Razor.TagHelpers;
using ZXing.QrCode;
using System.Web;
using System.DrawingCore;

namespace DrTech.Amal.Common.Helpers
{
    public static  class QRCodeTagHelper 
    {
        public static string QRCodeGenerator(StringBuilder str)
        {

            var QrcodeContent = str.ToString();
            var alt = "";
            var width = 400;
            var height = 400;
            var margin = 0;
            var qrCodeWriter = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions { Height = height, Width = width, Margin = margin }

            };
            var pixelData = qrCodeWriter.Write(QrcodeContent);
            using (var bitmap = new System.DrawingCore.Bitmap(pixelData.Width, pixelData.Height, System.DrawingCore.Imaging.PixelFormat.Format32bppRgb))
            using (var ms = new MemoryStream())
            {
                var bitmapData = bitmap.LockBits(new System.DrawingCore.Rectangle(0, 0, pixelData.Width, pixelData.Height),
                System.DrawingCore.Imaging.ImageLockMode.WriteOnly, System.DrawingCore.Imaging.PixelFormat.Format32bppRgb);
                try
                {
                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0,
                    pixelData.Pixels.Length);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }
                bitmap.Save(ms, System.DrawingCore.Imaging.ImageFormat.Png);
                return String.Format("data:image/png;base64,{0}", Convert.ToBase64String(ms.ToArray())); //);
            }
         ///   return "";
        }

        public static string QRCodeGeneratorImage(StringBuilder str)
        {

            var QrcodeContent = str.ToString();
            var alt = "";
            var width = 400;
            var height = 400;
            var margin = 0;
            var qrCodeWriter = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions { Height = height, Width = width, Margin = margin }

            };

            //   object o = new { Email = "abcd", Name =  "" };


            var pixelData = qrCodeWriter.Write(QrcodeContent);
            // var pixelData = qrCodeWriter.Write(o.ToString());
            using (var bitmap = new System.DrawingCore.Bitmap(pixelData.Width, pixelData.Height, System.DrawingCore.Imaging.PixelFormat.Format32bppRgb))
            using (var ms = new MemoryStream())
            {
                var bitmapData = bitmap.LockBits(new System.DrawingCore.Rectangle(0, 0, pixelData.Width, pixelData.Height),
                System.DrawingCore.Imaging.ImageLockMode.WriteOnly, System.DrawingCore.Imaging.PixelFormat.Format32bppRgb);
                try
                {
                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0,
                    pixelData.Pixels.Length);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }
                string imagePath = Guid.NewGuid() + ".png";
                bitmap.Save(HttpContext.Current.Server.MapPath("~/Template/images/") + imagePath, System.DrawingCore.Imaging.ImageFormat.Png);
                // bitmap.Save( System.DrawingCore.Imaging.ImageFormat.png);
                return "~/Template/images/" + imagePath;//String.Format("data:image/png;base64,{0}", Convert.ToBase64String(ms.ToArray())); //);
            }
     
            ///   return "";
        }

        public static string btnGenerate_Click(StringBuilder str)
        {
            try
            {
                var QrcodeContent = str.ToString();
                var alt = "";
                var width = 400;
                var height = 400;
                var margin = 0;
                var qrCodeWriter = new ZXing.BarcodeWriterPixelData
                {
                    Format = ZXing.BarcodeFormat.QR_CODE,
                    Options = new QrCodeEncodingOptions { Height = height, Width = width, Margin = margin }

                };


                var pixelData = qrCodeWriter.Write(QrcodeContent);

                System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();

                imgBarCode.Height = 150;

                imgBarCode.Width = 150;

                using (Bitmap bitMap = new System.DrawingCore.Bitmap(pixelData.Width, pixelData.Height, System.DrawingCore.Imaging.PixelFormat.Format32bppRgb))
                {

                    using (MemoryStream ms = new MemoryStream())

                    {
                        bitMap.Save(ms, System.DrawingCore.Imaging.ImageFormat.Png);
                        byte[] byteImage = ms.ToArray();
                        System.DrawingCore.Image img = System.DrawingCore.Image.FromStream(ms);
                        img.Save(HttpContext.Current.Server.MapPath("~/MYHTML/images/") + "Test.jpeg", System.DrawingCore.Imaging.ImageFormat.Jpeg);
                    }
                    return imgBarCode.ToString();
                }
            }

            catch (Exception e)
            {
                return null;
            }
        }
    }
}
