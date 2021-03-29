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
        MainPage main;
        ObservableCollection<Group> Groups;
        List<string> emailList = new List<string>();

        int type;

        public AddQuestionnairePage()
        {
            InitializeComponent();

            if(Device.RuntimePlatform == "iOS")
                BtnCancel.IsVisible = true;
            else
                BtnCancel.IsVisible = false;
        }

        public AddQuestionnairePage(MainPage m)
        {
            InitializeComponent();

            if (Device.RuntimePlatform == "iOS")
                BtnCancel.IsVisible = true;
            else
                BtnCancel.IsVisible = false;

            main = m;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            PopulateGroupList();

        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (PickerQuestionnaire.SelectedItem)
            {
                case "58 Question":
                    type = 59;
                    break;
                case "28 Question":
                    type = 28;
                    break;
            }
        }

        void BtnSendQuestionnaire_Clicked(object sender, EventArgs e)
        {
            Questionnaire item = null;
            int flag = 0;
            string otp = Classes.DatabaseController.GenerateOTP();

            foreach (string athlete in emailList)
            {
                if (athlete == emailList[emailList.Count() - 1])
                    flag = 1;
                else
                    flag = 0;

                item = Classes.DatabaseController.AssignAthletesQuestionnaires(EntryName.Text, type.ToString(), Utils.SavedSettings.LoginSettings, athlete, otp, flag);
            }
            if (item != null)
                main.UpdateQuestionnareList();
            Navigation.PopModalAsync();
        }

        void PickerGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PickerGroup.SelectedItem != null)
                foreach (var item in Groups)
                    if (item.Name == PickerGroup.SelectedItem.ToString())
                    {
                        LstViewEmails.ItemsSource = item.EmailList;
                        emailList = item.EmailList.ToList();
                    }
        }

        private async void ImgBtnAddEmail_Clicked(object sender, EventArgs e)
        {
            //Method to avoid items in list view duplicating
            //CheckList();

            //Checks that the entry field is not empty and then verifies text is a valid email
            if (!string.IsNullOrEmpty(entryEmail.Text))
            {
                if (Classes.Verification.IsValidEmail(entryEmail.Text))
                    emailList.Add(entryEmail.Text);
                else
                {
                    await DisplayAlert("Error", "Email is not valid", "OK");
                    entryEmail.Text = string.Empty;
                }

                if (LstViewEmails.Height < 160)
                    LstViewEmails.HeightRequest = 35 * emailList.Count();

                //Updates list view with newly added email
                LstViewEmails.ItemsSource = emailList;
                LstViewEmails.ScrollTo(emailList[emailList.Count() - 1], ScrollToPosition.MakeVisible, true);
                //ListVisibility();

            }
            else
            {
                await DisplayAlert("Error", "Email cannot be left empty. Please try again.", "OK");
            }
            if (LstViewEmails.Height < 160)
                LstViewEmails.HeightRequest = 35 * emailList.Count();
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
                emailList = Classes.GroupPageFunctions.PopulateListOnAppearing(splitText).ToList();//Method to populate array with emails from the file

                //Adds Group object to list, sets each property of the object
                Groups.Add(new Group
                {
                    Filename = filename,
                    Name = splitText[0],
                    GroupNo = Convert.ToInt32(splitText[1]),
                    ImageFilePath = splitText[2],
                    EmailList = emailList.ToArray()
                });
                emailList.Add(splitText[0]);

                PickerGroup.ItemsSource = emailList.ToArray();
            }
        }

        void BtnCancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}