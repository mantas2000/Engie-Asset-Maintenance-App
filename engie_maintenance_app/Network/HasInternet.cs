////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: HasInternet.cs
//FileType: Visual C# Source file
//Author : Archie Vann, Joel Carter
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : A class containing function to check internet connection.
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Net;

namespace engie_maintenance_app.Network
{
    public class HasInternet
    {
        /// <summary>
        /// Checks if the user has internet
        /// </summary>
        /// <returns>False if the user cannot access google.com</returns>
        public static bool HasInternetCheck()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}