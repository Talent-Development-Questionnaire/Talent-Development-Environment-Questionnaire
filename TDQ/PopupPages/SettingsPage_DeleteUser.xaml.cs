using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

namespace TDQ.PopupPages
{
    public partial class SettingsPage_DeleteUser : ContentPage
    {
        public SettingsPage_DeleteUser()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == "iOS")
                BtnCancel.IsVisible = true;
            else
                BtnCancel.IsVisible = false;
        }

        async void BtnEnterPassword_Clicked(object sender, EventArgs e)
        {
            //Check user details exist and delete them from the database
            var result = Classes.DatabaseController.DeleteUser(Utils.SavedSettings.LoginSettings, EntryPassword.Text);
            if (result == true)
            {
                //Reset app values
                Utils.SavedSettings.LoginSettings = null;
                Utils.SavedSettings.AccountImageSettings = null;
                DeleteGroups();
                DeleteGoals();
                (Application.Current).MainPage = new Navigation_Drawer();
            }
            else
            {
                await DisplayAlert("Error", "Password is incorrect, please try again", "OK");
            }
        }

        void DeleteGroups()
        {
            //Gets all the saved files for created groups
            var files = Directory.EnumerateFiles(App.FolderPath, "*.group.txt");
            //iterates through each file and eletes them from the device
            foreach (var filename in files)
            {
                File.Delete(filename);
            }
        }

        void DeleteGoals()
        {
            //Gets all the saved files for created groups
            var files = Directory.EnumerateFiles(App.FolderPath, "*.goalsgroup.txt");
            //iterates through each file and deletes them from the device
            foreach (var filename in files)
            {
                File.Delete(filename);
            }
        }

        void BtnCancel_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}
