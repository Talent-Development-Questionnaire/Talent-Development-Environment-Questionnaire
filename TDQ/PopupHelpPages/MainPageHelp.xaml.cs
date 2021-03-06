﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TDQ.Models;

namespace TDQ.PopupHelpPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageHelp : ContentPage
    {
        public MainPageHelp()
        {
            InitializeComponent();
        }

        // If the 'Okay' button is pressed go back to the previous page
        private void BtnOkay_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();

        }
    }
}