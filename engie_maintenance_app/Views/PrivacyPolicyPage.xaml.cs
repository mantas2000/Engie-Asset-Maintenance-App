////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: PrivacyPolicyPage.xaml.cs
//FileType: Visual C# Source file
//Author : Mantas Burcikas, Archie Vann
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : A class for displaying Privacy Policy Page
////////////////////////////////////////////////////////////////////////////////////////////////////////
using Xamarin.Forms.Xaml;

namespace engie_maintenance_app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrivacyPolicyPage
    {
        public PrivacyPolicyPage()
        {
            // Register Syncfusion license.
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mzg5MDAzQDMxMzgyZTM0MmUzMFFtRkZhWlpGWnArcDgyS0VEU2lDVTJiUFM1YmlZNEY4aVdNdjN1MmVPQVk9");
            
            InitializeComponent();

            // Parse Licences to the ListView.
            PrivacyPolicyList.ItemsSource = new DataAccessLayer.JsonParser().GetJsonData().PrivacyPolicy;

            // Initialize Binding Context Settings.
            BindingContext = new GlobalSettings();
        }
    }
}