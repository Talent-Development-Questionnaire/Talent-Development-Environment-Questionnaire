using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TDQ.Models;

namespace TDQ.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddQuestionnairePage : ContentPage
    {
        //Initialising global variables
        ObservableCollection<Group> Groups;
        List<string> emailList = new List<string>();
        List<string> groupNameList = new List<string>();
        int type;

        public AddQuestionnairePage()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == "iOS")
                BtnCancel.IsVisible = true;
            else
                BtnCancel.IsVisible = false;
        }

        protected override void OnAppearing()
        {
            //Empty list to stop duplication
            emailList = new List<string>();
            //Populate picker with groups created by user
            PopulateGroupList();

        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Set the question type to 59 or 28
            switch (PickerQuestionnaire.SelectedItem.ToString())
            {
                case "59 Question":
                    type = 59;
                    break;
                case "28 Question":
                    type = 28;
                    break;
            }
        }

        async void BtnSendQuestionnaire_Clicked(object sender, EventArgs e)
        {
            //Check user has an account before sending quesitonnaire
            if (!string.IsNullOrEmpty(Utils.SavedSettings.LoginSettings))
            {   
                //Confirm user wants to send the questionnaire
                var result = await DisplayAlert("Confirm Questionnaire", "Are you sure you want to send the questionnaire?", "Yes", "No");

                //if user clicks 'OK' run
                if (result)
                {
                    //Add each email to database and link to this questionnaire being created
                    foreach (string athlete in emailList)
                        Classes.DatabaseController.AssignAthletesQuestionnaires(EntryName.Text, type.ToString(), Utils.SavedSettings.LoginSettings, athlete, Classes.DatabaseController.GenerateOTP());
                }
              
            } else
            {
                await DisplayAlert("Error", "Make sure you're logged in before sending a questionnaire", "OK");
            }
            Navigation.PopModalAsync();
        }

        void PickerGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Check item selected has a value
            if (PickerGroup.SelectedItem != null)
                foreach (var item in Groups)
                    //Get emails saved to group and populated list view with them
                    if (item.Name == PickerGroup.SelectedItem.ToString())
                    {
                        LstViewEmails.ItemsSource = item.EmailList;
                        emailList = item.EmailList.ToList();

                        if (LstViewEmails.Height < 160)
                            LstViewEmails.HeightRequest = 35 * emailList.Count();
                    }
        }

        private async void ImgBtnAddEmail_Clicked(object sender, EventArgs e)
        {
            //Check there is text in entry 
            if (!string.IsNullOrEmpty(entryEmail.Text))
            {
                if (Classes.GlobalFunctions.IsValidEmail(entryEmail.Text))
                    emailList.Add(entryEmail.Text);
                else
                {
                    await DisplayAlert("Error", "Email is not valid", "OK");
                    entryEmail.Text = string.Empty;
                }

                if (LstViewEmails.Height < 160)
                    LstViewEmails.HeightRequest = 35 * emailList.Count();

                //Clear list view to stop duplication and add email to List View
                LstViewEmails.ItemsSource = null;
                LstViewEmails.ItemsSource = emailList;
                //Automatically scroll list view to the last item entered
                LstViewEmails.ScrollTo(emailList[emailList.Count() - 1], ScrollToPosition.MakeVisible, true);
                return;
            }

            await DisplayAlert("Error", "Email cannot be left empty. Please try again.", "OK");
        }

        private void PopulateGroupList()
        {
            //Initialising variables
            Groups = new ObservableCollection<Group>();

            //Gets all the saved files for created groups
            var files = Directory.EnumerateFiles(App.FolderPath, "*.group.txt");
            //iterates through each file and adds the Group to the list view
            foreach (var filename in files)
            {
                string text = (File.ReadAllText(filename));
                string[] splitText = text.Split('\n');
                emailList = Classes.GlobalFunctions.PopulateListOnAppearing(splitText).ToList();//Method to populate array with emails from the file

                //Adds Group object to list, sets each property of the object
                Groups.Add(new Group
                {
                    Filename = filename,
                    Name = splitText[0],
                    GroupNo = Convert.ToInt32(splitText[1]),
                    ImageFilePath = splitText[2],
                    EmailList = emailList.ToArray()
                });
                //Create list of all groups saved on device
                groupNameList.Add(splitText[0]);
                emailList = new List<string>();
            }
            //Add names of groups to pickers item source
            PickerGroup.ItemsSource = groupNameList;
        }

        void BtnCancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}