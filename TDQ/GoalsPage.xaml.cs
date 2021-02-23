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
            LstViewGroup.SelectedItem = null;

            //Gets all the saved files for created goalsgroups
            var files = Directory.EnumerateFiles(App.FolderPath, "*.goalsgroup.txt");
            //iterates through each file and adds the goalsgroup to the list view
            foreach (var filename in files)
            {
                string text = (File.ReadAllText(filename));
                string[] splitText = text.Split('\n');

                //Adds GoalsGroup object to list, sets each property of the object

                string backcolor;

                switch (splitText[3])
                {
                    case "Yellow":
                        backcolor = "#B1FFFF00";
                        break;

                    case "Red":
                        backcolor = "#B1FF0000";
                        break;

                    case "Green":
                        backcolor = "#B100FF00";
                        break;

                    default:
                        backcolor = "#00000000";
                        break;

                }

                GoalsGroups.Add(new GoalsGroup
                {
                    Filename = filename,
                    Name = splitText[0],
                    ImageFilePath = splitText[2],
                    Color = backcolor
                });
            }

            //Updates list view with generated list
            LstViewGroup.ItemsSource = GoalsGroups;

            

            HideList();
        }

        public void HideList()
        {
            if (GoalsGroups == null || GoalsGroups.Count() == 0)
            {
                LstViewGroup.IsVisible = false;
                
            }
            else
            {
                LstViewGroup.IsVisible = true;
                
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