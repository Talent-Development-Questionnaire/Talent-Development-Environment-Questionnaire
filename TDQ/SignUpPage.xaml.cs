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
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
        }

        async void BtnSignUp_Clicked(System.Object sender, System.EventArgs e)
        {
            if(EntryPassword.Text == EntryConfirmPassword.Text)
            {
                var result = Classes.DatabaseController.InsertNewUser(EntryEmail.Text, EntryPassword.Text);

                if (result == true)
                {
                    Utils.SavedSettings.LoginSettings = "LoggedIn";
                    (Application.Current).MainPage = new Navigation_Drawer_Logged_In();
                }
                else
                    await DisplayAlert("Connection Error", "Error occured connecting to database, please try again", "OK");
            }
            else
                await DisplayAlert("Error", "Passwords do not match, please try again", "OK");
        }
    }
}