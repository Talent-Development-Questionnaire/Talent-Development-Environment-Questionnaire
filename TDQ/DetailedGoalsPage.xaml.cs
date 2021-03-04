﻿using System;
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
        GoalsGroup xyz;
        public ObservableCollection<GoalsGroup> GoalsGroups;
        public ObservableCollection<String> goals;
        public ObservableCollection<GoalsGroup> correctGoalsGroup;

        public DetailedGoalsPage(GoalsGroup currentGoalsGroup)
        {
            InitializeComponent();
            xyz = currentGoalsGroup;
            System.Diagnostics.Debug.WriteLine(xyz.Name);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GoalsGroups = new ObservableCollection<GoalsGroup>();
            goals = new ObservableCollection<string>();
            correctGoalsGroup = new ObservableCollection<GoalsGroup>();

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
                    GoalsGroups.Add(new GoalsGroup
                    {                        
                        Filename = filename,
                        Name = splitText[0],
                        ImageFilePath = splitText[1],
                        Color = splitText[2],
                        GoalsList = splitText[3]
                    });
                }
            }

            //Checks the filename of each GoalsGroup in GoalsGroups against GoalsGroup passed in
            foreach(GoalsGroup goalsGroup in GoalsGroups)
            {
                if (goalsGroup.Filename == xyz.Filename)
                {
                    correctGoalsGroup.Add(goalsGroup);
                }
            }

            //Updates ListView
            ListViewGoals.ItemsSource = correctGoalsGroup;
        }

        private async void AddGoal_Clicked(object sender, EventArgs e)
        {        
            //opens new AddGoalPage
            await Navigation.PushModalAsync(new PopupPages.AddGoalPage(xyz));
        }
    }
}