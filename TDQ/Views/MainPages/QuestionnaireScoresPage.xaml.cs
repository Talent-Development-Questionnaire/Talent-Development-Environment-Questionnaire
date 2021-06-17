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
        static Models.Questionnaire newQuestionnaire;
        static List<Models.Question> q;
        static List<Models.Question> copyQ;
        static string copyFilter = "All";

        public QuestionnaireScoresPage()
        {
            InitializeComponent();
        }

        public QuestionnaireScoresPage(List<Models.Question> questions, Models.Questionnaire questionnaire)
        {
            InitializeComponent();

            newQuestionnaire = questionnaire;
            q = questions.OrderByDescending(x => x.Answer).ToList();
            copyQ = questions;
        }

        protected override void OnAppearing()
        {
            lblListType.Text = copyFilter;
            LstScore.ItemsSource = q;
        }

        async void ImgBtnFilterScores_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PopupPages.FilterMenuPage(), true);
        }

        public static void UpdateList(string filter)
        {
            copyFilter = filter;
            switch (filter)
            {
                case "Weaknesses":
                    
                    q = copyQ;
                    if (newQuestionnaire.Type == "59")
                    {
                        q = q.OrderBy(x => x.Answer).Take(10).ToList();
                        return;
                    }
                    q = q.OrderBy(x => x.Answer).Take(5).ToList();
                    break;

                case "Strengths":
                    q = copyQ;
                    if (newQuestionnaire.Type == "59")
                    {                        
                        q = q.OrderByDescending(x => x.Answer).Take(10).ToList();
                        return;
                    }

                    q = q.OrderByDescending(x => x.Answer).Take(5).ToList();
                    break;

                case "All":
                    q = copyQ.OrderByDescending(x => x.Answer).ToList();
                    break;
            }
        }
    }
}