using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TDQ.Models;

namespace TDQ.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddGoalsGroupPage : ContentPage
    {
        ObservableCollection<Group> Groups;
        List<string> groupList = new List<string>();
        string[] emailList;

        public AddGoalsGroupPage()
        {
            InitializeComponent();
        }

        
        protected override void OnAppearing()
        {
            base.OnAppearing();

            //Initialising variables
            Groups = new ObservableCollection<Group>();

            //Gets all the saved files for created groups
            var files = Directory.EnumerateFiles(App.FolderPath, "*.group.txt");
            //iterates through each file and adds the Group to the list view
            foreach (var filename in files)
            {
                string text = (File.ReadAllText(filename));
                string[] splitText = text.Split('\n');
                emailList = Classes.GroupPageFunctions.PopulateListOnAppearing(splitText);//Method to populate array with emails from the file

                //Adds Group object to list, sets each property of the object
                Groups.Add(new Group
                {
                    Filename = filename,
                    Name = splitText[0],
                    GroupNo = Convert.ToInt32(splitText[1]),
                    ImageFilePath = splitText[2],
                    EmailList = emailList
                });
                groupList.Add(splitText[0]);

                PickerGroup.ItemsSource = groupList.ToArray();
            }

        }

        private async void BtnSaveGoalsGroup_Clicked(object sender, EventArgs e)
        {
            //Initialises GoalsGroup object
            GoalsGroup goalsGroup = (GoalsGroup)BindingContext;

            //Using the group name from picker, assigns goalsGroup.Group to the correct group
            //Should probably be changed in the future but functions for now
            foreach (Group group in Groups)
            {
                if (group.Name.Equals((string)PickerGroup.SelectedItem))
                {
                    goalsGroup.Group = group;
                    break;
                } 
            }
            
            //Sets GoalsGroup object properties
            goalsGroup.Name = goalsGroup.Group.Name;
            goalsGroup.ImageFilePath = goalsGroup.Group.ImageFilePath;
            goalsGroup.Color = (String)PickerColor.SelectedItem;

            //Creates new file and writes the Group properties' values to said file
            var filename = Path.Combine(App.FolderPath, $"{Path.GetRandomFileName()}.goalsgroup.txt");

            File.WriteAllText(filename, goalsGroup.Name + "\n" + goalsGroup.Group.ToString() + "\n" + goalsGroup.ImageFilePath + "\n" + goalsGroup.Color);

            await Navigation.PopModalAsync();
        }
    }
}