using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TDQ.Models;
using System.IO;
using System.Collections.ObjectModel;

namespace TDQ
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailedGoalsPage : ContentPage
    {
        GoalsGroup selectedGoalsGroup;
        public ObservableCollection<GoalsGroup> GoalsGroups;
        public ObservableCollection<String> goals;

        public DetailedGoalsPage(GoalsGroup currentGoalsGroup)
        {
            InitializeComponent();
            selectedGoalsGroup = currentGoalsGroup;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GoalsGroups = new ObservableCollection<GoalsGroup>();           
            goals = new ObservableCollection<string>();

            //Gets all the saved files for created goalsgroups
            var files = Directory.EnumerateFiles(App.FolderPath, "*.goalsgroup.txt");
            //iterates through each file
            foreach (var filename in files)
            {
                string text = (File.ReadAllText(filename));
                string[] splitText = text.Split('\n');

                //Adds GoalsGroup object to list, sets each property of the object
                if (splitText.Length > 3)
                {
                    //Method to populate array with goals from the file
                    string[] goalsList = Classes.DetailedGoalsPageFunctions.PopulateListOnAppearing(splitText);

                    GoalsGroups.Add(new GoalsGroup
                    {                        
                        Filename = filename,
                        Name = splitText[0],
                        ImageFilePath = splitText[1],
                        Color = splitText[2],
                        GoalsList = goalsList
                    });
                }
            }

            //Checks the filename of each GoalsGroup in GoalsGroups against GoalsGroup passed in
            foreach(GoalsGroup goalsGroup in GoalsGroups)
            {
                if (goalsGroup.Filename == selectedGoalsGroup.Filename)
                {
                    foreach(string goal in goalsGroup.GoalsList)
                    {
                        goals.Add(goal);
                    }
                }
            }
            //Updates ListView
            ListViewGoals.HeightRequest = 30 * goals.Count();
            ListViewGoals.ItemsSource = goals;            
            HideList();
        }

        public void HideList()
        {
            if (goals == null || goals.Count() == 0)
            {
                ListViewGoals.IsVisible = false;
                LblNewGoal.IsVisible = true;
            }
            else
            {
                ListViewGoals.IsVisible = true;
                LblNewGoal.IsVisible = false;
            }
        }
        private async void AddGoal_Clicked(object sender, EventArgs e)
        {        
            //opens new AddGoalPage
            await Navigation.PushModalAsync(new PopupPages.AddGoalPage(selectedGoalsGroup));
        }
    }
}