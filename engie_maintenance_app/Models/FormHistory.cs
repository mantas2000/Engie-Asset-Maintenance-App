////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: FormHistory.cs
//FileType: Visual C# Source file
//Author : Mohammed Albulushi
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : A class for creating local database table and storing .net class objects 
////////////////////////////////////////////////////////////////////////////////////////////////////////
using SQLite;

namespace engie_maintenance_app
{
    [Table("FormHistory")]
    public class FormHistory
    {
        // Defines the columns for the local FormHistory databse
        [PrimaryKey, Column("FormHistoryId"), AutoIncrement]
        public int FormHistoryId { get; set; }
        
        [Column("FormName"), NotNull]
        public string FormName { get; set; }
        
        [Column("Date"), NotNull]
        public string Date { get; set; }
        
        [Column("Status"), NotNull]
        public string Status { get; set; }
        
        public double HeaderFontSize { get; set; } = new GlobalSettings().FontSize4;
        public double ContentFontSize { get; set; } = new GlobalSettings().FontSize2;
        public double StatusFontSize { get; set; } = new GlobalSettings().FontSize6;

        /// <summary>
        /// Is the delete button visible.
        /// </summary>
        public bool DeleteButtonIsVisible { get; set; }
        /// <summary>
        /// Is the edit button visible.
        /// </summary>
        public bool EditButtonIsVisible { get; set; }
        /// <summary>
        /// Is the retry button visible.
        /// </summary>
        public bool RetryButtonIsVisible { get; set; }
        
        /// <summary>
        /// Stores the data about a form.
        /// </summary>
        /// <param name="date">The time and date the form was created.</param>
        /// <param name="formName">The name of the form.</param>
        /// <param name="status">The current status of the form.</param>
        public FormHistory(string date, string formName, string status)
        {
            Date = date;
            FormName = formName;
            Status = status;
        }

        /// <summary>
        /// A blank FormHistory that can store the data about a form.
        /// </summary>
        public FormHistory()
        {
            
        }
    }
}