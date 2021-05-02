using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TDQ.PopupPages
{
    public partial class FilterMenuPage : ContentPage
    {
        public FilterMenuPage()
        {
            InitializeComponent();
        }

        void LowestTen_Tapped(object sender, EventArgs e)
        {
            //Reset list to lowest 10 scoring items
            QuestionnaireScoresPage.UpdateList("Weaknesses");
            Navigation.PopModalAsync();
        }

        void TopTen_Tapped(object sender, EventArgs e)
        {
            //Reset list to highest 10 scoring items
            QuestionnaireScoresPage.UpdateList("Strengths");
            Navigation.PopModalAsync();
        }

        void All_Tapped(object sender, EventArgs e)
        {
            //Reset list to original order - highest to lowest
            QuestionnaireScoresPage.UpdateList("All");
            Navigation.PopModalAsync();
            
        }

        void OutsideMenu_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}
