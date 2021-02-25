using System;
using System.Configuration;
using MySqlConnector;
using TDQ.Classes;
namespace TDQ.Classes
{
    public class DatabaseController
    {
        //Inserts new entry into User table in MySQL Database
        public static bool InsertNewUser (string email, string password, string name)
        {
            try
            { 
                MySqlConnection MyConn = new MySqlConnection(Constants.connectionString); //Creates MySQL object                
                MySqlCommand MyCommand = new MySqlCommand(Constants.insertNewUser, MyConn);//This is command class which will handle the query and connection object.
                MySqlDataReader MyReader;

                MyCommand.Parameters.Add("@name", MySqlDbType.VarChar).Value = name;
                MyCommand.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;
                MyConn.Open();

                MyReader = MyCommand.ExecuteReader();// Here our query will be executed and data saved into the database.  
                MyConn.Close(); //Closes connection to database
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool EmailCheck(string email)
        {
            string Query = "SELECT * FROM users WHERE email = '" + email + "';";

            MySqlConnection con = new MySqlConnection(Constants.connectionString);
            MySqlCommand cmd = new MySqlCommand(Query, con);
            MySqlDataReader dr;

            try
            {
                con.Open();
            }
            catch (Exception ex)
            {
                var d = ex.Message;
            }

            dr = cmd.ExecuteReader();
            while (dr.Read())
                if (dr.HasRows == true)
                    return false;

            return true;
        }

        public static bool AccountCheck(string email, string password)
        {
            string MyConnection = "datasource=localhost; Initial Catalog=tdq; username=root;password=Planetoftheapes12";
            string Query = "SELECT * FROM users WHERE email ='" + email + "' AND password = '" + password + "'";

            MySqlConnection con = new MySqlConnection(MyConnection);
            MySqlCommand cmd = new MySqlCommand(Query, con);
            MySqlDataReader dr;

            con.Open();

            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (dr.HasRows == false)
                    return false;
                else
                    return true;
            }


            return false;
        }
    }
}
