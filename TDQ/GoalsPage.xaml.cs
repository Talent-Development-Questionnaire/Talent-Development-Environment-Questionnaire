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
    public partial class GoalsPage : ContentPage
    {
        public ObservableCollection<GoalsGroup> GoalsGroups;
        public GoalsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            Classes.SettingsPageFunctions.SetBackground(ImgBg, GoalsContentPage);

            //Initialising variables
            GoalsGroups = new ObservableCollection<GoalsGroup>();
            Classes.SettingsPageFunctions.SetBackground(ImgBg, GoalsContentPage);
            ListViewGoalsGroup.SelectedItem = null;

            //Gets all the saved files for created goalsgroups
            var files = Directory.EnumerateFiles(App.FolderPath, "*.goalsgroup.txt");
            //iterates through each file and adds the goalsgroup to the list view
            foreach (var filename in files)
            {
                string text = (File.ReadAllText(filename));
                string[] splitText = text.Split('\n');

                //Adds GoalsGroup object to list, sets each property of the object

                string backgroundColor;

                switch (splitText[3])
                {
                    case "Red":
                        backgroundColor = "#FF8181";
                        break;

                    case "Orange":
                        backgroundColor = "#FFB553";
                        break;

                    case "Yellow":
                        backgroundColor = "#FFE979";
                        break;

                    case "Green":
                        backgroundColor = "#79F468";
                        break;

                    case "Blue":
                        backgroundColor = "#57ABFF";
                        break;

                    case "White":
                        backgroundColor = "#FFFFFF";
                        break;

                    default:
                        backgroundColor = "#FFFFFF";
                        break;
                }

                GoalsGroups.Add(new GoalsGroup
                {
                    Filename = filename,
                    Name = splitText[0],
                    ImageFilePath = splitText[2],
                    Color = backgroundColor
                });
            }

            //Updates list view with generated list
            ListViewGoalsGroup.ItemsSource = GoalsGroups;
                 

            HideList();
        }

       
        void DeleteGoalsGroup_Clicked(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);//initialises variable as a MenuItem
            var item = ((GoalsGroup)mi.CommandParameter);//sets item as the group item that was selected in the list view

            //Gets all the files and deletes the file that matches the Group's Filename property
            var files = Directory.EnumerateFiles(App.FolderPath, "*.goalsgroup.txt");
            foreach (var file in files)
                if (item.Filename == file)
                    File.Delete(file);

            //Removes the group from the list
            GoalsGroups.Remove(item);

            //Updates the list view with the new list
            ListViewGoalsGroup.ItemsSource = GoalsGroups;

            HideList();
        }

        private async void ListViewGoalsGroup_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                await Navigation.PushAsync(new DetailedGoalsPage());
            }
        }
        public void HideList()
        {
            if (GoalsGroups == null || GoalsGroups.Count() == 0)
            {
                ListViewGoalsGroup.IsVisible = false;
                
            }
            else
            {
                ListViewGoalsGroup.IsVisible = true;
                
            }
        }

        private async void AddButton_Clicked(object sender, EventArgs e)
        {
            //opens new AddGoalsGroupPage, sets BindingContext to GoalsGroup object
            await Navigation.PushModalAsync(new PopupPages.AddGoalsGroupPage {
            BindingContext = new GoalsGroup()
            });
        }
    }
}