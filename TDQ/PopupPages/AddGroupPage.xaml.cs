using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TDQ.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddGroupPage : ContentPage
    {
        public AddGroupPage()
        {
            InitializeComponent();
        }

        private async void ImgBtnAddGroupPhoto_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Placeholder", "Implement adding photo functionality", "OK");
        }
    }
}