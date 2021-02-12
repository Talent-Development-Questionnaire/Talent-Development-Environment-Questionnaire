using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TDQ.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddQuestionnairePage : ContentPage
    {
        public AddQuestionnairePage()
        {
            InitializeComponent();
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();
                switch (PickerQuestionnaire.SelectedItem)
                {
                    case "58 Question":
                        /*
                         * Change this to open the large questionnaire
                        Utils.SavedSettings.ThemeIndexSettings = "0";
                        Utils.SavedSettings.ThemeSettings = "Main";
                        mergedDictionaries.Add(new Themes.MainTheme());
                        */
                        break;
                    case "29 Question":
                        /*
                         * Change this to open the small questionnaire

                        Utils.SavedSettings.ThemeIndexSettings = "1";
                        Utils.SavedSettings.ThemeSettings = "Blue";
                        mergedDictionaries.Add(new Themes.BlueTheme());
                        */
                        break;
                }
            }
        }
    }
}