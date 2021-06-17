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

            if (Device.RuntimePlatform == "iOS")
                BtnCancel.IsVisible = true;
            else
                BtnCancel.IsVisible = false;
        }
                    

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            //displays error message if field is empty
            if (string.IsNullOrWhiteSpace(EntryGoal.Text))
            {               
                await DisplayAlert("Error", "Goal cannot be empty. Please try again.", "OK");   
            } 
            //displays error message if goal already exists for that group
            //nested if statement necessary because otherwise error if it is null
            else if (selectedGoalsGroup.GoalsList != null)
            {
                if (selectedGoalsGroup.GoalsList.Contains(EntryGoal.Text))
                    await DisplayAlert("Error", "This goal already exists.", "OK");
                else
                {
                    //save goal to group
                    string goalsText = "\n" + EntryGoal.Text;
                    File.AppendAllText(selectedGoalsGroup.Filename, goalsText);
                    await Navigation.PopModalAsync();
                }
            } 
            else
            {
                //save goal to group
                string goalsText = "\n" + EntryGoal.Text;
                File.AppendAllText(selectedGoalsGroup.Filename, goalsText);
                await Navigation.PopModalAsync();
            }            
        }

        void BtnCancel_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}