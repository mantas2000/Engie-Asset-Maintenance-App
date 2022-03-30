////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: ChangeRecipientAddressPage.xaml.cs
//FileType: Visual C# Source file
//Author : Joel Cartier, Mantas Burcikas
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : A class for changing recipients' email addresses
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using engie_maintenance_app.Network;
using engie_maintenance_app.WebServices;
using Xamarin.Forms.Xaml;

namespace engie_maintenance_app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangeRecipientAddressPage
    {
        public ChangeRecipientAddressPage()
        {
            InitializeComponent();
            AddPickerItems();
            // Initialize Binding Context Settings.
            BindingContext = new GlobalSettings();
        }

        /// <summary>
        /// Adds all form types to the picker
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
            }

            if (formTypesList != null)
            {
                int itemCount = formTypesList.Count;
                for (int i = 0; i < itemCount; i++)
                {
                    FormType.Items.Add(formTypesList[i]);
                }
            }
        }

        /// <summary>
        /// Gets the recipient of that type of form and makes CurrentRecipient visible on the page
        /// </summary>
        /// <param name="sender">The sender has no effect on this method</param>
        /// <param name="args">Event data has no effect on this method</param>
        private async void CurrentRecipientButtonClicked(object sender, EventArgs e)
        {
            if (HasInternet.HasInternetCheck())
            {
                if (FormType.SelectedItem != null)
                {
                    string formType = FormType.SelectedItem.ToString();
                    string recipientEmail = "";

                    try
                    {
                        recipientEmail = await WebServices.WebServices.GetRecipientEmail(formType);
                    }
                    catch (NoInternetException)
                    {
                        await DisplayAlert(NoInternetException.ErrorHeader,
                            NoInternetException.ErrorBody,
                            NoInternetException.ErrorButton);
                        return;
                    }

                    CurrentRecipient.Text = "Email recipient for " + formType + " forms: " + recipientEmail;
                    CurrentRecipient.IsVisible = true;
                }
                else
                {
                    await DisplayAlert("Error", "Please select a form type!", "Okay");
                }
            }
            else
            {
                await DisplayAlert("Error",
                    "No internet connection, please make sure you are connected to the internet and try again.", "OK");
            }
           
        }

        /// <summary>
        /// Changes the email address forms of that type will be sent to
        /// </summary>
        /// <param name="sender">The sender has no effect on this method</param>
        /// <param name="args">Event data has no effect on this method</param>
        private async void SubmitButtonClicked(object sender, EventArgs e)
        {
            if (HasInternet.HasInternetCheck())
            {
                try
                {
                    if (FormType.SelectedItem != null && NewEmailEntry!=null)
                    {
                        string formType = FormType.SelectedItem.ToString();
                        string newRecipientEmail = NewEmailEntry.Text;
                        
                        if (string.IsNullOrEmpty(newRecipientEmail))
                        {
                            await DisplayAlert("Error", "Please enter an email address", "Okay");
                            return;
                        }
                        
                        if(!Security.InputValidation.CheckEmail(newRecipientEmail))
                        {
                            await DisplayAlert("Error", "Please enter a valid email address", "Okay");
                            return;
                        }
                        

                        bool test;

                        try
                        {
                            test = await WebServices.WebServices.SetNewRecipientEmail(formType, newRecipientEmail);
                        }
                        catch (NoInternetException)
                        {
                            await DisplayAlert(NoInternetException.ErrorHeader,
                                NoInternetException.ErrorBody,
                                NoInternetException.ErrorButton);
                            await Navigation.PopToRootAsync();
                            return;
                        }

                        if (test)
                        {
                            string recipientEmail;
                            try
                            {
                                recipientEmail = await WebServices.WebServices.GetRecipientEmail(formType);
                            }
                            catch (NoInternetException)
                            {
                                await DisplayAlert(NoInternetException.ErrorHeader,
                                    NoInternetException.ErrorBody,
                                    NoInternetException.ErrorButton);
                                return;
                            }

                            CurrentRecipient.Text = "Email recipient for " + formType + " forms: " + recipientEmail;
                            CurrentRecipient.IsVisible = true;
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "Please select a form type and enter a new email address!", "Okay");
                    }
                }
                catch
                {
                    throw new Exception();
                }
            }
            else
            {
                await DisplayAlert(NoInternetException.ErrorHeader,
                    NoInternetException.ErrorBody,
                    NoInternetException.ErrorButton);
            }
        }
    }
}