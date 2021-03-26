using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TDQ
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuestionnairePage : ContentPage
    {
        List<Models.Question> questions;

        public QuestionnairePage()
        {
            InitializeComponent();         
        }


        async void GetQuestions()
        {
            LstQuestions.IsVisible = true;
            questions = Classes.DatabaseController.GenerateQuestions(EntryEmail.Text, EntryOTP.Text);

            if (questions != null)
            {
                questions.RemoveAt(questions.Count() - 1);
                LstQuestions.ItemsSource = questions;

            }
            else
            {
                await DisplayAlert("Error", "Email or One Time Password is incorrect, please try again!", "OK");
            }
        }

        void BtnConfirm_Clicked(System.Object sender, System.EventArgs e)
        {
            LayoutUserVerification.IsVisible = false;
            GetQuestions();
            LayoutUserDetails.IsVisible = true;
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.GoToAsync("//home");

            });
            return true;
        }

        void LstQuestions_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Navigation.PushModalAsync(new PopupPages.LikertScalePage(e.SelectedItem as Models.Question){
                BindingContext = e.SelectedItem as Models.Question
            });
        }

        public void UpdateQuestion(Models.Question question)
        {
            int index = questions.FindIndex(m => m.QuestionText == question.QuestionText);
            if (index >= 0)
                questions[index] = question;

            LstQuestions.ItemsSource = questions;
        }
    }    
}