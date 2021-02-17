using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TDQ.Classes
{
    public class SettingsPageFunctions
    {
        //Sets the chosen image as app background
        public static void SetBackground(Image img, ContentPage page)
        {
            if (!string.IsNullOrEmpty(Utils.SavedSettings.BackgroundSettings))
                img.Source = ImageSource.FromFile(Utils.SavedSettings.BackgroundSettings);
            else if (img.Source == null)
                page.BackgroundColor = Color.FromHex("#7F7F7F");
        }

        //Sets the desired app theme
        public static void ChangeTheme()
        {
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            //resets app's resource dictionary to null
            //changes app resource dictionary depending on what the user chooses
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();
            }
            switch (Utils.SavedSettings.ThemeSettings)
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

                default:
                    Utils.SavedSettings.ThemeIndexSettings = "0"; // Saves the chosen index
                    Utils.SavedSettings.ThemeSettings = "Main"; // Saves which theme was chosen
                    mergedDictionaries.Add(new Themes.MainTheme()); // Sets the chosen theme
                    break;
            }
        }


        public static void PickerChangeTheme(string item)
        {
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            //resets app's resource dictionary to null
            //changes app resource dictionary depending on what the user chooses
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();
            }
            switch (item)
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

                default:
                    Utils.SavedSettings.ThemeIndexSettings = "0"; // Saves the chosen index
                    Utils.SavedSettings.ThemeSettings = "Main"; // Saves which theme was chosen
                    mergedDictionaries.Add(new Themes.MainTheme()); // Sets the chosen theme
                    break;

            }           
        }
    }
}
