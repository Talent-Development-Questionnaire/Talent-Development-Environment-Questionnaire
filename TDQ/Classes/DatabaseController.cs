using System;
using System.Configuration;
using MySqlConnector;
using Renci.SshNet;
using TDQ.Classes;
namespace TDQ.Classes
{
    public class DatabaseController
    {
        public static bool CheckEmail(string email)
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
                        string query = "SELECT * FROM coachUsers WHERE email = @email";

                        MySqlConnection con = new MySqlConnection(Constants.connectionString);
                        MySqlCommand command = new MySqlCommand(query, con);
                        MySqlDataReader reader;

                        command.Parameters.AddWithValue("@email", email);
                        con.Open();
   
                        reader = command.ExecuteReader();
                        while (reader.Read())
                            if (reader.HasRows != true)
                            {
                                client.Dispose();
                                con.Close();
                                return true;
                            }
                    }
                }
            }

            return false;
        }

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
    }
}
