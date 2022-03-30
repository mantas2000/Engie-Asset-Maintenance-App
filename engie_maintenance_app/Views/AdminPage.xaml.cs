////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: AdminPage.xaml.cs
//FileType: Visual C# Source file
//Author : Mohammed Albulushi, Mantas Burcikas, Joel Carter
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : A class containing functions to redirect user to page linked with the buttons they tap. 
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using engie_maintenance_app.Network;
using SkiaSharp;
using Xamarin.Forms;

namespace engie_maintenance_app.Views
{
    public partial class AdminPage
    {
        public AdminPage()
        {
            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            if (Device.RuntimePlatform == Device.iOS)
            {
                BindingContext = new GlobalSettings();
            }
        }
        
        protected override void OnAppearing()
        {
            BindingContext = new GlobalSettings();
            base.OnAppearing();
        }
        
        /// <summary>
        /// Pushes a new HelpPage to the top of the Navigation stack
        /// </summary>
        /// <param name="sender">The sender has no effect on this method</param>
        /// <param name="args">Event data has no effect on this method</param>
        private async void RedirectToHelp(object sender, EventArgs args)
        {
            IsEnabled = false;
            await Navigation.PushAsync(new HelpPage());
            IsEnabled = true;
        }

        /// <summary>
        /// Pushes a new UploadNewFormPage to the top of the Navigation stack
        /// </summary>
        /// <param name="sender">The sender has no effect on this method</param>
        /// <param name="args">Event data has no effect on this method</param>
        private async void RedirectUploadForm(object sender, EventArgs args)
        {
            if (HasInternet.HasInternetCheck())
            {
                IsEnabled = false;
                await Navigation.PushAsync(new UploadNewFormPage());
                IsEnabled = true;
            }
            else
            {
                await DisplayAlert("Error",
                    "No internet connection, please make sure you are connected to the internet and try again.", "OK");
            }
        }

        /// <summary>
        /// Pushes a new SettingsPage to the top of the Navigation stack
        /// </summary>
        /// <param name="sender">The sender has no effect on this method</param>
        /// <param name="args">Event data has no effect on this method</param>
        private async void RedirectToSettings(object sender, EventArgs args)
        {
            IsEnabled = false;
            await Navigation.PushAsync(new SettingsPage());
            IsEnabled = true;
        }

        /// <summary>
        /// Pushes a new ChangeRecipientAddressPage to the top of the Navigation stack
        /// </summary>
        /// <param name="sender">The sender has no effect on this method</param>
        /// <param name="args">Event data has no effect on this method</param>
        private async void RedirectToChangeAddress(object sender, EventArgs args)
        {
            if (HasInternet.HasInternetCheck())
            {
                IsEnabled = false;
                await Navigation.PushAsync(new ChangeRecipientAddressPage());
                IsEnabled = true;
            }
            else
            {
                await DisplayAlert("Error",
                    "No internet connection, please make sure you are connected to the internet and try again.", "OK");
            }
        }
    }
}
