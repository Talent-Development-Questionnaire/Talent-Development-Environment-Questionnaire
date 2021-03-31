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
        //List<Models.Question> questions;
        public QuestionnaireScoresPage()
        {
            InitializeComponent();
        }

        public QuestionnaireScoresPage(List<Models.Question> questions)
        {
            InitializeComponent();

            //this.questions = questions;
            LstScore.ItemsSource = questions;
        }
    }
}