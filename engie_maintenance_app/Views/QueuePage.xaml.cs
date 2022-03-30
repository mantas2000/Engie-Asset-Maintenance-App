////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: QueuePage.xaml.cs
//FileType: Visual C# Source file
//Author : Mohammed Albulushi, Mantas Burcikas, Joel Carter
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : A class to handle user's forms history.
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.IO;
using engie_maintenance_app.Interfaces;
using engie_maintenance_app.Network;
using Plugin.SecureStorage;
using Xamarin.Forms;

namespace engie_maintenance_app.Views
{
    public partial class QueuePage
    {
        public QueuePage()
        {
            InitializeComponent();
            BindingContext = new GlobalSettings();
            HistoryList.BindingContext = new FormHistoryListviewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Updates the page with latest list on appearing.
            HistoryList.BindingContext = new FormHistoryListviewModel();
        }

        /// <summary>
        /// Shows the buttons on the clicked form
        /// </summary>
        /// <param name="sender">The sender has no effect on this method</param>
        /// <param name="args">Event data has no effect on this method</param>
        private void HistoryListClicked(object sender, ItemTappedEventArgs e)
        {
            // Gets the model view page from HistoryList binding context.
            var vm = HistoryList.BindingContext as FormHistoryListviewModel;

            // Gets the selected element from list view .
            var formHistory = e.Item as FormHistory;

            // Passes the selected FormHistory to view model to decide to show what buttons.
            if (vm != null) vm.HideOrShowActions(formHistory);
        }

        /// <summary>
        /// Opens the corrosponding file to the one clicked on.
        /// </summary>
        /// <param name="sender">The sender has no effect on this method</param>
        /// <param name="args">Event data has no effect on this method</param>
        private async void EditSubmitClicked(object sender, EventArgs e)
        {
            // Gets the tapped form name.
            Button editSubmitButton = (Button) sender;
            StackLayout stackLayoutChild = (StackLayout) editSubmitButton.Parent;
            StackLayout stackLayoutParent = (StackLayout) stackLayoutChild.Parent;
            Label formName = (Label) stackLayoutParent.Children[1];
            
            // Gets the user specific directory and creates filePath using formName and that directory.
            string directoryPath =  DependencyService.Get<IDeviceOrientation>().GetDirectory(CrossSecureStorage.Current.GetValue("ID"));
            string filePath = directoryPath + "/" + formName.Text;

            // If the file exists opens it.
            if (File.Exists(filePath))
            {
                await Navigation.PushAsync(new PdfViewerPage(filePath));
            }
        }

        /// <summary>
        /// Submits the form again
        /// </summary>
        /// <param name="sender">The sender has no effect on this method</param>
        /// <param name="args">Event data has no effect on this method</param>
        private async void ReSubmitClicked(object sender, EventArgs e)
        {
            if (HasInternet.HasInternetCheck())
            {
                 // Gets the tapped form name.
                Button resubmitButton = (Button) sender;
                StackLayout stackLayoutChild = (StackLayout) resubmitButton.Parent;
                StackLayout stackLayoutParent = (StackLayout) stackLayoutChild.Parent;
                Label formName = (Label) stackLayoutParent.Children[1];

                // Gets the user specific directory and creates filePath using formName and that directory.
                var directoryPath =  DependencyService.Get<IDeviceOrientation>().GetDirectory(CrossSecureStorage.Current.GetValue("ID"));
                string filePath = directoryPath + "/" + formName.Text;
            
                // Initializes FormHistory Object
                FormHistory form;

                // Test for if the form submission failed or not.
                bool test;
            
                // Email the PDF
                test = await EmailService.Email.SendForm(filePath);

                if (test)
                {
                    // Creates a new FormHistory object and set the test to true (Success)
                    form = new FormHistory(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), formName.Text, "Success");
                }
                else
                {
                    // Creates a new FormHistory object and set the test to true (Failed).
                    form = new FormHistory(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), formName.Text, "Failed"); 
                }
            
                // Updates the formHistory status.
                await DataAccessLayer.FormHistory.Update(form);

                // If the submission success deletes the file and updates the message variable to success.
                if (test)
                {
                    // Deletes the downloaded file.
                    File.Delete(filePath);

                    // Shows the result of submission.
                    await DisplayAlert("Success: ", "The form has been successfully submitted", "OK");
                }
            
                // Updates the list view.
                HistoryList.BindingContext = new FormHistoryListviewModel();
            }
            else 
            {
                await DisplayAlert("Error",
                    "No internet connection, please make sure you are connected to the internet and try again.", "OK");
            }
        }

        /// <summary>
        /// Promts user if they would like to delete the form. If yes deletes the form.
        /// </summary>
        /// <param name="sender">The sender has no effect on this method</param>
        /// <param name="args">Event data has no effect on this method</param>
        private async void DeleteButton(object sender, EventArgs e)
        {
            // User confirmation to delete file
            var confirm = await DisplayAlert("Alert", "Are you sure you want to delete this file/info ?", "Yes", "No");

            if (confirm)
            {
                // Gets the FormName && FormHistoryId. 
                Button deleteButton = (Button) sender;
                StackLayout stackLayoutChild = (StackLayout) deleteButton.Parent;
                StackLayout stackLayoutParent = (StackLayout) stackLayoutChild.Parent;
                Label formHistoryId = (Label) stackLayoutParent.Children[0];
                Label formName = (Label) stackLayoutParent.Children[1];
                Label status = (Label) stackLayoutParent.Children[3];

                // Deletes the selected form history .
                Delete(formHistoryId.Text, formName.Text, status.Text);

                // Updates the page.
                HistoryList.BindingContext = new FormHistoryListviewModel();
            }
        }

        /// <summary>
        /// Deleted the specified form from the device and the history
        /// </summary>
        /// <param name="formHistoryId">The form historyID of the form to be deleted</param>
        /// <param name="formName">The name of the form to be deleted</param>
        /// <param name="status">The current status of the form</param>
        private void Delete(string formHistoryId, string formName, string status)
        {
            // Gets the user specific directory and create filePath using form name.
            var directoryPath =  DependencyService.Get<IDeviceOrientation>().GetDirectory(CrossSecureStorage.Current.GetValue("ID"));
            string filePath = directoryPath + "/" + formName;

            // If the status is not success then try to delete the file.
            if (!status.Equals("Success"))
            {
                // If the file exists deletes it.
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            // Then deletes the history from the db.
            DataAccessLayer.FormHistory.Delete(formHistoryId);
        }
    }
}
