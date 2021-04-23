using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TDQ.PopupPages
{
    public partial class LikertScalePage : ContentPage
    {
        Models.Question q;

        public LikertScalePage()
        {
            InitializeComponent();
        }

        public LikertScalePage(Models.Question question)
        {
            InitializeComponent();
            q = question;
            switch (q.Answer)
            {
                case "1":
                    RbOne.IsChecked = true;
                    break;
                case "2":
                    RbTwo.IsChecked = true;
                    break;
                case "3":
                    RbThree.IsChecked = true;
                    break;
                case "4":
                    RbFour.IsChecked = true;
                    break;
                case "5":
                    RbFive.IsChecked = true;
                    break;
                case "6":
                    RbSix.IsChecked = true;
                    break;
            }
        }

        void BtnConfirm_Clicked(object sender, EventArgs e)
        {
            if (q.Answer != null)
                q.QuestionColor = Color.Green;
            Navigation.PopModalAsync();
        }

        void AnswerRadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var x = sender as RadioButton;

            q.Answer = x.Content.ToString();
        }
    }
}
