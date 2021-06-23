using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO;
using TDQ.Models;
using Xamarin.Forms;
using TDQ.ViewModels;

namespace TDQ
{
    public partial class MainPage : ContentPage
    {
        List<Questionnaire> Questionnaires;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new HomePageViewModel();
        }

        protected override void OnAppearing()
        {

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
           // UpdateQuestionnareList();
        }

        // If the help button is pressed then launch the help popup
        private async void BtnHomeHelp_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PopupHelpPages.MainPageHelp());

        }
    }
}
