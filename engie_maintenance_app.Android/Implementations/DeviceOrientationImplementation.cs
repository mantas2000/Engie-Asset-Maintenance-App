////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: DeviceOrientationImplementation.cs
//FileType: Visual C# Source file
//Author : Mohammed Albulushi
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : A class containing android specific code for some functions
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.IO;
using Android.Content;
using Android.Graphics;
using engie_maintenance_app.Droid.Implementations;
using engie_maintenance_app.Interfaces;
using ZXing;
using ZXing.Common;
using File = Java.IO.File;
using Path = System.IO.Path;
using Uri = Android.Net.Uri;

[assembly: Xamarin.Forms.Dependency(typeof(DeviceOrientationImplementation))]
namespace engie_maintenance_app.Droid.Implementations
{
    public class DeviceOrientationImplementation : IDeviceOrientation
    {
        /// <summary>
        /// Creates binary bitmap from byte array
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public BinaryBitmap GetBinaryBitmap(byte[] image)
        {
            Bitmap bitmap = BitmapFactory.DecodeByteArray(image, 0, image.Length);
            byte[] rgbBytes = GetRgbBytes(bitmap);
            HybridBinarizer bin = new HybridBinarizer(new RGBLuminanceSource(rgbBytes, bitmap.Width, bitmap.Height));

            return new BinaryBitmap(bin);
        }

        /// <summary>
        /// Gets the RgbBytes from an image
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private byte[] GetRgbBytes(Bitmap image)
        {
            var rgbBytes = new List<byte>();
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color color = new Color(image.GetPixel(x, y));

                    rgbBytes.AddRange(new[] {color.R, color.G, color.B});
                }
            }

            return rgbBytes.ToArray();
        }

        /// <summary>
        /// creates a qr code and return it as a stream
        /// </summary>
        /// <param name="text"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Stream CreateQrCode(string text, int width = 500, int height = 500)
        {
            var barcodeWriter = new ZXing.Mobile.BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Width = width,
                    Height = height,
                    Margin = 10
                }
            };

            barcodeWriter.Renderer = new ZXing.Mobile.BitmapRenderer();
            Bitmap bitmap = barcodeWriter.Write(text);
            Stream stream = new MemoryStream();
            bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
            stream.Position = 0;
            
            return stream;
        }


        /// <summary>
        /// Saves the qr code into local device
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="imageData"></param>
        public void SavePictureToDevice(string filename, byte[] imageData)
        {
            var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
            var pictures = dir.AbsolutePath;
            // Adding a time stamp time file name to allow saving more than one image... otherwise it overwrites the previous saved image of the same name.
            string name = filename + System.DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
            string filePath = System.IO.Path.Combine(pictures, name);
            try
            {
                System.IO.File.WriteAllBytes(filePath, imageData);
                // Mediascan adds the saved image into the gallery.
                var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                mediaScanIntent.SetData(Uri.FromFile(new File(filePath)));
                Xamarin.Forms.Forms.Context.SendBroadcast(mediaScanIntent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// creates a copy of the selected form and creates a copy of it by the user provided name
        /// </summary>
        /// <param name="formBytes"></param>
        /// <param name="formname"></param>
        /// <returns></returns>
        public string CopyOfForm(byte[] formBytes, string formname)
        {
            Stream stream = new MemoryStream(formBytes);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filepath = Path.Combine(path, formname);

            FileStream outputFileStream = System.IO.File.Open(filepath, FileMode.Create);
            stream.Position = 0;
            stream.CopyTo(outputFileStream);
            outputFileStream.Close();

            return filepath;
        }

        /// <summary>
        /// creates specific folders for each user and returns it full path
        /// </summary>
        /// <param name="DirectoryName"></param>
        /// <returns></returns>
        public string GetDirectory(String DirectoryName)
        {
            string documentBasePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var directoryPath = Path.Combine(documentBasePath, DirectoryName);  
            if (!Directory.Exists(directoryPath)) {  
                Directory.CreateDirectory(directoryPath);  
            }

            return directoryPath;
        }
    }
}




