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
    public partial class QuestionnaireScoresPage : ContentPage
    {
        static List<Models.Question> q;
        static List<Models.Question> copyQ;
        static List<Models.Question> negativeQ = new List<Models.Question>();
        static List<string> inverted59Questions = new List<string>(new string[] { "8", "11", "15", "21", "23", "27", "28", "29", "31", "41", "44" });

        public QuestionnaireScoresPage()
        {
            InitializeComponent();
        }

        public QuestionnaireScoresPage(List<Models.Question> questions)
        {
            InitializeComponent();

            q = questions;
            copyQ = questions;
            PopulateNegativeQuestions();
        }

        protected override void OnAppearing()
        {
            LstScore.ItemsSource = q;
            LstNegativeScores.ItemsSource = negativeQ;
        }

        async void ImgBtnFilterScores_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PopupPages.FilterMenuPage(), true);
        }

        private static void PopulateNegativeQuestions()
        {
            foreach (var item in q)
            {
                if (inverted59Questions.Contains(item.QuestionNo))
                {
                    negativeQ.Add(item);
                    q.Remove(item);
                }
            }
        }

        public static void UpdateList(string filter)
        {
            switch(filter)
            {
                case "LowestTen":
                    q = copyQ;

                    PopulateNegativeQuestions();

                    negativeQ = negativeQ.OrderBy(x => x.Answer).Take(10).ToList();
                    q = q.OrderBy(x => x.Answer).Take(10).ToList();
                    break;
                case "TopTen":
                    q = copyQ;

                    PopulateNegativeQuestions();

                    negativeQ = negativeQ.OrderByDescending(x => x.Answer).Take(10).ToList();
                    q = q.OrderByDescending(x => x.Answer).Take(10).ToList();
                    break;
                case "All":
                    q = copyQ;
                    PopulateNegativeQuestions();
                    break;
            }
        }
    }
}