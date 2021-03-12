using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TDQ.Classes
{
    public class AccountPageFunctions
    {
        public static void EditAccountDetails(Models.CoachUser user)
        {
            DatabaseController.EditAccountDetails(user.ID, user.Email, user.Name, user.Gender, user.Dob);
        }
    }
}
