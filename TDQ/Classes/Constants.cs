using System;
using System.Collections.Generic;
using System.Text;

namespace TDQ.Classes
{
    public static class Constants
    {
        #region MYSQL CONNECTION STRING

        public static string connectionString = "Server=127.0.0.1; Port=3306; Database=user; Connect Timeout=120; User=root; Password=Planetoftheapes12;";

        #endregion

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
