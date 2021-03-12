using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDQ.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TDQ.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditAccountPage : ContentPage
    {
        public EditAccountPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            /* Old buggy code
             * This line needs removed 
            Utils.SavedSettings.AccountImageSettings = null;

            if (Utils.SavedSettings.AccountImageSettings == null)
            {
                AccountImage.Source = ImageSource.FromResource("ic_account.png");
            }
            /* This line is causing an issue 
            else
            {
                //AccountImage.Source = Utils.SavedSettings.AccountImageSettings = null;
                AccountImage.Source = ImageSource.FromResource("ic_add.png");

            }
            */

            // Fix
            if (Utils.SavedSettings.AccountImageSettings == null)
            {
                AccountImage.Source = ImageSource.FromResource("ic_account.png");
            }

            else
            {
                AccountImage.Source = Utils.SavedSettings.AccountImageSettings;
            }
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

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
        }

        void BtnCancel_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}