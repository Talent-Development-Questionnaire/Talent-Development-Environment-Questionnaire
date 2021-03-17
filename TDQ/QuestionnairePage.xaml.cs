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
        public QuestionnairePage()
        {
            InitializeComponent();

             GetQuestions();
        }

        void GetQuestions()
        {
            string[] questions = Classes.DatabaseController.GenerateQuestions(59);
            var question_list = questions.ToList();
            question_list.RemoveAt(question_list.Count() - 1);
            LstQuestions.ItemsSource = question_list;
        }
    }
}