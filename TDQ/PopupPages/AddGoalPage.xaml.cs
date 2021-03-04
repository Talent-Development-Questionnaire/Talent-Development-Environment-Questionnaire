using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TDQ.Models;

namespace TDQ.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddGoalPage : ContentPage
    {
        GoalsGroup selectedGoalsGroup;
        public AddGoalPage(GoalsGroup currentGoalsGroup)
        {
            selectedGoalsGroup = currentGoalsGroup;
            InitializeComponent();
        }
                    

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {       
            string goalsText = "\n" + EntryGoal.Text;
            File.AppendAllText(selectedGoalsGroup.Filename, goalsText);
            await Navigation.PopModalAsync();
        }
    }
}