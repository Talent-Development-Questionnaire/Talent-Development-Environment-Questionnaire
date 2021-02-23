using System;
using System.Configuration;
using MySqlConnector;
namespace TDQ.Classes
{
    public class DatabaseController
    {
        //string assigned the database file address path update datasource with server address? 
        static readonly string MyConnection = "Server=146.176.251.103; port=3306; Database=talent_development_questionnaire; username=root;password=Planetoftheapes12;Connection Timeout=20";
        public DatabaseController()
        {
        }
        //Inserts new entry into User table in MySQL Database
        public static bool InsertNewUser (string email, string password, string name)
        {
            try
            {   
                string Query = "INSERT INTO coach-users(email,password,name) values('" + email + "','" + password + "','" + name + "');";//Creates query from user input 

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
            string Query = "SELECT * FROM coach-users WHERE email = '" + email + "';";

            MySqlConnection con = new MySqlConnection(MyConnection);
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
