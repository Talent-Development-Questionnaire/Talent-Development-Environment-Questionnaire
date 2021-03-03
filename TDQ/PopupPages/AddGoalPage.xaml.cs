using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TDQ.Models;

namespace TDQ.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddGoalPage : ContentPage
    {
        public string goals;

        GoalsGroup xyz;
        public AddGoalPage(GoalsGroup currentGoalsGroup)
        {
            xyz = currentGoalsGroup;
            InitializeComponent();
        }
                    

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {       
            string goalsText = EntryGoal.Text;
              
            /*System.Diagnostics.Debug.WriteLine(xyz.Filename);
            System.Diagnostics.Debug.WriteLine(xyz.Name);
            System.Diagnostics.Debug.WriteLine(xyz.ImageFilePath);
            System.Diagnostics.Debug.WriteLine(xyz.Color); */

            xyz.GoalsList = "iaikjhf";

            File.WriteAllText(xyz.Filename, xyz.Name + "\n" + xyz.ImageFilePath + "\n" + xyz.Color + "\n" + goalsText);

            await Navigation.PopModalAsync();
        }
    }
}