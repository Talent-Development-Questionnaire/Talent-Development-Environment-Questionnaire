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
            string goalsText = "\n" + EntryGoal.Text;

            //File.WriteAllText(xyz.Filename, xyz.Name + "\n" + xyz.ImageFilePath + "\n" + xyz.Color + "\n" + goalsText);
            File.AppendAllText(xyz.Filename, goalsText);


            await Navigation.PopModalAsync();
        }
    }
}