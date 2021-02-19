﻿using System;
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
            else if (img.Source == null || string.IsNullOrEmpty(Utils.SavedSettings.BackgroundSettings))
            {
                img.Source = null;
                page.BackgroundColor = Color.White;
            }
        }

        public static void DeleteBackgroundVisibility(Button delete)
        {
            if (string.IsNullOrEmpty(Utils.SavedSettings.BackgroundSettings))
                delete.IsVisible = false;
            else
                delete.IsVisible = true;
            

        }

        public static void ChangeTheme(string item)
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

        public static void ChangeFontSize(string item)
        {
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            //resets app's resource dictionary to null
            //changes app resource dictionary depending on what the user chooses
            if (mergedDictionaries != null)
            {
                switch (item)
                {
                    case "Small": //Small Font
                        Utils.SavedSettings.FontIndexSettings = "0"; // Saves the chosen index
                        Utils.SavedSettings.FontSettings = "Small"; // Saves which theme was chosen
                        mergedDictionaries.Add(new Themes.SmallFontStyle()); // Sets the chosen theme
                        break;

                    case "Medium": //Medium Font
                        Utils.SavedSettings.FontIndexSettings = "1";
                        Utils.SavedSettings.FontSettings = "Medium";
                        mergedDictionaries.Add(new Themes.MediumFontStyle());
                        break;

                    case "Large": //Large Font
                        Utils.SavedSettings.FontIndexSettings = "2";
                        Utils.SavedSettings.FontSettings = "Large";
                        mergedDictionaries.Add(new Themes.LargeFontStyle());
                        break;

                    case "X-Large": //Extra Large Font
                        Utils.SavedSettings.FontIndexSettings = "3";
                        Utils.SavedSettings.FontSettings = "X-Large";
                        mergedDictionaries.Add(new Themes.X_LargeFontStyle());
                        break;

                    default:
                        Utils.SavedSettings.FontIndexSettings = "0"; // Saves the chosen index
                        Utils.SavedSettings.FontSettings = "Small"; // Saves which theme was chosen
                        mergedDictionaries.Add(new Themes.SmallFontStyle()); // Sets the chosen theme
                        break;
                }
            }
        }
    }
}
