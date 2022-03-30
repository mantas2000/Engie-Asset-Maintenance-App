////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: JsonParser.cs
//FileType: Visual C# Source file
//Author : Mantas Burcikas
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : Class for parsing text data from the JSON files
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace engie_maintenance_app.DataAccessLayer
{
    public class JsonParser
    {
        /// <summary>
        /// Get all JSON data from /Resources/Data/resourceData.json
        /// </summary>
        /// <returns>The JSON from a file as a QuestionList object</returns>
        public QuestionList GetJsonData()
        {
            // Name of the file.
            const string jsonFileName = "engie_maintenance_app.Resources.Data.resourceData.json";
            
            // Initialize object.
            QuestionList  questionList;
            
            // Initialize assembly.
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            
            // Initialize stream.
            Stream stream = assembly.GetManifestResourceStream(jsonFileName);
            
            // Read JSON file and store it as an 'Question' object.
            using (StreamReader reader = new StreamReader(stream ?? throw new InvalidOperationException()))
            {
                string jsonString = reader.ReadToEnd();

                questionList = JsonConvert.DeserializeObject<QuestionList>(jsonString);
            }
            
            // Return JSON QuestionList.
            return questionList;
        }
    }
}