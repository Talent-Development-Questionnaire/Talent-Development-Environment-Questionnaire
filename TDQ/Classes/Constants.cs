using System;
using System.Collections.Generic;
using System.Text;

namespace TDQ.Classes
{
    public static class Constants
    {
        #region MYSQL CONNECTION STRING

        public static string connectionString = "Server=localhost; Port=3306; Database=test; Connect Timeout=120; User=root; Password=mollyis12yrs; ";

        #endregion

        #region MYSQL INSERT NEW USER

        public static string insertNewUser = "INSERT INTO users(name, email) values(@name, @email)";

        #endregion
    }
}
