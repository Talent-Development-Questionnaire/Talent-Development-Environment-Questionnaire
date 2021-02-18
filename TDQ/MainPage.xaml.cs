using System;
using System.Collections.ObjectModel;
using System.IO;
using TDQ.Models;
using Xamarin.Forms;

namespace TDQ
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Group> Groups;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            Classes.SettingsPageFunctions.SetBackground(ImgBg, MainContentPage);
            Classes.SettingsPageFunctions.ChangeTheme(Utils.SavedSettings.ThemeSettings);
        }

        private async void BtnAddQuestionnaire_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PopupPages.AddQuestionnairePage());
        }
    }
}
