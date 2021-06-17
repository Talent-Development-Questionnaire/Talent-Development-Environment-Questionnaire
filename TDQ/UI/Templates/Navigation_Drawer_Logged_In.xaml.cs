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
    public partial class Navigation_Drawer_Logged_In : Shell
    {
        public Navigation_Drawer_Logged_In()
        {
            InitializeComponent();
            Classes.GlobalFunctions.ChangeTheme(Utils.SavedSettings.ThemeSettings);
        }
    }
}