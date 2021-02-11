using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TDQ.Classes
{
    public class SettingsPageFunctions
    {
        public static void SetBackground(ContentPage page)
        {
            if (!string.IsNullOrEmpty(Utils.SavedSettings.BackgroundSettings))
                page.BackgroundImageSource = ImageSource.FromFile(Utils.SavedSettings.BackgroundSettings);
        }
    }
}
