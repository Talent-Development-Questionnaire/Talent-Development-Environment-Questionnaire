using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TDQ
{
    public partial class App : Application
    {
        public string Theme { get; set; }
        public App()
        {
            InitializeComponent();

            MainPage = new Navigation_Drawer();
        }

        protected override void OnStart()
        {
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();
                switch (Utils.SavedSettings.ThemeSettings)
                {
                    case "Main":
                        mergedDictionaries.Add(new Themes.MainTheme());
                        break;
                    case "Blue":
                        mergedDictionaries.Add(new Themes.BlueTheme());
                        break;
                    default:
                        mergedDictionaries.Add(new Themes.MainTheme());
                        break;

                }
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
