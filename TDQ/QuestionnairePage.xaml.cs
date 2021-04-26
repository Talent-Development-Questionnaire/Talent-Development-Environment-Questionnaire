using System;
using System.Collections.Generic;
using System.Globalization;
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
                await DisplayAlert("Error", "Email or One Time Password is incorrect, please try again!", "OK");
        }

        void BtnConfirm_Clicked(System.Object sender, System.EventArgs e)
        {
            LayoutUserVerification.IsVisible = false;
            GetQuestions();
            EntryEmail.Text = string.Empty;
            EntryOTP.Text = string.Empty;
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

        async void LstQuestions_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PushModalAsync(new PopupPages.LikertScalePage(e.SelectedItem as Models.Question){
                BindingContext = e.SelectedItem as Models.Question
            });

            LstQuestions.ItemsSource = questions;
            BindingContext = questions;
        }

        public void UpdateQuestion(Models.Question question)
        {
            int index = questions.FindIndex(m => m.QuestionText == question.QuestionText);
            if (index >= 0)
                questions[index] = question;

            LstQuestions.ItemsSource = questions;
        }

        async void BtnSendQuestionnaire_Clicked(object sender, EventArgs e)
        {
            //Need to check if information already exists, or do all this after checking the questionnaire has been answered
            int index = 0, years = 0;
            string gender = "%02%03";
            string age = "%02%03";

            if (string.IsNullOrEmpty(EntrySport.Text) || string.IsNullOrEmpty(EntryAcademy.Text))
            {
                await DisplayAlert("Error", "Please enter your sport and the academy/club you attend", "OK");
                return;
            }

            if (string.IsNullOrEmpty(EntryName.Text))
                EntryName.Text = "%02%03";

            if (string.IsNullOrEmpty(EntryDob.Text))
                EntryName.Text = "%02%03";

            if (PickerGender.SelectedIndex != -1)
                gender = PickerGender.SelectedItem.ToString();

            if (!string.IsNullOrEmpty(EntryDob.Text))
            {
                DateTime dob = DateTime.Parse(EntryDob.Text, new CultureInfo("en-GB").DateTimeFormat);
                DateTime today = DateTime.Now;
                years = today.Year - dob.Year;

                if (dob.Month == today.Month &&// if the start month and the end month are the same
                    today.Day < dob.Day // AND the end day is less than the start day
                    || today.Month < dob.Month)// OR if the end month is less than the start month
                {
                    years--;
                }

                age = years.ToString();
            }

             

            Classes.DatabaseController.SendUserDetails(EntryName.Text, EntrySport.Text, EntryAcademy.Text, age, gender);

            foreach (var item in questions)
            {
                if (string.IsNullOrEmpty(item.Answer))
                {
                    await DisplayAlert("Error", "You haven't answered all the questions, please complete the form!", "OK");
                    return;
                }
            }

            foreach (var item in questions)
            {
                index++;
                Classes.DatabaseController.SendCompletedQuestionnaire(item, index);

                if (item == questions[questions.Count - 1])
                    Classes.DatabaseController.UpdateQuestionnaireCompletions(item);
            }
            DisplayAlert("Successful", "Questionnaire was successfully completed, thank you!", "OK");

            LstQuestions.ItemsSource = null;
            LayoutUserDetails.IsVisible = false;
            LayoutUserVerification.IsVisible = true;

        }

        void BtnAutoComplete_Clicked(object sender, EventArgs e)
        {
            Random r = new Random();
            foreach(var item in questions)
            {
                item.Answer = r.Next(1,6).ToString();
            }

            LstQuestions.ItemsSource = questions;
        }
        private async void BtnQuestionnaireHelp_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PopupHelpPages.QuestionnaireHelp());

        }
    }
        
}