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
        }
        public EnterOTP_Page(string mail)
        {
            InitializeComponent();
            email = mail;
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
    }
}
