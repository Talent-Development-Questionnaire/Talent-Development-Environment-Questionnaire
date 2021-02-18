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
    public partial class AddQuestionnairePage : ContentPage
    {
        ObservableCollection<Group> Groups;
        List<string> groupList = new List<string>();
        string[] emailList;

        public AddQuestionnairePage()
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

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (PickerQuestionnaire.SelectedItem)
            {
                case "58 Question":
                    /*
                        * Change this to open the large questionnaire
                    */
                    break;
                case "29 Question":
                    /*
                        * Change this to open the small questionnaire
                    */
                    break;
            }
        }

        void BtnSendQuestionnaire_Clicked(System.Object sender, System.EventArgs e)
        {
            //Add functionality later, just use to escape page on iphone for now.
            Navigation.PopModalAsync();
        }

        void PickerGroup_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            if (PickerGroup.SelectedItem != null)
                foreach (var item in Groups)
                    if (item.Name == PickerGroup.SelectedItem.ToString())
                    {
                        LstViewEmails.ItemsSource = item.EmailList;
                    }
        }
    }
}