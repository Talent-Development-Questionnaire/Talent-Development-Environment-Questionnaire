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
        public static ObservableCollection<Questionnaire> Questionnaires;

        public MainPage()
        {
            InitializeComponent();
            LstQuestionnaire.ItemsSource = Questionnaires;
        }

        protected override void OnAppearing()
        {
            Classes.SettingsPageFunctions.SetBackground(ImgBg, MainContentPage);
            Classes.SettingsPageFunctions.ChangeTheme(Utils.SavedSettings.ThemeSettings);
            LstQuestionnaire.ItemsSource = Questionnaires;
        }

        private async void BtnAddQuestionnaire_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PopupPages.AddQuestionnairePage());
        }
    }
}
