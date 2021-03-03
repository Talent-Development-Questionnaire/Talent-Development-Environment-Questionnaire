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
            var validEmail = Classes.Verification.IsValidEmail(EntryEmail.Text);
            if (EntryPassword.Text == EntryConfirmPassword.Text && validEmail == true)
            {
                var result = Classes.DatabaseController.CheckEmailExists(EntryEmail.Text);

                if (result == false)
                {
                    EntryEmail.TextColor = Color.Red;
                    await DisplayAlert("Email Exists", "An account with this email already exists, please try again.", "OK");
                }
                else
                {
                    string name = EntryFirstName.Text + " " + EntryLastName.Text;
                    result = Classes.DatabaseController.InsertNewUser(EntryEmail.Text, EntryPassword.Text, name);

                    if (result == true)
                    {
                        Utils.SavedSettings.LoginSettings = "LoggedIn";
                        (Application.Current).MainPage = new Navigation_Drawer_Logged_In();
                    }
                    else
                        await DisplayAlert("Connection Error", "Error occured connecting to database, please try again", "OK");
                }
            }
            else if (validEmail == false)
                await DisplayAlert("Error", "Email is not valid, please try again", "OK");
            else
                await DisplayAlert("Error", "Passwords do not match, please try again", "OK");
        }

        void EntryEmail_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            EntryEmail.TextColor = Color.Black;
        }
    }
}