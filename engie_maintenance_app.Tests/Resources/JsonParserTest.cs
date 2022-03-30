////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: JsonParser.cs
//FileType: Visual C# Source file
//Author : Joel Carter
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : Test Class for parsing text data from the JSON files
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace engie_maintenance_app.Tests.Resources
{
    public class JsonParserTest
    {
        /// <summary>
        /// Get all JSON data from /Resources/Data/resourceData.json
        /// </summary>
        /// <returns>The JSON from a file as a QuestionList object</returns>
        /// 
        public string[] GetJsonData()
        {

            var json = File.ReadAllText("../../Resources/names.json");

            string[] result = JsonConvert.DeserializeObject<string[]>(json);

            // Return JSON QuestionList.
            return result;
        }
    }
}