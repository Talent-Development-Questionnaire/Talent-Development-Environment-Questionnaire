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

            Groups = new ObservableCollection<Group>();
            Classes.SettingsPageFunctions.SetBackground(ImgBg, GroupContentPage);
            LstViewGroup.SelectedItem = null;

            var files = Directory.EnumerateFiles(App.FolderPath, "*.group.txt");
            foreach (var filename in files)
            {
                string text = (File.ReadAllText(filename));
                string[] splitText = text.Split('\n');
                string[] emailList = PopulateListOnAppearing(splitText);

                    Groups.Add(new Group
                {
                    Filename = filename,
                    Name = splitText[0],
                    GroupNo = Convert.ToInt32(splitText[1]),
                    ImageFilePath = splitText[2],
                    EmailList = emailList
                });
            }

            LstViewGroup.ItemsSource = Groups;
        }

        string[] PopulateListOnAppearing(string[] list)
        {
            List<string> emails = new List<string>();
            for (int i = 3; i < list.Length; i++)
            {
                if(list[i] != "")
                    emails.Add(list[i]);
            }

            return emails.ToArray();
        }

        List<Group> PopulateListOnDelete()
        {
            List<Group> emailList = new List<Group>();

            if (LstViewGroup.ItemsSource != null)
                foreach(Group item in LstViewGroup.ItemsSource)
                {
                    emailList.Add(item);
                }

            return emailList;
        }
        private async void ImgBtnAddGroup_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PopupPages.AddGroupPage { 
            BindingContext = new Group()
            });
        }

        private async void LstViewGroup_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
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
            var mi = ((MenuItem)sender);
            Group item = ((Group)mi.CommandParameter);

            var files = Directory.EnumerateFiles(App.FolderPath, "*.group.txt");
            foreach (var file in files)
                if (item.Filename == file)
                    File.Delete(file);

            Groups.Remove(item);

            LstViewGroup.ItemsSource = Groups;
        }
    }
}