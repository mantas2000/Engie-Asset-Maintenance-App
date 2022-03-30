////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: App.xaml.cs
//FileType: Visual C# Source file
//Author : Joel Carter
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : App starting point
////////////////////////////////////////////////////////////////////////////////////////////////////////
using Xamarin.Forms;

[assembly: ExportFont("Lato-Regular.ttf", Alias = "LatoRegular")]
[assembly: ExportFont("Lato-Thin.ttf", Alias = "LatoThin")]
[assembly: ExportFont("Lato-Italic.ttf", Alias = "LatoItalic")]

[assembly: ExportFont("Lato-Black.ttf", Alias = "LatoBlack")]
[assembly: ExportFont("Lato-BlackItalic.ttf", Alias = "LatoBlackItalic")]

[assembly: ExportFont("Lato-Bold.ttf", Alias = "LatoBold")]
[assembly: ExportFont("Lato-BoldItalic.ttf", Alias = "LatoBoldItalic")]

[assembly: ExportFont("Lato-Hairline.ttf", Alias = "LatoHairline")]
[assembly: ExportFont("Lato-HairlineItalic.ttf", Alias = "LatoHairlineItalic")]

[assembly: ExportFont("Lato-Light.ttf", Alias = "LatoLight")]
[assembly: ExportFont("Lato-LightItalic.ttf", Alias = "LatoLightItalic")]

[assembly: ExportFont("Lato-Heavy.ttf", Alias = "LatoHeavy")]
[assembly: ExportFont("Lato-HeavyItalic.ttf", Alias = "LatoHeavyItalic")]

[assembly: ExportFont("Lato-Medium.ttf", Alias = "LatoMedium")]
[assembly: ExportFont("Lato-MediumItalic.ttf", Alias = "LatoMediumItalic")]

[assembly: ExportFont("Lato-Semibold.ttf", Alias = "LatoSemibold")]
[assembly: ExportFont("Lato-SemiboldItalic.ttf", Alias = "LatoSemiboldItalic")]

namespace engie_maintenance_app.Views
{
    public partial class App
    {
        public App()
        {
            InitializeComponent();
            BindingContext = new GlobalSettings();

            // Sets a new LoginPage as the root of the Navigation stack
            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
