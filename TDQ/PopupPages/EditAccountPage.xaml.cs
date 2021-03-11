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
            //Initialises Group object
            var account = (CoachUser)BindingContext;

            //Checks if group name has been set, displays error if it hasn't
            if (!string.IsNullOrWhiteSpace(account.Name))
            {
                //Checks if the image as been changed or not
                if ((Utils.SavedSettings.GroupImageSetting != account.ImageFilePath && !string.IsNullOrEmpty(Utils.SavedSettings.GroupImageSetting)) || string.IsNullOrEmpty(account.ImageFilePath))
                    account.ImageFilePath = Utils.SavedSettings.GroupImageSetting;
                
                //Checks if the group is a new one or is just being edited
                if (string.IsNullOrWhiteSpace(account.Filename))
                {
                    //Save
                    //Creates new file and writes the Group properties' values to said file
                    var filename = Path.Combine(App.FolderPath, $"{Path.GetRandomFileName()}.account.txt");

                    File.WriteAllText(filename, account.Name + "\n" + account.Email + "\n" + account.Dob + "\n");
                } 
            else
            {
                //Update
                //Overwrites the current file with Group values
                File.WriteAllText(account.Filename, account.Name + "\n" + account.Email + "\n" + account.Dob + "\n");
                // File.AppendAllLines(account.Filename);
            }

                await Navigation.PopModalAsync();
            }
        else
            {
                await DisplayAlert("Error", "Cannot save empty entry!\n Press back button at top of screen to exit page", "OK");
            }
        }

        void BtnCancel_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}