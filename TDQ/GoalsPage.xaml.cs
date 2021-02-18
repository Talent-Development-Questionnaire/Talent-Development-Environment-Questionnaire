using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TDQ
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GoalsPage : ContentPage
    {
        public GoalsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            Classes.SettingsPageFunctions.SetBackground(ImgBg, GoalsContentPage);
        }
    }
}