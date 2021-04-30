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
        List<string> inverted59Questions = new List<string>(new string[] { "8", "11", "15", "21", "23", "27", "28", "29", "31", "41", "44"});
        List<string> inverted28Questions = new List<string>(new string[] { "2", "5", "11", "12", "13", "17" });

        public QuestionnairePage()
        {
            InitializeComponent();
        }

        async void BtnConfirm_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(EntryEmail.Text) && !string.IsNullOrEmpty(EntryOTP.Text))
            {
                if (GetQuestions())
                {
                    LayoutUserVerification.IsVisible = false;
                    EntryEmail.Text = string.Empty;
                    EntryOTP.Text = string.Empty;
                    LayoutUserDetails.IsVisible = true;
                    return;
                }
            }
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

        async void BtnSendQuestionnaire_Clicked(object sender, EventArgs e)
        {
            //Set variables to default values
            int index = 0, years = 0;
            string gender = "%02%03";

            CheckAtheleteDetailsInput(gender, years);

            foreach (var item in questions)
            {
                if (string.IsNullOrEmpty(item.Answer))
                {
                    await DisplayAlert("Error", "You haven't answered all the questions, please complete the form!", "OK");
                    return;
                }

                index++;

                if(inverted59Questions.Contains(index.ToString()) && inverted28Questions.Contains(index.ToString()))
                {
                    switch(item.Answer)
                    {
                        case "1":
                            item.Answer = "6";
                            break;
                        case "2":
                            item.Answer = "5";
                            break;
                        case "3":
                            item.Answer = "4";
                            break;
                        case "4":
                            item.Answer = "3";
                            break;
                        case "5":
                            item.Answer = "2";
                            break;
                        case "6":
                            item.Answer = "1";
                            break;
                    }
                }
                

                Classes.DatabaseController.SendCompletedQuestionnaire(item, index);

                if (item == questions[questions.Count - 1])
                    Classes.DatabaseController.UpdateQuestionnaireCompletions(item);
            }
            DisplayAlert("Successful", "Questionnaire was successfully completed, thank you!", "OK");

            LstQuestions.ItemsSource = null;
            LayoutUserDetails.IsVisible = false;
            LayoutUserVerification.IsVisible = true;

        }

        //Method used for debugging, random auto answers all the questions in the questionnaire
        void BtnAutoComplete_Clicked(object sender, EventArgs e)
        {
            Random r = new Random();
            foreach(var item in questions)
            {
                item.Answer = r.Next(1,6).ToString();
            }

            LstQuestions.ItemsSource = questions;
        }

        // If the help button is pressed then launch the help popup
        private async void BtnQuestionnaireHelp_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PopupHelpPages.QuestionnaireHelp());

        }

        //Calculates the age from the inputted text and the present date
        int CalculateAge(int years)
        {
            //Initialising variables
            DateTime dob = DateTime.Parse(EntryDob.Text, new CultureInfo("en-GB").DateTimeFormat);
            DateTime today = DateTime.Now;
            years = today.Year - dob.Year;

            if (dob.Month == today.Month &&// if the start month and the end month are the same
                today.Day < dob.Day // AND the end day is less than the start day
                || today.Month < dob.Month)// OR if the end month is less than the start month
                years--;//minus one if the month and day are less than the present day

            return years;
        }

        //Gets the file of questions from the server
        bool GetQuestions()
        {
            LstQuestions.IsVisible = true;
            //returns a list of questions based on the inputted email and one time password
            questions = Classes.DatabaseController.GenerateQuestions(EntryEmail.Text, EntryOTP.Text);
            //checks that the questions is not empty - last item in list is always empty
            if (questions != null)
            {
                //removes the empty item in the list
                questions.RemoveAt(questions.Count() - 1);
                //sets the list view to the list of questions
                LstQuestions.ItemsSource = questions;
                return true;
            }
            //Throws error if the inputted email or One Time Password does not match whats in the database
            DisplayAlert("Error", "Email or One Time Password is incorrect, please try again!", "OK");
            return false;
        }

        async void CheckAtheleteDetailsInput(string gender, int years)
        {
            //Fields cannot be left empty, exit method if they are
            if (string.IsNullOrEmpty(EntrySport.Text) || string.IsNullOrEmpty(EntryAcademy.Text))
            {
                await DisplayAlert("Error", "Please enter your sport and the academy/club you attend", "OK");
                return;
            }

            //Set athlete name to value accepted by http url if it's left empty
            if (string.IsNullOrEmpty(EntryName.Text))
                EntryName.Text = "%02%03";
            //Set athlete date of birth to value accepted by http url if it's left empty
            if (string.IsNullOrEmpty(EntryDob.Text))
                EntryName.Text = "%02%03";
            else
                //Returns the age calculated from the inputed dob as a string
                years = CalculateAge(years);

            //set gender to selected choice if an item was selected
            if (PickerGender.SelectedIndex != -1)
                gender = PickerGender.SelectedItem.ToString();

            //Send all the inputted details to the server, the ones left empty set to special characters seen as 'null' when in a URL
            Classes.DatabaseController.SendUserDetails(EntryName.Text, EntrySport.Text, EntryAcademy.Text, years.ToString(), gender);
        }
    }    
}