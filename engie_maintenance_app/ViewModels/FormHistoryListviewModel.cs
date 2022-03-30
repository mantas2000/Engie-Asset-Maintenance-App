////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: FormHistoryListView.cs
//FileType: Visual C# Source file
//Author : Mohammed Albulushi
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : A class to connect the local database to user's view page.
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace engie_maintenance_app
{
    public class FormHistoryListviewModel
    {

        private FormHistory _oldFormHistory;
        
        /// <summary>
        /// Observable collection to store list of form histories because
        /// we don't need to manually notify of changes it does it
        /// automatically.
        /// </summary>
        public ObservableCollection<FormHistory> FormsHistory { get; set; }

        public FormHistoryListviewModel()
        {
            FormsHistory = new ObservableCollection<FormHistory>();
            Task<List<FormHistory>> list = DataAccessLayer.FormHistory.GetAll();
            
            // Sets the buttons invisible for all the elements in the list.
            foreach (var i in list.Result)
            {
                HidePreviouslySelectedFormButtons(i);
                FormsHistory.Add(i);
            }
            
        }
        
        /// <summary>
        /// checks the status of the selected element from the list view
        /// and according to that shows appropriate buttons for that element
        /// </summary>
        /// <param name="fhistory"></param>
        public void HideOrShowActions(FormHistory fhistory)
        {
            string caseSwitch = fhistory.Status;

            switch (caseSwitch)
            {
                case "Success":
                {
                    if (_oldFormHistory == fhistory)
                    {
                        // Click twice on the same form history will hide the buttons .
                        fhistory.DeleteButtonIsVisible = !fhistory.DeleteButtonIsVisible;
                    }
                    else
                    {
                        if (_oldFormHistory != null)
                        {
                            // Hide previous selected form history buttons.
                            HidePreviouslySelectedFormButtons(_oldFormHistory);
                            
                            UpdateFHistory(_oldFormHistory);
                        }
                        // Show buttons for selected form.
                        fhistory.DeleteButtonIsVisible = true;
                        UpdateFHistory(fhistory);
                    }
                    UpdateFHistory(fhistory);

                    // Updates the last selected element record in viewmodel.
                    _oldFormHistory = fhistory;

                    break;
                }
                case "Failed":
                {
                    if (_oldFormHistory == fhistory)
                    {
                        // Click twice on the same form history will hide the buttons.
                        fhistory.DeleteButtonIsVisible = !fhistory.DeleteButtonIsVisible;
                        fhistory.RetryButtonIsVisible = !fhistory.RetryButtonIsVisible;
                    }
                    else
                    {
                        if (_oldFormHistory != null)
                        {
                            // Hide buttons for previous selected form history.
                            HidePreviouslySelectedFormButtons(_oldFormHistory);
                            
                            UpdateFHistory(_oldFormHistory);
                        }
                        
                        // Show buttons for selected form.
                        fhistory.DeleteButtonIsVisible = true;
                        fhistory.RetryButtonIsVisible = true;

                        UpdateFHistory(fhistory);
                    }

                    UpdateFHistory(fhistory);
                    _oldFormHistory = fhistory;

                    break;
                }
                case "Not Submitted":
                {
                    if (_oldFormHistory == fhistory)
                    {
                        // Click twice on the same form history will hide the buttons.
                        fhistory.DeleteButtonIsVisible = !fhistory.DeleteButtonIsVisible;
                        fhistory.EditButtonIsVisible = !fhistory.EditButtonIsVisible;
                    }
                    else
                    {
                        if (_oldFormHistory != null)
                        {
                            // Hide previous selected form history buttons.
                            HidePreviouslySelectedFormButtons(_oldFormHistory);

                            UpdateFHistory(_oldFormHistory);
                        }
                        // Show selected form history buttons.

                        fhistory.DeleteButtonIsVisible = true;
                        fhistory.EditButtonIsVisible = true;

                        UpdateFHistory(fhistory);
                    }
                    UpdateFHistory(fhistory);

                    _oldFormHistory = fhistory;
                    break;
                }
            }
        }

        /// <summary>
        /// Function to update elements in our observable collection
        /// to update the list view
        /// </summary>
        /// <param name="formHistory"></param>
        private void UpdateFHistory(FormHistory formHistory)
        {
            int index = FormsHistory.IndexOf(formHistory);
            FormsHistory.Remove(formHistory);
            FormsHistory.Insert(index, formHistory);
        }

        /// <summary>
        /// Hides DeleteButton, EditButton and RetryButton
        /// </summary>
        /// <param name="formHistory"></param>
        private static void HidePreviouslySelectedFormButtons(FormHistory formHistory)
        {
            formHistory.DeleteButtonIsVisible = false;
            formHistory.EditButtonIsVisible = false;
            formHistory.RetryButtonIsVisible = false;
        }
    }
}