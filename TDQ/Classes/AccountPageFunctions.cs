using System;
using System.Collections.Generic;
using System.Text;

namespace TDQ.Classes
{
    public class AccountPageFunctions
    {

        public static Models.CoachUser GetAccountDetails()
        {
            if (!string.IsNullOrEmpty(Utils.SavedSettings.LoginSettings))
            {
                var userDetails = DatabaseController.GetUserDetails(Utils.SavedSettings.LoginSettings);
                var user = new Models.CoachUser
                {
                    Name = userDetails[0],
                    Email = userDetails[3],
                    Gender = userDetails[1],
                    Dob = userDetails[2]
                };
                return user;
            }
            return null;
        }
    }
}
