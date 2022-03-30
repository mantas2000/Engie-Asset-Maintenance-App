////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: Question.cs
//FileType: Visual C# Source file
//Author : Mantas Burcikas
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : An object for storing Text Data for Help and Legal Pages
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using Xamarin.Forms;

namespace engie_maintenance_app
{
    public class Question
    {
        public string Header { get; set; }
        public string Content { get; set; }
        public double HeaderFontSize { get; set; } = new GlobalSettings().FontSize4;
        public double ContentFontSize { get; set; } = new GlobalSettings().FontSize1;
        public Color TextColor { get; set; } = new GlobalSettings().DarkMode ? new GlobalSettings().WhiteColor : new GlobalSettings().BlackColor;
    }

    public class QuestionList
    {
        public List<Question> PublicFaq { get; set; }
        public List<Question> AdminFaq { get; set; }
        public List<Question> Licences { get; set; }
        public List<Question> PrivacyPolicy { get; set; }
        public List<Question> TermsOfUse { get; set; }
    }
}