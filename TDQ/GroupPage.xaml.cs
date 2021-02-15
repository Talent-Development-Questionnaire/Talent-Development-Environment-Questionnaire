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
    public partial class GroupPage : ContentPage
    {
        public ObservableCollection<Group> Groups;

        public GroupPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //Initialising variables
            Groups = new ObservableCollection<Group>();
            Classes.SettingsPageFunctions.SetBackground(ImgBg, GroupContentPage);
            LstViewGroup.SelectedItem = null;

            //Gets all the saved files for created groups
            var files = Directory.EnumerateFiles(App.FolderPath, "*.group.txt");
            //iterates through each file and adds the Group to the list view
            foreach (var filename in files)
            {
                string text = (File.ReadAllText(filename));
                string[] splitText = text.Split('\n');
                string[] emailList = PopulateListOnAppearing(splitText);//Method to populate array with emails from the file

                //Adds Group object to list, sets each property of the object
                    Groups.Add(new Group
                {
                    Filename = filename,
                    Name = splitText[0],
                    GroupNo = Convert.ToInt32(splitText[1]),
                    ImageFilePath = splitText[2],
                    EmailList = emailList
                });
            }

            //Updates list view with generated list
            LstViewGroup.ItemsSource = Groups;
        }

 
        string[] PopulateListOnAppearing(string[] list)
        {
            List<string> emails = new List<string>();
            //starts index at 3, this is where the emails start in the array
            for (int i = 3; i < list.Length; i++)
            {
                //Adds email to new list as long as the element is not empty.
                if(list[i] != "")
                    emails.Add(list[i]);
            }

            //returns the new array of just emails
            return emails.ToArray();
        }

        private async void ImgBtnAddGroup_Clicked(object sender, EventArgs e)
        {
            //opens new AddGroupPage, sets the binding context to Group object
            await Navigation.PushModalAsync(new PopupPages.AddGroupPage { 
            BindingContext = new Group()
            });
        }

        private async void LstViewGroup_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //Opens selected item in AddGroupPage and populates it with the set vales of the object
            if (e.SelectedItem != null)
            {


                await Navigation.PushModalAsync(new PopupPages.AddGroupPage(e.SelectedItem as Group)
                {
                    BindingContext = e.SelectedItem as Group
                });
            }
        }

        void DeleteGroup_Clicked(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);//initialises variable as a MenuItem
            var item = ((Group)mi.CommandParameter);//sets item as the group item that was selected in the list view

            //Gets all the files and deletes the file that matches the Group's Filename property
            var files = Directory.EnumerateFiles(App.FolderPath, "*.group.txt");
            foreach (var file in files)
                if (item.Filename == file)
                    File.Delete(file);

            //Removes the group from the list
            Groups.Remove(item);

            //Updates the list view with the new list
            LstViewGroup.ItemsSource = Groups;
        }
    }
}