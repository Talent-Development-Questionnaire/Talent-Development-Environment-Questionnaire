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

        public static string GenerateOTP()
        {
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";

            string characters = numbers;

            characters += alphabets + small_alphabets + numbers;
            int length = 12;
            string otp = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }
            return otp;
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
        public static bool InsertNewUser(string email, string password, string name, string otp)
        {
            password = HashPassword(password);
            string url = $"{Constants.ip}coach/addCoach/{email}/{password}/{name}/{otp}";
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

            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                string responseString = reader.ReadToEnd();
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

        public static string[] GenerateQuestions(int questionType)
        {
            string url = $"{Constants.ip}player/getQuestions/{questionType}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                string responseString = reader.ReadToEnd();
                string[] questions = responseString.Split(new string[] { "\n" }, StringSplitOptions.None);

                return questions;
            }
        }

        public static bool VerifyCoachAccount(string email, string otp)
        {
            string url = $"{Constants.ip}/coach/verifyAccount/{email}/{otp}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                string responseString = reader.ReadToEnd();

                return Convert.ToBoolean(responseString);
            }
        }

        public static Models.Questionnaire AssignAthletesQuestionnaires(string name, string type, string email, string athlete, string otp, int flag)
        {
            string url = $"{Constants.ip}/coach/createQuestionnaire/{name}/{type}/{email}/{athlete}/{otp}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if(flag == 1)
            {
                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream);
                    string responseString = reader.ReadToEnd();
                    responseString = responseString.Replace("(", string.Empty);
                    responseString = responseString.Replace(")", string.Empty);
                    responseString = responseString.TrimEnd(',');
                    var questionnaire = JsonConvert.DeserializeObject<Models.Questionnaire>(responseString);

                    if (questionnaire != null)
                        return questionnaire;
                    else
                        return null;
                }
            }

            return null;
        }

        public static bool DeleteUser(string email, string password)
        {
            password = HashPassword(password);

            string url = $"{Constants.ip}/coach/deleteCoach/{email}/{password}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                string responseString = reader.ReadToEnd();
                return Convert.ToBoolean(responseString);
            }
        }
    }
}