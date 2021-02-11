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
            await DisplayAlert("Placeholder", "Icon should be changed to photo from camera or gallery", "OK");
        }

        private async void ImgBtnAddEmail_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Placeholder", "Email should be added to list", "OK");
        }

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Placeholder", "Pop-up should disappear and data should be saved", "OK");
        }
    }
}