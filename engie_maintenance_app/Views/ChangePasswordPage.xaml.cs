////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: ChangePasswordPage.xaml.cs
//FileType: Visual C# Source file
//Author : Mohammed Albulushi, Mantas Burcikas
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : A class to handle users change password requests.
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using engie_maintenance_app.Network;
using engie_maintenance_app.Objects;
using engie_maintenance_app.Security;
using engie_maintenance_app.WebServices;
using Plugin.SecureStorage;
using Xamarin.Forms.Xaml;

namespace engie_maintenance_app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangePasswordPage
    {
        private readonly string _email;

        public ChangePasswordPage()
        {
            InitializeComponent();

            BindingContext = new GlobalSettings();
            if (CrossSecureStorage.Current.HasKey("Email"))
            {
                // Gets the user email saved on local device.
                _email = CrossSecureStorage.Current.GetValue("Email");
                label.Text = "Change your password: " + _email;
            }
        }

        /// <summary>
        /// If the password in the "old password" field matches the password in the database
        /// and the new passwords match
        /// updates that password in the database with the new password.
        /// </summary>
        /// <param name="sender">The sender has no effect on this method</param>
        /// <param name="args">Event data has no effect on this method</param>
        private async void ChangePasswordClicked(object sender, EventArgs e)
        {
            if (HasInternet.HasInternetCheck())
            {
                // Gets the user inputs.
                var oldPassword = OldPasswordEntry.Text;
                var newPassword1 = NewPasswordEntry1.Text;
                var newPassword2 = NewPasswordEntry2.Text;

                // Connects to db and gets the current hashed password and salt .. for the user.
                User userDetails = new User();
                try
                {
                    userDetails = await WebServices.WebServices.GetUserDetails(_email);
                }
                catch (NoInternetException)
                {
                    await DisplayAlert(NoInternetException.ErrorHeader,
                        NoInternetException.ErrorBody,
                        NoInternetException.ErrorButton);
                    return;
                }

                if (userDetails.Password == null)
                {
                    return;
                }
                var currentPassword = userDetails.Password;
                var salt = userDetails.Salt;
            
                // Hashes old password provided by the user using the salt from db.
                var oldPasswordHashed = Sha256Hash.GenerateHash(oldPassword, salt);

                // checks if old password provided by the user equals to current password in db
                if (currentPassword.Equals(oldPasswordHashed))
                {
                    if (!string.IsNullOrEmpty(newPassword1))
                    {
                        if (!InputValidation.CheckPass(newPassword1))
                        {
                            await DisplayAlert("Error",
                                "New Password must contain at least 1 digit, 1 lower case, 1 upper case, 1 symbol and be between 8 and 15 characters long",
                                "OK");
                            return;
                        }

                        // checks if confirm new password matches new password
                        if (newPassword1.Equals(newPassword2))
                        {
                            // generate a new salt, hashes the new password and updates the database;
                            var newSalt = Sha256Hash.CreateSalt(10);
                            var newHashedPassword = Sha256Hash.GenerateHash(newPassword1, newSalt);
                            bool success;
                            try
                            {
                                success = await WebServices.WebServices.ChangeUserPassword(_email, newSalt, newHashedPassword);
                            }
                            catch (NoInternetException)
                            {
                                await DisplayAlert(NoInternetException.ErrorHeader,
                                    NoInternetException.ErrorBody,
                                    NoInternetException.ErrorButton);
                                return;
                            }

                            if (success)
                            {
                                await DisplayAlert("Success", "Your password has been updated", "OK");
                                await Navigation.PopAsync();
                            }
                            else
                            {
                                await DisplayAlert("Error", "An error occured while updating your password", "OK");
                                await Navigation.PopAsync();
                            }
                        }
                        else
                        {
                            await DisplayAlert("Error", "Confirm new password doesn't match new password", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "Please write a new password", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Old password doesn't match the current password", "OK"); 
                }
            
                }
            
                else
                {
                    await DisplayAlert("Error",
                        "No internet connection, please make sure you are connected to the internet and try again.", "OK");
                }
            }
    }
}