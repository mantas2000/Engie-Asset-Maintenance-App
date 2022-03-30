////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: RegistrationPage.xaml.cs
//FileType: Visual C# Source file
//Author : Joel Carter, Mantas Burcikas
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : A class for registration functionality
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using engie_maintenance_app.Network;
using engie_maintenance_app.Security;
using engie_maintenance_app.WebServices;
using Xamarin.Forms;

namespace engie_maintenance_app.Views
{
    public partial class RegistrationPage
    {
        private string _code;
        private string _registrationUsername;
        private string _registrationEmail;
        private string _registrationPassword;
        private object _roleTypeObject;
        private string _registrationRoleType;
        private object _engineerTypeObject;
        private string _registrationEngineerType;
        private string _registrationCode;

        public RegistrationPage()
        {
            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetHasBackButton(this, true);
            InitializeComponent();
            BindingContext = new GlobalSettings();
            AddPickerItems();
        }

        /// <summary>
        /// Adds engineer types to the picker
        /// </summary>
        private async void AddPickerItems()
        {
            List<string> formTypesList = new List<string>();
            try
            {
                formTypesList = await WebServices.WebServices.GetFormTypes();
            }
            catch (NoInternetException)
            {
                await DisplayAlert(NoInternetException.ErrorHeader,
                    NoInternetException.ErrorBody,
                    NoInternetException.ErrorButton);
                await Navigation.PopToRootAsync();
                return;
            }

            if (formTypesList != null)
            {
                int itemCount = formTypesList.Count;
                for (int i = 0; i < itemCount; i++)
                {
                    EngineerType.Items.Add(formTypesList[i]);
                }
            }
        }

        /// <summary>
        /// Pushes LoginPage to the top of the Navigation stack.
        /// </summary>
        /// <param name="sender">The sender has no effect on this method</param>
        /// <param name="args">Event data has no effect on this method</param>
        private async void LoginRedirect(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new LoginPage());
        }

        /// <summary>
        /// Email the registration code to the specified email
        /// </summary>
        /// <param name="registrationEmail">The email address that will be sent the registration email</param>
        private void EmailCode(string registrationEmail)
        {
            Random generator = new Random ();
            _code = generator.Next(0, 999999).ToString();

            Username.IsVisible = false;
            Email.IsVisible = false;
            Password.IsVisible = false;
            RoleType.IsVisible = false;
            EngineerType.IsVisible = false;
            
            EmailService.Email.SendVerificationCode(registrationEmail, _code);
            
            DisplayAlert("Alert", "Email verification code has been sent, please check your inbox!", "Okay");
            EmailValidationCode.IsVisible = true;
            SubmitCodeButton.IsVisible = true;
            SignupButton.IsVisible = false;
        }

        /// <summary>
        /// Verifies the VertificationCode field contains the correct code
        /// </summary>
        /// <param name="sender">The sender has no effect on this method</param>
        /// <param name="args">Event data has no effect on this method</param>
        private void VerifyCode(object sender, EventArgs args)
        {
            if (HasInternet.HasInternetCheck())
            {
                _registrationCode = EmailValidationCode.Text;
            
                if (_registrationCode!= null)
                {
                    if (_registrationCode.Equals(_code))
                    {
                        CreateAccount();
                    }
                    else
                    {
                        DisplayAlert("Error", "Incorrect validation code please enter the code sent to your email address",
                            "OK");
                    }
                }
                else
                {
                    DisplayAlert("Error", "Please enter the verification code sent to your email address",
                        "OK");
                }
            }
            
            else
            {
                 DisplayAlert("Error",
                    "No internet connection, please make sure you are connected to the internet and try again.", "OK");
            }
        }

        /// <summary>
        /// Creates the account
        /// </summary>
        private async void CreateAccount()
        {
            // Create salt for the password.
            var salt = Sha256Hash.CreateSalt(10);

            // Hash password in SHA256.
            var hashedPassword = Sha256Hash.GenerateHash(_registrationPassword, salt);

            // Connect to the database and add a new account.
            try
            {
                var result = await WebServices.WebServices.CreateAccount(_registrationUsername, hashedPassword, _registrationEmail, salt, _registrationRoleType, _registrationEngineerType);

                if (result)
                {
                    // Display a success message.
                    await LoginPage.ConfirmLogin(_registrationEmail, _registrationPassword, false);
                    await DisplayAlert("Welcome", "You have registered successfully", "OK");
                    
                    // Redirect to the Main Page.
                    if (_registrationRoleType.Equals("Public"))
                    {
                        // Inserts MainPage as root then PopsToRoot so there is no back button on the MainPage.
                        Navigation.InsertPageBefore(new MainPage(), Navigation.NavigationStack[0]);
                        await Navigation.PopToRootAsync(true);
                    }

                    // Redirect to the Admin Page.
                    if (_registrationRoleType.Equals("Admin"))
                    {
                        Navigation.InsertPageBefore(new AdminPage(), Navigation.NavigationStack[0]);
                        await Navigation.PopToRootAsync(true);
                    }
                }
            }
            catch (MySqlConnector.MySqlException)
            {
                Console.WriteLine("SQL error");
                await DisplayAlert("Error", "Failed to log in. Do you have an internet connection?", "OK");
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
                await DisplayAlert("Error", "An unexcepted error occured. Please try again", "Ok");
            }
        }

        /// <summary>
        /// Verifies user's details and sends verification code
        /// </summary>
        /// <param name="sender">The sender has no effect on this method</param>
        /// <param name="args">Event data has no effect on this method</param>
        private async void RegistrationProtocol(object sender, EventArgs args)
        {
            if (HasInternet.HasInternetCheck())
            {
                // Get user's details from the fields
                _registrationUsername = Username.Text;
                _registrationEmail = Email.Text;
                _registrationPassword = Password.Text;
                _roleTypeObject = RoleType.SelectedItem;
                _engineerTypeObject = EngineerType.SelectedItem;

                if (_roleTypeObject != null)
                {
                    _registrationRoleType = _roleTypeObject.ToString();
                }
                if (_engineerTypeObject != null)
                {
                    _registrationEngineerType = _engineerTypeObject.ToString();
                }

                // Validate user's inputs.
                var validated = await FormValidation(_registrationUsername, _registrationEmail, _registrationPassword, _registrationRoleType, _registrationEngineerType);

                if (validated)
                {
                    // Convert email address to lower case.
                    _registrationEmail = _registrationEmail.ToLower();

                    // Send verification email.
                    EmailCode(_registrationEmail);
                }
            }
            else
            {
                await DisplayAlert("Error",
                    "No internet connection, please make sure you are connected to the internet and try again.", "OK");
            }
        }

        /// <summary>
        /// Validate all of the forms
        /// </summary>
        /// <param name="username">The username to be validated</param>
        /// <param name="email">The email to be validated</param>
        /// <param name="password">The password to be validated</param>
        /// <param name="roleType">The role type to be validated</param>
        /// <param name="engineerType">The engineer type to be validated</param>
        /// <returns>True if all fields are valid</returns>
        private async Task<bool> FormValidation(string username, string email, string password, string roleType, string engineerType)
        {
            if (string.IsNullOrEmpty(username))
            {
                await DisplayAlert("Error", "Please provide a username", "OK");
                return false;
            }

            bool emailUnique;
            try
            {
                emailUnique = await WebServices.WebServices.EmailUnique(email);
            }
            catch (NoInternetException)
            {
                await DisplayAlert(NoInternetException.ErrorHeader,
                    NoInternetException.ErrorBody,
                    NoInternetException.ErrorButton);
                return false;
            }

            if (!string.IsNullOrEmpty(email))
            {
                // Checks for email pattern.
                if (!InputValidation.CheckEmail(email))
                {
                    await DisplayAlert("Error", "Email is not valid", "OK");
                    return false;
                }
                if(!string.IsNullOrEmpty(password))
                        
                {
                    // Checks for password pattern.
                    if (!InputValidation.CheckPass(password))
                    {
                        await DisplayAlert("Error", "Password must contain at least 1 digit, 1 lower case, 1 upper case, 1 symbol and between 8-15 characters long", "OK");
                        return false;
                    }
                }
            }
            else
            {
                await DisplayAlert("Error", "Email is not valid", "OK");
                return false;
            }
            
            if (emailUnique == false)
            {
                await DisplayAlert("Error", "User with that email address already exists", "OK");
                return false;
            }

            if (string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Error", "Please provide a password", "OK");
                return false;
            }

            if (roleType == null)
            {
                await DisplayAlert("Error", "Please select a role type", "OK");
                return false;
            }
            if (engineerType == null)
            {
                await DisplayAlert("Error", "Please select what type of engineer you are", "OK");
                return false;
            }

            // Create a list of user inputs.
            var userInputs = new List<string> { username, email, password };

            // Make sure no illegal characters/words are in the input.
            if (!InputValidation.Validate(userInputs))
            {
                await DisplayAlert("Illegal Input found!", "Please remove any illegal character/word from your input and try again!", "OK");
                return false;
            }
            return true;
        }
    }
}