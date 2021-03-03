using System;
using System.Configuration;
using MySqlConnector;
using Renci.SshNet;
using System.Net;
using System.IO;
using TDQ.Classes;
namespace TDQ.Classes
{
    public class DatabaseController
    {
        //Method to check if inputted email already exists within the database
        public static bool CheckEmailExists(string email)
        {
            //Create SSH tunnel to server
            using (var client = new SshClient("146.176.251.103", "40402601", "leois12yrs"))
            {
                //Connect to server
                client.Connect();
                if (client.IsConnected)
                {
                    //Create ports to MYSQL database
                    var portForwarded = new ForwardedPortLocal("127.0.0.1", 3306, "127.0.0.1", 3306);
                    client.AddForwardedPort(portForwarded);
                    portForwarded.Start();

                    if(portForwarded.IsStarted)
                    {
                        //Initialising connection to database and MySQL query
                        MySqlConnection con = new MySqlConnection(Constants.connectionString);
                        MySqlCommand command = new MySqlCommand(Constants.checkEmailExists, con);
                        MySqlDataReader reader;

                        //Add user email to query
                        command.Parameters.AddWithValue("@email", email);
                        //open connection to MySQL database
                        con.Open();
                        //Execute MySQL query
                        reader = command.ExecuteReader();
                        while (reader.Read())
                            //Check if database already contains email return false if it does
                            if (reader.HasRows == true)
                            {
                                //Close connection to database and server
                                con.Close();
                                client.Dispose();
                                return false;
                            }

                        con.Close();
                        client.Dispose();
                        //return true if email does not exist in database
                        return true;
                    }
                }
            }

            return false;
        }

        //Method to insert new user data into the database 
        public static bool InsertNewUser(string email, string password, string name)
        {
            using (var client = new SshClient("146.176.251.103", "40402601", "leois12yrs"))
            {
                client.Connect();
                if (client.IsConnected)
                {
                    var portForwarded = new ForwardedPortLocal("127.0.0.1", 3306, "127.0.0.1", 3306);
                    client.AddForwardedPort(portForwarded);
                    portForwarded.Start();

                    if(portForwarded.IsStarted)
                    {

                        MySqlConnection con = new MySqlConnection(Constants.connectionString);
                        MySqlCommand command = new MySqlCommand(Constants.insertNewUser, con);
                        MySqlDataReader reader;

                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@password", password);
                        con.Open();

                        reader = command.ExecuteReader();

                        con.Close();
                        client.Dispose();

                        return true;
                    }
                }
            }
            return false;
        }

        //Method to check if account details exist in database
        public static bool AccountCheck(string email, string password)
        {
            using (var client = new SshClient("146.176.251.103", "40402601", "leois12yrs"))
            {
                client.Connect();
                if (client.IsConnected)
                {
                    var portForwarded = new ForwardedPortLocal("127.0.0.1", 3306, "127.0.0.1", 3306);
                    client.AddForwardedPort(portForwarded);
                    portForwarded.Start();

                    if (portForwarded.IsStarted)
                    {
                        MySqlConnection con = new MySqlConnection(Constants.connectionString);
                        MySqlCommand command = new MySqlCommand(Constants.checkAccountExists, con);
                        MySqlDataReader reader;

                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@password", password);
                        con.Open();

                        reader = command.ExecuteReader();
                        if (reader.HasRows == true)
                        {
                            //Close connection to database and server
                            con.Close();
                            client.Dispose();
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static void TestHttpRequest()
        {
            string url = "http://79.66.24.174:5000/oldwez";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream resStream = response.GetResponseStream();

        }
    }
}
