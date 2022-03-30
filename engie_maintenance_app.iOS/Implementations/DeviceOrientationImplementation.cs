////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: DeviceOrientationImplementation.cs
//FileType: Visual C# Source file
//Author : Mohammed Albulushi
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : A class containing iOS specific code for some functions
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.IO;
using CoreGraphics;
using engie_maintenance_app.Interfaces;
using engie_maintenance_app.iOS.Implementations;
using Foundation;
using UIKit;
using ZXing;
using ZXing.Common;

[assembly: Xamarin.Forms.Dependency(typeof(DeviceOrientationImplementation))]
namespace engie_maintenance_app.iOS.Implementations
{
    public class DeviceOrientationImplementation : IDeviceOrientation
    {
        /// <summary>
        /// Creates binary bitmap from byte array
        /// </summary>
        /// <param name="imageByte"></param>
        /// <returns></returns>
        public BinaryBitmap GetBinaryBitmap(byte[] imageByte)
        {
            Stream imageStream = new MemoryStream(imageByte);

            UIImage image = new UIImage(NSData.FromStream(imageStream));

            // First get the image into your data buffer.
            CGImage imageRef = image.CGImage;
            string width = imageRef.Width.ToString();
            var w = Convert.ToInt32(width);
            string height = imageRef.Height.ToString();
            var h = Convert.ToInt32(height);

            byte[] rgbBytes = GetRgbBytes(image);

            var bin = new HybridBinarizer(new RGBLuminanceSource(rgbBytes, w, h));

            return new BinaryBitmap(bin);
        }

        /// <summary>
        /// Gets the rgbbytes from an image
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private byte[] GetRgbBytes(UIImage image)
        {
            var rgbBytes = new List<byte>();

            NSMutableArray<UIColor> result = new NSMutableArray<UIColor>();

            // First get the image into your data buffer
            CGImage imageRef = image.CGImage;
            nint width = imageRef.Width;
            nint height = imageRef.Height;
            nint bytesPerPixel = 4;
            nint bytesPerRow = bytesPerPixel * width;
            nint bitsPerComponent = 8;

            CGColorSpace colorSpace = CGColorSpace.CreateDeviceRGB();
            byte[] rawData = new byte[height * width * 4];
      
            CGContext context = new CGBitmapContext(rawData, width, height, bitsPerComponent, bytesPerRow, colorSpace,
                CGBitmapFlags.ByteOrder32Big | CGBitmapFlags.PremultipliedLast);
            colorSpace.Dispose();

            context.DrawImage(new CGRect(0, 0, width, height), imageRef);
            context.Dispose();

            // Now your rawData contains the image data in the RGBA8888 pixel format.
            nint byteIndex = 0;

            string count = (image.Size.Width * image.Size.Height).ToString();
            int c = Convert.ToInt32(count);

            for (int i = 0; i < c; ++i)
            {
                byte alpha = rawData[byteIndex + 3];
                byte red = rawData[byteIndex];
                byte green = rawData[byteIndex + 1];
                byte blue = rawData[byteIndex + 2];
                byteIndex += bytesPerPixel;

                UIColor acolor = new UIColor(red, green, blue, alpha);
                result.Add(acolor);

                rgbBytes.AddRange(new[] {red, green, blue});
            }
            return rgbBytes.ToArray();
        }

        /// <summary>
        /// Creates qr code and return it as a stream
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
            var bitmap = barcodeWriter.Write(text);
            var stream = bitmap.AsPNG().AsStream();
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
            var chartImage = new UIImage(NSData.FromArray(imageData));
            chartImage.SaveToPhotosAlbum((image, error) =>
            {
                // You can retrieve the saved UI Image as well if needed using  
                if (error != null)
                {
                    Console.WriteLine(error.ToString());
                }
            });
        }

        /// <summary>
        /// Creates a copy of the selected form and creates a copy of it by the user provided name
        /// </summary>
        /// <param name="formBytes"></param>
        /// <param name="formname"></param>
        /// <returns></returns>
        public string CopyOfForm(byte[] formBytes, string formname)
        {
            Stream stream = new MemoryStream(formBytes);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filepath = Path.Combine(path, formname);

            FileStream outputFileStream = File.Open(filepath, FileMode.Create);
            stream.Position = 0;
            stream.CopyTo(outputFileStream);
            outputFileStream.Close();

            return filepath;
        }

        /// <summary>
        /// Creates specific folders for each user and returns it full path
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