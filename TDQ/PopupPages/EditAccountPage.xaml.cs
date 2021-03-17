using System;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TDQ.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditAccountPage : ContentPage
    {
        private Models.CoachUser user;
        public EditAccountPage()
        {
            InitializeComponent();
        }

        public EditAccountPage(Models.CoachUser coach)
        {
            InitializeComponent();
            user = coach;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Utils.SavedSettings.AccountImageSettings == null)
                AccountImage.Source = ImageSource.FromResource("ic_account.png");
            else
                AccountImage.Source = Utils.SavedSettings.AccountImageSettings;
        }

        private async void AccountImage_Tapped(object sender, EventArgs e)
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Pick a photo"
            });

            //if user doesn't pick image stop executing button press
            if (result == null)
                return;

            AccountImage.Source = ImageSource.FromFile(result.FullPath);

            try
            {
                Utils.SavedSettings.AccountImageSettings = result.FullPath;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Debug", ex.Message, "OK");
            }
        }

        private void BtnSave_Clicked(object sender, EventArgs e)
        {
            user.Name = entryName.Text;
            user.Email = entryEmail.Text;
            user.Dob = entryDOB.Text;
            user.Gender = entryGender.Text;
            Classes.AccountPageFunctions.EditAccountDetails(user);
            Navigation.PopModalAsync();
        }

        void BtnCancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}