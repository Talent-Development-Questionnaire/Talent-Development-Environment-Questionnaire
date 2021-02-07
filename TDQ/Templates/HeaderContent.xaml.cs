using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace TDQ.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HeaderContent : ContentView
    {
        public HeaderContent()
        {
            InitializeComponent();
            try
            {
                AccountImage.Source = ImageSource.FromFile(Utils.SavedSettings.AccountImageSettings);
            }
            catch(Exception e)
            {
                App.Current.MainPage.DisplayAlert("Debug", "Account image is not working", "OK");
            }
        }

        private async void AccountImage_Tapped(object sender, EventArgs e)
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Pick a photo"
            });

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