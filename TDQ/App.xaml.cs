using System;
using System.IO;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TDQ
{
    public partial class App : Application
    {
        public static string FolderPath { get; private set; }

        public App()
        {
            InitializeComponent();

           // Classes.DatabaseController.TestHttpRequest();
            Sharpnado.MaterialFrame.Initializer.Initialize(true, false);

            Classes.SettingsPageFunctions.ChangeTheme(Utils.SavedSettings.ThemeSettings);
            Classes.SettingsPageFunctions.ChangeFontSize(Utils.SavedSettings.FontSettings);

            FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

            if(!string.IsNullOrEmpty(Utils.SavedSettings.LoginSettings))
                MainPage = new Navigation_Drawer_Logged_In();
            else
                MainPage = new Navigation_Drawer();
        }
        
        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
