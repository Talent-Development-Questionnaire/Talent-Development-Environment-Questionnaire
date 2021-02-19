﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDQ.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TDQ.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddGroupPage : ContentPage
    {
        public readonly List<string> emails = new List<string>();

        public AddGroupPage()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == "iOS")
                BtnCanel.IsVisible = true;
            else
                BtnCanel.IsVisible = false;
        }

        public AddGroupPage(Group currentGroup)
        {
            InitializeComponent();

            if (Device.RuntimePlatform == "iOS")
                BtnCanel.IsVisible = true;
            else
                BtnCanel.IsVisible = false;

            //sets the image to the selected Groups image
            ImgBtnAddGroupPhoto.Source = currentGroup.ImageFilePath;
            //Resets save image filepath setting to null
            Utils.SavedSettings.GroupImageSetting = null;
            CheckList();
        }

        private async void ImgBtnAddGroupPhoto_Clicked(object sender, EventArgs e)
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Pick a photo"
            });

            if (result == null)
                return;

            ImgBtnAddGroupPhoto.Source = ImageSource.FromFile(result.FullPath);

            try
            {
                Utils.SavedSettings.GroupImageSetting = result.FullPath;
            }
            catch (Exception ex)
            {

                await App.Current.MainPage.DisplayAlert("Debug", ex.Message, "OK");
            }
        }

        private async void ImgBtnAddEmail_Clicked(object sender, EventArgs e)
        {
            //Method to avoid items in list view duplicating
            CheckList();

            //Checks that the entry field is not empty and then verifies text is a valid email
            if (!string.IsNullOrEmpty(entryEmail.Text))
                if (Classes.Verification.IsValidEmail(entryEmail.Text))
                    emails.Add(entryEmail.Text);
                else
                {
                    await DisplayAlert("Error", "Email is not valid", "OK");
                    entryEmail.Text = string.Empty;
                }

            //Updates list view with newly added email
            emailList.ItemsSource = emails.ToArray();
        }

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            CheckList();
            //Initialises Group object
            var group = (Group)BindingContext;

            //Checks if group name has been set, displays error if it hasn't
            if(!string.IsNullOrWhiteSpace(group.Name))
            {
                //Checks if the image as been changed or not
                if((Utils.SavedSettings.GroupImageSetting != group.ImageFilePath && !string.IsNullOrEmpty(Utils.SavedSettings.GroupImageSetting)) || string.IsNullOrEmpty(group.ImageFilePath))
                    group.ImageFilePath = Utils.SavedSettings.GroupImageSetting;

                //Sets group object properties
                group.EmailList = emails.ToArray();
                group.GroupNo = group.EmailList.Count();

                //Checks if the group is a new one or is just being edited
                if (string.IsNullOrWhiteSpace(group.Filename))
                {
                    //Checks if emails have been added to the group
                    if(emails.Count() != 0)
                    {
                        //Save
                        //Creates new file and writes the Group properties' values to said file
                        var filename = Path.Combine(App.FolderPath, $"{Path.GetRandomFileName()}.group.txt");

                        File.WriteAllText(filename, group.Name + "\n" + group.GroupNo.ToString() + "\n" + group.ImageFilePath + "\n");
                        File.AppendAllLines(filename, group.EmailList);
                    }
                    else
                    {
                        await DisplayAlert("Error", "Please add emails to the group, cannot be left empty", "OK");
                    }
                }
                else
                {
                    //Update
                    //Overwrites the current file with Group values
                    File.WriteAllText(group.Filename, group.Name + "\n" + group.GroupNo.ToString() + "\n" + group.ImageFilePath + "\n");
                    File.AppendAllLines(group.Filename, group.EmailList);
                }

                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Error", "Cannot save empty entry!\n Press back button at top of screen to exit page", "OK");
            }
        }

        private async void emailList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            CheckList();

            //Checks to make sure if the user wants to delete email from list view
            bool deleteEmail = await DisplayAlert("Delete email", "Are you sure you want to delete this email?", "Yes", "No");
            //if true then check if email list contains the selected email
            if (deleteEmail == true)
            {
                if (emails.Contains(e.SelectedItem.ToString()))
                {
                    //remove the selected email from the email list and update the list view with updated email list
                    emails.Remove(e.SelectedItem.ToString());
                    emailList.ItemsSource = emails.ToArray();
                }
            }
        }

        void CheckList()
        {
            //Check if listview is empty
            if(emailList.ItemsSource != null)
                foreach(var item in emailList.ItemsSource)
                    //Adds only new emails to list to stop duplicating
                    if (!emails.Contains(item.ToString()))
                        emails.Add(item.ToString());

        }

        void BtnCanel_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}