using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TDQ.Classes
{
    public class SettingsPageFunctions
    {
        public static void SetBackground(Image img, ContentPage page)
        {
            if (!string.IsNullOrEmpty(Utils.SavedSettings.BackgroundSettings))
                img.Source = ImageSource.FromFile(Utils.SavedSettings.BackgroundSettings);
            else if (img.Source == null)
                page.BackgroundColor = Color.FromHex("#7F7F7F");
        }
    }
}
