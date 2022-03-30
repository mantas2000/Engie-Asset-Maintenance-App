////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: SQLite_iOS.cs
//FileType: Visual C# Source file
//Author : Mohammed Albulushi
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : A class containing function to get connection for iOS local database
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.IO;
using engie_maintenance_app.Interfaces;
using engie_maintenance_app.iOS.Implementations;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLite_iOS))]
namespace engie_maintenance_app.iOS.Implementations
{
    public class SQLite_iOS : ISQLite
    {
        public SQLiteConnection GetConnection(string documentPath)
        {
            // Gets the path of the database
            var sqliteFilename = "FormHistory.db3";
            string documentsPath = documentPath;
            var path = Path.Combine(documentsPath, sqliteFilename);

            // Create the connection
            var conn = new SQLiteConnection(path);

            return conn;
        }
    }
}