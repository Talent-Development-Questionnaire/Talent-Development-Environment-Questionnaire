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
            Classes.SettingsPageFunctions.SetBackground(ImgBg, SettingsContentPage);

            if (!string.IsNullOrEmpty(Utils.SavedSettings.ThemeIndexSettings))
                PickerColour.SelectedIndex = Convert.ToInt32(Utils.SavedSettings.ThemeIndexSettings);
            else
                PickerColour.SelectedIndex = 0;
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();
                switch (PickerColour.SelectedItem)
                {
                    case "Red":
                        Utils.SavedSettings.ThemeIndexSettings = "0";
                        Utils.SavedSettings.ThemeSettings = "Main";
                        mergedDictionaries.Add(new Themes.MainTheme());
                        break;
                    case "Blue":
                        Utils.SavedSettings.ThemeIndexSettings = "1";
                        Utils.SavedSettings.ThemeSettings = "Blue";
                        mergedDictionaries.Add(new Themes.BlueTheme());
                        break;
                    case "Green":
                        Utils.SavedSettings.ThemeIndexSettings = "2";
                        Utils.SavedSettings.ThemeSettings = "Green";
                        mergedDictionaries.Add(new Themes.GreenTheme());
                        break;
                    case "Yellow":
                        Utils.SavedSettings.ThemeIndexSettings = "3";
                        Utils.SavedSettings.ThemeSettings = "Yellow";
                        mergedDictionaries.Add(new Themes.YellowTheme());
                        break;
                    case "Orange":
                        Utils.SavedSettings.ThemeIndexSettings = "4";
                        Utils.SavedSettings.ThemeSettings = "Orange";
                        mergedDictionaries.Add(new Themes.OrangeTheme());
                        break;
                    case "Dark":
                        Utils.SavedSettings.ThemeIndexSettings = "5";
                        Utils.SavedSettings.ThemeSettings = "Dark";
                        mergedDictionaries.Add(new Themes.DarkTheme());
                        break;
                }
            }        
        }

        private async void BtnAddBg_Clicked(object sender, EventArgs e)
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Pick a photo"
            });

            if (result == null)
                return;
            BtnAddBg.Source = ImageSource.FromFile(result.FullPath);
            Classes.SettingsPageFunctions.SetBackground(ImgBg, SettingsContentPage);

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