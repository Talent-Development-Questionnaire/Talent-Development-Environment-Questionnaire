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
    public partial class AccountPage : ContentPage
    {
        public AccountPage()
        {
            InitializeComponent();
            
        }

        protected override void OnAppearing()
        {
            
            base.OnAppearing();

            if (Utils.SavedSettings.AccountImageSettings == null)
                AccountImage.Source = ImageSource.FromResource("ic_account.png");         
            else
            {
                AccountImage.Source = Utils.SavedSettings.AccountImageSettings;
            /*
                AccountImage.Source = ImageSource.FromResource("ic_add.png");
                Utils.SavedSettings.AccountImageSettings = result.FullPath;
            */
            }

            if (!string.IsNullOrEmpty(Utils.SavedSettings.LoginSettings))
            {
                bool userDetails = (Classes.DatabaseController.GetUserDetails(Utils.SavedSettings.LoginSettings));
                new Models.CoachUser
                {
                    ImageFilePath = null,
                    Name = null,
                    Email = null,
                    Gender = null,
                   // Dob = null
                };

                BindingContext = this;
            }
            else
                return;
        }


        private async void BtnEditAccount_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PopupPages.EditAccountPage());
        }


        // Move this to the popup page and change this to a function which opens the image in fullscreen
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
    }
}