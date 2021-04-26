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
            var result = Classes.DatabaseController.AccountCheck(EntryEmail.Text, EntryPassword.Text);
            
            try
            {
                if (result == true)
                {
                    result = Classes.DatabaseController.VerifyCoachAccount(EntryEmail.Text, "%02%03");
                    if(result)
                    {
                        Utils.SavedSettings.LoginSettings = EntryEmail.Text;
                        (Application.Current).MainPage = new Navigation_Drawer_Logged_In();
                        return;
                    }

                    Navigation.PushModalAsync(new PopupPages.EnterOTP_Page(EntryEmail.Text));
                }
                else
                    await DisplayAlert("Account", "Account does not exist, please try again.", "OK");
            }
            catch (Exception)
            {

                await DisplayAlert("Account", "Account does not exist, please try again.", "OK");
            }
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