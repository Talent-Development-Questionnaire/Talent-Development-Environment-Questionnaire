using System;
using TDQ.Classes;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TDQ
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            //Check if user has set app theme to something other than the default 'red'
            if (!string.IsNullOrEmpty(Utils.SavedSettings.ThemeIndexSettings))
            {
                //set theme and them picker selected item to index last chosen by user
                PickerColour.SelectedIndex = Convert.ToInt32(Utils.SavedSettings.ThemeIndexSettings);
            }
            else
            {
                //set selected item to default theme - red
                PickerColour.SelectedIndex = 0;
            }

            //Check if user chose a specific font size from the default one - medium
            if (!string.IsNullOrEmpty(Utils.SavedSettings.FontIndexSettings))
            {
                //set font size and font size picker to th index last chosen by user
                PickerFontSize.SelectedIndex = Convert.ToInt32(Utils.SavedSettings.FontIndexSettings);
            } else {
                //set selected item to default font size - 'medium'
                PickerFontSize.SelectedIndex = 1;
            }

            

            if (!string.IsNullOrEmpty(Utils.SavedSettings.LoginSettings))
            {
                BtnLogInOut.Text = "Log Out";
                BtnDeleteUser.IsVisible = true;
            }
            else
            {
                BtnLogInOut.Text = "Log In/Sign Up";
                BtnDeleteUser.IsVisible = false;
            }
        }

        //Used to run function to change the apps theme colour when new item is selected
        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Changes theme colours of app
            GlobalFunctions.ChangeTheme(PickerColour.SelectedItem.ToString());
        }

        void BtnLogOut_Clicked(object sender, EventArgs e)
        {
            Utils.SavedSettings.LoginSettings = null;
            Utils.SavedSettings.AccountImageSettings = null;
            (Application.Current).MainPage = new Navigation_Drawer();
        }
        async void BtnLogInOut_Clicked(System.Object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(Utils.SavedSettings.LoginSettings))
            {
                //Log Out
                Utils.SavedSettings.LoginSettings = "";
                Utils.SavedSettings.AccountImageSettings = null;
                (Application.Current).MainPage = new Navigation_Drawer();
            }
            else
            {
                //Go to login/signup page
                await Shell.Current.GoToAsync("//login");
            }
        }

        void PickerFontSize_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            GlobalFunctions.ChangeFontSize(PickerFontSize.SelectedItem.ToString());
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.GoToAsync("//home");

            });
            return true;
        }

        void BtnDeleteUser_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new PopupPages.SettingsPage_DeleteUser());
        }

        // If the help button is pressed then launch the help popup
        private async void BtnSettingsHelp_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PopupHelpPages.SettingsHelp());

        }
    }
}