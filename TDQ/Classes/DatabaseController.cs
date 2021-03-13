using System;
using System.Net;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using Newtonsoft.Json;

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

            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                string responseString = reader.ReadToEnd();
                return Convert.ToBoolean(responseString);
            }
        }

        //Method to insert new user data into the database 
        public static bool InsertNewUser(string email, string password, string name)
        {
            password = HashPassword(password);
            string url = $"{Constants.ip}coach/addCoach/{email}/{password}/{name}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                string responseString = reader.ReadToEnd();
                return Convert.ToBoolean(responseString);
            }
        }

        //Method to check if account details exist in database
        public static bool AccountCheck(string email, string password)
        {
            password = HashPassword(password);
            string url = $"{Constants.ip}coach/checkAccountExists/{email}/{password}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                string responseString = reader.ReadToEnd();
                return Convert.ToBoolean(responseString);
            }
        }

        public static Models.CoachUser GetUserDetails(string email)
        {
            string url = $"{Constants.ip}coach/getUser/{email}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            string responseString = null;
            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                responseString = reader.ReadToEnd();
                responseString = responseString.Replace("(", string.Empty);
                responseString = responseString.Replace(")", string.Empty);
                responseString = responseString.TrimEnd(',');
                var user = JsonConvert.DeserializeObject<Models.CoachUser>(responseString);

                if (user != null)
                    return user;
                else
                    return null;
            }
        }

        public static void EditAccountDetails(string id, string email, string name, string gender, string dob)
        {
            string url = $"{Constants.ip}coach/editUser/{id}/{email}/{name}/{gender}/{dob}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        }
    }
}