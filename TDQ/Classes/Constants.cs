using System;
using System.Collections.Generic;
using System.Text;

namespace TDQ.Classes
{
    public static class Constants
    {
        #region MYSQL CONNECTION STRING

        public static string connectionString = "Server=127.0.0.1; Port=3306; Database=talent_development_questionnaire; Connect Timeout=120; User=root; Password=Planetoftheapes12;";

        #endregion

        #region MYSQL INSERT NEW USER

        public static string insertNewUser = "INSERT INTO coachUsers(name, email, password) values(@name, @email, @password)";

        #endregion
    }
}
