////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: SettingsPage.xaml.cs
//FileType: Visual C# Source file
//Author : Archie Vann, Mantas Burcikas
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : A class for handling all application's settings
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.IO;
using engie_maintenance_app.Interfaces;
using Plugin.SecureStorage;
using Xamarin.Forms;

namespace engie_maintenance_app.Views
{
    public partial class SettingsPage
    {
        public SettingsPage()
        {
            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetHasBackButton(this, true);
            InitializeComponent();
            BindingContext = new GlobalSettings();

            if (CrossSecureStorage.Current.GetValue("RoleType") == "Public")
            {
                DeleteButton.IsVisible = true;
            }
        }

        /// <summary>
        /// Deleted the CrossSecureStorage RoleType and Password
        /// Inserts LoginPage at the root of the Navigation stack then pops the Navigation stack to root
        /// </summary>
        /// <param name="sender">The sender has no effect on this method</param>
        /// <param name="args">Event data has no effect on this method</param>
        private async void LogoutProtocol(object sender, EventArgs args)
        {
            // Sets user values to null.
            CrossSecureStorage.Current.DeleteKey("RoleType");
            CrossSecureStorage.Current.DeleteKey("Password");
            CrossSecureStorage.Current.DeleteKey("Username");
            CrossSecureStorage.Current.DeleteKey("EngineerType");
            CrossSecureStorage.Current.DeleteKey("ID");

            // Send user to LoginPage.
            // Insert a LoginPage at the root of the stack then delete all above the root.
            Navigation.InsertPageBefore(new LoginPage(), Navigation.NavigationStack[0]);
            await Navigation.PopToRootAsync(true);
        }

        /// <summary>
        /// Pushes PrivacyPoliyPage to the top of the Navigation stack
        /// </summary>
        /// <param name="sender">The sender has no effect on this method</param>
        /// <param name="args">Event data has no effect on this method</param>
        private async void PrivacyPolicy(object sender, EventArgs args)
        {
            // Privacy Policy.
            await Navigation.PushAsync(new PrivacyPolicyPage());
        }

        /// <summary>
        /// Pushes LicencesPage to the top of the Navigation stack
        /// </summary>
        /// <param name="sender">The sender has no effect on this method</param>
        /// <param name="args">Event data has no effect on this method</param>
        private async void Licences(object sender, EventArgs args)
        {
            // Licences.
            await Navigation.PushAsync(new LicencesPage());
        }

        /// <summary>
        /// Pushes TermsOfUsePage to the top of the Navigation stack
        /// </summary>
        /// <param name="sender">The sender has no effect on this method</param>
        /// <param name="args">Event data has no effect on this method</param>
        private async void TermsOfUse(object sender, EventArgs args)
        {
            // Terms Of Use.
            await Navigation.PushAsync(new TermsOfUsePage());
        }

        /// <summary>
        /// Deletes all history
        /// </summary>
        /// <param name="sender">The sender has no effect on this method</param>
        /// <param name="args">Event data has no effect on this method</param>
        private async void DeleteAllHistory(object sender, EventArgs args)
        {
            bool response = await DisplayAlert("Important", "Are you sure you want to delete all your history?", "Yes",
                "No");

            if (response)
            {
                // Creates a user specific directory to keep current user files.
                var directoryPath =  DependencyService.Get<IDeviceOrientation>().
                    GetDirectory(CrossSecureStorage.Current.GetValue("ID"));

                // Checks if the directory exists.
                if (Directory.Exists(directoryPath))
                {
                    // Gets all the files from that directory.
                    var list = Directory.GetFiles(directoryPath, "*");

                    if (list.Length > 0) 
                    {
                        // Deletes all the files from the directory.
                        for (int i = 0; i < list.Length; i++)
                        {
                            File.Delete(list[i]);
                        }
                    }
                    // Finally deletes the directory path.
                    Directory.Delete(directoryPath);
                }
            }
        }

        /// <summary>
        /// Pushes ChangePasswordPage to the top of the Navigation stack
        /// </summary>
        /// <param name="sender">The sender has no effect on this method</param>
        /// <param name="args">Event data has no effect on this method</param>
        private async void ChangePasswordClicked(object sender, EventArgs args)
        {
            // Change Password Page.
            await Navigation.PushAsync(new ChangePasswordPage());
        }
    }  
}
