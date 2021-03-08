using System;
using System.Configuration;
using MySqlConnector;
using Renci.SshNet;
using System.Net;
using System.Security.Cryptography;
using System.IO;
using TDQ.Classes;
using System.Text;

namespace TDQ.Classes
{
    public class DatabaseController
    {
        private static string HashPassword(string pass)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(pass));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        //Method to check if inputted email already exists within the database
        public static bool CheckEmailExists(string email)
        {
            string url = $"{Constants.ip}coach/checkEmail/{email}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            return response.SupportsHeaders;
        }

        //Method to insert new user data into the database 
        public static bool InsertNewUser(string email, string password, string name)
        {
            password = HashPassword(password);
            string url = $"{Constants.ip}coach/addCoach/{email}/{password}/{name}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            return response.SupportsHeaders;
        }

        //Method to check if account details exist in database
        public static bool AccountCheck(string email, string password)
        {
            password = HashPassword(password);
            string url = $"{Constants.ip}coach/checkAccountExists/{email}/{password}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            return response.SupportsHeaders;
        }

        public static void GetUserDetails()
        {
            //get user details associated with login
            //initialise user object with details
            //return user
        }

        public static void EditAccountDetails()
        {
            //Pass in user object
            //Check to see if gender and d.o.b fields are null
            //input into database values if not null

        }
    }
}
