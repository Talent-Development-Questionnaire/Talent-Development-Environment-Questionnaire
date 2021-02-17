using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TDQ.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginTabView : TabbedPage
    {
        public LoginTabView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Classes.SettingsPageFunctions.ChangeTheme();
      
        }
    }
}