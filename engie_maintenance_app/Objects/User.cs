////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: CustomEntry.cs
//FileType: Visual C# Source file
//Author : Mantas Burcikas, Joel Carter
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : An object for storing users' details
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Xml.Serialization;

namespace engie_maintenance_app.Objects
{
    [XmlRoot(Namespace = "http://velocitysolutions.tk/",
        ElementName = "CompanName",
        DataType = "string",
        IsNullable = true)]
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Salt { get; set; }
        public string RoleType { get; set; }
        public string EngineerType { get; set; }
    }
}