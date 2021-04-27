using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TDQ.Classes;

namespace TDQ
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountPage : ContentPage
    {
        private Models.CoachUser user;
        public AccountPage()
        {
            InitializeComponent();
            
        }

        protected override void OnAppearing()
        {          
            base.OnAppearing();

            // If there is no profile photo saved on the users mobile device then set the image as the default 'add a profile picture' image
            if (Utils.SavedSettings.AccountVerified == null)
                AccountImage.Source = ImageSource.FromResource("ic_action_add_a_photo.png");

           // Otherwise, set the profile photo as the image saved previously by the user
            else
            {
                AccountImage.Source = Utils.SavedSettings.AccountVerified;
                new Templates.HeaderContent();
            }

            // Get the user details from the saved settings from the database, if there are none then leave these blank
            user = DatabaseController.GetUserDetails(Utils.SavedSettings.LoginSettings);
            if(user != null)
                BindingContext = user;
        }

        // Method which opens the edit account popup page when the edit account button is pressed
        private async void BtnEditAccount_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PopupPages.EditAccountPage(user)
            {
                BindingContext = user
            }) ;
        }


        // When the profile image is pressed allow the user to select a new profile photo
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
                Utils.SavedSettings.AccountVerified = result.FullPath;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Debug", ex.Message, "OK");
            }
        }

        // If the back button is pressed return to the home page
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.GoToAsync("//home");

            });
            return true;
        }

        // If the help button is pressed then launch the help popup
        private async void BtnAccountHelp_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PopupHelpPages.AccountsPageHelp());

        }
    }
}