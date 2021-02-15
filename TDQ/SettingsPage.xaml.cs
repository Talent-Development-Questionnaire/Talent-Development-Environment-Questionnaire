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
            //Sets the page background whenever page is opened
            Classes.SettingsPageFunctions.SetBackground(ImgBg, SettingsContentPage);

            //Changes the index of the picker to what the user picked last time
            //If the setting is null then the picker is set to deafult "Red"
            if (!string.IsNullOrEmpty(Utils.SavedSettings.ThemeIndexSettings))
                PickerColour.SelectedIndex = Convert.ToInt32(Utils.SavedSettings.ThemeIndexSettings);
            else
                PickerColour.SelectedIndex = 0;
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        { 
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            //resets app's resource dictionary to null
            //changes app resource dictionary depending on what the user chooses
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();
                switch (PickerColour.SelectedItem)
                {
                    case "Red": //Red and main theme
                        Utils.SavedSettings.ThemeIndexSettings = "0"; // Saves the chosen index
                        Utils.SavedSettings.ThemeSettings = "Main"; // Saves which theme was chosen
                        mergedDictionaries.Add(new Themes.MainTheme()); // Sets the chosen theme
                        break;

                    case "Blue": //Blue theme
                        Utils.SavedSettings.ThemeIndexSettings = "1";
                        Utils.SavedSettings.ThemeSettings = "Blue";
                        mergedDictionaries.Add(new Themes.BlueTheme());
                        break;

                    case "Green": //Green theme
                        Utils.SavedSettings.ThemeIndexSettings = "2";
                        Utils.SavedSettings.ThemeSettings = "Green";
                        mergedDictionaries.Add(new Themes.GreenTheme());
                        break;

                    case "Yellow": //Yellow theme
                        Utils.SavedSettings.ThemeIndexSettings = "3";
                        Utils.SavedSettings.ThemeSettings = "Yellow";
                        mergedDictionaries.Add(new Themes.YellowTheme());
                        break;

                    case "Orange": //Orange theme
                        Utils.SavedSettings.ThemeIndexSettings = "4";
                        Utils.SavedSettings.ThemeSettings = "Orange";
                        mergedDictionaries.Add(new Themes.OrangeTheme());
                        break;

                    case "Dark": //Dark theme
                        Utils.SavedSettings.ThemeIndexSettings = "5";
                        Utils.SavedSettings.ThemeSettings = "Dark";
                        mergedDictionaries.Add(new Themes.DarkTheme());
                        break;
                }
            }        
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
            //Mehtod to set the background of the current content page
            Classes.SettingsPageFunctions.SetBackground(ImgBg, SettingsContentPage);

            //Saves the file path to storage to be called again later
            try
            {
                Utils.SavedSettings.BackgroundSettings = result.FullPath;
            }
            catch (Exception ex)
            {

                await App.Current.MainPage.DisplayAlert("Debug", ex.Message, "OK");
            }
        }
    }
}