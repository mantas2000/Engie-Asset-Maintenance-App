////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: SqliteExtension.cs
//FileType: Visual C# Source file
//Author : Joel Carter
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : Class for connecting to the aws bucket to store and download files
////////////////////////////////////////////////////////////////////////////////////////////////////////
using engie_maintenance_app.Interfaces;
using Plugin.SecureStorage;
using SQLite;
using Xamarin.Forms;

namespace engie_maintenance_app.Database
{
    public static class SQLiteExtension
    {
        /// <summary>
        /// Gets a connection to the local database. If the database doesn't exist;
        /// creates one and creates a connection to it. When the connection is established;
        /// creates a table for FormHistory objects if one does not exist.
        /// </summary>
        /// <returns>The connection</returns>
        public static SQLiteConnection GetConnection()
        {
            string directoryPath =  DependencyService.Get<IDeviceOrientation>().GetDirectory(CrossSecureStorage.Current.GetValue("ID"));
            SQLiteConnection connection = DependencyService.Get<ISQLite>().GetConnection(directoryPath);
            connection.CreateTable<FormHistory>();

            return connection;
        }
    }
}