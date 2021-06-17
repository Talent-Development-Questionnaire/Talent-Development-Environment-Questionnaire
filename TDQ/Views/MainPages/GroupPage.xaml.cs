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
            LstViewGroup.SelectedItem = null;

            //Gets all the saved files for created groups
            var files = Directory.EnumerateFiles(App.FolderPath, "*.group.txt");
            //iterates through each file and adds the Group to the list view
            foreach (var filename in files)
            {
                string text = (File.ReadAllText(filename));
                string[] splitText = text.Split('\n');
                string[] emailList = Classes.GlobalFunctions.PopulateListOnAppearing(splitText);//Method to populate array with emails from the file

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
            HideList();
        }

        private async void ImgBtnAddGroup_Clicked(object sender, EventArgs e)
        {
            //opens new AddGroupPage, sets the binding context to Group object
            ImgBtnAddGroup.IsEnabled = false;
            await Navigation.PushModalAsync(new PopupPages.AddGroupPage { 
            BindingContext = new Group()
            });
            ImgBtnAddGroup.IsEnabled = true;

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

        private async void DeleteGroup_Clicked(object sender, EventArgs e)
        {
            bool confirmDelete = await DisplayAlert("Delete Group", "Are you sure you want to permanently delete this group?", "Delete", "Cancel");
            if (confirmDelete)
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

                HideList();
            }            
        }

        public void HideList()
        {
            if (Groups == null || Groups.Count() == 0)
            {
                LstViewGroup.IsVisible = false;
                LblNewGroup.IsVisible = true;
            }
            else
            {
                LstViewGroup.IsVisible = true;
                LblNewGroup.IsVisible = false;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.GoToAsync("//home");

            });
            return true;
        }

        // If the help button is pressed then launch the help popup
        private async void BtnGroupPageHelp_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PopupHelpPages.GroupPageHelp());

        }
    }
}