////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: HelpPage.xaml.cs
//FileType: Visual C# Source file
//Author : Mantas Burcikas, Archie Vann, Joel Carter
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : A class for displaying Public and Admin FAQ's
////////////////////////////////////////////////////////////////////////////////////////////////////////
using Plugin.SecureStorage;
using Xamarin.Forms.Xaml;

namespace engie_maintenance_app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HelpPage
    {
        public HelpPage()
        {
            // Register Syncfusion license.
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mzg5MDAzQDMxMzgyZTM0MmUzMFFtRkZhWlpGWnArcDgyS0VEU2lDVTJiUFM1YmlZNEY4aVdNdjN1MmVPQVk9");
            
            InitializeComponent();
            
            // Check user's role type and load it's role's FAQ.
            if (CrossSecureStorage.Current.GetValue("RoleType") == "Public")
            {
                // Parse Public FAQ questions to the ListView.
                HelpList.ItemsSource = new DataAccessLayer.JsonParser().GetJsonData().PublicFaq;
            }
            else if (CrossSecureStorage.Current.GetValue("RoleType") == "Admin")
            {
                // Parse Admin FAQ questions to the ListView.
                HelpList.ItemsSource = new DataAccessLayer.JsonParser().GetJsonData().AdminFaq;
            }
            
            // Initialize Binding Context Settings.
            BindingContext = new GlobalSettings();
        }
    }
}