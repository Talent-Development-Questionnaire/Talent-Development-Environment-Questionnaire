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
        }

        protected override void OnAppearing()
        {
            Classes.SettingsPageFunctions.SetBackground(ImgBg, MainContentPage);
        }

        private async void BtnAddQuestionnaire_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PopupPages.AddQuestionnairePage());
        }
    }
}
