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
            if (string.IsNullOrWhiteSpace(EntryGoal.Text))
            {               
                await DisplayAlert("Error", "Goal cannot be empty. Please try again.", "OK");   
            } 
            else
            {
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