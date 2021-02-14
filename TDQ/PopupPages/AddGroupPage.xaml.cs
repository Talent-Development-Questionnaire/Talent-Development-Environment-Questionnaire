using System;
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
        }

        public AddGroupPage(Group currentGroup)
        {
            InitializeComponent();

            ImgBtnAddGroupPhoto.Source = currentGroup.ImageFilePath;
            Utils.SavedSettings.GroupImageSetting = null;
            CheckList();
        }

        private async void ImgBtnAddGroupPhoto_Clicked(object sender, EventArgs e)
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Pick a photo"
            });

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
            CheckList();

            if (!string.IsNullOrEmpty(entryEmail.Text))
                if (Classes.Verification.IsValidEmail(entryEmail.Text))
                    emails.Add(entryEmail.Text);
                else
                {
                    await DisplayAlert("Error", "Email is not valid", "OK");
                    entryEmail.Text = string.Empty;
                }

            emailList.ItemsSource = emails.ToArray();
        }

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            CheckList();
            var group = (Group)BindingContext;

            if(!string.IsNullOrWhiteSpace(group.Name))
            {
                if(group.ImageFilePath == null || Utils.SavedSettings.GroupImageSetting != group.ImageFilePath)
                    group.ImageFilePath = Utils.SavedSettings.GroupImageSetting;

                group.EmailList = emails.ToArray();
                group.GroupNo = group.EmailList.Count();

                if (string.IsNullOrWhiteSpace(group.Filename))
                {
                    if(emails.Count() != 0)
                    {
                        //Save
                        var filename = Path.Combine(App.FolderPath, $"{Path.GetRandomFileName()}.group.txt");

                        File.WriteAllText(filename, group.Name + "\n" + group.GroupNo.ToString() + "\n" + group.ImageFilePath + "\n");
                        File.AppendAllLines(filename, group.EmailList);
                    }
                }
                else
                {
                    //Update
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

            bool deleteEmail = await DisplayAlert("Delete email", "Are you sure you want to delete this email?", "Yes", "No");
            if (deleteEmail == true)
            {
                var debug = e.SelectedItem.ToString() + " " +  e.SelectedItemIndex.ToString();
                if (emails.Contains(e.SelectedItem.ToString()))
                {
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
                    if (!emails.Contains(item.ToString()))
                        emails.Add(item.ToString());

        }
    }
}