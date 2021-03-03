using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TDQ.Models;

namespace TDQ
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailedGoalsPage : ContentPage
    {
        GoalsGroup xyz;
        public DetailedGoalsPage(GoalsGroup currentGoalsGroup)
        {
            InitializeComponent();
            xyz = currentGoalsGroup;
            System.Diagnostics.Debug.WriteLine(xyz.Name);
        }

       
        private async void AddGoal_Clicked(object sender, EventArgs e)
        {
            //opens new AddGoalPage
            await Navigation.PushModalAsync(new PopupPages.AddGoalPage(xyz));
        }
    }


}