using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                }
            }        
        }
    }
}