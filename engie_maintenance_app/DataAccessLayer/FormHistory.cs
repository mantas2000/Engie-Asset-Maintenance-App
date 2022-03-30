////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: FormHistory.cs
//FileType: Visual C# Source file
//Author : Mohammed Albulushi
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : Class for local database functions
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using System.Threading.Tasks;
using engie_maintenance_app.Database;

namespace engie_maintenance_app.DataAccessLayer
{
    public static class FormHistory
    {
        /// <summary>
        /// Creates a connections to local db and returns all of the data
        /// </summary>
        /// <returns>Task list of form history</returns>
        public static Task<List<engie_maintenance_app.FormHistory>> GetAll()
        {
            List<engie_maintenance_app.FormHistory> data = null;

            using (var connection = SQLiteExtension.GetConnection())
            {
                data = connection.Table<engie_maintenance_app.FormHistory>().ToList();
            }

            return Task.FromResult(data);
        }

        /// <summary>
        /// Checks whether to update the database for the current form
        /// or create a new history, if there is a form of the same name and
        /// status is not equal to success it updates that form history
        /// with the updated history for that form.
        /// </summary>
        /// <param name="form">The form to update</param>
        /// <returns></returns>
        public static Task<bool> Update(engie_maintenance_app.FormHistory form)
        {

            List<engie_maintenance_app.FormHistory> result = null;
            using (var _connection = SQLiteExtension.GetConnection())
            {
                result = _connection.Table<engie_maintenance_app.FormHistory>()
                    .ToList();
            }

            if (result != null)
            {
                for (int i = 0; i < result.Count; i++)
                {
                    if (result[i].FormName == form.FormName && !result[i].Status.Equals("Success"))
                    {
                        return Edit(form);
                    }
                }
            }

            return Add(form);
        }

        /// <summary>
        /// adds a new form history to the database
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static Task<bool> Add(engie_maintenance_app.FormHistory form)
        {
            using (var _connection = SQLiteExtension.GetConnection())
            {
                _connection.Insert(form);
            }

            return Task.FromResult(true);
        }


        /// <summary>
        /// Edits the form history from database
        /// </summary>
        /// <param name="form">The form to edit</param>
        /// <returns></returns>
        public static Task<bool> Edit(engie_maintenance_app.FormHistory form)
        {
            using (var connection = SQLiteExtension.GetConnection())
            {
                string sqlQuery =
                    $"Update FormHistory set Date = ?, Status =? where FormName = ? AND Status != ?";
                var ret = connection.Execute(sqlQuery, form.Date, form.Status, form.FormName, "Success");
            }

            return Task.FromResult(true);
        }

        /// <summary>
        /// Deletes form history from database
        /// </summary>
        /// <param name="formID">The form that's history will be deleted</param>
        /// <returns></returns>
        public static Task<bool> Delete(string formID)
        {
            using (var connection = SQLiteExtension.GetConnection())
            {
                string sqlQuery = $"DELETE FROM FormHistory WHERE FormHistoryId = ?";
                var ret = connection.Execute(sqlQuery, formID);
            }

            return Task.FromResult(true);
        }
    }
}