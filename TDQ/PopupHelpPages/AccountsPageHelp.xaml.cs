﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TDQ.PopupHelpPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountsPageHelp : ContentPage
    {
        public AccountsPageHelp()
        {
            InitializeComponent();
        }

        private void BtnOkay_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}