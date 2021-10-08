//using System;
//using System.Collections.Generic;
//using System.Text;
//using Microsoft.AspNetCore;
//using System.IO;
//using ZXing;
//using ZXing.Common;
//using System.DrawingCore.Imaging;
//using Microsoft.AspNetCore.Razor.TagHelpers;
//using ZXing.QrCode;

//namespace DrTech.Common.Helpers
//{
//    public static  class QRCodeTagHelper 
//    {
//        public static  string QRCodeGenerator(StringBuilder str)
//        {
//            var QrcodeContent = str.ToString(); 
//            var alt = ""; 
//            var width = 1000; 
//            var height = 1000; 
//            var margin = 0;
//            var qrCodeWriter = new ZXing.BarcodeWriterPixelData
//            {
//                Format = ZXing.BarcodeFormat.QR_CODE,
//                Options = new QrCodeEncodingOptions { Height = height, Width = width, Margin = margin }
//            };
//            var pixelData = qrCodeWriter.Write(QrcodeContent);
//            using (var bitmap = new System.DrawingCore.Bitmap(pixelData.Width, pixelData.Height, System.DrawingCore.Imaging.PixelFormat.Format32bppRgb))
//            using (var ms = new MemoryStream())
//            {
//                var bitmapData = bitmap.LockBits(new System.DrawingCore.Rectangle(0, 0, pixelData.Width, pixelData.Height),
//                System.DrawingCore.Imaging.ImageLockMode.WriteOnly, System.DrawingCore.Imaging.PixelFormat.Format32bppRgb);
//                try
//                {
//                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0,
//                    pixelData.Pixels.Length);
//                }
//                finally
//                {
//                    bitmap.UnlockBits(bitmapData);
//                }
//                bitmap.Save(ms, System.DrawingCore.Imaging.ImageFormat.Png);
//              return  String.Format("data:image/png;base64,{0}", Convert.ToBase64String(ms.ToArray())); //);
//            }
//        }
//    }
//}
