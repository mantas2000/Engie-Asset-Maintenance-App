////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: FormMethods.cs
//FileType: Visual C# Source file
//Author : Joel Carter
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : Class to run methods on form files
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.IO;
namespace engie_maintenance_app.EmailService
{
    public class FormMethods
    {
        /// <summary>
        /// Get the type of form from the given path.
        /// </summary>
        /// <param name="filePath">The path to the form.</param>
        /// <returns>The type of the form</returns>
        public static string GetFormTypeFromPath(string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            int end = fileName.IndexOf("_");
            
            // Sets Backup form type to prevent crash.
            string formType = "test";
            if (end != -1)
            {
                formType = fileName.Substring(0,end);
            }

            return formType;
        }
    }
}