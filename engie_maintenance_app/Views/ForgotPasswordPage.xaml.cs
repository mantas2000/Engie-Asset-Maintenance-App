////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: ForgotPasswordPage.xaml.cs
//FileType: Visual C# Source file
//Author : Mohammed Albulushi, Mantas Burcikas
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : A class to handle the user's request for the forgot password.
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using engie_maintenance_app.Network;
using engie_maintenance_app.Security;
using engie_maintenance_app.WebServices;
using Xamarin.Forms.Xaml;

namespace engie_maintenance_app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPasswordPage
    {
        private string _email;
        private string _code;
        public ForgotPasswordPage()
        {
            InitializeComponent();
            
            // Initialize Binding Context Settings.
            BindingContext = new GlobalSettings();
        }

        /// <summary>
        /// Sends the verification code to the user's email
        /// </summary>
        /// <param name="sender">The sender has no effect on this method</param>
        /// <param name="args">Event data has no effect on this method</param>
        private async void SendCodeClicked(object sender, EventArgs e)
        {
            if (HasInternet.HasInternetCheck())
            {
                // Gets the user provided email.
                _email = EmailField.Text;
            
                if (!string.IsNullOrEmpty(_email))
                {
                    _email = _email.ToLower();

                    if (!InputValidation.CheckEmail(_email))
                    {
                        await DisplayAlert("Error", "Email is not valid", "OK");
                        return;
                    }

                    bool emailUnique;

                    try
                    {
                        emailUnique = await WebServices.WebServices.EmailUnique(_email);
                    }
                    catch (NoInternetException)
                    {
                        await DisplayAlert(NoInternetException.ErrorHeader,
                            NoInternetException.ErrorBody,
                            NoInternetException.ErrorButton);
                        return;
                    }

                    bool userExists = !emailUnique;

                    try
                    {
                        if (userExists)
                        {
                            // Generates a random 6 digit code and set code equal to that.
                            Random generator = new Random();
                            string s = generator.Next(0, 1000000).ToString("000000");
                            _code = s;

                            // Emails the code to the user.
                            EmailService.Email.SendVerificationCode(_email, _code);
                        }

                        // Shows success message.
                        await DisplayAlert("Sent!", "If an account exists with the email address provided a reset code has been sent via email!", "OK");
                    
                        // Hides the fields and show the confirm fields.
                        EmailField.IsVisible = false;
                        SendCode.IsVisible = false;
                        CodeField.IsVisible = true;
                        ConfirmCode.IsVisible = true;
                    }
                    catch (Exception ex)
                    {
                        // Shows error message 
                        Console.WriteLine(ex);
                        await DisplayAlert("Error", "An error occured while emailing the code, please try again", "OK");
                    }
                }
                else
                {
                    // Shows error if the email field is empty.
                    await DisplayAlert("Error", "Please enter your email address", "OK");
                }
            
            }
            else
            {
                await DisplayAlert("Error",
                    "No internet connection, please make sure you are connected to the internet and try again.", "OK");
            }
            
        }

        /// <summary>
        /// Hides all currently visible fields and shows the change password fields
        /// </summary>
        /// <param name="sender">The sender has no effect on this method</param>
        /// <param name="args">Event data has no effect on this method</param>
        private async void ConfirmCodeClicked(object sender, EventArgs e)
        {
            // Checks if the entered code is the one sent to the email.
            if (_code != null && _code.Equals(CodeField.Text))
            {
                // Hide all the other fields.
                EmailField.IsVisible = false;
                CodeField.IsVisible = false;
                SendCode.IsVisible = false;
                ConfirmCode.IsVisible = false;

                // Shows the field for change password.
                NewPassword1.IsVisible = true;
                NewPassword2.IsVisible = true;
                ChangePsw.IsVisible = true;
            }
            else
            {
                // Shows error message if code doesn't match
                await DisplayAlert("Error", "Invalid code: please try again", "OK");
            }
        }

        /// <summary>
        /// Changes the password using the NewPassword1 and NewPassword2 fields
        /// </summary>
        /// <param name="sender">The sender has no effect on this method</param>
        /// <param name="args">Event data has no effect on this method</param>
        private async void ChangePasswordClicked(object sender, EventArgs e)
        {
            if (HasInternet.HasInternetCheck())
            {
                // Gets the new password.
            var newPassword1 = NewPassword1.Text;
            var newPassword2 = NewPassword2.Text;
            if (!string.IsNullOrEmpty(newPassword1))

            {
                // Checks if confirm new password = new password (maybe user have a typing mistake).
                if (newPassword1.Equals(newPassword2))
                { 
                    if (!InputValidation.CheckPass(newPassword1))
                    {
                        await DisplayAlert("Error",
                            "Password must contain at least 1 digit, 1 lower case, 1 upper case, 1 symbol and be 8-15 characters long",
                            "OK");
                        return;
                    }
                    
                    try
                    {
                        // Generates a salt and uses that to hash the new password.
                        var salt = Sha256Hash.CreateSalt(10);
                        var hashedNewPassword = Sha256Hash.GenerateHash(newPassword1, salt);

                        // Updates the user password in db.
                        await WebServices.WebServices.ChangeUserPassword(_email, salt, hashedNewPassword);

                        // Shows success message.
                        await DisplayAlert("Success", "Successfully updated password", "OK");
                        await Navigation.PopToRootAsync();
                    }
                    catch (NoInternetException)
                    {
                        await DisplayAlert(NoInternetException.ErrorHeader,
                            NoInternetException.ErrorBody,
                            NoInternetException.ErrorButton);
                        return;
                    }
                    catch (Exception ex)
                    {
                        // Shows an error if anything goes wrong while updating password and returns user to login page.
                        Console.WriteLine(ex);
                        await DisplayAlert("Error", "An error occured while updating password, please try again", "OK");
                        await Navigation.PopToRootAsync();
                    }
                }
                else
                {
                    // Shows error message password doesn't match.
                    await DisplayAlert("Error", "Please ensure that both passwords entered match", "OK");
                }
            }
            else
            {
                // Shows error message.
                await DisplayAlert("Error", "Please enter a new password!", "OK");
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