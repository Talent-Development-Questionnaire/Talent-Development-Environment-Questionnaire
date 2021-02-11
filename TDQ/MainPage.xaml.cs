using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TDQ
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            Classes.SettingsPageFunctions.SetBackground(MainContentPage);
        }

        protected override void OnAppearing()
        {
            Classes.SettingsPageFunctions.SetBackground(MainContentPage);
        }

        private async void BtnAddQuestionnaire_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PopupPages.AddQuestionnairePage());
        }
    }
}
