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
    public partial class Navigation_Drawer : Shell
    {
        public Navigation_Drawer()
        {
            InitializeComponent();
            Classes.SettingsPageFunctions.ChangeTheme(Utils.SavedSettings.ThemeSettings);
        }
    }
}