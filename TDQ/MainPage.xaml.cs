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

        private async void ImgBtnAddQuestionnaire_Clicked(object sender, EventArgs e)
        {
            ImgBtnAddQuestionnaire.IsEnabled = false;
            await Navigation.PushModalAsync(new PopupPages.AddQuestionnairePage());
            ImgBtnAddQuestionnaire.IsEnabled = true;
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Confirm Exit", "Are you sure you want to exit?", "Yes", "No");

                if (result)
                {
                    System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow(); 
                }
            });
            return true;
        }

    }
}
