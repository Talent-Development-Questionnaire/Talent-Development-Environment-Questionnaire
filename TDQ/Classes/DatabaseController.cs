using System;
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
    }
}
