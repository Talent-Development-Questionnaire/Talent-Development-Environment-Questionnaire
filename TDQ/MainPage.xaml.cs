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
            if (!string.IsNullOrEmpty(Utils.SavedSettings.BackgroundSettings))
                MainContentPage.BackgroundImageSource = ImageSource.FromFile(Utils.SavedSettings.BackgroundSettings);
        }

        protected override void OnAppearing()
        {
            if (!string.IsNullOrEmpty(Utils.SavedSettings.BackgroundSettings))
                MainContentPage.BackgroundImageSource = ImageSource.FromFile(Utils.SavedSettings.BackgroundSettings);
        }

        private async void BtnAddQuestionnaire_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PopupPages.AddQuestionnairePage());
        }
    }
}
