////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: ISQLite.cs
//FileType: Visual C# Source file
//Author : Mohammed Albulushi
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : A interface to get platform specific local database connection.
////////////////////////////////////////////////////////////////////////////////////////////////////////
using SQLite;

namespace engie_maintenance_app.Interfaces
{
    public interface ISQLite
    {
        /// <summary>
        /// Gets the connection to the local database.
        /// </summary>
        /// <returns>The connection to the local database.</returns>
        SQLiteConnection GetConnection(string documentPath);
    }
}