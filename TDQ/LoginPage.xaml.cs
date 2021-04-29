using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TDQ
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(EntryEmail.Text) && !string.IsNullOrEmpty(EntryPassword.Text))
            {
                var result = Classes.DatabaseController.AccountCheck(EntryEmail.Text, EntryPassword.Text);

                if (result == true)
                {
                    result = Classes.DatabaseController.VerifyCoachAccount(EntryEmail.Text, "%02%03");
                    if (result)
                    {
                        Utils.SavedSettings.LoginSettings = EntryEmail.Text;
                        (Application.Current).MainPage = new Navigation_Drawer_Logged_In();
                        return;
                    }

                    Navigation.PushModalAsync(new PopupPages.EnterOTP_Page(EntryEmail.Text));
                    return;
                }
                    
                await DisplayAlert("Account", "Email or password are incorrect, please try again.", "OK");
                return;
            }
            await DisplayAlert("Error", "Email and password must not be left empty, please try again.", "OK");
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.GoToAsync("//home");

            });
            return true;
        }
    }
}