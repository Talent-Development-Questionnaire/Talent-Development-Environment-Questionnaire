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
            BindingContext = this;
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

        private void ImgBtnAddEmail_Clicked(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(entryEmail.Text))
            {
                emails.Add(entryEmail.Text);
            }

            emailList.ItemsSource = emails.ToArray();
        }

        private async void BtnSave_Clicked(object sender, EventArgs e)
        { 
            var group = (Group)BindingContext;
            if(group == null)
            {
                await DisplayAlert("Debug", "Group is still null", "OK");
                return;
            }

            if(!string.IsNullOrWhiteSpace(group.Name))
            {
                if(string.IsNullOrWhiteSpace(group.Filename))
                {
                    if(emails.Count() != 0)
                    {
                        //Save
                        group.ImageFilePath = Utils.SavedSettings.GroupImageSetting;
                        group.EmailList = emails.ToArray();
                        group.GroupNo = group.EmailList.Count();
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
    }
}