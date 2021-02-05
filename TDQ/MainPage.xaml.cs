using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TDQ
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void AddQuestionnaire_Clicked(object sender, EventArgs e)
        {
            CreateQuestionnaireLayout.IsVisible = true;
        }

        private void btnSendQuestionnaire_Clicked(object sender, EventArgs e)
        {
            CreateQuestionnaireLayout.IsVisible = false;
        }
    }
}
