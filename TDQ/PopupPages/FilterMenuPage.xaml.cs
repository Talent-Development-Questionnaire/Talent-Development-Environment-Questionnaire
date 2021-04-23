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
            QuestionnaireScoresPage.UpdateList("LowestTen");
            Navigation.PopModalAsync();
        }

        void TopTen_Tapped(object sender, EventArgs e)
        {
            QuestionnaireScoresPage.UpdateList("TopTen");
            Navigation.PopModalAsync();
        }

        void All_Tapped(object sender, EventArgs e)
        {
            QuestionnaireScoresPage.UpdateList("All");
            Navigation.PopModalAsync();
            
        }

        void OutsideMenu_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}
