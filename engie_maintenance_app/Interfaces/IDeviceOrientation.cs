////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: IDeviceOrientation.cs
//FileType: Visual C# Source file
//Author : Mohammed Albulushi
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : This is an interface to run platform specific code
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.IO;
using ZXing;
namespace engie_maintenance_app.Interfaces
{
    /// <summary>
    /// Interface to run platform specific code.
    /// </summary>
    public interface IDeviceOrientation
    {
        /// <summary>
        /// Gets the binaryBitmap from a given byte array.
        /// </summary>
        /// <param name="image">The byte array to be returned as a binaryBitmap.</param>
        /// <returns>A binaryBitmap representative of the given byte array.</returns>
        BinaryBitmap GetBinaryBitmap(byte[] image);
        
        /// <summary>
        /// Creates a QR code of given height and width of the given string.
        /// </summary>
        /// <param name="text">The text to convert into a QR code.</param>
        /// <param name="width">The width of the QR code.</param>
        /// <param name="height">The height of the QR code.</param>
        /// <returns>A QR code Stream.</returns>
        Stream CreateQrCode(string text, int width, int height);    
        
        /// <summary>
        /// Saves given image as given filename.
        /// </summary>
        /// <param name="filename">The name to give the newly created file.</param>
        /// <param name="imageData">The data to save as an image.</param>
        void SavePictureToDevice(string filename, byte[] imageData);

        /// <summary>
        /// Copies a form from given formBytes to the given formName.
        /// </summary>
        /// <param name="formBytes">The data a copy will be made of.</param>
        /// <param name="formName">The name of the copy.</param>
        /// <returns></returns>
        string CopyOfForm(byte[] formBytes, string formName);

        /// <summary>
        /// Gets the full directory path from a directory name.
        /// </summary>
        /// <param name="directoryName">The name of a directory.</param>
        /// <returns>The full path of that directory.</returns>
        string GetDirectory(string directoryName);
    }
}