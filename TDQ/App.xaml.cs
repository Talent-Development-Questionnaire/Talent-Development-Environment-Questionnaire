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

        public string Theme { get; set; }
        public App()
        {
            InitializeComponent();

            FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            switch(Utils.SavedSettings.LoginSettings)
            {
                case "LoggedIn":
                    MainPage = new Navigation_Drawer_Logged_In();
                    break;
                default:
                    MainPage = new Navigation_Drawer();
                    break;
            }
            
        }

        protected override void OnStart()
        {
            Classes.SettingsPageFunctions.ChangeTheme();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
