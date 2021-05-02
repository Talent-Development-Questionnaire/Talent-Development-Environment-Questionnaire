using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TDQ.PopupPages
{
    public partial class EnterOTP_Page : ContentPage
    {
        private string email;

        public EnterOTP_Page()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == "iOS")
                BtnCancel.IsVisible = true;
            else
                BtnCancel.IsVisible = false;
        }
        public EnterOTP_Page(string mail)
        {
            InitializeComponent();
            email = mail;

            if (Device.RuntimePlatform == "iOS")
                BtnCancel.IsVisible = true;
            else
                BtnCancel.IsVisible = false;
        }

        async void BtnEnterOTP_Clicked(object sender, EventArgs e)
        {
            //Check that the entered OTP is valid to the entered email
            var result = Classes.DatabaseController.VerifyCoachAccount(email, EntryOTP.Text);
            if (result == false)
            {
                await DisplayAlert("Error", "OTP is incorrect, please try again.", "OK");
                return;
            }
            //Save entered email for later use
            Utils.SavedSettings.LoginSettings = email;

            if (Utils.SavedSettings.LoginSettings == email && result)
                (Application.Current).MainPage = new Navigation_Drawer_Logged_In();
        }

        void BtnCancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}
