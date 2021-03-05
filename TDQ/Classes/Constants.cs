using System;
using System.Collections.Generic;
using System.Text;

namespace TDQ.Classes
{
    public static class Constants
    {
        public static string ip = "http://79.66.24.174:5000/";

        #region MYSQL INSERT NEW USER

        public static string insertNewUser = "INSERT INTO coachUsers(name, email, password) values(@name, @email, @password)";

        #endregion

        #region MYSQL CHECK EMAIL EXISTS

        public static string checkEmailExists = "SELECT * FROM coachUsers WHERE email = @email";

        #endregion

        #region MYSQL CHECK ACCOUNT EXISTS

        public static string checkAccountExists = "SELECT * FROM coachUsers WHERE email = @email AND password = @password";

        #endregion
    }
}
