using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO;
using TDQ.Models;
using Xamarin.Forms;

namespace TDQ
{
    public partial class MainPage : ContentPage
    {
        List<Questionnaire> Questionnaires;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            UpdateQuestionnareList();

            LblNoQuestionnaires.IsVisible = true;
            if (LstQuestionnaire.ItemsSource != null)
                LblNoQuestionnaires.IsVisible = false;

            if(LstQuestionnaire.SelectedItem != null)
                LstQuestionnaire.SelectedItem = null;

            Classes.GlobalFunctions.ChangeTheme(Utils.SavedSettings.ThemeSettings);
        }

        private async void ImgBtnAddQuestionnaire_Clicked(object sender, EventArgs e)
        {
            ImgBtnAddQuestionnaire.IsEnabled = false;
            await Navigation.PushModalAsync(new PopupPages.AddQuestionnairePage());
            ImgBtnAddQuestionnaire.IsEnabled = true;
        }

        public void UpdateQuestionnareList()
        {
            if (!string.IsNullOrEmpty(Utils.SavedSettings.LoginSettings))
            {
                Questionnaires = Classes.DatabaseController.GetQuestionnaires(Utils.SavedSettings.LoginSettings);
                if(Questionnaires != null)
                    LstQuestionnaire.ItemsSource = Questionnaires;
            }
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

        private async void LstQuestionnaire_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var questionnaire = e.SelectedItem as Questionnaire; 
                await Navigation.PushAsync(new QuestionnaireScoresPage(questionnaire.Questions, questionnaire));
            }
        }

        private void LstQuestionnaire_Refreshing(object sender, EventArgs e)
        {
            UpdateQuestionnareList();
        }

        // If the help button is pressed then launch the help popup
        private async void BtnHomeHelp_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PopupHelpPages.MainPageHelp());

        }
    }
}
