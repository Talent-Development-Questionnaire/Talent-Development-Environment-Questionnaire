using System;
using System.Collections.ObjectModel;
using System.IO;
using TDQ.Models;
using Xamarin.Forms;

namespace TDQ
{
    public partial class MainPage : ContentPage
    {
        ObservableCollection<Questionnaire> Questionnaires = new ObservableCollection<Questionnaire>();

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

            await Navigation.PushModalAsync(new PopupPages.AddQuestionnairePage(this));
        }

        public void AddQuestionnaireToList(Questionnaire item)
        {
            Questionnaires.Add(item);
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
