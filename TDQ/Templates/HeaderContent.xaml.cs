using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace TDQ.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HeaderContent : ContentView
    {
        private Models.CoachUser user;
        public HeaderContent()
        {
            InitializeComponent();
            try
            {
                AccountImage.Source = ImageSource.FromFile(Utils.SavedSettings.AccountImageSettings);
            }
            catch(Exception e)
            {
                App.Current.MainPage.DisplayAlert("Debug", "Account image is not working", "OK");
            }

            if (!string.IsNullOrEmpty(Utils.SavedSettings.LoginSettings))
            {
                user = Classes.DatabaseController.GetUserDetails(Utils.SavedSettings.LoginSettings);
                BindingContext = user;
            }
            
        }
    }
}