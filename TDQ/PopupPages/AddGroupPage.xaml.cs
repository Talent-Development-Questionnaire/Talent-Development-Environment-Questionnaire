using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDQ.Models;
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
            await DisplayAlert("Placeholder", "Icon should be changed to photo from camera or gallery", "OK");
            
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
                        group.EmailList = emails.ToArray();
                        group.GroupNo = group.EmailList.Count();
                        var filename = Path.Combine(App.FolderPath, $"{Path.GetRandomFileName()}.group.txt");
                        File.WriteAllText(filename, group.Name + "\n" + group.GroupNo.ToString() + "\n");
                        File.AppendAllLines(filename, group.EmailList);
                    }
                }
                else
                {
                    //Update
                    File.WriteAllText(group.Filename, group.Name + "/n" + group.EmailList + "/n" + group.GroupNo.ToString());
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