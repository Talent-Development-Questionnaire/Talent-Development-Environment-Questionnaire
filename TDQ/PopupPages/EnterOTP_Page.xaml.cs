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
            var result = Classes.DatabaseController.VerifyCoachAccount(email, EntryOTP.Text);
            if (result == false)
                await DisplayAlert("Error", "OTP is incorrect, please try again.", "OK");
            else
            {
                Utils.SavedSettings.LoginSettings = email;
                await Navigation.PopModalAsync();
            }
        }

        void BtnCancel_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}
