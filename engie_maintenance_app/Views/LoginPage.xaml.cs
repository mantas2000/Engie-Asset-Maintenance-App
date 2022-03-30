////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: LoginPage.xaml.cs
//FileType: Visual C# Source file
//Author : Mantas Burcikas
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : A class for displaying Licence Page
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using engie_maintenance_app.Network;
using engie_maintenance_app.Objects;
using engie_maintenance_app.Security;
using engie_maintenance_app.WebServices;
using MySqlConnector;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using Plugin.SecureStorage;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace engie_maintenance_app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage
    {
        private string _email;
        public LoginPage()
        {
            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            BindingContext = new GlobalSettings();

            if (CrossSecureStorage.Current.HasKey("Email"))
            {
                _email = CrossSecureStorage.Current.GetValue("Email");
                // Insert previously used email in the email field.
                Email.Text = _email;
            }

            // Check if biometric authentication is available and run it accordingly.
            BiometricAuthentication();
        }
        
        
        private async void RegisterRedirect(object sender, EventArgs args)
        {
            if (HasInternet.HasInternetCheck())
            {
                await Navigation.PushAsync(new RegistrationPage());
            }
            else
            {
                await DisplayAlert("Error",
                    "No internet connection, please make sure you are connected to the internet and try again.", "OK");
            }
        }

        private async void ValidateUser(object sender, EventArgs args)
        {
            try
            {
                if (HasInternet.HasInternetCheck())
                {
                    // Get email and password from the fields.
                    _email = Email.Text;
                    string password = Password.Text;

                    // Create a list of user's inputs.
                    List<string> userInputs = new List<string> { _email, password };

                    // Make sure email and password fields are not empty.
                    if (string.IsNullOrEmpty(_email) || string.IsNullOrEmpty(password))
                    {
                        await DisplayAlert("Error", "Please enter email address/password", "OK");
                    }

                    // Make sure no illegal characters/words are in the input.
                    else if (!InputValidation.Validate(userInputs))
                    {
                        await DisplayAlert("Illegal Input found!", "Please remove any illegal character/word from your input and try again!", "OK");
                    }
                    // Continue if user's input is valid.
                    else
                    {
                        try
                        {
                            _email = _email.ToLower();
                            // Confirm that such account exists.
                            if (await ConfirmLogin(_email, password, false))
                            {
                                // Redirect to the next page.
                                PageRedirect();
                            }
                            else
                            {
                                await DisplayAlert("Error", "This email and password combination is incorrect", "OK");
                            }
                        }
                        catch (MySqlException mySqlException)
                        {
                            // No internet.
                            Console.WriteLine(mySqlException);
                            await DisplayAlert("Error", "Failed to log in. Do you have an internet connection?", "OK");
                        }
                        catch (NoInternetException)
                        {
                            await DisplayAlert(NoInternetException.ErrorHeader,
                                NoInternetException.ErrorBody,
                                NoInternetException.ErrorButton);
                            return;
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Error",
                        "No internet connection, please make sure you are connected to the internet and try again.", "OK");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await DisplayAlert("Error", "Unexpected error occured. Please try again", "OK");
            }
        }

        private async void BiometricsInterface()
        {
            // Create a new Authentication Request Configuration.
            var conf = new AuthenticationRequestConfiguration("Authentication");
            
            // Get biometric authentication
            FingerprintAuthenticationResult authResult = await CrossFingerprint.Current.AuthenticateAsync(conf);
            
            // Confirm the successful login and redirect user to the next page.
            if (authResult.Authenticated)  
            {  
                // Redirect to the next page.
                PageRedirect();
            }
            
            // If biometric authentication fails, display an error message.
            if (authResult.ErrorMessage != null)
            {
                await DisplayAlert("Error", authResult.ErrorMessage, "OK");
            }
        }

        private async void BiometricAuthentication()
        {
            try
            {
                // Check if email and password from previous login session are stored.
                if (CrossSecureStorage.Current.HasKey("Email") && CrossSecureStorage.Current.HasKey("Password"))
                {
                    // Get email and password from the previous session.
                    _email = CrossSecureStorage.Current.GetValue("Email");
                    var password = CrossSecureStorage.Current.GetValue("Password");

                    // Check if any of the biometrics authentication methods are available in the device.
                    if (await CrossFingerprint.Current.IsAvailableAsync())
                    {
                        // Confirm that details from the previous session are still correct.
                        if (await ConfirmLogin(_email, password, true))
                        {
                            // Run biometric authentication.
                            BiometricsInterface();
                        }
                    }
                }
            }
            catch (NoInternetException)
            {
                await DisplayAlert(NoInternetException.ErrorHeader,
                    NoInternetException.ErrorBody,
                    NoInternetException.ErrorButton);
                return;
            }
            catch (Exception)
            {
                await DisplayAlert("Error",
                    "An unexpected error has occured. Please try again",
                    "Ok");
                return;
            }
        }

        public static async Task<bool>ConfirmLogin(string email, string password, bool usesBiometrics)
        {
            // Connect to the database and get details of the account.
            try
            {
                User userDetails = await WebServices.WebServices.GetUserDetails(email);
                
                // Confirm that the database returned user's details.
                if (userDetails.Password != null)
                {
                    var hashedPassword = password;

                    // Hash input if biometric authentication is not used for login.
                    if (!usesBiometrics)
                    {
                        // Get the salt from database.
                        var salt = userDetails.Salt;

                        // Create hash of the user's provided password.
                        hashedPassword = Sha256Hash.GenerateHash(password, salt);
                    }

                    // Check if provided password matches with a password from the database.
                    if (hashedPassword.Equals(userDetails.Password))
                    {

                        // Store login session details.
                        CrossSecureStorage.Current.SetValue("ID", userDetails.Id.ToString());
                        CrossSecureStorage.Current.SetValue("Email", userDetails.Email);
                        CrossSecureStorage.Current.SetValue("RoleType", userDetails.RoleType);
                        CrossSecureStorage.Current.SetValue("Password", hashedPassword);
                        CrossSecureStorage.Current.SetValue("Username", userDetails.Username);
                        CrossSecureStorage.Current.SetValue("EngineerType", userDetails.EngineerType);
                        return true;
                    }
                }

                // If login detail's verification was unsuccessful, delete all session details.
                CrossSecureStorage.Current.DeleteKey("ID");
                CrossSecureStorage.Current.DeleteKey("Email");
                CrossSecureStorage.Current.DeleteKey("RoleType");
                CrossSecureStorage.Current.DeleteKey("Password");
                CrossSecureStorage.Current.DeleteKey("Username");
                CrossSecureStorage.Current.DeleteKey("EngineerType");
                return false;
            }
            catch (NoInternetException e)
            {
                throw e;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async void PageRedirect()
        {
            // Redirect to the Main Page.
            if (CrossSecureStorage.Current.GetValue("RoleType") == "Public")
            {
                // Inserts MainPage as root then PopsToRoot so there is no back button on the MainPage.
                Navigation.InsertPageBefore(new MainPage(), Navigation.NavigationStack[0]);
                await Navigation.PopToRootAsync(true);
            }

            // Redirect to the Admin Page.
            if (CrossSecureStorage.Current.GetValue("RoleType") == "Admin")
            {
                Navigation.InsertPageBefore(new AdminPage(), Navigation.NavigationStack[0]);
                await Navigation.PopToRootAsync(true);
            }
        }

        private void ForgotPassword(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ForgotPasswordPage());
        }
    }
}