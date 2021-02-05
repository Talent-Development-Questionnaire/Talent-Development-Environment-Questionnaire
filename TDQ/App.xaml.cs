using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TDQ
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

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
