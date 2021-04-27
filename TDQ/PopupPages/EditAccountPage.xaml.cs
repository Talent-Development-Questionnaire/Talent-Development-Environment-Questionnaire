﻿using System;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TDQ.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditAccountPage : ContentPage
    {
        private Models.CoachUser user;
        public EditAccountPage()
        {
            InitializeComponent();

            // If the application is being run on iOS then display the cancel button, otherwise do not display this
            if (Device.RuntimePlatform == "iOS")
                BtnCancel.IsVisible = true;
            else
                BtnCancel.IsVisible = false;
        }

        public EditAccountPage(Models.CoachUser coach)
        {
            InitializeComponent();
            user = coach;

            if (Device.RuntimePlatform == "iOS")
                BtnCancel.IsVisible = true;
            else
                BtnCancel.IsVisible = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // If there is a profile image stored in the saved settings then retreive it, if not then set the image as the default account photo

            if (Utils.SavedSettings.AccountVerified == null)
                AccountImage.Source = ImageSource.FromResource("ic_account.png");
            else
                AccountImage.Source = Utils.SavedSettings.AccountVerified;
        }

        // When the profile image is pressed allow the user to select a new profile photo
        private async void AccountImage_Tapped(object sender, EventArgs e)
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Pick a photo"
            });

            //if user doesn't pick image stop executing button press
            if (result == null)
                return;

            AccountImage.Source = ImageSource.FromFile(result.FullPath);

            try
            {
                Utils.SavedSettings.AccountVerified = result.FullPath;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Debug", ex.Message, "OK");
            }
        }

        // If the save button is pressed then save the current details and send this to the database
        private void BtnSave_Clicked(object sender, EventArgs e)
        {
            user.Name = entryName.Text;
            user.Email = entryEmail.Text;
            user.Dob = entryDOB.Text;
            user.Gender = entryGender.Text;
            Classes.AccountPageFunctions.EditAccountDetails(user);
            Navigation.PopModalAsync();
        }

        // If the cancel button is pressed then go back to the previous page
        void BtnCancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}