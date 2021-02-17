using System;
using System.Configuration;
using MySql.Data.MySqlClient;
namespace TDQ.Classes
{
    public class DatabaseController
    {
        public DatabaseController()
        {
        }

        //Insets new entry into User table in MySQL Database
        public static bool InsertNewUser (string email, string password)
        {
            try
            {
                //string assigned the database file address path update datasource with server address? 
                string MyConnection = "datasource=localhost; Initial Catalog=tdq; username=root;password=Mollyis12yrs!";
                string Query = "INSERT INTO users(email,password) values('" + email + "','" + password + "');";//Creates query from user input 

                MySqlConnection MyConn = new MySqlConnection(MyConnection); //Creates MySQL object                
                MySqlCommand MyCommand = new MySqlCommand(Query, MyConn);//This is command class which will handle the query and connection object.
                MySqlDataReader MyReader;
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
            string MyConnection = "datasource=localhost; Initial Catalog=tdq; username=root;password=Mollyis12yrs!";
            string Query = "SELECT * FROM users WHERE email = '" + email + "';";

            MySqlConnection con = new MySqlConnection(MyConnection);
            MySqlCommand cmd = new MySqlCommand(Query, con);
            MySqlDataReader dr;

            con.Open();

            dr = cmd.ExecuteReader();
            while (dr.Read())
                if (dr.HasRows == true)
                    return false;

            return true;
        }

        public static bool AccountCheck(string email, string password)
        {
            string MyConnection = "datasource=localhost; Initial Catalog=tdq; username=root;password=Mollyis12yrs!";
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
