using System;

using TDQ.Classes;
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

        async void BtnSignUp_Clicked(object sender, EventArgs e)
        {
            //Checks if the passwords entered match and if the email entered is a valid one
            if(EntryPassword.Text == EntryConfirmPassword.Text && GlobalFunctions.IsValidEmail(EntryEmail.Text))
            {
                //verification checks to catch any errors/mistakes
                VerifyDetails();
                return;
            }

            await DisplayAlert("Error", "Email may not be valid or the passwords entered do not match, please try again.", "OK");
        }

        private async void VerifyDetails()
        {
            //Check if an account with the entered email already exists
            if(!DatabaseController.CheckEmailExists(EntryEmail.Text))
            {
                //Display error if email already exists
                EntryEmail.TextColor = Color.Red;
                await DisplayAlert("Email Exists", "An account with this email already exists, please try again.", "OK");
                return;
            }

            string otp = DatabaseController.GenerateOTP();  //Create a One Time Password
            string name = EntryFirstName.Text + " " + EntryLastName.Text; //Concatonate first and last name together

            //Check if new user details are sent to the server correctly
            if (!DatabaseController.InsertNewUser(EntryEmail.Text, EntryPassword.Text, name, otp))
            {
                //Display error if can't connect to database
                await DisplayAlert("Connection Error", "Error occured connecting to database, please try again", "OK");
                return;
            }

            await Navigation.PushModalAsync(new PopupPages.EnterOTP_Page(EntryEmail.Text)); //Display page for user to enter their One Time Password
            ResetText(); //Reset entry fields to be empty
            return;
        }

        //Changes the email entry field back from red if user email is not valid
        void EntryEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            EntryEmail.TextColor = Color.Black;
        }

        //Send user back to main page when back button is pressed (primarily for android users)
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.GoToAsync("//home");

            });
            return true;
        }

        //Sets all entry fields in the form back to being empty
        private void ResetText()
        {
            EntryFirstName.Text = string.Empty;
            EntryLastName.Text = string.Empty;
            EntryEmail.Text = string.Empty;
            EntryPassword.Text = string.Empty;
            EntryConfirmPassword.Text = string.Empty;
        }
    }
}