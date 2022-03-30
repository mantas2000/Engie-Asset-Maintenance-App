////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: SQLite_Android.cs
//FileType: Visual C# Source file
//Author : Mohammed Albulushi
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : A class containing function to get connection for android local database
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.IO;
using engie_maintenance_app.Droid.Implementations;
using engie_maintenance_app.Interfaces;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLite_Android))]
namespace engie_maintenance_app.Droid.Implementations
{
    public class SQLite_Android : ISQLite
    {
        public SQLiteConnection GetConnection(string documentPath)
        {
            // Gets the path of the database
            var sqliteFilename = "FormHistory.db3";
            string documentsPath = documentPath;
            var path = Path.Combine(documentsPath, sqliteFilename);

            // Create the connection
            var conn = new SQLiteConnection(path);
            
            // Return the database connection
            return conn;
        }
    }

}