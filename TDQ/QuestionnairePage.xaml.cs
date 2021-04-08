﻿using System;
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
        }

        private async void BtnQuestionnaireHelp_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PopupHelpPages.QuestionnaireHelp());

        }
        
    }
}