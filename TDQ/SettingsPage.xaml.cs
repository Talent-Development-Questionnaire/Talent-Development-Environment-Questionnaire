using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Classes.SettingsPageFunctions.DeleteBackgroundVisibility(BtnDeleteBg);
            //Sets the page background whenever page is opened
            Classes.SettingsPageFunctions.SetBackground(ImgBg, SettingsContentPage);

            //Changes the index of the picker to what the user picked last time
            //If the setting is null then the picker is set to deafult "Red"
            if (!string.IsNullOrEmpty(Utils.SavedSettings.ThemeIndexSettings))
                PickerColour.SelectedIndex = Convert.ToInt32(Utils.SavedSettings.ThemeIndexSettings);
            else
                PickerColour.SelectedIndex = 0;

            if (!string.IsNullOrEmpty(Utils.SavedSettings.FontIndexSettings))
                PickerFontSize.SelectedIndex = Convert.ToInt32(Utils.SavedSettings.FontIndexSettings);
            else
                PickerFontSize.SelectedIndex = 0;

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

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Classes.SettingsPageFunctions.ChangeTheme(PickerColour.SelectedItem.ToString());
        }

        private async void BtnAddBg_Clicked(object sender, EventArgs e)
        {
            //Accesses user's camera roll, gets file path to chosen image
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Pick a photo"
            });

            //if user doesn't pick image stop executing button press
            if (result == null)
                return;

            //sets images source to selected image
            BtnAddBg.Source = ImageSource.FromFile(result.FullPath);

            //Saves the file path to storage to be called again later
            try
            {
                Utils.SavedSettings.BackgroundSettings = result.FullPath;
                //Mehtod to set the background of the current content page
                Classes.SettingsPageFunctions.SetBackground(ImgBg, SettingsContentPage);
            }
            catch (Exception ex)
            {

                await App.Current.MainPage.DisplayAlert("Debug", ex.Message, "OK");
            }
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
            Classes.SettingsPageFunctions.ChangeFontSize(PickerFontSize.SelectedItem.ToString());
        }

        private void BtnDeleteBg_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Utils.SavedSettings.BackgroundSettings))
            {
                Utils.SavedSettings.BackgroundSettings = null;
                Classes.SettingsPageFunctions.SetBackground(ImgBg, SettingsContentPage);
                Classes.SettingsPageFunctions.DeleteBackgroundVisibility(BtnDeleteBg);
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

        void BtnDeleteUser_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new PopupPages.SettingsPage_DeleteUser());
        }
        private async void BtnSettingsHelp_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PopupHelpPages.SettingsHelp());

        }
    }
}