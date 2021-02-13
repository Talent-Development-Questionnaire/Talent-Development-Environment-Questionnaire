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
        private ObservableCollection<Group> groups = new ObservableCollection<Group>();

        public GroupPage()
        {
            InitializeComponent();

            Classes.SettingsPageFunctions.SetBackground(GroupContentPage);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Classes.SettingsPageFunctions.SetBackground(GroupContentPage);

            var files = Directory.EnumerateFiles(App.FolderPath, "*.group.txt");
            foreach (var filename in files)
            {
                string text = (File.ReadAllText(filename));
                string[] splitText = text.Split('\n');
                string[] emailList = PopulateList(splitText);

                    groups.Add(new Group
                {
                    Filename = filename,
                    Name = splitText[0],
                    GroupNo = Convert.ToInt32(splitText[1]),
                    ImageFilePath = splitText[2],
                    EmailList = emailList
                });
            }

            LstViewGroup.ItemsSource = groups;
        }

        string[] PopulateList(string[] list)
        {
            List<string> emails = new List<string>();
            for (int i = 3; i < list.Length; i++)
            {
                if(list[i] != "")
                    emails.Add(list[i]);
            }

            return emails.ToArray();
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
                await Navigation.PushModalAsync(new PopupPages.AddGroupPage
                {
                    BindingContext = e.SelectedItem as Group
                });
            }
        }

        void DeleteGroup_Clicked(System.Object sender, System.EventArgs e)
        {
            
        }
    }
}