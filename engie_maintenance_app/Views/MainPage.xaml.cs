////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: MainPage.xaml.cs
//FileType: Visual C# Source file
//Author : Mantas Burcikas, Joel Carter
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : A class for displaying Licence Page
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using Xamarin.Forms;

namespace engie_maintenance_app.Views
{
    public partial class MainPage
    {
        public MainPage()
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
        /// Pushes new SettingsPage to top of Navigation stack
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
        /// Pushes new SettingsPage to top of Navigation stack
        /// </summary>
        /// <param name="sender">The sender has no effect on this method</param>
        /// <param name="args">Event data has no effect on this method</param>
        private async void RedirectToQueue(object sender, EventArgs args)
        {
            IsEnabled = false;
            await Navigation.PushAsync(new QueuePage());
            IsEnabled = true;
        }

        /// <summary>
        /// Pushes new SettingsPage to top of Navigation stack
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
        /// Pushes new SettingsPage to top of Navigation stack
        /// </summary>
        /// <param name="sender">The sender has no effect on this method</param>
        /// <param name="args">Event data has no effect on this method</param>
        private async void ScanButtonClicked(object sender, EventArgs args)
        {
            IsEnabled = false;
            await Navigation.PushAsync(new QrScanningPage());
            IsEnabled = true;
        }
    }
}