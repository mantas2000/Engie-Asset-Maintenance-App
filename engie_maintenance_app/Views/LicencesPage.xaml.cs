////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: LicencesPage.xaml.cs
//FileType: Visual C# Source file
//Author : Mantas Burcikas, Archie Vann
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : A class for displaying Licence Page
////////////////////////////////////////////////////////////////////////////////////////////////////////
using Xamarin.Forms.Xaml;

namespace engie_maintenance_app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LicencesPage
    {
        public LicencesPage()
        {
            // Register Syncfusion license.
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mzg5MDAzQDMxMzgyZTM0MmUzMFFtRkZhWlpGWnArcDgyS0VEU2lDVTJiUFM1YmlZNEY4aVdNdjN1MmVPQVk9");
            
            InitializeComponent();
            
            // Parse Licences to the ListView.
            LicencesList.ItemsSource = new DataAccessLayer.JsonParser().GetJsonData().Licences;
            
            // Initialize Binding Context Settings.
            BindingContext = new GlobalSettings();
        }
    }
}